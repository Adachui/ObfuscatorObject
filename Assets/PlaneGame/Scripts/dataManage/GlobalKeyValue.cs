using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;

public static class GlobalKeyValue
{
    private static XmlDocument curXmlDoc = new XmlDocument();
    public static Dictionary<string, XmlDocument> LoadFormXmlDic = new Dictionary<string, XmlDocument>();

    ///<summary>进游戏加载xml</summary> 
    public static void LoadDataXml(string _root)
    {
		string path = "config/" + _root;//Application.dataPath + "/PlaneGame/Resources/config/";
        //path = path + _root +".xml";
		TextAsset text = Resources.Load<TextAsset>(path);
		if(text != null)
        {
//            bool isExit = LoadFormXmlDic.ContainsKey(_root);
            if (!LoadFormXmlDic.ContainsKey(_root))
            {
                XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.LoadXml(text.text);
                LoadFormXmlDic.Add(_root, xmlDoc);
                curXmlDoc = xmlDoc;
            }else{
                curXmlDoc = LoadFormXmlDic[_root];
            }
        }
    }

    public static List<Dictionary<string, string>> GetXmlData(string _root)
    {
        LoadDataXml(_root);

        List<Dictionary<string, string>> _infoList = new List<Dictionary<string, string>>();

        XmlNodeList xmlNodeList1 = curXmlDoc.SelectSingleNode(_root).ChildNodes;
        foreach (XmlElement xl1 in xmlNodeList1){
            XmlNodeList xmlNodeList2 = xl1.ChildNodes;
            Dictionary<string, string> _dic = new Dictionary<string, string>();
            foreach (XmlElement xl2 in xmlNodeList2)
            {

                //Debug.Log(xl2.Name + " == " + xl2.InnerText);
                _dic.Add(xl2.Name, xl2.InnerText);

            }
            _infoList.Add(_dic);
        }
        return _infoList;
    }

    public static Dictionary<string, string> GetXmlDataByKey(string _root, string _name)
    {
        LoadDataXml(_root);

        Dictionary<string, string> _dic = new Dictionary<string, string>();
        XmlNode rootNode = curXmlDoc.SelectSingleNode(_root);
		if(_root == ManagerUserInfo._root){
			var c = rootNode.ChildNodes.Count;
			ManagerUserInfo.MaxLevel = c;
		}

        XmlNodeList nameNodeList = rootNode.SelectSingleNode(_name).ChildNodes;
        foreach (XmlElement xl1 in nameNodeList)
        {
            //Debug.Log(xl1.Name + " == " + xl1.InnerText);
            _dic.Add(xl1.Name, xl1.InnerText);
        }
        return _dic;
    }

	public static int GetIntDataByKey(Dictionary<string, string> _dic,string key, int value = 0)
	{
		int result = 0;

		result = int.TryParse(_dic[key], out result) ? result : value;
		return result;
	}

	public static float GetFloatDataByKey(Dictionary<string, string> _dic,string key, int value = 0)
	{
		float result = 0;

		result = float.TryParse(_dic[key], out result) ? result : value;
		return result;
	}

	public static double GetDoubleDataByKey(Dictionary<string, string> _dic,string key, int value = 0)
	{
		double result = 0;

		result = double.TryParse(_dic[key], out result) ? result : value;
		return result;
	}

    //public static Dictionary<string, string> BlockInfoXmlDic = new Dictionary<string, string>();
    //public static Dictionary<string, string> BuffInfoXmlDic = new Dictionary<string, string>();
    //public static Dictionary<string, string> AchieveInfoDic = new Dictionary<string, string>();
    //public static Dictionary<string, string> PathInfoDic = new Dictionary<string, string>();
    //public static Dictionary<string, string> BlockInfoDic = new Dictionary<string, string>();

    //private static string[] rootKey = new string[]{ "BlockInfo" , "BuffInfo" };
    //private static Dictionary<string, string>[] rootDic = new Dictionary<string, string>[] { BlockInfoXmlDic, BuffInfoXmlDic };

    //public static void InitDictionary()
    //{
    //    TextAsset text = Resources.Load<TextAsset>(path);
    //    if (text)
    //    {
    //        XmlDocument xmlDoc = new XmlDocument();
    //        xmlDoc.LoadXml(text.text);

    //        for (int i = 0; i < rootKey.Length;i++){
    //            var node = xmlDoc.SelectSingleNode(rootKey[i]);
    //            if (node != null)
    //            {
    //                XmlNodeList xmlNodeList = node.ChildNodes;
    //                foreach (XmlElement xl1 in xmlNodeList)
    //                {
    //                    Debug.Log(xl1.Name + " == " + xl1.InnerText);
    //                    rootDic[i].Add(xl1.Name, xl1.InnerText);
    //                }
    //            }
    //        }
    //    }
    //}

    //public static bool GetIndexByKey(Dictionary<string, string> dic,string key,out int index)
    //{
    //    key = "eds";
    //    string idx = "";
    //    index = 0;
    //    if (dic.TryGetValue(key, out idx))
    //    {
    //        if(int.TryParse(idx, out index)){
    //            return true;
    //        }else{
    //            return false;
    //        }
    //    }else{
    //        return false;
    //    }
    //}
}
