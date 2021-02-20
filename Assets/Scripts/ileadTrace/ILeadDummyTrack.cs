using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ILeadDummyTrack : ileadTrace
{
    public override void CancelEvent(string _key)
    {
   
    }

    public override void EndEvent(string _key)
    {
        Debug.Log("EndEvent" + _key);
    }

    public override void EndEvent(string _key, Dictionary<string, string> _dic, int _count, double _sum)
    {

    }

    public override void Init(CileadTrace trace)
    {

    }

    public override void Init(ref TraceParam _param)
    {

    }

    public override void RecordEvent(string _key, int _count)
    {
        Debug.Log("RecordEvent[ key:" + _key + " count:" + _count + "]");
    }

    public override void RecordEvent(string _key, Dictionary<string, string> _dic, int _count)
    {
        Debug.Log("RecordEvent From Editor");
    }

    public override void RecordEvent(string _key)
    {
      
    }

    public override void RecordEvent(string _key, double _sum)
    {
      
    }

    public override void RecordEvent(string _key, int _count, double _sum)
    {
    
    }

    public override void RecordEvent(string _key, Dictionary<string, string> _dic)
    {
       
    }

    public override void RecordEvent(string _key, Dictionary<string, string> _dic, int _count, double _sum)
    {

    }

    public override void RecordEvent(string _key, Dictionary<string, string> _dic, int _count, double _sum, int _duration)
    {

    }

    public override void RecordEventDuration(string _key, int _duration)
    {

    }

    public override void StartEvent(string _key)
    {
        Debug.Log("StartEvent" + _key);
    }
}
