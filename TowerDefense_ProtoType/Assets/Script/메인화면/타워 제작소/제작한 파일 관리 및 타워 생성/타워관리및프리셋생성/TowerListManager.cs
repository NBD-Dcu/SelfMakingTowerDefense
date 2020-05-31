using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;

public class TowerListManager : MonoBehaviour
{
    public Transform objectScrollView;
    List<TowerObjectInformation> towerObjInfoList = new List<TowerObjectInformation>();
    List<GameObject> interactableButtons = new List<GameObject>();
    GameManager gm = null;
    //현재 선택된 타워 정보 표기하는 곳
    public Image towerImageView = null;
    public Image projectileImageView = null;
    public Text attackDamage = null;
    public Text attackSpeed = null;
    public Text cost = null;
    public TowerObject towerObj = null; // 현재 선택된 타워 객체

    private void Awake()
    {
        LoadTowerObjectList();
        gm = GameManager.gameManager;
    }

    //towerobjectinformation을 가진 버튼으로 나오도록 교체
    void LoadTowerObjectList()
    {
        GameManager.gameManager.LoadTowerObjectList(Application.persistentDataPath + GameManager.gameManager.towerObjectPath, towerObjInfoList);
        GameObject InteractableImageButtonPrefab = Resources.Load<GameObject>("Prefab/DrawingScreen/CreatingTower/CreatingTowerPreset/타워객체");
        for (int i = 0; i < towerObjInfoList.Count; i++)
        {
            GameObject InteractableImageButton = Instantiate(InteractableImageButtonPrefab, objectScrollView);
            TowerObject towerObj = InteractableImageButton.GetComponent<TowerObject>();
            towerObj.towerObjInfo = towerObjInfoList[i];
            InteractableImageButton.GetComponent<Image>().sprite = GameManager.gameManager.LoadImageToSprite(towerObj.towerObjInfo.towerImagePath);
            InteractableImageButton.GetComponent<Button>().onClick.AddListener(ClickTowerImage);//버튼에 리스너 부착
            interactableButtons.Add(InteractableImageButton);
        }
    }
    public void ClickTowerImage()
    {
        towerObj = EventSystem.current.currentSelectedGameObject.GetComponent<TowerObject>();//눌러진 이미지버튼 객체를 가져옴

        towerImageView.sprite = gm.LoadImageToSprite(towerObj.towerObjInfo.towerImagePath);
        projectileImageView.sprite = gm.LoadImageToSprite(towerObj.towerObjInfo.projectileImagePath);
        attackDamage.text = towerObj.towerObjInfo.attackDamage.ToString();
        attackSpeed.text = towerObj.towerObjInfo.attackSpeed.ToString();
        cost.text = towerObj.towerObjInfo.cost.ToString();
    }

    public void RemoveTower()
    {
        DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath + GameManager.gameManager.towerObjectPath + "/" + towerObj.towerObjInfo.index);
        di.Delete(true);
        for (int i = 0; i < interactableButtons.Count; i++)
        {
            Destroy(interactableButtons[i]);
        }
        interactableButtons.Clear();
        towerObjInfoList.Clear();

        LoadTowerObjectList();
    }
}
