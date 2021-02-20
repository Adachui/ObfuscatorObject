using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : AppAdvisoryHelper {
	public static AudioSource bgm;
	public static AudioSource bgmSpeed;

	public AudioSource audiobgm;
	public AudioSource audiobgmSpeed;

	public AudioClip effectPayCash;
	public AudioClip effectOnRoad;
	public AudioClip effectOnRoadReward;
	public AudioClip effectMerge;
	public AudioClip effectUnlock;
	public AudioClip effectLevelUp;
	public AudioClip effectLocationSwitch;
	public AudioClip effectUIClick;
	public AudioClip effectUIQuit;
	public AudioClip effectRewardClaim;
	public AudioClip effectCurrencyExchange;
	public AudioClip effectBallonShow;
	public AudioClip effecBoxOpen;
	public AudioClip tower_attack_1_3;
	public AudioClip tower_attack_4_9;
	public AudioClip tower_attack_10_18;
	public AudioClip tower_attack_19_30;
	public AudioClip tower_attack_31_37;
	public AudioClip tower_attack_38_40;

	public Dictionary<string, AudioClip> tower_attack_dic = new Dictionary<string, AudioClip>();

	void Awake() {
		SoundManager.bgm = audiobgm;
		SoundManager.bgmSpeed = audiobgmSpeed;
		SoundBGM ();
	}

	void Start () {
		tower_attack_dic["tower_attack_1_3"] = tower_attack_1_3;
		tower_attack_dic["tower_attack_4_9"] = tower_attack_4_9;
		tower_attack_dic["tower_attack_10_18"] = tower_attack_10_18;
		tower_attack_dic["tower_attack_19_30"] = tower_attack_19_30;
		tower_attack_dic["tower_attack_31_37"] = tower_attack_31_37;
		tower_attack_dic["tower_attack_38_40"] = tower_attack_38_40;
	}

	// 消费 cashRegister
	public void SoundPayCash() {
		if (!PlaneGameManager.isAd && ManagerUserInfo.GetIntDataByKey ("SoundEffect") == 1) {
			AudioSource.PlayClipAtPoint(effectPayCash, transform.position);
		}
	}

	// 拖拽到场地 road_On
	public void SoundOnRoad() {
		if (!PlaneGameManager.isAd && ManagerUserInfo.GetIntDataByKey ("SoundEffect") == 1) {
			AudioSource.PlayClipAtPoint(effectOnRoad, transform.position);
		}
	}

	// 穿过终点 奖励 road_Finish
	public void SoundOnRoadReward() {
		if (!PlaneGameManager.isAd && ManagerUserInfo.GetIntDataByKey ("SoundEffect") == 1) {
			AudioSource.PlayClipAtPoint(effectOnRoadReward, transform.position);
		}
	}

	// 合并 Synthesis unlock
	public void SoundMerge(bool unlock) {
		if (ManagerUserInfo.GetIntDataByKey ("SoundEffect") == 1) {
			if (unlock) {
				AudioSource.PlayClipAtPoint (effectUnlock, transform.position);
			} else {
				AudioSource.PlayClipAtPoint(effectMerge, transform.position);
			}
		}
	}

	// 角色等级提升 levelUp
	public void SoundLevelUp() {
		if ( ManagerUserInfo.GetIntDataByKey ("SoundEffect") == 1) {
			AudioSource.PlayClipAtPoint(effectLevelUp, transform.position);
		}
	}

	// 交换地块 switchLocation
	public void SoundLocationSwitch() {
		if (ManagerUserInfo.GetIntDataByKey ("SoundEffect") == 1) {
			AudioSource.PlayClipAtPoint(effectLocationSwitch, transform.position);
		}
	}

	// UI点击 UI_Click
	public void SoundUIClick() {
		if (ManagerUserInfo.GetIntDataByKey ("SoundEffect") == 1) {
			AudioSource.PlayClipAtPoint(effectUIClick, transform.position);
		}
	}

	// 界面退出 UI_Quit
	public void SoundUIQuit() {
		if (ManagerUserInfo.GetIntDataByKey ("SoundEffect") == 1) {
			AudioSource.PlayClipAtPoint(effectUIQuit, transform.position);
		}
	}

	// 纸箱打开 box_Click
	public void SoundBoxOpen() {
		if (!PlaneGameManager.isAd &&  ManagerUserInfo.GetIntDataByKey ("SoundEffect") == 1) {
			AudioSource.PlayClipAtPoint(effecBoxOpen, transform.position);
		}
	}

	// 奖励领取 cliam
	public void SoundRewardClaim() {
		if (ManagerUserInfo.GetIntDataByKey ("SoundEffect") == 1) {
			AudioSource.PlayClipAtPoint(effectRewardClaim, transform.position);
		}
	}

	// 货币兑换
	public void SoundCurrencyExchange() {
		if (ManagerUserInfo.GetIntDataByKey ("SoundEffect") == 1) {
			AudioSource.PlayClipAtPoint(effectCurrencyExchange, transform.position);
		}
	}

	// BGM BGM BGM_speed
	public static void SoundBGM() {
		
		if (ManagerUserInfo.GetIntDataByKey ("SoundMusic") == 1)
		{
			if (PlaneGameManager.isSpeedUp) {
				if (!bgmSpeed.isPlaying) {
					bgmSpeed.Play ();
				}
				if (bgm.isPlaying) {
					bgm.Stop ();
				}
			} else {
				if (bgmSpeed.isPlaying) {
					bgmSpeed.Stop ();
				}
				if (!bgm.isPlaying) {
					bgm.Play ();
				}
			}
		} else {
			if (bgmSpeed.isPlaying) {
				bgmSpeed.Stop ();
			}
			if (bgm.isPlaying) {
				bgm.Stop ();
			}
		}
	}

	public static void StopBGM()
	{
		if (bgm != null && bgmSpeed != null) {
			bgm.Stop ();
			bgmSpeed.Stop ();
		}
	}

	public static void SoundAdsSwitch(bool on)
	{
		if (on) {
			SoundManager.SoundBGM ();
		} else {
			SoundManager.StopBGM ();
		}
	}

	// 热气球 BALLON
	public void SoundBallonShow() {
		if (ManagerUserInfo.GetIntDataByKey ("SoundEffect") == 1) {
			AudioSource.PlayClipAtPoint(effectBallonShow, transform.position);
		}
	}

	//攻击
	public void SoundAttack(string name){
		if (ManagerUserInfo.GetIntDataByKey ("SoundEffect") == 1) {
			AudioSource.PlayClipAtPoint(tower_attack_dic[name], transform.position);
		}
	}
}
