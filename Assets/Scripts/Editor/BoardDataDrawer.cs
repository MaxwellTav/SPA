using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(BoardData), false)]
[CanEditMultipleObjects]
[Serializable]
public class BoardDataDrawer : Editor
{
    BoardData GameDataInstance => target as BoardData;
    ReorderableList _dataList;

    private void OnEnable()
    {
        
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        //Dibujar el GRID
        DrawColumnsRowsInputFields();
        EditorGUILayout.Space();

        if (GameDataInstance.Board != null && GameDataInstance.Grid.x > 0 && GameDataInstance.Grid.y > 0)
            DrawBoardTable();

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
            fixedWidth = 50
        };

        var rowStyle = new GUIStyle()
        {
            fixedHeight = 25,
            fixedWidth = 40,
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
}
