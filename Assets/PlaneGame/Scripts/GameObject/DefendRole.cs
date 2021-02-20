using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyMobile;
using DG.Tweening;
using System;

/// <summary>
/// /炮台上的角色
/// </summary>
public class DefendRole : AppAdvisoryHelper {
	public int level = 1;

	public ParticleSystem speedEffect;
	public GameObject tow;

	public BigNumber value;          //实际每次攻击得到的金币数量
	public BigNumber profitSec;      //每次攻击的得到的金币数量（配置表）

	public float circleTime = 5.8f;  //攻击速度 即 每次攻击间隔时间（配置表）
	public float attackSpeed = 5.8f;   //实际攻击速度 即 每次攻击间隔时间
	public float attackDis = 5f;       //攻击距离

	public bool isStart = false;     //攻击开始

	public Dictionary<string, string> effectInfo = new Dictionary<string, string>();

	private bool isRotate = false;     //转动开始
	private float attackStart = 0f;  //攻击计时
	private Quaternion initRotation;                // 保存转身前的角度  
	private Quaternion monsterRotation;             // 准备面向的角度   
	private float per_second_rotate = 360.0f;       // 转身速度(每秒能转多少度)    
	float lerp_speed = 0.0f;                        // 旋转角度越大, lerp变化速度就应该越慢   
	float lerp_tm = 0.0f;                           // lerp的动态参数

	bool isPlay = false;


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
		GameObject roleMesh =  Resources.Load ("Prefabs/Tow/" + level) as GameObject;
		roleMesh = Instantiate (roleMesh);
		roleMesh.transform.parent = transform;
		roleMesh.transform.localPosition = Vector3.zero;
		roleMesh.transform.localScale = new Vector3 (1f, 1f, 1f);
		//transform.GetChild (0).gameObject.SetActive (true);
		//transform.GetChild (0).GetChild(0).GetComponent<TrailRenderer>().Clear ();
		//InitRoleAnimation ();

		DataManager.roleNum++;
		planeGameManager.RefreshDefenseNum ();

		effectInfo = GlobalKeyValue.GetXmlDataByKey ("TowerEffectsInfo", "TowerEffect" + level);

		//TrailColor ();
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

	//发出攻击 
	void PlayAttack(GameObject curMonster,int monsterIndex){
		Vector3 rolePos = transform.position;
		GameObject attack = attackMgr.GetAttack (effectInfo["Texiao"]);
		attack.SetActive (true);
		//Debug.Log (rolePos);
		attack.transform.position = new Vector3 (rolePos.x, rolePos.y + 1, rolePos.z);
		attack.transform.localScale = new Vector3 (1, 1, 1);

		attack.transform.GetComponent<Attack> ().setTarget (curMonster,monsterIndex,value,effectInfo["Texiao"]);
		soundManager.SoundAttack (effectInfo["Music"]);
	}

	//转向目标
	void LookAtTarget(Transform monster){
		// 保存转身前的角度   
		initRotation = transform.rotation;
		// 获得并保存目标角度  
		Vector3 monsterPos = new Vector3(monster.position.x,transform.position.y,monster.position.z);
		tow.transform.LookAt(monsterPos);
		//transform.localPosition = new Vector3(0,0,0);
		monsterRotation = tow.transform.rotation;
		// 恢复到原来的角度  
		transform.rotation = initRotation;
		// 计算出需要的旋转角度   
		float rotate_angle = Quaternion.Angle(initRotation, monsterRotation);
		// 获得lerp速度  
		lerp_speed = per_second_rotate / rotate_angle;
		lerp_tm = 0.0f;
		isRotate = true;

		//transform.localRotation = Quaternion.Euler(transform.position.x,transform.position.y + rotate_angle,transform.position.z);
	}

	//转动
	void RotateFunc()
	{
		// 旋转完成
		if (lerp_tm >= 1)
		{
			isRotate = false;
			transform.rotation = monsterRotation;
			return;
		}
		// 使用 Quaternion.Lerp 进行旋转
		lerp_tm += Time.deltaTime * lerp_speed * 300;
		transform.rotation = Quaternion.Lerp(initRotation, monsterRotation, lerp_tm);
	}


	void Update () {
		if (!isStart || attackStart > attackSpeed / DataManager.buffSpeed) {
			isStart = true;
			attackStart = 0;
			GameObject curMonster = null;
			int length = monsterPlaneMgr.monsterList.Count;
			float dis = 0f;
			int disInt = 0;
			int minDis = 0;
			int monsterIndex = -1;
			for (int i = 0; i < length; i++) {
				GameObject monster = monsterPlaneMgr.monsterList [i];
				Vector2 rolePos = new Vector2 (transform.position.x, transform.position.z);
				Vector2 monsterPos = new Vector2(monster.transform.position.x,monster.transform.position.z);
				//Debug.Log ("transform.position  " + transform.position);
				//Debug.Log ("monster.position  " + monster.transform.position + "  === index === " + i);
				dis = Vector2.Distance (rolePos, monsterPos);
				disInt = (int)dis;

				if (disInt <= minDis || minDis == 0) {
					minDis = disInt;
					curMonster = monster;
					monsterIndex = i;
				}
				
			}
			if (minDis > 0 && monsterIndex >= 0 && minDis <= attackDis){
				//Debug.Log ("monsterIndex = "+ monsterIndex + "minDis = " + minDis);
				LookAtTarget(curMonster.transform);
				PlayAttack (curMonster,monsterIndex);
			}
		}
		attackStart += Time.deltaTime;
		if(isRotate){
			RotateFunc ();
		}
		if(PlaneGameManager.isSpeedUp){
			if(speedEffect != null && !speedEffect.isPlaying){
				speedEffect.Play ();
			}
		}else{
			if(speedEffect != null && speedEffect.isPlaying)
				speedEffect.Stop ();

		}
	}
}
