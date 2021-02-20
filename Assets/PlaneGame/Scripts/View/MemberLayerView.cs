using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyMobile;
using UnityEngine.UI;

public class MemberLayerView : AppAdvisoryHelper {
	public GameObject effectBtn;
	public RectTransform content;
	public GameObject privacyPolicy;
	public GameObject appstoreBtn;
	public GameObject googleplayBtn;
	public GameObject vipMemberBtn;
	public GameObject restoreBtn;

	public GameObject itemPrivatePolicy;
	// Use this for initialization
	void OnEnable() {
		content.localPosition = new Vector3 (0, 0, 0);
		#if UNITY_EDITOR
			privacyPolicy.SetActive(true);
		#elif UNITY_ANDROID
			privacyPolicy.SetActive(true);
		#else
			privacyPolicy.SetActive(true);
		#endif
	}

	void Start () {
		if (Application.platform == RuntimePlatform.Android) {
			appstoreBtn.SetActive (false);
			itemPrivatePolicy.SetActive (false);
		} else {
			googleplayBtn.SetActive (false);
		}
	}

	// Update is called once per frame
	void Update () 
	{
		if (canvasManager.isOtherLayerOpen) {
			if (effectBtn.activeSelf) {
				effectBtn.SetActive (false);
			}
		} else {
			if (!effectBtn.activeSelf) {
				effectBtn.SetActive (true);
			}
		}

		if (!vipMemberBtn.activeSelf && InAppPurchasing.isSubscribedOwned () && DataManager.isGoldVip == 1) {
			vipMemberBtn.SetActive (true);
			googleplayBtn.SetActive (false);
			appstoreBtn.SetActive (false);
			restoreBtn.SetActive (false);
		} 
	}

	public void SubscribeGoldVip() {
		canvasManager.Loading.SetActive (true);
		canvasManager.memberSubClicked = true;
		PlaneGameManager.isAd = true;
		InAppPurchasing.Purchase (EM_IAPConstants.Product_Gold_Membership);
	}

	public void OpenPrivacyPolicyURL() {
		Application.OpenURL ("http://www.arabbaramj.com/terms.html");
		Application.OpenURL ("http://www.arabbaramj.com/policy.html");
	}

	public void RestoreVip()
	{
		canvasManager.Loading.SetActive (true);
		InAppPurchasing.RestorePurchases ();
	}
}
