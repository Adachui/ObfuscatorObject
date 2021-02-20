using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScale : MonoBehaviour {
	public Transform ButtonGroup;
	public Transform PathText;
	void Start ()   
	{  
		float standard_width = 640f;        //初始宽度  
		float standard_height = 960f;       //初始高度  
		float device_width = 0f;                //当前设备宽度  
		float device_height = 0f;               //当前设备高度  
		float adjustor = 0f;         //屏幕矫正比例  
		//获取设备宽高  
		device_width = transform.GetComponent<RectTransform>().sizeDelta.x;  
		device_height = transform.GetComponent<RectTransform>().sizeDelta.y;  
		//计算宽高比例  
		float standard_aspect = standard_width / standard_height;  
		float device_aspect = device_width / device_height; 
//		Debug.Log ("y: " +  Screen.width / Screen.height);
//		Debug.Log ("standard_aspect: " + device_width + " device_aspect: " + device_aspect);
		//计算矫正比例  
		if (device_aspect < standard_aspect)  
		{  
			adjustor = standard_aspect / device_aspect;  
		}  

		CanvasScaler canvasScalerTemp = transform.GetComponent<CanvasScaler>(); 
		//canvasScalerTemp.matchWidthOrHeight = 0.5f;
	} 
}

