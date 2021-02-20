using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyMobile;

public class DiamondStoreView : AppAdvisoryHelper {
	public Transform Title;
	public Transform Panel;
	public GameObject Bound;
	public GameObject Discount;
	public GameObject BtnTapClose;
	public GameObject BtnClose2;


	private bool isDiscount = false;
	public static bool isSecoundView = false;
	private int[] picNames = {19, 17, 16, 10, 18, 4};

	void Start () {
		InitiamondStoreData ();
	}
		
	void Update () 
	{
		if(isDiscount && DataManager.GetSpecialOfferTimeLeft() <= 0){
			isDiscount = false;
			FixItemPosition ();
		}
	}

	void InitiamondStoreData() {
		int i = 0;
		DataManager.InitGemInfo ();
		Debug.Log (DataManager.GemConfigList.Count);
		Sprite[] itemSps = new Sprite[20];
		itemSps = Resources.LoadAll<Sprite>("Sprites/diamond_items_");
		foreach (Dictionary<string, string> infoDic in DataManager.GemConfigList) {
			GameObject item = Resources.Load("Prefabs/UI/DiamondItem") as GameObject;
			item = Instantiate(item);
			item.transform.SetParent(Panel,false);
			item.transform.localScale = new Vector3(1, 1, 1);
			item.GetComponent<DiamondItemView> ().InitData (infoDic);
			item.transform.GetChild (0).GetComponent<Image> ().sprite = itemSps [picNames[i]];
			i++;
		}
	}

	void FixItemPosition() {
		if (!isDiscount) {
			Title.localPosition = new Vector3 (0, 300, 0); 
			Panel.localPosition = new Vector3 (0, 10, 0); 
			BtnClose2.transform.localPosition =  new Vector3 (255, 345, 0); 
			BtnClose2.SetActive (true);
			Bound.SetActive (true);
			Discount.SetActive (false);
			BtnTapClose.SetActive (true);
		} else {
			Title.localPosition = new Vector3 (0, 107, 0); 
			Panel.localPosition = new Vector3 (0, -150, 0); 

			BtnClose2.SetActive (false);
			Bound.SetActive (false);
			Discount.SetActive (true);
			BtnTapClose.SetActive (false);
		}
	}


	void OnEnable()
	{
		Debug.Log ("DiamondStoreView OnEnable");
		Debug.Log ("isOtherLayerOpen  " + canvasManager.isOtherLayerOpen);
		Debug.Log ("ExchangeView  " + ExchangeView.isSecoundView);
		Debug.Log ("isUILayer  " + canvasManager.isUILayer);
		//isOtherLayerOpen 为钻石或者兑换界面
		if (canvasManager.isOtherLayerOpen) {
			isSecoundView = ExchangeView.isSecoundView;
			// 兑换和钻石商城界面是互斥的 一个打开另一个就关闭
		} else {
			if (canvasManager.isUILayer > 0) {
				isSecoundView = true;
			} else {
				isSecoundView = false;

			}
		}
		canvasManager.isUILayer++;
		canvasManager.isOtherLayerOpen = true;

		if (DataManager.GetSpecialOfferTimeLeft () > 0) {
			isDiscount = true;
		}

		FixItemPosition ();
	}

	void OnDisable()
	{
		Debug.Log ("DiamondStoreView OnDisable");
		if (canvasManager.Loading.activeSelf) {
			canvasManager.Loading.SetActive (false);
		}
	}

	public void CloseBtnCallBack() {
		Debug.Log ("DiamondStoreView CloseBtnCallBack");
		canvasManager.isUILayer--;
		if (!isSecoundView) {
			canvasManager.coinTextLeft.SetActive (false);
		}
		if (canvasManager.isShowOfflinewView) {
			canvasManager.coinTextLeft.SetActive (false);
		}
		gameObject.SetActive (false);
		canvasManager.isOtherLayerOpen = false;
	}
}
