using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions.Must;

public class MonsterPlaneMgr : AppAdvisoryHelper {

	public List<GameObject> monsterList = new List<GameObject>();


	private Vector3 localPosition = new Vector3 (0f, 2.5f, 0f);

	private float scale = 1f;
	private float disTime = 0;

	private int inPathNum = 0;
	private bool isInit = false;
	private int[] indexList = new int[10];

	public void InitMonster(){
		isInit = true;
		indexList = ListConfig.GetMonsterList (DataManager.lv);
	}

	public void MonsterCreate(){
		
		GameObject monster =  Resources.Load ("Prefabs/Defend/Monster") as GameObject;
		monster = Instantiate (monster);
		monster.transform.parent = transform;
		monster.transform.position = new Vector3 (transform.position.x, localPosition.y, transform.position.z);
		monster.transform.localScale = new Vector3 (scale, scale, scale);
		monster.GetComponent<Monster>().index = inPathNum;
		monster.transform.GetComponent<Monster> ().level = indexList[inPathNum];
		inPathNum++;

		monsterList.Add (monster);
	}

	void Update(){
		if (!isInit) {
			InitMonster ();
		}
		disTime += Time.deltaTime;
		if (disTime > 1.7f && inPathNum < indexList.Length) {
			MonsterCreate ();
			disTime = 0;
		}else if (inPathNum == 0){
			MonsterCreate ();
		}
	}

}
