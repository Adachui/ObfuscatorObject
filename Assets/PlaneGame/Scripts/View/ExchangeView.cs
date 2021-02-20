using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExchangeView : AppAdvisoryHelper {
	public Text exchangeNumText;
	public Text useCoinsText;
	public Button exchangeBtn;
	public ParticleSystem effect1;
	public GameObject effect2;
	private BigNumber exchangeNum = new BigNumber(0 , 1);
	private BigNumber min = new BigNumber(0 , 1);
	private BigNumber one = new BigNumber(1 , 1);
	private BigNumber useCoinsNum = new BigNumber(0 , 1);
	public static bool isSecoundView = false;
	// Use this for initialization
	void Start () {
		Min ();
	}
	
	// Update is called once per frame
	void Update () {
		if (exchangeNum.IsBiggerAndNotEquals (min)) {
			exchangeBtn.enabled = true;
		} else {
			exchangeBtn.enabled = false;
		}
	}

	public void Add() {
		exchangeNum.Add (one);
		if (Check ()) {
			exchangeNumText.text = exchangeNum.ToString ();
		} else {
			exchangeNum.Minus (one);
		}
		SetUseCoinsText ();
	}

	public void Minus() {
		exchangeNum.Minus (one);
		if (exchangeNum.IsBiggerThan (min)) {
			exchangeNumText.text = exchangeNum.ToString ();
		} else {
			exchangeNum.Add (one);
		}
		SetUseCoinsText ();
	}

	public void Max() {
		if (DataManager.coins.number / 10000 < 1 && DataManager.coins.units >= 2) {
			exchangeNum.SetNumber (new BigNumber ((int)(DataManager.coins.number / 10), DataManager.coins.units - 1));
		} else {
			exchangeNum.SetNumber (new BigNumber ((int)(DataManager.coins.number / 10000), DataManager.coins.units));
		}
		exchangeNumText.text = exchangeNum.ToString ();
		SetUseCoinsText ();
	}

	public void Min() {
		exchangeNum.SetNumber (0.0, 0);
		exchangeNumText.text = exchangeNum.ToString ();
		SetUseCoinsText ();
	}

	private bool Check() {
		useCoinsNum.SetNumber (exchangeNum.number * 10, exchangeNum.units + 1);
		return DataManager.coins.IsBiggerThan (useCoinsNum);
	}

	private void SetUseCoinsText() {
		soundManager.SoundUIClick ();
		useCoinsNum.SetNumber (exchangeNum.number * 10, exchangeNum.units + 1);
		useCoinsText.text = useCoinsNum.ToString ();
	}

	public void Exchange() {
		soundManager.SoundUIClick ();
		effect1.Play ();
		effect2.SetActive (true);
		canvasManager.UpdateCoin("-",new BigNumber (exchangeNum.number * 10000, exchangeNum.units));
		canvasManager.UpdatePathCoin("+", exchangeNum);
		Min ();
		StartCoroutine (EffectHide ());
	}

	IEnumerator EffectHide() {
		soundManager.SoundCurrencyExchange ();
		yield return new WaitForSeconds (2.5f);
		effect2.SetActive (false);
	}

	void OnEnable()
	{
		Debug.Log ("ExchangeView OnEnable");
		Debug.Log ("isOtherLayerOpen  " + canvasManager.isOtherLayerOpen);
		Debug.Log ("DiamondStoreView  " + DiamondStoreView.isSecoundView);
		Debug.Log ("isUILayer  " + canvasManager.isUILayer);
		//isOtherLayerOpen 为钻石或者兑换界面
		if (canvasManager.isOtherLayerOpen) {
			isSecoundView = DiamondStoreView.isSecoundView;
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
	}

	void OnDisable()
	{
		canvasManager.isUILayer--;
		canvasManager.isOtherLayerOpen = false;
		if (!isSecoundView) {
			canvasManager.coinTextLeft.SetActive (false);
		}
		if (canvasManager.isOnShop) {
			shopManager.gameObject.SetActive (false);
			canvasManager.coinTextLeft.SetActive (false);
		}
		if (canvasManager.isShowOfflinewView) {
			canvasManager.coinTextLeft.SetActive (false);
		}
	}
}
