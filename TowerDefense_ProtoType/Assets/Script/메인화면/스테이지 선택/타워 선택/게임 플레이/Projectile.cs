using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject currentTarget;
    GameManager gm;
    Vector3 lastTargetPosition;
    public float moveSpeed = 10;
    public int attackDamage = 0;

    private void Awake()
    {
        
    }

    void Start()
    {

    }

    void Update()
    {
        Move();
        Rotation();
    }
    void Move()//이동,타격을 관장
    {
        if (currentTarget != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, currentTarget.transform.position, Time.deltaTime * moveSpeed);//타겟의 현재 지점으로 이동
            lastTargetPosition = currentTarget.transform.position;//타겟의 현재 지점을 기록

        }
        else if (currentTarget == null)
        {
            transform.position = Vector2.MoveTowards(transform.position, lastTargetPosition, Time.deltaTime * moveSpeed);//타겟의 마지막 지점으로 이동
            if (transform.position.Equals(lastTargetPosition))//타겟의 마지막 지점에 도착했을 경우 이 객체를 파괴
                Destroy(this.gameObject);
        }
    }
    void Rotation()//회전
    {
        if (currentTarget != null)
        {
            //타겟이 있는 방향으로 자신을 회전
            Vector3 dir = currentTarget.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            lastTargetPosition = currentTarget.transform.position;//타겟의 현재 지점을 기록
        }
        else if (currentTarget == null)
        {
            //타겟의 마지막 지점 방향으로 자신을 회전
            Vector3 dir = lastTargetPosition - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.Equals(currentTarget))
        {
            collision.gameObject.GetComponent<Enemy>().stat.hp = collision.gameObject.GetComponent<Enemy>().stat.hp - attackDamage;//투사체 데미지 따라 깎이도록 조정해야함
            Destroy(this.gameObject);
        }
    }
}
