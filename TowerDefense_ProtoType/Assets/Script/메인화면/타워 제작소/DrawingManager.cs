using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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
    //
    string drawingScreenPath = "Prefab/DrawingScreen";
    int mode = 0;//타워 제작모드 = 0, 투사체 제작모드 = 1
    public Color currentColor = new Color(255,255,255);

    

    private void Awake()
    {
        if (drawingManager == null)
        {//중복 생성이 되지 않도록 함
            drawingManager = this;
            //DontDestroyOnLoad(this);
            _32TilePrefab = Resources.Load<GameObject>(drawingScreenPath + "/" + "32상호작용1픽셀");
            CreateInterActiveTiles();
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
}
