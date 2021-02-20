using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SDKTestUI : MonoBehaviour {

	public Text mText;
	// Use this for initialization
	void Start () {
		
	}
	
	public void SetText(string _media)
	{
		mText.text = _media;
	}

	void OnGUI()
	{
		if(GUI.Button(new Rect(100, 100, 400, 200),"ileadTraceBridge Init"))
		{
//			ileadTraceBridge.Init();
			CileadTrace.RecordEvent("testOnGUI");
		}
	}
}
