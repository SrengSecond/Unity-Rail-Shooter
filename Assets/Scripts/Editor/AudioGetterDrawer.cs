using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AudioGetter))]
public class AudioGetterDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // EditorGUI.LabelField(position,"We 'vs modified Audio");
        position = EditorGUI.PrefixLabel(position, label);
        property.FindPropertyRelative("id").intValue = EditorGUI.Popup(position, property.FindPropertyRelative("id").intValue,AudioLibery.audioNameList.ToArray());
        // property.FindPropertyRelative("audioName").stringValue = AudioLibery.audioNameList[property.FindPropertyRelative("id").intValue];
    }
}
