using UnityEngine;

public class ExitButton : MonoBehaviour
{
    // ����������� ��� ��������� ������ "Exit"
    public void ExitButtonClicked()
    {
        // ���������� ��� ��� �������� � ��������� Unity � ����� ��������
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}