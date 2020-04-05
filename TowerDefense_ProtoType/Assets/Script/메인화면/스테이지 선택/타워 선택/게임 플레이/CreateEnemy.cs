using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEnemy : MonoBehaviour
{
    public GameObject prefab;
    void Start()
    {
        StartCoroutine(InstantiateEnemy());
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator InstantiateEnemy()
    {
        while (true)
        {
            Instantiate(prefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1.0f);
        }
    }
}
