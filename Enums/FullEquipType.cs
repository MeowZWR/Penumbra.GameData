using Lumina.Excel.GeneratedSheets;

namespace Penumbra.GameData.Enums;

/// <summary> A full equipment type representing any type of equipment a character can wear. </summary>
public enum FullEquipType : byte
{
    Unknown,

    Head,
    Body,
    Hands,
    Legs,
    Feet,

    Ears,
    Neck,
    Wrists,
    Finger,

    Fists, // PGL, MNK
    FistsOff,
    Sword, // GLA, PLD Main
    Axe,   // MRD, WAR
    Bow,   // ARC, BRD
    BowOff,
    Lance,   // LNC, DRG,
    Staff,   // THM, BLM, CNJ, WHM
    Wand,    // THM, BLM, CNJ, WHM Main
    Book,    // ACN, SMN, SCH
    Daggers, // ROG, NIN
    DaggersOff,
    Broadsword, // DRK,
    Gun,        // MCH,
    GunOff,
    Orrery, // AST,
    OrreryOff,
    Katana, // SAM
    KatanaOff,
    Rapier, // RDM
    RapierOff,
    Cane,     // BLU
    Gunblade, // GNB,
    Glaives,  // DNC,
    GlaivesOff,
    Scythe,   // RPR,
    Nouliths, // SGE
    Shield,   // GLA, PLD, THM, BLM, CNJ, WHM Off

    Saw,             // CRP
    CrossPeinHammer, // BSM
    RaisingHammer,   // ARM
    LapidaryHammer,  // GSM
    Knife,           // LTW
    Needle,          // WVR
    Alembic,         // ALC
    Frypan,          // CUL
    Pickaxe,         // MIN
    Hatchet,         // BTN
    FishingRod,      // FSH

    ClawHammer,    // CRP Off
    File,          // BSM Off
    Pliers,        // ARM Off
    GrindingWheel, // GSM Off
    Awl,           // LTW Off
    SpinningWheel, // WVR Off
    Mortar,        // ALC Off
    CulinaryKnife, // CUL Off
    Sledgehammer,  // MIN Off
    GardenScythe,  // BTN Off
    Gig,           // FSH Off

    Brush,        // PCT
    Palette,      // PCT Off
    Twinfangs,    // VPR
    TwinfangsOff, // VPR Off
    Whip,         // BMR TODO

    Glasses,

    UnknownMainhand,
    UnknownOffhand,
}

public static class FullEquipTypeExtensions
{
    /// <summary> Obtain the FullEquipType of an item. </summary>
    internal static FullEquipType ToEquipType(this Item item)
    {
        var slot   = (EquipSlot)item.EquipSlotCategory.Row;
        var weapon = (WeaponCategory)item.ItemUICategory.Row;
        return slot.ToEquipType(weapon);
    }

    /// <summary> Return whether a FullEquipType is a primary weapon type. </summary>
    public static bool IsWeapon(this FullEquipType type)
        => type switch
        {
            FullEquipType.Fists           => true,
            FullEquipType.Sword           => true,
            FullEquipType.Axe             => true,
            FullEquipType.Bow             => true,
            FullEquipType.Lance           => true,
            FullEquipType.Staff           => true,
            FullEquipType.Wand            => true,
            FullEquipType.Book            => true,
            FullEquipType.Daggers         => true,
            FullEquipType.Broadsword      => true,
            FullEquipType.Gun             => true,
            FullEquipType.Orrery          => true,
            FullEquipType.Katana          => true,
            FullEquipType.Rapier          => true,
            FullEquipType.Cane            => true,
            FullEquipType.Gunblade        => true,
            FullEquipType.Glaives         => true,
            FullEquipType.Scythe          => true,
            FullEquipType.Nouliths        => true,
            FullEquipType.Shield          => true,
            FullEquipType.Brush           => true,
            FullEquipType.Twinfangs       => true,
            FullEquipType.UnknownMainhand => true,
            _                             => false,
        };

    /// <summary> Return whether a FullEquipType is a primary or secondary tool. </summary>
    public static bool IsTool(this FullEquipType type)
        => type switch
        {
            FullEquipType.Saw             => true,
            FullEquipType.CrossPeinHammer => true,
            FullEquipType.RaisingHammer   => true,
            FullEquipType.LapidaryHammer  => true,
            FullEquipType.Knife           => true,
            FullEquipType.Needle          => true,
            FullEquipType.Alembic         => true,
            FullEquipType.Frypan          => true,
            FullEquipType.Pickaxe         => true,
            FullEquipType.Hatchet         => true,
            FullEquipType.FishingRod      => true,
            FullEquipType.ClawHammer      => true,
            FullEquipType.File            => true,
            FullEquipType.Pliers          => true,
            FullEquipType.GrindingWheel   => true,
            FullEquipType.Awl             => true,
            FullEquipType.SpinningWheel   => true,
            FullEquipType.Mortar          => true,
            FullEquipType.CulinaryKnife   => true,
            FullEquipType.Sledgehammer    => true,
            FullEquipType.GardenScythe    => true,
            FullEquipType.Gig             => true,
            _                             => false,
        };

    /// <summary> Return whether a FullEquipType is a piece of primary equipment. </summary>
    public static bool IsEquipment(this FullEquipType type)
        => type switch
        {
            FullEquipType.Head  => true,
            FullEquipType.Body  => true,
            FullEquipType.Hands => true,
            FullEquipType.Legs  => true,
            FullEquipType.Feet  => true,
            _                   => false,
        };

    /// <summary> Return whether a FullEquipType is a piece of secondary equipment. </summary>
    public static bool IsAccessory(this FullEquipType type)
        => type switch
        {
            FullEquipType.Ears   => true,
            FullEquipType.Neck   => true,
            FullEquipType.Wrists => true,
            FullEquipType.Finger => true,
            _                    => false,
        };

    /// <summary> Return whether a FullEquipType is a bonus slot. </summary>
    public static bool IsBonus(this FullEquipType type)
        => type switch
        {
            FullEquipType.Glasses => true,
            _                     => false,
        };

    public static BonusItemFlag ToBonus(this FullEquipType type)
        => type switch
        {
            FullEquipType.Glasses => BonusItemFlag.Glasses,
            _                     => BonusItemFlag.Unknown,
        };

    /// <summary> Obtain a human-readable name for a FullEquipType. </summary>
    public static string ToName(this FullEquipType type)
        => type switch
        {
            FullEquipType.Head            => EquipSlot.Head.ToName(),
            FullEquipType.Body            => EquipSlot.Body.ToName(),
            FullEquipType.Hands           => EquipSlot.Hands.ToName(),
            FullEquipType.Legs            => EquipSlot.Legs.ToName(),
            FullEquipType.Feet            => EquipSlot.Feet.ToName(),
            FullEquipType.Ears            => EquipSlot.Ears.ToName(),
            FullEquipType.Neck            => EquipSlot.Neck.ToName(),
            FullEquipType.Wrists          => EquipSlot.Wrists.ToName(),
            FullEquipType.Finger          => "戒指",
            FullEquipType.Fists           => "格斗武器",
            FullEquipType.FistsOff        => "格斗武器（副手）",
            FullEquipType.Sword           => "单手剑",
            FullEquipType.Axe             => "大斧",
            FullEquipType.Bow             => "弓",
            FullEquipType.BowOff          => "箭袋",
            FullEquipType.Lance           => "长枪",
            FullEquipType.Staff           => "双手杖",
            FullEquipType.Wand            => "单手杖",
            FullEquipType.Book            => "魔导书",
            FullEquipType.Daggers         => "双剑",
            FullEquipType.DaggersOff      => "双剑（副手）",
            FullEquipType.Broadsword      => "双手剑",
            FullEquipType.Gun             => "火枪",
            FullEquipType.GunOff          => "以太变换器",
            FullEquipType.Orrery          => "天球仪",
            FullEquipType.OrreryOff       => "卡套",
            FullEquipType.Katana          => "武士刀",
            FullEquipType.KatanaOff       => "刀鞘",
            FullEquipType.Rapier          => "刺剑",
            FullEquipType.RapierOff       => "触媒",
            FullEquipType.Cane            => "青魔杖",
            FullEquipType.Gunblade        => "枪刃",
            FullEquipType.Glaives         => "投掷武器",
            FullEquipType.GlaivesOff      => "投掷武器（副手）",
            FullEquipType.Scythe          => "镰刀",
            FullEquipType.Nouliths        => "贤具",
            FullEquipType.Shield          => "盾",
            FullEquipType.Saw             => "刻木工具",
            FullEquipType.CrossPeinHammer => "锻铁工具",
            FullEquipType.RaisingHammer   => "铸甲工具",
            FullEquipType.LapidaryHammer  => "雕金工具",
            FullEquipType.Knife           => "制革工具",
            FullEquipType.Needle          => "裁衣工具",
            FullEquipType.Alembic         => "炼金工具",
            FullEquipType.Frypan          => "烹调工具",
            FullEquipType.Pickaxe         => "采矿工具",
            FullEquipType.Hatchet         => "园艺工具",
            FullEquipType.FishingRod      => "捕鱼用具",
            FullEquipType.ClawHammer      => "羊角锤",
            FullEquipType.File            => "锉刀",
            FullEquipType.Pliers          => "手钳",
            FullEquipType.GrindingWheel   => "砂轮机",
            FullEquipType.Awl             => "平斩",
            FullEquipType.SpinningWheel   => "纺车",
            FullEquipType.Mortar          => "研钵",
            FullEquipType.CulinaryKnife   => "厨刀",
            FullEquipType.Sledgehammer    => "碎石锤",
            FullEquipType.GardenScythe    => "园艺镰刀",
            FullEquipType.Gig             => "渔叉",
            FullEquipType.Brush           => "笔刷",
            FullEquipType.Palette         => "调色盘",
            FullEquipType.Twinfangs       => "双剑",
            FullEquipType.TwinfangsOff    => "双剑（副手）",
            FullEquipType.Whip            => "鞭子",
            FullEquipType.Glasses         => "眼镜",
            FullEquipType.UnknownMainhand => EquipSlot.MainHand.ToName(),
            FullEquipType.UnknownOffhand  => EquipSlot.OffHand.ToName(),
            _                             => "未知",
        };

    /// <summary> Return the actual equipment slot a FullEquipType will be equipped to. </summary>
    public static EquipSlot ToSlot(this FullEquipType type)
        => type switch
        {
            FullEquipType.Head            => EquipSlot.Head,
            FullEquipType.Body            => EquipSlot.Body,
            FullEquipType.Hands           => EquipSlot.Hands,
            FullEquipType.Legs            => EquipSlot.Legs,
            FullEquipType.Feet            => EquipSlot.Feet,
            FullEquipType.Ears            => EquipSlot.Ears,
            FullEquipType.Neck            => EquipSlot.Neck,
            FullEquipType.Wrists          => EquipSlot.Wrists,
            FullEquipType.Finger          => EquipSlot.RFinger,
            FullEquipType.Fists           => EquipSlot.MainHand,
            FullEquipType.FistsOff        => EquipSlot.OffHand,
            FullEquipType.Sword           => EquipSlot.MainHand,
            FullEquipType.Axe             => EquipSlot.MainHand,
            FullEquipType.Bow             => EquipSlot.MainHand,
            FullEquipType.BowOff          => EquipSlot.OffHand,
            FullEquipType.Lance           => EquipSlot.MainHand,
            FullEquipType.Staff           => EquipSlot.MainHand,
            FullEquipType.Wand            => EquipSlot.MainHand,
            FullEquipType.Book            => EquipSlot.MainHand,
            FullEquipType.Daggers         => EquipSlot.MainHand,
            FullEquipType.DaggersOff      => EquipSlot.OffHand,
            FullEquipType.Broadsword      => EquipSlot.MainHand,
            FullEquipType.Gun             => EquipSlot.MainHand,
            FullEquipType.GunOff          => EquipSlot.OffHand,
            FullEquipType.Orrery          => EquipSlot.MainHand,
            FullEquipType.OrreryOff       => EquipSlot.OffHand,
            FullEquipType.Katana          => EquipSlot.MainHand,
            FullEquipType.KatanaOff       => EquipSlot.OffHand,
            FullEquipType.Rapier          => EquipSlot.MainHand,
            FullEquipType.RapierOff       => EquipSlot.OffHand,
            FullEquipType.Cane            => EquipSlot.MainHand,
            FullEquipType.Gunblade        => EquipSlot.MainHand,
            FullEquipType.Glaives         => EquipSlot.MainHand,
            FullEquipType.GlaivesOff      => EquipSlot.OffHand,
            FullEquipType.Scythe          => EquipSlot.MainHand,
            FullEquipType.Nouliths        => EquipSlot.MainHand,
            FullEquipType.Shield          => EquipSlot.OffHand,
            FullEquipType.Saw             => EquipSlot.MainHand,
            FullEquipType.CrossPeinHammer => EquipSlot.MainHand,
            FullEquipType.RaisingHammer   => EquipSlot.MainHand,
            FullEquipType.LapidaryHammer  => EquipSlot.MainHand,
            FullEquipType.Knife           => EquipSlot.MainHand,
            FullEquipType.Needle          => EquipSlot.MainHand,
            FullEquipType.Alembic         => EquipSlot.MainHand,
            FullEquipType.Frypan          => EquipSlot.MainHand,
            FullEquipType.Pickaxe         => EquipSlot.MainHand,
            FullEquipType.Hatchet         => EquipSlot.MainHand,
            FullEquipType.FishingRod      => EquipSlot.MainHand,
            FullEquipType.ClawHammer      => EquipSlot.OffHand,
            FullEquipType.File            => EquipSlot.OffHand,
            FullEquipType.Pliers          => EquipSlot.OffHand,
            FullEquipType.GrindingWheel   => EquipSlot.OffHand,
            FullEquipType.Awl             => EquipSlot.OffHand,
            FullEquipType.SpinningWheel   => EquipSlot.OffHand,
            FullEquipType.Mortar          => EquipSlot.OffHand,
            FullEquipType.CulinaryKnife   => EquipSlot.OffHand,
            FullEquipType.Sledgehammer    => EquipSlot.OffHand,
            FullEquipType.GardenScythe    => EquipSlot.OffHand,
            FullEquipType.Gig             => EquipSlot.OffHand,
            FullEquipType.Twinfangs       => EquipSlot.MainHand,
            FullEquipType.TwinfangsOff    => EquipSlot.OffHand,
            FullEquipType.Brush           => EquipSlot.MainHand,
            FullEquipType.Palette         => EquipSlot.OffHand,
            FullEquipType.Whip            => EquipSlot.MainHand,
            FullEquipType.Glasses         => BonusItemFlag.Glasses.ToEquipSlot(),
            FullEquipType.UnknownMainhand => EquipSlot.MainHand,
            FullEquipType.UnknownOffhand  => EquipSlot.OffHand,
            _                             => EquipSlot.Unknown,
        };

    /// <summary> Convert an EquipSlot and a weapon category to a FullEquipType. </summary>
    /// <param name="slot"> The slot to convert. </param>
    /// <param name="category"> The weapon category to use if the slot is mainhand or offhand. </param>
    /// <param name="mainhand"> Whether to use the mainhand or offhand type of weapon. </param>
    public static FullEquipType ToEquipType(this EquipSlot slot, WeaponCategory category = WeaponCategory.Unknown, bool mainhand = true)
        => slot switch
        {
            EquipSlot.Head              => FullEquipType.Head,
            EquipSlot.Body              => FullEquipType.Body,
            EquipSlot.Hands             => FullEquipType.Hands,
            EquipSlot.Legs              => FullEquipType.Legs,
            EquipSlot.Feet              => FullEquipType.Feet,
            EquipSlot.Ears              => FullEquipType.Ears,
            EquipSlot.Neck              => FullEquipType.Neck,
            EquipSlot.Wrists            => FullEquipType.Wrists,
            EquipSlot.RFinger           => FullEquipType.Finger,
            EquipSlot.LFinger           => FullEquipType.Finger,
            EquipSlot.HeadBody          => FullEquipType.Body,
            EquipSlot.BodyHandsLegsFeet => FullEquipType.Body,
            EquipSlot.LegsFeet          => FullEquipType.Legs,
            EquipSlot.FullBody          => FullEquipType.Body,
            EquipSlot.BodyHands         => FullEquipType.Body,
            EquipSlot.BodyLegsFeet      => FullEquipType.Body,
            EquipSlot.ChestHands        => FullEquipType.Body,
            EquipSlot.MainHand          => category.ToEquipType(mainhand),
            EquipSlot.OffHand           => category.ToEquipType(mainhand),
            EquipSlot.BothHand          => category.ToEquipType(mainhand),
            _                           => FullEquipType.Unknown,
        };

    public static FullEquipType ToEquipType(this BonusItemFlag bonusSlot)
        => bonusSlot switch
        {
            BonusItemFlag.Glasses => FullEquipType.Glasses,
            BonusItemFlag.UnkSlot => FullEquipType.Unknown,
            _                     => FullEquipType.Unknown,
        };

    /// <summary> Convert a weapon category to a FullEquipType. </summary>
    /// <param name="category"> The category to convert. </param>
    /// <param name="mainhand"> Whether to use the mainhand or offhand type of weapon. </param>
    public static FullEquipType ToEquipType(this WeaponCategory category, bool mainhand = true)
        => category switch
        {
            WeaponCategory.Pugilist when mainhand    => FullEquipType.Fists,
            WeaponCategory.Pugilist                  => FullEquipType.FistsOff,
            WeaponCategory.Gladiator                 => FullEquipType.Sword,
            WeaponCategory.Marauder                  => FullEquipType.Axe,
            WeaponCategory.Archer when mainhand      => FullEquipType.Bow,
            WeaponCategory.Archer                    => FullEquipType.BowOff,
            WeaponCategory.Lancer                    => FullEquipType.Lance,
            WeaponCategory.Thaumaturge1              => FullEquipType.Wand,
            WeaponCategory.Thaumaturge2              => FullEquipType.Staff,
            WeaponCategory.Conjurer1                 => FullEquipType.Wand,
            WeaponCategory.Conjurer2                 => FullEquipType.Staff,
            WeaponCategory.Arcanist                  => FullEquipType.Book,
            WeaponCategory.Shield                    => FullEquipType.Shield,
            WeaponCategory.CarpenterMain             => FullEquipType.Saw,
            WeaponCategory.CarpenterOff              => FullEquipType.ClawHammer,
            WeaponCategory.BlacksmithMain            => FullEquipType.CrossPeinHammer,
            WeaponCategory.BlacksmithOff             => FullEquipType.File,
            WeaponCategory.ArmorerMain               => FullEquipType.RaisingHammer,
            WeaponCategory.ArmorerOff                => FullEquipType.Pliers,
            WeaponCategory.GoldsmithMain             => FullEquipType.LapidaryHammer,
            WeaponCategory.GoldsmithOff              => FullEquipType.GrindingWheel,
            WeaponCategory.LeatherworkerMain         => FullEquipType.Knife,
            WeaponCategory.LeatherworkerOff          => FullEquipType.Awl,
            WeaponCategory.WeaverMain                => FullEquipType.Needle,
            WeaponCategory.WeaverOff                 => FullEquipType.SpinningWheel,
            WeaponCategory.AlchemistMain             => FullEquipType.Alembic,
            WeaponCategory.AlchemistOff              => FullEquipType.Mortar,
            WeaponCategory.CulinarianMain            => FullEquipType.Frypan,
            WeaponCategory.CulinarianOff             => FullEquipType.CulinaryKnife,
            WeaponCategory.MinerMain                 => FullEquipType.Pickaxe,
            WeaponCategory.MinerOff                  => FullEquipType.Sledgehammer,
            WeaponCategory.BotanistMain              => FullEquipType.Hatchet,
            WeaponCategory.BotanistOff               => FullEquipType.GardenScythe,
            WeaponCategory.FisherMain                => FullEquipType.FishingRod,
            WeaponCategory.FisherOff                 => FullEquipType.Gig,
            WeaponCategory.Rogue when mainhand       => FullEquipType.Daggers,
            WeaponCategory.Rogue                     => FullEquipType.DaggersOff,
            WeaponCategory.DarkKnight                => FullEquipType.Broadsword,
            WeaponCategory.Machinist when mainhand   => FullEquipType.Gun,
            WeaponCategory.Machinist                 => FullEquipType.GunOff,
            WeaponCategory.Astrologian when mainhand => FullEquipType.Orrery,
            WeaponCategory.Astrologian               => FullEquipType.OrreryOff,
            WeaponCategory.Samurai when mainhand     => FullEquipType.Katana,
            WeaponCategory.Samurai                   => FullEquipType.KatanaOff,
            WeaponCategory.RedMage when mainhand     => FullEquipType.Rapier,
            WeaponCategory.RedMage                   => FullEquipType.RapierOff,
            WeaponCategory.Scholar                   => FullEquipType.Book,
            WeaponCategory.BlueMage                  => FullEquipType.Cane,
            WeaponCategory.Gunbreaker                => FullEquipType.Gunblade,
            WeaponCategory.Dancer when mainhand      => FullEquipType.Glaives,
            WeaponCategory.Dancer                    => FullEquipType.GlaivesOff,
            WeaponCategory.Reaper                    => FullEquipType.Scythe,
            WeaponCategory.Sage                      => FullEquipType.Nouliths,
            WeaponCategory.Viper when mainhand       => FullEquipType.Twinfangs,
            WeaponCategory.Viper                     => FullEquipType.TwinfangsOff,
            WeaponCategory.Pictomancer when mainhand => FullEquipType.Brush,
            WeaponCategory.Pictomancer               => FullEquipType.Palette,
            WeaponCategory.Beastmaster               => FullEquipType.Whip,
            _ when mainhand                          => FullEquipType.UnknownMainhand,
            _                                        => FullEquipType.UnknownOffhand,
        };

    /// <summary> Obtain the correct offhand FullEquipType for a Mainhand FullEquipType, excluding tools. </summary>
    public static FullEquipType ValidOffhand(this FullEquipType type)
        => type switch
        {
            FullEquipType.Fists     => FullEquipType.FistsOff,
            FullEquipType.Sword     => FullEquipType.Shield,
            FullEquipType.Wand      => FullEquipType.Shield,
            FullEquipType.Daggers   => FullEquipType.DaggersOff,
            FullEquipType.Gun       => FullEquipType.GunOff,
            FullEquipType.Orrery    => FullEquipType.OrreryOff,
            FullEquipType.Rapier    => FullEquipType.RapierOff,
            FullEquipType.Glaives   => FullEquipType.GlaivesOff,
            FullEquipType.Bow       => FullEquipType.BowOff,
            FullEquipType.Katana    => FullEquipType.KatanaOff,
            FullEquipType.Twinfangs => FullEquipType.TwinfangsOff,
            FullEquipType.Brush     => FullEquipType.Palette,
            _                       => FullEquipType.Unknown,
        };

    /// <summary> Obtain the correct offhand FullEquipType for a Mainhand FullEquipType, including tools. </summary>
    public static FullEquipType Offhand(this FullEquipType type)
        => type switch
        {
            FullEquipType.Fists           => FullEquipType.FistsOff,
            FullEquipType.Sword           => FullEquipType.Shield,
            FullEquipType.Wand            => FullEquipType.Shield,
            FullEquipType.Daggers         => FullEquipType.DaggersOff,
            FullEquipType.Gun             => FullEquipType.GunOff,
            FullEquipType.Orrery          => FullEquipType.OrreryOff,
            FullEquipType.Rapier          => FullEquipType.RapierOff,
            FullEquipType.Glaives         => FullEquipType.GlaivesOff,
            FullEquipType.Bow             => FullEquipType.BowOff,
            FullEquipType.Katana          => FullEquipType.KatanaOff,
            FullEquipType.Saw             => FullEquipType.ClawHammer,
            FullEquipType.CrossPeinHammer => FullEquipType.File,
            FullEquipType.RaisingHammer   => FullEquipType.Pliers,
            FullEquipType.LapidaryHammer  => FullEquipType.GrindingWheel,
            FullEquipType.Knife           => FullEquipType.Awl,
            FullEquipType.Needle          => FullEquipType.SpinningWheel,
            FullEquipType.Alembic         => FullEquipType.Mortar,
            FullEquipType.Frypan          => FullEquipType.CulinaryKnife,
            FullEquipType.Pickaxe         => FullEquipType.Sledgehammer,
            FullEquipType.Hatchet         => FullEquipType.GardenScythe,
            FullEquipType.FishingRod      => FullEquipType.Gig,
            FullEquipType.Twinfangs       => FullEquipType.TwinfangsOff,
            FullEquipType.Brush           => FullEquipType.Palette,
            _                             => FullEquipType.Unknown,
        };

    /// <summary> Whether an Offhand FullEquipType allows putting Nothing into the Offhand slot. </summary>
    public static bool AllowsNothing(this FullEquipType type)
        => type switch
        {
            FullEquipType.Head      => true,
            FullEquipType.Body      => true,
            FullEquipType.Hands     => true,
            FullEquipType.Legs      => true,
            FullEquipType.Feet      => true,
            FullEquipType.Ears      => true,
            FullEquipType.Neck      => true,
            FullEquipType.Wrists    => true,
            FullEquipType.Finger    => true,
            FullEquipType.BowOff    => true,
            FullEquipType.GunOff    => true,
            FullEquipType.OrreryOff => true,
            FullEquipType.KatanaOff => true,
            FullEquipType.RapierOff => true,
            FullEquipType.Shield    => true,
            FullEquipType.Palette   => true,
            FullEquipType.Glasses   => true,
            _                       => false,
        };

    /// <summary> The human-readable suffix for inferred offhand types. </summary>
    internal static string OffhandTypeSuffix(this FullEquipType type)
        => type switch
        {
            FullEquipType.FistsOff     => "（副手）",
            FullEquipType.DaggersOff   => "（副手）",
            FullEquipType.GunOff       => "（以太变换器）",
            FullEquipType.OrreryOff    => "（卡套）",
            FullEquipType.RapierOff    => "（触媒）",
            FullEquipType.GlaivesOff   => "（副手）",
            FullEquipType.BowOff       => "（箭袋）",
            FullEquipType.KatanaOff    => "（刀鞘）",
            FullEquipType.TwinfangsOff => "（副手）",
            FullEquipType.Palette      => "（调色盘）",
            _                          => string.Empty,
        };

    /// <summary> Whether a FullEquipType is an inferred offhand type. </summary>
    public static bool IsOffhandType(this FullEquipType type)
        => type.OffhandTypeSuffix().Length > 0;

    /// <summary> A list of all weapon types. </summary>
    public static readonly IReadOnlyList<FullEquipType> WeaponTypes
        = Enum.GetValues<FullEquipType>().Where(v => v.IsWeapon()).Except([FullEquipType.UnknownMainhand])
            .ToArray();

    /// <summary> A list of all tool types, including offhands. </summary>
    public static readonly IReadOnlyList<FullEquipType> ToolTypes
        = Enum.GetValues<FullEquipType>().Where(v => v.IsTool()).ToArray();

    /// <summary> A list of all equipment types. </summary>
    public static readonly IReadOnlyList<FullEquipType> EquipmentTypes
        = Enum.GetValues<FullEquipType>().Where(v => v.IsEquipment()).ToArray();

    /// <summary> A list of all accessory types. </summary>
    public static readonly IReadOnlyList<FullEquipType> AccessoryTypes
        = Enum.GetValues<FullEquipType>().Where(v => v.IsAccessory()).ToArray();

    /// <summary> A list of all inferred offhand types. </summary>
    public static readonly IReadOnlyList<FullEquipType> OffhandTypes
        = Enum.GetValues<FullEquipType>().Where(IsOffhandType).ToArray();

    /// <summary> A list of all inferred offhand types. </summary>
    public static readonly IReadOnlyList<FullEquipType> BonusTypes
        = Enum.GetValues<FullEquipType>().Where(IsBonus).ToArray();
}
