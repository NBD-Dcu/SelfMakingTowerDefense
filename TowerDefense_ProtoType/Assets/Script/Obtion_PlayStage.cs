﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Obtion_PlayStage : MonoBehaviour
{
    private void Awake()
    {
       
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ChangeStage(string stageName)
    {
        GameManager.gameManager.ChangeStage(stageName);
    }
    public void TurnOffThis()
    {
        Destroy(this.gameObject);
    }
}
