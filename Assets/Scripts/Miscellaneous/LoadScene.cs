using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string SceneToLoad;

    [Space(15)]
    public float Delay;

    [Header("Config")]
    [SerializeField] bool OnStart;
    [SerializeField] bool OnTime;

    void Start()
    {
        enabled = false;

        if (OnStart)
            SceneManager.LoadScene(SceneToLoad);

        if (OnTime)
            StartCoroutine(delayedEvent());
    }

    public void ChangeScene(string _sceneToLoad)
    { SceneManager.LoadScene(_sceneToLoad); }

    IEnumerator delayedEvent()
    {
        yield return new WaitForSeconds(Delay);
        SceneManager.LoadScene(SceneToLoad);
    }
}
