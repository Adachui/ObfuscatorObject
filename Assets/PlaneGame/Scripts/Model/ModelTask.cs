/// CarWaypoints v1.0 (赛车路标点插件) <summary>
/// 作者：阿升哥哥
/// 博客：http://www.cnblogs.com/shenggege
/// 联系方式：6087537@qq.com
/// 最后修改：2015/2/14 23:20
/// </summary>

using UnityEngine;

/// 个人成就类 <summary>
/// 个人成就类
/// </summary>
public class ModelTask
{
    //成就id
    public string TaskId { get; set; }
    //达到数量
    public int CurAchieveNum { get; set; }
    //所需数量
    public int NeedAchieveNum { get; set; }
    //所需建筑等级
    public int NeedBuildLv { get; set; }
    //是否领取
    public int IsGet { get; set; }
}
