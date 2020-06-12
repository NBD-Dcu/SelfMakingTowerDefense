using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class TowerObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public TowerObjectInformation towerObjInfo;
    GameManager gm;
    GameObject dragingObj;
    //Image dragingObj;
    RectTransform dragingObjRtransform;
    TowerSelectingManager TSManager;
    Button targetSocket;
    private void Awake()
    {
        TSManager = TowerSelectingManager.instance;
        gm = GameManager.gameManager;
    }
    private void Update()
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragingObj = Instantiate(TSManager.dragingImage, TSManager.uiCanvas.transform);
        dragingObj.GetComponent<Image>().sprite = GameManager.gameManager.LoadImageToSprite(towerObjInfo.towerImagePath);
        dragingObjRtransform = dragingObj.GetComponent<RectTransform>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        dragingObjRtransform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        int socketIndex = 0;
        Vector2 targetPosition;
        Vector2 dragingObjP = dragingObj.GetComponent<RectTransform>().position;
        targetPosition = TSManager.towerSockets[0].GetComponent<RectTransform>().position;
        float shortestDistance = (dragingObjP - targetPosition).sqrMagnitude;

        for (int i = 1; i < TSManager.towerSockets.Length; i++)
        {
            targetPosition = TSManager.towerSockets[i].GetComponent<RectTransform>().position;
            if (shortestDistance > (dragingObjP - targetPosition).sqrMagnitude)
            {
                shortestDistance = (dragingObjP - targetPosition).sqrMagnitude;
                socketIndex = i;
            }
        }
        if (shortestDistance < 1000)
        {
            GameManager.gameManager.towerObjInfos[socketIndex] = towerObjInfo;

            targetSocket = TSManager.towerSockets[socketIndex];
            targetSocket.GetComponent<TowerSocket_SelectScene>().towerObjInfo = towerObjInfo;
            targetSocket.GetComponent<Image>().sprite = gm.LoadImageToSprite(towerObjInfo.towerImagePath);
        }
        Destroy(dragingObj);
    }
}
