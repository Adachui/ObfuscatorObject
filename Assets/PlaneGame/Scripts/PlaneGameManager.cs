using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyMobile;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;


public class PlaneGameManager : AppAdvisoryHelper
{
	
	public static bool isBack = false; //后台
	// 控制加速
	public static bool isSpeedUp = false;
	// 收益增加
	public static bool isIncomeUp = false;
	public float timeSaveInterval = 0;

	public static float lastOnlineTime = 0;
	// 引导
	public float guideTime = 0; // 引导计时
	public GameObject guide;
	public bool isGuideShow = false;
	public ParticleSystem guideClickedEffect;
	private Vector3 guideVector;
	private Vector3 startPos = new Vector3(-17, 15, 2-20);
	private Vector3 targetPos = new Vector3(-17, 15, 2-20);
	private bool guideStart = false;

	public GameObject ballon;
	public static float guideTimeWait = 60f;
	private float repostTime = 0f;
	// 跑道标示
	public List<GameObject> paths = new List<GameObject>();

	public  static bool isAd =false;
	private bool isVipPast = false;
	private bool isConnected = false;
	private float airDropBoxByRandomTime = 0f;
	private bool GAMESTART = false;
	public bool isGuideStep = false;

	public float guidePosY = 4f;

	private void OnApplicationPause(bool focus)
	{
		if (focus && !isAd) {
			if (canvasManager.isShowOfflinewView) {
				canvasManager.OfflineWindow.SetActive (false);
			}
			isBack = true;
		} else if(!isAd) {
			//			Debug.Log ("OnApplicationPause");
			canvasManager.ShowOffLineWindow();
			DataManager.curOnLineTime = 0f;
			isBack = false;
		}

		ManagerUserInfo.SaveData ();
	}
		
	public void OnApplicationQuit()
	{
		ManagerUserInfo.SaveData ();
	}

	private void Awake()
	{
		CileadTrace.RecordEvent (EventConst.MAINSCENE);
		//StartCoroutine(checkInternetConnection());
		Init ();
	}

	void OnGUI(){
		Event e=Event.current;
		if(e.isMouse&&(e.clickCount==2))
			Debug.Log("用户双击了鼠标PlaneGameManager");          
	}

	public void Init() {
//		PlayerPrefs.DeleteAll ();
//		ManagerUserInfo.SetDataByKey("GetMaxBuildLv", 40);
//		ManagerUserInfo.SetDataByKey("Level", 30);
		ManagerUserInfo.SetDataByKey("Diamond", "9999");
		ManagerUserInfo.SetDataByKey("DiamondUnit", 0);
		ManagerUserInfo.SaveData ();
//		Advertising.RemoveAds ();

		guideTime = 0f;
		lastOnlineTime = PlayerPrefs.GetFloat("ONLINETIME", 0);


		//		if (InAppPurchasing.isSubscribedOwned ()) {
		//			NativeUI.Alert ("isSubscribedOwned", "YES");
		//		} else {
		//			NativeUI.Alert ("isSubscribedOwned", "NO");
		//		}

		//		ManagerUserInfo.SetDataByKey("Name", PlayGamesPlatform.Instance.localUser.userName);
		#if UNITY_EDITOR
		ManagerUserInfo.SetDataByKey ("Name", "Custom");
		#else
		if (Social.localUser.authenticated)
		ManagerUserInfo.SetDataByKey ("Name", Social.localUser.userName);
		else {
		Social.localUser.Authenticate ((bAuthen) => {
		if (bAuthen)
		ManagerUserInfo.SetDataByKey ("Name", Social.localUser.userName);
		else
		ManagerUserInfo.SetDataByKey ("Name", "Custom");
		});
		}
		#endif


		//			ManagerUserInfo.SetDataByKey("Name", PlayGamesPlatform.Instance.localUser.userName);

		DataManager.InitXmlData();

		canvasManager.InitData();

		defendPlaneMgr.DefendListInit ();

		// 恢复为过期状态
		GoldVipOverdue ();
		ShowBannerAd ();
		InitDailyTask ();
		//Debug.Log ("GOLD = " + DataManager.isGoldVip);
		// 离线奖励
		canvasManager.ShowOffLineWindow();
		// 收益赋值
		SetBuffIncomeText();

		if (DataManager.buffIncomeTime > 0f) {
			isIncomeUp = true;
			DataManager.buffIncome += 5f;
		}

		if (DataManager.buffSpeedTime > 0f) {
			isSpeedUp = true;
		}

		RefreshDefenseNum ();
		InitBlock();


		GoldVipRestore ();
		canvasManager.ShowOffLineWindow();
		// 热气球
		InitBallon ();

		GamePlayGuide ();
		GAMESTART = true;
	}

	// 新手引导
	public void GamePlayGuide() {
		int isGuide = ManagerUserInfo.GetIntDataByKey ("NewPlayer");
		if (isGuide == 0) {
			isGuideStep = true;
			canvasManager.GuideLayer.SetActive (true);
			//			ManagerUserInfo.SetDataByKey ("NewPlayer", 1);
			//			canvasManager.UpdateCoin ("+", new BigNumber (20000, 0));
			//			guideTime = guideTimeWait + 1f;
		} else {
			isGuideStep = false;
			return;
		}
	}

	//跑道箭头指示 废弃 留着
	public void InitPathView() {
		/*for(int i = 0; i < DataManager.roleMaxNum; i++) {
			GameObject path =  Resources.Load ("Prefabs/UI/path") as GameObject;
			path = Instantiate (path);
			path.transform.parent = canvasManager.pathCapacity.transform;
			path.transform.localPosition = new Vector3 (0, 1f,  i * 0.4f);
			path.transform.localScale = new Vector3 (0.3f, 0.3f, 0.3f);
			path.transform.localRotation = Quaternion.Euler(90f, 180f,0.0f);
			paths.Add (path);
		}
		canvasManager.pathCapacity.transform.localPosition = new Vector3 (
			-3.73f, -0.8f, -DataManager.roleMaxNum * 0.2f
		);*/
	}

	//刷新防御台数量
	public void RefreshDefenseNum(){
		canvasManager.defenseText.text = ("Defense: " + DataManager.roleNum + " / " + DataManager.roleMaxNum);
	}


	// 刷新每日任务
	public void InitDailyTask() {

		long lastLoginTime = ManagerUserInfo.GetLongDataByKey("LoginTime");

		DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
		DateTime lastDate =  dtStart.AddSeconds(lastLoginTime);
		DateTime curDate =  DateTime.Now;

		// 比较
		bool isNewDay = false;
		if (curDate.Year > lastDate.Year) {
			isNewDay = true;
		} else if(curDate.Year == lastDate.Year && curDate.Month > lastDate.Month){
			isNewDay = true;
		} else if(curDate.Year == lastDate.Year && curDate.Month == lastDate.Month && curDate.Day > lastDate.Day){
			isNewDay = true;
		}

		if (isNewDay) {
			PlayerPrefs.SetInt ("IsGetDailyVipReward", 0);
			foreach(ModelTask taskInfo in DataManager.taskInfoList) {
				if (taskInfo.TaskId.Equals("1900") || taskInfo.TaskId.Equals("1901")) {
					taskInfo.CurAchieveNum = 0;
					taskInfo.IsGet = 0;
					ManagerTask.SetTaskXmlData (taskInfo);
				}
			}
		}

		// 刷新离线收益次数
		if (isNewDay){
			List<Dictionary<string, string>> OfflineConfigList = GlobalKeyValue.GetXmlData("OfflineInfo");

			for (int i = OfflineConfigList.Count - 1;i >= 0;i--)
			{
				Dictionary<string, string> dic = OfflineConfigList[i];
				int Multiple = int.Parse(dic["Multiple"]);
				ManagerUserInfo.SetDataByKey ("offlineTimes" + Multiple, 0);
			}
		}

		if (isNewDay && DataManager.isGoldVip == 1 && InAppPurchasing.isSubscribedOwned()) {
			GetDailyVipReward ();
		}

		canvasManager.newTipObj.SetActive (false);
		canvasManager.questsNewObj.SetActive (false);
		if (DataManager.getMaxBuildLv >= 8) {
			foreach(ModelTask taskInfo in DataManager.taskInfoList) {
				if(taskInfo.IsGet == 0 && taskInfo.CurAchieveNum >= taskInfo.NeedAchieveNum) {
					canvasManager.questsNewObj.SetActive (true);
					canvasManager.newTipObj.SetActive (true);
					break;
				}
			}
		}
	}

	public void InitBallon() {
		if (DataManager.lv < 10) {
			//DataManager.ballonShowInterval = Mathf.Pow (1.1f, (float)(DataManager.lv - 1)) * 3.0f * 60;
			DataManager.ballonShowInterval = Mathf.Pow (1.1f, (float)(DataManager.lv - 1)) * 1.0f * 30;
		} else if (DataManager.lv > 20) {
			DataManager.ballonShowInterval = Mathf.Pow (1.1f, 19f) * 1.0f * 60;
		} else {
			DataManager.ballonShowInterval = Mathf.Pow (1.1f, (float)(DataManager.lv - 1)) * 1.0f * 60;
		}
		//DataManager.ballonShowInterval = 10f;
		StartCoroutine (ShowBallon());
	}

	public void SetBuffIncomeText() {
		string formatText = ConfigFileMgr.GetContentByKey (EM_LocalizeConstants.Locaize_Income,canvasManager.buffIncomeText);
		canvasManager.buffIncomeText.text = string.Format(formatText, DataManager.buffIncome.ToString());
	}

	public void ShowBannerAd()
	{
		if (Advertising.IsAdRemoved())
		{
			// NativeUI.Alert("Alert", "Ads were removed.");
			return;
		}
		Advertising.ShowBannerAd(BannerAdPosition.Bottom);
	}

	//初始化地块
	private void InitBlock(){
		List<ModelBlock> blocksInfo = ManagerBlock.GetBlockXmlData ();

		if (isVipPast) {
			for (int i = blocksInfo.Count - 1; i > blocksInfo.Count - 3; i--) {
				if (blocksInfo [i].BuildLv > 0) {
					DataManager.boxLevel.Add (blocksInfo [i].BuildLv);
					Dictionary<string, string>  buildConfigInfo = GlobalKeyValue.GetXmlDataByKey("BuildInfo", "Build" + blocksInfo [i].BuildLv);
					if (blocksInfo [i].BuildState == 1) {
						double price = canvasManager.GetDoubleDataByKey(buildConfigInfo, "ProfitSec");
						int unit = canvasManager.GetIntDataByKey(buildConfigInfo, "Units");
						BigNumber iSec = new BigNumber(price, unit);
						canvasManager.UpdateOther("-", iSec);
					}
				}
				blocksInfo [i].BuildLv = -1;
				blocksInfo [i].BuildId = "-1";
				blocksInfo [i].BuildState = -1;
				ManagerBlock.SetBlockXmlData (blocksInfo [i]);
			}
		}

		//根据读取的数据判断游戏状态
		blocksManager.InitBlcoks(blocksInfo);

	}

	//初始化建筑购买信息
	private  void InitBuildBuyInfo(){

	}

	// Update is called once per frame
	void Update () {

		if (GAMESTART) {
			guideTime += Time.deltaTime;
			// 游戏引导
			if (guideTime >= guideTimeWait && !guideStart) {
				ShowGuideAnimation ();
			} else if(guideTime < guideTimeWait && !guideStart) {
				guideStart = false;
				guide.SetActive (false);
				//Debug.Log ("StopGuideAnimation");
				isGuideShow = false;
			}
			if (isGuideShow) {
				if(DataManager.roleNum < DataManager.roleMaxNum && !isGuideStep) {
					Transform emptyDefendBlock = defendPlaneMgr.GetEmptyDefendBlock ();
					if (emptyDefendBlock != null) {
						startPos = emptyDefendBlock.localPosition;//guide.transform.parent.InverseTransformPoint(emptyDefendBlock.position);
						startPos.y = guidePosY;
						startPos.x = startPos.x + 0.5f;
						startPos.z = startPos.z - 1f;
					}
					guide.transform.localPosition = Vector3.MoveTowards (guide.transform.localPosition, startPos, 40 * Time.deltaTime);
					if (guide.transform.localPosition.Equals (startPos)) {
						StartCoroutine (GuideReturen ());
					}
				} else {
					guide.transform.localPosition = Vector3.MoveTowards (guide.transform.localPosition, targetPos, 40 * Time.deltaTime);
					if (guide.transform.localPosition.Equals (targetPos)) {
						StartCoroutine (GuideReturen ());
					}
				}

			}

			// 宝箱掉落
			if(DataManager.boxLevel.Count > 0 && canvasManager.isUILayer <= 0) {
				GetGiftBox ();
			}
			// 时间计算

			// 收益
			if (DataManager.buffIncomeTime > float.Epsilon && isIncomeUp) {
				DataManager.buffIncomeTime -= Time.deltaTime;
				if (!canvasManager.goldRainEffect.isPlaying) {
					canvasManager.goldRainEffect.Play ();
				}
				isIncomeUp = true;
			} else if(isIncomeUp){
				DataManager.buffIncomeTime = 0f;
				ManagerUserInfo.SetDataByKey ("BuffIncomeTime", DataManager.buffIncomeTime.ToString ());
				isIncomeUp = false;
				DataManager.buffIncome -= 5.0f;
				canvasManager.goldRainEffect.Stop ();
			}

			// 加速
			if (DataManager.buffSpeedTime > float.Epsilon && isSpeedUp) {
				SoundManager.SoundBGM ();
				canvasManager.buffSpeedIcon.SetActive (true);
				DataManager.buffSpeedTime -= Time.deltaTime;
				DataManager.buffSpeed = 2f * ManagerUserInfo.GetFloatDataByKey("SpeedBuffNum");
			} else if(isSpeedUp){

				DataManager.buffSpeedTime = 0f;
				ManagerUserInfo.SetDataByKey ("SpeedBuffTime", DataManager.buffSpeedTime.ToString ());
				isSpeedUp = false;
				SoundManager.SoundBGM ();
				DataManager.buffSpeed = ManagerUserInfo.GetFloatDataByKey("SpeedBuffNum");
			}

			if (DataManager.buffSpeed > 1) {
				canvasManager.buffSpeedIcon.SetActive (true);
				string formatText = ConfigFileMgr.GetContentByKey (EM_LocalizeConstants.Locaize_BuffSpeed,canvasManager.buffIncomeText);
				string incomeFormatText = ConfigFileMgr.GetContentByKey (EM_LocalizeConstants.Locaize_Income,canvasManager.buffSpeedText);
				canvasManager.buffIncomeText.text = string.Format(incomeFormatText, DataManager.buffIncome.ToString());
				canvasManager.buffSpeedText.text = string.Format (formatText, DataManager.buffSpeed.ToString());
			} else {
				canvasManager.buffSpeedIcon.SetActive (false);
			}

			// 存储间隔
			timeSaveInterval += Time.deltaTime;
			if ((int)timeSaveInterval >= 1) {
				ManagerUserInfo.SetDataByKey ("BuffIncomeTime", DataManager.buffIncomeTime);
				ManagerUserInfo.SetDataByKey ("SpeedBuffTime", DataManager.buffSpeedTime);
				//Debug.Log (string.Format ("{0} -- {1}", DataManager.buffSpeedTime, DataManager.buffIncomeTime));
				timeSaveInterval = 0;
			}

			if(DataManager.incomeSec == null)
				DataManager.incomeSec = new BigNumber (ManagerUserInfo.GetDoubleDataByKey("IncomeSecond"), ManagerUserInfo.GetIntDataByKey("IncomeSecondUnit"));
			// 每秒收益
			BigNumber incomeText = new BigNumber (DataManager.incomeSec.number * DataManager.buffIncome * DataManager.buffSpeed, DataManager.incomeSec.units);
//			canvasManager.incomeSecText.text =  incomeText.ToString() + "/sec";
			canvasManager.UpdateIncomeSecText (incomeText.ToString());

			// buff数据
			SetBuffIncomeText();

			repostTime += Time.deltaTime;
			if (repostTime >= 120f) {
				ReportScore ();
				repostTime = 0f;
			}


			if(!canvasManager.isOnShop && !isGuideStep) {
				airDropBoxByRandomTime += Time.deltaTime;
				if (airDropBoxByRandomTime >= 30f) {
					blocksManager.CreateGiftBox (DataManager.GetBuildBoxLv (0, DataManager.getMaxBuildLv), "box_normal");
					airDropBoxByRandomTime = 0f;
				}
			}
		}

	}

	void OnDestory(){

	}

	IEnumerator ShowBallon () {
		while (true) {
			yield return new WaitForSeconds(DataManager.ballonShowInterval);

			ballon.GetComponent<Animator>().SetBool("isShow", true);
			int random = UnityEngine.Random.Range(1, 100);
			if(random % 2 == 0) {
				ballon.GetComponent<Animator>().SetBool("isLeft", true);
			} else {
				ballon.GetComponent<Animator>().SetBool("isRight", true);
			}
		}
	}

	public void StopBallon () {
		ballon.GetComponent<Animator>().SetBool("isShow", false);
		ballon.GetComponent<Animator>().SetBool("isLeft", false);
		ballon.GetComponent<Animator>().SetBool("isRight", false);
	}

	// 宝箱
	public void GetGiftBox() {
		blocksManager.CreateGiftBox (DataManager.boxLevel[0]);
	}



	// 刷新等级信息
	public void RefreshLevelData(int exp, bool queue = false) {
		bool newBlock = false;
		int newPath = DataManager.roleMaxNum;

		DataManager.exp += exp;
		ManagerUserInfo.SetDataByKey("Exp", DataManager.exp);
		float expProgress = float.Parse( DataManager.exp.ToString() ) / canvasManager.GetIntDataByKey(DataManager.userConfigDic, "Exp");
		DataManager.allExp += exp;
		ManagerUserInfo.SetDataByKey("AllExp", DataManager.allExp);

		if (DataManager.lv < ManagerUserInfo.MaxLevel && expProgress >= 1) {
			DataManager.lv += 1;
			if (DataManager.lv >= 5) {
				CileadTrace.RecordEvent (EventConst.USERLEVEL + DataManager.lv.ToString (),true);
			}

			if(DataManager.lv > ManagerUserInfo.MaxLevel){
				DataManager.lv = ManagerUserInfo.MaxLevel;
			}

			canvasManager.playerLvText.text = DataManager.lv.ToString ();

			ManagerUserInfo.SetDataByKey ("Level", DataManager.lv);
			DataManager.exp -= canvasManager.GetIntDataByKey (DataManager.userConfigDic, "Exp");
			ManagerUserInfo.SetDataByKey ("Exp", DataManager.exp);

			// 地块数量
			int curBlockNum = int.Parse (DataManager.userConfigDic ["MaxBuild"]);
			int nextBlockNum = int.Parse (DataManager.nextUserConfigDic ["MaxBuild"]);
			if (nextBlockNum > curBlockNum) {
				newBlock = true;
				int curIndex = DataManager.maxBlockNum;
				if (DataManager.isGoldVip <= 0 && DataManager.maxBlockNum > 4)
					curIndex--;
				for (int i = 0; i < nextBlockNum - curBlockNum; i++) {
					blocksManager.CreateBlock (curIndex,2);
				}
			}
				
			DataManager.userConfigDic = GlobalKeyValue.GetXmlDataByKey ("UserInfo", "User" + DataManager.lv);
			int nextLv = DataManager.lv + 1;
			if(nextLv > ManagerUserInfo.MaxLevel){
				nextLv = ManagerUserInfo.MaxLevel;
			}
			DataManager.nextUserConfigDic = GlobalKeyValue.GetXmlDataByKey ("UserInfo", "User" + nextLv);

			expProgress = float.Parse (DataManager.exp.ToString ()) / float.Parse (canvasManager.GetIntDataByKey (DataManager.userConfigDic, "Exp").ToString ());

			// 跑道数据
			DataManager.roleMaxNum = GlobalKeyValue.GetIntDataByKey (DataManager.userConfigDic, "MaxBuildPlay");
			DataManager.roleMaxNum = DataManager.isGoldVip > 0 ? DataManager.roleMaxNum + 2 : DataManager.roleMaxNum;
			defendPlaneMgr.RefreshDefendList ();

			if (!queue) {
				canvasManager.lvUpView.SetActive (true);
				canvasManager.lvUpView.transform.GetComponent<LevelUpView> ().InitView (
					DataManager.lv, newBlock, (DataManager.roleMaxNum - newPath > 0), false);
			} else {
				canvasManager.buildingUnlock.GetComponent<BuildingUnlockVIew> ().InitLvUp (
					DataManager.lv, newBlock, (DataManager.roleMaxNum - newPath > 0));
			}
			//跑道箭头指示 废弃 留着
//			if (DataManager.roleMaxNum - newPath > 0) {
//				GameObject path = Resources.Load ("Prefabs/UI/path") as GameObject;
//				path = Instantiate (path);
//				path.transform.parent = canvasManager.pathCapacity.transform;
//				path.transform.localPosition = new Vector3 (0, 1f, (DataManager.roleMaxNum - 1) * 0.4f);
//				path.transform.localScale = new Vector3 (0.3f, 0.3f, 0.3f);
//				path.transform.localRotation = Quaternion.Euler (90f, 180f, 0.0f);
//				canvasManager.pathCapacity.transform.localPosition = new Vector3 (
//					-3.75f, -0.8f, -DataManager.roleMaxNum * 0.2f
//				);
//				paths.Add (path);
//				canvasManager.defenseText.text = (DataManager.roleNum + " / " + DataManager.roleMaxNum);
//			}
		} 
		canvasManager.expImg.fillAmount = expProgress;
	}


	void OnEnable()
	{
		InAppPurchasing.PurchaseCompleted += OnPurchaseCompleted;
		InAppPurchasing.PurchaseFailed += OnPurchaseFailed;
		InAppPurchasing.RestoreCompleted += OnRestoreCompleted;
		InAppPurchasing.RestoreFailed += OnRestoreFailed;
		GameServices.UserLoginSucceeded += OnUserLoginSucceeded;
		GameServices.UserLoginFailed += OnUserLoginFailed;
		Advertising.InterstitialAdCompleted += OnInterstitialAdCompleted; 
	}

	void OnDisable()
	{

		InAppPurchasing.PurchaseCompleted -= OnPurchaseCompleted;
		InAppPurchasing.PurchaseFailed -= OnPurchaseFailed;
		InAppPurchasing.RestoreCompleted -= OnRestoreCompleted;
		InAppPurchasing.RestoreFailed -= OnRestoreFailed;
		GameServices.UserLoginSucceeded -= OnUserLoginSucceeded;
		GameServices.UserLoginFailed -= OnUserLoginFailed;
		Advertising.InterstitialAdCompleted -= OnInterstitialAdCompleted; 
	}

	void OnInterstitialAdCompleted(InterstitialAdNetwork arg1, AdLocation arg2)
	{
		PlaneGameManager.isAd = false;
	}


	void OnUserLoginSucceeded()
	{
		Debug.Log ("UserLoginSucceeded");
	}
	void OnUserLoginFailed()
	{
		Debug.Log ("UserLoginFailed");
	}

	void OnPurchaseCompleted(IAPProduct product)
	{
		isAd = false;
		if (product != null) {
			if (product.Name.Equals (EM_IAPConstants.Product_30_Diamonds_Pack)) {
				soundManager.SoundRewardClaim ();
				canvasManager.UpdateDiamond ("+", new BigNumber (30, 1));
				canvasManager.ShowDiamondAnimation ();
				CileadTrace.RecordEvent ("but_0.99");
			} else if (product.Name.Equals (EM_IAPConstants.Product_180_Diamonds_Pack)) {
				soundManager.SoundRewardClaim ();
				canvasManager.UpdateDiamond ("+", new BigNumber (180, 1));
				CileadTrace.RecordEvent ("but_4.99");
				canvasManager.ShowDiamondAnimation ();

			} else if (product.Name.Equals (EM_IAPConstants.Product_420_Diamonds_Pack)) {
				soundManager.SoundRewardClaim ();
				canvasManager.UpdateDiamond ("+", new BigNumber (420, 1));
				canvasManager.ShowDiamondAnimation ();
				CileadTrace.RecordEvent ("but_9.99");
			} else if (product.Name.Equals (EM_IAPConstants.Product_960_Diamonds_Pack)) {
				soundManager.SoundRewardClaim ();
				canvasManager.UpdateDiamond ("+", new BigNumber (960, 1));
				canvasManager.ShowDiamondAnimation ();
				CileadTrace.RecordEvent ("but_19.99");
			} else if (product.Name.Equals (EM_IAPConstants.Product_2550_Diamonds_Pack)) {
				soundManager.SoundRewardClaim ();
				canvasManager.ShowDiamondAnimation ();

				if (DataManager.isSpecialPurchase) {
					DataManager.isSpecialPurchase = false;
					canvasManager.UpdateDiamond ("+", new BigNumber (5700, 1));
					DataManager.SetSpecialOfferTime (0);

					BigNumber income = new BigNumber (DataManager.incomeSec.number * 24 * 3.6 * DataManager.buffIncome, DataManager.incomeSec.units + 1);
					canvasManager.UpdateCoin("+", income);
					CileadTrace.RecordEvent ("but_49.99_special");

					ManagerUserInfo.SetDataByKey ("BuyDiscount",1);

				} else {
					canvasManager.UpdateDiamond ("+", new BigNumber (2550, 1));
					CileadTrace.RecordEvent ("but_49.99");
				}
			} else if (product.Name.Equals (EM_IAPConstants.Product_5700_Diamonds_Pack)) {
				soundManager.SoundRewardClaim ();
				canvasManager.UpdateDiamond ("+", new BigNumber (5700, 1));
				canvasManager.ShowDiamondAnimation ();
				CileadTrace.RecordEvent ("but_99.99");
			} else if (product.Name.Equals (EM_IAPConstants.Product_Gold_Membership)) {
				GetGoldVipRward ();
				CileadTrace.RecordEvent ("vipmembership");
			}
		}

		if (canvasManager.Loading.activeSelf) {
			canvasManager.Loading.SetActive (false);
		}
	}

	void OnPurchaseFailed(IAPProduct product)
	{
		//		NativeUI.Alert("失败", product.Name);
		isAd = false;
		canvasManager.memberSubClicked = false;

		if (canvasManager.Loading.activeSelf) {
			canvasManager.Loading.SetActive (false);
		}
	}

	void OnRestoreCompleted()
	{
		if (canvasManager.Loading.activeSelf) {
			canvasManager.Loading.SetActive (false);
		}
	}

	void OnRestoreFailed()
	{
		if (canvasManager.Loading.activeSelf) {
			canvasManager.Loading.SetActive (false);
		}
	}

	public void GoldVipOverdue() {
		if (DataManager.isGoldVip == 1 && !InAppPurchasing.isSubscribedOwned ()) {
			//NativeUI.Alert ("InAppPurchasing.isSubscribedOwned ()", "NO");
			isVipPast = true;
			ManagerUserInfo.SetDataByKey ("IsGoldVip", 0);
			DataManager.isGoldVip = 0;
			ManagerUserInfo.SetDataByKey ("VipSubscribeTime", 0);
			ManagerUserInfo.SetDataByKey ("SpeedBuffNum", 1f);
			Advertising.ResetRemoveAds ();
			canvasManager.InitData ();
		} else if (DataManager.isGoldVip == 1 && InAppPurchasing.isSubscribedOwned ()) {
			DataManager.roleMaxNum += 2;
			DataManager.maxBlockNum += 2;
		}
	}


	public void GoldVipRestore() {
		if (DataManager.isGoldVip == 0 && !isVipPast) {
			if (InAppPurchasing.isSubscribedOwned ()) {
				canvasManager.ShowCongratulationView (6);
				DataManager.isGoldVip = 1;
				blocksManager.BuyBlocks ();
				ManagerUserInfo.SetDataByKey("IsGoldVip", DataManager.isGoldVip);

				DataManager.buffSpeed = 1.5f;
				ManagerUserInfo.SetDataByKey("SpeedBuffNum", DataManager.buffSpeed);

				Advertising.RemoveAds ();
				GetDailyVipReward ();
				DataManager.roleMaxNum += 2;
				defendPlaneMgr.RefreshDefendList ();
				//跑道箭头指示 废弃 留着
//				for (int i = 0; i < 2; i++) {
//					GameObject path =  Resources.Load ("Prefabs/UI/path") as GameObject;
//					path = Instantiate (path);
//					path.transform.parent = canvasManager.pathCapacity.transform;
//					path.transform.localPosition = new Vector3 (0, 1f,  (DataManager.roleMaxNum - (2-i)) * 0.4f);
//					path.transform.localScale = new Vector3 (0.3f, 0.3f, 0.3f);
//					path.transform.localRotation = Quaternion.Euler(90f, 180f,0.0f);
//					canvasManager.pathCapacity.transform.localPosition = new Vector3 (
//						-3.75f, -0.8f, -DataManager.roleMaxNum * 0.2f
//					);
//					paths.Add (path);
//					canvasManager.pathContent.text = (DataManager.roleNum + " / " + DataManager.roleMaxNum);
//				}
			}
		}
	}
	public void GetGoldVipRward() {

		if(InAppPurchasing.isSubscribedOwned() && DataManager.isGoldVip == 1 && canvasManager.memberSubClicked == true) {
			int subscribeTime = ManagerUserInfo.GetIntDataByKey ("VipSubscribeTime");
			subscribeTime += 7;
			ManagerUserInfo.SetDataByKey ("VipSubscribeTime", subscribeTime);
		} else if (DataManager.isGoldVip == 0) {
			canvasManager.ShowCongratulationView (6);
			DataManager.isGoldVip = 1;
			blocksManager.BuyBlocks ();
			ManagerUserInfo.SetDataByKey("IsGoldVip", DataManager.isGoldVip);

			DataManager.buffSpeed = 1.5f;
			ManagerUserInfo.SetDataByKey("SpeedBuffNum", DataManager.buffSpeed);

			Advertising.RemoveAds ();

			//			int subscribeTime = ManagerUserInfo.GetIntDataByKey ("VipSubscribeTime");
			//			subscribeTime += 7;
			//			ManagerUserInfo.SetDataByKey ("VipSubscribeTime", subscribeTime);
			GetDailyVipReward ();

			DataManager.roleMaxNum += 2;
			defendPlaneMgr.RefreshDefendList ();
			//跑道箭头指示 废弃 留着
//			for (int i = 0; i < 2; i++) {
//				GameObject path =  Resources.Load ("Prefabs/UI/path") as GameObject;
//				path = Instantiate (path);
//				path.transform.parent = canvasManager.pathCapacity.transform;
//				path.transform.localPosition = new Vector3 (0, 1f,  (DataManager.roleMaxNum - (2-i)) * 0.4f);
//				path.transform.localScale = new Vector3 (0.3f, 0.3f, 0.3f);
//				path.transform.localRotation = Quaternion.Euler(90f, 180f,0.0f);
//				canvasManager.pathCapacity.transform.localPosition = new Vector3 (
//					-3.75f, -0.8f, -DataManager.roleMaxNum * 0.2f
//				);
//				paths.Add (path);
//				canvasManager.pathContent.text = (DataManager.roleNum + " / " + DataManager.roleMaxNum);
//			}
		}

		canvasManager.memberSubClicked = false;
	}

	public void GetDailyVipReward() {
		//		int subscribeTime = ManagerUserInfo.GetIntDataByKey ("VipSubscribeTime");
		//		subscribeTime--;
		//		ManagerUserInfo.SetDataByKey ("VipSubscribeTime", subscribeTime);
		if(PlayerPrefs.GetInt("IsGetDailyVipReward", 0) == 0) {
			soundManager.SoundRewardClaim ();
			canvasManager.UpdateDiamond ("+", new BigNumber (10, 1));
			canvasManager.ShowDiamondAnimation ();
			PlayerPrefs.SetInt ("IsGetDailyVipReward", 1);
		}


	}

	public void ShowGuideAnimation() {
		if (DataManager.roleNum < DataManager.roleMaxNum && !isGuideStep) {
			for (int i = 0; i < blocksManager.GetAllBlockCount(); i++) {
				Transform _build = blocksManager.GetBuildInBlock (i);
				ModelBlock _bInfo = ManagerBlock.GetBlockXmlDataById (i);
				if (_bInfo != null && _bInfo.BuildLv != 0 && _bInfo.BuildState == 0 && _build != null) {
					guide.SetActive (true);
					guideVector = guide.transform.parent.InverseTransformPoint(_build.parent.position);//new Vector3 (_build.parent.localPosition.x + 1f, 5f, _build.parent.localPosition.z - 2f);
					guideVector.y = guidePosY;
					guideVector.x = guideVector.x + 1f;
					guideVector.z = guideVector.z - 3f;
					guide.transform.localPosition = guideVector;
					//					isGuideShow = true;
					//					guideClickedEffect.Play ();
					guideStart = true;

					StartCoroutine (GuideReturen ());
					//Debug.Log ("ShowGuideAnimation");
					break;
				}
			}
		} else {
			for(int i = 0; i < blocksManager.GetAllBlockCount();i++){
				Transform _build = blocksManager.GetBuildInBlock(i);
				ModelBlock _bInfo = ManagerBlock.GetBlockXmlDataById (i);
				if (_bInfo != null && _bInfo.BuildLv != 0 && _bInfo.BuildState == 0 && _build != null) {
					for(int j = i+1; j < blocksManager.GetAllBlockCount();j++){
						Transform _build2 = blocksManager.GetBuildInBlock(j);
						ModelBlock _bInfo2 = ManagerBlock.GetBlockXmlDataById (j);
						if (_bInfo2 != null && _bInfo2.BuildLv == _bInfo.BuildLv && _bInfo2.BuildState == 0 && _build2 != null) {

							guide.SetActive (true);
							guideVector = guide.transform.parent.InverseTransformPoint(_build.parent.position);//new Vector3 (_build.parent.localPosition.x + 1f, 5f, _build.parent.localPosition.z - 2f);
							guideVector.y = guidePosY;
							guideVector.x = guideVector.x + 1f;
							guideVector.z = guideVector.z - 3f;
							guide.transform.localPosition = guideVector;
							targetPos = guide.transform.parent.InverseTransformPoint(_build2.parent.position);//new Vector3 (_build2.parent.localPosition.x + 1f, 5f, _build2.parent.localPosition.z - 2f);
							targetPos.y = guidePosY;
							targetPos.x = targetPos.x + 1f;
							targetPos.z = targetPos.z - 3f;
							guideStart = true;
							StartCoroutine (GuideReturen ());
							break;
						}
					}
				}
			}
		}

	}

	public void StopGuideAnimation() {
		guideTime = 0;
		guideStart = false;
		guide.SetActive (false);
		//Debug.Log ("StopGuideAnimation");
		isGuideShow = false;
	}	


	private IEnumerator GuideReturen() {
		yield return new WaitForSeconds (0.3f);
		guide.SetActive (false);
		isGuideShow = false;
		guide.transform.localPosition = guideVector;
		yield return new WaitForSeconds (0.3f);
		guide.SetActive (true);
		if(!guideClickedEffect.isPlaying)
			guideClickedEffect.Play ();
		yield return new WaitForSeconds (0.4f);
		isGuideShow = true;

	}

	public void UpdateDailyTask(string taskID) {
		foreach(ModelTask taskInfo in DataManager.taskInfoList) {
			if (taskInfo.TaskId.Equals(taskID) && taskInfo.IsGet == 0) {
				taskInfo.CurAchieveNum += 1;
				ManagerTask.SetTaskXmlData (taskInfo);
				if (taskInfo.CurAchieveNum >= taskInfo.NeedAchieveNum) {
					canvasManager.newTipObj.SetActive (true);
					canvasManager.questsNewObj.SetActive (true);
				}
				break;
			}
		}
	}


	public void ReportScore()
	{
		if (!GameServices.IsInitialized ()) {
			//NativeUI.Alert("Alert", "You need to initialize the module first.");
			return;
		} else {

			long score = System.Convert.ToInt64(DataManager.incomeAll.number);
			if (DataManager.coins.units >= 2) {
				for (int i = 1; i < DataManager.coins.units; i++) {
					if (score > long.MaxValue / 1000) {	
						score = long.MaxValue;
						break;
					}
					score *= 1000; 
				}
			}
			GameServices.ReportScore(score, EM_GameServicesConstants.Leaderboard_leaderboard_fort_clash);
			//			Debug.Log("Score = "  + score);

			//			long score = DataManager.coins.number;
			//			for (int i = 0; i < DataManager.coins.units; i++) {
			//				score *= 1000; 
			//			}
			//			Debug.Log("Score = "  + score);
			//			GameServices.ReportScore(score, EM_GameServicesConstants.Leaderboard_Assets);
			//			//NativeUI.Alert("Alert", "Reported score " + score + " to leaderboard \"" + selectedLeaderboard.Name + "\".");
		}
	}
}
