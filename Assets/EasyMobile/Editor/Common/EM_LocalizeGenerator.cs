using UnityEditor;
using UnityEngine;
using EasyMobile;
using System.Reflection;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using SgLib.Editor;
using EasyMobile.Editor;

public class EM_LocalizeGenerator : EditorWindow
{
    public static EM_LocalizeGenerator window;

    [MenuItem(" Tools/多语言GeneratorKeys", false, 1)]
    public static void Execute()
    {
		ConfigFileMgr.LoadLocalizeXML ();
		// Proceed with adding resource keys.
		Hashtable resourceKeys = new Hashtable();

		//通过键的集合取
		foreach (string key in ConfigFileMgr.dictionary.Keys)
		{
			resourceKeys.Add ("Locaize_" + key,key);
		}

		if (resourceKeys.Count > 0)
		{
			// Now build the class.
			EM_EditorUtil.GenerateConstantsClass(
				EM_Constants.GeneratedFolder,
				EM_Constants.RootNameSpace + "." + EM_Constants.LOCALIZEConstantsClassName,
				resourceKeys,
				true
			);
		}
		else
		{
			EM_EditorUtil.Alert("Constants Class Generation", "Please fill in required information for all locaize key.");
		}
    }
}

