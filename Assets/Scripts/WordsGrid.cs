using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WordsGrid : MonoBehaviour
{
    public GameData currentGameData;
    public GameObject gridSquarePrefab;
    public AlphabetData alphabetData;

    public float squareOffset = 0.01f;
    public float topPosition = 0.01f;

    [SerializeField] List<SizeClass> SpacingSizes;
    [SerializeField] Slider timeSlider;
    public float _Time = 720f;
    [SerializeField] Text timeText;

    List<GameObject> squareList = new List<GameObject>();
    GridLayoutGroup gLayout;

    private void Start()
    {
        //Set timer
        timeSlider.maxValue = _Time;
        timeSlider.minValue = 0;
        timeSlider.wholeNumbers = true;
        
        gLayout = GetComponent<GridLayoutGroup>();

        SpawnGridSquares();
        SetSquaresPosition();
    }

    private void Update()
    {
        _Time -= Time.deltaTime;
        timeSlider.value = (int)_Time;
        timeText.text = Mathf.Round(_Time).ToString();
    }

    void SetSquaresPosition()
    {
        gLayout.constraintCount = (int)currentGameData.selectedBoardData.Grid.x;
        int cellCuantity = (int)(currentGameData.selectedBoardData.Grid.x * currentGameData.selectedBoardData.Grid.y);

        #region Spacing (Deprecated)
        try
        {
            SizeClass sX = SpacingSizes.Where(s => s.Numero == currentGameData.selectedBoardData.Grid.x).FirstOrDefault();
            SizeClass sY = SpacingSizes.Where(s => s.Numero == currentGameData.selectedBoardData.Grid.y).FirstOrDefault();

            gLayout.spacing = new Vector2(sX.Espacio, sY.Espacio);
            Debug.Log("Se definió el tamaño \"X = " + sX.Espacio + "\" & \"Y = " + sY.Espacio + "\"");
        }
        catch
        {
            Debug.Log("No se encontró el tamaño en la lista de tamaños, se definirá el tamaño cómo \"X = 0\" & \"Y = 0\"");
            gLayout.spacing = new Vector2(0, 0);
        }
        #endregion
    }

    #region Código anterior.
    //void SetSquaresPosition()
    //{
    //    Rect squareRect = squareList[0].GetComponent<SpriteRenderer>().sprite.rect;
    //    Transform squareTransform = squareList[0].GetComponent<Transform>();

    //    Vector2 offset = new Vector2
    //    {
    //        x = (squareRect.width * squareTransform.localScale.x + squareOffset) * 0.01f,
    //        y = (squareRect.height * squareTransform.localScale.y + squareOffset) * 0.01f,
    //    };

    //    //var startPosition = GetFirstSquarePosition();
    //    Vector2 cellNumber = new Vector2()
    //    {
    //        //Fila (Row)
    //        x = 0,
    //        //Columna (Column)
    //        y = 0
    //    };

    //    foreach (var square in squareList)
    //    {
    //        if (cellNumber.x + 1 > currentGameData.selectedBoardData.Grid.x)
    //        {
    //            cellNumber.y++;
    //            cellNumber.x = 0;
    //        }

    //        //var positionX = startPosition.x + offset.y + cellNumber.y;
    //        //var positionY = startPosition.y + offset.y + cellNumber.x;

    //        //square.GetComponent<Transform>().position = new Vector2(positionX, positionY);
    //        cellNumber.x++;
    //    }
    //}

    //Vector2 GetFirstSquarePosition()
    //{
    //    var startPosition = new Vector2(0f, transform.position.y);
    //    var squareRect = squareList[0].GetComponent<SpriteRenderer>().sprite.rect;
    //    var squareTransform = squareList[0].GetComponent<Transform>();
    //    var squareSize = new Vector2(0f, 0f);

    //    squareSize.x = squareRect.width * squareTransform.localScale.x;
    //    squareSize.y = squareRect.height * squareTransform.localScale.y;

    //    var midWidthPosition = (((currentGameData.selectedBoardData.Grid.y - 1) * squareSize.x) / 2) * 0.01f;
    //    var midHeightPosition = (((currentGameData.selectedBoardData.Grid.x - 1) * squareSize.y) / 2) * 0.01f;

    //    startPosition.x = (midWidthPosition != 0) ? midWidthPosition * -1 : midWidthPosition;
    //    startPosition.y += midHeightPosition;

    //    GameObject newGo = new GameObject();
    //    newGo.name = "StartPositionReferencia";
    //    newGo.transform.position = new Vector3(startPosition.x, startPosition.y, 1);

    //    return startPosition;
    //}
    #endregion

    void SpawnGridSquares()
    {
        if (currentGameData != null)
        {
            Vector3 squareScale = GetSquareScale(new Vector3(1.5f, 1.5f, 0.1f));

            foreach (var square in currentGameData.selectedBoardData.Board)
            {
                foreach (var squareLetter in square.Row)
                {
                    var normalLetterData = alphabetData.AlphabetNormal.Find(data => data.letter == squareLetter);
                    var selectedLetterData = alphabetData.AlphabetHighlighted.Find(data => data.letter == squareLetter);
                    var correctLetterData = alphabetData.AlphabetWrong.Find(data => data.letter == squareLetter);

                    if (normalLetterData.image == null || selectedLetterData.image == null)
                    {
                        Debug.Log("Todos los campos en la lista deben estar completos, presione \"Llenar al azar\" en tu \"Board Data\" para solucionar el problema - Letra: " + squareLetter);
#if UNITY_EDITOR
                        if (UnityEditor.EditorApplication.isPlaying)
                            UnityEditor.EditorApplication.isPlaying = false;
#endif
                    }
                    else
                    {
                        squareList.Add(Instantiate(gridSquarePrefab));
                        squareList[squareList.Count - 1].GetComponent<GridSquare>().SetSprite(normalLetterData, correctLetterData, selectedLetterData);
                        squareList[squareList.Count - 1].transform.SetParent(this.transform);
                        squareList[squareList.Count - 1].GetComponent<Transform>().position = new Vector3(0f, 0f, 0f);
                        squareList[squareList.Count - 1].transform.localScale = squareScale;
                    }
                }
            }
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
                finalScale.x = adjustment;
                finalScale.y = adjustment;

                return finalScale;
            }
        }

        return finalScale;
    }

    bool ShouldScaleDown(Vector3 targetScale)
    {
        Rect squareRect = gridSquarePrefab.GetComponent<SpriteRenderer>().sprite.rect;
        Vector2 squareSize = new Vector2()
        {
            x = (squareRect.width * targetScale.x) + squareOffset,
            y = (squareRect.height * targetScale.y) + squareOffset
        };

        float midWidthPosition = ((currentGameData.selectedBoardData.Grid.y * squareSize.x) / 2) * 0.01f;
        float midHeightPosition = ((currentGameData.selectedBoardData.Grid.x * squareSize.y) / 2) * 0.01f;

        Vector2 startPosition = new Vector2()
        {
            x = (midWidthPosition != 0) ? midWidthPosition * -1 : midWidthPosition,
            y = midHeightPosition
        };

        return (startPosition.x < GetHalfScreenWidth() * -1 || startPosition.y > topPosition);
    }

    float GetHalfScreenWidth()
    {
        float height = Camera.main.orthographicSize * 2;
        float width = (1.7f * height) * Screen.width / Screen.height;

        return width / 2;
    }

    [Serializable]
    public class SizeClass
    {
        public int Numero;
        public float Espacio;
        public float Tamano;
    }
}
