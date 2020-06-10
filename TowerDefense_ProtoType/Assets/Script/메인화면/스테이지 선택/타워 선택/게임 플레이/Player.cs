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
    public Text goldText;
    public int life = 10;
    //맵 정보, 맵 빼고는 가져올 필요가 없을 가능성이 농후
    Map map;

    void Start()
    {
        player = this;
        gm = GameManager.gameManager;
        goldResources = gm.currentStageInfo.startGold;
        StartCoroutine(RisingOfResources());
        //맵 로드
        map = Instantiate(Resources.Load<Map>("Play/Map/Stage1"));
        life = map.playerLife;
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

    public void ClickObtion()
    {
        GameObject obtionPrefab = Resources.Load<GameObject>("Play/옵션호출-플레이스테이지");
        Instantiate(obtionPrefab, gm.uiCanvas.transform);
    }
    IEnumerator RisingOfResources()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            goldResources = goldResources + gm.currentStageInfo.resourceRisingRatio;
            RenewalGoldResource();
        }
    }
    public void RenewalGoldResource()
    {
        goldText.text = goldResources.ToString() + "G";
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
