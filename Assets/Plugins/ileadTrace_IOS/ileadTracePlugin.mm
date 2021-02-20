//
//  TraceSDKPlugin.c
//  ileadSDKDemo
//
//  Created by ileadgame_5 on 2018/11/1.
//  Copyright © 2018年 ileadgame_5. All rights reserved.
//

#include "ileadTracePlugin.h"
@implementation ileadTracePlugin
// SDK中初始化方法
-(void)initTraceSDK{
    CountlyConfig* config = [CountlyConfig new];
    config.appKey = @"f11bd536d3118158717b64cf74282d044c0ac84f";
    config.host = @"http://47.74.244.137";
    config.enableDebug = YES;
    
    //    config.features = @[CLYPushNotifications, CLYCrashReporting, CLYAutoViewTracking];     //Optional features
    
    //    config.requiresConsent = YES;                                 //Optional consents
    
    //    config.isTestDevice = YES;                                    //Optional marking as test device for CLYPushNotifications
    config.sendPushTokenAlways = YES;                             //Optional forcing to send token always
    config.doNotShowAlertForNotifications = YES;                  //Optional disabling alerts shown by notification
    
    //----location city ioscountryCode IP这几个都有后台直接获取
    //    config.location = (CLLocationCoordinate2D){35.9925643207,137.4250404704}; //Optional location for geo-location push
    //    config.city = @"Anguilla";                                       //Optional city name for geo-location push
    //    config.ISOCountryCode = @"NA";                                //Optional ISO country code for geo-location push
    //    config.IP = @"192.168.1.201";                                     //Optional IP address for geo-location push
    //这个硬件码是由系统自主获取的
    //    config.deviceID = @"customDeviceID";                          //Optional custom or system generated device ID
    //    config.forceDeviceIDInitialization = YES;                     //Optional forcing to re-initialize device ID
    config.applyZeroIDFAFix = YES;                                //Optional Zero-IDFA fix
    
    //    config.updateSessionPeriod = 30;                              //Optional update session period (default 60 seconds)
    //    config.manualSessionHandling = YES;                           //Optional manual session handling
    
    //    config.eventSendThreshold = 5;                                //Optional event send threshold (default 10 events)
    //    config.storedRequestsLimit = 500;                             //Optional stored requests limit (default 1000 requests)
    config.alwaysUsePOST = YES;                                   //Optional forcing for POST method
    
    //    config.enableAppleWatch = YES;                                //Optional Apple Watch related features
    //    config.enableAttribution = YES;                               //Optional attribution
    
    config.crashSegmentation = @{@"SomeOtherSDK":@"v3.4.5"};      //Optional crash segmentation for CLYCrashReporting
    config.crashLogLimit = 5;                                     //Optional crash log limiy
    
    config.pinnedCertificates = @[@"count.ly.cer"];               //Optional bundled certificates for certificate pinning
    config.customHeaderFieldName = @"X-My-Custom-Field";          //Optional custom header field name
    config.customHeaderFieldValue = @"my_custom_value";           //Optional custom header field value
    config.secretSalt = @"secretsalt";                             //Optional salt for parameter tampering protection
    
    config.starRatingMessage = @"Would you rate the app?";        //Optional star-rating dialog message
    config.starRatingSessionCount = 3;                            //Optional star-rating dialog auto-ask by session count
    config.starRatingDisableAskingForEachAppVersion = YES;        //Optional star-rating dialog auto-ask by versions disabling
    config.starRatingCompletion = ^(NSInteger rating){ NSLog(@"rating %d",(int)rating); };        //Optional star-rating dialog auto-ask completion block
    
    [Countly.sharedInstance startWithConfig:config];
}

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
starRatingDisableAskingForEachAppVersion:(Boolean) _starRatingDisableAskingForEachAppVersion
{
    CountlyConfig* config = [CountlyConfig new];
    config.appKey = _appkey;
    config.host = _port;
    config.enableDebug = _debug;
    if(_viewTrack)
    config.features = @[CLYPushNotifications, CLYCrashReporting, CLYAutoViewTracking];     //Optional features
    
    config.requiresConsent = _requiresConsent;                                 //Optional consents
    
    config.isTestDevice = _isTestDevice;                                    //Optional marking as test device for CLYPushNotifications
    config.sendPushTokenAlways = _sendPush;                             //Optional forcing to send token always
    config.doNotShowAlertForNotifications = _doNotShowAlertForNotifications;                  //Optional disabling alerts shown by notification
    
    //----location city ioscountryCode IP这几个都有后台直接获取
    //    config.location = (CLLocationCoordinate2D){35.9925643207,137.4250404704}; //Optional location for geo-location push
    //    config.city = @"Anguilla";                                       //Optional city name for geo-location push
    //    config.ISOCountryCode = @"NA";                                //Optional ISO country code for geo-location push
    //    config.IP = @"192.168.1.201";                                     //Optional IP address for geo-location push
    //这个硬件码是由系统自主获取的
    //    config.deviceID = @"customDeviceID";                          //Optional custom or system generated device ID
    //    config.forceDeviceIDInitialization = YES;                     //Optional forcing to re-initialize device ID
    config.applyZeroIDFAFix = _applyZeroIDFAFix;                                //Optional Zero-IDFA fix
    
        config.updateSessionPeriod = _updateSessionPeriod;                              //Optional update session period (default 60 seconds)
        config.manualSessionHandling = _manualSessionHandling;                           //Optional manual session handling
    
        config.eventSendThreshold = _eventSendThreshold;                                //Optional event send threshold (default 10 events)
        config.storedRequestsLimit = _storeRequestsLimit;                             //Optional stored requests limit (default 1000 requests)
    config.alwaysUsePOST = _alwaysUsePOST;                                   //Optional forcing for POST method
    
        config.enableAppleWatch = _enableAppleWatch;                                //Optional Apple Watch related features
        config.enableAttribution = _enableAttribution;                               //Optional attribution
    
    config.crashSegmentation = @{@"SomeOtherSDK":@"v3.4.5"};      //Optional crash segmentation for CLYCrashReporting
    config.crashLogLimit = _crashLogLimit;                                     //Optional crash log limiy
    
    config.pinnedCertificates = @[@"count.ly.cer"];               //Optional bundled certificates for certificate pinning
    config.customHeaderFieldName = @"X-My-Custom-Field";          //Optional custom header field name
    config.customHeaderFieldValue = @"my_custom_value";           //Optional custom header field value
    config.secretSalt = _secretSalt;                             //Optional salt for parameter tampering protection
    
    config.starRatingMessage = _starRatingMessage;        //Optional star-rating dialog message
    config.starRatingSessionCount = _starRatingSessionCount;                            //Optional star-rating dialog auto-ask by session count
    config.starRatingDisableAskingForEachAppVersion = _starRatingDisableAskingForEachAppVersion;        //Optional star-rating dialog auto-ask by versions disabling
    config.starRatingCompletion = ^(NSInteger rating){ NSLog(@"rating %d",(int)rating); };        //Optional star-rating dialog auto-ask completion block
    
    [Countly.sharedInstance startWithConfig:config];
}
@end
