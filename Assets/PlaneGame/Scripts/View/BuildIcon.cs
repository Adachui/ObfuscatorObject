using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildIcon : MonoBehaviour {
	public Vector3 scale = new Vector3(1f,1f,1f);               //预制体大小，一般不动
	public Quaternion rota = Quaternion.Euler(0.0f,90f,0.0f);   //预制体旋转角度，一般不动

	public Vector3 buildScale = new Vector3(1f,1f,1f);  //建筑模型大小
	public Vector2 imgSize = new Vector2(100f,100f);    //显示图片大小，调节icon大小可改变此项


	public Transform build = null;
	public RectTransform img = null;
	//public RectTransform img = null;
	// Use this for initialization
	void Start () {
		
		transform.localScale = scale;
		transform.localRotation = rota;

		if (build == null)
			build = transform.GetChild (1);
		build.localScale = buildScale;
		if (img == null)
			img = transform.GetChild (0).GetComponent<RectTransform> ();
		img.sizeDelta = imgSize;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StopAnimation(bool dismiss) {
		Debug.Log ("StopAnimation");
		transform.GetComponent<Animator> ().speed = 0;
//		if (dismiss) {
//			transform.localScale = Vector3.zero;
//		} else {
//			transform.localScale = new Vector3(1, 1, 1);
//		}
		transform.GetComponent<Animator>().SetBool("left", false);
		transform.GetComponent<Animator>().SetBool("right", false);
		transform.GetComponent<Animator>().SetBool("center", false);
	}
}
