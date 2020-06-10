using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayStageManager : MonoBehaviour
{
    public static GamePlayStageManager GPSM = null;
    public GameObject gameFailedUi;
    public GameObject gameClearUi;
    GameManager gm;
    private void Awake()
    {
        gm = GameManager.gameManager;
        GPSM = this;
    }
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public void GameFailed()
    {
        Time.timeScale = 0;
        gameFailedUi.SetActive(true);
    }
    public void GameCleared()
    {
        gameClearUi.SetActive(true);
        Time.timeScale = 0;
    }

    public void ChangeStage(string stageName)
    {
        gm.ChangeStage(stageName);
    }
}
