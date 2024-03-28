using UnityEngine;
using System.Collections;


public abstract class Abilities : MonoBehaviour
{
	[SerializeField] private InputOfCarMovement _input;
	[SerializeField] private float _�ooldown;
	[SerializeField] private int _maxCharge;

	private float _timeOfLastUsed = 0;
	private int _currentCharge = 1;


    private void Awake()
    {
		_input.UseAbility += Use;
    }


    protected abstract void Activate();
	

	private IEnumerator StartTimer()
	{
		_timeOfLastUsed = 0f;

		while(_timeOfLastUsed < _�ooldown)
		{
			_timeOfLastUsed += Time.deltaTime;
			yield return null;
		}
	}


	private void Use()
	{
		if(_timeOfLastUsed > _�ooldown && _currentCharge > 0)
		{
			Activate();
			StartCoroutine("StartTimer");
		}
	}
}