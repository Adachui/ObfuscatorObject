using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
public struct TraceParam
{
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
	public string _appkey;
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
	public string _secretSalt;
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
	public string _starRatingMessage;
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
	public string _port;
	[MarshalAs(UnmanagedType.I1)] 
	public System.Boolean _debug;
	[MarshalAs(UnmanagedType.I1)] 
	public System.Boolean _viewTrack;
	[MarshalAs(UnmanagedType.I1)] 
	public System.Boolean _requiresConsent;
	[MarshalAs(UnmanagedType.I1)] 
	public System.Boolean _isTestDevice;
	[MarshalAs(UnmanagedType.I1)] 
	public System.Boolean _sendPush;
	[MarshalAs(UnmanagedType.I1)] 
	public System.Boolean _doNotShowAlertForNotifications;
	[MarshalAs(UnmanagedType.I1)] 
	public System.Boolean _applyZeroIDFAFix;
	[MarshalAs(UnmanagedType.I1)] 
	public System.Boolean _manualSessionHandling;
	[MarshalAs(UnmanagedType.I1)] 
	public System.Boolean _alwaysUsePOST;
	[MarshalAs(UnmanagedType.I1)] 
	public System.Boolean _enableAppleWatch;
	[MarshalAs(UnmanagedType.I1)] 
	public System.Boolean _enableAttribution;
	[MarshalAs(UnmanagedType.I1)] 
	public System.Boolean _starRatingDisableAskingForEachAppVersion;

	[MarshalAs(UnmanagedType.I4)] 
	public System.Int32 _updateSessionPeriod;
	[MarshalAs(UnmanagedType.I4)] 
	public System.Int32 _eventSendThreshold;
	[MarshalAs(UnmanagedType.I4)] 
	public System.Int32 _storeRequestsLimit;
	[MarshalAs(UnmanagedType.I4)] 
	public System.Int32 _crashLogLimit;
	[MarshalAs(UnmanagedType.I4)] 
	public System.Int32 _starRatingSessionCount;

}



public class ileadTraceIOS : ileadTrace
{

	private System.Text.StringBuilder _stringBuilder = new System.Text.StringBuilder();


	public override void Init(ref TraceParam _param)
	{
		Debug.Log ("ileadTraceIOS----init");
		if (Application.platform != RuntimePlatform.IPhonePlayer)
			return;
		IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(_param));
		Marshal.StructureToPtr(_param, ptr, false);
		initTraceSDKWithParam2(ptr);
	}

	public static void InitAndroid(ref TraceParam _param)
	{
		if (Application.platform != RuntimePlatform.Android)
			return;
	}

	public override void RecordEvent (string _key)
	{
		Debug.Log ("_key:"+_key);
		recordEvent (_key);
	}
	
	public override void RecordEvent (string _key, int _count)
	{
			recordEvent2 (_key, _count);
	}
	
	public override void RecordEvent (string _key, double _sum)
	{
			recordEvent3 (_key, _sum);
	}
	
	public override void RecordEventDuration (string _key, int _duration)
	{
			recordEvent4 (_key, _duration);
	}
	
	public override void RecordEvent (string _key, int _count, double _sum)
	{
			recordEvent5 (_key, _count, _sum);
	}
	
	public override void RecordEvent (string _key, Dictionary<string, string> _dic)
	{
			recordEvent6 (_key, ConvertMapToString(_dic));
	}
	
	public override void RecordEvent (string _key, Dictionary<string, string>  _dic, int _count)
	{
			recordEvent7 (_key, ConvertMapToString(_dic), _count);
	}
	
	public override void RecordEvent (string _key, Dictionary<string, string>  _dic, int _count, double _sum)
	{
			recordEvent8 (_key, ConvertMapToString(_dic), _count, _sum);
	}
	
	public override void RecordEvent (string _key, Dictionary<string, string>  _dic, int _count, double _sum, int _duration)
	{
			recordEvent9 (_key, ConvertMapToString(_dic), _count, _sum, _duration);
	}
	
	public override void StartEvent (string _key)
	{
			startEvent (_key);
	}
	
	public override void EndEvent (string _key)
	{
			endEvent (_key);
	}
	
	public override void EndEvent (string _key, Dictionary<string, string>  _dic, int _count, double _sum)
	{
			endEvent2 (_key, ConvertMapToString(_dic), _count, _sum);
	}
	
	public override void CancelEvent (string _key)
	{
			cancelEvent (_key);
	}

	string ConvertMapToString(Dictionary<string, string> segmentation) 
	{
		if (segmentation == null)
			return string.Empty;
		else 
		{
			_stringBuilder.Length = 0;
			if (segmentation != null)
			{
				int index = 0;
				foreach (var pair in segmentation)
				{
					_stringBuilder.Append(pair.Key);
					_stringBuilder.Append(",");
					_stringBuilder.Append(pair.Value);
					if(index < segmentation.Count-1)
						_stringBuilder.Append(",");
					index++;
				}
			}
			return _stringBuilder.ToString();
		}
	}

	[DllImport("__Internal")]
	private static extern void initTraceSDKWithParam2 (IntPtr ptr);
	[DllImport("__Internal")]
	private static extern void recordEvent (string _key);
	[DllImport("__Internal")]
	private static extern void recordEvent2 (string _key, int _count);
	[DllImport("__Internal")]
	private static extern void recordEvent3 (string _key, double _sum);
	[DllImport("__Internal")]
	private static extern void recordEvent4 (string _key, int _duration);
	[DllImport("__Internal")]
	private static extern void recordEvent5 (string _key, int _count, double _sum);
	[DllImport("__Internal")]
	private static extern void recordEvent6(string _key, string _dicStr);
	[DllImport("__Internal")]
	private static extern void recordEvent7 (string _key, string _dicStr, int _count);
	[DllImport("__Internal")]
	private static extern void recordEvent8 (string _key, string _dicStr, int _count, double _sum);
	[DllImport("__Internal")]
	private static extern void recordEvent9 (string _key, string _dicStr, int _count, double _sum, int _duration);
	[DllImport("__Internal")]
	private static extern void startEvent (string _key);
	[DllImport("__Internal")]
	private static extern void endEvent (string _key);
	[DllImport("__Internal")]
	private static extern void endEvent2 (string _key, string _dicStr, int _count, double _sum);
	[DllImport("__Internal")]
	private static extern void cancelEvent (string _key);
}
