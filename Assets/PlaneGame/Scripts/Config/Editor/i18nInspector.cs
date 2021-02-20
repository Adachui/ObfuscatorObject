using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

[ExecuteInEditMode()]
[CustomEditor(typeof(i18n))]
public class i18nInspector : Editor
{
    GUIStyle m_boxStyle;
    i18n m_script;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        m_script = (i18n)target;

        if(GUILayout.Button("预览"))
        {
            ApplyForEditor();
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        m_boxStyle = new GUIStyle(GUI.skin.box);
        m_boxStyle.normal.textColor = m_script.GetColor(m_script.m_colorType);

        switch(m_script.m_colorType)
        {
            case i18n.ColorType.COLOR_WHITE:
                m_boxStyle.normal.textColor = Color.white;
                GUILayout.Box("白色", m_boxStyle, GUILayout.ExpandWidth(true));
                break;

            case i18n.ColorType.COLOR_WHITE_ASH:
                GUILayout.Box("白烟灰色 - f7f7ed", m_boxStyle, GUILayout.ExpandWidth(true));
                break;

            case i18n.ColorType.COLOR_PASTEL_YELLOW:
                GUILayout.Box("粉黄色 - fde9a4", m_boxStyle, GUILayout.ExpandWidth(true));
                break;

            case i18n.ColorType.COLOR_COIR_BROWNNESS:
                GUILayout.Box("棕褐色 - c8bba04", m_boxStyle, GUILayout.ExpandWidth(true));
                break;

            case i18n.ColorType.COLOR_ONDINE:
                GUILayout.Box("淡绿色 - 72de32", m_boxStyle, GUILayout.ExpandWidth(true));
                break;

            case i18n.ColorType.COLOR_LIGHT_GRASS_GREEN:
                GUILayout.Box("淡草绿色 - 6a984b", m_boxStyle, GUILayout.ExpandWidth(true));
                break;

            case i18n.ColorType.COLOR_CROCI:
                GUILayout.Box("橘黄色 - fd6e0e", m_boxStyle, GUILayout.ExpandWidth(true));
                break;

            case i18n.ColorType.COLOR_LIGHT_RED:
                GUILayout.Box("浅红色 - de3232", m_boxStyle, GUILayout.ExpandWidth(true));
                break;

            case i18n.ColorType.COLOR_LIGHT_GRAY:
                GUILayout.Box("浅灰色 - a7a79b", m_boxStyle, GUILayout.ExpandWidth(true));
                break;

            case i18n.ColorType.COLOR_LIGHT_YELLOW:
                GUILayout.Box("浅黄色 - F4E191", m_boxStyle, GUILayout.ExpandWidth(true));
                break;

			case i18n.ColorType.COLOR_DARK_YELLOW:
				GUILayout.Box("浅黄色 - FD6E0EFF", m_boxStyle, GUILayout.ExpandWidth(true));
				break;

        }
    }

    /// <summary>
    /// 编辑器预览效果
    /// </summary>
    private void ApplyForEditor()
    {
        Text text = m_script.gameObject.GetComponent<Text>();

        // 文本设置
        if(!string.IsNullOrEmpty(m_script.m_tag))
        {
			ConfigFileMgr.SetUIText(text,m_script.m_tag);
        }

        // 颜色设置
        text.color = m_script.GetColor(m_script.m_colorType);

        // 界面不会自动刷新，这里手动刷新
        text.gameObject.SetActive(false);
        text.gameObject.SetActive(true);
    }
}