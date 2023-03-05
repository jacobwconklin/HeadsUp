using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingStageController : MonoBehaviour
{
    // Responsible for consistently moving camera, death wall, and placing new stage chunks to keep the level moving
    // Should also delete old stage chunks after sufficient time. 
    [SerializeField] private Camera mainCamera;
    [SerializeField] private List<GameObject> mapChunks = new List<GameObject>();
    [SerializeField] private GameObject deathWall;
    [SerializeField] private Vector3 moveDirection = new Vector3(98.4f, 12f, 0);
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float initialDelay = 4.5f; // Give delay for game to begin
    private float beginTime;
    private Vector3 cameraMoveTo;
    private Vector3 deathWallMoveTo;
    private List<GameObject> spawnedChunks = new List<GameObject>();
    private float nextSpawnpointX;

    // Start is called before the first frame update
    void Start()
    {
        beginTime = Time.time + initialDelay;
        nextSpawnpointX = mainCamera.transform.position.x + 1; // Spawn second chunk right after camera starts moving
        // Instantiate first extra map chunk
        // The very first chunk the players start on
        // can stay vanilla and consistent. 
        // pick randomly from list
        spawnedChunks.Add(Instantiate(mapChunks[Random.Range(0, mapChunks.Count)], new Vector3(0, 50, 0), Quaternion.identity));
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time < beginTime) return;

        cameraMoveTo = mainCamera.transform.position + (moveDirection * Time.deltaTime * moveSpeed);
        mainCamera.transform.position = cameraMoveTo;
        //mainCamera.transform.Translate(cameraMoveTo * Time.deltaTime * moveSpeed);
        deathWallMoveTo = deathWall.transform.position + (moveDirection * Time.deltaTime * moveSpeed); ;
        deathWall.transform.position = deathWallMoveTo;
        //deathWall.transform.Translate(cameraMoveTo * Time.deltaTime * moveSpeed);

        // Check for "Checkpoints" to have been passed to instantiate new chunks
        // Save chunks at the end of a list, if the list length is > 3 start deleting the
        // first and oldest chunk
        if (mainCamera.transform.position.x > nextSpawnpointX)
        {
            Debug.Log("camera passed checkpoint");
            // update nextSpawnpointX
            nextSpawnpointX += moveDirection.x;
            // Calculate new map chunk location based on last in the list of spawned chunks
            Vector3 newChunkLocation = spawnedChunks[spawnedChunks.Count - 1].transform.position;
            newChunkLocation += moveDirection;
            // Instantiate the new map chunk
            spawnedChunks.Add(Instantiate(mapChunks[Random.Range(0, mapChunks.Count)], newChunkLocation, Quaternion.identity));
            // delete oldest map chunk if more than 3 present
            if (spawnedChunks.Count > 3)
            {
                spawnedChunks.RemoveAt(0);
            }
        }

    }
}
