using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ConstructionPoint : MonoBehaviour
{
    Player player;
    GameManager gm;
    bool isBuilt = false;
    //Sprite projectileSprite;

    void Awake()
    {
        player = Player.player;
        gm = GameManager.gameManager;
    }
    void Start()
    {
        
    }
    
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            CreateTower();
        }
    }

    void CreateTower()
    {
        if (isBuilt == false && player.currentTowerInfo !=null)
        {
            if (player.currentTowerInfo.cost <= player.goldResources)
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D[] hits = Physics2D.RaycastAll(touchPos, Camera.main.transform.forward);
                for (int i = 0; i < hits.Length; i++)
                {
                    if (this.gameObject.Equals(hits[i].collider.gameObject))
                    {
                        Tower tower = Resources.Load<Tower>(gm.towerPrefabPath);
                        tower.towerObjInfo = player.currentTowerInfo;
                        tower.transform.position = transform.position;
                        Instantiate(tower);
                        isBuilt = true;
                        player.goldResources = player.goldResources - tower.towerObjInfo.cost;
                        player.RenewalGoldResource();
                        player.currentTowerInfo = null;
                    }
                }
            }
        }
    }
}
