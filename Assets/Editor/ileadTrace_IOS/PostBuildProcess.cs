using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;
using System.Collections.Generic;
using XcodeUnityCapability = UnityEditor.iOS.Xcode.ProjectCapabilityManager;

public class PostBuildProcess : MonoBehaviour
{
	internal static void CopyAndReplaceDirectory(string srcPath, string dstPath)
	{
		if (Directory.Exists(dstPath))
			Directory.Delete(dstPath);
		if (File.Exists(dstPath))
			File.Delete(dstPath);

		Directory.CreateDirectory(dstPath);

		foreach (var file in Directory.GetFiles(srcPath))
			File.Copy(file, Path.Combine(dstPath, Path.GetFileName(file)));

		foreach (var dir in Directory.GetDirectories(srcPath))
			CopyAndReplaceDirectory(dir, Path.Combine(dstPath, Path.GetFileName(dir)));
	}

	//-----编译IOS的时候添加代码，库，编写Info.plist
	[PostProcessBuild]
	public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
	{
		if (buildTarget == BuildTarget.iOS) {
			BuildForiOS(path);
		}
	}

	private static void BuildForiOS(string path)
	{
		string projPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";
		Debug.Log("Build iOS. path: " + projPath);

		PBXProject proj = new PBXProject();
		var file = File.ReadAllText(projPath);
		proj.ReadFromString(file);

		string target = proj.TargetGuidByName("Unity-iPhone");

		CopyAndReplaceDirectory("Assets/Plugins/ileadTrace_IOS/tracesdk.framework", Path.Combine(path, "Frameworks/tracesdk.framework"));
		proj.AddFileToBuild(target, proj.AddFile("Frameworks/tracesdk.framework", "Frameworks/tracesdk.framework", PBXSourceTree.Source));

		proj.AddFrameworkToProject(target, "WebKit.framework", false);
		proj.AddFrameworkToProject(target, "CoreTelephony.framework", false);
//		proj.AddFrameworkToProject(target, "CoreLocation.framework", false);
		proj.AddFrameworkToProject(target, "SafariServices.framework", false);
		proj.AddFrameworkToProject(target, "WatchConnectivity.framework", false);
		proj.AddFrameworkToProject(target, "UserNotifications.framework", false);
		proj.AddFrameworkToProject(target, "AdSupport.framework", false);
		proj.AddFrameworkToProject(target, "Security.framework", false);


//		CopyAndReplaceDirectory("Frameworks/GoogleMobileAdsSdkiOS-7.1.0/GoogleMobileAds.framework", Path.Combine(path, "Frameworks/GoogleMobileAds.framework"));
//		proj.AddFileToBuild(target, proj.AddFile("Frameworks/GoogleMobileAds.framework", "Frameworks/GoogleMobileAds.framework", PBXSourceTree.Source));

		proj.SetBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", "$(inherited)");
		proj.AddBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", "$(PROJECT_DIR)/Libraries");
		proj.SetBuildProperty(target, "ENABLE_BITCODE", "false");  

		proj.SetBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", "$(SRCROOT)/Frameworks");
		proj.AddBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", "$(inherited)");

		proj.AddBuildProperty(target, "OTHER_LDFLAGS", "-ObjC");
		proj.AddBuildProperty (target, "OTHER_LDFLAGS", "-lc++");
		proj.AddBuildProperty (target, "GCC_PREPROCESSOR_DEFINITIONS", "DEBUG=1");

		/*======== system capabilities ========*/
//		XcodeUnityCapability projCapability = new XcodeUnityCapability(projPath, "Unity-iPhone/mmk.entitlements", "Unity-iPhone");
//		projCapability.AddBackgroundModes(BackgroundModesOptions.LocationUpdates);
//		projCapability.WriteToFile();


//		proj.AddCapability (target, PBXCapabilityType.BackgroundModes);

		//-------------xcode code change-------------//	


		string xcodePath = Path.GetFullPath (path);
		UnityEditor.XCodeEditor.XClass UnityAppController = new UnityEditor.XCodeEditor.XClass(xcodePath + "/Classes/UnityAppController.mm");
		UnityAppController.WriteBelow("#include \"PluginBase/AppDelegateListener.h\"", "#import <tracesdk/Countly.h> \n#import <tracesdk/CountlyCommon.h>");
		UnityAppController.WriteBelow ("SensorsCleanup();\n}", "- (BOOL)application:(UIApplication *)app openURL:(NSURL *)url options:(NSDictionary<UIApplicationOpenURLOptionsKey, id> *)options\n{\n    //    NSLog(@\"sourceApplication: %@\", sourceApplication);\n    NSLog(@\"URL scheme:%@\", [url scheme]);\n    NSLog(@\"URL query: %@\", [url query]);\n    NSArray *array = [[url query] componentsSeparatedByString:@\"&\"];\n    NSLog(@\"array query = %@\", array);\n    NSString* mediaSourceStr = @\"\";\n    NSString* ileadCode = @\"\";\n    for(int i = 0; i < [array count]; ++i)\n    {\n        NSString* curStr = [array objectAtIndex:i];\n        NSArray* curArr = [curStr componentsSeparatedByString:@\"=\"];\n        NSString* keyStr = [curArr objectAtIndex:0];\n        if([keyStr isEqualToString:kCountlyMediaSource])\n        {\n            mediaSourceStr = [curStr substringFromIndex:[kCountlyMediaSource length]+1];\n        }\n        else if ([keyStr isEqualToString:kCountlyIleadCode])\n        {\n            ileadCode = [curStr substringFromIndex:[kCountlyIleadCode length]+1];\n        }\n    }\n    [Countly.sharedInstance SetMediaSource:mediaSourceStr];\n    [Countly.sharedInstance SetIleadCode:ileadCode];\n    if (!CountlyCommon.sharedInstance.manualSessionHandling)\n        [CountlyConnectionManager.sharedInstance beginSession];//session第一条\n\n    return YES;\n}");

		UnityEditor.XCodeEditor.XClass UnityViewControllerBase = new UnityEditor.XCodeEditor.XClass(xcodePath + "/Classes/UI/UnityViewControllerBase.mm");
		UnityViewControllerBase.WriteBelow("    AppController_SendUnityViewControllerNotification(kUnityViewWillAppear);\n}", "- (void)presentViewController:(UIViewController *)viewControllerToPresent animated: (BOOL)flag completion:(void (^ __nullable)(void))completion\n{\n    Class SFAuthenticationViewController = NSClassFromString(@\"SFAuthenticationViewController\");\n    if (SFAuthenticationViewController && [viewControllerToPresent isKindOfClass:SFAuthenticationViewController]) {\n        viewControllerToPresent.view.alpha = 0.0;\n        viewControllerToPresent.view.frame = CGRectZero;\n        viewControllerToPresent.modalPresentationStyle = UIModalPresentationOverCurrentContext;\n    }\n    [super presentViewController:viewControllerToPresent animated:flag completion:completion];\n}");



		proj.SetBuildProperty(target, "DEVELOPMENT_TEAM", "ileadgamech@163.com");
		File.WriteAllText(projPath, proj.WriteToString());


		/*======== plist ========*/
		string plistPath = path + "/Info.plist";
		PlistDocument plist = new PlistDocument();
		plist.ReadFromString(File.ReadAllText(plistPath));

		// Get root
		PlistElementDict rootDict = plist.root;

		// URL schemes 追加
		var urlTypeArray = plist.root.CreateArray("CFBundleURLTypes");
		var urlTypeDict = urlTypeArray.AddDict();
		urlTypeDict.SetString("CFBundleTypeRole", "Editor");
		urlTypeDict.SetString("CFBundleURLName", "ileadsoft.tracesdkScheme");
		var urlScheme = urlTypeDict.CreateArray("CFBundleURLSchemes");
		urlScheme.AddString(Application.identifier);

		//-----set location 
//		plist.root.SetString ("NSLocationAlwaysUsageDescription", "Requires to locate your location.");
//		plist.root.SetString ("NSLocationWhenInUseUsageDescription", "Requires to locate your location.");
		//		urlTypeDict = urlTypeArray.AddDict();
		//		urlTypeDict.SetString("CFBundleURLName", "com.abc.sampleApp");
		//		urlScheme = urlTypeDict.CreateArray("CFBundleURLSchemes");
		//		urlScheme.AddString("sampleApp2");
		File.WriteAllText(plistPath, plist.WriteToString());
	}

	private static void AddUsrLib(PBXProject proj, string targetGuid, string framework)
	{
		string fileGuid = proj.AddFile("usr/lib/"+framework, "Frameworks/"+framework, PBXSourceTree.Sdk);
		proj.AddFileToBuild(targetGuid, fileGuid);
	}
}