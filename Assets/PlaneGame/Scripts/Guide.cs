using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guide : AppAdvisoryHelper {
	public GameObject[] npc;
	public GameObject guide;
	public GameObject maskUI;
	// Use this for initialization
	void Start () {
		
	}
	void Awake() {
		GuideStep_0 ();
	}
		
	// Update is called once per frame
	void Update () {
		
	}

	void OnDisable() {

	}

	///<summary>0 欢迎对话框</summary>
	public void GuideStep_0() {
		ManagerUserInfo.SetDataByKey ("NewPlayerStep1", 0);
		ManagerUserInfo.SetDataByKey ("NewPlayerStep2", 0);
		ManagerUserInfo.SetDataByKey ("NewPlayerStep3", 0);
		ManagerUserInfo.SetDataByKey ("NewPlayerStep4", 0);
		if (ManagerUserInfo.GetIntDataByKey ("NewPlayerStep0") == 0) {
			canvasManager.UpdateCoin ("+", new BigNumber (20000, 0));
		}
		npc[0].SetActive (true);
	}

	///<summary>1 购买</summary>
	public void GuideStep_1() {
		ManagerUserInfo.SetDataByKey ("NewPlayerStep0", 1);
		if (ManagerUserInfo.GetIntDataByKey ("NewPlayerStep1") == 0) {
			canvasManager.showBuild.GetComponent<Canvas> ().overrideSorting = true;
			canvasManager.showBuild.GetComponent<Canvas> ().sortingOrder = 2;
			guideLayer.guide.SetActive (true);
		} else {
			guideLayer.guide.SetActive (false);
			canvasManager.showBuild.GetComponent<Canvas> ().sortingOrder = 0;
			guideLayer.npc [2].SetActive (true);
		}
	}

	///<summary>2 合成</summary>
	public void GuideStep_2() {
		if (ManagerUserInfo.GetIntDataByKey ("NewPlayerStep2") == 0) {
			guideLayer.maskUI.SetActive (false);
			planeGameManager.guideTime = PlaneGameManager.guideTimeWait + 1f;
		} else {
			planeGameManager.isGuideStep = false;
			guideLayer.maskUI.SetActive (true);
			guideLayer.npc [3].SetActive (true);
		}
	}

	///<summary>3 放置</summary>
	public void GuideStep_3() {
		if (ManagerUserInfo.GetIntDataByKey ("NewPlayerStep3") == 0) {
			guideLayer.maskUI.SetActive (false);
			planeGameManager.guideTime = PlaneGameManager.guideTimeWait + 1f;
		} else {
			guideLayer.maskUI.SetActive (true);
			guideLayer.npc [5].SetActive (true);
		}
	}

	public void GuideStep_4() {
		if (ManagerUserInfo.GetIntDataByKey ("NewPlayerStep4") == 0) {
			ManagerUserInfo.SetDataByKey ("NewPlayerStep4", 1);
			ManagerUserInfo.SetDataByKey ("NewPlayer", 1);
			GuideDismiss ();
		} else {
			return;
		}
	}


	public void NpcDismiss() {
		for (int i = 0; i < guideLayer.npc.Length; i++) {
			guideLayer.npc [i].SetActive (false);
		}
		guideLayer.maskUI.SetActive (false);
	}

	public void GuideDismiss() {
		NpcDismiss ();
		gameObject.SetActive (false);
	}
}
