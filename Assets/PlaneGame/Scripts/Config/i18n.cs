using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

[AddComponentMenu("Custom/UI/i18n")]
[RequireComponent(typeof(Text))]
public class i18n : MonoBehaviour
{
    public enum ColorType
    {
        COLOR_WHITE,
        COLOR_WHITE_ASH,
        COLOR_PASTEL_YELLOW,
        COLOR_COIR_BROWNNESS,
        COLOR_ONDINE,
        COLOR_LIGHT_GRASS_GREEN,
        COLOR_CROCI,
        COLOR_LIGHT_RED,
        COLOR_LIGHT_GRAY,
        COLOR_LIGHT_YELLOW,
		COLOR_DARK_YELLOW,
		COLOR_BLUE,
        COLOR_PURPLE,
		COLOR_DARK_GRASS,
        COLOR_LIGHT_PURPLE,
        COLOR_LITTLE_YELLOW,
		COLOR_CITY_MONSTER_1,
    }

	public static Font arabicFont= null;
    public string m_tag = null;
    public ColorType m_colorType = ColorType.COLOR_WHITE;

    // 用于编辑器模式下预览文字内容
	public SystemLanguage m_editor = SystemLanguage.English;
	public int m_arabicLineWrapping = 0;

    private static Dictionary<ColorType, Color> m_colorDic = null;
    private Text m_text = null;
	private object param = null;

    void Awake()
    {
        m_text = gameObject.GetComponent<Text>();
//        m_text.color = GetColor(m_colorType);

		ChangeTextByFormat ();
    }
		
    /// <summary>
    /// 文本设置 默认是没有参数的 如果有参数需要重新传入参数设置一次
    /// </summary>
    /// <param name="obj"></param>
	public void ChangeTextByFormat(object obj = null)
    {
		if (GetTextFont() != null) {
			m_text.font =  GetTextFont();
		}

        if(string.IsNullOrEmpty(m_tag))
        {
            return;
        }
		param = obj;
        try
        {
			if(Application.systemLanguage == SystemLanguage.Arabic)
			{
				ConfigFileMgr.SetArabicUIText(m_text,m_tag);
			}
            else
			{
				ConfigFileMgr.SetUIText(m_text,m_tag);
			}
        }
        catch (Exception)
        {
            Debug.LogError("content missing " + m_tag);
        }
    }

    public Color GetColor(ColorType colorType)
    {
        return GetColorDic()[colorType];
    }

    private static Dictionary<ColorType, Color> GetColorDic()
    {
        if(m_colorDic == null)
        {
            m_colorDic = new Dictionary<ColorType, Color>();

            m_colorDic.Add(ColorType.COLOR_WHITE, GetColor(255, 255, 255));
            m_colorDic.Add(ColorType.COLOR_WHITE_ASH, GetColor(247, 247, 237));
            m_colorDic.Add(ColorType.COLOR_PASTEL_YELLOW, GetColor(253, 233, 164));
            m_colorDic.Add(ColorType.COLOR_COIR_BROWNNESS, GetColor(200, 187, 160));
            m_colorDic.Add(ColorType.COLOR_ONDINE, GetColor(114, 222, 50));
            m_colorDic.Add(ColorType.COLOR_LIGHT_GRASS_GREEN, GetColor(106, 152, 75));
            m_colorDic.Add(ColorType.COLOR_CROCI, GetColor(253, 110, 14));
            m_colorDic.Add(ColorType.COLOR_LIGHT_RED, GetColor(222, 50, 50));
            m_colorDic.Add(ColorType.COLOR_LIGHT_GRAY, GetColor(167, 167, 155));
            m_colorDic.Add(ColorType.COLOR_LIGHT_YELLOW, GetColor(244, 225, 145));
			m_colorDic.Add(ColorType.COLOR_DARK_YELLOW, GetColor(253, 110, 14));
			m_colorDic.Add(ColorType.COLOR_BLUE, GetColor(30, 144, 255));
            m_colorDic.Add(ColorType.COLOR_PURPLE, GetColor(117, 1, 189));
            m_colorDic.Add(ColorType.COLOR_DARK_GRASS, GetColor(139, 255, 120));
            m_colorDic.Add(ColorType.COLOR_LIGHT_PURPLE, GetColor(255, 120, 244));
			m_colorDic.Add(ColorType.COLOR_LITTLE_YELLOW, GetColor(253, 241, 193));
			m_colorDic.Add(ColorType.COLOR_CITY_MONSTER_1, GetColor(185,193,216));
        }

        return m_colorDic;
    }

    private static Color GetColor(float r, float g, float b)
    {
        return new Color(r / 255, g / 255, b / 255);
    }


	public static Font GetTextFont()
	{
		if (Application.systemLanguage == SystemLanguage.Arabic) {
			if(arabicFont == null)
				arabicFont =  Resources.Load<Font>("arabicfont");
			return arabicFont;
		}
		else if(Application.systemLanguage == SystemLanguage.Chinese || Application.systemLanguage == SystemLanguage.ChineseSimplified){
			if(arabicFont == null)
				arabicFont =  Resources.Load<Font>("GBK");
			return arabicFont;
		}
		else {
			return null;
		}
	}
}
