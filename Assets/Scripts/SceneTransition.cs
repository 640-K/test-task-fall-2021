using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public GameObject loadingScreen;
    public string targetScene;
    public Slider progressBar;

    AsyncOperation loadSceneOperation;
    public void Transition()
    {
        loadingScreen.SetActive(true);
        SceneManager.LoadScene(targetScene);
        loadSceneOperation = SceneManager.LoadSceneAsync(targetScene);

        StartCoroutine(SceneProgress(loadSceneOperation));
    }


    IEnumerator SceneProgress(AsyncOperation operation)
    {
        while (operation.isDone)
        {
            progressBar.value = operation.progress / 0.9f;
            yield return null;
        }

        operation.allowSceneActivation = true;
    }
}
