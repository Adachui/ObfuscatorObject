using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : AppAdvisoryHelper {

	//android
	const float devHeight = 854f;      //设计的尺寸高度
	const float devWidth = 480f;       //设计的尺寸宽度

	//ios
	const float devHeight_ios = 16f;      //设计的尺寸高度
	const float devWidth_ios = 9f;       //设计的尺寸宽度
	// Use this for initialization
	void Start () {
		float devRatio = devWidth / devHeight;

		float screenHeight = Screen.height;   //获取屏幕高度
		float screenWidth = Screen.width;     //获取屏幕宽度
		Debug.Log("屏幕宽度 ： " + screenWidth + "屏幕高度 ： " + screenHeight);

		float orthographicSize = this.GetComponent<Camera> ().orthographicSize;
		Debug.Log ("orthographicSize 11 = " + orthographicSize);
		//宽高比
		float screenRatio = screenWidth / screenHeight;
		#if UNITY_ANDROID
			Debug.Log("宽高比11 ： " + devRatio + "宽高比22 ： " + screenRatio);
			if(screenRatio < devRatio){
				orthographicSize = 31f;
				Debug.Log ("orthographicSize 22 = " + orthographicSize);
				this.GetComponent<Camera> ().orthographicSize = orthographicSize;

				Vector2 pos = new Vector2(110f,250f);
				canvasManager.moreBtn.GetComponent<RectTransform> ().anchoredPosition = pos;
				pos = new Vector2(235f,-37f);
				canvasManager.recyleText.GetComponent<RectTransform> ().anchoredPosition = pos;

				canvasManager.earning.localPosition = new Vector3 (canvasManager.earning.localPosition.x,canvasManager.earning.localPosition.y + 100f,canvasManager.earning.localPosition.z);
			}
		#else
			Debug.Log("宽高比11 ： " + devRatio + "宽高比22 ： " + screenRatio);
			devRatio = devWidth_ios / devHeight_ios;
			if(screenRatio > devRatio){
				orthographicSize = 23f;
				Debug.Log ("orthographicSize 22 = " + orthographicSize);
				this.GetComponent<Camera> ().orthographicSize = orthographicSize;

				Vector2 pos = new Vector2(140f,75f);
				canvasManager.moreBtn.GetComponent<RectTransform> ().anchoredPosition = pos;
				pos = new Vector2(209f,-127f);
				canvasManager.recyleText.GetComponent<RectTransform> ().anchoredPosition = pos;

				canvasManager.earning.localPosition = new Vector3 (canvasManager.earning.localPosition.x,canvasManager.earning.localPosition.y - 75f,canvasManager.earning.localPosition.z);
			}
		#endif


	}

}
