using UnityEngine;

/// <summary>
/// buff信息
/// </summary>
public class ModelBuff: IModel
{
    ///<summary>buffid</summary>
    public int BuffId { get; set; }

    ///<summary>buffLv</summary>
    public int BuffLv { get; set; }

    ///<summary>是否购买（0 解锁 1 已拥有 2 未解锁） </summary>
    public int BuffState { get; set; }

    ///<summary>buff类型（1 收益 2 降价）</summary>
    public int BuffType { get; set; }

}
