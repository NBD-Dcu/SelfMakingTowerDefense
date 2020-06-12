using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Player : MonoBehaviour
{
    public static Player player;
    GameManager gm;
    public Button[] towerSocket;
    public TowerObjectInformation currentTowerInfo;
    public int goldResources = 0;
    public int life = 10;
    public int point = 0;
    public int upgradeFigures = 0;
    
    public Map map;

    void Start()
    {
        player = this;
        gm = GameManager.gameManager;
        StartCoroutine(RisingOfGoldResources());
        //맵 로드
        map = Instantiate(gm.currentMap);
        life = map.playerLife;
        goldResources = map.stageInfos.startGold;
        currentTowerInfo = null;
        //타워소켓에 선택한 타워들 동기화
        for (int i=0; i<towerSocket.Length; i++)
        {
            TowerSocket_Play tempTowerSocket = towerSocket[i].GetComponent<TowerSocket_Play>();
            tempTowerSocket.towerObjInfo = gm.towerObjInfos[i];
            tempTowerSocket.GetComponent<Image>().sprite = gm.LoadImageToSprite(tempTowerSocket.towerObjInfo.towerImagePath);
            towerSocket[i].onClick.AddListener(ClickTowerSocket);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        JudgeGameFail();
        JudgeGameClear();
    }

    public void ClickTowerSocket()
    {
        currentTowerInfo = EventSystem.current.currentSelectedGameObject.GetComponent<TowerSocket_Play>().towerObjInfo;
    }

    IEnumerator RisingOfGoldResources()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            goldResources = goldResources + map.stageInfos.resourceRisingRatio + map.stageInfos.upgradeValueOfReource*upgradeFigures;
        }
    }

    public void UpgradeGoldRisingRatio()
    {
        point -= 5;
        upgradeFigures++;
    }
    public void JudgeGameClear()
    {
        GameObject[] enemyTemp = GameObject.FindGameObjectsWithTag("Enemy1");
        if (life != 0)
            if (map.isLastEnemy)
            {
                if (enemyTemp.Length == 0)
                {
                    Debug.Log("게임 클리어");
                    GamePlayStageManager.GPSM.GameCleared();
                }
            }
    }
    public void JudgeGameFail()
    {
        if(life <= 0)
        {
            Debug.Log("클리어 실패");
            GamePlayStageManager.GPSM.GameFailed();
        }
    }
}
