using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs; //This is our container for all our waves which is our scriptable object
    [SerializeField] int startingWave = 0;
    [SerializeField] bool isLooping = false;

    // Start is called before the first frame update and since its a Coroutine it will have a yield
    IEnumerator Start()
    {
        do //This is mainly here so if I want to repeat waves this is how I would do it
        {
            yield return StartCoroutine(SpawnAllWaves()); //Don't leave this statement till this method has finished then loop. 
        }
        while (isLooping); //By having a do while loop we are able to repeat the spawning once all the other ships have been spawned
    }

    private IEnumerator SpawnAllWaves() //This function will loop through all our waves
    {
        for (int waveIndex = startingWave; waveIndex < waveConfigs.Count; waveIndex++) //This checks and goes over all the waves we have.
        {
            var currentWave = waveConfigs[waveIndex]; //We are putting our paths waves in a list
            yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave)); //We are passing in the first wave. The Yield pauses the loop till the method has finished.
        }
    }

    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig) //Call our first path
    {
        for (int enemyCount = 0; enemyCount < waveConfig.GetNumberOfEnemies(); enemyCount++) //On our scriptable object waveConfig we have set how many enemies we want to spawn.
        {
            var newEnemy = Instantiate(waveConfig.GetEnemyPrefab(), waveConfig.GetWayPoints()[0].transform.position, Quaternion.identity); //We are passing in the first wave and placing it in our starting point then keeping the rotation. Creating an instance just creats the OBJECT.
            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig); //We need to get access to the class so we have to grab the class component and then we call the function     
            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
