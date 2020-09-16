using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.IO;

public class StageSelectManager : MonoBehaviour
{
    public static StageSelectManager instance;
    
    //StageInformation[] stageInfos;
    List<Map> maps;

    GameManager gm;
    int currentStageNum = 0;
    public Button nextStage, previousStage, currentStage;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            gm = GameManager.gameManager;
            gm.BackToStageSelect();

            //stageInfos = new StageInformation[5];
            maps = new List<Map>();
            for (int i = 0; i < Resources.LoadAll<Map>(gm.MapFolderPath).Length; i++)
            {
                maps.Add(Resources.LoadAll<Map>(gm.MapFolderPath)[i]);
            }
            
            //DirectoryInfo di = new DirectoryInfo(Resources.LoadAll<Map>(gm.MapFolderPath));
            //foreach(FileInfo f in di.GetFiles())
            //{
            //    maps.Add(Resources.Load<Map>(f.ToString()));
            //}
            //for (int i = 0; i < stageInfos.Length; i++)
            //{
            //    stageInfos[i] = Resources.Load<StageInformation>(gm.StageInformationFolderPath+"/"+(i+1));
            //}
        }
    }

    private void Update()
    {
        StageButtonImageManage();
    }
    //리스너들
    public void StageChangeButtonClick()
    {
        Button currentSelec = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();

        //이전 스테이지 버튼을 누른 경우
        if (currentSelec.Equals(previousStage))
        {
            currentStageNum--;
        }
        //다음 스테이지 버튼을 누른 경우
        else if (currentSelec.Equals(nextStage))
        {
            currentStageNum++;
        }

        //현재스테이지 번호가 오버플로가 된 경우
        if (currentStageNum >= maps.Count ) currentStageNum = 0;
        //현재스테이지 번호가 언더플로우가 된 경우
        else if (currentStageNum <= -1) currentStageNum = maps.Count-1;
    }

    public void MoveNextScene(string nextSceneName)
    {
        gm.currentMap = maps[currentStageNum];
        //gm.currentStageInfo = maps[currentStageNum];
        SceneManager.LoadScene(nextSceneName);
    }

    public void ChangeStage(string SceneName)
    {
        gm.ChangeStage(SceneName);
    }
    //
    void StageButtonImageManage()
    {
        currentStage.image.sprite = maps[currentStageNum].stageInfos.stagePreviewImage;
    }
    
}
