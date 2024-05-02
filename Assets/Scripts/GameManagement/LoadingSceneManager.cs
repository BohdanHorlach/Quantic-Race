using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    // Reference to the text UI element for displaying progress
    [SerializeField] private TextMeshProUGUI progressText;
    private static string sceneNameToLoad;

    private AsyncOperation asyncOperation;

    public static void SetSceneToLoad(string sceneName)
    {
        sceneNameToLoad = sceneName;
    }

    void Start()
    {
        asyncOperation = SceneManager.LoadSceneAsync(sceneNameToLoad);

        // Prevent automatic scene activation
        asyncOperation.allowSceneActivation = false;
    }

    void Update()
    {
        if (asyncOperation != null)
        {
            // Adjust factor for smoother progress display
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            progressText.text = "Loading... " + (int)(progress * 100) + "%";

            // Allow scene activation when progress is near completion
            if (asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
            }
        }
    }
}