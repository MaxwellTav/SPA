using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SearchingWord : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI displayedText;
    string _word;

    [Header("Debug")]
    [SerializeField] bool Founded = false;

    void Start()
    {
        Founded = false;
    }

    void OnEnable()
    {
        GameEvents.OnCorrectWord += CorrectWord;
    }

    void OnDisable()
    {
        GameEvents.OnCorrectWord -= CorrectWord;
    }

    public void SetWord(string word)
    {
        _word = word;
        displayedText.text = word;
    }

    void CorrectWord(string word, List<int> squaresIndexes)
    {
        if (word == _word && !Founded)
        {
            string beforeText = displayedText.text;
            displayedText.text = "<s>" + beforeText;
            GetComponent<Image>().color = Color.gray;
            Founded = true;
        }
    }
}
