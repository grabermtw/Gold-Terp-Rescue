using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject obstacle;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnObstacle());
    }

    private IEnumerator SpawnObstacle()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(2, 8));
            GameObject newObstacle = Instantiate(obstacle, transform.position, Quaternion.Euler(Random.Range(0, 360),Random.Range(0, 360),Random.Range(0, 360)));
            Destroy(newObstacle, 15);
            yield return null;
        }
    }

}
