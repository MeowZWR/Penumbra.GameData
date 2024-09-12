namespace Penumbra.GameData.Enums;

/// <summary> All options that can be configured for humans via the customize array. </summary>
public enum CustomizeIndex : byte
{
    Race,
    Gender,
    BodyType,
    Height,
    Clan,
    Face,
    Hairstyle,
    Highlights,
    SkinColor,
    EyeColorRight,
    HairColor,
    HighlightsColor,
    FacialFeature1,
    FacialFeature2,
    FacialFeature3,
    FacialFeature4,
    FacialFeature5,
    FacialFeature6,
    FacialFeature7,
    LegacyTattoo,
    TattooColor,
    Eyebrows,
    EyeColorLeft,
    EyeShape,
    SmallIris,
    Nose,
    Jaw,
    Mouth,
    Lipstick,
    LipColor,
    MuscleMass,
    TailShape,
    BustSize,
    FacePaint,
    FacePaintReversed,
    FacePaintColor,
}

public static class CustomizationExtensions
{
    /// <summary> The total number of options. </summary>
    public const int NumIndices = (int)CustomizeIndex.FacePaintColor + 1;

    /// <summary> A list of all options that are not race or body type. </summary>
    public static readonly CustomizeIndex[] All = Enum.GetValues<CustomizeIndex>()
        .Where(v => v is not CustomizeIndex.Race and not CustomizeIndex.BodyType).ToArray();

    /// <summary> A set of all options that are not race, gender, clan or body type. </summary>
    public static readonly CustomizeIndex[] AllBasic = All
        .Where(v => v is not CustomizeIndex.Gender and not CustomizeIndex.Clan).ToArray();

    /// <summary> A set of all options that are not race, gender, clan, face, or body type. </summary>
    public static readonly CustomizeIndex[] AllBasicWithoutFace = AllBasic
        .Where(v => v is not CustomizeIndex.Face).ToArray();

    /// <summary> Get the index of the customization option in the customize array, and a mask for its value. </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (int ByteIdx, byte Mask) ToByteAndMask(this CustomizeIndex index)
        => index switch
        {
            CustomizeIndex.Race              => (0, 0xFF),
            CustomizeIndex.Gender            => (1, 0xFF),
            CustomizeIndex.BodyType          => (2, 0xFF),
            CustomizeIndex.Height            => (3, 0xFF),
            CustomizeIndex.Clan              => (4, 0xFF),
            CustomizeIndex.Face              => (5, 0xFF),
            CustomizeIndex.Hairstyle         => (6, 0xFF),
            CustomizeIndex.Highlights        => (7, 0x80),
            CustomizeIndex.SkinColor         => (8, 0xFF),
            CustomizeIndex.EyeColorRight     => (9, 0xFF),
            CustomizeIndex.HairColor         => (10, 0xFF),
            CustomizeIndex.HighlightsColor   => (11, 0xFF),
            CustomizeIndex.FacialFeature1    => (12, 0x01),
            CustomizeIndex.FacialFeature2    => (12, 0x02),
            CustomizeIndex.FacialFeature3    => (12, 0x04),
            CustomizeIndex.FacialFeature4    => (12, 0x08),
            CustomizeIndex.FacialFeature5    => (12, 0x10),
            CustomizeIndex.FacialFeature6    => (12, 0x20),
            CustomizeIndex.FacialFeature7    => (12, 0x40),
            CustomizeIndex.LegacyTattoo      => (12, 0x80),
            CustomizeIndex.TattooColor       => (13, 0xFF),
            CustomizeIndex.Eyebrows          => (14, 0xFF),
            CustomizeIndex.EyeColorLeft      => (15, 0xFF),
            CustomizeIndex.EyeShape          => (16, 0x7F),
            CustomizeIndex.SmallIris         => (16, 0x80),
            CustomizeIndex.Nose              => (17, 0xFF),
            CustomizeIndex.Jaw               => (18, 0xFF),
            CustomizeIndex.Mouth             => (19, 0x7F),
            CustomizeIndex.Lipstick          => (19, 0x80),
            CustomizeIndex.LipColor          => (20, 0xFF),
            CustomizeIndex.MuscleMass        => (21, 0xFF),
            CustomizeIndex.TailShape         => (22, 0xFF),
            CustomizeIndex.BustSize          => (23, 0xFF),
            CustomizeIndex.FacePaint         => (24, 0x7F),
            CustomizeIndex.FacePaintReversed => (24, 0x80),
            CustomizeIndex.FacePaintColor    => (25, 0xFF),
            _                                => (0, 0x00),
        };


    /// <summary> Get the human-readable name for a customization option. </summary>
    public static string ToDefaultName(this CustomizeIndex customizeIndex)
        => customizeIndex switch
        {
            CustomizeIndex.Race              => "种族",
            CustomizeIndex.Gender            => "性别",
            CustomizeIndex.BodyType          => "身型",
            CustomizeIndex.Height            => "身高",
            CustomizeIndex.Clan              => "部族",
            CustomizeIndex.Face              => "脸型",
            CustomizeIndex.Hairstyle         => "发型",
            CustomizeIndex.Highlights        => "启用挑染",
            CustomizeIndex.SkinColor         => "肤色",
            CustomizeIndex.EyeColorRight     => "右眼",
            CustomizeIndex.HairColor         => "发色",
            CustomizeIndex.HighlightsColor   => "挑染颜色",
            CustomizeIndex.TattooColor       => "纹身颜色",
            CustomizeIndex.Eyebrows          => "眉形",
            CustomizeIndex.EyeColorLeft      => "左眼",
            CustomizeIndex.EyeShape          => "小瞳孔",
            CustomizeIndex.Nose              => "鼻型",
            CustomizeIndex.Jaw               => "脸部轮廓",
            CustomizeIndex.Mouth             => "嘴型",
            CustomizeIndex.MuscleMass        => "肌肉",
            CustomizeIndex.TailShape         => "尾巴形状",
            CustomizeIndex.BustSize          => "胸围",
            CustomizeIndex.FacePaint         => "面妆",
            CustomizeIndex.FacePaintColor    => "面妆颜色",
            CustomizeIndex.LipColor          => "唇色",
            CustomizeIndex.FacialFeature1    => "黑痣与伤痕等 1",
            CustomizeIndex.FacialFeature2    => "黑痣与伤痕等 2",
            CustomizeIndex.FacialFeature3    => "黑痣与伤痕等 3",
            CustomizeIndex.FacialFeature4    => "黑痣与伤痕等 4",
            CustomizeIndex.FacialFeature5    => "黑痣与伤痕等 5",
            CustomizeIndex.FacialFeature6    => "黑痣与伤痕等 6",
            CustomizeIndex.FacialFeature7    => "黑痣与伤痕等 7",
            CustomizeIndex.LegacyTattoo      => "遗产纹身",
            CustomizeIndex.SmallIris         => "较小眼瞳",
            CustomizeIndex.Lipstick          => "启用唇色",
            CustomizeIndex.FacePaintReversed => "反转面妆",
            _                                => string.Empty,
        };
}
