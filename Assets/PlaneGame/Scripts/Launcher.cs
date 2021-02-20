using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using EasyMobile;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviour {
	
	InterstitialAd _interstitialAd;

	bool CoroutineSwitch = false;

	void Start () {
//		Debug.unityLogger.logEnabled = false;
		InAppPurchasing.InitializePurchasing();
		GameServices.ManagedInit ();
		//AppsflyerMgr.Initialize ();
		StartShowSplashInterstitialAd ();
	}
		
	//加载开机插屏广告
	public void LoadSplashInterstitial() 
	{
		string adUnitId = "unexpected_platform";
		#if UNITY_ANDROID
		adUnitId = "ca-app-pub-2332937665226737/1629091269";
		#elif UNITY_IPHONE
		adUnitId = "ca-app-pub-2332937665226737/1565458188";
		#else
		adUnitId = "unexpected_platform";
		#endif

		_interstitialAd = new InterstitialAd(adUnitId);
		_interstitialAd.OnAdClosed += HandleAdMobInterstitialClosed;

		var builder = new AdRequest.Builder();
		var settings = EM_Settings.Advertising;
		var request = builder.Build();
		_interstitialAd.LoadAd(request);
	}

	//展示开机插屏广告
	IEnumerator ShowSplashInterstitialAd()
	{
		yield return new WaitForSeconds (3.0f);
		if (_interstitialAd.IsLoaded ()) {
			_interstitialAd.Show ();
			CileadTrace.RecordEvent (EventConst.SHOWSTARTINTERSTITIAL);
			StartCoroutine (GotoMainScene());
		}
		else {
			SceneManager.LoadScene ("mainScene");
		}
	}
	//点击关闭开机插屏广告
	void HandleAdMobInterstitialClosed(object sender, EventArgs args)
	{
		CoroutineSwitch = true;
		_interstitialAd.Destroy();
		_interstitialAd = null;
	}
	
	void Update()
	{
		if (CoroutineSwitch) {
			SceneManager.LoadScene ("mainScene");
			CoroutineSwitch = false;
		}
	}
	
	//启动展示插屏广告
	public void StartShowSplashInterstitialAd()
	{
		#if UNITY_EDITOR
			SceneManager.LoadScene ("mainScene");
			_interstitialAd = null;
		#else
			if (Advertising.IsAdRemoved ()) {
				SceneManager.LoadScene ("mainScene");
				_interstitialAd = null;
			} else {
				LoadSplashInterstitial();
				StartCoroutine (ShowSplashInterstitialAd());
			}
		#endif
	}

	IEnumerator GotoMainScene(){
		yield return new WaitForSeconds(7.0f);
		SceneManager.LoadScene ("mainScene");
	}
}
