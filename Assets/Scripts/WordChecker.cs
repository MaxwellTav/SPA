using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordChecker : MonoBehaviour
{
    public GameData currentGameData;

    string _word;
    
    int 
        _assignedPoints = 0,
        _completeWords = 0;

    Ray _currentRay,
        _rayUp, _rayDown,
        _rayLeft, _rayRight,
        _rayDiagonalLeftUp, _rayDiagonalLeftDown,
        _rayDiagonalRightUp, _rayDiagonalRightDown;

    Vector3 _rayStartPosition;
    List<int> _correctSquareList = new List<int>();

    void Start()
    {
        _assignedPoints = 0;
        _completeWords = 0;
    }

    void Update()
    {
        if (_assignedPoints > 0 && Application.isEditor)
        {
            Debug.DrawRay(_rayUp.origin, _rayUp.direction * 2);
            Debug.DrawRay(_rayDown.origin, _rayDown.direction * 2);

            Debug.DrawRay(_rayLeft.origin, _rayLeft.direction * 2);
            Debug.DrawRay(_rayRight.origin, _rayRight.direction * 2);

            Debug.DrawRay(_rayDiagonalLeftUp.origin, _rayDiagonalLeftUp.direction * 2);
            Debug.DrawRay(_rayDiagonalLeftDown.origin, _rayDiagonalLeftDown.direction * 2);

            Debug.DrawRay(_rayDiagonalRightUp.origin, _rayDiagonalRightUp.direction * 2);
            Debug.DrawRay(_rayDiagonalRightDown.origin, _rayDiagonalRightDown.direction * 2);
        }
    }

    void OnEnable()
    {
        GameEvents.OnCheckSquare += SquareSelected;
        GameEvents.OnClearSelection += ClearSelection;
    }

    void OnDisable()
    {   
        GameEvents.OnCheckSquare -= SquareSelected;
        GameEvents.OnClearSelection -= ClearSelection;
    }

    void SquareSelected(string letter, Vector3 squarePosition, int squareIndex)
    {
        if (_assignedPoints == 0)
        {
            _rayStartPosition = squarePosition;
            _correctSquareList.Add(squareIndex);
            _word += letter;

            _rayUp = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(0f, 1f));
            _rayDown = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(0f, -1f));

            _rayLeft = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(-1f, 0f));
            _rayRight = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(1f, 0f));

            _rayDiagonalLeftUp = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(-1f, 1f));
            _rayDiagonalLeftDown = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(-1f, -1f));

            _rayDiagonalRightUp = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(1f, 1f));
            _rayDiagonalRightDown = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(1f, -1f));
        }
        else if (_assignedPoints == 1)
        {
            _correctSquareList.Add(squareIndex);
            _currentRay = SelectRay(_rayStartPosition, squarePosition);

            GameEvents.SelectSquareMethod(squarePosition);
            _word += letter;
            CheckWord();
        }
        else
        {
            if (IsPointOnTheRay(_currentRay, squarePosition))
            {
                _correctSquareList.Add(squareIndex);
                GameEvents.SelectSquareMethod(squarePosition);
                _word += letter;
                CheckWord();
            }
        }
        _assignedPoints++;
    }

    void CheckWord()
    {
        foreach (var searchingWord in currentGameData.selectedBoardData.SearchWords)
        {
            if (_word == searchingWord.Word)
            {
                GameEvents.CorrectWordMethod(_word, _correctSquareList);
                _word = string.Empty;

                _correctSquareList.Clear();
                return;
            }
        }
    }

    bool IsPointOnTheRay(Ray currentRay, Vector3 point)
    {
        RaycastHit[] hits = Physics.RaycastAll(currentRay, 2000f);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.position == point)
                return true;
        }
        return false;
    }

    Ray SelectRay(Vector2 firstPosition, Vector2 secondPosition)
    {
        Vector2 direction = (secondPosition - firstPosition).normalized;
        float tolerance = 0.01f;

        //Arriba
        if (Math.Abs(direction.x) < tolerance && Math.Abs(direction.y - 1f) < tolerance)
        {
            return _rayUp;
        }

        //Abajo
        if (Math.Abs(direction.x) < tolerance && Math.Abs(direction.y - (-1f)) < tolerance)
        {
            return _rayDown;
        }

        //Izquierda
        if (Math.Abs(direction.x - (-1f)) < tolerance && Math.Abs(direction.y) < tolerance)
        {
            return _rayLeft;
        }

        //Derecha
        if (Math.Abs(direction.x -1f) < tolerance && Math.Abs(direction.y) < tolerance)
        {
            return _rayRight;
        }

        //Diagonal izquierda ariba.
        if (direction.x < 0f && direction.y > 0f)
        {
            return _rayDiagonalLeftUp;
        }

        //Diagonal izquierda abajo
        if (direction.x < 0f && direction.y < 0f)
        {
            return _rayDiagonalLeftDown;
        }

        //Diagonal derecha arriba
        if (direction.x > 0f && direction.y > 0f)
        {
            return _rayDiagonalRightUp;
        }

        //Diagonal derecha abajo
        if (direction.x > 0f && direction.y < 0f)
        {
            return _rayDiagonalRightDown;
        }

        return _rayDown;
    }

    void ClearSelection()
    {
        _assignedPoints = 0;
        _correctSquareList.Clear();
        _word = string.Empty;
    }
}
