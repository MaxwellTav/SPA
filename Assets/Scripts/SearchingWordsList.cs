using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchingWordsList : MonoBehaviour
{
    [SerializeField] GameData currentGameData;
    [SerializeField] GameObject searchingWordPrefab;
    [SerializeField] float offset = 0.0f;
    [SerializeField] int maxColumns = 5;
    [SerializeField] int maxRows = 4;

    int _columns = 2,
        _rows,
        _wordsNumber;

    List<GameObject> _words = new List<GameObject>();

    void Start()
    {
        _wordsNumber = currentGameData.selectedBoardData.SearchWords.Count;

        if (_wordsNumber < _columns)
            _rows = 1;
        else
            CalculateColumnsAndRowsNumbers();

        CreateWordsObjects();
        SetWordsPositions();
    }

    void CalculateColumnsAndRowsNumbers()
    {
        do
        {
            _columns++;
            _rows = _wordsNumber / _columns;
        } while (_rows >= maxRows);

        if (_columns > maxColumns)
        {
            _columns = maxColumns;
            _rows = _wordsNumber / _columns;
        }
    }

    bool TryIncreaseColumnNumber()
    {
        _columns++;
        _rows = _wordsNumber / _columns;

        if (_columns > maxColumns)
        {
            _columns = maxColumns;
            _rows = _wordsNumber / _columns;

            return false;
        }

        if (_wordsNumber % _columns > 0)
            _rows++;

        return true;
    }

    void CreateWordsObjects()
    {
        Vector3 squareScale = GetSquareScale(new Vector3(1f, 1f, 0.1f));

        for (int index = 0; index < _wordsNumber; index++)
        {
            _words.Add(Instantiate(searchingWordPrefab));
            _words[index].transform.SetParent(transform);
            _words[index].GetComponent<RectTransform>().localScale = squareScale;
            _words[index].GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
            _words[index].GetComponent<SearchingWord>().SetWord(currentGameData.selectedBoardData.SearchWords[index].Word);
        }
    }

    Vector3 GetSquareScale(Vector3 defaultScale)
    {
        Vector3 finalScale = defaultScale;
        float adjustment = 0.01f;

        while (ShouldScaleDown(finalScale))
        {
            finalScale.x -= adjustment;
            finalScale.y -= adjustment;

            if (finalScale.x <= 0 || finalScale.y <= 0)
            {
                finalScale = new Vector3(adjustment, adjustment, 0.01f);
                return finalScale;
            }
        }

        return finalScale;
    }

    bool ShouldScaleDown(Vector3 targetScale)
    {
        RectTransform squareRect = searchingWordPrefab.GetComponent<RectTransform>();
        RectTransform parentRect = GetComponent<RectTransform>();

        Vector3 squareSize = new Vector2(
            squareRect.rect.width * targetScale.x + offset,
            squareRect.rect.height * targetScale.y + offset);

        float totalSquareHeight = squareSize.y * _rows;

        //Ajustar tamaño.
        if (totalSquareHeight > parentRect.rect.height)
        {
            while (totalSquareHeight > parentRect.rect.height)
            {
                if (TryIncreaseColumnNumber())
                    totalSquareHeight = squareSize.y * _rows;
                else
                    return true;
            }
        }

        float totalSquareWidth = squareSize.x * _columns;
        if (totalSquareWidth > parentRect.rect.width)
            return true;
        else            
            return false;
        
    }

    void SetWordsPositions()
    {
        RectTransform squareRect = _words[0].GetComponent<RectTransform>();
        Vector2 wordOffset = new Vector2()
        {
            x = squareRect.rect.width * squareRect.transform.localScale.x + offset,
            y = squareRect.rect.height * squareRect.transform.localScale.y + offset
        };

        int 
            columnNumber = 0,
            rowNumber = 0;

        Vector2 startPosition = GetFirstSquarePosition();

        foreach (var word in _words)
        {
            if (columnNumber + 1 > _columns)
            {
                columnNumber = 0;
                rowNumber++;
            }

            float 
                positionX = startPosition.x + wordOffset.x * columnNumber,
                positionY = startPosition.y - wordOffset.x * rowNumber;

            word.GetComponent<RectTransform>().localPosition = new Vector2(positionX, positionY);
            columnNumber++;
        }
    }

    Vector2 GetFirstSquarePosition()
    {
        Vector2
            startPosition = new Vector2(0f, transform.position.y),
            squareSize = new Vector2(0f ,0f);
        
        RectTransform
            squareRect = _words[0].GetComponent<RectTransform>(),
            parentRect = GetComponent<RectTransform>();

        squareSize = new Vector2(
            squareRect.rect.width * squareRect.transform.localScale.x + offset,
            squareRect.rect.height * squareRect.transform.localScale.y + offset);

        //Centrar.
        float shiftBy = (parentRect.rect.width - (squareSize.x * _columns)) / 2;

        startPosition.x = ((parentRect.rect.width - squareSize.x) / 2) * (-1);
        startPosition.x += shiftBy;
        startPosition.y = (parentRect.rect.height - squareSize.y) / 2;

        return startPosition;
    }
}
