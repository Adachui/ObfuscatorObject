using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyMobile;

public class ShopManager : AppAdvisoryHelper
{
    private static int openNum = 0;
    public List<int> buyBuildList = new List<int>();

    public GameObject buildScrollView;
    public GameObject otherScrollView;

    public Button buildBtn;
    public Button OtherBtn;

    public ModelBuild buildModel;
	public int buildLv = 0; 
	public int payType = 0;

    private int isLoadOther = 0;
    private int isLoadBuild = 0;
	private float discount = 0;
    public void SetGetInfo(ModelBuild bm, int bl, int pt) {
		buildModel = bm;
		buildLv = bl; 
		payType = pt;
	}
    //private int viewType = 0; //0 是建筑列表 1 是buff列表

    private void Awake()
    {
        Debug.Log("ShopManager Awake");
        openNum += 1;
    }
    // Use this for initialization
//    void Start () {
//        //SetBuildList();
//        Debug.Log("ShopManager Start");
//    }
	
	// Update is called once per frame
//	void Update () {
//		
//	}
		
    //关闭商店
    public void CloseCallBack(){
		if (!canvasManager.isAdLoading) {
			CleanView();
			if (Random.Range(1, 100) <= 10 || openNum >= 10){
				ShowInterstitialAd ();
				openNum = 0;
			}
			canvasManager.coinTextLeft.SetActive (false);
			gameObject.SetActive (false);
		}
    }

    public void SetBuildList(int loadType)
    {
		discount = DataManager.buffCutPrice;
        isLoadOther = 0;
        isLoadBuild = 0;
        switch (loadType){
            case 0:
                buildBtn.transform.GetChild(0).gameObject.SetActive(false);
                buildBtn.transform.GetChild(1).gameObject.SetActive(true);
                OtherBtn.transform.GetChild(0).gameObject.SetActive(true);
                OtherBtn.transform.GetChild(1).gameObject.SetActive(false);
                BuildBtnCallBack();
                break;
            case 1:
                buildBtn.transform.GetChild(0).gameObject.SetActive(true);
                buildBtn.transform.GetChild(1).gameObject.SetActive(false);
                OtherBtn.transform.GetChild(0).gameObject.SetActive(false);
                OtherBtn.transform.GetChild(1).gameObject.SetActive(true);
                OtherBtnCallBack();
                break;
        }

    }

    public void OtherBtnCallBack(){
		soundManager.SoundUIClick ();
        if(isLoadOther == 0){
			canvasManager.shopLoadType = 1;
            isLoadOther++;
            otherScrollView.gameObject.SetActive(true);
            buildScrollView.gameObject.SetActive(false);
            otherScrollView.GetComponent<UIScrollRect>().dataList = DataManager.buffInfoList;
            otherScrollView.GetComponent<UIScrollRect>().SetData();
        }
    }

    public void BuildBtnCallBack() {	
		soundManager.SoundUIClick ();
		if (DataManager.buffCutPrice != discount) {
			isLoadBuild = 0;
			discount = DataManager.buffCutPrice;
			buildScrollView.GetComponent<UIScrollRect>().CleanAllItem();
		}
		if (isLoadBuild == 0) {
			canvasManager.shopLoadType = 0;
			isLoadBuild++;
			otherScrollView.gameObject.SetActive (false);
			buildScrollView.gameObject.SetActive (true);
			buildScrollView.GetComponent<UIScrollRect> ().dataList = DataManager.buildInfoList;
			buildScrollView.GetComponent<UIScrollRect> ().SetData ();
		}
    }

    private void CleanView(){
        isLoadOther = 0;
        isLoadBuild = 0;
        buildScrollView.GetComponent<UIScrollRect>().CleanAllItem();
        otherScrollView.GetComponent<UIScrollRect>().CleanAllItem();
    }
		
	public static void LoadInterstitialAd()
	{
		if (Advertising.IsAutoLoadDefaultAds())
		{
			// NativeUI.Alert("Alert", "autoLoadDefaultAds is currently enabled. Ads will be loaded automatically in background without you having to do anything.");
		}

		Advertising.LoadInterstitialAd();
	}

	public static void ShowInterstitialAd()
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

	void OnEnable()
	{
		canvasManager.isOnShop = true;
		canvasManager.isUILayer++;
		LoadInterstitialAd ();

//		Advertising.ShowBannerAd (BannerAdPosition.Bottom);
    }

	void OnDisable()
	{
		canvasManager.isOnShop = false;
		canvasManager.isUILayer--;
//		Advertising.HideBannerAd ();
	}
}
