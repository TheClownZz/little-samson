using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullBossController : MonoBehaviour
{
    public SKullAI m_SKull;
    public GameObject m_SKullBullet;
    public Transform m_LT, m_RT, m_LB, m_RB;
    public CircleCollider2D m_Center;

    public bool m_AllowApear = true;
    void Start()
    {
        
    }

    void SkullApear()
    {
        GameObject bullet;
        SkullbulletController control;

        bullet = Instantiate(m_SKullBullet, m_LT.position, Quaternion.identity);
        control = bullet.GetComponent<SkullbulletController>();
        if(control)
            control.m_Direct = new Vector2(1, -1);
        //
        bullet = Instantiate(m_SKullBullet, m_LB.position, Quaternion.identity);
        control = bullet.GetComponent<SkullbulletController>();
        if (control)
            control.m_Direct = new Vector2(1, 1);
        //
        bullet = Instantiate(m_SKullBullet, m_RT.position, Quaternion.identity);
        control = bullet.GetComponent<SkullbulletController>();
        if (control)
            control.m_Direct = new Vector2(-1, -1);
        //
        bullet = Instantiate(m_SKullBullet, m_RB.position, Quaternion.identity);
        control = bullet.GetComponent<SkullbulletController>();
        if (control)
            control.m_Direct = new Vector2(-1, 1);
    }
    void Update()
    {
        if(m_AllowApear)
        {
            m_AllowApear = false;
            SkullApear();
        }
    }
    public void SetSkullApear()
    {
        m_AllowApear = true;
        m_Center.enabled = true;
    }
    void ShowSkull()
    {
        m_SKull.Appear();
        m_Center.enabled = false;
    }

    float num = 0;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag=="enemyWeapon")
        {
            SkullbulletController control;
            control = other.gameObject.GetComponent<SkullbulletController>();
            if (control)
                control.m_Speed = 0;
            GameObject.Destroy(other.gameObject, 0.5f);
            num++;
            if (num >= 4)
            {
                Invoke("ShowSkull", 0.5f);
                num = 0;
            }
        }
    }
}
