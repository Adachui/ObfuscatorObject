using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyMobile;

public class DiamondItemView : AppAdvisoryHelper
{
    public Image Icon;
    public Text DiamondValueText;
    public Text PriceText;
	public Text BonusText;
	public GameObject Bonus;
	public GameObject Hot;

	private BigNumber diamondValue = new BigNumber();
	private string price;
	private bool isHot = false;
	private double bonus = 0;

	public void InitData(Dictionary<string, string> info,bool hot = false){
		diamondValue.SetNumber (GlobalKeyValue.GetDoubleDataByKey (info, "Diamond"), 0);
		price = info ["Price"];
		bonus = GlobalKeyValue.GetDoubleDataByKey (info, "Bonus");
		isHot = hot;
		InitView ();
    }

	private void InitView() {
		//Icon.sprite = Resources.Load("Image/pic" + iconName, typeof(Sprite)) as Sprite;
		DiamondValueText.text = diamondValue.ToString ();
		PriceText.text = "$" + price.ToString();
	
		if (bonus <= 0) {
			Bonus.SetActive (false);
		}

		Hot.SetActive (isHot);

		BonusText.text = int.Parse((bonus * 100).ToString()).ToString() + "%";
	}

    public void BuyDiamond(){
//		soundManager.SoundUIClick ();
		canvasManager.Loading.SetActive (true);

		if (InAppPurchasing.IsInitialized ()) {
			
			PlaneGameManager.isAd = true;
			string product = ((int)diamondValue.number).ToString () + " Diamonds Pack";
			InAppPurchasing.Purchase (product);
		} else {
//			canvasManager.ShowWarning ("No Internet");
		}

		Debug.Log ("BuyDiamond == " + diamondValue.ToString ());
    }
}
