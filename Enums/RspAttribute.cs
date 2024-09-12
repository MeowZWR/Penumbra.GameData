namespace Penumbra.GameData.Enums;

/// <summary> All available racial scaling parameters. </summary>
public enum RspAttribute : byte
{
    MaleMinSize,
    MaleMaxSize,
    MaleMinTail,
    MaleMaxTail,
    FemaleMinSize,
    FemaleMaxSize,
    FemaleMinTail,
    FemaleMaxTail,
    BustMinX,
    BustMinY,
    BustMinZ,
    BustMaxX,
    BustMaxY,
    BustMaxZ,
    NumAttributes,
}

public static class RspAttributeExtensions
{
    /// <summary> For which gender a certain racial scaling parameter is available. </summary>
    public static Gender ToGender(this RspAttribute attribute)
        => attribute switch
        {
            RspAttribute.MaleMinSize   => Gender.Male,
            RspAttribute.MaleMaxSize   => Gender.Male,
            RspAttribute.MaleMinTail   => Gender.Male,
            RspAttribute.MaleMaxTail   => Gender.Male,
            RspAttribute.FemaleMinSize => Gender.Female,
            RspAttribute.FemaleMaxSize => Gender.Female,
            RspAttribute.FemaleMinTail => Gender.Female,
            RspAttribute.FemaleMaxTail => Gender.Female,
            RspAttribute.BustMinX      => Gender.Female,
            RspAttribute.BustMinY      => Gender.Female,
            RspAttribute.BustMinZ      => Gender.Female,
            RspAttribute.BustMaxX      => Gender.Female,
            RspAttribute.BustMaxY      => Gender.Female,
            RspAttribute.BustMaxZ      => Gender.Female,
            _                          => Gender.Unknown,
        };

    /// <summary> Human-readable names for all racial scaling parameters. </summary>
    public static string ToFullString(this RspAttribute attribute)
        => attribute switch
        {
            RspAttribute.MaleMinSize   => "男性身体最小尺寸",
            RspAttribute.MaleMaxSize   => "男性身体最大尺寸",
            RspAttribute.FemaleMinSize => "女性身体最小尺寸",
            RspAttribute.FemaleMaxSize => "女性身体最大尺寸",
            RspAttribute.BustMinX      => "胸围最小X轴",
            RspAttribute.BustMaxX      => "胸围最大X轴",
            RspAttribute.BustMinY      => "胸围最小Y轴",
            RspAttribute.BustMaxY      => "胸围最大Y轴",
            RspAttribute.BustMinZ      => "胸围最小Z轴",
            RspAttribute.BustMaxZ      => "胸围最大Z轴",
            RspAttribute.MaleMinTail   => "男性尾巴最小长度",
            RspAttribute.MaleMaxTail   => "男性尾巴最大长度",
            RspAttribute.FemaleMinTail => "女性尾巴最小长度",
            RspAttribute.FemaleMaxTail => "女性尾巴最大长度",
            _                          => throw new InvalidEnumArgumentException(),
        };
}
