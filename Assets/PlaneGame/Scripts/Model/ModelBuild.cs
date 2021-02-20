/// CarWaypoints v1.0 (赛车路标点插件) <summary>
/// 作者：阿升哥哥
/// 博客：http://www.cnblogs.com/shenggege
/// 联系方式：6087537@qq.com
/// 最后修改：2015/2/14 23:20
/// </summary>

using UnityEngine;

/// <summary>
/// 建筑信息
/// </summary>
public class ModelBuild: IModel
{
    ///<summary>建筑lv</summary>
    public int BuildLv { get; set; }

    ///<summary>建筑id</summary>
    public string BuildId { get; set; }

    ///<summary>购买次数</summary>
    public int BuyNum { get; set; }

    /// <summary>历史拥有次数</summary>
    public int HistoryHaveNum { get; set; }
}
