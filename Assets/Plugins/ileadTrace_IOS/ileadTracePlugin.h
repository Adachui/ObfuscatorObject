//
//  ileadTracePlugin.h
//  ileadSDKDemo
//
//  Created by ileadgame_5 on 2018/11/1.
//  Copyright © 2018年 ileadgame_5. All rights reserved.
//

#include <stdio.h>
#import "ileadTracePlugin.h"
#import <tracesdk/Countly.h>
#include <string>
#include <vector>

using namespace std;

//#ifdef __clang__
//#define NS_ROOT_CLASS __attribute__((objc_root_class))
//#else
//#define NS_ROOT_CLASS
//#endif
//固定代码
#if defined(__cplusplus)
extern "C"{
#endif
    extern void UnitySendMessage(const char *, const char *, const char *);
//    extern NSString* _CreateNSString (const char* string);
#if defined(__cplusplus)
}
#endif
#pragma pack(push,1)
struct TraceParam
{
    char _appkey[50];
    char _secretSalt[50];
    char _starRatingMessage[50];
    char _port[50];
    bool _debug;
    bool _viewTrack;
    bool _requiresConsent;
    bool _isTestDevice;
    bool _sendPush;
    bool _doNotShowAlertForNotifications;
    bool _applyZeroIDFAFix;
    bool _manualSessionHandling;
    bool _alwaysUsePOST;
    bool _enableAppleWatch;
    bool _enableAttribution;
    bool _starRatingDisableAskingForEachAppVersion;

    int _updateSessionPeriod;
    int _eventSendThreshold;
    int _storeRequestsLimit;

    int _crashLogLimit;
    int _starRatingSessionCount;
};
#pragma pack(pop) 
@interface ileadTracePlugin : NSObject
-(void)initTraceSDK;
-(void)initTraceSDK:(NSString* )_appkey
               port:(NSString* )_port
        enableDebug:(Boolean) _debug
    enableViewTrack:(Boolean) _viewTrack
    requiresConsent:(Boolean) _requiresConsent
    isTestDevice:(Boolean) _isTestDevice
    sendPushTokenAlways:(Boolean) _sendPush
    doNotShowAlertForNotifications:(Boolean) _doNotShowAlertForNotifications
    applyZeroIDFAFix:(Boolean) _applyZeroIDFAFix
    updateSessionPeriod:(int) _updateSessionPeriod
    manualSessionHandling:(Boolean) _manualSessionHandling
    eventSendThreshold:(int) _eventSendThreshold
    storedRequestsLimit:(int) _storeRequestsLimit
    alwaysUsePOST:(Boolean) _alwaysUsePOST
    enableAppleWatch:(Boolean) _enableAppleWatch
    enableAttribution:(Boolean) _enableAttribution
    crashLogLimit:(int) _crashLogLimit
         secretSalt:(NSString* ) _secretSalt
  starRatingMessing:(NSString* ) _starRatingMessage
starRatingSessionCount:(int) _starRatingSessionCount
starRatingDisableAskingForEachAppVersion:(Boolean) _starRatingDisableAskingForEachAppVersion;

@end

#if defined(__cplusplus)
extern "C"{
#endif
    
    //字符串转化的工具函数
    char* _MakeStringCopy( const char* string)
    {
        if (NULL == string) {
            return NULL;
        }
        char* res = (char*)malloc(strlen(string)+1);
        strcpy(res, string);
        return res;
    }
    
    NSString* _CreateNSString (const char* string)
    {
//        char* curStr = _MakeStringCopy(string);
        if(string == NULL)
	    return [[NSString alloc] initWithUTF8String: ""];
//        if( strcmp(string, "null") != 0 )
//            return [[NSString alloc] initWithUTF8String: ""];
//        if( strcmp(string, "\0") != 0 )
//            return [[NSString alloc] initWithUTF8String: ""];
//        if (string)
//            return [[NSString alloc] initWithUTF8String: string];
//        else
//            return [[NSString alloc] initWithUTF8String: ""];
        if (string)
            return [NSString stringWithUTF8String: string];
        else
            return [NSString stringWithUTF8String: ""];
    }


    
    static ileadTracePlugin *mTrace;
    
    //供u3d调用的c函数 ( 因测试的sdk调用初始化为不传参方法，如果调用的sdk需要传参调用，此处应相应修改为含参数方法)
    void initTraceSDK()
    {
        if(mTrace==NULL)
        {
            mTrace = [[ileadTracePlugin alloc] init];
        }
        [mTrace initTraceSDK];
    }
    
    void initTraceSDKWithParam2 ( TraceParam _param)
    {
        NSLog(@"sizeof TraceParam:%d", (int)sizeof(_param));
        NSLog(@"sizeof TraceParam _appkey :%d", (int)sizeof(_param._appkey));
        NSLog(@"sizeof TraceParam _secretSalt :%d", (int)sizeof(_param._secretSalt));
        NSLog(@"sizeof TraceParam _starRatingMessage :%d", (int)sizeof(_param._starRatingMessage));
        NSLog(@"sizeof TraceParam _debug :%d", (int)sizeof(_param._debug));
        NSLog(@"sizeof TraceParam _viewTrack :%d", (int)sizeof(_param._viewTrack));
        NSLog(@"sizeof TraceParam _requiresConsent :%d", (int)sizeof(_param._requiresConsent));
        NSLog(@"sizeof TraceParam _isTestDevice :%d", (int)sizeof(_param._isTestDevice));
        NSLog(@"sizeof TraceParam _sendPush :%d", (int)sizeof(_param._sendPush));
        NSLog(@"sizeof TraceParam _doNotShowAlertForNotifications :%d", (int)sizeof(_param._doNotShowAlertForNotifications));
        NSLog(@"sizeof TraceParam _applyZeroIDFAFix :%d", (int)sizeof(_param._applyZeroIDFAFix));
        NSLog(@"sizeof TraceParam _updateSessionPeriod :%d", (int)sizeof(_param._updateSessionPeriod));
        NSLog(@"sizeof TraceParam _manualSessionHandling :%d", (int)sizeof(_param._manualSessionHandling));
        NSLog(@"sizeof TraceParam _eventSendThreshold :%d", (int)sizeof(_param._eventSendThreshold));
        NSLog(@"sizeof TraceParam _storeRequestsLimit :%d", (int)sizeof(_param._storeRequestsLimit));
        NSLog(@"sizeof TraceParam _alwaysUsePOST :%d", (int)sizeof(_param._alwaysUsePOST));
        NSLog(@"sizeof TraceParam _enableAppleWatch :%d", (int)sizeof(_param._enableAppleWatch));
        NSLog(@"sizeof TraceParam _enableAttribution :%d", (int)sizeof(_param._enableAttribution));
        NSLog(@"sizeof TraceParam _crashLogLimit :%d", (int)sizeof(_param._crashLogLimit));
        NSLog(@"sizeof TraceParam _starRatingSessionCount :%d", (int)sizeof(_param._starRatingSessionCount));
        NSLog(@"sizeof TraceParam _starRatingDisableAskingForEachAppVersion :%d", (int)sizeof(_param._starRatingDisableAskingForEachAppVersion));
        if(mTrace==NULL)
        {
            mTrace = [[ileadTracePlugin alloc] init];
        }
        NSString* curAppkey = _CreateNSString(_param._appkey);
        NSString* curSecret = _CreateNSString(_param._secretSalt);
        NSString* curRateMes = _CreateNSString(_param._starRatingMessage);
        NSString* curPort = _CreateNSString(_param._port);
        [mTrace initTraceSDK: curAppkey
                        port:curPort
                 enableDebug: _param._debug
             enableViewTrack: _param._viewTrack
             requiresConsent: _param._requiresConsent
                isTestDevice: _param._isTestDevice
         sendPushTokenAlways: _param._sendPush
doNotShowAlertForNotifications: _param._doNotShowAlertForNotifications
            applyZeroIDFAFix: _param._applyZeroIDFAFix
         updateSessionPeriod: _param._updateSessionPeriod
       manualSessionHandling: _param._manualSessionHandling
          eventSendThreshold: _param._eventSendThreshold
         storedRequestsLimit: _param._storeRequestsLimit
               alwaysUsePOST: _param._alwaysUsePOST
            enableAppleWatch: _param._enableAppleWatch
           enableAttribution: _param._enableAttribution
               crashLogLimit: _param._crashLogLimit
                  secretSalt:curSecret
           starRatingMessing:curRateMes
      starRatingSessionCount: _param._starRatingSessionCount
starRatingDisableAskingForEachAppVersion: _param._starRatingDisableAskingForEachAppVersion];
    }
#pragma mark ----Events Function
    void SplitString(const string& s, vector<string>& v, const string& c)
    {
        string::size_type pos1, pos2;
        pos2 = s.find(c);
        pos1 = 0;
        while(string::npos != pos2)
        {
            v.push_back(s.substr(pos1, pos2-pos1));
            
            pos1 = pos2 + c.size();
            pos2 = s.find(c, pos1);
        }
        if(pos1 != s.length())
            v.push_back(s.substr(pos1));
    }
    
    NSDictionary* SplitDic(const char* _dicStr)
    {
        std::string dicstr(_dicStr);
        vector<string> v;
        SplitString(dicstr, v,","); //可按多个字符来分隔;
        NSMutableDictionary* seg = [[NSMutableDictionary alloc] init];
        for(vector<string>::iterator i = v.begin(); i != v.end(); ++i)
        {
            string key = *i;
            string value = *(i++);
            [seg setValue:[NSString stringWithCString:value.c_str() encoding:[NSString defaultCStringEncoding]] forKey:[NSString stringWithCString:key.c_str() encoding:[NSString defaultCStringEncoding]]];
        }
        return seg;
    }
    
    void recordEvent(const char* _key)
    {
        [Countly.sharedInstance recordEvent:_CreateNSString(_key)];
    }
    
    void recordEvent2(const char* _key, int _count)
    {
        [Countly.sharedInstance recordEvent:_CreateNSString(_key) count:_count];
    }
    
    void recordEvent3(const char* _key, double _sum)
    {
        [Countly.sharedInstance recordEvent:_CreateNSString(_key) sum:_sum];
    }
    
    void recordEvent4(const char* _key, int _duration)
    {
        [Countly.sharedInstance recordEvent:_CreateNSString(_key) duration:_duration];
    }
    
    void recordEvent5(const char* _key, int _count, double _sum)
    {
        [Countly.sharedInstance recordEvent:_CreateNSString(_key) count:_count sum:_sum];
    }
    
    void recordEvent6(const char* _key, const char* _dicStr)
    {
//        char* keyStr = (char*)malloc(strlen(_key)+1);
//        char* dicStr = (char*)malloc(strlen(_dicStr)+1);
//        strcpy(keyStr, _key);
//        strcpy(dicStr, _dicStr);
//        NSLog(@"keyStr = %@",[NSString stringWithUTF8String:keyStr]);
//        NSLog(@"dicStr = %@",[NSString stringWithUTF8String:dicStr]);
//        const char* delim = ",";
//        char* p;
//        p = strtok((char* )_dicStr, delim);
//        while(p)
//        {
//            printf("%s\n", p);
//            p = strtok(NULL, delim);
//        }
        [Countly.sharedInstance recordEvent:[NSString stringWithUTF8String:_key] segmentation:SplitDic(_dicStr)];
    }
    
    void recordEvent7(const char* _key, const char* _dicStr, int _count)
    {
        [Countly.sharedInstance recordEvent:[NSString stringWithUTF8String:_key] segmentation:SplitDic(_dicStr) count:_count];
    }
    
    void recordEvent8(const char* _key, const char* _dicStr, int _count, double _sum)
    {
        [Countly.sharedInstance recordEvent:[NSString stringWithUTF8String:_key] segmentation:SplitDic(_dicStr) count:_count sum:_sum];
    }
    
    void recordEvent9(const char* _key, const char* _dicStr, int _count, double _sum, int _duration)
    {
        [Countly.sharedInstance recordEvent:[NSString stringWithUTF8String:_key] segmentation:SplitDic(_dicStr) count:_count sum:_sum duration:_duration];
    }
    
    void startEvent(const char* _key)
    {
        [Countly.sharedInstance startEvent:[NSString stringWithUTF8String:_key]];
    }
    
    void endEvent(const char* _key)
    {
        [Countly.sharedInstance endEvent:[NSString stringWithUTF8String:_key]];
    }
    
    void endEvent2(const char* _key, const char* _dicStr, int _count, double _sum)
    {
        [Countly.sharedInstance endEvent:[NSString stringWithUTF8String:_key] segmentation:SplitDic(_dicStr) count:_count sum:_sum];
    }
    
    void cancelEvent(const char* _key)
    {
        [Countly.sharedInstance cancelEvent:[NSString stringWithUTF8String:_key]];
    }
#if defined(__cplusplus)
}
#endif /* TraceSDKPlugin_h */
