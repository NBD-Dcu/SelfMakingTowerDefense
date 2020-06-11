using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class TowerSelectingManager : MonoBehaviour
{
    public static TowerSelectingManager instance;

    public Button[] towerSockets;
    public Transform towerObjListScroll;
    TowerObjectInformation currentSelecTowerObjInfo;
    GameManager gm;
    public Canvas uiCanvas;
    public Image dragingImage;
    public GameObject towerInfoOutput;

    private void Awake()
    {
        gm = GameManager.gameManager;
        gm.BackToTowerSelcet();
        instance = this;
        dragingImage = Resources.Load<Image>("드래깅시 생성되는 이미지"); 
        CreateTowerObjList();
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
        }
    }
    
    void CreateTowerObjList()
    {
        List<TowerObjectInformation> towerObjInfoList = new List<TowerObjectInformation>();
        gm.LoadTowerObjectList(Application.persistentDataPath + gm.towerObjectPath, towerObjInfoList);
        GameObject InteractableImageButtonPrefab = Resources.Load<GameObject>(gm.towerObjInfoStore);

        for (int i = 0; i < towerObjInfoList.Count; i++)
        {
            GameObject InteractableImageButton = Instantiate(InteractableImageButtonPrefab, towerObjListScroll);
            InteractableImageButton.GetComponent<TowerObject>().towerObjInfo = towerObjInfoList[i];
            InteractableImageButton.GetComponent<Image>().sprite = gm.LoadImageToSprite(towerObjInfoList[i].towerImagePath);
            InteractableImageButton.GetComponent<Button>().onClick.AddListener(ClickTowerObjList);
        }
    }

    void ClickTowerObjList()
    {
        RectTransform clickedButtonTr = EventSystem.current.currentSelectedGameObject.GetComponent<RectTransform>();
        TowerObjectInformation clickedTowerInfo = EventSystem.current.currentSelectedGameObject.GetComponent<TowerObject>().towerObjInfo;
        RenewalTowerInfoOutput(clickedTowerInfo, new Vector2(clickedButtonTr.position.x, clickedButtonTr.position.y));
    }

    public void ClickTowerSocketList()
    {
        Button clickedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        RectTransform clickedBtTr = EventSystem.current.currentSelectedGameObject.GetComponent<RectTransform>();
        int socketIndex = 0;
        for(int i=0; i<towerSockets.Length; i++)
        {
            if (towerSockets[i].Equals(clickedButton))
            {
                socketIndex = i;
            }
        }
        //towerObjInfos의 널을 체크하는 방법을 연구하기
        if (string.IsNullOrEmpty(gm.towerObjInfos[socketIndex].towerImagePath))
        {

        }
        else
        {
            RenewalTowerInfoOutput(gm.towerObjInfos[socketIndex],new Vector2(clickedBtTr.position.x,clickedBtTr.position.y));
        }
    }

    void RenewalTowerInfoOutput(TowerObjectInformation towerInfo, Vector2 position)
    {
        towerInfoOutput.SetActive(true);
        towerInfoOutput.transform.Find("타워이미지").GetComponent<Image>().sprite = gm.LoadImageToSprite(towerInfo.towerImagePath);
        towerInfoOutput.transform.Find("투사체이미지").GetComponent<Image>().sprite = gm.LoadImageToSprite(towerInfo.projectileImagePath);
        towerInfoOutput.transform.Find("공격력수치표기공간").GetComponent<Text>().text = towerInfo.attackDamage.ToString();
        towerInfoOutput.transform.Find("공격속도수치표기공간").GetComponent<Text>().text = towerInfo.attackSpeed.ToString();
        towerInfoOutput.transform.Find("비용수치표기공간").GetComponent<Text>().text = towerInfo.cost.ToString();
        towerInfoOutput.GetComponent<RectTransform>().position = position;
    }

    public void HideTowerInformation()
    {
        towerInfoOutput.SetActive(false);
    }

    public void MoveToNstage(string stageName)
    {
        int canCount = 0;
        for (int i = 0; i < gm.towerObjInfos.Length; i++)
        {
            if (gm.towerObjInfos[i].isTrue)
            {
                canCount++;
            }
        }
        if(canCount >= gm.towerObjInfos.Length)
        gm.ChangeStage(stageName);
    }
    public void ChangeStage(string stageName)
    {
        gm.ChangeStage(stageName);
    }
}
