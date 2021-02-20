using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyMobile;

public class Attack : AppAdvisoryHelper {

	private GameObject targetMonster = null;
	private bool isMove = false;     //是否开始移动
	private Vector3 disPos = new Vector3(0,0,0);
	private float dis = 0f;
	private int targetTag = 0;   //碰撞目标的tag
	private BigNumber getCoins;      //碰撞后得到的金币
	public string attackName = "gongjian";
	public int attackType = 0;   //1有位移 2无位移，直接特效连接

	//public GameObject particle = null;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isMove  && attackType == 1 && targetMonster != null){
			transform.position=Vector3.Lerp (transform.position,targetMonster.transform.position,Time.deltaTime*10f);//闪电5  火球9

			//transform.RotateAround (targetMonster.transform.position,Vector3.forward, Time.deltaTime*500);
		}
	}
	/// <summary>设置攻击目标的参数</summary>
	public void setTarget(GameObject monster,int index,BigNumber coins,string name){
		attackName = name;
		targetMonster = monster;
		targetTag = index;
		isMove = true;
		getCoins = coins;
		transform.LookAt (targetMonster.transform);
		if (attackType == 2) {
			disPos = targetMonster.transform.position - transform.position;
			Vector3 size = transform.GetComponent<BoxCollider> ().size;
			dis = disPos.magnitude / 3.5f;
			transform.localScale = new Vector3 (2, 2, dis);
			transform.GetComponent<BoxCollider> ().size = new Vector3 (1, 1, disPos.magnitude);
		}
	}

	// 碰撞事件
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Monster" && other.gameObject.GetComponent<Monster>().index == targetTag) {
			//Debug.Log ("Monster === " + targetTag );
			isMove = false;
			Reward (new BigNumber(getCoins.number * DataManager.buffIncome, getCoins.units), other.transform.GetChild (1).GetComponent<ParticleSystem> ());
			gameObject.SetActive (false);
			other.gameObject.GetComponent<Monster>().ShowHitEffect (attackName);
		} 
	}

	// 奖励
	private void Reward(BigNumber incomeValue, ParticleSystem particle) {

		//Debug.Log(DataManager.coins);
		particle.Play ();
		soundManager.SoundOnRoadReward ();
		//ReportScore ();
		canvasManager.UpdateCoin("+", incomeValue);
		GameObject coin =  attackMgr.GetCoinGet();
		coin.SetActive (true);
		coin.transform.localPosition = new Vector3(targetMonster.transform.localPosition.x, 5f, targetMonster.transform.localPosition.z);
		Vector3 pos = coin.transform.position;
		pos.y = 6f;
		coin.transform.position = pos;
		coin.transform.GetChild (0).GetComponent<TextMesh>().text = incomeValue.ToString ();
		coin.transform.GetComponent<CoinUpVIew> ().StartAnimation ();
		canvasManager.coinAnimation.Play ();


	}
}
