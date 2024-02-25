using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject objectToSpawn;
    [SerializeField] int numToSpawn;
    [SerializeField] int timeBetweenSpawns;
    [SerializeField] Transform[] spawnPos;

    int spawnCount;
    bool isSpawining;
    bool startSpawning;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.updateGameGoal(numToSpawn);
    }

    // Update is called once per frame
    void Update()
    {
        if (startSpawning && spawnCount < numToSpawn && !isSpawining)
        {
            StartCoroutine(spawn());
        }
    }

    IEnumerator spawn()
    {
        isSpawining = true;

        int arrPos = Random.Range(0, spawnPos.Length);

        Instantiate(objectToSpawn,
                    spawnPos[arrPos].
                    transform.
                    position,
                    objectToSpawn.
                    transform.
                    rotation);

        spawnCount++;
        yield return new WaitForSeconds(timeBetweenSpawns);
        isSpawining = false;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startSpawning = true;
        }
    }
}
