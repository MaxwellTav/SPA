using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellCollisionComprobation : MonoBehaviour
{
    [SerializeField] WordsGrid wg;

    private void Start()
    {
        wg = GameObject.FindGameObjectWithTag("WordsGrid").GetComponent<WordsGrid>();
    }
}
