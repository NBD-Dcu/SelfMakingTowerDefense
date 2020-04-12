﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DrawingManager : MonoBehaviour
{
    public static DrawingManager drawingManager;
    //32픽셀 - 타워
    float _32DistantUnit = 0.03125f;
    public GameObject _32Canvas;
    public GameObject _32TilePrefab;
    Vector2 _32StartPosition = new Vector2(-0.484375f, -0.484375f);
    GameObject[,] _32InterActiveTiles = new GameObject[32, 32];
    //16픽셀 - 투사체

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

    List<GameObject> interactableButtons = new List<GameObject>();

    private void Awake()
    {
        if (drawingManager == null)
        {
            drawingManager = this;
            _32TilePrefab = Resources.Load<GameObject>(drawingScreenPath + "/" + "32상호작용1픽셀");
            CreateInterActiveTiles();
            ClearCanvas();
        }
    }

    void Start()
    {
        
    }
    
    void Update()
    {
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
                _32InterActiveTiles[i, j] = interActiveTile;
                interActiveTile.GetComponent<_32TileScript>().positionX = i;
                interActiveTile.GetComponent<_32TileScript>().positionY = j;
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
        if (mode == 0)
        {

            if (!Directory.Exists(Application.persistentDataPath + towerImagePath))
            {
                Directory.CreateDirectory(Application.persistentDataPath + towerImagePath);
            }
            else
            {
                if (InputNameField.text.Length!=0)
                {
                    bool isExistion = false;
                    DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath + towerImagePath);
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
                        byte[] bytes = _32Canvas.GetComponent<SpriteRenderer>().sprite.texture.EncodeToPNG();
                        File.WriteAllBytes(Application.persistentDataPath + towerImagePath + "/" + InputNameField.text + ".png", bytes);
                        Debug.Log("파일 저장됨");
                    }
                    else
                    {
                        Debug.Log("같은 이름이 존재합니다");
                    }
                }
                else
                {
                    Debug.Log("이름을 입력해 주세요");
                }
            }
        }

    }

    public void LoadLoadScreen()
    {
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
        List<Sprite> imageList = new List<Sprite>();
        Texture2D imageTexture = new Texture2D(0, 0);
        Sprite PointedImage;
        GameObject InteractableImageButtonPrefab = Resources.Load<GameObject>("Prefab/DrawingScreen/상호작용 가능한 이미지 버튼"); //상호작용 가능한 이미지 리스트를 나타내는데 사용될 버튼 프리팹

        string imageName = null;

        string FolderPath;
        if (mode == 0)
        {
            FolderPath = towerImagePath;
        }
        else
        {
            FolderPath = projectileImagePath;
        }

        DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath + FolderPath);

        int i = 0, j = 0;// 이미지 리스트 배치에 사용될 가로, 세로 카운터
        foreach (FileInfo f in di.GetFiles())
        {
            imageName = f.Name.Substring(0, f.Name.Length - 4);//확장자와 경로를 제외한 해당 파일의 이름
            byte[] bytes = File.ReadAllBytes(f.ToString());

            if (bytes.Length > 0)
            {
                imageTexture = new Texture2D(0, 0);
                imageTexture.LoadImage(bytes);
                Rect rect = new Rect(0, 0, imageTexture.width, imageTexture.height);
                PointedImage = Sprite.Create(imageTexture, rect, new Vector2(0.5f, 0.5f),32);
                PointedImage.texture.filterMode = FilterMode.Point;

                InteractableImageButtonPrefab.GetComponent<Image>().sprite = PointedImage;
                GameObject InteractableImageButton = Instantiate(InteractableImageButtonPrefab, loadScreen.transform);

                InteractableImageButton.GetComponent<Button>().onClick.AddListener(ClickImage);

                InteractableImageButton.GetComponent<ImageInformation>().filePath = f.ToString();
                InteractableImageButton.GetComponent<ImageInformation>().fileName = imageName;

                interactableButtons.Add(InteractableImageButton);
                if (InteractableImageButton.transform.parent.GetComponent<RectTransform>().sizeDelta.x / (i * 90) < 1)
                {
                    i = 0;
                    j++;
                }
                InteractableImageButton.transform.localPosition = new Vector2(-400 + i * 90, 0 - j * 90);
                i++;
            }
        }
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

        TerminateLoadScreen();
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
        
        drawingScreen.gameObject.SetActive(true);
        loadScreen.gameObject.SetActive(false);
    }
}
