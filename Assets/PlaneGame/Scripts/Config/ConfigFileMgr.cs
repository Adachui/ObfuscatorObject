using System;
using System.Xml;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyAlphabetArabic;

public class ConfigFileMgr
{
	public static Dictionary<string,string> dictionary = new Dictionary<string, string> ();
	public static void LoadLocalizeXML()
	{
		if (dictionary.Count > 0)
			return;
		
		TextAsset text = Resources.Load<TextAsset> ("Config/LocalizeXML");
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(text.text);
	
		var node = xmlDoc.SelectSingleNode ("language/" + GetSysLanguage ());
		if (node != null) {
			XmlNodeList xmlNodeList = node.ChildNodes;
			foreach(XmlElement xl1 in xmlNodeList)
			{
				dictionary.Add (xl1.Name,xl1.InnerText);
			}
		}
	}

	public static string GetSysLanguage()
	{
		SystemLanguage language = Application.systemLanguage;
		if (language == SystemLanguage.Japanese) {
			return "Japanese";
		}
		else if (language == SystemLanguage.ChineseSimplified || language == SystemLanguage.Chinese) {
			return "Chinese";
		}
		else if (language == SystemLanguage.French) {
			return "French";
		}
		else if (language == SystemLanguage.Italian) {
			return "Italian";
		}
		else if (language == SystemLanguage.Spanish) {
			return "Spanish";
		}
		else if (language == SystemLanguage.German) {
			return "German";
		}
		else if (language == SystemLanguage.Portuguese) {
			return "Portuguese";
		}
		else if (language == SystemLanguage.Korean) {
			return "Korea";
		}
		else if (language == SystemLanguage.Russian) {
			return "Russia";
		}
		else if (language == SystemLanguage.ChineseTraditional) {
			return "TraditionChinese";
		}
		return "English";
	}

	public static string GetTaskDesSubffix()
	{
		SystemLanguage language = Application.systemLanguage;
		if (language == SystemLanguage.Japanese) {
			return "Desc_JP";
		}
		else if (language == SystemLanguage.ChineseSimplified || language == SystemLanguage.Chinese) {
			return "Desc_CN";
		}
		else if (language == SystemLanguage.French) {
			return "Desc_Fr";
		}
		else if (language == SystemLanguage.Italian) {
			return "Desc_IT";
		}
		else if (language == SystemLanguage.Spanish) {
			return "Desc_SP";
		}
		else if (language == SystemLanguage.German) {
			return "Desc_GR";
		}
		else if (language == SystemLanguage.Portuguese) {
			return "Desc_PT";
		}
		else if (language == SystemLanguage.Korean) {
			return "Desc_RU";
		}
		else if (language == SystemLanguage.Russian) {
			return "Desc_KR";
		}
		else if (language == SystemLanguage.ChineseTraditional) {
			return "Desc_TW";
		}
		return "Desc_EN";
	}

	//阿拉伯字体显示
	public static void SetArabicUIText	(Text text,string key,int WithLineWrapping = 0)
	{
		string content = GetContentByKey (key);

		if (text != null && content != null) 
		{
			if (WithLineWrapping == 0) 
			{
				text.text = EasyArabicCore.CorrectString (content);
			}
			else 
			{
				text.text = content;
				text.text = EasyArabicCore.CorrectWithLineWrapping(content,text,1);
			}
		}
	}

	public static void SetUIText(Text text,string key)
	{
		string content = GetContentByKey (key);
		text.text = content;
	}
		
	public static string GetContentByKey(string key,Text text = null)
	{
		string result = "";
		if (dictionary.Count == 0)
			LoadLocalizeXML ();

		dictionary.TryGetValue (key,out result);
		if (text != null) {
			var font = i18n.GetTextFont ();
			if(font != null)
				text.font = font;
		}
		return result;
	}

	/// <summary>
	/// Sets the UGUI text.
	/// </summary>
	/// <param name="text">Text. ugui text组件</param>
	/// <param name="content">Content. 组件显示内容</param>
	/// <param name="WithLineWrapping">With line wrapping. 阿语换行设置</param>
	public static void SetUGUIText(Text text,string content,int WithLineWrapping = 0)
	{
		if(Application.systemLanguage == SystemLanguage.Arabic)
		{
			if (WithLineWrapping == 0) 
			{
				text.text = EasyArabicCore.CorrectString (content);
			}
			else 
			{
				text.text = content;
				text.text = EasyArabicCore.CorrectWithLineWrapping(content,text,1);
			}
		}
		else
		{
			text.text = content;
		}
	}
}





