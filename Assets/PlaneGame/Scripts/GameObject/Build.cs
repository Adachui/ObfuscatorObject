using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build : AppAdvisoryHelper
{
	public int level = 1;
	public int value = 100;
	public int exp = 100;
	public ModelBlock info; // 数据模型
	public bool isInit = false;

	public MeshRenderer Num;
	public GameObject buildDown;

	public float scale = 1f;

	Vector3 m_Offset;
	Vector3 m_TargetScreenVec;
	Vector3 m_Origin;
	Vector3 m_OriginPosition; 
	private float y_Offset = 5f;
	private bool hasCollision = false;
	private bool hasRole = false;
	private bool isReturn = false;
	private bool isExpeffct = false;
	private Vector3 expVector = new Vector3 (-9f, 85f, 5f);
	private GameObject role;
	private bool isCanChangePosition = true;
	public static int maxLevel = 40;
	public Dictionary<string, string> buildInfo = new Dictionary<string, string>();
	private Vector3 localPosition = new Vector3 (0f, 0f, 0f);
	public bool isOnMove = false;
	void Start () {
		
		GameObject buildMesh =  Resources.Load ("Prefabs/Tow/" + level) as GameObject;
		buildMesh = Instantiate (buildMesh);
		buildMesh.transform.parent = transform;
		buildMesh.transform.localPosition = Vector3.zero;
		buildMesh.transform.localScale = new Vector3 (1, 1, 1);
		buildMesh.transform.localRotation = Quaternion.Euler(0.0f,135f,0.0f);

		Num.material = Resources.Load<Material> ("caizhiqiu/No/No." + level);
	
		transform.localPosition = localPosition;
		InitBuildingData ();

		transform.GetComponent<Rigidbody> ().Sleep ();
		m_Origin = localPosition;
		m_OriginPosition = transform.position;

		//transform.GetComponent<BoxCollider> ().center = new Vector3 (0, 1.5f, 0);
		//transform.GetComponent<BoxCollider> ().size = new Vector3 (2.2f, 4.5f, 2.2f);

	}

	public void InitBuildingData() {

		buildInfo = GlobalKeyValue.GetXmlDataByKey ("BuildInfo", "Build" + level);
		exp = int.Parse (buildInfo ["MergeExp"]);
		// 增加成就
		//Debug.Log("PlaneGameManager.isInit == " + PlaneGameManager.isInit );
		if(!isInit) {
			transform.parent.GetComponent<Block>().ShowCreateEffect ();
		}
	}

	void Update () {
		if (isReturn && role) {
			role.transform.parent = transform.parent;
			role.transform.GetComponent<BoxCollider> ().isTrigger = false;
			role.transform.localPosition = Vector3.MoveTowards (role.transform.localPosition, transform.localPosition, 200 * Time.deltaTime);
			role.transform.localRotation = Quaternion.Euler(0.0f,0.0f,0.0f);
			if (role.transform.localPosition.Equals(transform.localPosition)) {
//				canvasManager.pathContent.transform.localPosition = new Vector3 (
//					canvasManager.pathContent.transform.localPosition.x,
//					400 + 15 * (DataManager.roleMaxNum - 2),
//					0
//				);
//				Sprite[] sp = Resources.LoadAll<Sprite> ("Sprites/path");
//				planeGameManager.paths[DataManager.roleNum-1].GetComponent<SpriteRenderer>().sprite = sp[0];
				//Debug.Log (DataManager.roleNum + " / " + DataManager.roleMaxNum);
				DataManager.roleNum--;
				planeGameManager.RefreshDefenseNum ();
				isReturn = false;
				hasRole = false;
				ChangeShared ();
				Destroy (role);
			}
		}
//		if (isExpeffct) {
//			Debug.Log ("isExpeffct");
//			blocksManager.expEffect.transform.parent = transform.parent.parent;
//			blocksManager.expEffect.transform.localPosition = Vector3.MoveTowards (blocksManager.expEffect.transform.localPosition, expVector, 100 * Time.deltaTime);
//			blocksManager.expEffect.transform.localRotation = Quaternion.Euler(0.0f,0.0f,0.0f);
//			if(blocksManager.expEffect.transform.localPosition.Equals(expVector)) {
//				isExpeffct = false;
//				blocksManager.expEffect.SetActive (false);
//			}
//		}
	}

	private IEnumerator OnMouseDown()
	{
		planeGameManager.StopGuideAnimation ();

		isOnMove = true;
		isCanChangePosition = true;
		if (!hasRole && canvasManager.isUILayer <= 0) {
			//transform.GetComponent<Animation> ().Play ();
			CompoundCheck ();
			transform.GetComponent<Rigidbody> ().WakeUp();
			hasCollision = false;
			m_TargetScreenVec = Camera.main.WorldToScreenPoint (transform.position);
			m_Offset = transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, m_TargetScreenVec.z));
			while (Input.GetMouseButton (0)) {
				Vector3 curPosition = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, m_TargetScreenVec.z)) + m_Offset;
				// 固定y轴
				curPosition.y = 0;
				transform.position = curPosition;
				transform.position = new Vector3 (transform.position.x, transform.position.y + y_Offset, transform.position.z);
				yield return new WaitForFixedUpdate ();
			}
		} else if(canvasManager.isUILayer <= 0){
			transform.position = new Vector3 (transform.position.x, transform.position.y + y_Offset - 2f, transform.position.z);
			isReturn = true;
			canvasManager.UpdateIncomeSec("-",role.GetComponent<DefendRole>().profitSec);


			// 状态信息
			info.BuildState = 0;
			ManagerBlock.SetBlockXmlData(info);
			transform.parent.GetComponent<Block> ().info = info;

			//role.GetComponent<Animator>().SetBool("isReturn", true);
			//role.GetComponent<Animation> ().Stop ();
			//role.GetComponent<Animator>().enabled = false;
			//role.transform.GetChild (0).gameObject.SetActive(true);
			yield return new WaitForFixedUpdate ();
			ChangeShared ();
			tag = "Build";
		}
	}

	private IEnumerator OnMouseUp() {
		planeGameManager.StopGuideAnimation ();
		if (!hasRole && canvasManager.isUILayer <= 0) {
			CompoundCheckDismiss ();
			transform.position = new Vector3 (transform.position.x, transform.position.y - y_Offset, transform.position.z);
			if (transform.localPosition.y < 0) {
				transform.localPosition = new Vector3 (transform.localPosition.x, 0f, transform.localPosition.z);
			}
		}
			yield return new WaitForSeconds(0f);
			//Debug.Log (m_Origin);
			transform.localPosition = m_Origin;
			//transform.position = m_OriginPosition;
		transform.GetComponent<Animation> ().Stop ();
		isOnMove = false;
	}


	// 合成高亮
	private void CompoundCheck() {
		int count = blocksManager.GetAllBlockCount();
//		if (DataManager.isGoldVip == 0) {
//			count--;
//		}
		for(int i = 0; i < count;i++){
			GameObject _bGameObject = blocksManager.GetBlockById(i);
			ModelBlock _bInfo = ManagerBlock.GetBlockXmlDataById (i);
			if (_bInfo != null && 
				_bInfo.BlockId != info.BlockId && 
				_bInfo.BuildLv == level &&
				_bInfo.BuildState == 0) {
				_bGameObject.transform.GetComponent<Block> ().Light ();
			}
		}
//		Debug.Log (
//			"info.BlockId: " + info.BlockId + " / " + transform.parent.GetComponent<Block>().info.BlockId
//		);
	}
	// 合成高亮消失
	private void CompoundCheckDismiss() {
		
		foreach(GameObject block in blocksManager.Blocks) {
			if (block.transform.GetComponent<Block> ().lightEffect.activeSelf) {
				block.transform.GetComponent<Block> ().Dim ();
			}
		}
	}


	// 碰撞事件
	void OnTriggerEnter(Collider other)
	{
		if(true) 
		{
			if (other.tag == tag && other.GetComponent<Build>().level == level) {
				StartCoroutine( Merge (other.transform));
			} else if (other.tag == "Start") {
				CreateRoleOnCycle (other.transform, false);
			} else if (other.tag == "Trash") {
				Sell2Destroy ();
			} else if (other.tag == "Block" || other.tag.Contains("Build")) {
				ChangeLocation (other.transform);
			} else if (other.tag == "DefendBlock"){
				CreateRoleAttack (other.transform, false);
			}
		}
	}

	/*
	 *  拖拽和碰撞效果
	 * 		合并升级	Merge
	 * 		跑道循环	StartCycle
	 * 		奖励		Reward
	 * 		贩卖销毁	Sell2Destroy
	 */
	// 升级
	private IEnumerator Merge(Transform other){
		bool queue = false;
		if (!transform.GetComponent<Rigidbody> ().IsSleeping() && level < maxLevel && isOnMove == true) {
			other.transform.localPosition = other.gameObject.GetComponent<Build> ().localPosition;
			other.GetComponent<BoxCollider> ().isTrigger = false;
			Debug.Log ("Merge: " + info.BlockId);

			if (level + 1 > DataManager.getMaxBuildLv) {
				queue = true;
				canvasManager.buildingUnlock.SetActive (true);
				DataManager.getMaxBuildLv = level + 1;
				DataManager.InitBuildInfo ();
				canvasManager.InitCanvas ();
				canvasManager.buildingUnlock.GetComponent<BuildingUnlockVIew> ().InitView (DataManager.getMaxBuildLv);
				soundManager.SoundMerge (true);

			
			} else {
				soundManager.SoundMerge (false);
			}

			blocksManager.expEffect.transform.parent = other.parent;
			blocksManager.expEffect.transform.localPosition = new Vector3 (0, 1, 0);
			blocksManager.expEffect.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
			blocksManager.fireworksEffect.transform.parent = other.parent;
			blocksManager.fireworksEffect.transform.localPosition = new Vector3 (0, 9, 0);
			blocksManager.fireworksEffect.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);

			blocksManager.ShowExpEffect ();



			if (DataManager.getMaxBuildLv >= 8) {
				planeGameManager.UpdateDailyTask ("1900");
			}

			GameObject build = Resources.Load ("Prefabs/Defend/Build") as GameObject;
			build = Instantiate (build); 
			build.transform.parent = other.parent;
			build.transform.localPosition = localPosition;
			build.transform.localScale = BlocksManager.BUILDSCALE;
			//build.transform.localRotation = Quaternion.Euler(0.0f,135f,0.0f);
			build.transform.GetComponent<Build> ().level = level + 1;
			hasCollision = true;

			// 存储信息
			info.BuildLv = 0;
			info.BuildId = "0";
			ManagerBlock.SetBlockXmlData (info);
			transform.parent.GetComponent<Block> ().info = info;

			// 存储信息
			other.GetComponent<Build>().info.BuildLv = level + 1;
			build.transform.GetComponent<Build> ().info = other.GetComponent<Build>().info;
			ManagerBlock.SetBlockXmlData (build.transform.GetComponent<Build> ().info);
			other.parent.GetComponent<Block>().info = other.GetComponent<Build>().info;


			Destroy (other.gameObject);
			Destroy (gameObject);

			planeGameManager.RefreshLevelData (exp, queue);

            ManagerUserInfo.SetDataByKey("GetMaxBuildLv", DataManager.getMaxBuildLv);
            if (DataManager.getMaxBuildLv == 3){
                canvasManager.speedBtn.gameObject.SetActive(true);
                canvasManager.shopBtn.gameObject.SetActive(true);
            }
			yield return new WaitForSeconds(0f);
        }
	}

	// 产生角色进入跑道循环
	public void CreateRoleOnCycle(Transform start, bool isInit = true) {
		if (!hasRole && DataManager.roleNum < DataManager.roleMaxNum) {
			role =  Resources.Load ("Prefabs/Role") as GameObject;
			role = Instantiate (role);
			role.transform.parent = GameObject.FindGameObjectsWithTag ("ObjectManager") [0].transform;
			role.transform.position = new Vector3 (transform.position.x, localPosition.y, transform.position.z);
			role.transform.localScale = new Vector3 (scale, scale, scale);
			role.transform.GetComponent<Role> ().level = level;
			tag = "Object";
			transform.localPosition = localPosition;
			hasRole = true;
			ChangeShared ();

			// 信息
			info.BuildState = 1;
			ManagerBlock.SetBlockXmlData(info);
			transform.parent.GetComponent<Block> ().info = info;

			Role roleIntance = role.GetComponent<Role> ();
			roleIntance.circleTime = float.Parse(buildInfo ["CircleTime"]);
			roleIntance.profitSec = new BigNumber (double.Parse (buildInfo ["ProfitSec"]), int.Parse (buildInfo ["Units"]));
			roleIntance.value = new BigNumber (roleIntance.profitSec.number * roleIntance.circleTime, roleIntance.profitSec.units);
			if (!isInit) {
				Debug.Log ("CreateRoleOnCycle");
				canvasManager.UpdateIncomeSec("+",roleIntance.profitSec);
				start.GetChild (0).transform.GetComponent<ParticleSystem> ().Play ();
			} else {
				roleIntance.isStart = true;
				Debug.Log (5.8f / roleIntance.circleTime + info.BlockId * 0.1f);
				role.GetComponent<Animator>().speed = 5.8f/ roleIntance.circleTime + info.BlockId * 0.1f;
			}

			// 新手引导
			if (ManagerUserInfo.GetIntDataByKey ("NewPlayerStep3") == 0) {
				ManagerUserInfo.SetDataByKey ("NewPlayerStep3", 1);
				guideLayer.GuideStep_3 ();
			}
		}
	}

	//产生角色进入炮台开始攻击
	public void CreateRoleAttack(Transform defendBlock, bool isInit = true){
		Transform roleInBlock = defendBlock.Find ("DefendRole(Clone)");
		if (roleInBlock == null && !hasRole && DataManager.roleNum < DataManager.roleMaxNum) {
			role =  Resources.Load ("Prefabs/Defend/DefendRole") as GameObject;
			role = Instantiate (role);
			role.transform.parent = defendBlock;
			role.transform.localPosition = new Vector3 (0f, 2f, 0f);
			//role.transform.position = new Vector3 (defendBlock.position.x, defendBlock.position.y + 4f, defendBlock.position.z);
			role.transform.localScale = new Vector3 (0.7f, 0.7f, 0.7f);
			role.transform.GetComponent<DefendRole> ().level = level;
			role.transform.GetComponent<DefendRole> ().attackDis = float.Parse(buildInfo ["Distance"]);
			tag = "DefendPlane";
			transform.localPosition = localPosition;
			hasRole = true;
			ChangeShared ();

			info.BuildState = 1;
			info.DefendBlockId = defendBlock.GetComponent<DefendBlock> ().DefendBlockId;
			ManagerBlock.SetBlockXmlData(info);
			transform.parent.GetComponent<Block> ().info = info;

			DefendRole roleIntance = role.GetComponent<DefendRole> ();
			roleIntance.circleTime = float.Parse(buildInfo ["CircleTime"]);
			roleIntance.attackSpeed = roleIntance.circleTime / 2;
			roleIntance.profitSec = new BigNumber (double.Parse (buildInfo ["ProfitSec"]), int.Parse (buildInfo ["Units"]));
			roleIntance.value = new BigNumber (roleIntance.profitSec.number * 2, roleIntance.profitSec.units);

			if (!isInit) {
				Debug.Log ("CreateRoleOnCycle");
				canvasManager.UpdateIncomeSec("+",roleIntance.profitSec);
			} else {
				roleIntance.isStart = true;
				Debug.Log (5.8f / roleIntance.circleTime + info.BlockId * 0.1f);
				//role.GetComponent<Animator>().speed = 5.8f/ roleIntance.circleTime + info.BlockId * 0.1f;
			}

			// 新手引导
			if (ManagerUserInfo.GetIntDataByKey ("NewPlayerStep3") == 0) {
				ManagerUserInfo.SetDataByKey ("NewPlayerStep3", 1);
				guideLayer.GuideStep_3 ();
			}
		}
	}


	// 销毁
	private void Sell2Destroy() {
		//Debug.Log ("销毁 " + level);
		// 存储信息

		double price = canvasManager.GetDoubleDataByKey(buildInfo, "Price");
		int unit = canvasManager.GetIntDataByKey(buildInfo, "PriceUnit");
		BigNumber priceBigNumber = new BigNumber(price, unit);
		canvasManager.UpdateCoin ("+", priceBigNumber);


		info.BuildLv = 0;
		info.BuildId = "0";
		ManagerBlock.SetBlockXmlData (info);
		transform.parent.GetComponent<Block> ().info = info;

		Destroy (gameObject);
		canvasManager.TrashSmokeEffectShow ();
	}

	// 修改建筑位置
	private void ChangeLocation(Transform other) {
//		Transform buildInBlock = other.Find ("Build(Clone)");
//		Transform boxInBlock = other.Find ("box(Clone)");
//		Transform boxNormalInBlock = other.Find ("box_normal(Clone)");
		//固定资源类都放入buildbase 创建类为第二子节点，防止改来改去
		if (other.childCount <= BlocksManager.baseChildCount && other.tag == "Block" ) {
			ModelBlock _curBlockInfo = info;
			ModelBlock _targetBlockInfo = other.transform.GetComponent<Block> ().info;
			_targetBlockInfo.BuildLv = _curBlockInfo.BuildLv;
			_targetBlockInfo.BuildId = _curBlockInfo.BuildId;

			other.transform.GetComponent<Block> ().info = _targetBlockInfo;

			_curBlockInfo.BuildLv = 0;
			_curBlockInfo.BuildId = "0";

			ManagerBlock.SetBlockXmlData(_curBlockInfo);
			ManagerBlock.SetBlockXmlData(_targetBlockInfo);
			info = _targetBlockInfo;
			transform.parent = other;
			transform.localPosition = localPosition;
			m_Origin = localPosition;
			m_OriginPosition = transform.position;
			soundManager.SoundLocationSwitch ();
		} else {
			if (other.GetComponent<Build> () != null) {
				if (isCanChangePosition &&  other.GetComponent<Build> ().info.BuildState == 0 && other.GetComponent<Build> ().info.BuildLv != info.BuildLv) {
					other.GetComponent<Build> ().isCanChangePosition = false;
				
					string id = info.BuildId;
					int lv = info.BuildLv;
					int bId = info.BlockId;

					info.BlockId = other.GetComponent<Build> ().info.BlockId;
					other.GetComponent<Build> ().info.BlockId = bId;


					transform.parent.GetComponent<Block> ().info = other.GetComponent<Build> ().info;
					other.parent.GetComponent<Block> ().info = info;
					
					transform.parent = blocksManager.Blocks [info.BlockId].transform;

					transform.localPosition = localPosition;
					m_Origin = localPosition;
					m_OriginPosition = transform.position;

					other.parent = blocksManager.GetBlockById(bId).transform;
				
					other.localPosition = localPosition;
					other.GetComponent<Build> ().m_Origin = localPosition;
					other.GetComponent<Build> ().m_OriginPosition = other.position;
					ManagerBlock.SetBlockXmlData (info);
					ManagerBlock.SetBlockXmlData (other.GetComponent<Build> ().info);
					StartCoroutine (ModifiedBlockID (other));
					soundManager.SoundLocationSwitch ();
				}
			} 
		}
	}

	private IEnumerator ModifiedBlockID(Transform other) {
		yield return new WaitForFixedUpdate ();
		Debug.Log("this buidl " + info.BlockId + "/block " + transform.parent.GetComponent<Block>().info.BlockId);
		Debug.Log("other buidl " + other.GetComponent<Build>().info.BlockId + "/block " + other.parent.GetComponent<Block> ().info.BlockId);

		//transform.parent.GetComponent<Block> ().info.BlockId = info.BlockId;
		//other.parent.GetComponent<Block> ().info.BlockId = other.GetComponent<Build> ().info.BlockId;
	}
	// 修改材质
	private void ChangeShared() {
		
		if (hasRole) {
			buildDown.SetActive (true);
			/*if (transform.childCount > BlocksManager.baseChildCount) {
				if (transform.GetChild (0).GetComponent<MeshRenderer> () != null) {
					transform.GetChild (0).GetComponent<MeshRenderer> ().material = Resources.Load<Material> ("Colors Transparency");
				}
				for (int i = 0; i < transform.GetChild (0).childCount; i++) {
					transform.GetChild (0).GetChild (i).GetComponent<MeshRenderer> ().material = Resources.Load<Material> ("Colors Transparency");
				}
			} else {
				transform.GetComponent<MeshRenderer> ().material = Resources.Load<Material> ("Colors Transparency");
			}*/

		} else {
			buildDown.SetActive (false);
			/*if (transform.childCount > BlocksManager.baseChildCount) {
				if (transform.GetChild (0).GetComponent<MeshRenderer> () != null) {
					transform.GetChild (0).GetComponent<MeshRenderer> ().material = Resources.Load<Material> ("Colors");
				}
				for (int i = 0; i < transform.GetChild (0).childCount; i++) {
					transform.GetChild (0).GetChild (i).GetComponent<MeshRenderer> ().material = Resources.Load<Material> ("Colors");
				}
			} else {
				transform.GetComponent<MeshRenderer> ().material = Resources.Load<Material> ("Colors");
			}*/
		}

	} 
}
