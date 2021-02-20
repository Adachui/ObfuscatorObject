using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : AppAdvisoryHelper {
	private int rewardType = 0;
	// Use this for initialization
	public GameObject rewardPage;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RandomRewardType () {
		rewardType = Random.Range (1, 100) % 2 == 0 ? 1 : 0;
	}

	public void StopMoving() {
		planeGameManager.StopBallon ();
	}

	public void PlayingMusic() {
		soundManager.SoundBallonShow ();
	}

	public void ShowRewardPage() {
		rewardPage.SetActive (true);
		rewardPage.GetComponent<CommonAdWindowView> ().SetData (rewardType);
	}

	private IEnumerator OnMouseDown()
	{
		if (canvasManager.isUILayer <= 0) {
			Debug.Log ("OnMouseDown");
			RandomRewardType ();
			yield return new WaitForSeconds(0.1f);
		}

	}

	private IEnumerator OnMouseUp() {
		if (canvasManager.isUILayer <= 0) {
			Debug.Log ("OnMouseUp");
			ShowRewardPage ();
			StopMoving ();
			transform.localPosition = new Vector3 (-100, 0, 0);
			yield return new WaitForSeconds(0.1f);
		}

	}
}
