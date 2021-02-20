using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyMobile;

public class GemShopView : AppAdvisoryHelper
{
    public Text discountLeftTime;
    private float leftTime = 0;

	void OnEnable() {
		InitDiscount ();
	}

	void Update () {
        leftTime -= Time.deltaTime;
        discountLeftTime.text = DataManager.SecondToHours(leftTime);
    }
		
    private void InitDiscount(){
        discountLeftTime.text = "24:00:00";
		leftTime = 86400 - (float)DataManager.GetSpecialOfferTimeLeft ();
    }

    public void BuyDiscount(){
		canvasManager.Loading.SetActive (true);
		DataManager.isSpecialPurchase = true;
		InAppPurchasing.Purchase (EM_IAPConstants.Product_2550_Diamonds_Pack);
    }
}
