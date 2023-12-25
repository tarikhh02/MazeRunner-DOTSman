using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenControll : MonoBehaviour
{
    AsyncOperation sceneLoadingOperation;
    public Image loadingBarFill;

    void Start()
    {
        sceneLoadingOperation = SceneManager.LoadSceneAsync(2);
    }

    void Update()
    {
        float2 position = new float2
        {
            x = -(1980 - (sceneLoadingOperation.progress * 1920)),
            y = loadingBarFill.rectTransform.anchoredPosition.y,
        };
        loadingBarFill.rectTransform.anchoredPosition = position;
    }
}