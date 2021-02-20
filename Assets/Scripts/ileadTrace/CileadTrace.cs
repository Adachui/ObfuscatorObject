using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CileadTrace : MonoBehaviour 
{
	[Header("跟踪后台 iOS 配置的app key")]
	public string appkey_ios = "";
	[Header("跟踪后台 Android 配置的app key")]
	public string appkey_android = "";
	[Header("发送的默认域名端口")]
	public string tracePort = "http://countly.ileadsoft.com";
	[Header("跟踪后台 IOS 配置的加密串")]
	public string secertSalt_ios = "ileadsoft@2018game";//	
	[Header("跟踪后台 Android 配置的加密串")]
	public string secertSalt_android = "ileadsoft@2018game";//	
	[Header("客户端评星弹框显示的语句")]
	public string rateMessage = "Would you rate the app?";
	[Header("客户端评星次数间隔几次")]
	public int m_starRatingSessionCount = 5;
	[Header("客户端评星是否对每个不同的版本进行评星")]
	public bool m_starRatingDisableAskingForEachAppVersion = true;
	[Header("是否为调试状态")]
	public bool m_debug = false;
	[Header("是否跟踪页面")]
	public bool m_viewTrack = false;
	[Header("权限许可请求，不知道干嘛的，不开")]
	public bool m_requiresConsent = false;
	[Header("是否为测试设备")]
	public bool m_isTestDevice = false;
	[Header("是否接受发送push")]
	public bool m_sendPush = false;
	[Header("不显示通知弹框警告")]
	public bool m_doNotShowAlertForNotifications = false;
	[Header("开启IDFA为0的校验")]
	public bool m_applyZeroIDFAFix = true;
	[Header("开启后将定期发送session，现在媒体源MediaSource获取后会通过session发送，要开启这个开关")]
	public bool m_manualSessionHandling = true;
	[Header("使用POST模式发送信息")]
	public bool m_alwaysUsePOST = true;
	[Header("AppleWatch也可追踪")]
	public bool m_enableAppleWatch = false;
	[Header("开启后会在begin session的时候发送IDFA，除非用户限制了IDFA的获取")]
	public bool m_enableAttribution = false;
	[Header("多久发送一次session，默认60秒")]
	public int m_updateSessionPeriod = 60;
	[Header("开启几个线程发送追踪信息，默认5个")]
	public int m_eventSendThreshold = 5;
	[Header("请求限制，500")]
	public int m_storeRequestsLimit = 500;
	[Header("奔溃log限制，5条，默认")]
	public int m_crashLogLimit = 5;

    public static CileadTrace Instance { get; private set; }

    static ileadTrace mTrace = null;

	static ileadTrace GetIleadTrace()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer) 
		{
			if(mTrace == null)
				mTrace = new ileadTraceIOS ();
		}
		else if (Application.platform == RuntimePlatform.Android) 
		{
			if(mTrace == null)
				mTrace = new ILeadTrackAndroid ();
		}
		else 
		{
			if(mTrace == null)
				mTrace = new ILeadDummyTrack ();
		}
		return mTrace;
	}

	// Use this for initialization
	private void Awake ()
	{
        DontDestroyOnLoad(this);
        Instance = this;
		if (Application.platform == RuntimePlatform.IPhonePlayer) 
		{
            TraceParam tparam = new TraceParam
            {
                _appkey = appkey_ios,
                _secretSalt = secertSalt_ios,
                _starRatingMessage = rateMessage,
				_port = tracePort,
                _debug = m_debug,
                _viewTrack = m_viewTrack,
                _requiresConsent = m_requiresConsent,
                _isTestDevice = m_isTestDevice,
                _sendPush = m_sendPush,
                _doNotShowAlertForNotifications = m_doNotShowAlertForNotifications,
                _applyZeroIDFAFix = m_applyZeroIDFAFix,

                _manualSessionHandling = m_manualSessionHandling,
                _alwaysUsePOST = m_alwaysUsePOST,
                _enableAppleWatch = m_enableAppleWatch,
                _enableAttribution = m_enableAttribution,
                _starRatingDisableAskingForEachAppVersion = m_starRatingDisableAskingForEachAppVersion,

                _updateSessionPeriod = m_updateSessionPeriod,
                _eventSendThreshold = m_eventSendThreshold,
                _storeRequestsLimit = m_storeRequestsLimit,


                _crashLogLimit = m_crashLogLimit,
                _starRatingSessionCount = m_starRatingSessionCount
            };
            GetIleadTrace().Init(ref tparam);
		} 
		else if (Application.platform == RuntimePlatform.Android) 
		{
            GetIleadTrace().Init(this);
        } else
            GetIleadTrace().Init(this);
    }

    private void OnDestroy()
    {
        if (mTrace.OnDestroy != null)
            mTrace.OnDestroy.Invoke();
        Instance = null;
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) {
            if (mTrace.OnStop != null)
                mTrace.OnStop.Invoke();
        }
        else {
            if (mTrace.OnStart != null)
                mTrace.OnStart.Invoke();
        }
        //Debug.Log("pause" + pause);
    }

    /// <summary>
    /// Records the event.
    /// </summary>
    /// <param name="_key">Key.</param>
    /// <param name="isUnique">如果为true，该事件只发一次，后续发送请求会被忽略。</param>
    public static void RecordEvent(string _key, bool isUnique = false)
    {
        if (Instance == null)
        {
            Debug.LogError("not inited!");
            return;
        }
        if (isUnique)
        {
            if (PlayerPrefs.GetInt("ilead" + _key, 0) == 0)
                PlayerPrefs.SetInt("ilead" + _key, 1);
            else
                return;
        }
        mTrace.RecordEvent(_key);
    }

    public static void RecordEvent(string _key, int _count, bool isUnique = false) {
        if (Instance == null) {
            Debug.LogError("not inited!");
            return;
        }
        if (isUnique)
        {
            if (PlayerPrefs.GetInt("ilead" + _key, 0) == 0)
                PlayerPrefs.SetInt("ilead" + _key, 1);
            else
                return;
        }
        mTrace.RecordEvent(_key, _count);
    }

    public static void RecordEvent(string _key, Dictionary<string, string> _dic, int _count, bool isUnique = false) {
        if (Instance == null)
        {
            Debug.LogError("not inited!");
            return;
        }
        if (isUnique)
        {
            if (PlayerPrefs.GetInt("ilead" + _key, 0) == 0)
                PlayerPrefs.SetInt("ilead" + _key, 1);
            else
                return;
        }
        mTrace.RecordEvent(_key, _dic, _count);
    }

    public static void StartEvent(string _key) {
        if (Instance == null)
        {
            Debug.LogError("not inited!");
            return;
        }
        mTrace.StartEvent(_key);
    }

    public static void EndEvent(string _key) {
        if (Instance == null)
        {
            Debug.LogError("not inited!");
            return;
        }
        mTrace.EndEvent(_key);
    }

    public static void EndEvent(string _key, Dictionary<string, string> _dic, int _count, double _sum) {
        if (Instance == null)
        {
            Debug.LogError("not inited!");
            return;
        }
        mTrace.EndEvent(_key, _dic, _count, _sum);
    }
		
}
