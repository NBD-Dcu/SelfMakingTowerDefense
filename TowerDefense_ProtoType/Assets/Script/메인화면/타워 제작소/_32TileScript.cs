﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _32TileScript : MonoBehaviour
{
    DrawingManager DM = DrawingManager.drawingManager;
    public int positionX;
    public int positionY;
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
    
    private void OnMouseUp()
    {
        DrawingManager.drawingManager.ChangePixel(positionX, positionY, DM.currentColor);
    }

    private void OnMouseEnter()
    {
        if(Input.GetMouseButton(0))
            DrawingManager.drawingManager.ChangePixel(positionX, positionY, DM.currentColor);
    }
}
