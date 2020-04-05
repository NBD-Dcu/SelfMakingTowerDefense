using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    public float moveSpeed;
    public int hp;

    private void Update()
    {
        if (hp <= 0)
            Destroy(this.gameObject);
    }
}
