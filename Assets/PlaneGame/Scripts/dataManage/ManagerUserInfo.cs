using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;

public static class ManagerUserInfo
{

	public static int MaxLevel = 30;
	public static string _root = "UserInfo";

    ///<summary>通过key获取key对应的值</summary> 举例 GetDataByKe("Level");
    public static string GetStringValueByKey(string key)
    {
        return PlayerPrefs.GetString(key, "");
    }

    public static int GetIntDataByKey(string key)
    {
        int num = 0;
        switch(key)
        {	
			case "DialAdsCount":
				num = 3;
				break;
			case "BoxLevel":
			case "GetMaxBuildLv":
            case "Level":
            case "SoundEffect":
            case "SoundMusic":
                num = 1;
                break;
            case "BlockNum":
                num = 4;
                break;
			case "UserRank":
            case "equipmentId1":
            case "equipmentId2":
                num = -1;
                break;
        }
        int result = PlayerPrefs.GetInt(key, num);
        //Debug.Log(key + " == " + result);
        return result;
    }

	public static float GetFloatDataByKey(string key)
	{
		float num = 0;
		switch(key)
		{	
			case "SpeedBuffNum":
			case "IncomeBuffNum":
				num = 1f;
				break;
		}
		float result = PlayerPrefs.GetFloat(key, num);
		return result;
    }
    
	public static double GetDoubleDataByKey(string key)
	{
		double result = 0;

		result = double.TryParse(PlayerPrefs.GetString(key, ""), out result) ? result : 0;
		return result;
	}

    public static long GetLongDataByKey(string key)
    {
        long result = 0;

        result = long.TryParse(PlayerPrefs.GetString(key, ""), out result) ? result : 0;
        return result;
    }


    ///<summary>通过key设置key对应的值</summary>  举例 SetDataByKe("Level",10);
    public static void SetDataByKey(string key,int num)
    {
        PlayerPrefs.SetInt(key, num);
    }

    public static void SetDataByKey(string key, string num)
    {
        PlayerPrefs.SetString(key, num);
    }
	public static void SetDataByKey(string key, float num)
	{
		PlayerPrefs.SetFloat(key, num);
	}

	public static void SaveData()
	{
		PlayerPrefs.Save ();
	}
}
