using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomColor : MonoBehaviour
{
    [SerializeField] Color[] colors;
    [SerializeField] Text[] textReference;

    void Start()
    {
        if (textReference.Length > 0 && colors.Length > 0)
            foreach (var item in textReference)
                item.color = colors[Random.Range(0, colors.Length)];
    }
}
