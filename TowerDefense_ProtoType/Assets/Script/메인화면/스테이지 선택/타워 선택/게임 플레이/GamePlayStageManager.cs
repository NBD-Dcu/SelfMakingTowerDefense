using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GamePlayStageManager : MonoBehaviour
{
    public static GamePlayStageManager GPSM = null;
    public Text stageNameUi;
    public Text currentWaveUi;
    public GameObject gameFailedUi;
    public GameObject gameClearUi;
    public Text goldUi;
    public Text pointUi;
    public Text LifeUi;

    public int currentWaveNumber;

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
        RenewalUi();
    }
    public void RenewalUi()
    {
        goldUi.text = Player.player.goldResources + "G";
        pointUi.text = Player.player.point + "P";
        LifeUi.text = "남은 목숨 : "+Player.player.life;
        stageNameUi.text = Player.player.map.stageInfos.stageName;
        currentWaveUi.text = "Wave - " + Player.player.map.currentWaveInfo.waveNumber.ToString();
    }

    public void ClickObtion()
    {
        Time.timeScale = 0;
        GameObject obtionPrefab = Resources.Load<GameObject>("Play/옵션호출-플레이스테이지");
        Instantiate(obtionPrefab, gm.uiCanvas.transform);
    }

    public void GameFailed()
    {
        Time.timeScale = 0;
        gameFailedUi.SetActive(true);
    }
    public void GameCleared()
    {
        Time.timeScale = 0;
        gameClearUi.SetActive(true);
    }
    public void RestartStage()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ChangeStage(string stageName)
    {
        Time.timeScale = 1;
        gm.ChangeStage(stageName);
    }
}
