using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Random = UnityEngine.Random;

[CustomEditor(typeof(BoardData), false)]
[CanEditMultipleObjects]
[Serializable]
public class BoardDataDrawer : Editor
{
    BoardData GameDataInstance => target as BoardData;
    ReorderableList _dataList;

    private void OnEnable()
    {
        InitializeReordableList(ref _dataList, "SearchWords", "Lista de palabras");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawColumnsRowsInputFields();
        EditorGUILayout.Space();
        ConvertToUpperButton();

        if (GameDataInstance.Board != null && GameDataInstance.Grid.x > 0 && GameDataInstance.Grid.y > 0)
            DrawBoardTable();

        GUILayout.BeginHorizontal();
        ClearBoardButton();
        FillUpRandomLettersButton();
        GUILayout.EndHorizontal();


        EditorGUILayout.Space();
        _dataList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
            EditorUtility.SetDirty(GameDataInstance);
    }

    void DrawColumnsRowsInputFields()
    {
        ///X Filas (Rows)
        ///Y Columnas (Columns)
        Vector2 Temps = new Vector2(
            GameDataInstance.Grid.x,
            GameDataInstance.Grid.y);

        GameDataInstance.Grid.y = EditorGUILayout.IntField("Columnas", (int)GameDataInstance.Grid.y);
        GameDataInstance.Grid.x = EditorGUILayout.IntField("Filas", (int)GameDataInstance.Grid.x);

        if ((GameDataInstance.Grid.x != Temps.x ||
            GameDataInstance.Grid.y != Temps.y) &&

            GameDataInstance.Grid.y > 0 &&
            GameDataInstance.Grid.x > 0)
        {
            GameDataInstance.CreateNewBoard();
        }
    }

    void DrawBoardTable()
    {
        var tableStyle = new GUIStyle("box")
        {
            padding = new RectOffset(10, 10, 10, 10),
        };
        tableStyle.margin.left = 32;

        var headerColumnsStyle = new GUIStyle()
        {
            fixedWidth = 35
        };

        var columnStyle = new GUIStyle()
        {
            fixedWidth = 25
        };

        var rowStyle = new GUIStyle()
        {
            fixedHeight = 25,
            fixedWidth = 25,
            alignment = TextAnchor.MiddleCenter
        };

        var textFieldStyle = new GUIStyle()
        {
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter
        };
        textFieldStyle.normal.background = Texture2D.grayTexture;
        textFieldStyle.normal.textColor = Color.white;

        EditorGUILayout.BeginHorizontal(tableStyle);
        for (int x = 0; x < GameDataInstance.Grid.y; x++)
        {
            EditorGUILayout.BeginVertical(x == -1 ? headerColumnsStyle : columnStyle);

            for (int y = 0; y < GameDataInstance.Grid.x; y++)
            {
                if (x >= 0 && y >= 0)
                {
                    EditorGUILayout.BeginHorizontal(rowStyle);
                    var character = EditorGUILayout.TextArea(GameDataInstance.Board[x].Row[y], textFieldStyle);

                    if (GameDataInstance.Board[x].Row[y].Length > 1)
                        character = GameDataInstance.Board[x].Row[y].Substring(0, 1);

                    GameDataInstance.Board[x].Row[y] = character;
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
    }

    void InitializeReordableList(ref ReorderableList list, string propertyName, string listLabel)
    {
        list = new ReorderableList(serializedObject, serializedObject.FindProperty(propertyName), true, true, true, true);

        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, listLabel);
        };

        var l = list;
        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = l.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;

            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("Word"), GUIContent.none);
        };
    }

    void ConvertToUpperButton()
    {
        if (GUILayout.Button("Mayúsculas"))
        {
            for (int i = 0; i < GameDataInstance.Grid.y; i++)
            {
                for (int j = 0; j < GameDataInstance.Grid.x; j++)
                {
                    int errorCounter = Regex.Matches(GameDataInstance.Board[i].Row[j], @"[a-z]").Count;

                    if (errorCounter > 0)
                    {
                        GameDataInstance.Board[i].Row[j] = GameDataInstance.Board[i].Row[j].ToUpper();
                    }
                }
            }

            foreach (var word in GameDataInstance.SearchWords)
            {
                int errorCounter = Regex.Matches(word.Word, @"[a-z]").Count;

                if (errorCounter > 0)
                    word.Word = word.Word.ToUpper();
            }
        }
    }

    void ClearBoardButton()
    {
        if (GUILayout.Button("Borrar tablero"))
            for (int i = 0; i < GameDataInstance.Grid.y; i++)
                for (int j = 0; j < GameDataInstance.Grid.x; j++)
                    GameDataInstance.Board[i].Row[j] = string.Empty;
    }

    void FillUpRandomLettersButton()
    {
        if (GUILayout.Button("Rellenar vacios al azar"))
            for (int i = 0; i < GameDataInstance.Grid.y; i++)
                for (int j = 0; j < GameDataInstance.Grid.x; j++)
                {
                    int errorCounter = Regex.Matches(GameDataInstance.Board[i].Row[j], @"[a-zA-Z]").Count;
                    char letter = RandomCharA2Z;

                    if (errorCounter == 0)
                    {
                        GameDataInstance.Board[i].Row[j] = letter.ToString().ToUpper();
                    }
                }
    }

    public char RandomCharA2Z => (char)Random.Range('a', 'z');
}
