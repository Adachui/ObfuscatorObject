using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyMobile;

public class MoreLayerView : AppAdvisoryHelper {
	public GameObject[] Menus;
	public Button[] TabButtons;
	public Image[] TabIcon;
	public GameObject[] objLock;
	// Use this for initialization
//	void Start () {
//	}

	void OnEnable () {
		Advertising.HideBannerAd ();
		canvasManager.isUILayer++;
		SetMenuActive (2);
		if (DataManager.getMaxBuildLv >= 10) {
			objLock [1].SetActive (false);
			TabIcon [1].GetComponent<Image> ().color = Color.white;
			TabButtons [1].enabled = true;
		} else {
			TabIcon [1].GetComponent<Image> ().color = Color.gray;
			TabButtons [1].enabled = false;
		}

		if (DataManager.getMaxBuildLv >= 8) {
			objLock [0].SetActive (false);
			TabIcon [0].GetComponent<Image> ().color = Color.white;
			TabButtons [0].enabled = true;
		} else {
			TabButtons [0].enabled = false;
			TabIcon [0].GetComponent<Image> ().color = Color.gray;
		}
	}
	// Update is called once per frame
//	void Update () {
//		
//	}

	public void SetMenuActive(int index) {
		for (int i = 0; i < Menus.Length; i++) {
			Menus [i].SetActive (i == index);
			TabButtons [i].transform.GetChild (0).gameObject.SetActive (i == index);
		}
	}

	void OnDisable()
	{
		if (!Advertising.IsAdRemoved()){
			Advertising.ShowBannerAd(BannerAdPosition.Bottom);
		}
		canvasManager.isUILayer--;
	}

	public void ShowLockWarning(int lv) {
		string warningFormat = ConfigFileMgr.GetContentByKey (EM_LocalizeConstants.Locaize_LockWarning);
		canvasManager.ShowWarning (string.Format (warningFormat,lv));
	}
		
}
