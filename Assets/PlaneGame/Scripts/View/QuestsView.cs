using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyMobile;

public class QuestsView : AppAdvisoryHelper {
	public Transform Content;

	public List<GameObject> dailyTasks = new List<GameObject>();
	public List<GameObject> tasks = new List<GameObject>();
	// Use this for initialization
	public List<ModelTask> dailyTasksData = new List<ModelTask>();
	public List<ModelTask> tasksData = new List<ModelTask>();
	public List<ModelTask> taskAvchievement = new List<ModelTask>(); 
	public GameObject dalyTitle;
	private GameObject title;
	void Start () {
		UpdateData (false);
	}
	
//	// Update is called once per frame
//	void Update () {
//		
//	}

	void OnEnable() {
		dalyTitle.SetActive (true);
		UpdateAllData ();
	}

	public void UpdateAllData() {
		UpdateData ();
		FixContentSize();
	}

	void OnDisable() {
		
	}


	public void UpdateData(bool isRefresh = true) {
		DataManager.taskInfoList = ManagerTask.GetTaskXmlData();
		GetDialyTaskData ();
		GetTaskData();
		GetAhievementsData ();
		SetDailyTaskItem (isRefresh);
		SetTaskItem (isRefresh);

	}


	public void GetDialyTaskData() {
		dailyTasksData.Clear ();
		foreach (ModelTask taskModel in DataManager.taskInfoList) {
			if (taskModel.IsGet < 1) {
				int taskId = int.Parse(taskModel.TaskId);
				if (taskId >= 1900 && taskId < 2000) {
					dailyTasksData.Add (taskModel);
				}
			}
		}
	}
	public void GetAhievementsData() {
		taskAvchievement.Clear ();
		foreach (ModelTask taskModel in DataManager.taskInfoList) {
			if (taskModel.IsGet < 1) {
				int taskId = int.Parse(taskModel.TaskId);
				if (taskId >= 1872 && taskId <= 1881) {
					taskAvchievement.Add (taskModel);
					break;
				}
			}
		}
	}
		
	public void GetTaskData() {
		
		tasksData.Clear ();
		foreach (ModelTask taskModel in DataManager.taskInfoList) {
			if (taskModel.IsGet < 1) {
				int taskId = int.Parse (taskModel.TaskId);
				if (taskId < 1872) {
					tasksData.Add (taskModel);
					if (tasksData.Count >= 9) {
						break;
					}
				}
			}
		}
	}


	void SetDailyTaskItem(bool isRefresh = true) {
		if (dailyTasksData.Count == 0) {
			dalyTitle.SetActive (false);
		}
		if (isRefresh) {
			if (dailyTasks.Count != dailyTasksData.Count) {
				SetDailyTaskItem (false);
				return;
			}
			for(int i = 0; i < dailyTasks.Count; i++) {
				dailyTasks[i].GetComponent<QuestItemView> ().InitData (dailyTasksData[i]);
			}
		} else {
			for (int i = 0; i < dailyTasks.Count; i++) {
				Destroy (dailyTasks [i]);
			}
			dailyTasks.Clear ();
			foreach (ModelTask taskModel in dailyTasksData) {
				GameObject item = Resources.Load("Prefabs/UI/QuestItem") as GameObject;
				item = Instantiate(item);
				item.transform.SetParent(Content);
				item.transform.localPosition = Vector3.zero;
				item.transform.localScale = new Vector3(1, 1, 1);
				item.GetComponent<QuestItemView> ().InitData (taskModel);
				dailyTasks.Add (item);
			}
		}
	}
		
	public void SetTaskItem(bool isRefresh = true) {
		if (isRefresh) {
			if (tasks.Count != tasksData.Count) {
				SetTaskItem (false);
				return;
			}
			for(int i = 0; i < tasks.Count; i++) {
				tasks[i].GetComponent<QuestItemView> ().InitData (tasksData[i]);
			}
		} else {
			if (title) {
				Destroy (title);
			}
			for (int i = 0; i < tasks.Count; i++) {
				Destroy (tasks [i]);
			}
			tasks.Clear ();

			title = Resources.Load("Prefabs/UI/Title") as GameObject;
			title = Instantiate(title);
			title.transform.SetParent(Content);
			title.transform.localScale = new Vector3(1, 1, 1);

			var textComponent = title.transform.GetChild (0).transform.GetComponent<Text> ();
			textComponent.text = ConfigFileMgr.GetContentByKey(EM_LocalizeConstants.Locaize_ACHIEVEMENTS,textComponent);

			foreach (ModelTask taskModel in tasksData) {
				GameObject item = Resources.Load("Prefabs/UI/QuestItem") as GameObject;
				item = Instantiate(item);
				item.transform.SetParent(Content);
				item.transform.localPosition = Vector3.zero;
				item.transform.localScale = new Vector3(1, 1, 1);
				item.GetComponent<QuestItemView> ().InitData (taskModel);
				tasks.Add (item);
			}

			if (taskAvchievement.Count >= 1) {
				GameObject item = Resources.Load("Prefabs/UI/QuestItem") as GameObject;
				item = Instantiate(item);
				item.transform.SetParent(Content);
				item.transform.localScale = new Vector3(1, 1, 1);
				item.transform.localPosition = Vector3.zero;
				item.GetComponent<QuestItemView> ().InitData (taskAvchievement[0]);
				tasks.Add (item);
			}
		}
	}

	public void FixContentSize() {
		int count = dailyTasks.Count + tasks.Count;
		count = count > 3 ? count + 2 : count;
		Content.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, 155 * count);
	}
}
