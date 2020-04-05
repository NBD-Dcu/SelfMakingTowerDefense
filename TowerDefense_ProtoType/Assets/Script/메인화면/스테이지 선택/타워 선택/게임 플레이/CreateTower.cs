using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTower : MonoBehaviour
{
    public int createCount = 0;
    GameObject player;
    
    private void Awake()
    {
        player = GameObject.Find("Player");
    }
    void Start()
    {
        
    }
    
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (createCount == 0)
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D[] hits = Physics2D.RaycastAll(touchPos, Camera.main.transform.forward);
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].collider.tag.Equals("Test") && this.gameObject.Equals(hits[i].collider.gameObject))
                    {
                        Instantiate(player.GetComponent<PlayerInformation>().currentSelectedTower, transform.position, Quaternion.identity);
                        createCount++;
                    }
                }
            }
        }
    }
}
