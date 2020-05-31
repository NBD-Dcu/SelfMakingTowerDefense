using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public TowerObjectInformation towerObjInfo;
    GameManager gm;
    CircleCollider2D attackRangeCollider;
    List<GameObject> targets;
    GameObject currentTarget;
    float attackTimer = 1;
    private void Awake()
    {
        gm = GameManager.gameManager;
        gameObject.GetComponent<SpriteRenderer>().sprite = gm.LoadImageToSprite(towerObjInfo.towerImagePath);
        targets = new List<GameObject>();
    }
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        attackTimer = attackTimer + Time.deltaTime;//공격탄환 장전
        Attack();
        Rotation();
    }
    void Attack()
    {
        if(currentTarget != null)
        {
            if (attackTimer >= 1f / towerObjInfo.attackSpeed)
            {
                Projectile projectile = Resources.Load<Projectile>(gm.projectilePrefabPath);
                Instantiate(projectile);
                projectile.currentTarget = currentTarget;
                projectile.transform.position = transform.position;
                projectile.GetComponent<SpriteRenderer>().sprite = gm.LoadImageToSprite(towerObjInfo.projectileImagePath);
                attackTimer = 0;
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

            //transform.Rotate(new Vector3(0, 0, -90));//스프라이트의 방향에 따라 정해져야함
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer.Equals(8))
        {
            targets.Add(other.gameObject);
            if(currentTarget == null)
            {
                currentTarget = other.gameObject;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer.Equals(8))
        {
            for (int i = 0; i < targets.Count; i++)
            { //공격범위 콜라이더 밖으로 나가는 객체가 생길경우 해당 객체를 타겟리스트에서 삭제
                if (targets[i].Equals(other.gameObject))
                    targets.RemoveAt(i);
            }
            if (currentTarget.Equals(other.gameObject)) //만약 해당 객체가 현재타겟으로 지정한 객체와 같으면 현재타겟도 삭제
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
