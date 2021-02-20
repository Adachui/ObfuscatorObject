using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyMobile;

public class BuildingUnlockVIew : AppAdvisoryHelper {
	public Text name;
	public Image progressSpeed;
	public Image progressIncome;
	public GameObject left;
	public GameObject right;
	public GameObject center;
	Dictionary<string, string> buildConfigInfo;

	private int uplv = 1;
	private bool upNb = false;
	private bool upNp = false;
	private bool isShowLvUp = false;
	private bool isShowCon = false;

	public void InitView(int level) {
		if (level >= 5) {
			CileadTrace.RecordEvent (EventConst.BUILDINGLEVEL + level.ToString ());
		}
		buildConfigInfo = GlobalKeyValue.GetXmlDataByKey("BuildInfo", "Build" + level);
		name.text = ConfigFileMgr.GetContentByKey ("Building" + level,name);//buildConfigInfo["BuildName"];
		progressSpeed.fillAmount = GlobalKeyValue.GetFloatDataByKey (buildConfigInfo, "Speed");
		progressIncome.fillAmount = GlobalKeyValue.GetFloatDataByKey (buildConfigInfo, "Earnings");

		//left
		left = Resources.Load ("Prefabs/BuildIcon/BICON" + (level - 1)) as GameObject;
		left = Instantiate (left);
		left.transform.parent = transform;
		left.transform.localPosition = new Vector3 (-300f, 148f, -200f);
		left.transform.GetComponent<BuildIcon> ().imgSize = new Vector2 (350, 350);
		left.GetComponent<Animator>().SetBool("left", true);

		//right
		right = Resources.Load ("Prefabs/BuildIcon/BICON" + (level - 1)) as GameObject;
		right = Instantiate (right);
		right.transform.parent = transform;
		right.transform.localPosition = new Vector3 (0f, 148f, -200f);
		right.transform.GetComponent<BuildIcon> ().imgSize = new Vector2 (350, 350);
		right.GetComponent<Animator>().SetBool("right", true);

		//center
		center = Resources.Load ("Prefabs/BuildIcon/BICON" + level) as GameObject;
		center = Instantiate (center);
		center.transform.parent = transform;
		center.transform.localPosition = new Vector3 (-140, 168f, -200f);
		center.transform.GetComponent<BuildIcon> ().imgSize = new Vector2 (400, 400);
		center.transform.GetComponent<BuildIcon> ().img.GetComponent<Canvas> ().overrideSorting = true;
		center.transform.GetComponent<BuildIcon> ().img.GetComponent<Canvas> ().sortingOrder = 2;
		center.GetComponent<Animator>().SetBool("center", true);

		if (DataManager.getMaxBuildLv == 8) {
			isShowCon = true;
		}

		if (DataManager.getMaxBuildLv == 10) {
			isShowCon = true;
		}
	}

	public void InitLvUp(int lv, bool newB, bool newP) {
		uplv = lv;
		upNb = newB;
		upNp = newP;
		isShowLvUp = true;
	}

	public void ShowLvUp() {
		if (isShowLvUp) {
			canvasManager.lvUpView.SetActive (true);
			canvasManager.lvUpView.transform.GetComponent<LevelUpView> ().InitView (
				DataManager.lv, upNb, upNp, isShowCon);
		}
	}

	void OnEnable() {
		canvasManager.isUILayer++;
		isShowLvUp = false;
		uplv = 1;
		upNb = false;
		upNp = false;
		isShowCon = false;
	}

	void OnDisable() {
		canvasManager.isUILayer--;
		Destroy (left);
		Destroy (right);
		Destroy (center);

		if (isShowCon && !isShowLvUp) {
			if (DataManager.getMaxBuildLv == 8) {
				canvasManager.ShowCongratulationViewWithIcon (
					Resources.Load<Sprite> ("Sprites/daily"),
					ConfigFileMgr.GetContentByKey(EM_LocalizeConstants.Locaize_UnlockQuest),
					1.8f
				);
				return;
			}

			if (DataManager.getMaxBuildLv == 10) {
				canvasManager.ShowCongratulationViewWithIcon (
					Resources.Load<Sprite> ("Sprites/tp1"),
					ConfigFileMgr.GetContentByKey(EM_LocalizeConstants.Locaize_UnlockBoosts),
					1.8f
				);
				return;
			}
		}

		if (!isShowCon && !isShowLvUp) {
			if (DataManager.GetSpecialOfferTimeLeft () <= 0 && ManagerUserInfo.GetIntDataByKey ("BuyDiscount") != 1) {
				if (DataManager.getMaxBuildLv >= 6) {
					canvasManager.ShowCongratulationView (7);
					DataManager.SetSpecialOfferTime (DataManager.ConvertDateTimeToLong (System.DateTime.Now));
				}
			} else {
				if (DataManager.getMaxBuildLv > 6) {
					if (StoreReview.CanRequestRating ()) {
						Sprite[] icons = new Sprite[6];
						icons = Resources.LoadAll<Sprite> ("Sprites/Congratulation");
						canvasManager.ShowCongratulationViewWithIcon (
							icons [3],
							ConfigFileMgr.GetContentByKey (EM_LocalizeConstants.Locaize_RateGetDiamond20),
							0.6f,
							false,
							canvasManager.CongratulationViewDisableActionBack
						); 
					} else {
						Debug.LogError ("************ CanRequestRating return false ****************");
					}
				}
			}
		}
	
		//新手引导
		if (ManagerUserInfo.GetIntDataByKey ("NewPlayerStep2") == 0 ) {
			ManagerUserInfo.SetDataByKey ("NewPlayerStep2", 1);
			guideLayer.GuideStep_2 ();
		}
	}
}
