using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject Enemy;
    Coroutine spawn;
    int i = 1;
    // Start is called before the first frame update
    void Start()
    {
        spawn = StartCoroutine(SpawnEnemy());
    }
    IEnumerator SpawnEnemy()
    {
        while (i > 0)
        {
            yield return new WaitForSeconds(Random.Range(2, 5));
            Instantiate(Enemy, new Vector3(Random.Range(-15, 15), 0, Random.Range(-15, 15)), Quaternion.identity);
        }
    }
}
