using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : AppAdvisoryHelper {
	public ModelBlock info = new ModelBlock ();
	public int BlockId = 0;
	public bool isLockBlock = false;
	public GameObject MoreLayer;
	public GameObject lightEffect;
	public GameObject createEffect;
	public GameObject glowEffect;

	private IEnumerator OnMouseDown()
	{
		if (canvasManager.isUILayer <= 0) {

			yield return new WaitForSeconds(0f);
		}

	}

	private IEnumerator OnMouseUp() {
		if (canvasManager.isUILayer <= 0) {
			if (isLockBlock) {
				MoreLayer.SetActive (true);
				MoreLayer.GetComponent<MoreLayerView> ().SetMenuActive (2);	
			}
			yield return new WaitForSeconds (0f);
		}
	}

	//合成高亮
	public void Light() {
		lightEffect.SetActive (true);
	}
	//高亮消失
	public void Dim() {
		lightEffect.SetActive (false);
	}

	//建筑建成光效
	public void ShowCreateEffect() {
		StartCoroutine (ShowMergeEffect (createEffect, 2f));
		StartCoroutine (ShowMergeEffect (glowEffect, 1.5f, 1f));
	}

	IEnumerator ShowMergeEffect(GameObject effec, float sec, float delay = 0f) {
		yield return new WaitForSeconds (delay);
		effec.SetActive (true);
		yield return new WaitForSeconds (sec);
		effec.SetActive (false);
	}

}
