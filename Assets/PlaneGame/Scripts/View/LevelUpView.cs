using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyMobile;

public class LevelUpView : AppAdvisoryHelper {
	public Text lvText;
	public GameObject reawrdBlock;
	public GameObject reawrdPath;
	public GameObject[] lvReward;
	public Image progress;
	private static int[] rlv = {1, 3, 5 ,10};
	private int level = 1;
	private bool isShowCon = false;
	// Use this for initialization
//	void Start () {
//		
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		
//	}

	void OnEnable() {
		isShowCon = false;
		soundManager.SoundLevelUp ();
		level = 1;
	}
		
	void OnDisabel() {
		if (isShowCon) {
			if (isShowCon) {
				if (DataManager.getMaxBuildLv == 8) {
					canvasManager.ShowCongratulationViewWithIcon (
						Resources.Load<Sprite> ("Sprites/daily"),
						ConfigFileMgr.GetContentByKey(EM_LocalizeConstants.Locaize_UnlockQuest),
						1.8f
					);
				}

				if (DataManager.getMaxBuildLv == 10) {
					canvasManager.ShowCongratulationViewWithIcon (
						Resources.Load<Sprite> ("Sprites/tp1"),
						ConfigFileMgr.GetContentByKey(EM_LocalizeConstants.Locaize_UnlockBoosts),
						1.8f
					);
				}
			}
		}
	}
	public void InitView(int lv, bool newbolck, bool newpath, bool isShowCon) {
		this.isShowCon = isShowCon;
		level = lv;
		int l = lv / 10;
		for (int i = 0; i < lvReward.Length; i++) {
//			if (i != 0 && PlayerPrefs.GetInt("LVReward" + (rlv[i] + 10*l), 0) == 0) {
//				lvReward [i].transform.GetChild (0).gameObject.SetActive (true);
//			}
			lvReward [i].transform.GetChild (1).GetComponent<Text>().text = "Lv." + (rlv[i] + 10*l);
		}
		lvText.text = DataManager.lv.ToString();
		progress.fillAmount = ((float)DataManager.lv) / (DataManager.lv / 10 * 10 + 10);
		if (lv > 10) {
			reawrdBlock.SetActive (false);
		}
		if (lv > 9) {
			reawrdPath.SetActive (false);
		}
	}

	public void GetReward(int i) {
		int l = level / 10;
		if(DataManager.lv >= rlv [i] + 10 * l && PlayerPrefs.GetInt("LVReward" + (rlv[i] + 10*l), 0) == 0 && i != 0) {
			PlayerPrefs.SetInt ("LVReward" + (rlv [i] + 10 * l), 1);
			canvasManager.UpdateDiamond ("+" , new BigNumber(3, 0));
			//lvReward [i].transform.GetChild (0).gameObject.SetActive (false);
			canvasManager.ShowDiamondAnimation ();
			soundManager.SoundRewardClaim ();
		}
	}

	public void GetAllReward() {
		for (int i = 0; i < 4; i++) {
			GetReward (i);
		}
	}
}
