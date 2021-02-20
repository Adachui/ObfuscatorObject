using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyMobile;

public class Role : AppAdvisoryHelper {
    private string[] MeshName = {
        "SimpleCitizens_Hippie_White"
        ,"SimpleCitizens_Luchador_White"
        ,"SimpleCitizens_Nerd_White"
        ,"SimpleCitizens_Prisoner_White"
        ,"SimpleCitizens_Racer_White"
        ,"SimpleCitizens_Runner_White"
        ,"SimpleCitizens_ShopKeeper_White"
        ,"SimpleCitizens_Hippie_Brown"
        ,"SimpleCitizens_Luchador_Brown"
        ,"SimpleCitizens_Nerd_Brown"
        ,"SimpleCitizens_Prisoner_Brown"
        ,"SimpleCitizens_Racer_Brown"
        ,"SimpleCitizens_Runner_Brown"
        ,"SimpleCitizens_ShopKeeper_Brown"
        ,"SimpleCitizens_Hippie_Black"
        ,"SimpleCitizens_Luchador_Black"
        ,"SimpleCitizens_Nerd_Black"
        ,"SimpleCitizens_Prisoner_Black"
        ,"SimpleCitizens_Racer_Black"
        ,"SimpleCitizens_Runner_Black"
        ,"SimpleCitizens_ShopKeeper_Black"
        ,"SimpleCitizens_Mountie_White"
        ,"SimpleCitizens_Biker_White"
        ,"SimpleCitizens_Cheerleader_White"
        ,"SimpleCitizens_Clown_White"
        ,"SimpleCitizens_Emo_White"
        ,"SimpleCitizens_Footballer_White"
        ,"SimpleCitizens_Grandma_White"
        ,"SimpleCitizens_Mountie_Brown"
        ,"SimpleCitizens_Biker_Brown"
        ,"SimpleCitizens_Cheerleader_Brown"
        ,"SimpleCitizens_Clown_Brown"
        ,"SimpleCitizens_Emo_Brown"
        ,"SimpleCitizens_Footballer_Brown"
        ,"SimpleCitizens_Grandma_Brown"
        ,"SimpleCitizens_Mountie_Black"
        ,"SimpleCitizens_Biker_Black"
        ,"SimpleCitizens_Cheerleader_Black"
        ,"SimpleCitizens_Clown_Black"
        ,"SimpleCitizens_Emo_Black"
    };
	public int level = 1;
	public BigNumber value;
	public BigNumber profitSec; 
	public float circleTime = 5.8f;

	private float startTime = 0f;
	public bool isStart = false;
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
		
        //transform.gameObject.GetComponent<Animation> ().Play ();	
//        value *= level;
        //动态加载角色；
        GameObject roleMesh =  Resources.Load ("Prefabs/SimpleCitizens/Prefabs/" + MeshName[level-1]) as GameObject;
		roleMesh = Instantiate (roleMesh);
		roleMesh.transform.parent = transform;
		roleMesh.transform.localPosition = Vector3.zero;
		roleMesh.transform.localScale = new Vector3 (1, 1, 1);
		transform.GetChild (0).gameObject.SetActive (true);
		transform.GetChild (0).GetChild(0).GetComponent<TrailRenderer>().Clear ();
		InitRoleAnimation ();
		Sprite[] sp = Resources.LoadAll<Sprite> ("Sprites/path");
		planeGameManager.paths[DataManager.roleNum].GetComponent<SpriteRenderer>().sprite = sp[1];
		DataManager.roleNum++;
		planeGameManager.RefreshDefenseNum ();
//		canvasManager.pathContent.transform.localPosition = new Vector3 (
//			canvasManager.pathContent.transform.localPosition.x,
//			400 + 15 * (DataManager.roleMaxNum - 2),
//			0
//		);

		TrailColor ();
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
		

		if (!isStart) {
			transform.GetComponent<Animator>().speed = 5.8f/ circleTime;
			soundManager.SoundOnRoad ();
		}
		transform.GetComponent<Animator>().SetBool("isReturn", false);

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
			if (PlaneGameManager.isSpeedUp) {
				if (transform.GetChild (0).gameObject.activeSelf == false) {
					transform.GetChild (0).gameObject.SetActive (true);
					transform.GetChild (0).GetChild(0).GetComponent<TrailRenderer>().Clear ();
				}
			} else {
				transform.GetChild (0).gameObject.SetActive (false);
				transform.GetChild (0).GetChild(0).GetComponent<TrailRenderer>().Clear ();
			}
			transform.GetComponent<Animator> ().speed = DataManager.buffSpeed * 5.8f / circleTime;
		}
	}

	// 碰撞事件
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Reward") {
			//Debug.Log ("Reward");
			Reward (new BigNumber(value.number * DataManager.buffIncome, value.units), other.transform.GetChild (0).GetComponent<ParticleSystem> ());
		} 
	}
//	// 碰撞事件
//	void OnTriggerEnter(Collider other)
//	{
//		if(tag == "Reward") {
//			Debug.Log ("Coin + " + value);
//			Reward (value, other.transform.GetChild (0).GetComponent<ParticleSystem> ());
//		} else if(tag == "Start") {
//			StartCycle ();
//		}
//	}
		
	// 奖励
	private void Reward(BigNumber incomeValue, ParticleSystem particle) {
       
		//Debug.Log(DataManager.coins);
//		if (!canvasManager.destinationEffect.isPlaying) {
//			canvasManager.destinationEffect.Play ();
//		}
		particle.Play ();
		soundManager.SoundOnRoadReward ();
		//ReportScore ();
		canvasManager.UpdateCoin("+", incomeValue);
		GameObject coin =  Resources.Load("Prefabs/UI/coin") as GameObject;
		coin = Instantiate (coin);
		coin.transform.SetParent(canvasManager.coinContant);
		coin.transform.localPosition = new Vector3(181f, 42.5f, 0f);
		coin.transform.localScale = new Vector3 (0.7f, 0.7f, 0.7f);
		coin.transform.GetChild (0).GetComponent<Text> ().text = incomeValue.ToString ();

		canvasManager.coinAnimation.Play ();


	}

	private IEnumerator ShowCoinGet(BigNumber incomeValue, ParticleSystem particle) {
		//Debug.Log(DataManager.coins);
//		if (!canvasManager.destinationEffect.isPlaying) {
//			canvasManager.destinationEffect.Play ();
//		}
		particle.Play ();
		soundManager.SoundOnRoadReward ();
		//ReportScore ();
		yield return new WaitForSeconds (0.2f);
		canvasManager.UpdateCoin("+", incomeValue);
		GameObject coin =  Resources.Load("Prefabs/UI/coin") as GameObject;
		coin = Instantiate (coin);
		coin.transform.SetParent(canvasManager.coinContant);
		coin.transform.localPosition = new Vector3(181f, 42.5f, 0f);
		coin.transform.localScale = new Vector3 (0.7f, 0.7f, 0.7f);
		coin.transform.GetChild (0).GetComponent<Text> ().text = incomeValue.ToString ();

		canvasManager.coinAnimation.Play ();

//		if(!canvasManager.coinAnimation.isPlaying) {
//			canvasManager.coinAnimation.Play ();
//		}
	}
	// 进入跑道循环
	private void StartCycle() {
//		Debug.Log ("进入跑道 " + level);
//		transform.GetChild (0).gameObject.SetActive (true);
//		transform.gameObject.GetComponent<Animation> ().Play();	
	}



}
