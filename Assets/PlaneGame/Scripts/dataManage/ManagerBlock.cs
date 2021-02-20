using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System.Xml.Linq;

public static class ManagerBlock
{
    private static readonly string path = "BlockInfoXML";
    private static string persistentDataPath = "";
    private static XElement XelRoot;
    private static readonly string _root = "BlockInfo";

    private static void CheckXmlFile()
    {
        persistentDataPath = XmlManager.GetPersistentFilePath(path);
        //xml是否存在，不存在则创建
        TextAsset text = Resources.Load<TextAsset>("data/" + path);
        if(text != null){
            XmlManager.InitFile(persistentDataPath, text.text);
        }
        else if (text == null && !File.Exists(persistentDataPath))
        {

            XElement xml = new XElement(_root,
                new XElement("Block0",
                     new XElement("BlockId", 0),
                     new XElement("BuildId", 1),
                     new XElement("BuildLv", 1),
					 new XElement("BuildState", 0),
					 new XElement("DefendBlockId", 0)
                    ),
                new XElement("Block1",
                     new XElement("BlockId", 1),
                     new XElement("BuildId", 0),
                     new XElement("BuildLv", 0),
					 new XElement("BuildState", 0),
					 new XElement("DefendBlockId", 0)
                    ),
                new XElement("Block2",
                     new XElement("BlockId", 2),
                     new XElement("BuildId", 0),
                     new XElement("BuildLv", 0),
					 new XElement("BuildState", 0),
					 new XElement("DefendBlockId", 0)
                    ),
                new XElement("Block3",
                     new XElement("BlockId", 3),
                     new XElement("BuildId", 0),
                     new XElement("BuildLv", 0),
					 new XElement("BuildState", 0),
					 new XElement("DefendBlockId", 0)
                    )
            );
            XmlManager.InitFile(persistentDataPath, xml.ToString());
        }
    }

    ///<summary>加载地块xml</summary>
    public static void LoadBlockInfoXml(){
        CheckXmlFile();
        if (File.Exists(persistentDataPath))
            XelRoot = XmlManager.DecrtyptLoadXML(persistentDataPath);
    }

    ///<summary>设置单个地块信息（可用于更新数据）</summary>
    public static void SetBlockXmlData(ModelBlock mBlock)
    {
		Debug.Log ("ModelBlockModelBlock = " + mBlock.BlockId + " == mBlock.BuildLv ==" + mBlock.BuildLv);
        if (XelRoot != null){
            string name = "Block" + mBlock.BlockId;
            XElement newXel = new XElement(name,
                 new XElement("BlockId", mBlock.BlockId),
                 new XElement("BuildId", mBlock.BuildId),
                 new XElement("BuildLv", mBlock.BuildLv),
				 new XElement("BuildState", mBlock.BuildState),
				 new XElement("DefendBlockId", mBlock.DefendBlockId)
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


    ///<summary>获取地块信息列表</summary>
    public static List<ModelBlock> GetBlockXmlData()
    {
        List<ModelBlock> _BlockInfoList = new List<ModelBlock>();
        if (File.Exists(persistentDataPath))
        {
            if (!XelRoot.IsEmpty)
            {
                IEnumerable xelNodeList = XelRoot.Elements();
                foreach (XElement xel1 in xelNodeList)
                {
					ModelBlock _mBlock = XmlToModelBlock (xel1);
					_BlockInfoList.Add(_mBlock);
                }
            }
        }
        return _BlockInfoList;
    }

	public static ModelBlock GetBlockXmlDataById(int id)
	{

		ModelBlock _mBlock = new ModelBlock();
		if (File.Exists(persistentDataPath))
		{
			if (!XelRoot.IsEmpty)
			{
				XElement xel1 = XelRoot.Element("Block" + id);

				_mBlock = XmlToModelBlock(xel1);
			}
		}
		return _mBlock;
	}

	private static ModelBlock XmlToModelBlock(XElement xel1)
	{
		int id = 0;
		int lv = 0;
		int state = 0;
		int did = 0;
		int.TryParse(xel1.Element("BlockId").Value, out id);
		int.TryParse(xel1.Element("BuildLv").Value, out lv);
		int.TryParse(xel1.Element("BuildState").Value, out state);
		int.TryParse(xel1.Element("DefendBlockId").Value, out did);
		ModelBlock _mBlock = new ModelBlock {
			BlockId = id,
			BuildId = xel1.Element ("BuildId").Value,
			BuildLv = lv,
			BuildState = state,
			DefendBlockId = did
		};

		return _mBlock;
	}
}
