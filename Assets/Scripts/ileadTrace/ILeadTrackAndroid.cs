using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ILeadTrackAndroid : ileadTrace
{
    AndroidJavaObject _tracker;
    System.Text.StringBuilder _stringBuilder = new System.Text.StringBuilder();

    public override void Init(CileadTrace trace)
    {
//#if !UNITY_EDITOR && UNITY_ANDROID
        using (AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        using (AndroidJavaObject activity = unity.GetStatic<AndroidJavaObject>("currentActivity"))
        {
            try
            {
                _tracker = new AndroidJavaObject("com.ilead.track.ILeadTrack", activity);
                Debug.Log("_tracker" + (_tracker != null));
                EnableCrashReporting();
                SetLoggingEnabled(trace.m_debug);
                if (!string.IsNullOrEmpty(trace.tracePort))
                    SetServerUrl(trace.tracePort);
                EnableParameterTamperingProtection(trace.secertSalt_android);
                _tracker.Call("init", trace.appkey_android);
                SetHttpPostForced(trace.m_alwaysUsePOST);
                CheckLocation();
                OnStart = StartTrack;
                OnStop = StopTrack;
                OnDestroy = Destroy;
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.ToString());
            }
        }
//#endif
    }

    void SetServerUrl(string value) {
        if (_tracker == null) return;
        _tracker.Call("setServerURL", value);
    }

    void SetLoggingEnabled(bool value) {
        if (_tracker == null) return;
        _tracker.Call("setLoggingEnabled", value);
    }

    void EnableCrashReporting() {
        if (_tracker == null) return;
        _tracker.Call("enableCrashReporting");
    }

    void EnableParameterTamperingProtection(string salt) {
        if (_tracker == null) return;
        if (string.IsNullOrEmpty(salt)) return;
        _tracker.Call("enableParameterTamperingProtection", salt);
    }

    void SetHttpPostForced(bool isItForced) {
        if (_tracker == null) return;
        _tracker.Call("setHttpPostForced", isItForced);
    }

    void CheckLocation() {
        if (_tracker == null) return;
        _tracker.Call("checkLocation");
    }

    void StopTrack() {
        if (_tracker == null) return;
        _tracker.Call("stop");
    }

    void StartTrack() {
        if (_tracker == null) return;
        _tracker.Call("start");
    }

    private void Destroy()
    {
        _tracker.Dispose();
        _tracker = null;
    }

    public bool IsInited() {
        return _tracker != null && IsInitialized();
    }

    bool IsInitialized() {
        if (_tracker == null) return false;
        return _tracker.Call<bool>("isInitialized");
    }

    public override void RecordEvent(string _key)
    {
        RecordEvent(_key, 1);
    }

    public override void RecordEvent(string key, int count) {
        if (_tracker == null) return;
        _tracker.Call("recordEvent", key, count);
    }

    /// <summary>
    /// Records the event.
    /// </summary>
    /// <param name="key">Key.</param>
    /// <param name="segmentation">Segmentation. sample
    ///     segmentation.put("country", "Germany");
    ///     segmentation.put("app_version", "1.0");
    /// </param>
    /// <param name="count">Count.</param>
    public override void RecordEvent(string key, Dictionary<string, string> segmentation, int count) {
        if (_tracker == null) return;
        _tracker.Call("recordEvent", key, ConvertMapToString(segmentation), count);
    }

    public override void StartEvent(string key) {
        if (_tracker == null) return;
        _tracker.Call("startEvent", key);
    }

    public override void EndEvent(string key) {
        if (_tracker == null) return;
        _tracker.Call("endEvent", key);
    }

    public override void EndEvent(string key, Dictionary<string, string> segmentation, int count, double sum) {
        if (_tracker == null) return;
        _tracker.Call("endEvent", key, ConvertMapToString(segmentation), count, sum);
    }

    string ConvertMapToString(Dictionary<string, string> segmentation) {
        if (segmentation == null)
            return string.Empty;
        else {
            _stringBuilder.Length = 0;
            if (segmentation != null)
            {
                foreach (var pair in segmentation)
                {
                    _stringBuilder.Append(pair.Key);
                    _stringBuilder.Append("=");
                    _stringBuilder.Append(pair.Value);
                    _stringBuilder.Append(";");
                }
            }
            return _stringBuilder.ToString();
        }
    }
}
