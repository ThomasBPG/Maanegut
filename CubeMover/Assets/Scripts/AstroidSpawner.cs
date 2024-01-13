using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroidSpawner : MonoBehaviour
{
    public GameObject astroid;

    public Transform Player;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnAstroids());
    }

    IEnumerator SpawnAstroids()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5, 7));
            Vector3 spawnPoint = Player.position;
            spawnPoint += Vector3.up*6.5f + Vector3.right * Random.Range(-9f, 9f);
            GameObject temp = Instantiate(astroid,spawnPoint,Quaternion.identity);
        }
        
    }
}
