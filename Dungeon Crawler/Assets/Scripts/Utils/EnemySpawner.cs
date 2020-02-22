using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject player;
    public Transform location;
    public GameObject enemyPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(enemyPrefab);
        newEnemy.GetComponent<EnemyController>().player = player;
        newEnemy.transform.position = location.transform.position;
        newEnemy.transform.rotation = location.transform.rotation;
    }
}
