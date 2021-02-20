/// CarWaypoints v1.0 (赛车路标点插件) <summary>
/// 作者：阿升哥哥
/// 博客：http://www.cnblogs.com/shenggege
/// 联系方式：6087537@qq.com
/// 最后修改：2015/2/14 23:20
/// </summary>

using UnityEngine;

/// 地块类 <summary>
/// 地块类
/// </summary>
public class ModelBlock
{
    ///<summary>地块id</summary>
    public int BlockId { get; set; }

    ///<summary>建筑id</summary>
    public string BuildId { get; set; }

    ///<summary>建筑等级</summary>
    public int BuildLv { get; set; }

    /// <summary>建筑状态（0 停 1 走）</summary>
    public int BuildState { get; set; }

	/// <summary>炮台id</summary>
	public int DefendBlockId { get; set; }
}
