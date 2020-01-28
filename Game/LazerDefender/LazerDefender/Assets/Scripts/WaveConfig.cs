using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Wave Config")]
public class WaveConfig : ScriptableObject // Scriptable Objects are used as a data container.
{
    //All instances of enemies will have these characteristics
    [SerializeField] GameObject enemyPrefab; //This is our enemy ship
    [SerializeField] GameObject pathPrefab; //This is our enemy path we created with multiple transforms.
    [SerializeField] float timeBetweenSpawns = 0.5f;
    [SerializeField] float spawnRandomFactor = 0.3f;
    [SerializeField] int numberOfEnemies = 5;
    [SerializeField] float moveSpeed = 2f;

    public GameObject GetEnemyPrefab() //Other scripts can call this
    {
        return enemyPrefab;
    }

    public List<Transform> GetWayPoints() //The point of this is so we don't have to drag the individual way points to our script and connect them in that way. We are passing in a GameObject that has all the points then pushing it through another list that gets called from the enemy.
    {
        var waveWayPoints = new List<Transform>(); //We are creating a empty list which we will pass into the enemy

        foreach(Transform child in pathPrefab.transform) //The foreach knows that it has children and will grab each child's transform.
        {
            waveWayPoints.Add(child); //Copy the list into our temp List
        }

        return waveWayPoints; //We are returning a list of all the way points that have been found.
    }

    public float GetTimeBetweenSpawns()
    {
        return timeBetweenSpawns;
    }

    public float GetSpawnRandomFactor()
    {
        return spawnRandomFactor;
    }

    public int GetNumberOfEnemies()
    {
        return numberOfEnemies;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
}
