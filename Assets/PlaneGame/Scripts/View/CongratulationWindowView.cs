using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CongratulationWindowView : AppAdvisoryHelper {
	public Image icon;
	public Text des;
	private int type;
	public GameObject commonReward;
	public GameObject vipReward;
	public GameObject frameIcon;
	public GameObject specialOfferObj;
	public GameObject effectObj;

	private Action<int> callback = null;

	public string[] desString = { 
		"Get 4 hours worth of profit!",
		"Get 1 day worth of profit!",
		"Get 4 gift box!",
		"Get Diamonds X20",
		"Income Up X5",
		"Speed Up X2",
	};

	public void OnEnable() {
		type = 0;
		commonReward.SetActive (true);
		vipReward.SetActive (false);
		specialOfferObj.SetActive (false);
		//icon.rectTransform.sizeDelta = new Vector2 (230, 250);
		icon.SetNativeSize ();
	}
	
	public void InitWithType(int _type) {
		if (_type == 6) {
			commonReward.SetActive (false);
			vipReward.SetActive (true);
			specialOfferObj.SetActive (false);
			effectObj.SetActive (true);
		} else if (_type == 7) {
			commonReward.SetActive (false);
			vipReward.SetActive (false);
			effectObj.SetActive (false);
			specialOfferObj.SetActive (true);
		}
	}

	public void OnDisable() {
		if (type == 3) {
			canvasManager.ShowDiamondAnimation ();
		}

		if (callback != null)
			callback (1);
	}

	public void InitWithIcon(Sprite sp, string description, float scale, bool hasFrame,Action<int> type) {
		icon.sprite = sp;
		des.text = description;
		var font = i18n.GetTextFont ();
		if(font != null)
			des.font = font;
		
		//icon.rectTransform.sizeDelta = new Vector2(w ,y);
		icon.SetNativeSize ();
		if (scale != null)
			icon.rectTransform.localScale = new Vector3(scale,scale,scale);
		/*if (hasFrame) {
			frameIcon.SetActive (true);
		} else {
			frameIcon.SetActive (false);
		}*/

		effectObj.SetActive (true);
		callback = type;
	}
}
