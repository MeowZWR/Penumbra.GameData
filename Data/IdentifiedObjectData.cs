using FFXIVClientStructs.FFXIV.Client.Graphics.Scene;
using Lumina.Excel.Sheets;
using Luna;
using Penumbra.Api.Enums;
using Penumbra.GameData.Enums;
using Penumbra.GameData.Structs;
using Action = Lumina.Excel.Sheets.Action;
using Emote = Lumina.Excel.Sheets.Emote;
using ModelChara = Lumina.Excel.Sheets.ModelChara;

namespace Penumbra.GameData.Data;

public interface IIdentifiedObjectData
{
    object? ToInternalObject();

    (ChangedItemType Type, uint Id) ToApiObject();

    string ToName(string key);

    string AdditionalData { get; }

    bool FilteredOut(ReadOnlySpan<char> key, ReadOnlySpan<char> filter);

    ChangedItemIcon Icon  { get; }
    int             Count { get; set; }
}

public static class IdentifiedObjectExtensions
{
    public static (ChangedItemType Type, uint Id) ToApiObject(this object? value)
        => value switch
        {
            null                      => (ChangedItemType.None, 0),
            EquipItem it              => (it.Type.IsOffhandType() ? ChangedItemType.ItemOffhand : ChangedItemType.Item, it.Id.Item.Id),
            (Item i, FullEquipType t) => (t.IsOffhandType() ? ChangedItemType.ItemOffhand : ChangedItemType.Item, i.RowId),
            Action a                  => (ChangedItemType.Action, a.RowId),
            ModelChara m              => (ChangedItemType.Model, m.RowId),
            Emote e                   => (ChangedItemType.Emote, e.RowId),
            (ModelRace r, Gender g, CustomizeIndex t, CustomizeValue v) => (ChangedItemType.Customization,
                IdentifiedCustomization.Combine(r, g, t, v)),
            _ => (ChangedItemType.Unknown, 0),
        };

    public static bool IsFilteredOut(this IIdentifiedObjectData? data, ReadOnlySpan<char> key, ReadOnlySpan<char> filter)
        => data?.FilteredOut(key, filter) ?? !key.Contains(filter, StringComparison.OrdinalIgnoreCase);

    public static ChangedItemIcon GetIcon(this IIdentifiedObjectData? data)
        => data?.Icon ?? ChangedItemIcon.Unknown;

    public static ChangedItemIcon GetCategoryIcon(this FullEquipType type)
        => type switch
        {
            FullEquipType.Unknown         => ChangedItemIcon.Unknown,
            FullEquipType.Head            => ChangedItemIcon.Head,
            FullEquipType.Body            => ChangedItemIcon.Body,
            FullEquipType.Hands           => ChangedItemIcon.Hands,
            FullEquipType.Legs            => ChangedItemIcon.Legs,
            FullEquipType.Feet            => ChangedItemIcon.Feet,
            FullEquipType.Ears            => ChangedItemIcon.Ears,
            FullEquipType.Neck            => ChangedItemIcon.Neck,
            FullEquipType.Wrists          => ChangedItemIcon.Wrists,
            FullEquipType.Finger          => ChangedItemIcon.Finger,
            FullEquipType.Fists           => ChangedItemIcon.Mainhand,
            FullEquipType.FistsOff        => ChangedItemIcon.Offhand,
            FullEquipType.Sword           => ChangedItemIcon.Mainhand,
            FullEquipType.Axe             => ChangedItemIcon.Mainhand,
            FullEquipType.Bow             => ChangedItemIcon.Mainhand,
            FullEquipType.BowOff          => ChangedItemIcon.Offhand,
            FullEquipType.Lance           => ChangedItemIcon.Mainhand,
            FullEquipType.StaffWhm        => ChangedItemIcon.Mainhand,
            FullEquipType.StaffBlm        => ChangedItemIcon.Mainhand,
            FullEquipType.Wand            => ChangedItemIcon.Mainhand,
            FullEquipType.Scepter         => ChangedItemIcon.Mainhand,
            FullEquipType.Book            => ChangedItemIcon.Mainhand,
            FullEquipType.Daggers         => ChangedItemIcon.Mainhand,
            FullEquipType.DaggersOff      => ChangedItemIcon.Offhand,
            FullEquipType.Broadsword      => ChangedItemIcon.Mainhand,
            FullEquipType.Gun             => ChangedItemIcon.Mainhand,
            FullEquipType.GunOff          => ChangedItemIcon.Offhand,
            FullEquipType.Orrery          => ChangedItemIcon.Mainhand,
            FullEquipType.OrreryOff       => ChangedItemIcon.Offhand,
            FullEquipType.Katana          => ChangedItemIcon.Mainhand,
            FullEquipType.KatanaOff       => ChangedItemIcon.Offhand,
            FullEquipType.Rapier          => ChangedItemIcon.Mainhand,
            FullEquipType.RapierOff       => ChangedItemIcon.Offhand,
            FullEquipType.Cane            => ChangedItemIcon.Mainhand,
            FullEquipType.Gunblade        => ChangedItemIcon.Mainhand,
            FullEquipType.Glaives         => ChangedItemIcon.Mainhand,
            FullEquipType.GlaivesOff      => ChangedItemIcon.Offhand,
            FullEquipType.Scythe          => ChangedItemIcon.Mainhand,
            FullEquipType.Nouliths        => ChangedItemIcon.Mainhand,
            FullEquipType.Shield          => ChangedItemIcon.Offhand,
            FullEquipType.Saw             => ChangedItemIcon.Mainhand,
            FullEquipType.CrossPeinHammer => ChangedItemIcon.Mainhand,
            FullEquipType.RaisingHammer   => ChangedItemIcon.Mainhand,
            FullEquipType.LapidaryHammer  => ChangedItemIcon.Mainhand,
            FullEquipType.Knife           => ChangedItemIcon.Mainhand,
            FullEquipType.Needle          => ChangedItemIcon.Mainhand,
            FullEquipType.Alembic         => ChangedItemIcon.Mainhand,
            FullEquipType.Frypan          => ChangedItemIcon.Mainhand,
            FullEquipType.Pickaxe         => ChangedItemIcon.Mainhand,
            FullEquipType.Hatchet         => ChangedItemIcon.Mainhand,
            FullEquipType.FishingRod      => ChangedItemIcon.Mainhand,
            FullEquipType.ClawHammer      => ChangedItemIcon.Offhand,
            FullEquipType.File            => ChangedItemIcon.Offhand,
            FullEquipType.Pliers          => ChangedItemIcon.Offhand,
            FullEquipType.GrindingWheel   => ChangedItemIcon.Offhand,
            FullEquipType.Awl             => ChangedItemIcon.Offhand,
            FullEquipType.SpinningWheel   => ChangedItemIcon.Offhand,
            FullEquipType.Mortar          => ChangedItemIcon.Offhand,
            FullEquipType.CulinaryKnife   => ChangedItemIcon.Offhand,
            FullEquipType.Sledgehammer    => ChangedItemIcon.Offhand,
            FullEquipType.GardenScythe    => ChangedItemIcon.Offhand,
            FullEquipType.Gig             => ChangedItemIcon.Offhand,
            FullEquipType.Brush           => ChangedItemIcon.Mainhand,
            FullEquipType.Palette         => ChangedItemIcon.Offhand,
            FullEquipType.Twinfangs       => ChangedItemIcon.Mainhand,
            FullEquipType.TwinfangsOff    => ChangedItemIcon.Offhand,
            FullEquipType.Handaxe         => ChangedItemIcon.Mainhand,
            FullEquipType.UnknownMainhand => ChangedItemIcon.Mainhand,
            FullEquipType.UnknownOffhand  => ChangedItemIcon.Offhand,
            FullEquipType.Glasses         => ChangedItemIcon.Head,
            _                             => ChangedItemIcon.Unknown,
        };

    public static void UpdateCountOrSet<T>(this IDictionary<string, IIdentifiedObjectData> dict, string key, Func<T> generate)
        where T : IIdentifiedObjectData
    {
        if (dict.TryGetValue(key, out var data))
            ++data.Count;
        else
            dict[key] = generate();
    }

    public static void AddEmote(this IDictionary<string, IIdentifiedObjectData> dict, string key, Emote emote, int? pose)
    {
        if (dict.TryGetValue(key, out var data) && data is IdentifiedEmote identified)
            identified.Merge(pose);
        else
            dict[key] = new IdentifiedEmote(emote, pose);
    }
}

public sealed class IdentifiedItem(EquipItem item, int count = 1) : IIdentifiedObjectData
{
    public EquipItem Item = item;

    public object ToInternalObject()
        => Item;

    public int Count { get; set; } = count;

    public (ChangedItemType Type, uint Id) ToApiObject()
    {
        if (Item.Id.IsItem)
            return (Item.Type.IsOffhandType() ? ChangedItemType.ItemOffhand : ChangedItemType.Item, Item.ItemId.Id);

        if (Item.Type.ToSlot() is EquipSlot.MainHand or EquipSlot.OffHand)
            return (ChangedItemType.Unknown, 0);

        return (ChangedItemType.CustomArmor, Item.PrimaryId.Id | ((uint)Item.Variant.Id << 16) | ((uint)Item.Type << 24));
    }

    public string ToName(string key)
        => Item.Name;

    public string AdditionalData
        => Item.ModelString;

    public bool FilteredOut(ReadOnlySpan<char> key, ReadOnlySpan<char> filter)
        => !key.Contains(filter, StringComparison.OrdinalIgnoreCase) && !AdditionalData.Contains(filter, StringComparison.OrdinalIgnoreCase);

    public ChangedItemIcon Icon
        => Item.Type.GetCategoryIcon();

    public static (PrimaryId Model, Variant Variant, FullEquipType Type) Split(uint id)
        => ((PrimaryId)id, (Variant)(id >> 16), (FullEquipType)(id >> 24));
}

public sealed class IdentifiedCustomization(int count = 1) : IIdentifiedObjectData
{
    public ModelRace      Race   = ModelRace.Unknown;
    public Gender         Gender = Gender.Unknown;
    public CustomizeIndex Type;
    public CustomizeValue Value;

    public int Count { get; set; } = count;

    public static IdentifiedCustomization FacePaint(CustomizeValue value, int count = 1)
        => new(count)
        {
            Type  = CustomizeIndex.FacePaint,
            Value = value,
        };

    public static IdentifiedCustomization Hair(ModelRace race, Gender gender, CustomizeValue value, int count = 1)
        => new(count)
        {
            Race   = race,
            Gender = gender,
            Type   = CustomizeIndex.Hairstyle,
            Value  = value,
        };

    public static IdentifiedCustomization Tail(ModelRace race, Gender gender, CustomizeValue value, int count = 1)
        => new(count)
        {
            Race   = race,
            Gender = gender,
            Type   = CustomizeIndex.TailShape,
            Value  = value,
        };

    public static IdentifiedCustomization Ears(ModelRace race, Gender gender, CustomizeValue value, int count = 1)
        => new(count)
        {
            Race   = race,
            Gender = gender,
            Type   = CustomizeIndex.TailShape,
            Value  = value,
        };

    public static IdentifiedCustomization Face(ModelRace race, Gender gender, CustomizeValue value, int count = 1)
        => new(count)
        {
            Race   = race,
            Gender = gender,
            Type   = CustomizeIndex.Face,
            Value  = value,
        };

    public object ToInternalObject()
        => (Race, Gender, Type, Value);

    public (ChangedItemType Type, uint Id) ToApiObject()
        => (ChangedItemType.Customization, Combine(Race, Gender, Type, Value));

    public static uint Combine(ModelRace r, Gender g, CustomizeIndex t, CustomizeValue v)
        => (uint)r | ((uint)g << 8) | ((uint)t << 16) | ((uint)v.Value << 24);

    public static (ModelRace r, Gender g, CustomizeIndex t, CustomizeValue v) Split(uint value)
        => ((ModelRace)(value & 0xFF), (Gender)(value >> 8), (CustomizeIndex)(value >> 16), (CustomizeValue)(value >> 24));

    public string ToName(string key)
        => key;

    public string AdditionalData
        => string.Empty;

    public bool FilteredOut(ReadOnlySpan<char> key, ReadOnlySpan<char> filter)
        => !key.Contains(filter, StringComparison.OrdinalIgnoreCase);

    public ChangedItemIcon Icon
        => ChangedItemIcon.Customization;
}

public sealed class IdentifiedName(int count = 1) : IIdentifiedObjectData
{
    public object? ToInternalObject()
        => null;

    public (ChangedItemType Type, uint Id) ToApiObject()
        => (ChangedItemType.None, 0);

    public string ToName(string key)
        => key;

    public string AdditionalData
        => string.Empty;

    public bool FilteredOut(ReadOnlySpan<char> key, ReadOnlySpan<char> filter)
        => !key.Contains(filter, StringComparison.OrdinalIgnoreCase);

    public ChangedItemIcon Icon
        => ChangedItemIcon.Unknown;

    public int Count { get; set; } = count;
}

public sealed class IdentifiedCounter(int counter = 1) : IIdentifiedObjectData
{
    public int Counter = counter;

    public int Count
    {
        get => Counter;
        set => Counter = value;
    }

    public static IdentifiedCounter operator +(IdentifiedCounter lhs, IdentifiedCounter rhs)
        => new(lhs.Counter + rhs.Counter);

    public object ToInternalObject()
        => Counter;

    public (ChangedItemType Type, uint Id) ToApiObject()
        => (ChangedItemType.None, 0);

    public string ToName(string key)
        => $"{Counter} 个文件正在操作 {key}s";

    public string AdditionalData
        => string.Empty;

    public bool FilteredOut(ReadOnlySpan<char> key, ReadOnlySpan<char> filter)
        => !key.Contains(filter, StringComparison.OrdinalIgnoreCase)
         && !Counter.Contains(filter)
         && !" 个文件正在操作".Contains(filter);

    public ChangedItemIcon Icon
        => ChangedItemIcon.Unknown;
}

public sealed class IdentifiedModel(ModelChara model, int count = 1) : IIdentifiedObjectData
{
    public readonly ModelChara Model = model;

    public int Count { get; set; } = count;

    public object ToInternalObject()
        => Model;

    public (ChangedItemType Type, uint Id) ToApiObject()
        => (ChangedItemType.Model, Model.RowId);

    public string ToName(string key)
        => key;

    public string AdditionalData
        => $"({((CharacterBase.ModelType)Model.Type).ToName()} {Model.Model}-{Model.Base}-{Model.Variant})";

    public bool FilteredOut(ReadOnlySpan<char> key, ReadOnlySpan<char> filter)
        => !key.Contains(filter, StringComparison.OrdinalIgnoreCase) && !AdditionalData.Contains(filter);

    public ChangedItemIcon Icon
        => (CharacterBase.ModelType)Model.Type switch
        {
            CharacterBase.ModelType.DemiHuman => ChangedItemIcon.Demihuman,
            CharacterBase.ModelType.Monster   => ChangedItemIcon.Monster,
            _                                 => ChangedItemIcon.Unknown,
        };
}

public sealed class IdentifiedAction(Action action, int count = 1) : IIdentifiedObjectData
{
    public readonly Action Action = action;

    public int Count { get; set; } = count;

    public object ToInternalObject()
        => Action;

    public (ChangedItemType Type, uint Id) ToApiObject()
        => (ChangedItemType.Action, Action.RowId);

    public string ToName(string key)
        => key;

    public string AdditionalData
        => $"({Action.RowId})";

    public bool FilteredOut(ReadOnlySpan<char> key, ReadOnlySpan<char> filter)
        => !key.Contains(filter, StringComparison.OrdinalIgnoreCase) && !AdditionalData.Contains(filter);

    public ChangedItemIcon Icon
        => ChangedItemIcon.Action;
}

public sealed class IdentifiedEmote(Emote emote, int? pose = null, int count = 1) : IIdentifiedObjectData
{
    private const ushort ChairPoses  = 0b1_1111;
    private const ushort GroundPoses = 0b1111;
    private const ushort StandPoses  = 0b0111_1110;

    public readonly Emote Emote = emote;

    private readonly ushort _fullPoseMask = FullMaskFor(emote.RowId);
    private          ushort _poseMask     = PoseBit(pose);
    private          bool   _hasGenericFile = PoseBit(pose) == 0;

    public int Count { get; set; } = count;

    public void Merge(int? nextPose)
    {
        ++Count;
        var bit = PoseBit(nextPose);
        if (bit == 0)
            _hasGenericFile = true;
        else
            _poseMask |= bit;
    }

    public static bool TryGetPoseIndex(string fileName, out int pose)
    {
        pose = -1;
        if (!fileName.EndsWith(".pap", StringComparison.OrdinalIgnoreCase))
            return false;

        var stem = fileName.ToLowerInvariant()[..^4];
        if (stem.EndsWith("_center_loop"))
            stem = stem[..^"_center_loop".Length];
        else if (stem.EndsWith("_loop"))
            stem = stem[..^"_loop".Length];
        else if (stem.EndsWith("_start"))
            stem = stem[..^"_start".Length];

        if (stem is "sit" or "jmn")
        {
            pose = 0;
            return true;
        }

        var mask = StandPoses;
        if (stem.Length > 2 && stem[1] == '_' && stem[0] is 's' or 'j' or 'l' or 'b')
        {
            mask = stem[0] switch { 's' => ChairPoses, 'j' => GroundPoses, _ => StandPoses };
            stem = stem[2..];
        }

        if (stem.Length != 6 || !stem.StartsWith("pose") || stem[4] is < '0' or > '9' || stem[5] is < '0' or > '9')
            return false;

        var number = (stem[4] - '0') * 10 + (stem[5] - '0');
        if (number is < 1 or > 6 || (mask & (1 << number)) == 0)
            return false;

        pose = number;
        return true;
    }

    public object ToInternalObject()
        => Emote;

    public (ChangedItemType Type, uint Id) ToApiObject()
        => (ChangedItemType.Emote, Emote.RowId);

    public string ToName(string key)
        => key;

    public string AdditionalData
        => FormatPoseSummary() is { Length: > 0 } summary ? $"({Emote.RowId}) {summary}" : $"({Emote.RowId})";

    public bool FilteredOut(ReadOnlySpan<char> key, ReadOnlySpan<char> filter)
        => !key.Contains(filter, StringComparison.OrdinalIgnoreCase) && !AdditionalData.Contains(filter);

    public ChangedItemIcon Icon
        => ChangedItemIcon.Emote;

    private static ushort FullMaskFor(uint rowId)
        => rowId switch
        {
            50               => ChairPoses,
            52               => GroundPoses,
            90 or 218 or 219 => StandPoses,
            _                => 0,
        };

    private static ushort PoseBit(int? pose)
        => pose is >= 0 and <= 6 ? (ushort)(1 << pose.Value) : (ushort)0;

    private string FormatPoseSummary()
    {
        if (_poseMask == 0)
            return string.Empty;

        if (_fullPoseMask != 0 && _poseMask == _fullPoseMask)
            return _hasGenericFile ? "全部姿势及通用" : "全部姿势";

        var text = "姿势";
        for (var index = 0; index <= 6; ++index)
            if ((_poseMask & (1 << index)) != 0)
                text = text.Length == 2 ? $"{text} {index}" : $"{text}、{index}";

        return _hasGenericFile ? $"{text} 及通用" : text;
    }
}
