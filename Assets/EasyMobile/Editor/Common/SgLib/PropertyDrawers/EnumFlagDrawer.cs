using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Reflection;
using SgLib.Attributes;

namespace SgLib.Editor
{
    [CustomPropertyDrawer(typeof(EnumFlagAttribute))]
    public class EnumFlagDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EnumFlagAttribute flagSettings = (EnumFlagAttribute)attribute;
            Enum targetEnum = GetBaseProperty<Enum>(property);
            string propName = flagSettings.enumName;
            Enum enumNew;

            EditorGUI.BeginProperty(position, label, property);

            if (string.IsNullOrEmpty(propName))
            {
                #if UNITY_2017_1_OR_NEWER
                //enumNew = EditorGUI.EnumFlagsField(position, label, targetEnum);
                #else
                enumNew = EditorGUI.EnumMaskField(position, label, targetEnum);
                #endif
            }
            else
            {
                #if UNITY_2017_1_OR_NEWER
                //enumNew = EditorGUI.EnumFlagsField(position, propName, targetEnum);
                #else
                enumNew = EditorGUI.EnumMaskField(position, propName, targetEnum);
                #endif
            }

            //property.intValue = (int)Convert.ChangeType(enumNew, targetEnum.GetType());
            EditorGUI.EndProperty();
        }

        static T GetBaseProperty<T>(SerializedProperty prop)
        {
            // Separate the steps it takes to get to this property
            string[] separatedPaths = prop.propertyPath.Split('.');

            // Go down to the root of this serialized property
            System.Object reflectionTarget = prop.serializedObject.targetObject as object;
            // Walk down the path to get the target object
            foreach (var path in separatedPaths)
            {
                FieldInfo fieldInfo = reflectionTarget.GetType().GetField(path, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                reflectionTarget = fieldInfo.GetValue(reflectionTarget);
            }
            return (T)reflectionTarget;
        }
    }
}