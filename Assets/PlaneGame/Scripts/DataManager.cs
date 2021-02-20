using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyMobile;

public static class DataManager {

	//数值类/////////////////////////////////////////////////////////////////////////
	///<summary>金币</summary>
	public static BigNumber coins;
    ///<summary>累计收益</summary>
    public static BigNumber incomeAll;
    ///<summary>跑道金币</summary>
    public static BigNumber pathCoins;
	///<summary>钻石</summary>
	public static BigNumber diamond;
	///<summary>经验</summary>
	public static int exp = 0;
	///<summary>总经验</summary>
	public static int allExp = 0;
	///<summary>等级</summary>
	public static int lv = 0;
	///<summary>收益buff</summary>/// 
	public const float buffIncomeBase = 1;
	public static float buffIncome = 1;
	public static float buffSpeed = 1;
	///<summary>降价buff</summary>
	public static float buffCutPrice = 0f;
	///<summary>每秒收益</summary>
	public static BigNumber incomeSec;
	///<summary>速度buff（记录持续时间，以秒为单位）</summary>
	public static float buffSpeedTime = 0;
	///<summary>收益buff（记录持续时间，以秒为单位）</summary>
	public static float buffIncomeTime = 0;
	///<summary>当前得到的最大建筑等级</summary>
	public static int getMaxBuildLv = 1;
    ///<summary>当前金币可购买的最大建筑等级</summary>
    public static int maxBuildByCoinLv = 1;

	///<summary>宝箱获得时的等级</summary>
	public static List<int> boxLevel = new List<int>();
//	public static int boxLevel = 1;
	///<summary>待降落的宝箱数量</summary>
	public static int boxNum = 0;

    ///<summary>是否免费</summary>
	public static int isFree = 0;
	///<summary>根据时间计算是否免费</summary>
	public static float freeTime = 0;

	///<summary>在线时间</summary>
	public static float curOnLineTime = 0;
	///<summary>离线时间</summary>
	public static long offLineTime = 0;

	///<summary>当前跑道上的角色数量</summary>
	public static int roleNum = 0;
	///<summary>当前跑道上的最大角色数量</summary>
	public static int roleMaxNum = 1;
	public static int isGoldVip = -1;

    ///<summary>个人信息装备1，2</summary>
    public static int equipmentId1 = 0;
    public static int equipmentId2 = 0;
    //表类/////////////////////////////////////////////////////////////////////////
    ///<summary>当前等级的用户信息</summary>
    public static Dictionary<string, string> userConfigDic = new Dictionary<string, string>();
	///<summary>下一等级的用户信息</summary>
	public static Dictionary<string, string> nextUserConfigDic = new Dictionary<string, string>();

	///<summary>建筑信息配置表</summary>
	public static List<Dictionary<string, string>> buildConfigList = new List<Dictionary<string, string>>();
	///<summary>建筑信息表</summary>
	public static List<IModel> buildInfoList = new List<IModel>();

    ///<summary>buff信息配置表</summary>
    public static List<Dictionary<string, string>> buffConfigList = new List<Dictionary<string, string>>();
    ///<summary>buff信息表</summary>
    public static List<IModel> buffInfoList = new List<IModel>();

    ///<summary>任务信息配置表</summary>
    public static List<Dictionary<string, string>> taskConfigList = new List<Dictionary<string, string>>();
	///<summary>任务信息表</summary>
	public static List<ModelTask> taskInfoList = new List<ModelTask>();

	///<summary>钻石商店配置表</summary>
	public static List<Dictionary<string, string>> GemConfigList = new List<Dictionary<string, string>>();

	///<summary>单位配置表</summary>
	public static List<Dictionary<string, string>> UnitConfigList = new List<Dictionary<string, string>>();

	///<summary>转盘配置表</summary>
	public static List<Dictionary<string, string>> DialConfigList = new List<Dictionary<string, string>>();

    ///<summary>免费配置表</summary>
    //public static List<Dictionary<string, string>> FreeConfigList = new List<Dictionary<string, string>>();

	///<summary>高级宝箱配置表</summary>
	public static List<Dictionary<string, string>> HighRandomConfigList = new List<Dictionary<string, string>>();
	///<summary>宝箱配置表</summary>
	public static List<Dictionary<string, string>> RandomConfigList = new List<Dictionary<string, string>>();

	///<summary>炮台对应的攻击受击音效配置表</summary>
	public static List<Dictionary<string, string>> TowEffectConfigList = new List<Dictionary<string, string>>();

	///<summary>加速时间上限</summary>
	public static float speedUpTimeLimit = 1800f;

	///<summary>热气球出现间隔</summary>
	public static float ballonShowInterval = 1800; 
	public static int rank = 999; 

	public static long offLineTimeSec = 0;
	public static int offLineMultiple = 0;
	public static int adWacthTimes = 0;
	public static int specialOfferTime = 0;
	public static bool isSpecialPurchase = false;

	///<summary>当前地块数量（配置表根据等级取出值）</summary>
	public static int curBlockNum = 4;
	///<summary>最大地块数量（未解锁地块或购买的两个地块）</summary>
	public static int maxBlockNum = 4;

	public static bool GetOffLineTime() {
		PlaneGameManager.lastOnlineTime = PlayerPrefs.GetFloat("ONLINETIME", 0);
		long lastLoginTime = ManagerUserInfo.GetLongDataByKey("LoginTime");
		long LoginTime = ConvertDateTimeToLong(System.DateTime.Now);
		ManagerUserInfo.SetDataByKey("LoginTime", LoginTime.ToString());
		//Debug.Log("LoginTime: " +  LoginTime  + " - lastLoginTime: " + lastLoginTime + " - lastOnlineTime: " + (long)PlaneGameManager.lastOnlineTime);
		int Multiple = 0;
		if (ManagerUserInfo.GetIntDataByKey ("NewPlayer") == 0) {
			return false;
		}
		if (incomeSec.number - 0 < 0.01) {
			return false;
		}
		if (lastLoginTime != 0){
			offLineTimeSec = LoginTime - lastLoginTime - (long)PlaneGameManager.lastOnlineTime;
			Debug.Log ("offLineTimeSec :" + offLineTimeSec);
			if (offLineTimeSec <= 0) {
				offLineTimeSec = 0;
				return false;
			} 
			long offLineTime =  Mathf.RoundToInt((float)offLineTimeSec / 3600f);
			Debug.Log ("offLineTimeSec :" + offLineTimeSec + " offLineTime: " + offLineTime);
			List<Dictionary<string, string>> OfflineConfigList = GlobalKeyValue.GetXmlData("OfflineInfo");
			if (offLineTime > 24) {
				offLineTime = 24;
				offLineTimeSec = 24 * 3600;
			}
			for (int i = OfflineConfigList.Count - 1;i >= 0;i--) {
				Dictionary<string, string> dic = OfflineConfigList[i];
				if (offLineTime >= int.Parse(dic["MinTime"]) && offLineTime <= int.Parse(dic["MaxTime"]))
				{
					Multiple = int.Parse(dic["Multiple"]);
				}
			}
		}
		int multipleTimes = ManagerUserInfo.GetIntDataByKey ("offlineTimes" + Multiple);
		if(Multiple > 0 && DataManager.incomeSec.number > 0 && multipleTimes < Multiple){
			multipleTimes++;
			offLineMultiple = Multiple;
			ManagerUserInfo.SetDataByKey ("offlineTimes" + Multiple, multipleTimes);
			return true;
			//弹出离线收益界面
		}
		return false;
	}


	//初始化数据信息
	public static void InitData()
	{
//		Debug.Log("System.DateTime.Now == " + System.DateTime.Now);
//		Debug.Log("System.DateTime.Now 22 == " + ConvertDateTimeToLong(System.DateTime.Now));
//		Debug.Log("System.DateTime.Now 33 == " + ManagerUserInfo.GetIntDataByKey("CoinsUnit"));

		coins = new BigNumber (ManagerUserInfo.GetDoubleDataByKey("Coins"), ManagerUserInfo.GetIntDataByKey("CoinsUnit"));
        incomeAll = new BigNumber(ManagerUserInfo.GetDoubleDataByKey("IncomeAll"), ManagerUserInfo.GetIntDataByKey("IncomeAllUnit"));
        pathCoins = new BigNumber( ManagerUserInfo.GetDoubleDataByKey("PathCoins"), ManagerUserInfo.GetIntDataByKey("PathCoinsUnit"));
		diamond = new BigNumber( ManagerUserInfo.GetDoubleDataByKey("Diamond"), ManagerUserInfo.GetIntDataByKey("DiamondUnit"));
//		Debug.LogError (" InitData diamond " + ManagerUserInfo.GetDoubleDataByKey("Diamond").ToString());
//		Debug.LogError (" InitData diamond1111   " + diamond.ToString());

		exp = ManagerUserInfo.GetIntDataByKey("Exp");
		allExp = ManagerUserInfo.GetIntDataByKey("AllExp");
		lv = ManagerUserInfo.GetIntDataByKey("Level");
		buffSpeed = ManagerUserInfo.GetFloatDataByKey("SpeedBuffNum");
		buffIncome = ManagerUserInfo.GetFloatDataByKey("IncomeBuffNum");
		buffCutPrice = ManagerUserInfo.GetFloatDataByKey("CutPriceBuffNum");
		//incomeSec = ManagerUserInfo.GetIntDataByKey("IncomeSecond");
		incomeSec = new BigNumber (ManagerUserInfo.GetDoubleDataByKey("IncomeSecond"), ManagerUserInfo.GetIntDataByKey("IncomeSecondUnit"));

		buffSpeedTime = ManagerUserInfo.GetFloatDataByKey("SpeedBuffTime");
		buffIncomeTime = ManagerUserInfo.GetFloatDataByKey("BuffIncomeTime");

		//Debug.Log (string.Format ("GET {0} -- {1}", buffSpeedTime, buffIncomeTime));
		isFree = ManagerUserInfo.GetIntDataByKey("IsExitFree");

        getMaxBuildLv = ManagerUserInfo.GetIntDataByKey("GetMaxBuildLv");
        equipmentId1 = ManagerUserInfo.GetIntDataByKey("equipmentId1");
        equipmentId2 = ManagerUserInfo.GetIntDataByKey("equipmentId2");
		InitBuildInfo ();


		GetBoxList ();
		boxNum = ManagerUserInfo.GetIntDataByKey("BoxNum");
		isGoldVip = ManagerUserInfo.GetIntDataByKey("IsGoldVip");
		rank = ManagerUserInfo.GetIntDataByKey("UserRank");

		specialOfferTime = ManagerUserInfo.GetIntDataByKey ("SpecialOffer");

		Debug.LogError ("***** DataManager****** " + specialOfferTime);


    }

	public static void GetBoxList() {
		boxLevel.Clear ();
		string bl = ManagerUserInfo.GetStringValueByKey ("BoxLevel");

		if (bl != "") {
			string[] bls = bl.Split (',');
			for (int i = 0; i < bls.Length; i++) {
				int ilv = 1;
				int.TryParse(bls[i], out ilv);
				if(ilv >= 1)
					boxLevel.Add (ilv);
			}
		}
	}

	public static void SetBoxList() {
		string bl = "";
		for (int i = 0; i < boxLevel.Count; i++) {
			if(i == (boxLevel.Count -1))
				bl += boxLevel [i].ToString ();
			else
				bl += boxLevel [i].ToString () + ",";
		}
		ManagerUserInfo.SetDataByKey ("BoxLevel", bl);
	}
	//初始化所有的xml数据
	public static void InitXmlData()
	{
		//GlobalKeyValue.InitDictionary();

		ManagerBlock.LoadBlockInfoXml();

        InitData();

		InitListData();

		curBlockNum = int.Parse (userConfigDic ["MaxBuild"]);
		maxBlockNum = curBlockNum;

		InitGoldVip ();

		// 跑道数据
		roleMaxNum = GlobalKeyValue.GetIntDataByKey (userConfigDic, "MaxBuildPlay");
		//roleMaxNum = isGoldVip > 0 ? roleMaxNum + 2 : roleMaxNum;
	}

	//初始化所有的数据
	private static void InitListData()
	{

		userConfigDic = GlobalKeyValue.GetXmlDataByKey("UserInfo", "User" + ManagerUserInfo.GetIntDataByKey("Level"));
		int nextLv = lv + 1;
		if(nextLv > ManagerUserInfo.MaxLevel){
			nextLv = ManagerUserInfo.MaxLevel;
		}

		nextUserConfigDic = GlobalKeyValue.GetXmlDataByKey("UserInfo", "User" + nextLv);

		buildConfigList = GlobalKeyValue.GetXmlData("BuildInfo");
		buildInfoList = ManagerBuild.GetBuildXmlData();
		if (buildInfoList.Count == 0)
		{
            ManagerBuild.LoadBuildInfoXml(buildConfigList);
			buildInfoList = ManagerBuild.GetBuildXmlData();
		}
		Debug.Log (buildInfoList);
        buffConfigList = GlobalKeyValue.GetXmlData("BuffInfo");
        buffInfoList = ManagerBuff.GetBuffXmlData();
        if (buffInfoList.Count == 0)
        {
            ManagerBuff.LoadBuffInfoXml(buffConfigList);
            buffInfoList = ManagerBuff.GetBuffXmlData();
        }
        InitBuffInfo();

        taskConfigList = GlobalKeyValue.GetXmlData("TaskInfo");
		taskInfoList = ManagerTask.GetTaskXmlData();
		if (taskInfoList.Count == 0)
		{
            ManagerTask.LoadTaskInfoXml(taskConfigList);
			taskInfoList = ManagerTask.GetTaskXmlData();
		}

		UnitConfigList = GlobalKeyValue.GetXmlData("UnitInfo");

		HighRandomConfigList = GlobalKeyValue.GetXmlData("HighRandomInfo");

		RandomConfigList = GlobalKeyValue.GetXmlData("RandomInfo");

		TowEffectConfigList = GlobalKeyValue.GetXmlData("TowerEffectsInfo");


    }

	// 初始化黄金会员加成
	public static void InitGoldVip() {
		if (isGoldVip > 0) {
			buffSpeed = 1.5f;
		}
	}

	//钻石商店初始化
	public static void InitGemInfo(){
		if(GemConfigList.Count == 0){
			GemConfigList = GlobalKeyValue.GetXmlData("GemInfo");
		}
	}

    //build信息初始化
    public static void InitBuildInfo(){
        Dictionary<string, string> _dic = GlobalKeyValue.GetXmlDataByKey("BuildInfo", "Build" + getMaxBuildLv);

        int lv = 0;
        maxBuildByCoinLv = int.TryParse(_dic["UnlockLv"], out lv) ? lv : maxBuildByCoinLv;
    }

    //buff道具表初始化
    public static void InitBuffInfo(){
        foreach(IModel _model in buffInfoList){
            ModelBuff buffModel = _model as ModelBuff;

            int maxLv = equipmentId1 + 1;

            if(buffModel.BuffType == 2)
                maxLv = equipmentId2 + 1;
                
            if (buffModel.BuffLv <= maxLv)
            {
                buffModel.BuffState = 1;
            }
            else if (buffModel.BuffLv == maxLv + 1)
            {
                buffModel.BuffState = 0;
            }
        }
        buffInfoList = ManagerBuff.SortList(buffInfoList);
    }

	public static long ConvertDateTimeToLong(System.DateTime time)
	{
		System.DateTime startTime = System.TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
		return (long)(time - startTime).TotalSeconds;
	}

	public static string SecondToHours(float time)
	{
		int hours = (int)(time / 3600);
		int minute = (int)(time % 3600 / 60);
		int second = (int)(time % 3600 % 60);
		return hours + ":" + minute + ":" + second;
	}

	public static long GetSpecialOfferTimeLeft()
	{
		long now = DataManager.ConvertDateTimeToLong (System.DateTime.Now);

//		Debug.LogError ("****** GetSpecialOfferTimeLeft now  ****** " + now);
//		Debug.LogError ("****** GetSpecialOfferTimeLeft specialOfferTime  ****** " + specialOfferTime);
//
//
		if ((now - DataManager.specialOfferTime) > 86400)
			return 0;

//		Debug.LogError ("****** GetSpecialOfferTimeLeft specialOfferTime consume ****** " + (now - specialOfferTime));
		return now - DataManager.specialOfferTime;
	}

	public static void SetSpecialOfferTime(long time)
	{
		ManagerUserInfo.SetDataByKey ("SpecialOffer",(int)time);
		specialOfferTime = (int)time;
//
//		Debug.LogError ("****** SetSpecialOfferTime ****** " + time);
//
//		Debug.LogError ("****** SetSpecialOfferTime 11111 ****** " + ManagerUserInfo.GetIntDataByKey("SpecialOffer"));
	}

    public static string GetRewardNameByType(string type){
        string name = "";
        switch(type){
            //时间道具
            case "1":
                name = "h";
                break;
            //金箱子
            case "2":
                name = "";
                break;
            //加速道具
            case "3":
                name = "s";
                break;
            //收益提升
            case "4":
                name = "coins";
                break;
            //钻石
            case "5":
                name = "";
                break;

        }
        return name;
    }

    //获取宝箱里的建筑级别
	public static int GetBuildBoxLv(int randomType, int lv)
    {
        int boxLv = 1;
        Dictionary<string, string> _dic = new Dictionary<string, string>();
        switch (randomType)
        {
            case 0:
                _dic = RandomConfigList[lv - 1];
                break;
            case 1:
                _dic = HighRandomConfigList[lv - 1];
                break;
        }

        int minLv = 1;
        int maxLv = 1;
        if (_dic["RandomLv"].IndexOf('#') != -1)
        {
            string[] sArray = _dic["RandomLv"].Split('#');
            int.TryParse(sArray[0], out minLv);
            int.TryParse(sArray[1], out maxLv);
        }
        else
        {
            int.TryParse(_dic["RandomLv"], out boxLv);
            return boxLv;
        }

        int minRate = 100;
        int maxRate = 0;
        if (_dic["RandomWeight"].IndexOf('#') != -1)
        {
            string[] sArray = _dic["RandomWeight"].Split('#');
            int.TryParse(sArray[0], out minRate);
            int.TryParse(sArray[1], out maxRate);
        }

        int num = Random.Range(1, 100);
        if (num <= minRate)
            boxLv = minLv;
        else if (num <= 100)
            boxLv = maxLv;

        return boxLv;
    }
		
}
