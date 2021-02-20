using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyMobile;

public class Monster : AppAdvisoryHelper {
	public int level = 1;
	public BigNumber value;
	public BigNumber profitSec; 
	public float circleTime = 58f;

	public bool isStart = false;
	public int _index = 0;
	public int index
	{
		get
		{
			return _index;
		}
		set
		{
			_index = value;
		}
	}

	private float startTime = 0f;
	//受击
	private Dictionary<string, List<GameObject>> hitDic = new Dictionary<string, List<GameObject>>();

	//	<BuildID>1001</BuildID>
	//	<BuildLv>1</BuildLv>
	//	<BuildName>House_1Room_Blue</BuildName>
	//	<RunName>SimpleCitizens_Hippie_White</RunName>
	//	<CircleTime>5.8</CircleTime>
	//	<ProfitSec>4.31</ProfitSec>
	//	<Units>0</Units>
	//	<MergeExp>1</MergeExp>
	//	<Price>100.0</Price>
	//	<PriceUnit>0</PriceUnit>
	//	<PriceX>1.07</PriceX>
	//	<UnlockLv>1</UnlockLv>
	//	<DimondPrice>1</DimondPrice>
	//	<Earnings>0.01</Earnings>
	//	<Speed>0.01</Speed>
	//	<ShowLv>1</ShowLv>


	void Start () {
		//动态加载角色；
		GameObject roleMesh =  Resources.Load ("Prefabs/ORCSPrefab/" + level) as GameObject;
		roleMesh = Instantiate (roleMesh);
		roleMesh.transform.parent = transform;
		roleMesh.transform.localPosition = Vector3.zero;
		roleMesh.transform.localScale = new Vector3 (1, 1, 1);
		roleMesh.transform.localRotation = Quaternion.Euler(0f,0f,0f);
//		transform.GetChild (0).gameObject.SetActive (true);
//		transform.GetChild (0).GetChild(0).GetComponent<TrailRenderer>().Clear ();
		InitRoleAnimation ();
//		Sprite[] sp = Resources.LoadAll<Sprite> ("Sprites/path");
//		planeGameManager.paths[DataManager.roleNum].GetComponent<SpriteRenderer>().sprite = sp[1];

		//TrailColor ();

		hitDic["gongjian"] = new List<GameObject>();
		hitDic["huoqiu_1"] = new List<GameObject>();
		hitDic["huoqiu_2"] = new List<GameObject>();
		hitDic["huoqiu_3"] = new List<GameObject>();
		hitDic["jiguang_1"] = new List<GameObject>();
		hitDic["jiguang_2"] = new List<GameObject>();
		hitDic["jiguang_3"] = new List<GameObject>();
		hitDic["shandian_1"] = new List<GameObject>();
		hitDic["shandian_2"] = new List<GameObject>();
		hitDic["shandian_3"] = new List<GameObject>();
	}

	void TrailColor() {
		Color[] colors = {
			Color.blue,
			Color.yellow,
			new Color(0, 0.5f, 0.5f, 1),
			Color.green,
			Color.magenta,
		};

		Gradient gradient;
		GradientColorKey[] colorKey;
		GradientAlphaKey[] alphaKey;
		gradient = new Gradient();
		// Populate the color keys at the relative time 0 and 1 (0 and 100%)
		colorKey = new GradientColorKey[2];
		colorKey[0].color = colors[level%5-1 >=0 ? level%5-1 : 4];
		colorKey[0].time = 0.0f;
		colorKey[1].color = colors[level%5-1 >=0 ? level%5-1 : 4];
		colorKey[1].time = 1.0f;
		alphaKey = new GradientAlphaKey[2];
		alphaKey[0].alpha = 1.0f;
		alphaKey[0].time = 0.0f;
		alphaKey[1].alpha = 0.8f;
		alphaKey[1].time = 1.0f;
		gradient.SetKeys(colorKey, alphaKey);
		transform.GetChild (0).GetChild(0).GetComponent<TrailRenderer>().colorGradient = gradient;
		Debug.Log (DataManager.roleNum + " / " + DataManager.roleMaxNum);
	}
	void InitRoleAnimation() {


		transform.GetComponent<Animator> ().speed = 0.8f;

	}


	public void ShowHitEffect(string name){
		int length = hitDic[name].Count;
		bool isExit = false;
		GameObject needHit = null;
		ParticleSystem trashSmokeEffect = null;
		if (length > 0) {
			for (int i = 0; i < length; i++) {
				GameObject curHit = hitDic[name][i];
				trashSmokeEffect = curHit.transform.GetChild(0).GetComponent<ParticleSystem>();
				if(!trashSmokeEffect.isPlaying){
					isExit = true;
					//needHit = curHit;
					break;
				}
			}
		}

		if (!isExit) {
			needHit = Resources.Load ("Prefabs/Defend/Attack/" + name + "_shouji") as GameObject;
			needHit = Instantiate (needHit);
			needHit.transform.parent = transform;
			needHit.transform.localPosition = new Vector3 (0, 2.5f, 0);
			needHit.transform.localScale = new Vector3 (1, 1, 1);
			hitDic [name].Add (needHit);
		} else {
			trashSmokeEffect.Play ();
		}
	}


	void Update () {
		if (isStart) {
			if (startTime < 1f && isStart) {
				startTime += Time.deltaTime;
			} else {
				isStart = false;
				transform.GetComponent<Animator>().speed = 5.8f/ circleTime;
				//Debug.Log (5.8f / circleTime);
			}
		} else {
//			if (PlaneGameManager.isSpeedUp) {
//				if (transform.GetChild (0).gameObject.activeSelf == false) {
//					transform.GetChild (0).gameObject.SetActive (true);
//					transform.GetChild (0).GetChild(0).GetComponent<TrailRenderer>().Clear ();
//				}
//			} else {
//				transform.GetChild (0).gameObject.SetActive (false);
//				transform.GetChild (0).GetChild(0).GetComponent<TrailRenderer>().Clear ();
//			}
//			transform.GetComponent<Animator> ().speed = DataManager.buffSpeed * 5.8f / circleTime;
		}
	}
}
