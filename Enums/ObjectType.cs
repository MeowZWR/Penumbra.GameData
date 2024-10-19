namespace Penumbra.GameData.Enums;

/// <summary> Types of game objects or identities. </summary>
public enum ObjectType : byte
{
    Unknown,
    Vfx,
    DemiHuman,
    Accessory,
    World,
    Housing,
    Monster,
    Icon,
    LoadingScreen,
    Map,
    Interface,
    Equipment,
    Character,
    Weapon,
    Font,
}

public static class ObjectTypeExtensions
{
    /// <summary> Obtain human-readable names for ObjectType. </summary>
    public static string ToName(this ObjectType type)
        => type switch
        {
            ObjectType.Vfx           => "视觉效果",
            ObjectType.DemiHuman     => "亚人",
            ObjectType.Accessory     => "配饰",
            ObjectType.World         => "小物件",
            ObjectType.Housing       => "装修物品",
            ObjectType.Monster       => "怪物",
            ObjectType.Icon          => "图标",
            ObjectType.LoadingScreen => "加载界面",
            ObjectType.Map           => "地图",
            ObjectType.Interface     => "UI元素",
            ObjectType.Equipment     => "装备",
            ObjectType.Character     => "角色",
            ObjectType.Weapon        => "武器",
            ObjectType.Font          => "字体",
            _                        => "未知",
        };

    /// <summary> A list of valid object types for IMC files. </summary>
    public static readonly IReadOnlyList<ObjectType> ValidImcTypes =
    [
        ObjectType.Equipment,
        ObjectType.Accessory,
        ObjectType.DemiHuman,
        ObjectType.Monster,
        ObjectType.Weapon,
    ];
}
