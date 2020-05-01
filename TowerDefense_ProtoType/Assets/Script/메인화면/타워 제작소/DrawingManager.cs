using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DrawingManager : MonoBehaviour
{
    public static DrawingManager drawingManager;
    //32픽셀 - 타워
    public GameObject _32Canvas;
    public GameObject _32CanvasCover;
    public GameObject _32TilePrefab;
    Vector2 _32StartPosition = new Vector2(-0.484375f, -0.484375f);
    float _32DistantUnit = 0.03125f;
    GameObject[,] _32InterActiveTiles = new GameObject[32, 32];
    //8픽셀 - 투사체
    public GameObject _8Canvas;
    public GameObject _8CanvasCover;
    public GameObject _8TilePrefab;
    Vector2 _8StartPosition = new Vector2(-0.4375f, -0.4375f);//0-0.0625-3*0.125
    float _8DistantUnit = 0.125f;
    GameObject[,] _8InterActiveTiles = new GameObject[8, 8];
    //기타등등
    public string towerImagePath;
    public string projectileImagePath;
    public string towerListPath;
    string drawingScreenPath = "Prefab/DrawingScreen";

    int mode = 0;//타워 제작모드 = 0, 투사체 제작모드 = 1

    public Color currentColor;
    public Text InputNameField;

    public GameObject loadScreen;
    public GameObject drawingScreen;

    Button selectedImageButton;
    public Image selectedImageView;
    public Text selectedImageNameView;

    public Transform scrollView;
    List<GameObject> interactableButtons = new List<GameObject>();

    private void Awake()
    {
        if (drawingManager == null)
        {
            drawingManager = this;
            _32TilePrefab = Resources.Load<GameObject>(drawingScreenPath + "/" + "32상호작용1픽셀");
            _8TilePrefab = Resources.Load<GameObject>(drawingScreenPath + "/" + "8상호작용1픽셀");
            CreateInterActiveTiles();
            InitGrid(32, 32);
            mode = 1;
            InitGrid(8, 8);
            mode = 0;
            GameManager.gameManager.uiCanvas = GameObject.Find("UICanvas");
        }
    }
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    //그림 그리기 화면에서 사용
    public void ChangeMode(int mode)
    {
        this.mode = mode;
        if (mode == 0)
        {
            _32Canvas.SetActive(true);
            _32CanvasCover.SetActive(true);
            _8Canvas.SetActive(false);
            _8CanvasCover.SetActive(false);
        }
        else if (mode == 1)
        {
            _32Canvas.SetActive(false);
            _32CanvasCover.SetActive(false);
            _8Canvas.SetActive(true);
            _8CanvasCover.SetActive(true);
        }
    }
    public void ChangePixel(int x, int y, Color color)
    {
        Texture2D tex;
        if(mode == 0)
        {
            tex = _32Canvas.GetComponent<SpriteRenderer>().sprite.texture;
            tex.SetPixel(x, y, color);
            tex.Apply();//실제 그림 파일에 픽셀 반영
        }
        else if(mode == 1)
        {
            tex = _8Canvas.GetComponent<SpriteRenderer>().sprite.texture;
            tex.SetPixel(x, y, color);
            tex.Apply();//실제 그림 파일에 픽셀 반영
        }
    }
    public void CreateInterActiveTiles()
    {
        //32픽셀
        for(int i=0; i<32; i++)
        {
            for(int j=0; j<32; j++)
            {
                Vector2 positionValue = new Vector2(_32StartPosition.x + _32DistantUnit * i, _32StartPosition.y + _32DistantUnit * j);
                GameObject interActiveTile = Instantiate(_32TilePrefab, positionValue, Quaternion.identity);

                interActiveTile.transform.SetParent(_32Canvas.transform);

                _32InterActiveTiles[i, j] = interActiveTile;
                interActiveTile.GetComponent<TileScript>().positionX = i;
                interActiveTile.GetComponent<TileScript>().positionY = j;
            }
        }

        //8픽셀
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Vector2 positionValue = new Vector2(_8StartPosition.x + _8DistantUnit * i, _8StartPosition.y + _8DistantUnit * j);
                GameObject interActiveTile = Instantiate(_8TilePrefab, positionValue, Quaternion.identity);

                interActiveTile.transform.SetParent(_8Canvas.transform);//테스트중

                _8InterActiveTiles[i, j] = interActiveTile;
                interActiveTile.GetComponent<TileScript>().positionX = i;
                interActiveTile.GetComponent<TileScript>().positionY = j;
            }
        }


    }
    void InitGrid(int x, int y)
    {
        for (int i = 0; i < x; i++)
        {
            for(int j=0; j< y; j++)
            {
                ChangePixel(j, i, new Color(1, 1, 1, 0));//투명색으로 초기화
            }
        }
    }
    public void ClearCanvas()
    {
        if (mode == 0)
            InitGrid(32, 32);
        if (mode == 1)
            InitGrid(8, 8);
    }
    public void ChangeBrushColor()
    {
        GameObject clikedButton = GameObject.Find(EventSystem.current.currentSelectedGameObject.name);
        currentColor = clikedButton.GetComponent<Image>().color;
    }
    public void ChangeBrushToEraser()
    {
        currentColor = new Color(255, 255, 255, 0);
    }
    public void SaveImage()
    {
        string folderPath = null;
        if(mode == 0)
        {
            folderPath = towerImagePath;
        }
        else if(mode == 1)
        {
            folderPath = projectileImagePath;
        }
        if (!Directory.Exists(Application.persistentDataPath + folderPath))
        {
            Directory.CreateDirectory(Application.persistentDataPath + folderPath);
        }
        else
        {
            if (InputNameField.text.Length != 0)
            {
                bool isExistion = false;
                DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath + folderPath);
                foreach (FileInfo f in di.GetFiles())
                {
                    string NameInFolder = f.Name.Substring(0, f.Name.Length - 4);
                    if (f.Extension.ToLower().CompareTo(".png") == 0)
                    {
                        if (NameInFolder.Equals(InputNameField.text))
                            isExistion = true;
                    }
                }

                if (isExistion == false)
                {
                    byte[] bytes = null;
                    if (mode == 0)
                    {
                        bytes = _32Canvas.GetComponent<SpriteRenderer>().sprite.texture.EncodeToPNG();
                    }
                    else if(mode == 1)
                    {
                        bytes = _8Canvas.GetComponent<SpriteRenderer>().sprite.texture.EncodeToPNG();
                    }
                        File.WriteAllBytes(Application.persistentDataPath + folderPath + "/" + InputNameField.text + ".png", bytes);
                        GameManager.gameManager.ShowGuideMessage("파일 저장됨");
                    
                }
                else
                {
                    GameManager.gameManager.ShowGuideMessage("동일한 이름이 존재합니다");
                }
            }
            else
            {
                GameManager.gameManager.ShowGuideMessage("이름을 입력해 주세요");
            }
        }

    }
    
    //불러오기 화면에서 사용
    public void LoadLoadScreen()
    {
        _32Canvas.SetActive(false);
        _8Canvas.SetActive(false);
        loadScreen.gameObject.SetActive(true);
        drawingScreen.gameObject.SetActive(false);
        for (int i = 0; i < interactableButtons.Count; i++)
        {
            Destroy(interactableButtons[i]);
        }
        interactableButtons.Clear();
        selectedImageView.sprite = null;
        selectedImageNameView.text = null;
        LoadImageList();
    }
    void LoadImageList()
    {
        string FolderPath;
        if (mode == 0)
        {
            FolderPath = towerImagePath;
        }
        else
        {
            FolderPath = projectileImagePath;
        }
        List<SpriteWithInformation> imageList = new List<SpriteWithInformation>();
        GameManager.gameManager.LoadImageList(Application.persistentDataPath + FolderPath, imageList);
        GameObject InteractableImageButtonPrefab = Resources.Load<GameObject>("Prefab/DrawingScreen/상호작용 가능한 이미지 버튼"); //상호작용 가능한 이미지 리스트를 나타내는데 사용될 버튼 프리팹

        for (int i = 0; i < imageList.Count; i++)
        {
            GameObject InteractableImageButton = Instantiate(InteractableImageButtonPrefab, scrollView);//버튼 생성
            RectTransform rectTransForm = InteractableImageButton.GetComponent<RectTransform>();

            InteractableImageButton.GetComponent<Image>().sprite = imageList[i].sprite;
            Debug.Log(imageList[1].spriteName);
            InteractableImageButton.GetComponent<Button>().onClick.AddListener(ClickImage);//버튼에 리스너 부착
            InteractableImageButton.GetComponent<ImageInformation>().filePath = imageList[i].spritePath;
            InteractableImageButton.GetComponent<ImageInformation>().fileName = imageList[i].spriteName;
            interactableButtons.Add(InteractableImageButton);
        }
    }
    public void ClickImage()
    {
        selectedImageButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        selectedImageView.sprite = selectedImageButton.GetComponent<Image>().sprite;
        selectedImageNameView.text = selectedImageButton.GetComponent<ImageInformation>().fileName;
    }
    public void DeleteImageFile()
    {
        File.Delete(selectedImageButton.GetComponent<ImageInformation>().filePath);
        for(int i=0; i< interactableButtons.Count; i++)
        {
            Destroy(interactableButtons[i]);
        }
        interactableButtons.Clear();
        selectedImageView.sprite = null;
        selectedImageNameView.text = null;
        LoadImageList();
    }
    public void TerminateLoadScreen()
    {
        if (mode == 0)
            _32Canvas.SetActive(true);
        else if (mode == 1)
            _8Canvas.SetActive(true);
        drawingScreen.gameObject.SetActive(true);
        loadScreen.gameObject.SetActive(false);
    }
    public void ChangeCanvasImage()
    {
        Sprite sourceImage = selectedImageButton.GetComponent<Image>().sprite;
        if (mode == 0)
        {
            for (int i = 0; i < 32; i++)
            {
                for (int j = 0; j < 32; j++)
                {
                    ChangePixel(i, j, sourceImage.texture.GetPixel(i, j));
                }
            }
        }
        else if(mode == 1)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    ChangePixel(i, j, sourceImage.texture.GetPixel(i, j));
                }
            }
        }
        TerminateLoadScreen();
    }
}
