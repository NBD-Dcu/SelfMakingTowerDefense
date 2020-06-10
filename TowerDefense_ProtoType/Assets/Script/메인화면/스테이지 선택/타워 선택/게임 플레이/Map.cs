using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public CreateEnemyInformation[] createEnemyInfos;
    public static Map map;
    Vector2 startPosition;
    public Vector2 endPosition;
    public GameObject navigationPoints;
    public bool isLastEnemy = false;
    public int playerLife = 10;
    GameObject constructionPoints;

    private void Awake()
    {
        map = this;
        startPosition = transform.Find("StartPoint").position;
        navigationPoints = transform.Find("NavigationPoints").gameObject;
        endPosition = navigationPoints.transform.GetChild(navigationPoints.transform.childCount-1).position;
    }
    void Start()
    {
        StartCoroutine("CreateEnemy");
    }

    
    void Update()
    {
        
    }

    IEnumerator CreateEnemy()
    {
        for (int i=0; i<createEnemyInfos.Length; i++)
        {
            yield return new WaitForSeconds(createEnemyInfos[i].startTime);
            for (int j = 0; j < createEnemyInfos[i].NumberOfCreation; j++)
            {
                Enemy _enemy = Instantiate(createEnemyInfos[i].enemy);
                _enemy.transform.position = startPosition;
                yield return new WaitForSeconds(createEnemyInfos[i].CreationCycle);
            }
        }
        isLastEnemy = true;
    }
}
