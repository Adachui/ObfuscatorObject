using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System.Xml.Linq;

public static class ManagerTask
{
    private static readonly string path = "TaskInfoXML";
    private static string persistentDataPath = "";
    private static XElement XelRoot;
    private static readonly string _root = "TaskInfo";
    private static List<Dictionary<string, string>> taskConfigList = new List<Dictionary<string, string>>();

    ///<summary>加载buddxml</summary>
    public static void LoadTaskInfoXml(List<Dictionary<string, string>> _taskConfigList)
    {
        taskConfigList = _taskConfigList;
        persistentDataPath = XmlManager.GetPersistentFilePath(path);
        if (File.Exists(persistentDataPath))
        {

        }
        else
        {
            InitTaskXml();
        }
        XelRoot = XmlManager.DecrtyptLoadXML(persistentDataPath);
    }

    public static void InitTaskXml()
    {
        TextAsset text = Resources.Load<TextAsset>("data/" + path);
        if (text != null)
        {
            XmlManager.InitFile(persistentDataPath, text.text);
        }
        else
        {
            XElement xml = new XElement(_root);

            for (int i = 0; i < taskConfigList.Count; i++)
            {
                int needLv = 0;
                int needNum = 0;
				if (taskConfigList [i] ["Cond"].IndexOf ('|') != -1) {
					string[] sArray = taskConfigList [i] ["Cond"].Split ('|');
					int.TryParse (sArray [0], out needLv);
					int.TryParse (sArray [1], out needNum);
				} else {
					int.TryParse (taskConfigList [i] ["Cond"], out needNum);
				}

                string name = "Task" + taskConfigList[i]["TaskId"];
                XElement newXel = new XElement(name,
                     new XElement("TaskId", taskConfigList[i]["TaskId"]),
                     new XElement("CurAchieveNum", 0),
                     new XElement("NeedAchieveNum", needNum),
                     new XElement("NeedBuildLv", needLv),
                     new XElement("IsGet", 0)
                );
                xml.Add(newXel);
            }
            XmlManager.InitFile(persistentDataPath, xml.ToString());
        }
    }


    ///<summary>设置单个荣耀成就信息（可用于更新数据）</summary>
    public static void SetTaskXmlData(ModelTask mTask)
    {
        if (XelRoot != null)
        {
            string name = "Task" + mTask.TaskId;
            XElement newXel = new XElement(name,
                 new XElement("TaskId", mTask.TaskId),
                 new XElement("CurAchieveNum", mTask.CurAchieveNum),
                 new XElement("NeedAchieveNum", mTask.NeedAchieveNum),
                 new XElement("NeedBuildLv", mTask.NeedBuildLv),
                 new XElement("IsGet", mTask.IsGet)
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


    ///<summary>获取荣耀成就信息列表</summary>
    public static List<ModelTask> GetTaskXmlData()
    {
        List<ModelTask> _TaskInfoList = new List<ModelTask>();
        if (File.Exists(persistentDataPath))
        {
            if (!XelRoot.IsEmpty)
            {
                IEnumerable xelNodeList = XelRoot.Elements();
                foreach (XElement xel1 in xelNodeList)
                {
                    ModelTask _mTask = XmlToModelTask(xel1);
                    _TaskInfoList.Add(_mTask);
                }
            }
        }
        return _TaskInfoList;
    }


    public static ModelTask GetTaskXmlDataById(int id)
    {

        ModelTask _mTask = new ModelTask();
        if (File.Exists(persistentDataPath))
        {
            if (!XelRoot.IsEmpty)
            {
                XElement xel1 = XelRoot.Element("Task" + id);

                _mTask = XmlToModelTask(xel1);
            }
        }
        return _mTask;
    }

    private static ModelTask XmlToModelTask(XElement xel1)
    {
        int curNum = 0;
        int needNUm = 0;
        int needLv = 0;
        int isGet = 0;
        int.TryParse(xel1.Element("CurAchieveNum").Value, out curNum);
        int.TryParse(xel1.Element("NeedAchieveNum").Value, out needNUm);
        int.TryParse(xel1.Element("NeedBuildLv").Value, out needLv);
        int.TryParse(xel1.Element("IsGet").Value, out isGet);
        ModelTask _mTask = new ModelTask
        {
            TaskId = xel1.Element("TaskId").Value,
            CurAchieveNum = curNum,
            NeedAchieveNum = needNUm,
            NeedBuildLv = needLv,
            IsGet = isGet
        };
        return _mTask;
    }

}
