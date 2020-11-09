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
    public GameObject dragingImage;
    public GameObject towerInfoOutput;
    public Sprite towerSocketImage;

    private void Awake()
    {
        gm = GameManager.gameManager;
        gm.BackToTowerSelcet();
        instance = this;
        dragingImage = Resources.Load<GameObject>("Play/드래깅이미지");
        towerSocketImage = towerSockets[0].GetComponent<Image>().sprite;
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
        towerInfoOutput.transform.Find("타워이름").GetComponent<Text>().text = towerInfo.towerObjectName;
        RectTransform tempTIOP = towerInfoOutput.GetComponent<RectTransform>();

        Vector2 targetPos = position;
        //if(targetPos.x > 650)
        //    targetPos.x = 650;
        //if(targetPos.x < -650)
        //    targetPos.x = -650;
        //if (targetPos.y > 274)
        //    targetPos.y = 274;
        //if (targetPos.y < -274)
        //    targetPos.y = -274;
        tempTIOP.position = targetPos;

        StartCoroutine(HideTowerInforationByTime());
    }
    IEnumerator HideTowerInforationByTime()
    {
        yield return new WaitForSeconds(2.5f);
        towerInfoOutput.SetActive(false);
    }
    public void HideTowerInformation()
    {
        towerInfoOutput.SetActive(false);
        StopCoroutine(HideTowerInforationByTime());
    }

    public void MoveToNstage(string stageName)
    {
        int canCount = 0;
        for (int i = 0; i < gm.towerObjInfos.Length; i++)
        {
            try
            {
                if (gm.towerObjInfos[i].isTrue)
                {
                    canCount++;
                }
            }
            catch
            {

            }
        }
        if(canCount >= gm.towerObjInfos.Length)
        gm.ChangeStage(stageName);
        else
        {
            gm.ShowGuideMessage("타워목록을 모두 채워주세요");
        }
    }
    public void ChangeStage(string stageName)
    {
        gm.ChangeStage(stageName);
    }
}
