using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System;
using System.Xml.Linq;

public static class ManagerBuild
{
    private static readonly string path = "BuildInfoXML";
    private static string persistentDataPath = "";
    private static XElement XelRoot;
    private static readonly string _root = "BuildInfo";
    private static List<Dictionary<string, string>> buildConfigList = new List<Dictionary<string, string>>();

    ///<summary>加载buddxml</summary>
    public static void LoadBuildInfoXml(List<Dictionary<string, string>> _buildConfigList)
    {
        buildConfigList = _buildConfigList;
        persistentDataPath = XmlManager.GetPersistentFilePath(path);
        if (File.Exists(persistentDataPath))
        {

        }
        else
        {
            InitBuildXml();
        }
        XelRoot = XmlManager.DecrtyptLoadXML(persistentDataPath);
    }

    public static void InitBuildXml()
    {
        TextAsset text = Resources.Load<TextAsset>("data/" + path);
        if (text != null)
        {
            XmlManager.InitFile(persistentDataPath, text.text);
        }
        else
        {
            XElement xml = new XElement(_root);

            for (int i = 0; i < buildConfigList.Count; i++)
            {
                string name = "Build" + buildConfigList[i]["BuildID"];
                XElement newXel = new XElement(name,
                     new XElement("BuildLv", buildConfigList[i]["BuildLv"]),
                     new XElement("BuildId", buildConfigList[i]["BuildID"]),
                     new XElement("BuyNum", 0),
                     new XElement("HistoryHaveNum", 0)
                );
                xml.Add(newXel);
            }
            XmlManager.InitFile(persistentDataPath, xml.ToString());
        }
    }

    ///<summary>设置单个build信息（可用于更新数据）</summary>
    public static void SetBuildXmlData(ModelBuild mBuild)
    {
        if (XelRoot != null)
        {
            string name = "Build" + mBuild.BuildId;
            XElement newXel = new XElement(name,
                 new XElement("BuildId", mBuild.BuildId),
                 new XElement("BuildLv", mBuild.BuildLv),
                 new XElement("BuyNum", mBuild.BuyNum),
                 new XElement("HistoryHaveNum", mBuild.HistoryHaveNum)
            );
            if (XelRoot.Element(name) != null && !XelRoot.Element(name).IsEmpty)
            {
                XmlManager.SetElementValue(persistentDataPath, XelRoot, name, newXel);
            }
            else
            {
                XmlManager.AddElement(persistentDataPath, XelRoot, newXel);
            }
            XelRoot = XmlManager.DecrtyptLoadXML(persistentDataPath);
        }

    }

    ///<summary>获取建筑信息列表</summary>
    public static List<IModel> GetBuildXmlData()
    {
        List<IModel> _BuildInfoList = new List<IModel>();
        if (File.Exists(persistentDataPath))
        {
            if (!XelRoot.IsEmpty)
            {
                IEnumerable xelNodeList = XelRoot.Elements();
                foreach (XElement xel1 in xelNodeList)
                {
					ModelBuild _mBuild = XmlToModelBuild (xel1);
                    _BuildInfoList.Add(_mBuild);
                }
            }
        }
        _BuildInfoList.Sort((a, b) => { return (a as ModelBuild).BuildLv.CompareTo((b as ModelBuild).BuildLv); });
        return _BuildInfoList;
    }

	public static ModelBuild GetBuildXmlDataById(int lv)
    {

		ModelBuild _mBuild = new ModelBuild();
        if (File.Exists(persistentDataPath))
        {
            if (!XelRoot.IsEmpty)
            {

				XElement xel1 = XelRoot.Element("Build" + lv);

				_mBuild = XmlToModelBuild(xel1);
            }
        }
		return _mBuild;
    }

	private static ModelBuild XmlToModelBuild(XElement xel1)
	{
		int lv = 0;
		int buyNum = 0;
		int historyHaveNum = 0;
		int.TryParse(xel1.Element("BuildLv").Value, out lv);
		int.TryParse(xel1.Element("BuyNum").Value, out buyNum);
		int.TryParse(xel1.Element("HistoryHaveNum").Value, out historyHaveNum);

		ModelBuild _mBuild = new ModelBuild
		{
			BuildLv = lv,
			BuildId = xel1.Element("BuildId").Value,
			BuyNum = buyNum,
			HistoryHaveNum = historyHaveNum,
		};
		return _mBuild;
	}

}
