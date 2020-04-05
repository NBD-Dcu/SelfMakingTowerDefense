using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public PointAccessor _PointAccessor;
    public EnemyStat _stat;
    int checkPointIndex = 0;

    Vector2 startPostion, endPosition;

    Vector3 rightAngle = new Vector3(0,0,0), 
        leftAngle = new Vector3(0, 0, 180), 
        upAngle = new Vector3(0, 0, 90), 
        downAngle = new Vector3(0, 0, 270);
    Quaternion rotation;// 미리 선언한 쿼터니언
    
    void Start()
    {
        _PointAccessor = GameObject.Find("Points").GetComponent<PointAccessor>();
        startPostion = _PointAccessor.startPoint.transform.position;
        endPosition = _PointAccessor.endPoint.transform.position;
        _stat = GetComponent<EnemyStat>();
    }

    
    void Update()
    {
        EnemyMove();
    }

    void EnemyMove()
    {
        Vector2 targetPosition;
        if (checkPointIndex < _PointAccessor.checkPoints.Length)
        {
            targetPosition= _PointAccessor.checkPoints[checkPointIndex].transform.position;

            EnemyRotation(targetPosition);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, Time.deltaTime * _stat.moveSpeed);

            if (transform.position.x == targetPosition.x)
            {
                if (transform.position.y == targetPosition.y)
                    checkPointIndex++;
            }
        }
        else if (checkPointIndex >= _PointAccessor.checkPoints.Length) {
            targetPosition = endPosition;

            EnemyRotation(targetPosition);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, Time.deltaTime * _stat.moveSpeed);
        }
    }
    void EnemyRemove()
    {

    }
    void EnemyRotation(Vector2 targetPosition)
    {
        //우측
        if (transform.position.x < targetPosition.x && transform.position.y == targetPosition.y)
        {
            rotation.eulerAngles = rightAngle;
            transform.rotation = rotation;
        }
        //좌측
        if (transform.position.x > targetPosition.x && transform.position.y == targetPosition.y)
        {
            rotation.eulerAngles = leftAngle;
            transform.rotation = rotation;
        }
        //아래측
        if (transform.position.x == targetPosition.x && transform.position.y > targetPosition.y)
        {
            rotation.eulerAngles = downAngle;
            transform.rotation = rotation;
        }
        //위측
        if (transform.position.x == targetPosition.x && transform.position.y < targetPosition.y)
        {
            rotation.eulerAngles = upAngle;
            transform.rotation = rotation;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EndPoint"))
        {
            Destroy(this.gameObject);
        }
    }
}
