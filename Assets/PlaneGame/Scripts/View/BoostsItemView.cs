using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyMobile;

public class BoostsItemView : AppAdvisoryHelper {
	public GameObject DiamondStore;
	public Text TitleText;
	public Text GetText;
	public Text DiamondNeedText;
	public Text DescriptionTexts;
	public double diamond = 0;
	public int time = 0;
	// Use this for initialization
	private BigNumber income;
	void OnEnable () {
		string timeText = Hours2Day ();
		TitleText.text = timeText + ConfigFileMgr.GetContentByKey (EM_LocalizeConstants.Locaize_TimeWerp,TitleText);// "Time Warp";
		income = new BigNumber (DataManager.incomeSec.number * time * 3.6 * DataManager.buffIncome, DataManager.incomeSec.units + 1);
		Debug.Log ("income Time = " + "" + DataManager.incomeSec.number * time * 3.6);
		Debug.Log(string.Format("time {0} incomeSec.number {1} incomeSec.units {2} all {3}", 
			time, DataManager.incomeSec.number, income.units, DataManager.incomeSec.number * time * 3.6
		));
		GetText.text = income.ToString ();
		Debug.Log (income.ToString ());
		DiamondNeedText.text = int.Parse (diamond.ToString ()).ToString();
		DescriptionTexts.text = string.Format (ConfigFileMgr.GetContentByKey(EM_LocalizeConstants.Locaize_BoostShopTip,DescriptionTexts),timeText);
	}

	string Hours2Day() {
		if (time >= 24) {
			int day = time / 24;
			return day > 1 ? day + ConfigFileMgr.GetContentByKey (EM_LocalizeConstants.Locaize_Manydays): day + ConfigFileMgr.GetContentByKey (EM_LocalizeConstants.Locaize_Oneday);
		} else {
			return time > 1 ? time + ConfigFileMgr.GetContentByKey (EM_LocalizeConstants.Locaize_Manyhours): time + ConfigFileMgr.GetContentByKey (EM_LocalizeConstants.Locaize_Onehour);
		}
	}
		
	public void GetCoins() {
		BigNumber need = new BigNumber(diamond, 0);
		if (income.number - 0 < 0.01) {
			soundManager.SoundUIClick ();
			canvasManager.ShowWarning(ConfigFileMgr.GetContentByKey (EM_LocalizeConstants.Locaize_NoProfit));
			return;
		}

		if (DataManager.diamond.IsBiggerThan (need)) {
			soundManager.SoundPayCash ();
			canvasManager.UpdateCoin("+", income);
			canvasManager.UpdateDiamond("-", need);
			int i = 1;
			if (time < 24) {
				i = 1;
				canvasManager.ShowCongratulationViewWithIcon (
					Resources.Load<Sprite> ("Sprites/tp" + i),
					DescriptionTexts.text,
					1.8f
				);
			} else if (time == 24) {
				i = 2;
				canvasManager.ShowCongratulationViewWithIcon (
					Resources.Load<Sprite> ("Sprites/tp" + i),
					DescriptionTexts.text,
					1.6f
				);
			} else if(time > 24) {
				i = 3;
				canvasManager.ShowCongratulationViewWithIcon (
					Resources.Load<Sprite> ("Sprites/tp" + i),
					DescriptionTexts.text,
					1.6f
				);
			}

			soundManager.SoundRewardClaim ();
		} else {
			DiamondStore.SetActive (true);
		}
	}
}
