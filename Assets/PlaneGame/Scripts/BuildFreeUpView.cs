using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyMobile;

public class BuildFreeUpView : AppAdvisoryHelper {
	public Text curText;
	public Text nextText;
	// 数据属性 
	public int buildLv = 1;
	public int upLv = 0;
	public int buyNum = 0;
	public int payType = 0;
	public ModelBuild buildModel= new ModelBuild();
	public BigNumber priceBigNumber = new BigNumber();
	public BigNumber priceDiamond = new BigNumber (5, 0);

	public Button buyBtn;
	public GameObject cur;
	public GameObject next;
	public GameObject shop;

	private bool isGet = false; // 判断是否购买或者收看广告
	private bool up = false;
	private bool down = false;
	// Use this for initialization
//	void Start () {
//		
//	}
//	
//	// Update is called once per frame
//	void Update () {
//	}
//
	void OnEnable() {
		isGet = false;
		buyBtn.enabled = true;
		buildLv = 1;
		upLv = 0;
		buyNum = 0;
		payType = 0;
		buildModel = new ModelBuild();
		priceBigNumber = new BigNumber();
		priceDiamond = new BigNumber (5, 0);
		Advertising.RewardedAdSkipped += OnRewardedAdSkipped;
		Advertising.RewardedAdCompleted += OnRewardedAdCompleted;
	}

	void OnDisable() {
		Destroy (cur);
		Destroy (next);

		Advertising.RewardedAdSkipped -= OnRewardedAdSkipped;
		Advertising.RewardedAdCompleted -= OnRewardedAdCompleted;

		if (!isGet) {
			Debug.Log ("Get Build" + buildLv);
			buyNum = buyNum + blocksManager.CreatBuildingByCoin(priceBigNumber, buildModel, buildLv, payType);
		}
	}
		
	void OnRewardedAdCompleted(RewardedAdNetwork arg1, AdLocation arg2)
	{	
		if (DataManager.getMaxBuildLv >= 8) {
			planeGameManager.UpdateDailyTask ("1901");
		}
		GetNextLvBuilding ();

		PlaneGameManager.isAd = false;
//		planeGameManager.isAd = false;
	}

	void OnRewardedAdSkipped(RewardedAdNetwork arg1, AdLocation arg2)
	{
		PlaneGameManager.isAd = false;
//		planeGameManager.isAd = false;
		// buyNum = buyNum + blocksManager.CreatBuildingByCoin(priceBigNumber, buildModel, buildLv, payType);
	}

	// 数据初始化
	public void InitData(int blv, int ulv, int bnum, ModelBuild bmodel, BigNumber pb, int ptype) {
		buildLv = blv;
		upLv = ulv;
		buyNum = bnum;
		payType = ptype;
		buildModel = bmodel;
		priceBigNumber = pb;

		curText.text = ConfigFileMgr.GetContentByKey ("Building" + blv,curText);//curInfo["BuildName"];
		nextText.text = ConfigFileMgr.GetContentByKey ("Building" + (blv+ upLv),nextText);//nextInfo["BuildName"];

		StartCoroutine (CreateCur (0.5f));
		StartCoroutine (CreateNext (0.7f));
	}


	private IEnumerator CreateCur(float sec) {
		yield return new WaitForSeconds (sec);
		cur = Resources.Load ("Prefabs/BuildIcon/BICON" + buildLv) as GameObject;
		cur = Instantiate (cur);
		cur.transform.parent = transform;
		cur.transform.localPosition = new Vector3 (-150f, -40f, -200f);
		cur.transform.GetComponent<BuildIcon> ().imgSize = new Vector2 (300, 300);
		down = true;
	}

	private IEnumerator CreateNext(float sec) {
		yield return new WaitForSeconds (sec);
		next = Resources.Load ("Prefabs/BuildIcon/BICON" + (buildLv + upLv)) as GameObject;
		next = Instantiate (next);
		next.transform.parent = transform;
		next.transform.localPosition = new Vector3 (-150f, 200f, -200f);
		next.transform.GetComponent<BuildIcon> ().imgSize = new Vector2 (300, 300);
		next.transform.GetComponent<BuildIcon> ().img.GetComponent<Canvas> ().overrideSorting = true;
		next.transform.GetComponent<BuildIcon> ().img.GetComponent<Canvas> ().sortingOrder = 2;
		up = true;
	}

	// 购买
	public void BtnBuyCallBack() {
		if(DataManager.diamond.IsBiggerThan(priceDiamond)){
			soundManager.SoundPayCash ();
			canvasManager.UpdateDiamond("-", priceDiamond);
			GetNextLvBuilding ();
		}else{
			soundManager.SoundUIClick ();
			canvasManager.diamondStore.gameObject.SetActive(true);
		}
	}

	// 广告
	public void AdBtnClicked() {
		canvasManager.ShowRewardAd ();
	}

	// 奖励
	public void GetNextLvBuilding() {
		buyBtn.enabled = false;
		soundManager.SoundRewardClaim ();
		ModelBuild _model = DataManager.buildInfoList[buildLv + upLv - 1] as ModelBuild;
		buyNum = buyNum + blocksManager.CreatBuildingByCoin(priceBigNumber, _model, _model.BuildLv, payType);
		// 动画
		shop.transform.GetComponent<ShopManager> ().CloseCallBack ();
		isGet = true;

		shop.SetActive (false);
		gameObject.SetActive (false);

	}
}
