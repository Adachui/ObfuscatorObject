using System;
using System.Collections;
using System.Collections.Generic;
using EasyMobile;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemView : AppAdvisoryHelper
{
	public Text buildNameText;
	public Text discountText;
	public Text buyBtnText;
	public Text questionMarkText;
	public Transform iconPanel;

	public Image speedProgressImg;
	public Image earnProgressImg;
	public Image priceImg;   //价格图片 没解锁时为锁 免费时为广告图片
    public Image grayImg;

	public Button buyBtn;

	public BigNumber priceBigNumber = new BigNumber();
	public GameObject discountIcon;
	ModelBuild buildModel;

    private GameObject icon;

    private int buildLv = 0;
	private int payType = 0; //0 金币 1 钻石 2 免费
    private int buyNum = 0;
	private int upLv = 0;
	private bool isListen = false;
    Dictionary<string, string> buildConfigInfo;

	private Sprite[] priceSps;

//    void Start()
//	{
//
//	}
//
	// Update is called once per frame
	void FixedUpdate()
	{
		if (buyBtn.interactable == false && icon != null && icon.activeSelf)
			UpdateCoinBtn ();
 	}

	public override void InitModelData(IModel _model)
	{
		payType = 0;
		isListen = false;
		buildModel = _model as ModelBuild;

		buildConfigInfo = GlobalKeyValue.GetXmlDataByKey("BuildInfo", "Build" + buildModel.BuildLv);
		Dictionary<string, string> freeConfigInfo = GlobalKeyValue.GetXmlDataByKey("FreeBuildInfo", "FreeBuild" + DataManager.getMaxBuildLv);

		buyBtn.interactable = false;
		questionMarkText.gameObject.SetActive(false);
        //freeImg.gameObject.SetActive(false);
        //diamondImg.gameObject.SetActive(false);
        grayImg.gameObject.SetActive(false);

        if(icon!=null)
            Destroy(icon);
        // 设置图片
        icon = Resources.Load ("Prefabs/BuildIcon/BICON" + buildModel.BuildLv) as GameObject;
        icon = Instantiate (icon);
		icon.transform.parent = iconPanel;
        icon.transform.localPosition = new Vector3 (-145f, 65f, -200f);
		icon.transform.GetComponent<BuildIcon> ().imgSize = new Vector2 (300, 300);

		buildNameText.text = ConfigFileMgr.GetContentByKey ("Building" + buildModel.BuildLv,buildNameText);//buildConfigInfo["BuildName"];

		buildLv = buildModel.BuildLv;
        buyNum = buildModel.BuyNum;
        int needBuildLv = canvasManager.GetIntDataByKey(buildConfigInfo, "UnlockNeedLv");

		int pricePath = 1;
        priceSps = new Sprite[4];
        priceSps = Resources.LoadAll<Sprite>("Sprites/ShopItem");
        int subLv = buildLv - DataManager.maxBuildByCoinLv; 



		if(buildLv <= DataManager.getMaxBuildLv){
			print("needBuildLv = " + needBuildLv + " buildLv = " + buildLv + " getMaxBuildLv = " + DataManager.getMaxBuildLv +  " maxBuildByCoinLv = " +  DataManager.maxBuildByCoinLv );
            icon.SetActive(true);
            if (subLv > 0)
			{
				//钻石价格
				if (subLv < 3){
					payType = 1;

					buyBtn.interactable = true;
                    //diamondImg.gameObject.SetActive(true);
                    buyBtn.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/BtnDiamond");
                    pricePath = 0;

					priceBigNumber.SetNumber(canvasManager.GetDoubleDataByKey(buildConfigInfo, "DimondPrice"), 0);
					buyBtnText.text = priceBigNumber.ToString();

                    

				}
				//图标亮起 但不可购买
				else if(subLv < 5){
                    pricePath = 3;

                    grayImg.gameObject.SetActive(true);

					buyBtnText.text = needBuildLv.ToString();
				}

			}
			//金币和免费
			else
			{
				double priceX = canvasManager.GetDoubleDataByKey(buildConfigInfo, "PriceX");
				double price = canvasManager.GetDoubleDataByKey(buildConfigInfo, "Price");
				int unit = canvasManager.GetIntDataByKey(buildConfigInfo, "PriceUnit");

				if (buyNum != 0)
					price = price * Math.Pow(priceX, buyNum) * (1 - DataManager.buffCutPrice);
				
				priceBigNumber.SetNumber(price, unit);


				int freeLv = canvasManager.GetIntDataByKey(freeConfigInfo, "FreeBuildLv");
				if (DataManager.isFree == 1 && buildLv == freeLv)
				{
					payType = 2;
					pricePath = 2;
					buyBtnText.text = ConfigFileMgr.GetContentByKey(EM_LocalizeConstants.Locaize_Free,buyBtnText);
					buyBtn.interactable = true;
                    buyBtn.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/BtnFree");
                }
				else
				{
					buyBtn.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/BtnCoin");
					buyBtnText.text = priceBigNumber.ToString();
					var font = i18n.GetTextFont ();
					if(font != null)
						buyBtnText.font = font;
					print(" buildLv = " + buildLv + " interactable = " + !priceBigNumber.IsBiggerAndNotEquals(DataManager.coins) );
                    buyBtn.interactable = !priceBigNumber.IsBiggerAndNotEquals(DataManager.coins);
                }
			}

			speedProgressImg.fillAmount = GlobalKeyValue.GetFloatDataByKey (buildConfigInfo, "Speed");
			earnProgressImg.fillAmount = GlobalKeyValue.GetFloatDataByKey (buildConfigInfo, "Earnings");
		}
		else{
            icon.SetActive(false);

            grayImg.gameObject.SetActive(true);
			questionMarkText.gameObject.SetActive(true);

            pricePath = 3;

            buyBtnText.text = needBuildLv.ToString();
			buildNameText.text = "???????";

			speedProgressImg.fillAmount = 0;
			earnProgressImg.fillAmount = 0;
		}
		UpdateDiscount ();
        priceImg.sprite = priceSps[pricePath];
    }


    private void UpdateCoinBtn(){
        double priceX = canvasManager.GetDoubleDataByKey(buildConfigInfo, "PriceX");
        double price = canvasManager.GetDoubleDataByKey(buildConfigInfo, "Price");
        int unit = canvasManager.GetIntDataByKey(buildConfigInfo, "PriceUnit");

        if (buyNum != 0)
			price = price * Math.Pow(priceX, buyNum) * (1 - DataManager.buffCutPrice);

        priceBigNumber.SetNumber(price, unit);
	
        buyBtn.interactable = !priceBigNumber.IsBiggerAndNotEquals(DataManager.coins);
        buyBtnText.text = priceBigNumber.ToString();
    }

	public void BuyBuildCallBack()
	{
		Debug.Log ("BuyBuildCallBack " + buildLv + ": " + payType);
        upLv = FreeUp.GetFreeUpLv(buildLv, payType);
		int emptyBlockId = blocksManager.GetEmptyBlockId();

		if (emptyBlockId != -1) {
			if (upLv > 0) {

				canvasManager.buildingFreeUp.SetActive (true);
				canvasManager.buildingFreeUp.transform.GetComponent<BuildFreeUpView> ().InitData (buildLv, upLv, buyNum, buildModel, priceBigNumber, payType);
			
			} else {
				if (payType == 2) {
					ShowRewardedAd ();
				} else {
					buyNum = buyNum + blocksManager.CreatBuildingByCoin (priceBigNumber, buildModel, buildLv, payType);
				}

				//soundManager.SoundPayCash ();
            
			}
		} else {
			canvasManager.ShowWarning(ConfigFileMgr.GetContentByKey (EM_LocalizeConstants.Locaize_WarningNoMoreBlock));
		}
        if (payType == 0)
            UpdateCoinBtn();
    }

	public void UpdateDiscount() {
		if (DataManager.buffCutPrice <= 0) {
			discountIcon.SetActive (false);
		} else {
			discountIcon.SetActive (true);
		}
		discountText.text = (100 * DataManager.buffCutPrice) + "%\noff";
	}
  
		
	void OnEnable() {
		upLv = 0;
	}
    
	public void ShowRewardedAd()
	{
		if(!isListen) {
			isListen = true;
			Advertising.RewardedAdSkipped += OnRewardedAdSkipped;
			Advertising.RewardedAdCompleted += OnRewardedAdCompleted;
		}
		canvasManager.ShowRewardAd ();
	}

	void OnRewardedAdCompleted(RewardedAdNetwork arg1, AdLocation arg2)
	{
		if (DataManager.getMaxBuildLv >= 8) {
			planeGameManager.UpdateDailyTask ("1901");
		}

		PlaneGameManager.isAd = false;
		isListen = false;
		DataManager.isFree = 0;

		Advertising.RewardedAdSkipped -= OnRewardedAdSkipped;
		Advertising.RewardedAdCompleted -= OnRewardedAdCompleted;

		buyNum = buyNum + blocksManager.CreatBuildingByCoin(priceBigNumber, buildModel, buildLv, payType);
		payType = 0;
		priceImg.sprite = priceSps[1];
		buyBtn.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/BtnCoin");
		buyBtnText.text = priceBigNumber.ToString();
		buyBtn.interactable = !priceBigNumber.IsBiggerAndNotEquals(DataManager.coins);
	}

	void OnRewardedAdSkipped(RewardedAdNetwork arg1, AdLocation arg2)
	{
		PlaneGameManager.isAd = false;
		isListen = false;
		Advertising.RewardedAdSkipped -= OnRewardedAdSkipped;
		Advertising.RewardedAdCompleted -= OnRewardedAdCompleted;
	}

	void OnDisable() {
		if (isListen) {
			Advertising.RewardedAdSkipped -= OnRewardedAdSkipped;
			Advertising.RewardedAdCompleted -= OnRewardedAdCompleted;
		}
	}
}
