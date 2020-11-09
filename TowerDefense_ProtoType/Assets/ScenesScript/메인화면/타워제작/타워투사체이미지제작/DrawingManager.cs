using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DrawingManager : MonoBehaviour
{
    public static DrawingManager drawingManager;
    //32픽셀 - 타워
    public GameObject _32Canvas;
    public GameObject _32CanvasCover;
    public GameObject _32TilePrefab;
    Vector2 _32StartPosition = new Vector2(-0.484375f, -0.584375f);
    float _32DistantUnit = 0.03125f;
    GameObject[,] _32InterActiveTiles = new GameObject[32, 32];
    //8픽셀 - 투사체
    public GameObject _8Canvas;
    public GameObject _8CanvasCover;
    public GameObject _8TilePrefab;
    Vector2 _8StartPosition = new Vector2(-0.4375f, -0.5375f);//0-0.0625-3*0.125
    float _8DistantUnit = 0.125f;
    GameObject[,] _8InterActiveTiles = new GameObject[8, 8];
    //기타등등
    GameManager gmInstance = null;
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

    public Vector2 startPosition_draw;
    public Vector2 endPosition_draw;

    UndoRedoHistory<Sprite> _32CanvasUndoRedoHis;
    UndoRedoHistory<Sprite> _8CanvasUndoRedoHis;

    public GameObject currentReferdCanvas;//추후에 이걸로 사용하는 코드로 교체하기
    public GameObject currentReferdCanvasCover;//추후에 이걸로 사용하는 코드로 교체하기
    public UndoRedoHistory<Sprite> currentReferdCanvasHis;
    private void Awake()
    {
        //초기화
        if (drawingManager == null)
        {
            gmInstance = GameManager.gameManager;
            drawingManager = this;
            _32TilePrefab = Resources.Load<GameObject>("Prefab/DrawingScreen" + "/" + "32상호작용1픽셀");
            _8TilePrefab = Resources.Load<GameObject>("Prefab/DrawingScreen" + "/" + "8상호작용1픽셀");
            CreateInterActiveTiles();
            InitGrid(32, 32);
            mode = 1;
            InitGrid(8, 8);
            mode = 0;
            GameManager.gameManager.uiCanvas = GameObject.Find("UICanvas");
            _32CanvasUndoRedoHis = new UndoRedoHistory<Sprite>();
            _8CanvasUndoRedoHis = new UndoRedoHistory<Sprite>();
        }
    }
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (mode == 0)
        {
            currentReferdCanvas = _32Canvas;
            currentReferdCanvasCover = _32CanvasCover;
            currentReferdCanvasHis = _32CanvasUndoRedoHis;
        }
        else if (mode == 1)
        {
            currentReferdCanvas = _8Canvas;
            currentReferdCanvasCover = _8CanvasCover;
            currentReferdCanvasHis = _8CanvasUndoRedoHis;
        }
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

    //좌표와 색깔을 받아 현재 모드에 따라 캔버스의 픽셀을 교체하고 영구적으로 저장
    public void ChangePixel(int x, int y, Color color)
    {
        Texture2D tex;
        if(mode == 0)
        {
            tex = _32Canvas.GetComponent<SpriteRenderer>().sprite.texture;
            tex.SetPixel(x, y, color);
            tex.Apply();
        }
        else if(mode == 1)
        {
            tex = _8Canvas.GetComponent<SpriteRenderer>().sprite.texture;
            tex.SetPixel(x, y, color);
            tex.Apply();
        }
    }
    //타일생성
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
    //모드에 따른 x,y크기를 입력시 초기화
    void InitGrid(int x, int y)
    {
        for (int i = 0; i < x; i++)
        {
            for(int j=0; j< y; j++)
            {
                ChangePixel(j, i, new Color(1, 1, 1, 0));
            }
        }
    }
    //캔버스를 초기화
    public void ClearCanvas()
    {
        if (mode == 0)
            InitGrid(32, 32);
        if (mode == 1)
            InitGrid(8, 8);
    }
    //사용할 색 변경
    public void ChangeBrushColor()
    {
        GameObject clikedButton = GameObject.Find(EventSystem.current.currentSelectedGameObject.name);
        currentColor = clikedButton.GetComponent<Image>().color;
    }
    //지우개로 변경
    public void ChangeBrushToEraser()
    {
        currentColor = new Color(255, 255, 255, 0);
    }
    //만든 이미지 저장
    public void SaveImage()
    {
        string folderPath = null;
        if(mode == 0)
        {
            folderPath = gmInstance.towerImagePath;
        }
        else if(mode == 1)
        {
            folderPath = gmInstance.projectileImagePath;
        }
        if (!Directory.Exists(Application.persistentDataPath + folderPath))
        {
            Directory.CreateDirectory(Application.persistentDataPath + folderPath);
        }
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
    
    //불러오기 화면으로 전환
    public void LoadLoadScreen()
    {
        _32Canvas.SetActive(false);
        _32CanvasCover.SetActive(false);
        _8Canvas.SetActive(false);
        _8CanvasCover .SetActive(false);
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
    //타워or투사체 이미지를 연결된 스크롤 뷰에 나타냄
    void LoadImageList()
    {
        int colorIndex = 0;
        string FolderPath;
        if (mode == 0)
        {
            FolderPath = gmInstance.towerImagePath;
        }
        else
        {
            FolderPath = gmInstance.projectileImagePath;
        }
        System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(Application.persistentDataPath + FolderPath);
        if (di.Exists)
        {
            List<SpriteWithInformation> imageList = new List<SpriteWithInformation>();
            GameManager.gameManager.LoadImageList(Application.persistentDataPath + FolderPath, imageList);
            GameObject InteractableImageButtonPrefab = Resources.Load<GameObject>("Prefab/DrawingScreen/상호작용이미지버튼배경"); //상호작용 가능한 이미지 리스트를 나타내는데 사용될 버튼 프리팹
            for (int i = 0; i < imageList.Count; i++)
            {
                GameObject interactableImageButton = Instantiate(InteractableImageButtonPrefab, scrollView);//버튼 생성
                GameObject realButton = interactableImageButton.transform.GetChild(0).gameObject;
                interactableImageButton.GetComponent<Image>().color = gmInstance.imageBackgroundColor[colorIndex++];//리스트 업 된 이미지의 순서마다 배경색이 정해진다
                if (colorIndex >= gmInstance.imageBackgroundColor.Length)
                    colorIndex = 0;
                realButton.GetComponent<Image>().sprite = imageList[i].sprite;
                realButton.GetComponent<Button>().onClick.AddListener(ClickImage);//버튼에 리스너 부착
                realButton.GetComponent<ImageInformation>().filePath = imageList[i].spritePath;
                realButton.GetComponent<ImageInformation>().fileName = imageList[i].spriteName;
                interactableButtons.Add(interactableImageButton);
            }
        }
        else
        {
            Debug.Log("폴더가 존재하지 않음");
        }
    }
    //스크롤 뷰에 있는 이미지를 클릭했을때 동작
    public void ClickImage()
    {
        selectedImageButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        selectedImageView.sprite = selectedImageButton.GetComponent<Image>().sprite;
        selectedImageNameView.text = selectedImageButton.GetComponent<ImageInformation>().fileName;
    }
    //선택한 이미지를 삭제
    public void DeleteImageFile()
    {
        try
        {
            File.Delete(selectedImageButton.GetComponent<ImageInformation>().filePath);
            for (int i = 0; i < interactableButtons.Count; i++)
            {
                Destroy(interactableButtons[i]);
            }
            interactableButtons.Clear();
            selectedImageView.sprite = null;
            selectedImageNameView.text = null;
            LoadImageList();
        }
        catch
        {
        }
    }
    //불러오기 화면에서 나갈때 동작
    public void TerminateLoadScreen()
    {
        if (mode == 0)
        {
            _32Canvas.SetActive(true);
            _32CanvasCover.SetActive(true);
        }
        else if (mode == 1)
        {
            _8Canvas.SetActive(true);
            _8CanvasCover.SetActive(true);
        }
        for (int i = 0; i < interactableButtons.Count; i++)
        {
            Destroy(interactableButtons[i]);
        }
        interactableButtons.Clear();
        drawingScreen.gameObject.SetActive(true);
        loadScreen.gameObject.SetActive(false);
    }

    public void ChangeCanvasImage()
    {
        try
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
            else if (mode == 1)
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
        catch
        {

        }
    }

    public void moveToNstage(string stageName)//다음
    {
        gmInstance.ChangeStage(stageName);
    }

    public void Undo()
    {
        if (currentReferdCanvasHis.IsCanUndo)
        {
            Vector2 spriteSize = currentReferdCanvas.GetComponent<SpriteRenderer>().sprite.rect.size;
            Sprite stackImage = currentReferdCanvasHis.Undo();
            for (int i = 0; i < spriteSize.x; i++)
            {
                for (int j = 0; j < spriteSize.y; j++)
                {
                    ChangePixel(i, j, stackImage.texture.GetPixel(i, j));
                    Debug.Log(stackImage.texture.GetPixel(i, j));
                }
            }
            Debug.Log("언두");
        }
    }

    public void Redo()
    {
        if (currentReferdCanvasHis.IsCanRedo)
        {
            Vector2 spriteSize = currentReferdCanvas.GetComponent<SpriteRenderer>().sprite.rect.size;
            Sprite stackImage =  currentReferdCanvasHis.Redo();
            for (int i = 0; i < spriteSize.x; i++)
            {
                for (int j = 0; j < spriteSize.y; j++)
                {
                    ChangePixel(i, j, stackImage.texture.GetPixel(i, j));
                }
            }
            Debug.Log("리두");
        }
    }

    public void SaveCurrentState()
    {
        Debug.Log("언두 스택에 추가");
        currentReferdCanvasHis.AddState(currentReferdCanvas.GetComponent<SpriteRenderer>().sprite);
    }
}
