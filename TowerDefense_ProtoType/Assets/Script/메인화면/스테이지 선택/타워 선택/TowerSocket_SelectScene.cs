using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class TowerSocket_SelectScene : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    int test = 0;

    GameManager gm;
    TowerSelectingManager tm;
    GameObject dragingImage;
    public TowerObjectInformation towerObjInfo;
    public int index;
    private void Awake()
    {
    }

    private void Start()
    {
        gm = GameManager.gameManager;
        tm = TowerSelectingManager.instance;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (towerObjInfo.isTrue)
        {
            dragingImage = Instantiate(tm.dragingImage, tm.uiCanvas.transform);
            dragingImage.GetComponent<Image>().sprite = gm.LoadImageToSprite(towerObjInfo.towerImagePath);
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (towerObjInfo.isTrue)
        {
            dragingImage.GetComponent<RectTransform>().position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (towerObjInfo.isTrue)
        {
            float dragDistance = (dragingImage.GetComponent<RectTransform>().position - GetComponent<RectTransform>().position).sqrMagnitude;
            if (dragDistance > 1500)
            {
                gm.towerObjInfos[index] = null;
                towerObjInfo = null;
                GetComponent<Image>().sprite = tm.towerSocketImage;
            }
            Destroy(dragingImage);
        }
    }
}
