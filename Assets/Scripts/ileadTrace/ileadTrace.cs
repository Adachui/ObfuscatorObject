using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ileadTrace 
{
    public System.Action OnStart;
    public System.Action OnStop;
    public System.Action OnDestroy;

	public virtual void Init(CileadTrace trace)
	{
	}

	public virtual void Init(ref TraceParam _param)
	{
	}

	public virtual void RecordEvent (string _key)
	{
        Debug.LogWarning("Not Implement!!! Ignore it, if you are in editor");
    }

    public abstract void RecordEvent(string _key, int _count);

	public virtual void RecordEvent (string _key, double _sum)
	{
        Debug.LogWarning("Not Implement!!! Ignore it, if you are in editor");
    }

	public virtual void RecordEventDuration (string _key, int _duration)
	{
        Debug.LogWarning("Not Implement!!! Ignore it, if you are in editor");
    }

	public virtual void RecordEvent (string _key, int _count, double _sum)
	{
        Debug.LogWarning("Not Implement!!! Ignore it, if you are in editor");
    }

	public virtual void RecordEvent (string _key, Dictionary<string, string> _dic)
	{
        Debug.LogWarning("Not Implement!!! Ignore it, if you are in editor");
    }

    public abstract void RecordEvent(string _key, Dictionary<string, string> _dic, int _count);

	public virtual void RecordEvent (string _key, Dictionary<string, string>  _dic, int _count, double _sum)
	{
        Debug.LogWarning("Not Implement!!! Ignore it, if you are in editor");
    }

	public virtual void RecordEvent (string _key, Dictionary<string, string>  _dic, int _count, double _sum, int _duration)
	{
        Debug.LogWarning("Not Implement!!! Ignore it, if you are in editor");
    }

    public abstract void StartEvent(string _key);

    public abstract void EndEvent(string _key);

    public abstract void EndEvent(string _key, Dictionary<string, string> _dic, int _count, double _sum);

	public virtual void CancelEvent (string _key)
	{
        Debug.LogWarning("Not Implement!!! Ignore it, if you are in editor");
    }

}
