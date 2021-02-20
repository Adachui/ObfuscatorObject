using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyMobile;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

#if UNITY_ANDROID
using GooglePlayGames.BasicApi;
using GooglePlayGames;
#endif

//using GoogleMobileAdsMediationTestSuite.Api;
//using GoogleMobileAds.Api.Mediation.AppLovin;

public class RankView : AppAdvisoryHelper {
	public GameObject content;
	public GameObject userItem;
	public Text rankText;
	public List<GameObject> rankItemList = new List<GameObject> ();
	// Use this for initialization
	private IScore[] scores;

//	private List<IScore> scs = new List<IScore> ();
//	private List<IUserProfile> us = new List<IUserProfile> ();

	public bool isLoadRank = false;

	//	void Start () {
//
//	}

	void OnEnable() {
//				GameServices.ShowLeaderboardUI ();
		canvasManager.isOtherLayerOpen = true;
		canvasManager.isUILayer++;
		canvasManager.Loading.SetActive (true);
		//		userItem.SetActive (false);

		isLoadRank = false;
		string username = "--";
		if (GameServices.LocalUser != null)
			username = GameServices.LocalUser.userName;
		
		userItem.GetComponent<RankItemView> ().InitView (DataManager.rank, username, DataManager.incomeAll);
		rankText.text = DataManager.rank > 0 ? DataManager.rank.ToString() : "--";
		// 获取自身成绩和排名

		if (GameServices.IsInitialized () && GameServices.LocalUser != null) {
			GameServices.LoadLocalUserScore (
				EM_GameServicesConstants.Leaderboard_leaderboard_fort_clash,
				OnLocalUserLoaded
			);
			if (!isLoadRank) {
				userItem.GetComponent<RankItemView> ().SetRankUnkonw (DataManager.rank > 0 ? DataManager.rank.ToString() : "--");
				rankText.text = DataManager.rank > 0 ? DataManager.rank.ToString() : "--";
			}
			userItem.GetComponent<RankItemView> ().SetIcon (GameServices.LocalUser.image);
		} else {
			userItem.GetComponent<RankItemView> ().InitView (999, "You", DataManager.coins);
			userItem.GetComponent<RankItemView> ().SetRankUnkonw (DataManager.rank > 0 ? DataManager.rank.ToString() : "--");
			rankText.text = DataManager.rank > 0 ? DataManager.rank.ToString() : "--";
		}

		#if UNITY_ANDROID
		PlayGamesPlatform.Instance.LoadScores(
			EM_GPGSIds.leaderboard_fort_clash,
			LeaderboardStart.TopScores,
			25,
			LeaderboardCollection.Public,
			LeaderboardTimeSpan.AllTime,
			(data)=> {
				this.scores = data.Scores;
				//Debug.Log ("A: " + scores.Length + " B:" + scores [0].value);
				string[] userIDs = new string[scores.Length]; 
				int i = 0;
				if (scores != null && scores.Length > 0) {
					for (;i < scores.Length; i++) {
						if (i == 25) {
							break;
						} else {
							userIDs [i] = scores [i].userID;
						}
					}
				}
				GameServices.LoadUsers (
					userIDs, OnUserLoaded
				);
			}
		);
		#else
		GameServices.LoadScores (
			EM_GameServicesConstants.Leaderboard_leaderboard_fort_clash,
			1,
			25,
			TimeScope.AllTime,
			UserScope.Global,
			OnScoresLoaded
		);
		#endif

//		#if !UNITY_ANDROID
//		MediationTestSuite.Show ("ca-app-pub-3549401711589014~2482198150");
//		AppLovin.Initialize();
//		#else
//		MediationTestSuite.Show ("ca-app-pub-3549401711589014~6122060268");
//		AppLovin.Initialize();
//		#endif
	}

	void OnLocalUserLoaded(string leaderboardName, IScore score) {;
		BigNumber sc = DataManager.incomeAll;
		string username = "--";
		if (GameServices.LocalUser != null)
			username = GameServices.LocalUser.userName;
		
		userItem.GetComponent<RankItemView> ().InitView (score.rank, username, sc);
		isLoadRank = true;
		DataManager.rank = score.rank;
		ManagerUserInfo.SetDataByKey("UserRank", DataManager.rank);
		canvasManager.rankingText.text = DataManager.rank > 0 ? DataManager.rank.ToString() : "--";
		rankText.text = DataManager.rank > 0 ? DataManager.rank.ToString() : "--";
	}

	void OnScoresLoaded(string leaderboardName, IScore[] scores)
	{
		Debug.Log ("OnScoresLoaded " + leaderboardName + scores.Length);

		this.scores = scores;
		string[] userIDs = new string[scores.Length]; 
		int i = 0;
		if (scores != null && scores.Length > 0) {
			for (;i < scores.Length; i++) {
				if (i == 25) {
					break;
				} else {
					userIDs [i] = scores [i].userID;
				}
			}
		}
		GameServices.LoadUsers (
			userIDs, OnUserLoaded
		);
	}
		
	void OnUserLoaded(IUserProfile[] users) {

		Debug.LogError ("OnUserLoaded " + users.Length);

		if (rankItemList.Count > 0) {
			for (int i = 0; i < rankItemList.Count; i++) {
				Destroy (rankItemList [i]);
			}
		}

		rankItemList.Clear ();

		int count = users.Length;

		if (scores.Length < users.Length) {
			count = scores.Length;
		}

		for (int i = 0; i < count; i++) {
			if (users [i] != null) {
				GameObject rankItem = Resources.Load ("Prefabs/UI/RankItem") as GameObject;
				rankItem = Instantiate (rankItem);
				rankItem.transform.SetParent (content.transform);

				BigNumber sc = new BigNumber (scores[i].value);
				rankItem.transform.localScale = new Vector3 (1, 1, 1);
				rankItem.transform.localPosition = Vector3.zero;
				rankItem.GetComponent<RankItemView> ().InitView (scores[i].rank, scores[i].userID, sc);
				rankItem.GetComponent<RankItemView> ().SetName (users [i].userName);
				rankItem.GetComponent<RankItemView> ().SetIcon (users [i].image);
				rankItemList.Add (rankItem);
			}
		}

		if (canvasManager.Loading.activeSelf) {
			canvasManager.Loading.SetActive (false);
		}
	}

	void OnDisable() {
		canvasManager.isOtherLayerOpen = false;
		canvasManager.isUILayer--;
		if (canvasManager.Loading.activeSelf) {
			canvasManager.Loading.SetActive (false);
		}
	}


	void OnLocalUserLoaded(string leaderboardName, IScore[] scores) {
		foreach(IScore score in scores) {
			if(GameServices.LocalUser.id.Equals(score.userID)) {
				BigNumber sc = DataManager.incomeAll;
				string username = "--";
				if (GameServices.LocalUser != null)
					username = GameServices.LocalUser.userName;
				
				userItem.GetComponent<RankItemView> ().InitView (score.rank, username, sc);
				isLoadRank = true;
				DataManager.rank = score.rank;
				ManagerUserInfo.SetDataByKey("UserRank", DataManager.rank);
				canvasManager.rankingText.text = DataManager.rank > 0 ? DataManager.rank.ToString() : "--";
				rankText.text = DataManager.rank > 0 ? DataManager.rank.ToString() : "--";
				break;
			}
		}
	}

}
