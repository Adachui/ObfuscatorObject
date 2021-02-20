using System;

namespace Model
{
	public class ModelBlock
	{
		// 地块是否为空
		public bool isEmpty() {
			//if(building_id != 0) {
			//	return false;
			//}
			return true;
		}

		///<summary>地块</summary>
		public int id { get; set; }

		///<summary>建筑id</summary>
		public string building_id { get; set; }

		///<summary>建筑等级</summary>
		public int building_lv { get; set; }

		/// <summary>建筑状态（0 停 1 走）</summary>
		public int building_status { get; set; }

	}
}

