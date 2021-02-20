/// CarWaypoints v1.0 (赛车路标点插件) <summary>
/// 作者：阿升哥哥
/// 博客：http://www.cnblogs.com/shenggege
/// 联系方式：6087537@qq.com
/// 最后修改：2015/2/14 23:20
/// </summary>

using UnityEngine;

/// <summary>
/// 建筑表（建筑和人物除了模型，属性全部一样）（配置）
/// </summary>
public class ModelBuildFrom
{
    ///<summary>建筑id</summary>
    public string BuildId { get; set; }

    ///<summary>等级</summary>
    public int BuildLv { get; set; }

    /// <summary>速度</summary>
    public int Speed { get; set; }

    /// <summary>收益</summary>
    public int Income { get; set; }

    /// <summary>收益单位 0代表个位，2代表k千位，3代表M百亿，4代表B十亿，5代表T万亿 </summary>
    public int IncomeUnit { get; set; }

    /// <summary>合成经验</summary>
    public int ComposeExp { get; set; }

    /// <summary>金币价格</summary>
    public int CoinPrice { get; set; }

    /// <summary>价格单位 0代表个位，2代表k千位，3代表M百亿，4代表B十亿，5代表T万亿 </summary>
    public int CoinPriceUnit { get; set; }



    /// <summary>钻石价格</summary>
    public int DiamondPricee { get; set; }
}
