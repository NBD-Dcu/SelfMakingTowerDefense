using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;//어디서든 접근 가능하게 하는 static 인스턴스

    public string resourcesFolderPath = "Assets/Resources";
    public string towerListPath = "유저제작파일저장소/타워객체";
    public int previousBulidindex;

    public GameObject[] selectedTowerList = new GameObject[2];//public이라 실제로는 인스펙터에서 사이즈가 지정됨, 후에 접근자로 제어를 하든가 하자

    

    private void Awake()
    {
        if (gameManager == null)
        {//중복 생성이 되지 않도록 함
            gameManager = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Update()
    {
    }

    public void ChangeStage(string stageName)
    {
        SceneManager.LoadScene(stageName);
    }
    public void GotoBack()
    {
        SceneManager.LoadScene(previousBulidindex);
    }
}
