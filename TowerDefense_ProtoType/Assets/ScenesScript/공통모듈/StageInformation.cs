using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInformation : MonoBehaviour
{
    public Sprite stagePreviewImage = null;
    public string stageName = null;
    public int stageIndex = 0;
    public int numberOfWaves = 0;
    public int startGold = 0;//시작 골드
    public int resourceRisingRatio = 1;//초기 자원 상승량
    public int upgradeValueOfReource = 1;//업그레이드에 따른 자원상승량 증가값
    public float cycleOfRiseResources = 1;//자원 상승 주기
    
}
