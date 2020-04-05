using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{


    TowerStat _TowerStat; // 타워의 정보를 저장하는 스크립트를 담는변수
    CircleCollider2D attackRangeCircle; // 이 객체가 가지는 서클 콜라이더를 담는변수
    List<GameObject> targets; // 공격범위 콜라이더에 들어오는 타겟들을 저장하는 변수
    GameObject currentTarget; // 현재 공격을 하는 타겟을 저장하는 변수
    public GameObject currentProjectile; // 투사체 리소스를 담아두는 변수
    
    float isAttack;

    void Start()
    {
        //각종 변수에 객체들을 생성
        _TowerStat = GetComponent<TowerStat>();
        attackRangeCircle = GetComponent<CircleCollider2D>();
        targets = new List<GameObject>();
        //_TowerStat의 정보에 따라 타워 능력치 설정
        isAttack = _TowerStat.attackSpeed;
        attackRangeCircle.radius = _TowerStat.attackRange;
       
    }
    
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        isAttack = isAttack + Time.deltaTime;//공격탄환 장전
        InstantiateProjectile();
        Rotation();
    }

    void InstantiateProjectile()
    {
        Projectile _Projectile;
        ProjectileStat _ProjectileStat;
        if (currentTarget != null)
        {
            if (isAttack >= 1/_TowerStat.attackSpeed)
            {
                _Projectile = Instantiate(currentProjectile, transform.position, Quaternion.identity).GetComponent<Projectile>();
                _Projectile.currentTarget = currentTarget;

                Vector3 dir = currentTarget.transform.position - transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                _Projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                
                _ProjectileStat = _Projectile.GetComponent<ProjectileStat>();
                _ProjectileStat.damage = _TowerStat.damage;
                isAttack = 0;
            }
        }
    }

    void Rotation()
    {
        if (currentTarget != null)
        {
            Vector3 dir = currentTarget.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            transform.Rotate(new Vector3(0,0,-90));//스프라이트의 방향에 따라 정해져야함
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(8))//공격범위에 들어온 객체가 8번 레이어(Enemy)일 경우
        {
            targets.Add(collision.gameObject);//공격범위에 들어올 경우 해당 객체를 공격 후보에 저장
            if (currentTarget == null) // 현재 가지고있는 타겟이 없고, 공격사정거리 범위 안에 타겟이 들어올 경우 해당 타겟을 현재 타겟으로 설정함
            {
                currentTarget = collision.gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(8))
        {
            for (int i = 0; i < targets.Count; i++)
            { //공격범위 콜라이더 밖으로 나가는 객체가 생길경우 해당 객체를 타겟리스트에서 삭제
                if (targets[i].Equals(collision.gameObject))
                    targets.RemoveAt(i);
            }
            if (currentTarget.Equals(collision.gameObject)) //만약 해당 객체가 현재타겟으로 지정한 객체와 같으면 현재타겟도 삭제
            {
                currentTarget = null;
                if (targets.Count != 0)//이후 타겟리스트에 객체가 남아있으면 공격 대상으로 설정한다. 기준은 제일 먼저 타겟목록에 들어온 객체
                {
                    currentTarget = targets[0];
                }
            }
        }
    }

}
