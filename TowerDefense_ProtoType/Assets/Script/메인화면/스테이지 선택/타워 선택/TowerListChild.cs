using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerListChild : MonoBehaviour
{
    public GameObject TowerPrefab;
    public void GetTowerResources()
    {
        for (int i = 0; i < GameManager.gameManager.selectedTowerList.Length; i++)
        {
            if (GameManager.gameManager.selectedTowerList[i] == null)
            {
                GameManager.gameManager.selectedTowerList[i] = TowerPrefab;
                break;
            }
        }
    }
}
