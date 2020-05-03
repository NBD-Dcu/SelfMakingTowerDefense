using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;//어디서든 접근 가능하게 하는 static 인스턴스

    public string resourcesFolderPath = "Assets/Resources";//문제없을시 나중에 삭제할 것
    public string towerListPath = "유저제작파일저장소/타워객체";//문제없을시 나중에 삭제할 것
    public string towerImagePath = "/유저제작파일저장소/타워이미지";
    public string projectileImagePath = "/유저제작파일저장소/투사체이미지";
    public string towerObjectPath = "/유저제작파일저장소/타워객체";


    string uiPrefabFolderPath = "Prefab/UI/";
    public GameObject uiCanvas = null;
    public GameObject[] selectedTowerList = new GameObject[2];//public이라 실제로는 인스펙터에서 사이즈가 지정됨, 후에 접근자로 제어를 하든가 하자

    

    private void Awake()
    {
        if (gameManager == null)
        {
            Screen.SetResolution(1920, 1080, true);
            gameManager = this;
            DontDestroyOnLoad(this);
        }
        GameManager.gameManager.uiCanvas = GameObject.Find("UICanvas");
    }

    private void Update()
    {
    }

    public void ChangeStage(string stageName)
    {
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
        Texture2D imageTexture = new Texture2D(0, 0);
        Sprite pointedImage;
        DirectoryInfo di = new DirectoryInfo(folderPath);

        int i = 0;
        foreach(FileInfo f in di.GetFiles())
        {
            SpriteWithInformation instance = new SpriteWithInformation();
            byte[] bytes = File.ReadAllBytes(f.ToString());
            if(bytes.Length > 0)
            {
                imageTexture = new Texture2D(0, 0);
                imageTexture.LoadImage(bytes);
                Rect rect = new Rect(0, 0, imageTexture.width, imageTexture.height);
                pointedImage = Sprite.Create(imageTexture, rect, new Vector2(0.5f, 0.5f), 32);
                pointedImage.texture.filterMode = FilterMode.Point;
                instance.sprite = pointedImage;
                instance.spriteName = f.Name.Substring(0, f.Name.Length - 4);
                instance.spritePath = f.ToString();
                imageList.Add(instance);
                i++;
            }
        }
    }
}
