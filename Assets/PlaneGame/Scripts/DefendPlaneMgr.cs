using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendPlaneMgr : AppAdvisoryHelper {

	// 打中目标时金币获取提示
	public Transform coinGet;
	public List<GameObject> defendBlockList = new List<GameObject> ();
	public int blockNum = 12;

	//防守地块初始化
	public void DefendListInit () {
		int length = transform.childCount;
		for (int i = 0; i < blockNum; i++) {
			if (i >= DataManager.roleMaxNum) {
				transform.GetChild (i).GetComponent<DefendBlock> ().isLock = true;
				//transform.GetChild (i).gameObject.SetActive (false);
			}
			defendBlockList.Add (transform.GetChild(i).gameObject);
		}
	}

	//根据id获取防守地块
	public Transform GetDefendBlockById(int id){
		if (id > 0) {
			return defendBlockList [id - 1].transform;
		}
		return null;
	}

	//刷新防守地块
	public void RefreshDefendList(){
		planeGameManager.RefreshDefenseNum ();
		int length = defendBlockList.Count;
		for (int i = 0; i < blockNum; i++) {
			if (i >= DataManager.roleMaxNum) {
				defendBlockList [i].transform.GetComponent<DefendBlock> ().isLock = true;
				//defendBlockList[i].SetActive (false);
			}else{
				defendBlockList [i].transform.GetComponent<DefendBlock> ().isLock = false;
				//defendBlockList[i].SetActive (true);
			}
		}
	}

	//获取空着的可用防守地块
	public Transform GetEmptyDefendBlock(){
		int length = defendBlockList.Count;
		for (int i = 0; i < length; i++) {
			if (!defendBlockList [i].transform.GetComponent<DefendBlock> ().isLock) {
				Transform role = defendBlockList [i].transform.Find ("DefendRole(Clone)");
				if (role == null) {
					return defendBlockList [i].transform;
				}
			}
		}
		return null;
	}
}
