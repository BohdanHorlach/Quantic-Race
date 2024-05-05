using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigator : MonoBehaviour
{
    public const string LOADING_SCENE_NAME = "Loading";

    public const string CAR_SELECTION_MENU = "CarSelectionMenu";
    public const string TRACK_SELECTION_MENU = "TrackSelectionMenu";
    public const string TRACK1_SINGLE_PLAYER = "Track1Singleplayer";
    public const string MULTIPLAYER = "Connect";
    public const string GAME_CONTROL_SCENE = "GameControlScene";

    private void LoadSceneWithLoadingScreen(string sceneName)
    {
        LoadingSceneManager.SetSceneToLoad(sceneName);
        SceneManager.LoadScene(LOADING_SCENE_NAME);
    }

    private void LoadSceneWithoutLoadingScreen(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void CarSelectionMenu()
    {
        LoadSceneWithLoadingScreen(CAR_SELECTION_MENU);
    }

    public void Singleplayer()
    {
        UserDataManager.selectedGameOptionsSO.isSinglePlayer = true;
        LoadSceneWithLoadingScreen(TRACK_SELECTION_MENU);
    }

    public void Multiplayer()
    {
        UserDataManager.selectedGameOptionsSO.isSinglePlayer = false;
        LoadSceneWithoutLoadingScreen(MULTIPLAYER);
    }

    public void Track1()
    {
        UserDataManager.selectedGameOptionsSO.selectedTrackIndex = 0;

        LoadSceneWithLoadingScreen(GAME_CONTROL_SCENE);
    }

    public void StartRace()
    {
        if (UserDataManager.selectedGameOptionsSO.isSinglePlayer)
        {
            LoadSceneWithLoadingScreen(TRACK1_SINGLE_PLAYER);
        }
        else
        {
            LoadSceneWithLoadingScreen(TRACK1_SINGLE_PLAYER);
        }
    }

    public void ExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
    }

    //---------------------------------------------------------------------------
}
