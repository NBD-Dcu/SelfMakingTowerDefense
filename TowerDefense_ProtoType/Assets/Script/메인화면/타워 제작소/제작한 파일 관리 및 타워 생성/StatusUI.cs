using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StatusUI : MonoBehaviour
{
    public Text attackDamage;
    public Text attackSpeed;
    public Text cost;

    private void Update()
    {
       TowerCostCalculation();
    }

    public void ChangeDamageStatus(int mode)
    {
        int status = 0;
        if(mode == 0)
        {
            status = int.Parse(attackDamage.text);
            status++;
            attackDamage.text = status.ToString();
        }
        else if(mode == 1)
        {
            if (int.Parse(attackDamage.text) >= 2)
            {
                status = int.Parse(attackDamage.text);
                status--;
                attackDamage.text = status.ToString();
            }
        }
    }
    public void ChangeSpeedStatus(int mode)
    {
        int status = 0;
        if (mode == 0)
        {
            status = int.Parse(attackSpeed.text);
            status++;
            attackSpeed.text = status.ToString();
        }
        else if (mode == 1)
        {
            if (int.Parse(attackSpeed.text) >= 2)
            {
                status = int.Parse(attackSpeed.text);
                status--;
                attackSpeed.text = status.ToString();
            }
        }
    }
    public void TowerCostCalculation()
    {
        int status = 0;
        status = int.Parse(attackDamage.text) + int.Parse(attackSpeed.text); // 비용 산출 공식 , 나중에 밸런스 지침 정해지면 수정하기
        cost.text = status.ToString();
    }
}
