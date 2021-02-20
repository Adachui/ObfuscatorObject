//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
using System.Collections.Generic;

public static class ListConfig{

	static string[] MonsterName = {
		"1,2,3,1,2,3,1,2,3,1",
		"4,5,6,4,5,6,4,5,6,4",
		"7,8,9,7,8,9,7,8,9,7",
		"1,3,5,9,1,3,5,9,1,3",
		"2,4,6,7,2,4,6,7,2,4",
		"3,5,8,9,3,5,8,9,3,5",
	};

	static int[] Level = {5,10,15,20,25,30};

	public static int[] GetMonsterList(int curLv){
		int[] list = new int[10];
		int index = 0;
		for (int i = 0; i < Level.Length; i++) {
			if (curLv <= Level[i]) {
				index = i;
				break;
			}
		}
		int _i = 0;
		string name = MonsterName[index];
		if (name != "") {
			string[] nameList = name.Split (',');
			for (int i = 0; i < nameList.Length; i++) {
				int id = 1;
				int.TryParse(nameList[i], out id);
				list[_i] = id;
				_i++;
			}
		}
		return list;
	}
		
}
