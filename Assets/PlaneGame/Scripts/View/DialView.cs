using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using EasyMobile;

public class DialView : AppAdvisoryHelper
{
    public Image imgPiece;
    public Image imgHand;

    public int adsCount;
	public Text adsCountText;
    public Text price;
	public Text nextFreeTime;
	public Button adsBtn;
	public Button buyBtn;

	private long waitTime = 28800L;
//	private long waitTime = 20L; 
	private bool clock = false;
	private long adsTime = 0;
    private List<int> RateList = new List<int>();

    private int rotateAngle = 0;

    private int time = 3;
    private int _cyclesNum = 5; //旋转的整圈数
    private int _last = 0;

    private bool isRotate = false;
	public GameObject effectBtn;
	private List<int> showBannerTypes = new List<int>();
    private void Awake()
    {

        if (DataManager.DialConfigList.Count == 0)
        {
            DataManager.DialConfigList = GlobalKeyValue.GetXmlData("DialInfo");
        }

        rotateAngle = 0;
        _last = 0;
        isRotate = true;

        RateList.Clear();
        int rateNum = 0;
        RateList.Add(rateNum);
        foreach (Dictionary<string, string> dic in DataManager.DialConfigList)
        {
            int probability = (int)(float.Parse(dic["Rate"]) * 100);
            rateNum = rateNum + probability;
            RateList.Add(rateNum);
        }
        InitData();
		Init ();
    }

    // Use this for initialization
    void Start()
    {

    }
		
    public void InitData(){
        for (int i = 0; i < 6;i++){
            Transform iconFrom = imgPiece.transform.GetChild(i);
            Image icon = iconFrom.GetComponent<Image>();
            //icon.sprite = Resources.Load("Image/pic" + ManagerUserInfo.GetStringValueByKey("IncomeBuffId"), typeof(Sprite)) as Sprite;
            Text numText = iconFrom.GetChild(0).GetComponent<Text>();

            string type = DataManager.DialConfigList[i]["RewardType"];
            string rewardName = DataManager.GetRewardNameByType(type);
            int rewardTime = 0;
            int.TryParse(DataManager.DialConfigList[i]["Time"],out rewardTime);
            string rewardNum = DataManager.DialConfigList[i]["RewardNum"];
			//Debug.Log ("type ==" + type + "   " + "rewardNum = " + rewardNum + "   rewardName = " + rewardName);
            switch (type){
                case "4":
                    numText.text = rewardName + "X" + rewardNum;
                    break;
                case "1":
                    int hour = rewardTime / 3600;
                    if(hour >= 24){
                        int day = hour / 24;
                        hour = hour % 24;
                        if(hour > 0){
                            numText.text = "X"  + day + " day" + hour + rewardName;
                        }else{
                            numText.text = "X" + day + " day";
                        }
                    }else{
                        numText.text = "X" + hour + rewardName;
                    }
                    break;
                default:
                    if(rewardNum != "0") 
                        numText.text = "X" + rewardNum + rewardName;
                    else if(rewardTime > 0) 
                        numText.text = "X" + rewardTime + rewardName;
                        
                    break;
            }
        }
    }

    public void CloseCallBack()
    {

    }

    public void ShowRewardAdsCallBack()
    {
		soundManager.SoundUIClick ();
		// 次数
		if(adsCount > 0 && isRotate) {
			ShowRewardedAd ();
		}
    }

    public void BuyCallBack()
    {
		if (isRotate) {
			if(DataManager.diamond.IsBiggerThan(new BigNumber(5, 0))){
				soundManager.SoundPayCash ();
				canvasManager.UpdateDiamond("-", new BigNumber(5, 0));
				RunDial();


			}else{
				soundManager.SoundUIClick ();
				canvasManager.diamondStore.gameObject.SetActive(true);
			}
		}
    }

    private void RunDial()
    {
		if (isRotate)
        {
            isRotate = false;
            int num = UnityEngine.Random.Range(1, 100);
            int index = 1;
            for (int i = 0; i < RateList.Count; i++)
            {
                if (i + 1 < RateList.Count)
                {
                    if (num > RateList[i] && num <= RateList[i + 1])
                    {
                        index = i;
                        break;
                    }
                }

            }

            rotateAngle = _cyclesNum * 360 + index * 60;
            //Debug.Log("本次旋转的度数： " + (rotateAngle - _last) + "\r\n" + "本次随机的结果： " + index);
            //本次旋转的度数
            imgPiece.transform.DORotate(new Vector3(0, 0, rotateAngle - _last), time, RotateMode.FastBeyond360).SetEase(Ease.OutCubic).OnComplete(delegate
            {
                isRotate = true;
                //因为每次旋转后转盘没有归零，就必须减去上次的%360度数
                _last = (int)imgPiece.transform.localEulerAngles.x;

                GetReward(index);
            });
        }


    }

    private void GetReward(int index)
    {
		//index = 4;
        string type = DataManager.DialConfigList[index]["RewardType"];
        string rewardName = DataManager.GetRewardNameByType(type);
        int rewardTime = 0;
        int.TryParse(DataManager.DialConfigList[index]["Time"], out rewardTime);
        int rewardNum = 0;
        int.TryParse(DataManager.DialConfigList[index]["RewardNum"], out rewardNum);
//		Debug.Log ("rewardNum == " + rewardNum);
//		Debug.Log ("Time == " + rewardTime);
		Sprite[] icons = new Sprite[6];
		icons = Resources.LoadAll<Sprite>("Sprites/Congratulation");
        switch(type){
            //金币*时间道具 持续
			case "1":
				BigNumber income = new BigNumber (DataManager.incomeSec.number * rewardNum * 3.6, DataManager.incomeSec.units + 1);
				canvasManager.UpdateCoin ("+", income);
					if (rewardNum == 4) {
						canvasManager.ShowCongratulationViewWithIcon (
							icons [0],
							ConfigFileMgr.GetContentByKey(EM_LocalizeConstants.Locaize_FourHoursProfit),
							1.8f
						);
					} else {
						canvasManager.ShowCongratulationViewWithIcon (
							icons [1],
							ConfigFileMgr.GetContentByKey(EM_LocalizeConstants.Locaize_OneDayProfit),
							1.6f
						);
					}
		            break;
	        //金箱子
			case "2":
				AddToBannerType (3);
				//DataManager.boxNum += 4;
				int boxlv = DataManager.GetBuildBoxLv (1, DataManager.getMaxBuildLv);
				for(int i = 0; i < 4; i++) {
					DataManager.boxLevel.Add(boxlv);
				}
				DataManager.SetBoxList ();
//				ManagerUserInfo.SetDataByKey("BoxNum", DataManager.boxNum);
//				ManagerUserInfo.SetDataByKey("BoxLevel", DataManager.boxLevel);
				canvasManager.ShowCongratulationViewWithIcon (
					icons [2],
					ConfigFileMgr.GetContentByKey(EM_LocalizeConstants.Locaize_FourGiftBox),
					0.6f
				);
			    break;
	        //加速 持续
	        case "3":
				AddToBannerType(1);
				DataManager.buffSpeedTime += rewardNum;
				DataManager.buffSpeedTime = DataManager.buffSpeedTime >= DataManager.speedUpTimeLimit ? DataManager.speedUpTimeLimit : DataManager.buffSpeedTime;
				PlaneGameManager.isSpeedUp = true;
				ManagerUserInfo.SetDataByKey("SpeedBuffTime", DataManager.buffSpeedTime.ToString());
				canvasManager.ShowCongratulationViewWithIcon (
					icons [5],
					ConfigFileMgr.GetContentByKey(EM_LocalizeConstants.Locaize_SpeedUpX2),
					0.9f
				);        
				break;
	        //收益 持续
			case "4":
					AddToBannerType(2);
					//Debug.Log ("DataManager.buffIncomeTime " + DataManager.buffIncomeTime);
					if (DataManager.buffIncomeTime <= float.Epsilon) {
						DataManager.buffIncome += rewardNum;
					}
					DataManager.buffIncomeTime += rewardTime;
					ManagerUserInfo.SetDataByKey("BuffIncomeTime", DataManager.buffIncomeTime.ToString());
					PlaneGameManager.isIncomeUp = true;
					canvasManager.ShowCongratulationViewWithIcon (
						icons [4],
						ConfigFileMgr.GetContentByKey(EM_LocalizeConstants.Locaize_IncomePackDes),
						0.6f
					);        
	                break;
	            //钻石
	         case "5":
	         	canvasManager.UpdateDiamond("+", new BigNumber(rewardNum, 0));
				canvasManager.ShowCongratulationViewWithIcon (
					icons [3],
					ConfigFileMgr.GetContentByKey(EM_LocalizeConstants.Locaize_GetDiamond20),
					1.5f
				); 
	       		break;
        }
    }

	void AddToBannerType(int type) {
		for (int i = 0; i < showBannerTypes.Count; i++) {
			if (showBannerTypes [i] == type) {
				return;
			}
		}
		showBannerTypes.Add (type);
	}

	public void ShowRewardedAd()
	{
		if (isRotate && adsCount > 0) {
			canvasManager.ShowRewardAd ();
		}
	}

	public void LoadRewardedAd()
	{
		if (Advertising.IsAutoLoadDefaultAds())
		{
			// NativeUI.Alert("Alert", "autoLoadDefaultAds is currently enabled. Ads will be loaded automatically in background without you having to do anything.");
		}

		Advertising.LoadRewardedAd();
	}

	void Init() {
		clock = false;
		if (adsCount == 0) {
			buyBtn.transform.localPosition = new Vector3 (0, buyBtn.transform.localPosition.y, buyBtn.transform.localPosition.z);
			adsBtn.gameObject.SetActive (false);
			adsCountText.gameObject.SetActive (false);
		} else {
			adsCountText.gameObject.SetActive (true);
			buyBtn.transform.localPosition = new Vector3 (128, buyBtn.transform.localPosition.y, buyBtn.transform.localPosition.z);
			adsBtn.gameObject.SetActive (true);
		}
		adsCountText.text = "(" + adsCount.ToString() + "/3)";
		if (adsCount >= 3) {
			nextFreeTime.gameObject.SetActive (false);
		} else {
			nextFreeTime.gameObject.SetActive (true);
			clock = true;
		}
	}
		
	void Update()
	{
		if (canvasManager.isOtherLayerOpen) {
			if (effectBtn.activeSelf) {
				effectBtn.SetActive (false);
			}
		} else {
			if (!effectBtn.activeSelf) {
				effectBtn.SetActive (true);
			}
		}
		if (clock && adsCount < 3) {
			long now = DataManager.ConvertDateTimeToLong (DateTime.Now);
			adsTime = ManagerUserInfo.GetLongDataByKey ("DialAdsTime");
			long subTime = now - adsTime;
			if (subTime >= waitTime) {
				int count = (int)((subTime / waitTime));
				adsCount += count;
				adsCount = adsCount > 3 ? 3 : adsCount;
				adsTime += (count * waitTime);
				ManagerUserInfo.SetDataByKey ("DialAdsTime", adsTime.ToString ());
				ManagerUserInfo.SetDataByKey ("DialAdsCount", adsCount);

				nextFreeTime.text = Long2Time(adsTime + waitTime - now);
				adsCountText.text = "(" + adsCount.ToString() + "/3)";
				Init ();
			} else {
				nextFreeTime.text = Long2Time(adsTime + waitTime - now);
				adsCountText.text = "(" + adsCount.ToString() + "/3)";
			}
		}
	}

	string Long2Time(long t) {
		long h = t / 3600;
		long m = (t % 3600) / 60;
		long s = t % 60;

		String format = ConfigFileMgr.GetContentByKey (EM_LocalizeConstants.Locaize_SpinTip);
		return string.Format(format, h, m, s);
	}

	void OnEnable()
	{
		canvasManager.isOtherLayerOpen = true;
		adsCount = ManagerUserInfo.GetIntDataByKey ("DialAdsCount");
		Init ();
		showBannerTypes.Clear ();
		canvasManager.isUILayer++;
		LoadRewardedAd ();
		Advertising.RewardedAdSkipped += OnRewardedAdSkipped;
		Advertising.RewardedAdCompleted += OnRewardedAdCompleted;
	}
		

	void OnDisable()
	{
		canvasManager.isOtherLayerOpen = false;
		canvasManager.isUILayer--;
		Advertising.RewardedAdSkipped -= OnRewardedAdSkipped;
		Advertising.RewardedAdCompleted -= OnRewardedAdCompleted;
	}

	void OnRewardedAdCompleted(RewardedAdNetwork arg1, AdLocation arg2) {
		PlaneGameManager.isAd = false;
		if (DataManager.getMaxBuildLv >= 8) {
			planeGameManager.UpdateDailyTask ("1901");
		}
		//播放激励广告
		RunDial();

		// 次数
		if (adsCount >= 3) {
			ManagerUserInfo.SetDataByKey ("DialAdsTime", DataManager.ConvertDateTimeToLong(DateTime.Now).ToString());
			//Debug.Log (DataManager.ConvertDateTimeToLong (DateTime.Now));
		}
		adsCount--;
		adsCount = adsCount < 0 ? 0 : adsCount;
		ManagerUserInfo.SetDataByKey ("DialAdsCount", adsCount);
		Init ();
	}

	void OnRewardedAdSkipped(RewardedAdNetwork arg1, AdLocation arg2) {
		PlaneGameManager.isAd = false;
	}

	public void BtnCloseCallback() {
		if (isRotate) {
			canvasManager.ShowBuffGetBanner(showBannerTypes);
			canvasManager.coinTextLeft.gameObject.SetActive (false);
			this.gameObject.SetActive (false);
		}
	}
}
