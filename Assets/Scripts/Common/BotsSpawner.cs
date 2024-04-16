using UnityEngine;


public class BotSpawner : MonoBehaviour
{
    [SerializeField] private CarPool _carPool;
    [SerializeField] private SpawnPoints _spawnPoints;


    public void Spawn()
    {
        int amount = _spawnPoints.CountFreePositions;

        for(int i = 0; i < amount; i++)
        {
            _carPool.SpawnRandom();
        }
    }
}