using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyMobile;

public class UserInfoView : AppAdvisoryHelper
{
	
	public GameObject UserInfoLayer;
	public GameObject AchievementLayer;
	public GameObject SettingLayer;

	public Button[] TabBtns;

    public Text playerLvText;
    public Text playerNameText;
    public Text coinText;
    public Text incomeSecText;
    public Text rankingText;
	public Text CollectText;

    public Image equipImg1;
    public Image equipImg2;

    public Button SoundEffectBtn;
    public Button SoundMusicBtn;

	public Button[] GolryButtons; // 成就
    // Use this for initialization
    void Awake () {
		UserInfoLayer.SetActive (true);
		AchievementLayer.SetActive (false);
		SettingLayer.SetActive (false);
		InitUserData ();
    }
		
	void OnEnable()
	{
		InitUserData ();
		canvasManager.isUILayer++;

//		Advertising.ShowBannerAd (BannerAdPosition.Bottom);
	}

	void OnDisable()
	{
		canvasManager.isUILayer--;
//		Advertising.HideBannerAd ();
	}

    //个人信息界面
    public void InitUserData(){
		playerLvText.text = DataManager.lv.ToString();

        playerNameText.text = ManagerUserInfo.GetStringValueByKey("Name");
        coinText.text = DataManager.incomeAll.ToString();

		BigNumber incomeText = new BigNumber (DataManager.incomeSec.number * DataManager.buffIncome * DataManager.buffSpeed, DataManager.incomeSec.units);
		incomeSecText.text = incomeText.ToString();

        //排行榜数值实时获取
		rankingText.text = DataManager.rank > 0 ? DataManager.rank.ToString() : "--";
		ChangeTabBtnState (0);
        if (DataManager.equipmentId1 >= 0)
        {
            Sprite[] equip1Sps = new Sprite[10];
            equip1Sps = Resources.LoadAll<Sprite>("Sprites/Runaway");
            equipImg1.sprite = equip1Sps[DataManager.equipmentId1];
        }
        if (DataManager.equipmentId2 >= 0)
        {
            Sprite[] equip2Sps = new Sprite[10];
            equip2Sps = Resources.LoadAll<Sprite>("Sprites/Discount");
            equipImg2.sprite = equip2Sps[DataManager.equipmentId2];
        }

    }

    //荣耀成就界面
    public void InitGloryData()
    {
		ChangeTabBtnState (1);
//		int rank = DataManager.rank;
		int minId = 1872;
		int maxID = 1881;
		int mCollectCount = 0;
        foreach(ModelTask mTask in DataManager.taskInfoList){
			int taskId = int.Parse (mTask.TaskId);
			if (taskId >= minId && taskId <= maxID) {	
				if (mTask.IsGet == 1) {
					GolryButtons [taskId - minId].transform.GetChild (1).gameObject.SetActive (true);
					mCollectCount++;
				}
			}
        }

		string formatText = ConfigFileMgr.GetContentByKey (EM_LocalizeConstants.Locaize_CollectGlory,CollectText);
		CollectText.text = string.Format(formatText, mCollectCount);
    }

    //设置界面
    public void SetSound(){
		ChangeTabBtnState (2);
		InitMusicUIState ();
    }

    //音效按钮回调
    public void SetSoundEffect(){
        if (ManagerUserInfo.GetIntDataByKey("SoundEffect") == 1)
        {
            ManagerUserInfo.SetDataByKey("SoundEffect", 0);
			SoundEffectBtn.transform.GetChild (0).gameObject.SetActive (false);
			SoundEffectBtn.transform.GetChild (1).gameObject.SetActive (true);
        }
        else
        {
            ManagerUserInfo.SetDataByKey("SoundEffect", 1);
			SoundEffectBtn.transform.GetChild (0).gameObject.SetActive (true);
			SoundEffectBtn.transform.GetChild (1).gameObject.SetActive (false);
        }
    }

    //音乐按钮回调
    public void SetSoundMusic()
    {
        if (ManagerUserInfo.GetIntDataByKey("SoundMusic") == 1)
        {
            ManagerUserInfo.SetDataByKey("SoundMusic", 0);
			SoundMusicBtn.transform.GetChild (0).gameObject.SetActive (false);
			SoundMusicBtn.transform.GetChild (1).gameObject.SetActive (true);
			SoundManager.StopBGM();
        }
        else
        {
            ManagerUserInfo.SetDataByKey("SoundMusic", 1);
			SoundMusicBtn.transform.GetChild (0).gameObject.SetActive (true);
			SoundMusicBtn.transform.GetChild (1).gameObject.SetActive (false);
			SoundManager.SoundBGM ();
        }
    }
		
	public void ChangeTabBtnState(int index) {
		for (int i = 0; i < TabBtns.Length; i++) {
			if (i == index) {
				TabBtns [i].transform.GetChild (0).gameObject.SetActive (false);
				TabBtns [i].transform.GetChild (1).gameObject.SetActive (true);
			} else {
				TabBtns [i].transform.GetChild (0).gameObject.SetActive (true);
				TabBtns [i].transform.GetChild (1).gameObject.SetActive (false);
			}
		}
	}

	public void InitMusicUIState() {
		if (ManagerUserInfo.GetIntDataByKey("SoundEffect") == 1)
		{
			SoundEffectBtn.transform.GetChild (0).gameObject.SetActive (true);
			SoundEffectBtn.transform.GetChild (1).gameObject.SetActive (false);
		}
		else
		{
			SoundEffectBtn.transform.GetChild (0).gameObject.SetActive (false);
			SoundEffectBtn.transform.GetChild (1).gameObject.SetActive (true);
		}

		if (ManagerUserInfo.GetIntDataByKey("SoundMusic") == 1)
		{
			SoundMusicBtn.transform.GetChild (0).gameObject.SetActive (true);
			SoundMusicBtn.transform.GetChild (1).gameObject.SetActive (false);
			SoundManager.SoundBGM ();
		}
		else
		{
			SoundMusicBtn.transform.GetChild (0).gameObject.SetActive (false);
			SoundMusicBtn.transform.GetChild (1).gameObject.SetActive (true);
			SoundManager.StopBGM();
		}

	}
	public void BtnFeedbackClicked() {
		
	}
}
