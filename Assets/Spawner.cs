using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public bool on = false;
    public FallingBlock fallingBlockPrefab;
    public float spawnHeight = 5;
    public float spawnRate = 5;
    public bool canSpawn = true;


	// Use this for initialization
	void Start () {
        StartCoroutine("SpawnRoutine");
	}

    private IEnumerator SpawnRoutine() {
        for (; ; ) {
            if (on)
            {
                SpawnBlock();
            }
            yield return new WaitForSeconds(spawnRate);
        }

    }

    private void SpawnBlock()
    {

        MemorySpace[] memorySpaces = GameObject.FindObjectsOfType<MemorySpace>();
        List<MemorySpace> availableSpaces = new List<MemorySpace>();
        List<MemorySpace> specialSpaces = new List<MemorySpace>();


        for (int i = 0; i < memorySpaces.Length; i++)
        {
            if (!memorySpaces[i].occupied && !memorySpaces[i].locked && !memorySpaces[i].doNotSpawn) availableSpaces.Add(memorySpaces[i]);
            if (!memorySpaces[i].occupied && !memorySpaces[i].locked && memorySpaces[i].doNotSpawn) specialSpaces.Add(memorySpaces[i]);
        }

        if (availableSpaces.Count < 1) availableSpaces = specialSpaces;

        canSpawn = true;
        if (availableSpaces.Count < 1 ) {
            on = false;
            canSpawn = false;
            return;
        }
        int chosenMemorySpace = UnityEngine.Random.Range(0, availableSpaces.Count);
        MemorySpace memSpace = availableSpaces[chosenMemorySpace];

        memSpace.Lock();
        FallingBlock fallingBlock = Instantiate(fallingBlockPrefab, memSpace.transform.position + spawnHeight * Vector3.up, Quaternion.identity);
        fallingBlock.setValue(UnityEngine.Random.Range(1, 8));
        fallingBlock.setTargetHeight(memSpace.transform.position.y);
    }


    // Update is called once per frame
    void Update () {
		
	}
}
