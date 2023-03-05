using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private float randomTimeLow = 1f; // Minimum seconds between spawns
    [SerializeField] private float randomTimeHigh = 6.0f; // Maximum time between spawns
    [SerializeField] private float randomSpeedLow = 7.0f; // Minimum seconds between spawns
    [SerializeField] private float randomSpeedHigh = 10.0f; // Maximum time between spawns
    [SerializeField] private float initialDelay = 5.0f; // Additional delay before first spawn
    private float nextSpawnTime = 0;
    [SerializeField] private List<GameObject> objectsToSpawn;
    [SerializeField] private GameObject startLocation;
    [SerializeField] private GameObject endLocation;


    // Start is called before the first frame update
    void Start()
    {
        // Initial delay of 5 seconds?
        nextSpawnTime = Time.time + Random.Range(randomTimeLow, randomTimeHigh) + initialDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextSpawnTime)
        {
            // SEND A CAR
            nextSpawnTime = Random.Range(randomTimeLow, randomTimeHigh) + Time.time;

            StartCoroutine(SpawnAndMove());
        }
    }

    IEnumerator SpawnAndMove()
    {
        int selectedObject = Random.Range(0, objectsToSpawn.Count);
        // Make copy of randomly selected gameObject
        GameObject newObject = Instantiate(objectsToSpawn[selectedObject]);
        // Set copy at the start location and rotation
        newObject.transform.position = startLocation.transform.position;
        newObject.transform.rotation = startLocation.transform.rotation;
        float time = 0f;
        float speed = Random.Range(randomSpeedLow, randomSpeedHigh);

        while (newObject.transform.position != endLocation.transform.position)
        {
            newObject.transform.position = Vector3.Lerp(startLocation.transform.position, endLocation.transform.position, 
                (time / Vector3.Distance(startLocation.transform.position, endLocation.transform.position)) * speed);
            time += Time.deltaTime;
            yield return null;
        }
        // Done with copy destroy it
        Destroy(newObject);
    }
}
