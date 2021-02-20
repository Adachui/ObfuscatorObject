using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGuideNpc :AppAdvisoryHelper {
	public Button npcBtn;
	public Button messageBtn;
	public bool isShowAnimation = true;
	// Use this for initialization
	void Start () {
		
	}
	void Awake() {
		npcBtn.enabled = !isShowAnimation;
		messageBtn.enabled = !isShowAnimation;
		transform.GetComponent<Animator>().SetBool("isShowAnimation", isShowAnimation);
	}
		
	// Update is called once per frame
	void Update () {
		
	}
	public void SetClickedUnEnable() {
		npcBtn.enabled = !isShowAnimation;
		messageBtn.enabled = !isShowAnimation;
	}

	public void SetClickedEnable() {
		npcBtn.enabled = true;
		messageBtn.enabled = true;
	}
		
	public void NpcClicked(int step) {
		switch (step) {
		case 0:
			guideLayer.npc[0].SetActive (false);
			guideLayer.npc[1].SetActive (true);
			break;
		case 1:
			Invoke ("GuideStep_1", 0.8f);
			transform.GetComponent<Animator>().SetBool("isDismiss", true);
			break;
		case 2:
			Invoke ("GuideStep_2", 0.8f);
			transform.GetComponent<Animator>().SetBool("isDismiss", true);
			break;
		case 3:
			guideLayer.npc[3].SetActive (false);
			guideLayer.npc[4].SetActive (true);
			break;
		case 4:
			Invoke ("GuideStep_3", 0.8f);
			transform.GetComponent<Animator> ().SetBool ("isDismiss", true);
			break;
		case 5:
			Invoke ("GuideStep_4", 0.8f);
			transform.GetComponent<Animator> ().SetBool ("isDismiss", true);
			break;
		}
		npcBtn.enabled = false;
		messageBtn.enabled = false;
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
