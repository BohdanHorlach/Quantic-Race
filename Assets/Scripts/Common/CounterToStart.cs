using System;
using System.Collections;
using TMPro;
using UnityEngine;


public class CounterToStart : MonoBehaviour
{
    [SerializeField] private int coutndownTimeSeconds = 3;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private Canvas countdownCanvas;

    private string goText = "Go!!!";

    public event Action OnCounterEnd;


    private IEnumerator StartCountdown()
    {
        countdownCanvas.gameObject.SetActive(true);
        while (coutndownTimeSeconds > 0)
        {
            Debug.Log("Countdown: " + coutndownTimeSeconds);
            countdownText.text = coutndownTimeSeconds.ToString();
            yield return new WaitForSeconds(1);
            coutndownTimeSeconds--;
        }
        countdownText.text = goText;
        OnCounterEnd?.Invoke();
        
        yield return new WaitForSeconds(1);
        Debug.Log("Go!");
        countdownCanvas.gameObject.SetActive(false);
    }


    public void StartCounter()
    {
        StartCoroutine("StartCountdown");
    }
}