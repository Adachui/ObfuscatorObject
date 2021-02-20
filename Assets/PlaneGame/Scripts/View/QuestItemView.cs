using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestItemView : AppAdvisoryHelper {
	public Text descriptionText;
	public Text progressText;
	public Image RewardIcon; 
	public Text RewardText;
	public Image progressImg;
	public Button ClaimBtn;

	public int rewardType;
	public BigNumber rewardNum = new BigNumber ();
	public ModelTask info; 


	public void InitData(ModelTask taskModel) {
		info = taskModel;
		Dictionary<string, string> taskInfo = GlobalKeyValue.GetXmlDataByKey ("TaskInfo", "Task" + taskModel.TaskId);

		string taskDesTitle = ConfigFileMgr.GetTaskDesSubffix ();
		descriptionText.text = taskInfo [taskDesTitle];

		string[] rewards = taskInfo ["Award"].Split('|');
		rewardType = int.Parse (rewards [0]);
		rewardNum.SetNumber (double.Parse (rewards [1]), 0);

		// 进度
		if (taskModel.CurAchieveNum >= taskModel.NeedAchieveNum) {
			progressText.text = taskModel.NeedAchieveNum.ToString () + "/" + taskModel.NeedAchieveNum.ToString ();
			float expProgress = 1;
			progressImg.fillAmount = expProgress;
			ClaimBtn.gameObject.SetActive (true);
			canvasManager.questsNewObj.SetActive (true);
			canvasManager.newTipObj.SetActive (true);
		} else if(int.Parse (taskModel.TaskId) >= 1872 && int.Parse (taskModel.TaskId) <= 1881) {
			if (DataManager.rank <= taskModel.NeedAchieveNum && DataManager.rank > 0) {
				progressText.text = "1/1";
				float expProgress = 1;
				progressImg.fillAmount = expProgress;
				ClaimBtn.gameObject.SetActive (true);
				canvasManager.questsNewObj.SetActive (true);
				canvasManager.newTipObj.SetActive (true);
			} else {
				progressText.text = "0/1";
				float expProgress = 0;
				progressImg.fillAmount = expProgress;
				ClaimBtn.gameObject.SetActive (false);
			}
		} else {
			progressText.text = taskModel.CurAchieveNum.ToString() + "/" + taskModel.NeedAchieveNum.ToString();
			float expProgress = (float)taskModel.CurAchieveNum / (float)taskModel.NeedAchieveNum;
			progressImg.fillAmount = expProgress;
			ClaimBtn.gameObject.SetActive (false);
		}


		//修改图片RewardIcon
		int id = int.Parse(taskModel.TaskId);
		if(id >=  1872 && id <= 1881) {
			Sprite itemSps = Resources.Load<Sprite> ("Sprites/Badge/" + (id - 1872 + 1));
			RewardIcon.sprite = itemSps;
		} else if (rewardType == 1) {
			Sprite[] itemSps = new Sprite[20];
			itemSps = Resources.LoadAll<Sprite>("Sprites/reward");
			RewardIcon.sprite = itemSps [1];
		} else if(rewardType == 2){
			Sprite[] itemSps = new Sprite[20];
			itemSps = Resources.LoadAll<Sprite>("Sprites/diamond_items_");
			RewardIcon.sprite = itemSps [19];
			RewardIcon.rectTransform.sizeDelta = new Vector2 (100, 85);
		} 
		RewardText.text = "X" + rewardNum.ToString();

	}

	public void GetReward() {
		switch (rewardType) {
		case 1:
			GetCoins ();
			break;
		default:
			GetDiamond ();
			break;
		}
		info.IsGet = 1;
		ManagerTask.SetTaskXmlData (info);

		canvasManager.newTipObj.SetActive (false);
		canvasManager.questsNewObj.SetActive (false);
		if (DataManager.getMaxBuildLv >= 8) {
			foreach (ModelTask taskInfo in DataManager.taskInfoList) {
				if (taskInfo.IsGet == 0 && int.Parse (taskInfo.TaskId) >= 1872 && int.Parse (taskInfo.TaskId) <= 1881) {
					if (DataManager.rank <= taskInfo.NeedAchieveNum && DataManager.rank > 0) {
						canvasManager.questsNewObj.SetActive (true);
						canvasManager.newTipObj.SetActive (true);
						break;
					} 
				}
				if (taskInfo.IsGet == 0 && taskInfo.CurAchieveNum >= taskInfo.NeedAchieveNum) {
					canvasManager.questsNewObj.SetActive (true);
					canvasManager.newTipObj.SetActive (true);
					break;
				}
			}
		}
		canvasManager.questsView.GetComponent<QuestsView>().UpdateAllData();

	}
		
	void GetDiamond() {
		canvasManager.UpdateDiamond ("+",rewardNum);
		canvasManager.diamondText.text = DataManager.diamond.ToString ();
		canvasManager.ShowDiamondAnimation ();
		soundManager.SoundRewardClaim ();
	}

	void GetCoins() {
		canvasManager.ShowCongratulationViewWithIcon (
			Resources.LoadAll<Sprite> ("Sprites/Congratulation")[4],
			"X"+rewardNum,
			0.6f
		);
		canvasManager.UpdateCoin ("+",rewardNum);
		canvasManager.coinText.text = DataManager.coins.ToString ();
		soundManager.SoundCurrencyExchange ();
	}
}
