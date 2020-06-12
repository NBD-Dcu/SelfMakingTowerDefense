using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject NavigationPoints;
    int navigationPointIndex = 0;
    public EnemyStat stat;
    Vector3 rightAngle = new Vector3(0, 0, 0),
        leftAngle = new Vector3(0, 0, 180),
        upAngle = new Vector3(0, 0, 90),
        downAngle = new Vector3(0, 0, 270);
    Quaternion rotation;

    private void Awake()
    {
        NavigationPoints = Map.map.navigationPoints;
        stat = gameObject.GetComponent<EnemyStat>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        EnemyMove();
        EnemyDeath();
    }

    public void EnemyMove()
    {
        Vector2 targetPosition;
        targetPosition = NavigationPoints.transform.GetChild(navigationPointIndex).position;
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, Time.deltaTime * stat.moveSpeed);

        if (transform.position.x == targetPosition.x)
        {
            if (transform.position.y == targetPosition.y)
                navigationPointIndex++;
        }
        EnemyRotation(targetPosition);
        if(transform.position.Equals(Map.map.endPosition))
        {
            Player.player.life--;
            Destroy(this.gameObject);
        }
    }

    public void EnemyRotation(Vector3 targetPosition)
    {
        Vector3 dir = targetPosition - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //transform.Rotate(new Vector3(0, 0, -90));//스프라이트의 방향에 따라 정해져야함
    }

    public void EnemyDeath()
    {
        if(stat.hp <= 0)
        {
            Player.player.point += stat.point;
            Destroy(this.gameObject);
        }
    }
}
