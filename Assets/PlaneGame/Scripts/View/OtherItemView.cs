using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyMobile;

public class OtherItemView : AppAdvisoryHelper
{
    public Text pathNameText;
    public Text descriptionText;
    public Text priceText;

    public Image priceImg;
    public Image iconImg;
    public Image lockImg;
    public Image hadImg;
    public Image grayImg;

    public Button buyBtn;

    public BigNumber priceBigNumber = new BigNumber();

    private float buffValue = 0;
    ModelBuff buffModel;
    // Use this for initialization
//    void Start()
//    {
//
//    }
//
//    // Update is called once per frame
//    void Update()
//    {
//
//    }

    public override void InitModelData(IModel _model)
    {

        buffModel = _model as ModelBuff;
        Dictionary<string, string> buffInfo = GlobalKeyValue.GetXmlDataByKey("BuffInfo", "Buff" + buffModel.BuffId);

		string buffName = buffInfo ["BuffName"];
		if (buffName.Contains (EM_LocalizeConstants.Locaize_Runway))
			buffName = buffName.Replace (EM_LocalizeConstants.Locaize_Runway,ConfigFileMgr.GetContentByKey(EM_LocalizeConstants.Locaize_Runway));
		else
			buffName = buffName.Replace (EM_LocalizeConstants.Locaize_Discount,ConfigFileMgr.GetContentByKey(EM_LocalizeConstants.Locaize_Discount));

		pathNameText.text = buffName;
		var font = i18n.GetTextFont ();
		if(font != null)
			pathNameText.font = font;

        lockImg.gameObject.SetActive(false);
        hadImg.gameObject.SetActive(false);
        grayImg.gameObject.SetActive(false);
        priceText.gameObject.SetActive(true);
        buyBtn.enabled = true;
		buyBtn.interactable = true;
        priceText.transform.localPosition = new Vector3(0,0,0);

        buffValue = GlobalKeyValue.GetFloatDataByKey(buffInfo, "BuffValue");
        float buffNum = buffValue * 100;
        string buffText = "";

        Sprite[] equipSps = new Sprite[10];
        switch (buffModel.BuffType)
        {
            case 1:
				buffText = ConfigFileMgr.GetContentByKey (EM_LocalizeConstants.Locaize_EarningsInc,descriptionText);
				descriptionText.text = string.Format(buffText, buffNum);
				equipSps = Resources.LoadAll<Sprite>("Sprites/Runaway");

//                buffText = "Earnings increase\n";
//                equipSps = Resources.LoadAll<Sprite>("Sprites/Runaway");
//				descriptionText.text = buffText + buffNum + "%";
                break;
            case 2:
				buffText = ConfigFileMgr.GetContentByKey (EM_LocalizeConstants.Locaize_BuildingOff,descriptionText);
				descriptionText.text = string.Format(buffText, buffNum);
				equipSps = Resources.LoadAll<Sprite>("Sprites/Discount");
//				buffText = "All building\n";
//                equipSps = Resources.LoadAll<Sprite>("Sprites/Discount");
//				descriptionText.text = buffText + buffNum + "% off";
                break;
        }

        iconImg.sprite = equipSps[buffModel.BuffLv - 1];

        switch (buffModel.BuffState){
            //可购买
            case 0:
				priceImg.gameObject.SetActive(true);
                priceText.transform.localPosition = new Vector3(10, 0, 0);
                double price = canvasManager.GetDoubleDataByKey(buffInfo, "Buff_price");
                int unit = canvasManager.GetIntDataByKey(buffInfo, "Units");
                priceBigNumber.SetNumber(price, unit);
                priceText.text = priceBigNumber.ToString();
//                buyBtn.enabled &= !priceBigNumber.IsBiggerAndNotEquals(DataManager.pathCoins);
				buyBtn.interactable &= !priceBigNumber.IsBiggerAndNotEquals(DataManager.pathCoins);
                break;
            //已拥有
			case 1:
				hadImg.gameObject.SetActive (true);
				priceText.text = ConfigFileMgr.GetContentByKey (EM_LocalizeConstants.Locaize_Owned,priceText) ;
				buyBtn.enabled = false;
                break;
            //未解锁
            case 2:
                grayImg.gameObject.SetActive(true);
                lockImg.gameObject.SetActive(true);
                priceImg.gameObject.SetActive(false);
                priceText.gameObject.SetActive(false);
				buyBtn.enabled = false;
                break;
        }
    }

    public void BuyBtnCallBack(){
		soundManager.SoundPayCash ();
		priceText.text = ConfigFileMgr.GetContentByKey (EM_LocalizeConstants.Locaize_Owned,priceText);
        hadImg.gameObject.SetActive(true);
        priceImg.gameObject.SetActive(false);

        canvasManager.UpdatePathCoin("-", priceBigNumber);

        switch (buffModel.BuffType)
        {
		case 1:
				DataManager.equipmentId1 = buffModel.BuffLv - 1;
				ManagerUserInfo.SetDataByKey ("equipmentId1", DataManager.equipmentId1);
				float buff = DataManager.buffIncomeBase + buffValue;
				if (PlaneGameManager.isIncomeUp)
					buff += 5;
				DataManager.buffIncome = buff;
                ManagerUserInfo.SetDataByKey("IncomeBuffNum", DataManager.buffIncome);
                break;
			case 2:
				DataManager.equipmentId2 = buffModel.BuffLv - 1;
				ManagerUserInfo.SetDataByKey ("equipmentId2", DataManager.equipmentId2);
				DataManager.buffCutPrice = buffValue;
				ManagerUserInfo.SetDataByKey ("CutPriceBuffNum", DataManager.buffCutPrice);
				canvasManager.InitShowBuild ();
	            break;
        }
        DataManager.InitBuffInfo();
		canvasManager.ShowCongratulationViewWithIcon (
			iconImg.sprite,
			descriptionText.text,
			1f,
			true
		);
		shopManager.gameObject.SetActive (false);
		canvasManager.coinTextLeft.SetActive (false);
    }
}
