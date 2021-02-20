using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CileadTrace))]
public class ileadBridgeEditor : Editor 
{
//	private CileadTrace mTrace;
//	public override void OnInspectorGUI()
//	{
//		mTrace = (CileadTrace)target;  
//		// 绘制全部原有属性
//		base.DrawDefaultInspector();
//		mTrace.m_eventSendThreshold = EditorGUILayout.IntSlider("Thread hold", mTrace.m_eventSendThreshold, 1, 10);
////
////		//当Inspector 面板发生变化时保存数据
////		if (GUI.changed)
////		{
////			EditorUtility.SetDirty(target);
////		}
//	}
}
