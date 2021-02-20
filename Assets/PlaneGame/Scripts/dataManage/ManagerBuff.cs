using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using UnityEngine;
using System.Collections;

public static class ManagerBuff
{
    private static readonly string path = "BuffInfoXML";
    private static string persistentDataPath = "";
    private static XElement XelRoot;
    private static readonly string _root = "BuffInfo";
    private static List<Dictionary<string, string>> buffConfigList = new List<Dictionary<string, string>>();


    ///<summary>加载buddxml</summary>
    public static void LoadBuffInfoXml(List<Dictionary<string, string>> _buffConfigList)
    {
        buffConfigList = _buffConfigList;
        persistentDataPath = XmlManager.GetPersistentFilePath(path);
        if (File.Exists(persistentDataPath)){

        }else{
            InitBuffXml();
        }
        XelRoot = XmlManager.DecrtyptLoadXML(persistentDataPath);
    }

    public static void InitBuffXml()
    {
        TextAsset text = Resources.Load<TextAsset>("data/" + path);
        if (text != null)
        {
            XmlManager.InitFile(persistentDataPath, text.text);
        }
        else
        {
            XElement xml = new XElement(_root);

            for (int i = 0; i < buffConfigList.Count; i++)
            {
                string name = "Buff" + buffConfigList[i]["BuffId"];
                XElement newXel = new XElement(name,
                     new XElement("BuffId", buffConfigList[i]["BuffId"]),
                     new XElement("BuffLv", buffConfigList[i]["BuffLv"]),
                     new XElement("BuffState", 2),
                     new XElement("BuffType", buffConfigList[i]["BuffType"])
                );
                xml.Add(newXel);
            }
            XmlManager.InitFile(persistentDataPath, xml.ToString());
        }
    }

    ///<summary>设置单个buff信息（可用于更新数据）</summary>
    public static void SetBuffXmlData(ModelBuff mBuff)
    {
        if (XelRoot != null)
        {
			string name = "Buff" + mBuff.BuffId;
            XElement newXel = new XElement(name,
                 new XElement("BuffId", mBuff.BuffId),
                 new XElement("BuffLv", mBuff.BuffLv),
                 new XElement("BuffState", mBuff.BuffState),
                 new XElement("BuffType", mBuff.BuffType)
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

    ///<summary>获取buff信息列表</summary>
    public static List<IModel> GetBuffXmlData()
    {
        List<IModel> _BuffInfoList = new List<IModel>();
        if (File.Exists(persistentDataPath))
        {
            if (!XelRoot.IsEmpty){
                IEnumerable xelNodeList = XelRoot.Elements();
                foreach (XElement xel1 in xelNodeList)
                {
                    ModelBuff _mBuff = XmlToModelBuff(xel1);
                    _BuffInfoList.Add(_mBuff);
                }
            }
        }
        return _BuffInfoList;
    }

    public static List<IModel> SortList(List<IModel> _BuffInfoList)
    {
        _BuffInfoList.Sort((a, b) => {

            if ((a as ModelBuff).BuffState.CompareTo((b as ModelBuff).BuffState) != 0)
                return (a as ModelBuff).BuffState.CompareTo((b as ModelBuff).BuffState);
            else if ((a as ModelBuff).BuffType.CompareTo((b as ModelBuff).BuffType) != 0)
                return (a as ModelBuff).BuffType.CompareTo((b as ModelBuff).BuffType);
            else if ((a as ModelBuff).BuffId.CompareTo((b as ModelBuff).BuffId) != 0)
                return (a as ModelBuff).BuffId.CompareTo((b as ModelBuff).BuffId);
            else
                return 1;
        });
        return _BuffInfoList;
    }

    public static ModelBuff GetBuffXmlDataById(int id)
    {

        ModelBuff _mBuff = new ModelBuff();
        if (File.Exists(persistentDataPath))
        {
            if (!XelRoot.IsEmpty)
            {
                XElement xel1 = XelRoot.Element("Buff" + id);

                _mBuff = XmlToModelBuff(xel1);
            }
        }
        return _mBuff;
    }

    private static ModelBuff XmlToModelBuff(XElement xel1)
    {
        int id = 0;
        int lv = 0;
        int state = 0;
        int type = 0;
        int.TryParse(xel1.Element("BuffId").Value, out id);
        int.TryParse(xel1.Element("BuffLv").Value, out lv);
        int.TryParse(xel1.Element("BuffState").Value, out state);
        int.TryParse(xel1.Element("BuffType").Value, out type);
        ModelBuff _mBuff = new ModelBuff
        {
            BuffId = id,
            BuffLv = lv,
            BuffState = state,
            BuffType = type
        };
        return _mBuff;
    }
}
