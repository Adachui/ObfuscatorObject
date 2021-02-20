using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyMobile;
using System;

public class CanvasManager : AppAdvisoryHelper
{
	public int isUILayer = 0;  //在主场景上打开的UI层计数
	public Text leftCoinText;
	public Text coinText;
	public Animation coinAnimation; // 金币放大
	public Text diamondText;
	public Text playerLvText;
	public Text playerNameText;
	public Text playerBuffText;
	public Text pathCoinText;
	public Text incomeSecText;
	public Text rankingText;
	public Text showBuildPriceText;
	public Text defenseText; //防御数量
	public Image expImg;
	public Image showBuildImg;
	public GameObject coinTextLeft;
	public GameObject Warning;
	//钻石商店
	public GameObject diamondStore;
	//商店
	public GameObject store;
	public GameObject OfflineWindow;

	public GameObject buildingUnlock;
	public Button speedBtn;
	public Button shopBtn;

	// 收益图标
	public GameObject buffIncomeIcon;
	public Text buffIncomeText;
	public GameObject buffSpeedIcon;
	public Text buffSpeedText;
	//private int buyBuildPrice = 0;

	// 当前购买建筑等级
	public ModelBuild showBuildModel;
	public BigNumber showPriceBigNumber;

	// 垃圾桶特效
	public ParticleSystem trashSmokeEffect;

	// 特效横幅
	public GameObject banner;
	// 金币雨
	public ParticleSystem goldRainEffect;
	public GameObject congratulationView;
	// 钻石特效
	public GameObject diamondAnimation;
	public GameObject diamondImg;
	// 等级提升页面
	public GameObject lvUpView;

	// new 
	public GameObject newTipObj;
	public GameObject questsNewObj;

	public GameObject showBuild;

	public GameObject buildingFreeUp;
	// 跑道容量指示
	//public GameObject pathCapacity;
	public Transform earning;
	public GameObject showBuildIcon;
	// 终点金币获取提示
	public Transform coinContant;
	// Loading
	public GameObject Loading;

	// 任务页面
	public GameObject questsView;

//	public GameObject guide;
//	public ParticleSystem guideEffec;

	// 是否在商店购买页面
	public bool isOnShop = false;
	public GameObject shopFreeIcon;
	// 转盘图标
	public GameObject spin;
	public bool isAdLoading = false;
	public bool isShowOfflinewView = false;
	public bool memberSubClicked = false;

	public GameObject trash;
	public GameObject recycle;

	public bool isOtherLayerOpen = false;

	public GameObject GuideLayer;

	private int lastShowLV = -1;

	public Transform moreBtn;
	public Transform moreBtnMesh;
	public Transform recyleBtnMesh;
	public Transform recyleText;
	public Camera UICamera;
	public GameObject planeBg;

	public int shopLoadType = 0;  //记录上一次关掉界面时的商店列表的类型 0 建筑 1 buff

	private void Awake()
	{
		goldRainEffect.Stop ();
	}

//	public void jiajingyan(){
//		planeGameManager.RefreshLevelData (10);
//	}

//	void OnGUI(){
//		Event e=Event.current;
//		if(e.isMouse&&(e.clickCount==2))
//			Debug.Log("用户双击了鼠标");          
//	}
	//初始化数据信息
	public void InitData(){
		isOnShop = false;
		leftCoinText.text = DataManager.coins.ToString();
		coinText.text = DataManager.coins.ToString();
		pathCoinText.text = DataManager.pathCoins.ToString();
		diamondText.text = DataManager.diamond.ToString();
		DataManager.exp = ManagerUserInfo.GetIntDataByKey("Exp");
		float expProgress = float.Parse( DataManager.exp.ToString() ) / float.Parse(canvasManager.GetIntDataByKey(DataManager.userConfigDic, "Exp").ToString());
		Debug.Log ("expProgress: " + expProgress + " = " + DataManager.exp + "/" + canvasManager.GetIntDataByKey(DataManager.userConfigDic, "Exp").ToString());
		expImg.fillAmount = expProgress;
		//Debug.Log("expProgress == " + expProgress);
		playerLvText.text = DataManager.lv.ToString();

		playerNameText.text = ManagerUserInfo.GetStringValueByKey("Name");


		playerBuffText.text = "Coins x" + DataManager.buffIncome.ToString();

		UpdateIncomeSecText (DataManager.incomeSec.ToString());

		//incomeSecText.text = DataManager.incomeSec.ToString() + "/sec";

		//排行榜数值实时获取
		rankingText.text = DataManager.rank > 0 ? DataManager.rank.ToString() : "--";

		InitShowBuild();
		InitCanvas();
	}

	public void InitCanvas(){
		//shopBtn.gameObject.SetActive(true);
		if (DataManager.getMaxBuildLv >= 3)
		{
			speedBtn.gameObject.SetActive(true);
			shopBtn.gameObject.SetActive(true);
		} 
		if (DataManager.getMaxBuildLv >= 8) {
			spin.SetActive (true);
		}

//		Vector2 pos = moreBtn.GetComponent<RectTransform> ().anchoredPosition;//GetUGUIPostionByWorldPosition (moreBtnMesh.position, transform, UICamera);
//		pos.y = pos.y + 100;
//		moreBtn.GetComponent<RectTransform> ().anchoredPosition = pos;
	}

	public Vector2 GetUGUIPostionByWorldPosition(Vector3 worldPos, Transform uiRoot,Camera uiCamera)
	{
		CanvasScaler canvasScaler = uiRoot.GetComponent<CanvasScaler>() as CanvasScaler;

		float scaleFator = 0f;

		float match = canvasScaler.matchWidthOrHeight;

		if (match == 0)
		{
			scaleFator = canvasScaler.referenceResolution.x / Screen.width;
		}
		else
		{
			scaleFator = canvasScaler.referenceResolution.y / Screen.height;
		}

		Vector2 povot = uiRoot.GetComponent<RectTransform>().pivot;

		float fix_x = povot.x * canvasScaler.referenceResolution.x;
		float fix_y = povot.y * canvasScaler.referenceResolution.y;

		Vector2 screenPos = Camera.main.WorldToScreenPoint(worldPos);

		return new Vector2(screenPos.x * scaleFator, screenPos.y * scaleFator);
	}

	public void InitShowBuild() {

		Dictionary<string, string> buildConfig = DataManager.buildConfigList[DataManager.getMaxBuildLv - 1];
		int showLv = GetIntDataByKey(buildConfig, "ShowLv");
		Dictionary<string, string> showConfig = DataManager.buildConfigList[showLv - 1];
		showBuildModel = DataManager.buildInfoList[showLv - 1] as ModelBuild;

		double priceX = GetDoubleDataByKey(showConfig, "PriceX");
		double price = GetDoubleDataByKey(showConfig, "Price");
		int unit = GetIntDataByKey(showConfig, "PriceUnit");

		if (showBuildModel.BuyNum != 0)
			price = price * Math.Pow(priceX, showBuildModel.BuyNum) * (1 - DataManager.buffCutPrice);

		showPriceBigNumber = new BigNumber(price, unit);
		showBuildPriceText.text = showPriceBigNumber.ToString();

		if (lastShowLV != showLv) {
			//RefreshIcon (showLv);
			if (showBuildIcon.transform.childCount > 0) {
				var childObj = showBuildIcon.transform.GetChild (0);
				if (childObj != null) {
					Destroy (childObj.gameObject);
				}
			}
			GameObject sbi = Resources.Load ("Prefabs/BuildIcon/BICON" + showLv) as GameObject;
			sbi = Instantiate (sbi);
			sbi.transform.parent = showBuildIcon.transform;
			sbi.transform.localPosition = new Vector3 (-147, 60, -208f);
			sbi.transform.GetComponent<BuildIcon> ().imgSize = new Vector2 (150, 150);

			lastShowLV = showLv;
		}
	}

	//替换炮图标
	public void RefreshIcon(int lv){
		showBuildImg.sprite = Resources.Load<Sprite> ("image/Turrets/NO." + lv);;
	}


	public void GetBuildByCoin() {
		//新手引导
		if (ManagerUserInfo.GetIntDataByKey ("NewPlayerStep1") == 0) {
			ManagerUserInfo.SetDataByKey ("NewPlayerStep1", 1);
			guideLayer.GuideStep_1 ();
		}
		blocksManager.CreatBuildingByCoin(showPriceBigNumber, showBuildModel, showBuildModel.BuildLv, 0);
	}

	public void ShowShop(){
		store.GetComponent<ShopManager>().SetBuildList(shopLoadType);
	}

	////////////////////////////////更新各种货币////////////////////////////////////////
	public void UpdateCoin(string symbol,BigNumber bigNumber){
		switch (symbol)
		{
		case "+":
			DataManager.coins.Add(bigNumber);
			DataManager.incomeAll.Add(bigNumber);

			ManagerUserInfo.SetDataByKey("IncomeAll", DataManager.coins.number.ToString());
			ManagerUserInfo.SetDataByKey("IncomeAllUnit", DataManager.coins.units.ToString());
			break;
		default:
			DataManager.coins.Minus(bigNumber);
			break;
		}
		coinText.text = DataManager.coins.ToString();
		leftCoinText.text = DataManager.coins.ToString();
		ManagerUserInfo.SetDataByKey("Coins", DataManager.coins.number.ToString());
		ManagerUserInfo.SetDataByKey("CoinsUnit", DataManager.coins.units);

	}


	public void UpdateDiamond(string symbol, BigNumber bigNumber)
	{
		switch (symbol)
		{
			case "+":
				DataManager.diamond.Add (bigNumber);
				diamondText.text = DataManager.diamond.ToString ();
				DiamondEvent (bigNumber);
				break;
			default:
				DataManager.diamond.Minus(bigNumber);
				diamondText.text = DataManager.diamond.ToString();
				break;
		}

		ManagerUserInfo.SetDataByKey("Diamond", DataManager.diamond.number.ToString());
		ManagerUserInfo.SetDataByKey("DiamondUnit", DataManager.diamond.units);
		ManagerUserInfo.SaveData ();
	}

	public void DiamondEvent(BigNumber diamond){
		int index = (int)diamond.number / 50;
		if (index > 0 && index <= 10) {
			for (int j = 1; j <= index; j++) {
				int d = j * 50;
				if (PlayerPrefs.GetInt (EventConst.DIAMOND + d.ToString (), 0) == 0) {
					CileadTrace.RecordEvent (EventConst.DIAMOND + d.ToString ());
					PlayerPrefs.SetInt (EventConst.DIAMOND + d.ToString (), 1);
				}
			}
		}
	}
		
	public IEnumerator GetDiamonDelay() {
		yield return new WaitForSeconds (1.5f);
		diamondText.text = DataManager.diamond.ToString();
	}

	public void UpdatePathCoin(string symbol, BigNumber bigNumber)
	{
		switch (symbol)
		{
		case "+":
			DataManager.pathCoins.Add(bigNumber);
			break;
		default:
			DataManager.pathCoins.Minus(bigNumber);
			break;
		}
		pathCoinText.text = DataManager.pathCoins.ToString();
		ManagerUserInfo.SetDataByKey("PathCoins", DataManager.pathCoins.number.ToString());
		ManagerUserInfo.SetDataByKey("PathCoinsUnit", DataManager.pathCoins.units);
	}

	public void UpdateIncomeSec(string symbol, BigNumber bigNumber)
	{
		switch (symbol)
		{
		case "+":
			DataManager.incomeSec.Add(bigNumber);
			break;
		default:
			DataManager.incomeSec.Minus(bigNumber);
			break;
		}
		BigNumber incomeText = new BigNumber (DataManager.incomeSec.number * DataManager.buffIncome, DataManager.incomeSec.units);
//		incomeSecText.text =  incomeText.ToString() + "/sec";

		UpdateIncomeSecText (incomeText.ToString());

		ManagerUserInfo.SetDataByKey("IncomeSecond", DataManager.incomeSec.number.ToString());
		ManagerUserInfo.SetDataByKey("IncomeSecondUnit", DataManager.incomeSec.units);
	}

	public void UpdateOther(string symbol, BigNumber bigNumber)
	{
		switch (symbol)
		{
		case "+":
			DataManager.incomeSec.Add(bigNumber);
			break;
		default:
			DataManager.incomeSec.Minus(bigNumber);
			break;
		}

//		incomeSecText.text = DataManager.incomeSec.ToString();
		UpdateIncomeSecText (DataManager.incomeSec.ToString());
		ManagerUserInfo.SetDataByKey("IncomeSecond", DataManager.incomeSec.number.ToString());
		ManagerUserInfo.SetDataByKey("IncomeSecondUnit", DataManager.incomeSec.units);

	}


	////////////////////////////////获取数值//////////////////////////////
	public int GetIntDataByKey(Dictionary<string, string> _dic,string key, int value = 0)
	{
		int result = 0;

		result = int.TryParse(_dic[key], out result) ? result : value;
		return result;
	}

	public float GetFloatDataByKey(Dictionary<string, string> _dic,string key, int value = 0)
	{
		float result = 0;

		result = float.TryParse(_dic[key], out result) ? result : value;
		return result;
	}

	public double GetDoubleDataByKey(Dictionary<string, string> _dic, string key, int value = 0)
	{
		double result = 0;

		result = double.TryParse(_dic[key], out result) ? result : value;
		return result;
	}

	// Use this for initialization
//	void Start () {
//
//	}

	// Update is called once per frame
	void Update () {
		if (!PlaneGameManager.isBack) {
			DataManager.curOnLineTime += Time.deltaTime;
		}
		PlayerPrefs.SetFloat ("ONLINETIME", DataManager.curOnLineTime);

		if (DataManager.isFree == 0) {
			shopFreeIcon.SetActive (false);
			DataManager.freeTime += Time.deltaTime;
			if ((int)DataManager.freeTime == 60) {
				DataManager.freeTime = 0;
				DataManager.isFree = 1;
				ManagerUserInfo.SetDataByKey ("IsExitFree", DataManager.isFree);
			}
		} else {
			shopFreeIcon.SetActive (true);
		}


		//coinText.text = DataManager.coins.ToString();
		//leftCoinText.text = DataManager.coins.ToString();
	}

	public void UpdateIncomeSecText(string text)
	{
		canvasManager.incomeSecText.text =  text + ConfigFileMgr.GetContentByKey(EM_LocalizeConstants.Locaize_PerSecond,canvasManager.incomeSecText);
	}

	public void ShowOffLineWindow() {
		if(DataManager.GetOffLineTime ()) {
			OfflineWindow.SetActive (true);
		}
	}

	public void ShowWarning(string type) {
		Warning.SetActive (true);
		var textComponent = Warning.transform.GetChild (0).GetComponent<Text> ();
		var font = i18n.GetTextFont ();
		if(font != null)
			textComponent.font = font;
		
		textComponent.text = type;
		StartCoroutine (RemoveWarning());
	}

	IEnumerator RemoveWarning(){
		yield return new WaitForSeconds(1);
		Warning.SetActive (false);
	}

	public void TrashSmokeEffectShow() {
		if (!trashSmokeEffect.isPlaying) {
			trashSmokeEffect.Play ();
			soundManager.SoundPayCash ();
		}
	}

	public void ShowBuffGetBanner(List<int> showTypes) {
		if (showTypes.Count == 1) {
			banner.SetActive (true);
			banner.transform.GetChild (0).GetChild (showTypes[0]).gameObject.SetActive (true);
		} else if (showTypes.Count > 1) {
			for (int i = 0; i < showTypes.Count; i++) {
				//Debug.Log ("showTypes[i]" + showTypes [i]);
				StartCoroutine (ShowBannerType (i * 2, showTypes[i]));
			}
		}
	}

	private IEnumerator ShowBannerType(float sec, int type) {
		yield return new WaitForSeconds(sec);
		banner.SetActive (true);
		for (int i = 1; i <= 3; i++) {
			banner.transform.GetChild (0).GetChild (i).gameObject.SetActive (i == type);
		}

//		Warning.SetActive (false);
	}

	// 奖励确认
	public void ShowCongratulationView(int type) {
		congratulationView.SetActive (true);
		congratulationView.GetComponent<CongratulationWindowView> ().InitWithType (type);
	}

	public void ShowCongratulationViewWithIcon(Sprite sp, string description, float scale, bool hasFrame = false,Action<int> type = null) {
		congratulationView.SetActive (true);
		congratulationView.GetComponent<CongratulationWindowView> ().InitWithIcon (sp, description,scale, hasFrame,type);
	}

	public void CongratulationViewDisableActionBack(int type){
		if (StoreReview.CanRequestRating ()) {
			StoreReview.RequestRating (null, RateGameUserAction);
		}
	}

	public void RateGameUserAction(StoreReview.UserAction userAction){
		if(userAction == StoreReview.UserAction.Rate){
			canvasManager.UpdateDiamond("+", new BigNumber(20, 0));
			StoreReview.DisableRatingRequest ();
		
			soundManager.SoundRewardClaim ();
			ShowDiamondAnimation ();
		}
	}

	public void ShowDiamondAnimation() {
		if (!diamondAnimation.activeSelf) {
			StartCoroutine (DiamondAnimationDealy ());
		}
	}


	public IEnumerator DiamondAnimationDealy() {
		diamondImg.GetComponent<Animator> ().SetBool ("isDiamondEffect", true);
		diamondImg.transform.GetChild (0).GetComponent<ParticleSystem> ().Play ();
		diamondAnimation.SetActive (true);
		yield return new WaitForSeconds(2.5f);
		diamondAnimation.SetActive (false);
	}


	public IEnumerator LoadingAnimation(bool ad = true) {
		if (ad) {
			isAdLoading = true;
			//Advertising.LoadRewardedAd ();
			Loading.SetActive (true);
			yield return new WaitForSeconds (3f);
			isAdLoading = false;
			if (Advertising.IsRewardedAdReady ()) {
				SoundManager.SoundAdsSwitch (false);
				Advertising.ShowRewardedAd ();
				PlaneGameManager.isAd = true;
				CileadTrace.RecordEvent (EventConst.SHOWREWARDAD);
			} else {
				ShowWarning (ConfigFileMgr.GetContentByKey (EM_LocalizeConstants.Locaize_NetWordBroken));
			}
			Loading.SetActive (false);
		} else {
			Loading.SetActive (true);
			yield return new WaitForSeconds(3f);
			ShowWarning (ConfigFileMgr.GetContentByKey (EM_LocalizeConstants.Locaize_NetWordBroken));
			Loading.SetActive (false);
		}

	}

	public void ShowRewardAd() {
		if (Advertising.IsRewardedAdReady ()) {
			SoundManager.SoundAdsSwitch (false);
			Advertising.ShowRewardedAd ();
			PlaneGameManager.isAd = true;
			CileadTrace.RecordEvent (EventConst.SHOWREWARDAD);
		} else {
			StartCoroutine (LoadingAnimation ());
		}
	}

	public void ShowTrashTip()
	{
		//ShowWarning (ConfigFileMgr.GetContentByKey(EM_LocalizeConstants.Locaize_Recycle));
	}
}


