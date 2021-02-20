using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BannerWindowView : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void HideBanner() {
		gameObject.SetActive(false);
		transform.GetChild (0).GetChild (1).gameObject.SetActive (false);
		transform.GetChild (0).GetChild (2).gameObject.SetActive (false);
		transform.GetChild (0).GetChild (3).gameObject.SetActive (false);
	}
}
