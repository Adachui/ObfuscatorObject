using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyMobile;
using UnityEngine.UI;

public class CommonAdWindowView : AppAdvisoryHelper {
	public Text[] titles;
	public GameObject[] types;
	public Text price;
	public int type = 0;
	private BigNumber priceDiamond = new BigNumber(3, 0);
	// Use this for initialization
	private List<int> showBannerTypes = new List<int>();
	public GameObject effectBg;
	public GameObject effectBtn;
	void Start () {
		
	}

	public void SetData(int type) {
		this.type = type;
		for (int i = 0; i < types.Length; i++) {
			titles [i].gameObject.SetActive (i == type);
			types [i].SetActive (i == type);
		}
		price.text = priceDiamond.ToString ();
	}
	// Update is called once per frame
	void Update () {
		if (canvasManager.isOtherLayerOpen) {
			if (types [0].activeSelf) {
				types [0].SetActive (false);
			}
			if (types [1].activeSelf) {
				types [1].SetActive (false);
			}
			if (effectBg.activeSelf) {
				effectBg.SetActive (false);
			}
			if (effectBtn.activeSelf) {
				effectBtn.SetActive (false);
			}
		} else {
			if (!types [type].activeSelf) {
				types[type].SetActive (true);
			}
			if (!effectBg.activeSelf) {
				effectBg.SetActive (true);
			}
			if (!effectBtn.activeSelf) {
				effectBtn.SetActive (true);
			}
		}
	}

	public void BtnBuyCallBack() {
		if(DataManager.diamond.IsBiggerThan(priceDiamond)){
			soundManager.SoundPayCash ();
			canvasManager.UpdateDiamond("-", priceDiamond);
			StartCoroutine (GetReward ());
		}else{
			soundManager.SoundUIClick ();
			canvasManager.diamondStore.gameObject.SetActive(true);

		}
	}

	private IEnumerator GetReward() {
		switch (type) {
			case 0:
				if (!showBannerTypes.Contains (3)) {
					showBannerTypes.Add (3);
				}
				int boxlv = DataManager.GetBuildBoxLv (1, DataManager.getMaxBuildLv);
				for(int i = 0; i < 4; i++) {
					DataManager.boxLevel.Add(boxlv);
				}
				DataManager.SetBoxList ();
//				ManagerUserInfo.SetDataByKey("BoxNum", DataManager.boxNum);
//				ManagerUserInfo.SetDataByKey("BoxLevel", DataManager.boxLevel);
				break;
			case 1:
				if (!showBannerTypes.Contains (2)) {
					showBannerTypes.Add (2);
				}
				if (DataManager.buffIncomeTime <= float.Epsilon) {
					DataManager.buffIncome += 5;
				}
				DataManager.buffIncomeTime += 60;
				ManagerUserInfo.SetDataByKey("BuffIncomeTime", DataManager.buffIncomeTime.ToString());
				PlaneGameManager.isIncomeUp = true;
				break;
		}
		yield return new WaitForSeconds (0f);
		canvasManager.ShowBuffGetBanner(showBannerTypes);
		gameObject.SetActive (false);
	}

	public void ShowRewardedAd()
	{
		soundManager.SoundUIClick ();
		canvasManager.ShowRewardAd ();
	}

	public void LoadRewardedAd()
	{
		if (Advertising.IsAutoLoadDefaultAds())
		{
			// NativeUI.Alert("Alert", "autoLoadDefaultAds is currently enabled. Ads will be loaded automatically in background without you having to do anything.");
		}

		Advertising.LoadRewardedAd();
	}

	void OnEnable()
	{
		canvasManager.isUILayer++;
		LoadRewardedAd ();
		showBannerTypes.Clear ();
		LoadRewardedAd ();
		Advertising.RewardedAdSkipped += OnRewardedAdSkipped;
		Advertising.RewardedAdCompleted += OnRewardedAdCompleted;
	}



	void OnDisable()
	{
		canvasManager.isUILayer--;
		Advertising.RewardedAdSkipped -= OnRewardedAdSkipped;
		Advertising.RewardedAdCompleted -= OnRewardedAdCompleted;
	}

	void OnRewardedAdCompleted(RewardedAdNetwork arg1, AdLocation arg2) {
		//播放激励广告
		PlaneGameManager.isAd = false;
		if (DataManager.getMaxBuildLv >= 8) {
			planeGameManager.UpdateDailyTask ("1901");
		}
		StartCoroutine (GetReward ());
	}

	void OnRewardedAdSkipped(RewardedAdNetwork arg1, AdLocation arg2) {
		PlaneGameManager.isAd = false;
	}

	public void BtnCloseCallback() {
		canvasManager.ShowBuffGetBanner(showBannerTypes);
	}
}
