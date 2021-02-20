using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//攻击池//
public class AttackMgr:AppAdvisoryHelper{

	//弓箭类攻击池
	public List<GameObject> gongjianPool = new List<GameObject>();
	//火焰类攻击池
	public List<GameObject> huoqiu1Pool = new List<GameObject>();
	public List<GameObject> huoqiu2Pool = new List<GameObject>();
	public List<GameObject> huoqiu3Pool = new List<GameObject>();
	//激光类攻击池
	public List<GameObject> jiguang1Pool = new List<GameObject>();
	public List<GameObject> jiguang2Pool = new List<GameObject>();
	public List<GameObject> jiguang3Pool = new List<GameObject>();
	//闪电类攻击池
	public List<GameObject> shandian1Pool = new List<GameObject>();
	public List<GameObject> shandian2Pool = new List<GameObject>();
	public List<GameObject> shandian3Pool = new List<GameObject>();
	//金币获得动画池
	public List<GameObject> coinGetPool = new List<GameObject>();

	//攻击类
	public Dictionary<string, List<GameObject>> attackDic = new Dictionary<string, List<GameObject>>();


	// Use this for initialization
	void Start () {
		attackDic["gongjian"] = gongjianPool;
		attackDic["huoqiu_1"] = huoqiu1Pool;
		attackDic["huoqiu_2"] = huoqiu2Pool;
		attackDic["huoqiu_3"] = huoqiu3Pool;
		attackDic["jiguang_1"] = jiguang1Pool;
		attackDic["jiguang_2"] = jiguang2Pool;
		attackDic["jiguang_3"] = jiguang3Pool;
		attackDic["shandian_1"] = shandian1Pool;
		attackDic["shandian_2"] = shandian2Pool;
		attackDic["shandian_3"] = shandian3Pool;
		attackDic["coin"] = coinGetPool;


	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public GameObject GetAttack(string key){
		int length = attackDic[key].Count;
		bool isExit = false;
		GameObject needAttack = null;
		if (length > 0) {
			for (int i = 0; i < length; i++) {
				GameObject curAttack = attackDic[key][i];
				if(!curAttack.activeInHierarchy){
					isExit = true;
					needAttack = curAttack;
					break;
				}
			}
		}

		if (!isExit){
			needAttack = Resources.Load ("Prefabs/Defend/Attack/" + key) as GameObject;
			needAttack = Instantiate (needAttack);
			needAttack.transform.parent = GameObject.FindGameObjectWithTag ("MonsterPlane").transform;
			attackDic[key].Add (needAttack);
		}
		return needAttack;
	}



	public GameObject GetGongJianAttack(){
		int length = gongjianPool.Count;
		bool isExit = false;
		GameObject needAttack = null;
		if (length > 0) {
			for (int i = 0; i < length; i++) {
				GameObject curAttack = gongjianPool [i];
				if(!curAttack.activeInHierarchy){
					isExit = true;
					needAttack = curAttack;
					break;
				}
			}
		}

		if (!isExit){
			needAttack = Resources.Load ("Prefabs/Defend/" + "Attack") as GameObject;
			needAttack = Instantiate (needAttack);
			needAttack.transform.parent = GameObject.FindGameObjectWithTag ("MonsterPlane").transform;
			gongjianPool.Add (needAttack);
		}
		return needAttack;
	}




	public GameObject GetCoinGet(){
		int length = coinGetPool.Count;
		bool isExit = false;
		GameObject needCoin = null;
		if (length > 0) {
			for (int i = 0; i < length; i++) {
				GameObject coin = coinGetPool [i];
				if(!coin.activeInHierarchy){
					isExit = true;
					needCoin = coin;
					break;
				}
			}
		}

		if (!isExit){
			needCoin = Resources.Load ("Prefabs/Defend/" + "CoinGet") as GameObject;
			needCoin = Instantiate (needCoin);
			needCoin.transform.parent = GameObject.FindGameObjectWithTag ("MonsterPlane").transform;
			coinGetPool.Add (needCoin);
		}
		return needCoin;
	}

}
