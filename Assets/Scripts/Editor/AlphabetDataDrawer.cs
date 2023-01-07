using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(AlphabetData))]
[CanEditMultipleObjects]
[Serializable]
public class AlphabetDataDrawer : Editor
{
    ReorderableList AlphabetPlainList;
    ReorderableList AlphabetNormalList;
    ReorderableList AlphabetHighlightedList;
    ReorderableList AlphabetWrongList;

    void OnEnable()
    {
        InitializeReordableList(ref AlphabetPlainList, "AlphabetPlain", "Texto plano");
        InitializeReordableList(ref AlphabetNormalList, "AlphabetNormal", "Texto normal");
        InitializeReordableList(ref AlphabetHighlightedList, "AlphabetHighlighted", "Texto subrayado");
        InitializeReordableList(ref AlphabetWrongList, "AlphabetWrong", "Texto incorrecto");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        AlphabetPlainList.DoLayoutList();
        AlphabetNormalList.DoLayoutList();
        AlphabetHighlightedList.DoLayoutList();
        AlphabetWrongList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }

    void InitializeReordableList(ref ReorderableList list, string propertyName, string listLabel)
    {
        list = new ReorderableList(serializedObject, serializedObject.FindProperty(propertyName),
            true, true, true, true);

        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, listLabel);
        };

        var l = list;

        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = l.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;

            EditorGUI.PropertyField
            (new Rect (rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("letter"), GUIContent.none);

            EditorGUI.PropertyField
            (new Rect(rect.x + 70, rect.y, rect.width - 60 - 30, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("image"), GUIContent.none);
        };
    }
}
