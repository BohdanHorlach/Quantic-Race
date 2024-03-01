using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{


    // Визначаємо ім'я сцени, яку ми хочемо запустити
    public string sceneName = "SampleScene";

    // Викликається при натисканні кнопки
    public void PlayButtonClicked()
    {
        // Запускаємо гру, переходячи до визначеної сцени
        SceneManager.LoadScene(sceneName);
    }
}
