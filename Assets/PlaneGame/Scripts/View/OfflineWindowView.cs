using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyMobile;

public class OfflineWindowView : AppAdvisoryHelper {
	public Text incomeText;
	public Button getByAdButton;
	public Button getByDiamondButton;
	public GameObject DiamondStore;
	public GameObject MoreLayer;
	BigNumber income; //收入
	BigNumber price = new BigNumber(5, 0);
	private bool isGetOffline = false;


//	// Use this for initialization
//	void Start () {
//		
//	}
	
	// Update is called once per frame
//	void Update () {
//		// 购买后刷新
//		if(false) {
////			income.SetIntegerNumber (income.number * 1, income.units);
////			incomeText.text = income.ToString ();
//		}
//	}

	public void BtnAdCallBack(){
		//播放激励广告
		ShowRewardedAd();
	}

	public void BtnBuyCallBack()
	{
		if (DataManager.diamond.IsBiggerThan (price)){
			soundManager.SoundPayCash ();
			canvasManager.UpdateDiamond("-", price);
			GetReward (3);
			ShowInterstitialAd ();
		} else {
			DiamondStore.SetActive (true);
		}
	}


	public void ShowInterstitialAd()
	{
		if (Advertising.IsAdRemoved())
		{
			// NativeUI.Alert("Alert", "Ads were removed.");
			return;
		}
		if (Advertising.IsInterstitialAdReady()) {
			PlaneGameManager.isAd = true;
			Advertising.ShowInterstitialAd ();
			CileadTrace.RecordEvent (EventConst.SHOWINTERSTITIAL);
		}
	}

	public void GetReward(int coefficient) {
		isGetOffline = true;
		canvasManager.UpdateCoin ("+", new BigNumber (income.number * coefficient, income.units));
		//canvasManager.coinText.text = DataManager.coins.ToInternationalizationString ();
		gameObject.SetActive (false);
	}

	public void showVipBuyView() {
		MoreLayer.SetActive (true);
		MoreLayer.GetComponent<MoreLayerView> ().SetMenuActive (2);	
	}

	public void ShowRewardedAd()
	{
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
		canvasManager.isShowOfflinewView = true;
		isGetOffline = false;
		canvasManager.isUILayer++;

//		if (DataManager.isGoldVip == 1) {
//			income = new BigNumber (1 * DataManager.incomeSec.number * DataManager.offLineTimeSec * DataManager.buffIncome, DataManager.incomeSec.units);
//		} else {
//			income = new BigNumber (1.2 * DataManager.incomeSec.number * DataManager.offLineTimeSec * DataManager.buffIncome, DataManager.incomeSec.units);
//		}

		if (DataManager.isGoldVip == 1) {
			income = new BigNumber (0.36 * DataManager.incomeSec.number * DataManager.offLineTimeSec * DataManager.buffIncome, DataManager.incomeSec.units);
		} else {
			income = new BigNumber (0.3 * DataManager.incomeSec.number * DataManager.offLineTimeSec * DataManager.buffIncome, DataManager.incomeSec.units);
		}

		incomeText.text = income.ToString ();
		LoadRewardedAd ();
		Advertising.RewardedAdSkipped += OnRewardedAdSkipped;
		Advertising.RewardedAdCompleted += OnRewardedAdCompleted;
		DataManager.curOnLineTime = 0;
	}

	void OnDisable()
	{
		canvasManager.isShowOfflinewView = false;
		if (!isGetOffline) {
			GetReward (1);
		}

		canvasManager.isUILayer--;
		Advertising.RewardedAdSkipped -= OnRewardedAdSkipped;
		Advertising.RewardedAdCompleted -= OnRewardedAdCompleted;
	}

	void OnRewardedAdCompleted(RewardedAdNetwork arg1, AdLocation arg2) {
		PlaneGameManager.isAd = false;
		if (DataManager.getMaxBuildLv >= 8) {
			planeGameManager.UpdateDailyTask ("1901");
		}
		GetReward (2);
	}

	void OnRewardedAdSkipped(RewardedAdNetwork arg1, AdLocation arg2) {
		PlaneGameManager.isAd = false;
	}
}
