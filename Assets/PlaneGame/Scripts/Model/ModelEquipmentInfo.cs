/// CarWaypoints v1.0 (赛车路标点插件) <summary>
/// 作者：阿升哥哥
/// 博客：http://www.cnblogs.com/shenggege
/// 联系方式：6087537@qq.com
/// 最后修改：2015/2/14 23:20
/// </summary>

using UnityEngine;

/// 装备信息类 <summary>
/// 装备信息类
/// </summary>
public class ModelEquipmentInfo
{
    //装备id
    public int EquipmentId { get; set; }
    //类型 （1 飞机 2 跑道）
    public int EquipmentType { get; set; }
    //是否拥有
    public bool IsHave { get; set; }
    //购买次数
    public int BuyNum { get; set; }
}
