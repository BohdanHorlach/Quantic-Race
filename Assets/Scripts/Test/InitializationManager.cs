using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializationManager : MonoBehaviour
{

    [SerializeField] private GameObject skidMarksController;
    //private void Awake()
    //{
    //    StartCoroutine(InitializeScripts());
    //}

    //private IEnumerator InitializeScripts()
    //{

    //    // Wait for the first script to initialize
    //    //yield return StartCoroutine(InitializeAndWaitFor(skidMarksController.GetComponent<Skidmarks>().Initialize()));

    //    //// Initialize any other scripts
    //    Debug.Log("All scripts initialized");
    //}

    private IEnumerator InitializeAndWaitFor(IEnumerator coroutine)
    {
        yield return StartCoroutine(coroutine);
    }
}