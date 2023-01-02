using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuScript : MonoBehaviour
{
    [Header("Puntaje máximo")]
    [SerializeField] TextMeshProUGUI _Text;
    [Tooltip("El título qué irá antes de la puntuación\nEj. \"Puntuación -\" 999")]
    [SerializeField] string scoreTitle;
    public PlayerPrebfsKeys SaveLibrary;

    private void Start()
    {
        int maxScore = PlayerPrefs.GetInt(SaveLibrary.Library[0].Name, 0);
        _Text.text = scoreTitle + " " + maxScore;
    }
}
