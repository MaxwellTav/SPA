using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoardData", menuName = "RMContamax/New Board Data")]
public class BoardData : ScriptableObject
{
    [Serializable]
    public class SearchingWord
    {
        public string Word;
    }

    [Serializable]
    public class BoardRow
    {
        public int Size;
        public string[] Row;

        public BoardRow()
        {
            
        }

        public BoardRow(int size)
        {
            CreateRow(size);
        }

        public void CreateRow(int size)
        {
            Size = size;
            Row = new string[Size];

            ClearRow();
        }

        public void ClearRow()
        {
            for (int i = 0; i < Row.Length; i++)
                Row[i] = string.Empty;
        }
    }

    [Tooltip("\"X\" Filas (Rows)\n" +
             "\"Y\" Columnas (Columns)")]
    public Vector2 Grid;
    
    [Space(7)]
    public float LeftTime;

    [Space(7)]
    public BoardRow[] Board;
    public List<SearchingWord> SearchWords = new List<SearchingWord>();

    public void ClearWithEmptyString()
    {
        for (int i = 0; i < Grid.y; i++)
        {
            Board[i].ClearRow();
        }
    }

    public void CreateNewBoard()
    {
        Board = new BoardRow[(int)Grid.y];

        for (int i = 0; i < (int)Grid.y; i++)
        {
            Board[i] = new BoardRow((int)Grid.x);
        }
    }
}
