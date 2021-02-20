using System;

namespace AssemblyCSharp
{
	public class ModelBuilding
	{
		public ModelBuilding (string id, int lv, int status = 0)
		{
			this.id = id;
			this.lv = lv;
			this.status = status;
			// 从表读取属性赋值

		}
		/*
		 * 作用于建筑
		 */
		///<summary>id</summary>
		public string id { get; set; }

		///<summary>等级</summary>
		public int lv { get; set; }

		///<summary>名称</summary>
		public string name { get; set; }

		///<summary>经验</summary>
		public int exp { get; set; }

		///<summary>价格（金币）</summary>
		public float price_coin { get; set; }

		///<summary>价格（钻石）</summary>
		public int price_diamond { get; set; }

		///<summary>解锁建筑等级</summary>
		public int unlock_lv { get; set; }

		///<summary>建筑等级</summary>
		public int price_unit { get; set; }

		/// <summary>建筑状态（0 停 1 走）</summary>
		public int status { get; set; }

		/*
		 * 作用于生成角色
		 */
		///<summary>每圈时间</summary>
		public float  per_circle { get; set; }

		///<summary>每圈收益</summary>
		public float profit_per { get; set; }

		///<summary>收益单位</summary>
		public int profit_unit { get; set; }

		///<summary>收入</summary>
		public float percent_income { get; set; }

		///<summary>速度</summary>
		public float percent_speed { get; set; }
	}
}

