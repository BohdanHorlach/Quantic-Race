using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void Loadlevel(int temp)
    {
        SceneManager.LoadScene(temp);
    }
}
