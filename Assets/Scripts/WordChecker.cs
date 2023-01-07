using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordChecker : MonoBehaviour
{
    public GameData currentGameData;

    string _word;

    void OnEnable()
    {
        GameEvents.OnCheckSquare += SquareSelected;
    }

    void OnDisable()
    {   
        GameEvents.OnCheckSquare -= SquareSelected;
    }

    void SquareSelected(string letter, Vector3 squarePosition, int squareIndex)
    {
        GameEvents.SelectSquareMethod(squarePosition);
        _word += letter;
        CheckWord();
    }

    void CheckWord()
    {
        foreach (var searchingWord in currentGameData.selectedBoardData.SearchWords)
        {
            if (_word == searchingWord.Word)
            {
                _word = string.Empty;
                return;
            }
        }
    }
}
