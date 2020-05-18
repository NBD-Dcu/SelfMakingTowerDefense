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
        instance = this;
        dragingImage = Resources.Load<Image>("드래깅시 생성되는 이미지"); 
        CreateTowerObjList();
    }

    // Update is called once per frame
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
        towerInfoOutput.SetActive(true);
        TowerObjectInformation clickedButton = EventSystem.current.currentSelectedGameObject.GetComponent<TowerObject>().towerObjInfo;
        towerInfoOutput.transform.Find("타워이미지").GetComponent<Image>().sprite = gm.LoadImageToSprite(clickedButton.towerImagePath);
        towerInfoOutput.transform.Find("투사체이미지").GetComponent<Image>().sprite = gm.LoadImageToSprite(clickedButton.projectileImagePath);
        towerInfoOutput.transform.Find("공격력수치표기공간").GetComponent<Text>().text = clickedButton.attackDamage.ToString();
        towerInfoOutput.transform.Find("공격속도수치표기공간").GetComponent<Text>().text = clickedButton.attackSpeed.ToString();
        towerInfoOutput.transform.Find("비용수치표기공간").GetComponent<Text>().text = clickedButton.cost.ToString();
    }

    public void ClickTowerSocketList()
    {
        Button clickedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
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
            towerInfoOutput.SetActive(true);
            towerInfoOutput.transform.Find("타워이미지").GetComponent<Image>().sprite = gm.LoadImageToSprite(gm.towerObjInfos[socketIndex].towerImagePath);
            towerInfoOutput.transform.Find("투사체이미지").GetComponent<Image>().sprite = gm.LoadImageToSprite(gm.towerObjInfos[socketIndex].projectileImagePath);
            towerInfoOutput.transform.Find("공격력수치표기공간").GetComponent<Text>().text = gm.towerObjInfos[socketIndex].attackDamage.ToString();
            towerInfoOutput.transform.Find("공격속도수치표기공간").GetComponent<Text>().text = gm.towerObjInfos[socketIndex].attackSpeed.ToString();
            towerInfoOutput.transform.Find("비용수치표기공간").GetComponent<Text>().text = gm.towerObjInfos[socketIndex].cost.ToString();
        }
    }

    public void HideTowerInformation()
    {
        towerInfoOutput.SetActive(false);
    }
    
}
