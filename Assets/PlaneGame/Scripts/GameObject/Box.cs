using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : AppAdvisoryHelper {
	public ModelBlock info;

	void Awake () {
		StartCoroutine (OpenBox ());
	}

	private IEnumerator OnMouseDown()
	{
		if (canvasManager.isUILayer <= 0) {
			yield return new WaitForEndOfFrame();
		}
	}

	private IEnumerator OnMouseUp() {
		if (canvasManager.isUILayer <= 0) {
			PlayOpenBox ();
			yield return new WaitForSeconds(1f);
			CreateBuilding ();
		} 
		yield return new WaitForEndOfFrame ();
	}

	private IEnumerator OpenBox() {
		yield return new WaitForSeconds (10f);
		PlayOpenBox ();
		yield return new WaitForSeconds(1f);
		CreateBuilding ();
	}

	private void PlayOpenBox()
	{
		soundManager.SoundBoxOpen ();
		transform.GetComponent<Animator>().SetBool("isOpen", true);
	}

	private void CreateBuilding()
	{
		blocksManager.CreateBuilding (transform.parent.gameObject, info.BlockId, info.BuildLv);
		Destroy (gameObject);
	}
}
