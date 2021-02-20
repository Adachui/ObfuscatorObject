using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyMobile;

public class BlocksManager : AppAdvisoryHelper
{
	public GameObject MoreLayer;
	public List<GameObject> Blocks =  new List<GameObject>();
	// 是否为初始化状态
	public bool isInit = true;
	// 间隔计算
	private bool isBlocksPurchased = false;
	private float centerX = 0;
	private float centerZ = -3f;
	private float spaceX = 2f;
	private float spaceZ = 4f;
	private const int maxRow = 5;
	private const int index2Col = maxRow * 2 - 1;
	// 地块Y
	private float posY = 1.8f;
	// 放大
	public static Vector3 BLOCKSCALE = new Vector3(1.5f, 1.5f, 1.5f);
	public static Vector3 BUILDSCALE = new Vector3(1f, 1f, 1f);
	public static Vector3 BOXCALE = new Vector3(2f, 2f, 2f);

	public GameObject expEffect;
	public GameObject fireworksEffect;

	public const int baseChildCount = 1;//地块基础子节点，需要保证只有一个

	private GameObject LockBlock;  //锁住的地块

	// 地块初始化
	public void InitBlcoks(List<ModelBlock> blocksInfo) {
		isInit = true;
		int maxNum = DataManager.maxBlockNum;
		if (DataManager.isGoldVip <= 0 && DataManager.maxBlockNum > 4) {
			CreateLockingBlock ();
			maxNum = DataManager.curBlockNum;
		}
		for (int i = 0; i < blocksInfo.Count; i++) {
			if(blocksInfo[i].BuildLv > -1 && i < maxNum)
				CreateBlock (i,0, blocksInfo[i]);
		}

		isInit = false;
	
	}

	// 创建地块 _type 0 初始化时使用，1 未解锁地块 ，2 升级时新增地块
	private void NewBlock(int index, ModelBlock blockInfo = null,int _type = 0) {
		string[] names = {"Block","Block(Lock)","Block"};
		string prefabs = names[_type];

		if (_type != 0) {
			DataManager.maxBlockNum++;
			if (_type == 2) {
				if (LockBlock == null) {
					CreateLockingBlock ();
				}else if (LockBlock.activeInHierarchy) {
					LockBlock.transform.localPosition = GetBlockPos (DataManager.maxBlockNum - 1);
				}
			}
		}

		// 创建地块
		GameObject block = Resources.Load ("Prefabs/Defend/" + prefabs) as GameObject;
		block = Instantiate (block); 
		block.transform.parent = transform;
		block.transform.localScale = BLOCKSCALE;
		block.transform.localPosition = GetBlockPos (index);//new Vector3 (posX, posY, posZ);
		//block.transform.localRotation = Quaternion.Euler (18f, 48f, 18f);

		if (_type == 1) {
			LockBlock = block;
			block.GetComponent<Block> ().isLockBlock = true;
			block.GetComponent<Block> ().MoreLayer = MoreLayer;
		} else {
			if (blockInfo == null) {
				blockInfo = new ModelBlock ();
				blockInfo.BlockId = index;
				blockInfo.BuildId = "0";
				blockInfo.BuildLv = 0;
				blockInfo.BuildState = 0;
				blockInfo.DefendBlockId = 0;
				ManagerBlock.SetBlockXmlData (blockInfo);
			}
			block.transform.GetComponent<Block> ().info = blockInfo;
			block.transform.GetComponent<Block> ().BlockId = index;
			// 存储地块
			Blocks.Add (block);
			CreateBuilding (block, blockInfo);
		}
		if (_type == 2) {
			for (int i = 0; i < GetAllBlockCount(); i++) {
				Blocks[i].transform.localPosition = GetBlockPos (i);
			}
		}
    }

	// 新增地块
	public void CreateBlock(int index,int _type,ModelBlock blockInfo = null) {
		if (index > 13 && DataManager.isGoldVip == 0)
			return;

		if (index > 14 && DataManager.isGoldVip == 0)
			return;

		NewBlock (index, blockInfo,_type);
	}
		
	// 创建锁定地块(付费解锁)
	private void CreateLockingBlock() {
		NewBlock (DataManager.maxBlockNum, null, 1);
	}

	// 购买地块
	public void BuyBlocks(int num = 2) {
		if (DataManager.isGoldVip == 1) {
//			if (Blocks.Count > 4) {
//				Destroy (Blocks[Blocks.Count - 1]);
//				Blocks.RemoveAt (Blocks.Count - 1);
//			}
			LockBlock.SetActive(false);
			DataManager.maxBlockNum--;
			isBlocksPurchased = true;
			CreateBlock (DataManager.maxBlockNum,2);
			CreateBlock (DataManager.maxBlockNum,2);
		}
	}

	private float GetPosZ(int lineNum,int curLine){
		float startZ = 4f;
		float posZ = centerZ;

		//以中心算z坐标，分为上下两侧
		int halfLineNum =  (int)Mathf.Ceil((float)lineNum / 2);
		//偶数行
		if (lineNum % 2 == 0) {
			startZ = startZ / 2;
			if(curLine <= halfLineNum){
				posZ += startZ + spaceZ * (halfLineNum - curLine);
			}else {
				posZ -= startZ + spaceZ * (curLine - halfLineNum - 1);
			}
		} else {
			if(curLine < halfLineNum){
				posZ += startZ + spaceZ * (halfLineNum - curLine - 1);
			}else if(curLine > halfLineNum){
				posZ -= startZ + spaceZ * (curLine - halfLineNum - 1);
			}
		}
		return posZ;
	}

	// 计算地块坐标
	private Vector3 GetBlockPos(int index){
		// 计算坐标
		float posZ = centerZ;
		float posX = centerX;

		int numOfBlocks = index + 1;
		//10以内的地块
		if (index <= index2Col) {
			//地块列数
			int lineNum =  (int)Mathf.Ceil((float)DataManager.maxBlockNum / 2);
			if (lineNum > 5)
				lineNum = 5;
			//当前index地块所在列数
			int curLine = (int)Mathf.Ceil((float)numOfBlocks / 2);


			if (numOfBlocks % 2 == 0) {
				posX += spaceX;
			} else {
				posX -= spaceX;
			}

			if (curLine == lineNum && DataManager.maxBlockNum % 2 != 0 && DataManager.maxBlockNum <= maxRow * 2)
				posX = centerX;
			
			posZ = GetPosZ(lineNum,curLine);
		} 

		//10以上的地块
		if(DataManager.maxBlockNum > maxRow * 2){
			int lineNum = DataManager.maxBlockNum - maxRow * 2;
			if (index < lineNum*2) {
				if (numOfBlocks % 2 == 0) {
					posX += spaceX;
				} else {
					posX -= spaceX;
				}
			} else if(index >= maxRow*2) {
				posX = centerX;

				int curLine = 13 - numOfBlocks;
				posZ = centerZ + curLine * spaceZ;
			}

		}

		return new Vector3 (posX, posY, posZ);
	}

	//得到横屏x坐标
	private float GetPosX(int cowNum,int curCow){
		float startX = 5f;
		float posX = centerX;

		//以中心算横坐标，分为左右两侧
		int halfCowNum =  (int)Mathf.Ceil((float)cowNum / 2);
		//偶数列
		if (cowNum % 2 == 0) {
			startX = startX / 2;
			if(curCow <= halfCowNum){
				posX -= startX + spaceX * (halfCowNum - curCow);
			}else {
				posX += startX + spaceX * (curCow - halfCowNum - 1);
			}
		} else {
			if(curCow < halfCowNum){
				posX -= startX + spaceX * (halfCowNum - curCow - 1);
			}else if(curCow > halfCowNum){
				posX += startX + spaceX * (curCow - halfCowNum - 1);
			}
		}
		return posX;
	}

	// 计算地块坐标(横屏)
	private Vector3 GetBlockPosH(int index){
		// 计算坐标
		float posZ = centerZ;
		float posX = centerX;

		int numOfBlocks = index + 1;
		//10以内的地块
		if (index <= index2Col) {
			//地块列数
			int cowNum =  (int)Mathf.Ceil((float)DataManager.maxBlockNum / 2);
			if (cowNum > 5)
				cowNum = 5;
			//当前index地块所在列数
			int curCow = (int)Mathf.Ceil((float)numOfBlocks / 2);


			if (numOfBlocks % 2 == 0) {
				posZ -= spaceZ;
			} else {
				posZ += spaceZ;
			}

			if (curCow == cowNum && DataManager.maxBlockNum % 2 != 0 && DataManager.maxBlockNum <= maxRow * 2)
				posZ = centerZ;

			posX = GetPosX(cowNum,curCow);
		} 

		//10以上的地块
		if(DataManager.maxBlockNum > maxRow * 2){
			if (index <= index2Col) {
				posZ += spaceZ;
			} else {
				posZ = centerZ - spaceZ*2;
				int cowNum = DataManager.maxBlockNum - 10;
				int curCow = numOfBlocks - 10;

				posX = GetPosX(cowNum,curCow);
			}

		}

		return new Vector3 (posX, posY, posZ);
	}

	// 初始化建筑
	public void InitBuilding() {
		
	}

	// 创建建筑
    public void CreateBuilding(GameObject block, ModelBlock blockInfo) {
		if(blockInfo != null && blockInfo.BuildId != "0" && blockInfo.BuildLv != 0){
			GameObject building = Resources.Load("Prefabs/Defend/Build") as GameObject;
            building = Instantiate(building);
            building.transform.parent = block.transform;
			building.transform.localScale = BUILDSCALE;
            building.transform.localPosition = new Vector3(0f, 0f, 0f);
			//building.transform.localRotation = Quaternion.Euler(0.0f,135f,0.0f);
			building.transform.GetComponent<Build> ().level = blockInfo.BuildLv;
			building.GetComponent<Build> ().info = blockInfo;
			block.transform.GetComponent<Block>().info = blockInfo;
			building.GetComponent<Build> ().isInit = isInit;
			if (blockInfo.BuildState == 1 && blockInfo.DefendBlockId > 0) {
				building.GetComponent<Build> ().InitBuildingData ();
				Transform dBlock = defendPlaneMgr.GetDefendBlockById (blockInfo.DefendBlockId);
				if (dBlock != null) {
					building.GetComponent<Build> ().CreateRoleAttack (dBlock);
				}
			}
        }

//		building.transform.GetComponent<Blocks>().info = BlocksInfo
//		if (BlocksInfo [index].buildingStatus == 1) {
//			building.GetComponent<Build> ().CreateRoleOnCycle ();
//		}
	}

    public void CreateBuilding(GameObject block, int index, int level = 1)
	{
		GameObject building = Resources.Load("Prefabs/Defend/Build") as GameObject;
        building = Instantiate(building);
        building.transform.parent = block.transform;
		building.transform.localScale = BUILDSCALE;
        building.transform.localPosition = new Vector3(0f, 0f, 0f);
		//building.transform.localRotation = Quaternion.Euler(0.0f,135f,0.0f);
		building.transform.GetComponent<Build> ().level = level;

		ModelBlock blockInfo = new ModelBlock();
		blockInfo.BlockId = index;
		blockInfo.BuildId = "1";
		blockInfo.BuildLv = level;
		blockInfo.BuildState = 0;
		blockInfo.DefendBlockId = 0;
		ManagerBlock.SetBlockXmlData(blockInfo);
		//Debug.Log("new blcok Level: " +  level);
		building.GetComponent<Build> ().info = blockInfo;
		block.transform.GetComponent<Block>().info = blockInfo;
        //      building.transform.GetComponent<Blocks>().info = BlocksInfo
        //      if (BlocksInfo [index].buildingStatus == 1) {
        //          building.GetComponent<Build> ().CreateRoleOnCycle ();
        //      }
    }


    // 创建礼包 
	public void CreateGiftBox(int level, string type = "box") {
		//Debug.Log ("创建礼包");
		int emptyBlockId = GetEmptyBlockId();
		if (emptyBlockId != -1) {
			GameObject box = Resources.Load("Prefabs/Defend/" + type) as GameObject;
			box = Instantiate(box);
			box.transform.parent = Blocks[emptyBlockId].transform;
			if (type.Equals ("box")) {
				box.transform.localScale = BOXCALE;
			} else {
				box.transform.localScale = BOXCALE;
			}
			box.transform.localPosition = new Vector3(0f, 0f, 0f);
//			box.transform.localRotation = Quaternion.Euler(-90.0f, 0f, 0f);

			ModelBlock blockInfo = new ModelBlock();
			blockInfo.BlockId = emptyBlockId;
			blockInfo.BuildId = "1";
			blockInfo.BuildLv = level;
			blockInfo.BuildState = 0;
			blockInfo.DefendBlockId = 0;
			ManagerBlock.SetBlockXmlData(blockInfo);
			Blocks[emptyBlockId].transform.GetComponent<Block>().info = blockInfo;
			box.GetComponent<Box> ().info = blockInfo;
//			box.GetComponent<Box> ().info = blockInfo;
//			box.GetComponent<Box> ().lv = level;
			if(type.Equals("box")) {
				DataManager.boxLevel.RemoveAt (0);
				DataManager.SetBoxList ();
			}
			//ManagerUserInfo.SetDataByKey("BoxNum", DataManager.boxNum);
		}
	}

	public void CreatBuildingByOrder() {
//		int count = GetAllBlockCount();
//		// 条件判断
//
//		// 创建
//		// 是否有锁定地块
//		if(DataManager.isGoldVip == 0) {
//			count -= 1;
//		}
//		for(int i = 0; i < count; i++) {
//			if (Blocks [i].transform.childCount == PlaneGameManager.buildInBlockChildCount) {
//				CreateBuilding (Blocks [i], i, Random.Range(1, 40));
//				break;
//			}
//		}
	}

	public GameObject GetBlockById(int id){
		return Blocks [id];
	}

	public int GetAllBlockCount(){
		return Blocks.Count;
	}

	/// <summary>
	/// 获取地块上的建筑 没有返回null
	/// </summary>
	public Transform GetBuildInBlock(int id){
		return Blocks [id].transform.Find ("Build(Clone)");
	} 

	/// <summary>
	/// 获取空地块id 如果没有则返回-1
	/// </summary>
	public int GetEmptyBlockId(){
		int count = -1;
		for (int i = 0; i < Blocks.Count; i++)
		{
			if (Blocks[i].transform.childCount <= baseChildCount)
			{
				count = i;
				break;
			}
		}
		return count;
	}


	public int CreatBuildingByCoin(BigNumber showPrice,ModelBuild buildModel, int buildLv, int payType) {
        int buyByCoin = 0;
		int emptyBlockId = GetEmptyBlockId ();

		if(emptyBlockId != -1){
            bool isCanBuy = false;
            switch (payType)
            {
                case 0:
                    if(DataManager.coins.IsBiggerThan(showPrice)){
                        buyByCoin = 1;
                        isCanBuy = true;
						canvasManager.UpdateCoin ("-", showPrice);
//                        DataManager.coins.Minus(showPrice);
//                        canvasManager.coinText.text = DataManager.coins.ToString();
//						canvasManager.leftCoinText.text = DataManager.coins.ToString();
                    }
                    break;
                case 1:
                    if (DataManager.diamond.IsBiggerThan(showPrice))
                    {
                        isCanBuy = true;
						
						canvasManager.UpdateDiamond ("-", showPrice);
//                        DataManager.diamond.Minus(showPrice);
//                        canvasManager.diamondText.text = DataManager.diamond.ToString();
                    }
                    break;
                case 2:
                    isCanBuy = true;
                    break;
            }



            if (isCanBuy){
                //刷新每个建筑的数据
                buildModel.HistoryHaveNum = buildModel.HistoryHaveNum + 1;


				if (DataManager.getMaxBuildLv >= 8) {
					foreach(ModelTask taskInfo in DataManager.taskInfoList) {
						if(taskInfo.IsGet == 0 && taskInfo.NeedBuildLv == buildModel.BuildLv) {
							taskInfo.CurAchieveNum += 1;
							Debug.Log("ID:" + taskInfo.TaskId + " BuildLv: " + taskInfo.NeedBuildLv + " 进度:" + taskInfo.CurAchieveNum + "/" + taskInfo.NeedAchieveNum );
							ManagerTask.SetTaskXmlData (taskInfo);
							if(taskInfo.CurAchieveNum >= taskInfo.NeedAchieveNum) {
								canvasManager.newTipObj.SetActive (true);
								canvasManager.questsNewObj.SetActive (true);
							}
						}
					}
				}

                if(payType == 0){
                    buildModel.BuyNum = buildModel.BuyNum + 1;
                    canvasManager.InitShowBuild();
                }
                    
                ManagerBuild.SetBuildXmlData(buildModel);

				CreateBuilding(Blocks[emptyBlockId], emptyBlockId, buildLv);
				soundManager.SoundPayCash ();
            }else{
				soundManager.SoundUIClick ();
                switch (payType)
                {
					case 0:
						canvasManager.ShowWarning(ConfigFileMgr.GetContentByKey (EM_LocalizeConstants.Locaize_NoMoreCoin));
                        break;
                    case 1:
                        canvasManager.diamondStore.gameObject.SetActive(true);
                        break;
                }
            }

        }else{
			canvasManager.ShowWarning(ConfigFileMgr.GetContentByKey (EM_LocalizeConstants.Locaize_WarningNoMoreBlock));
        }
        return buyByCoin;
    }

	public void ShowExpEffect() {
		blocksManager.expEffect.SetActive (true);
		blocksManager.fireworksEffect.SetActive (true);
		StartCoroutine (ExpEffectDismiss ());
	}

	public IEnumerator ExpEffectDismiss() {
		blocksManager.expEffect.transform.parent = transform;
		blocksManager.fireworksEffect.transform.parent = transform;
		yield return new WaitForSeconds(3f);
		blocksManager.expEffect.SetActive (false);
		blocksManager.fireworksEffect.SetActive (false);
	}
}
