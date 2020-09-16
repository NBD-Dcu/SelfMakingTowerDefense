using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;//어디서든 접근 가능하게 하는 static 인스턴스

    //각종 패스
    public string resourcesFolderPath = "Assets/Resources";//문제없을시 나중에 삭제할 것
    public string towerListPath = "/유저제작파일저장소/타워객체";//문제없을시 나중에 삭제할 것
    public string towerImagePath = "/유저제작파일저장소/타워이미지";
    public string projectileImagePath = "/유저제작파일저장소/투사체이미지";
    public string towerObjectPath = "/유저제작파일저장소/타워객체";
    public string towerObjInfoStore = "Play/타워오브젝트정보체";
    string uiPrefabFolderPath = "Prefab/UI/";

    //모든 씬 공통사항
    public GameObject uiCanvas = null;
    int previousSceneIndex = 0;
    public GameObject exitButton;
    public Color[] imageBackgroundColor;
    //스테이지 선택 씬 변수
    public string StageInformationFolderPath = "StageInformation";//나중에 검토후 지울 것
    public string MapFolderPath = "Resources/Play/Map";
    StageInformation _currentStageInfo = null;//아마도 get ,set으로 구현해야 할 거같음
    //public StageInformation currentStageInfo
    //{
    //    get { return _currentStageInfo;}
    //    set { _currentStageInfo = value; }
    //}
    public Map currentMap;
    //타워 선택 씬 변수
    public TowerObjectInformation[] towerObjInfos;

    //게임 플레이 씬 변수
    //public string MapFolderPath = "Play/Map";
    public string towerPrefabPath = "Play/타워";
    public string projectilePrefabPath = "Play/투사체베이스";

    private void Awake()
    {
        if (gameManager == null)
        {
            Screen.SetResolution(1920, 1080, true);
            gameManager = this;
            DontDestroyOnLoad(this);
            exitButton = null;
        }
    }

    private void Update()
    {
        if(uiCanvas == null)
        {
            uiCanvas = GameObject.Find("UICanvas");
        }
        if(exitButton == null)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                ShowExitMessage();
            }
        }
    }

    public void ChangeStage(string stageName)
    {
        previousSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(stageName);
    }
    public void ShowGuideMessage(string context)
    {
        GameObject guideMessagePrefab = Resources.Load<GameObject>(uiPrefabFolderPath+"안내문구 출력 버튼");
        guideMessagePrefab = Instantiate(guideMessagePrefab,uiCanvas.transform);
        guideMessagePrefab.transform.GetChild(0).GetComponent<Text>().text = context;
        Destroy(guideMessagePrefab, 1);
    }

    //입력한 폴더의 이미지들을 파라미터로 지정된 리스트에 Sprite형태로 저장하는 함수
    public void LoadImageList(string folderPath, List<SpriteWithInformation> imageList)
    {
        try
        {
            DirectoryInfo di = new DirectoryInfo(folderPath);
            int i = 0;
            foreach (FileInfo f in di.GetFiles())
            {
                SpriteWithInformation instance = new SpriteWithInformation();
                instance.sprite = LoadImageToSprite(f.ToString());
                instance.spriteName = f.Name.Substring(0, f.Name.Length - 4);
                instance.spritePath = f.ToString();
                imageList.Add(instance);
                i++;
            }
        }
        catch (DirectoryNotFoundException e)
        {
            Debug.Log("폴더가 없습니다");
        }
    }
    public Sprite LoadImageToSprite(string filePath)
    {
        Texture2D imageTexture = new Texture2D(0, 0);
        Sprite pointedImage = null;
        byte[] bytes = File.ReadAllBytes(filePath);
        if (bytes.Length > 0)
        {
            imageTexture = new Texture2D(0, 0);
            imageTexture.LoadImage(bytes);
            Rect rect = new Rect(0, 0, imageTexture.width, imageTexture.height);
            pointedImage = Sprite.Create(imageTexture, rect, new Vector2(0.5f, 0.5f), 32);
            pointedImage.texture.filterMode = FilterMode.Point;
        }
        else
        {
            Debug.Log("해당 경로에 파일이 존재하지 않습니다");
        }
        return pointedImage;
    }
    public void LoadTowerObjectList(string folderPath, List<TowerObjectInformation> objectList)
    {
        DirectoryInfo di = new DirectoryInfo(folderPath);
        TowerObjectInformation instance = new TowerObjectInformation();
        if (!Directory.Exists(Application.persistentDataPath + GameManager.gameManager.towerObjectPath))
        {
            Debug.Log("오류! 폴더가 존재하지 않습니다");
        }
        else {
            for(int i=0; i< di.GetDirectories().Length; i++)
            {
                string fromJsonData = File.ReadAllText(di.GetDirectories()[i] + "/타워정보/status.json");
                objectList.Add(JsonUtility.FromJson<TowerObjectInformation>(fromJsonData));
            }
        }
    }

    void ShowExitMessage()
    {
        GameObject exitButtonPrefab = Resources.Load<GameObject>("Prefab/UI/게임종료버튼");
        Instantiate(Resources.Load<GameObject>("Prefab/UI/게임종료버튼"),uiCanvas.transform);
        exitButton = exitButtonPrefab;
    }

    //초기화 관련 함수들
    public void BackToStageSelect()
    {
        currentMap = null;
        _currentStageInfo = null;
    }

    public void BackToTowerSelcet()
    {
       for(int i=0; i < towerObjInfos.Length; i++)
        {
            towerObjInfos[i] = null;
        }
    }
    
}
