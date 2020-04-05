using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class TowerList : MonoBehaviour
{
    List<GameObject>towerListToChoose = new List<GameObject>();
    void Start()
    {
        GetTowerPaths();
    }
    
    void Update()
    {
        for (int i = 0; i < towerListToChoose.Count; i++)
        {
            transform.GetChild(i).GetComponent<Image>().sprite = towerListToChoose[i].GetComponent<SpriteRenderer>().sprite;
            transform.GetChild(i).GetComponent<TowerListChild>().TowerPrefab = towerListToChoose[i];
        }
    }

    void GetTowerPaths()
    {
        DirectoryInfo di = new DirectoryInfo(GameManager.gameManager.resourcesFolderPath + "/" + GameManager.gameManager.towerListPath);
        foreach(FileInfo f in di.GetFiles())
        {
            string towerName = f.Name.Substring(0, f.Name.Length - 7);
            if (f.Extension.ToLower().CompareTo(".prefab") == 0)
            {
                towerListToChoose.Add(Resources.Load<GameObject>(GameManager.gameManager.towerListPath + "/" + towerName));
            }
        }
    }
}