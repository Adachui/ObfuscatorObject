using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CoinUpVIew : AppAdvisoryHelper {


	// Use this for initialization
	public void StartAnimation () {
		if (transform.localPosition.x < -10f) {
			transform.localPosition = new Vector3 (-10f,transform.localPosition .y,transform.localPosition.z);
		}else if(transform.localPosition.x > 13f){
			transform.localPosition = new Vector3 (13f,transform.localPosition .y,transform.localPosition.z);
		}
		float dis = 5f;
		if (transform.localPosition.x < 0) {
			dis = -5f;
		}
		transform.DOLocalMoveZ(transform.localPosition.z + 5f,1f).SetEase(Ease.OutCubic).OnComplete(delegate
			{
				DestorySelf();
			});
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void DestorySelf() {
		//Destroy (gameObject);
		gameObject.SetActive(false);
	}


}
