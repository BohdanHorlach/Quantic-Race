using UnityEngine;


public class BrakeSpace : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        SetBrake(other, true);
    }


    private void OnTriggerExit(Collider other) 
    {
        SetBrake(other, false);
    }


    private void SetBrake(Collider other, bool value)
    {
        BotsCarMovement bot;
        if (other.TryGetComponent(out bot) == true)
        {
            bot.SetBrake(value);
        }
    }
}