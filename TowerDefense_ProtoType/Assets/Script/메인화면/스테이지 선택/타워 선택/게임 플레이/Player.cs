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

    //맵 정보, 맵 빼고는 가져올 필요가 없을 가능성이 농후
    GameObject map;

    void Start()
    {
        player = this;
        gm = GameManager.gameManager;
        //맵 로드
        map = Resources.Load<GameObject>("Play/Map/Stage1");
        Instantiate(map);
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

    public void ClickTowerSocket()
    {
        currentTowerInfo = EventSystem.current.currentSelectedGameObject.GetComponent<TowerSocket_Play>().towerObjInfo;
    }

    public void ClickObtion()
    {
        GameObject obtionPrefab = Resources.Load<GameObject>("Play/옵션호출-플레이스테이지");
        Instantiate(obtionPrefab, gm.uiCanvas.transform);
    }
}
