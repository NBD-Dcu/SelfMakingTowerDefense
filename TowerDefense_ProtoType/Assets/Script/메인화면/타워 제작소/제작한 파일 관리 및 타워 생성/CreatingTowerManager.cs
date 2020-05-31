using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
public class CreatingTowerManager : MonoBehaviour
{
    Button selectedImageButton = null;
    GameManager gmInstance = null;

    public StatusUI statusUi = null;
    public Transform towerScrollView = null;
    public Transform ProjectilScrollView = null;
    public Image towerImageView = null;
    public Image projectileImageView = null;

    //추후에 필요없으면 삭제-1
    string currentSelectedTowerImagePath = null;
    string currentSelectedTowerImageName = null;
    string currentSelectedProjectileImagePath = null;
    string currentSelectedProjectileImageName = null;
    
    public void Awake()
    {
        gmInstance = GameManager.gameManager;
        gmInstance.uiCanvas = GameObject.Find("UICanvas");
        LoadImageList();
    }

    public void ClickTowerImage()
    {
        selectedImageButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();//눌러진 이미지버튼 객체를 가져옴
        ImageInformation btInfo = selectedImageButton.GetComponent<ImageInformation>(); 
        towerImageView.sprite = selectedImageButton.GetComponent<Image>().sprite;
        currentSelectedTowerImagePath = btInfo.filePath;
        currentSelectedTowerImageName = btInfo.fileName;
    }
    public void ClickProjectileImage()
    {
        selectedImageButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        ImageInformation btInfo = selectedImageButton.GetComponent<ImageInformation>();
        projectileImageView.sprite = selectedImageButton.GetComponent<Image>().sprite;
        currentSelectedProjectileImagePath = btInfo.filePath;
        currentSelectedProjectileImageName = btInfo.fileName;
    }

    void LoadImageList()
    {
        
        List<SpriteWithInformation> imageList = new List<SpriteWithInformation>();
        GameManager.gameManager.LoadImageList(Application.persistentDataPath + gmInstance.towerImagePath, imageList);
        GameObject InteractableImageButtonPrefab = Resources.Load<GameObject>("Prefab/DrawingScreen/상호작용 가능한 이미지 버튼"); //상호작용 가능한 이미지 리스트를 나타내는데 사용될 버튼 프리팹

        //타워 이미지 리스트 생성
        for (int i = 0; i < imageList.Count; i++)
        {
            GameObject InteractableImageButton = Instantiate(InteractableImageButtonPrefab, towerScrollView);
            RectTransform rectTransForm = InteractableImageButton.GetComponent<RectTransform>();//나중에 지우기

            InteractableImageButton.GetComponent<Image>().sprite = imageList[i].sprite;
            InteractableImageButton.GetComponent<Button>().onClick.AddListener(ClickTowerImage);//버튼에 리스너 부착
            InteractableImageButton.GetComponent<ImageInformation>().filePath = imageList[i].spritePath;
            InteractableImageButton.GetComponent<ImageInformation>().fileName = imageList[i].spriteName;
        }
        //투사체 이미지 리스트 생성
        imageList.Clear();
        GameManager.gameManager.LoadImageList(Application.persistentDataPath + gmInstance.projectileImagePath, imageList);
        for (int i = 0; i < imageList.Count; i++)
        {
            GameObject InteractableImageButton = Instantiate(InteractableImageButtonPrefab, ProjectilScrollView);
            RectTransform rectTransForm = InteractableImageButton.GetComponent<RectTransform>();//나중에 지우기

            InteractableImageButton.GetComponent<Image>().sprite = imageList[i].sprite;
            InteractableImageButton.GetComponent<Button>().onClick.AddListener(ClickProjectileImage);//버튼에 리스너 부착
            InteractableImageButton.GetComponent<ImageInformation>().filePath = imageList[i].spritePath;
            InteractableImageButton.GetComponent<ImageInformation>().fileName = imageList[i].spriteName;
        }
    }
    
    public void CreatingTower()
    {
        if (!Directory.Exists(Application.persistentDataPath + GameManager.gameManager.towerObjectPath))
        {
            Directory.CreateDirectory(Application.persistentDataPath + GameManager.gameManager.towerObjectPath);
        }

        if (currentSelectedTowerImagePath != null && currentSelectedProjectileImagePath != null)
        {

            TowerObjectInformation towerInfo = new TowerObjectInformation();
            //타워 오브젝트 폴더 지정
            DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath + GameManager.gameManager.towerObjectPath);
            int nextFolderIndex = Directory.GetDirectories(Application.persistentDataPath + GameManager.gameManager.towerObjectPath, "*", SearchOption.TopDirectoryOnly).Length;
            string nextFolderPath = Application.persistentDataPath + GameManager.gameManager.towerObjectPath + "/" + nextFolderIndex;
            Directory.CreateDirectory(nextFolderPath);
            Directory.CreateDirectory(nextFolderPath + "/타워이미지");
            Directory.CreateDirectory(nextFolderPath + "/투사체이미지");
            Directory.CreateDirectory(nextFolderPath + "/타워정보");

            string towerObjTowerImagePath = Path.Combine(nextFolderPath + "/타워이미지", currentSelectedTowerImageName + ".png");
            string towerobjProjectileImagePath = Path.Combine(nextFolderPath + "/투사체이미지", currentSelectedProjectileImageName + ".png");
            File.Copy(currentSelectedTowerImagePath, towerObjTowerImagePath);
            File.Copy(currentSelectedProjectileImagePath, towerobjProjectileImagePath);

            towerInfo.towerImagePath = towerObjTowerImagePath;
            towerInfo.projectileImagePath = towerobjProjectileImagePath;
            towerInfo.towerObjectName = currentSelectedTowerImageName;
            towerInfo.projectileName = currentSelectedProjectileImageName;
            towerInfo.attackDamage = int.Parse(statusUi.attackDamage.text);
            towerInfo.attackSpeed = int.Parse(statusUi.attackSpeed.text);
            towerInfo.cost = int.Parse(statusUi.cost.text);
            towerInfo.index = nextFolderIndex;
            towerInfo.isTrue = true;
            string filePath = nextFolderPath + "/타워정보/status.json";
            towerInfo.thisFilePath = filePath;
            
            string toJsonData = JsonUtility.ToJson(towerInfo);
            File.WriteAllText(filePath, toJsonData);

            GameManager.gameManager.ShowGuideMessage("타워 객체가 생성되었습니다");
        }
        else GameManager.gameManager.ShowGuideMessage("타워와 투사체를 선택해주세요");
    }
    public void changeStage(string stageName)
    {
        gmInstance.ChangeStage(stageName);
    }
}
