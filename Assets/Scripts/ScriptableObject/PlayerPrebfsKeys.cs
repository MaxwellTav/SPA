using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerPrefsKeyLibrary", menuName = "RMContamax/New Key Library")]
public class PlayerPrebfsKeys : ScriptableObject
{
    //Añadir las Keys mientras más se vayan agregando.
    [Tooltip("\nKeys:\n" +
        "0 = Puntuación máxima (HigherScore)\n" +
        "1 = Semilla de generación (SavedSeed)")]
    public PPKey[] Library;
}

[Serializable]
public class PPKey
{
    public string Name;

    [TextArea(7, 7)] [Tooltip("Descripción rápida de lo qué hace esta clave")]
    public string Description;
}
