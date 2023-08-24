using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lumina.Data.Parsing;

namespace Penumbra.GameData.Files;

public partial class MtrlFile
{
    public const string DummyTexturePath    = "common/graphics/texture/dummy.tex";
    public const uint   DefaultSamplerFlags = 0x000F8340u;

    public int FindOrAddConstant(uint id, int numFloats)
    {
        for (var i = 0; i < ShaderPackage.Constants.Length; ++i)
            if (ShaderPackage.Constants[i].Id == id)
                return i;

        var offset = ShaderPackage.ShaderValues.Length;
        if (offset >= 0x4000 || numFloats >= 0x4000)
            throw new InvalidOperationException("Constant capacity exceeded");
        ShaderPackage.ShaderValues = ShaderPackage.ShaderValues.AddItem(0.0f, numFloats);

        var newI = ShaderPackage.Constants.Length;
        ShaderPackage.Constants = ShaderPackage.Constants.AddItem(new Constant
        {
            Id         = id,
            ByteOffset = (ushort)(offset << 2),
            ByteSize   = (ushort)(numFloats << 2),
        });

        return newI;
    }

    public ref Constant GetOrAddConstant(uint id, int numFloats, out int i)
    {
        i = FindOrAddConstant(id, numFloats);
        return ref ShaderPackage.Constants[i];
    }

    public ref Constant GetOrAddConstant(uint id, int numFloats)
        => ref GetOrAddConstant(id, numFloats, out _);

    public int FindOrAddSampler(uint id, string defaultTexture)
    {
        for (var i = 0; i < ShaderPackage.Samplers.Length; ++i)
            if (ShaderPackage.Samplers[i].SamplerId == id)
                return i;

        var newTextureI = Textures.Length;
        if (newTextureI >= 0x100)
            throw new InvalidOperationException("Sampler capacity exceeded");
        Textures = Textures.AddItem(new Texture
        {
            Path  = defaultTexture.Length > 0 ? defaultTexture : DummyTexturePath,
            Flags = 0,
        });

        var newI = ShaderPackage.Samplers.Length;
        ShaderPackage.Samplers = ShaderPackage.Samplers.AddItem(new Sampler
        {
            Flags        = DefaultSamplerFlags,
            SamplerId    = id,
            TextureIndex = (byte)newTextureI,
        });

        return newI;
    }

    public ref Sampler GetOrAddSampler(uint id, string defaultTexture, out int i)
    {
        i = FindOrAddSampler(id, defaultTexture);
        return ref ShaderPackage.Samplers[i];
    }

    public ref Sampler GetOrAddSampler(uint id, string defaultTexture)
        => ref GetOrAddSampler(id, defaultTexture, out _);

    public int FindOrAddColorSet()
    {
        for (var i = 0; i < ColorSets.Length; ++i)
            if (ColorSets[i].HasRows)
                return i;

        if (ColorSets.Length > 0)
            ColorSets[0].HasRows = true;
        else
        {
            ColorSets = new[]
            {
                new ColorSet
                {
                    Name    = "colorSet1",
                    Index   = 0,
                    HasRows = true,
                },
            };
        }

        ref var rows = ref ColorSets[0].Rows;
        for (var i = 0; i < ColorSet.RowArray.NumRows; ++i)
            rows[i] = ColorSet.Row.Default;

        if (AdditionalData.Length < 4)
            AdditionalData = AdditionalData.AddItem((byte)0, 4 - AdditionalData.Length);
        AdditionalData[0] |= 0x04;

        return 0;
    }

    public ref ColorSet GetOrAddColorSet(out int i)
    {
        i = FindOrAddColorSet();
        return ref ColorSets[i];
    }

    public ref ColorSet GetOrAddColorSet()
        => ref GetOrAddColorSet(out _);

    public int FindOrAddColorDyeSet()
    {
        var i = FindOrAddColorSet();
        if (i < ColorDyeSets.Length)
            return i;

        ColorDyeSets = ColorSets.Select(c => new ColorDyeSet
        {
            Index = c.Index,
            Name  = c.Name,
        }).ToArray();

        if (AdditionalData.Length < 4)
            AdditionalData = AdditionalData.AddItem((byte)0, 4 - AdditionalData.Length);
        AdditionalData[0] |= 0x08;

        return i;
    }

    public ref ColorDyeSet GetOrAddColorDyeSet(out int i)
    {
        i = FindOrAddColorDyeSet();
        return ref ColorDyeSets[i];
    }

    public ref ColorDyeSet GetOrAddColorDyeSet()
        => ref GetOrAddColorDyeSet(out _);

    public int FindShaderKey(uint category)
    {
        for (var i = 0; i < ShaderPackage.ShaderKeys.Length; ++i)
            if (ShaderPackage.ShaderKeys[i].Category == category)
                return i;

        return -1;
    }

    public ShaderKey? GetShaderKey(uint category, out int i)
    {
        i = FindShaderKey(category);
        return (i >= 0) ? ShaderPackage.ShaderKeys[i] : null;
    }

    public ShaderKey? GetShaderKey(uint category)
        => GetShaderKey(category, out _);

    public int FindOrAddShaderKey(uint category, uint defaultValue)
    {
        for (var i = 0; i < ShaderPackage.ShaderKeys.Length; ++i)
            if (ShaderPackage.ShaderKeys[i].Category == category)
                return i;

        var newI = ShaderPackage.ShaderKeys.Length;
        ShaderPackage.ShaderKeys = ShaderPackage.ShaderKeys.AddItem(new ShaderKey
        {
            Category = category,
            Value    = defaultValue,
        });

        return newI;
    }

    public ref ShaderKey GetOrAddShaderKey(uint category, uint defaultValue, out int i)
    {
        i = FindOrAddShaderKey(category, defaultValue);
        return ref ShaderPackage.ShaderKeys[i];
    }

    public ref ShaderKey GetOrAddShaderKey(uint category, uint defaultValue)
        => ref GetOrAddShaderKey(category, defaultValue, out _);

    public void GarbageCollect(ShpkFile? shpk, IReadOnlySet<uint> keepSamplers, bool keepColorDyeSet)
    {
        static bool ShallKeepConstant(MtrlFile mtrl, ShpkFile shpk, Constant constant)
        {
            if ((constant.ByteOffset & 0x3) != 0 || (constant.ByteSize & 0x3) != 0)
                return true;
            var shpkParam = shpk.GetMaterialParamById(constant.Id);
            if (!shpkParam.HasValue)
                return false;
            foreach (var value in mtrl.GetConstantValues(constant))
                if (value != 0.0f)
                    return true;
            return false;
        }

        if (!keepSamplers.Contains(ShpkFile.TableSamplerId))
        {
            for (var i = 0; i < ColorSets.Length; i++)
                ColorSets[i].HasRows = false;
            ColorDyeSets = Array.Empty<ColorDyeSet>();
            if (AdditionalData.Length > 0)
                AdditionalData[0] &= unchecked((byte)~0x0C);
        }
        else if (!keepColorDyeSet)
        {
            ColorDyeSets = Array.Empty<ColorDyeSet>();
            if (AdditionalData.Length > 0)
                AdditionalData[0] &= unchecked((byte)~0x08);
        }

        for (var i = ShaderPackage.Samplers.Length; i-- > 0;)
            if (!keepSamplers.Contains(ShaderPackage.Samplers[i].SamplerId))
                ShaderPackage.Samplers = ShaderPackage.Samplers.RemoveItems(i);

        var samplersByTexture = GetSamplersByTexture(null);
        for (var i = samplersByTexture.Length; i-- > 0;)
        {
            if (samplersByTexture[i].MtrlSampler == null)
            {
                Textures = Textures.RemoveItems(i);
                for (var j = 0; j < ShaderPackage.Samplers.Length; ++j)
                    if (ShaderPackage.Samplers[j].TextureIndex > i)
                        --ShaderPackage.Samplers[j].TextureIndex;
            }
        }

        if (shpk == null)
            return;

        for (var i = ShaderPackage.ShaderKeys.Length; i-- > 0;)
        {
            var key = ShaderPackage.ShaderKeys[i];
            var shpkKey = shpk.GetMaterialKeyById(key.Category);
            if (!shpkKey.HasValue || key.Value == shpkKey.Value.DefaultValue)
                ShaderPackage.ShaderKeys = ShaderPackage.ShaderKeys.RemoveItems(i);
        }

        var usedValues = new BitArray(ShaderPackage.ShaderValues.Length, false);
        for (var i = ShaderPackage.Constants.Length; i-- > 0;)
        {
            var constant = ShaderPackage.Constants[i];
            if (ShallKeepConstant(this, shpk, constant))
            {
                var start = constant.ByteOffset >> 2;
                var end = Math.Min((constant.ByteOffset + constant.ByteSize + 0x3) >> 2, ShaderPackage.ShaderValues.Length);
                for (var j = start; j < end; j++)
                    usedValues[j] = true;
            }
            else
                ShaderPackage.Constants = ShaderPackage.Constants.RemoveItems(i);
        }

        for (var i = ShaderPackage.ShaderValues.Length; i-- > 0;)
        {
            if (usedValues[i])
                continue;
            var end = i + 1;
            while (i >= 0 && !usedValues[i])
                --i;
            ++i;
            ShaderPackage.ShaderValues = ShaderPackage.ShaderValues.RemoveItems(i, end - i);
            var byteStart = (ushort)(i << 2);
            var byteShift = (ushort)((end - i) << 2);
            for (var j = 0; j < ShaderPackage.Constants.Length; ++j)
                if (ShaderPackage.Constants[j].ByteOffset > byteStart)
                    ShaderPackage.Constants[j].ByteOffset -= byteShift;
        }
    }
}
