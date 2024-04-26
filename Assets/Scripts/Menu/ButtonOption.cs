using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// TODO
//
// rename later:
//                  PlayGame -> PlaySinglePlayer
//                  




//
// menu -> Single player TrackSelect -> 
//      -> Multiplayer TrackSelect -> 
//

public class ButtonOptions : MonoBehaviour
{
    // sceneName Track 
    //const public List<string> sceneName = new List<string> { "SampleScene", "SampleScene1", "SampleScene2" };
    //public string sceneName = "SampleScene";
    // sceneName TrackSelect
    //public string sceneTrackSelect = "TrackSelect";
    // sceneName Menu
    //public string sceneMenu = "Menu";


    public void PlayGame()
    {
        SceneManager.LoadScene("TrackSelect");
    }

    //public void TrackSelect()
    //{
    //    SceneManager.LoadScene("TrackSelect");
    //}

    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void PlayMultiplayer()
    {
        SceneManager.LoadScene("Connect");
    }

    ///////////////////////////////
    public void Track1()
    {
        SceneManager.LoadScene("SampleScene1");
    }

    public void Track2()
    {
        SceneManager.LoadScene("TestScene");
    }

    public void Track3()
    {
        SceneManager.LoadScene("SampleScene3");
    }

    public void ExitButton()
    {
        #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

}
