using UnityEngine;
using System.Collections;

public class Spawner_Generic : MonoBehaviour
{

    public int maxSpawnCount = -1;
    public float timeBetweenSpawns = 1.0f;
    public Vector3 spawnOrigin = new Vector3(0, 0, 0);
    public Vector3 spawnRotation = new Vector3(0, 0, 0);
    public Vector3 spawnOffset = new Vector3(10, 10, 10);
    public GameObject spawnObject;

    private float lastSpawnTime = 0.0f;
    private Quaternion spawnQuaternion;
    private int spawnCount = 0;

    // Use this for initialization
    void Start()
    {
        spawnQuaternion = Quaternion.Euler(spawnRotation);
        lastSpawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Time.time - lastSpawnTime) > timeBetweenSpawns && (spawnCount < maxSpawnCount || spawnCount == -1))
        {
            Object newObject = Instantiate(spawnObject, randomizeVector3(spawnOrigin, spawnOffset), spawnQuaternion);
            spawnCount++;
            lastSpawnTime = Time.time;
        }
    }

    Vector3 randomizeVector3(Vector3 origin, Vector3 offset)
    {
        float x = Random.Range(-offset.x, offset.x);
        float y = Random.Range(-offset.y, offset.y);
        float z = Random.Range(-offset.z, offset.z);
        return new Vector3(origin.x + x, origin.y + y, origin.z + z);
    }

}
