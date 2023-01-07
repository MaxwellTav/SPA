using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AlphabetData", menuName = "RMContamax/New Alphabet Data")]
public class AlphabetData : ScriptableObject
{
    public List<LetterData> AlphabetPlain = new List<LetterData>();
    public List<LetterData> AlphabetNormal = new List<LetterData>();
    public List<LetterData> AlphabetHighlighted = new List<LetterData>();
    public List<LetterData> AlphabetWrong = new List<LetterData>();

    [Serializable]
    public class LetterData
    {
        public string letter;
        public Sprite image;
    }
}
