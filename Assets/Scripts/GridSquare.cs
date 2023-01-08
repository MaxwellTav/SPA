using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSquare : MonoBehaviour
{
    public int SQUAREINDEX { get; set; }

    AlphabetData.LetterData _normalLetterData;
    AlphabetData.LetterData _selectedLetterData;
    AlphabetData.LetterData _correctLetterData;

    SpriteRenderer _displayedImage;

    bool _selected;
    bool _clicked;
    bool _correct;
    int _index = -1;

    public void SetIndex(int index) => _index = index;
    public int GetIndex() => _index;

    private void Start()
    {
        _correct = false;
        _selected = false;
        _clicked = false;

        _displayedImage = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        GameEvents.OnEnableSquareSelection += OnEnableSquareSelection;
        GameEvents.OnDisableSquareSelection += OnDisableSquareSelection;
        GameEvents.OnSelectSquare += OnSelectSquare;
        GameEvents.OnCorrectWord += CorrectWord;
    }

    private void OnDisable()
    {
        GameEvents.OnEnableSquareSelection -= OnEnableSquareSelection;
        GameEvents.OnDisableSquareSelection -= OnDisableSquareSelection;
        GameEvents.OnSelectSquare -= OnSelectSquare;
        GameEvents.OnCorrectWord -= CorrectWord;
    }

    void CorrectWord(string word, List<int> squareIndexes)
    {
        if (_selected && squareIndexes.Contains(_index))
        {
            _correct = true;
            _displayedImage.sprite = _correctLetterData.image;
        }

        _selected = false;
        _clicked = false;
    }

    public void SetSprite(
        AlphabetData.LetterData normalLetterData, 
        AlphabetData.LetterData selectedLetterData, 
        AlphabetData.LetterData correctLetterData)
    {
        _normalLetterData = normalLetterData;
        _selectedLetterData = selectedLetterData;
        _correctLetterData = correctLetterData;

        GetComponent<SpriteRenderer>().sprite = normalLetterData.image;
    }

    public void OnEnableSquareSelection()
    {
        _clicked = true;
        _selected = false;
    }
    public void OnDisableSquareSelection()
    {
        _clicked = false;
        _selected = false;

        if (_correct)
            _displayedImage.sprite = _correctLetterData.image;
        else
            _displayedImage.sprite = _normalLetterData.image;
    }

    void OnSelectSquare(Vector3 position)
    {
        if (gameObject.transform.position == position)
            _displayedImage.sprite = _selectedLetterData.image;
    }

    void OnMouseDown()
    {
        OnEnableSquareSelection();
        GameEvents.EnableSquareSelectionMethod();
        CheckSquare();
        _displayedImage.sprite = _selectedLetterData.image;
    }

    void OnMouseEnter()
    {
        CheckSquare();
    }

    void OnMouseUp()
    {
        GameEvents.ClearSelectionMethod();
        GameEvents.DisableSquareSelectionMethod();
    }

    public void CheckSquare()
    {
        if ((!_selected) && _clicked)
        {
            _selected = true;
            GameEvents.CheckSquareMethod(_normalLetterData.letter, gameObject.transform.position, _index);
        }
    }
}
