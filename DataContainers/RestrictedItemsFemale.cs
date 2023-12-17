using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Lumina.Excel.GeneratedSheets;
using Penumbra.GameData.Data;
using Penumbra.GameData.DataContainers.Bases;
using Penumbra.GameData.Enums;
using Penumbra.GameData.Structs;
using Race = Penumbra.GameData.Enums.Race;

namespace Penumbra.GameData.DataContainers;

public sealed class RestrictedItemsFemale(DalamudPluginInterface pluginInterface, IPluginLog log, IDataManager gameData)
    : DataSharer<IReadOnlyDictionary<uint, uint>>(pluginInterface, log, "GenderRestrictedItemsFemale", gameData.Language, 1,
        () => CreateItems(log, gameData))
{
    public (bool Replaced, CharacterArmor Armor) Resolve(CharacterArmor armor, EquipSlot slot, Race race, Gender gender)
    {
        if (gender is Gender.Female or Gender.FemaleNpc)
            return (false, armor);

        var needle = armor.Set.Id | (uint)armor.Variant.Id << 16 | (uint)slot.ToSlot() << 24;
        if (Value.TryGetValue(needle, out var newValue))
            return (true, new CharacterArmor((ushort)newValue, (byte)(newValue >> 16), armor.Stain));
        return (false, armor);
    }

    private static IReadOnlyDictionary<uint, uint> CreateItems(IPluginLog log, IDataManager gameData)
    {
        var ret = new Dictionary<uint, uint>();
        var items = gameData.GetExcelSheet<Item>()!;
        foreach (var pair in GenderRestrictedItems.KnownItems)
            GenderRestrictedItems.AddItemFemale(ret, pair, items, log);
        GenderRestrictedItems.AddUnknownItems(ret, items, log, 3);
        return ret;
    }

    public override long ComputeMemory()
        => DataUtility.DictionaryMemory(8, Value.Count);

    public override int ComputeTotalCount()
        => Value.Count;
}