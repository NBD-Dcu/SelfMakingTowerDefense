using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
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

    private void OnMouseDown()
    {
        DM.startPosition_draw = new Vector2(positionX, positionY);
        DrawingManager.drawingManager.ChangePixel(positionX, positionY, DM.currentColor);
        DM.SaveCurrentState();
    }

    private void OnMouseEnter()
    {
        if (Input.GetMouseButton(0))
        {
            DM.endPosition_draw = new Vector2(positionX, positionY);

            int xDifference = (int)(DM.startPosition_draw.x - DM.endPosition_draw.x);
            int yDifference = (int)(DM.startPosition_draw.y - DM.endPosition_draw.y);
            //이전 상태와 비교해 변화가 있는 경우
            if (DM.startPosition_draw.x != DM.endPosition_draw.x || DM.startPosition_draw.y != DM.endPosition_draw.y)
            {
                //x,y축 양축으로 다 벗어난 경우
                if (DM.startPosition_draw.x != DM.endPosition_draw.x)
                {
                    if (DM.startPosition_draw.y != DM.endPosition_draw.y)
                    {
                        DM.startPosition_draw = new Vector2(positionX, positionY);
                        DM.ChangePixel(positionX, positionY, DM.currentColor);
                    }
                }

                //x축으로만 벗어난 경우
                if (DM.startPosition_draw.x != DM.endPosition_draw.x && yDifference == 0)
                {
                    //앞으로 벗어난 경우
                    if (xDifference < 0)
                    {
                        for (int i = 0; i <= Mathf.Abs(xDifference); i++)
                            DM.ChangePixel((int)(DM.startPosition_draw.x) + i, positionY, DM.currentColor);
                    }
                    //뒤로 벗어난 경우
                    if (xDifference > 0)
                    {
                        for (int i = 0; i <= Mathf.Abs(xDifference); i++)
                            DM.ChangePixel((int)(DM.startPosition_draw.x) - i, positionY, DM.currentColor);
                    }
                }

                //y축으로만 벗어난 경우
                if (DM.startPosition_draw.y != DM.endPosition_draw.y && xDifference == 0)
                {
                    //위로 벗어난 경우
                    if (yDifference < 0)
                    {
                        for (int i = 0; i <= Mathf.Abs(yDifference); i++)
                            DM.ChangePixel(positionX, (int)(DM.startPosition_draw.y) + i, DM.currentColor);
                    }
                    //아래로 벗어난 경우
                    if (yDifference > 0)
                    {
                        for (int i = 0; i <= Mathf.Abs(yDifference); i++)
                            DM.ChangePixel(positionX, (int)(DM.startPosition_draw.y) - i, DM.currentColor);
                    }
                }
                DM.SaveCurrentState();
            }
        }
    }
}
