using UnityEngine;

public class ExitButton : MonoBehaviour
{
    // Викликається при натисканні кнопки "Exit"
    public void ExitButtonClicked()
    {
        // Завершення гри або виходить з редактора Unity у режимі розробки
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}