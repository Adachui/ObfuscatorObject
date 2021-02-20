using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyMobile;

public class SpeedUpWindowView: AppAdvisoryHelper 
{
	public GameObject DiamondStore;

	public Text title;
    public Text time;
    public Text explain;
    public Text price;
	public Image progress;

	public BigNumber priceNumber = new BigNumber(3, 0);
	private List<int> showBannerTypes = new List<int>();
	public GameObject effectBtn;

    public void InitInfo(){
        title.text = "";
        time.text = "";
        explain.text = "";
        price.text = "3";


    }

	public void Update() {
		if (canvasManager.isOtherLayerOpen) {
			if (effectBtn.activeSelf) {
				effectBtn.SetActive (false);
			}
		} else {
			if (!effectBtn.activeSelf) {
				effectBtn.SetActive (true);
			}
		}
		progress.fillAmount = DataManager.buffSpeedTime / DataManager.speedUpTimeLimit;
		//Debug.Log (DataManager.buffSpeedTime / 60 + ":" + DataManager.buffSpeedTime % 60);
		time.text = string.Format ("{0:D2} : {1:D2}", (int)DataManager.buffSpeedTime / 60, (int)DataManager.buffSpeedTime % 60);
	}

    public void BtnFreeCallBack(){
		soundManager.SoundUIClick ();
        //播放激励广告
		ShowRewardedAd();
    }

    public void BtnBuyCallBack()
	{
		if (DataManager.buffSpeedTime <= (DataManager.speedUpTimeLimit - 90)) {
			if (DataManager.diamond.IsBiggerThan (priceNumber)) {
				soundManager.SoundPayCash ();
				canvasManager.UpdateDiamond ("-", priceNumber);
				GetReward ();
			} else {
				soundManager.SoundUIClick ();
				DiamondStore.SetActive (true);
			}
		} else {
			soundManager.SoundUIClick ();
			canvasManager.ShowWarning (ConfigFileMgr.GetContentByKey (EM_LocalizeConstants.Locaize_SpeedTimeLimit));
		}
    }

	public void GetReward() {
		soundManager.SoundRewardClaim ();
		DataManager.buffSpeedTime += 150f;
		DataManager.buffSpeedTime = DataManager.buffSpeedTime >= DataManager.speedUpTimeLimit ? DataManager.speedUpTimeLimit : DataManager.buffSpeedTime;
		PlaneGameManager.isSpeedUp = true;
		ManagerUserInfo.SetDataByKey("SpeedBuffTime", DataManager.buffSpeedTime.ToString());
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
		LoadRewardedAd ();
		canvasManager.isUILayer++;
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
		PlaneGameManager.isAd = false;
		if (DataManager.getMaxBuildLv >= 8) {
			planeGameManager.UpdateDailyTask ("1901");
		}
		GetReward ();
	}

	void OnRewardedAdSkipped(RewardedAdNetwork arg1, AdLocation arg2) {
		PlaneGameManager.isAd = false;
	}

	public void BtnCloseCallback() {
		canvasManager.ShowBuffGetBanner(showBannerTypes);
	}
}
