using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class StageSelectManager : MonoBehaviour
{
    public static StageSelectManager instance;

    StageInformation[] stageInfos;
    GameManager gm;
    int currentStageNum = 0;
    public Button nextStage, previousStage, currentStage;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            gm = GameManager.gameManager;
            stageInfos = new StageInformation[5];

            for (int i = 0; i < stageInfos.Length; i++)
            {
                stageInfos[i] = Resources.Load<StageInformation>(gm.StageInformationFolderPath+"/"+(i+1));
            }
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
        if (currentStageNum >= stageInfos.Length ) currentStageNum = 0;
        //현재스테이지 번호가 언더플로우가 된 경우
        else if (currentStageNum <= -1) currentStageNum = stageInfos.Length-1;
    }

    public void MoveNextScene(string nextSceneName)
    {
        gm.currentStageInfo = stageInfos[currentStageNum];
        SceneManager.LoadScene(nextSceneName);
    }

    public void ChangeStage(string SceneName)
    {
        gm.ChangeStage(SceneName);
    }
    //
    void StageButtonImageManage()
    {
        currentStage.image.sprite = stageInfos[currentStageNum].stagePreviewImage;

        //if (currentStageNum == 0)
        //{
        //    previousStage.GetComponent<Image>().sprite = stageInfos[stageInfos.Length - 1].stagePreviewImage;
        //    nextStage.image.sprite = stageInfos[currentStageNum + 1].stagePreviewImage;
        //}

        //else if(currentStageNum == stageInfos.Length - 1)
        //{
        //    previousStage.image.sprite = stageInfos[currentStageNum - 1].stagePreviewImage;
        //    nextStage.image.sprite = stageInfos[0].stagePreviewImage;
        //}

        //else
        //{
        //    previousStage.image.sprite = stageInfos[currentStageNum - 1].stagePreviewImage;
        //    nextStage.image.sprite = stageInfos[currentStageNum + 1].stagePreviewImage;
        //}
    }
    
}
