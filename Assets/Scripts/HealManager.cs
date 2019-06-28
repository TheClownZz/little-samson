using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealManager : MonoBehaviour
{
    public int num;
    public GameObject[] m_ListHealUI;

    public void Innit()
    {
        m_ListHealUI = new GameObject[num];
        for(int i=0;i<transform.childCount;i++)
        {
            m_ListHealUI[i] = transform.GetChild(i).gameObject;
        }
    }
    public void DrawUI(int maxHeal,int currentHeal)
    {
        if (maxHeal > num)
            maxHeal = num;
        if (currentHeal > maxHeal)
            currentHeal = maxHeal;
        if (currentHeal < 0)
            currentHeal = 0;
        for(int i=0;i< currentHeal; i++)
        {
            SpriteRenderer[] list = m_ListHealUI[i].GetComponentsInChildren<SpriteRenderer>();
            list[0].enabled = true;
            list[1].enabled = true;
        }

        for (int i = currentHeal; i < maxHeal; i++)
        {
            SpriteRenderer[] list = m_ListHealUI[i].GetComponentsInChildren<SpriteRenderer>();
            list[0].enabled = false;
            list[1].enabled = true;
        }

        for(int i= maxHeal;i<m_ListHealUI.Length;i++)
        {
            SpriteRenderer[] list = m_ListHealUI[i].GetComponentsInChildren<SpriteRenderer>();
            list[0].enabled = false;
            list[1].enabled = false;
        }
    }

}
