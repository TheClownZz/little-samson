using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonGunContoller : MonoBehaviour
{
    public Transform m_shooter_1, m_shooter_2;
    public GameObject m_bulletObject;
    private float m_time = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_time += Time.deltaTime;
        if(m_time>=3)
        {
            m_time -= 3;
            GameObject bullet = Instantiate(m_bulletObject, m_shooter_1.position, Quaternion.identity);
            bullet.transform.parent = transform.parent;
            dragonBulletController control = bullet.GetComponent<dragonBulletController>();
            if(control)
            {
                control.SetDirect(-1, 0, 1);
                control.m_speed = 1.5f;
            }


            bullet = Instantiate(m_bulletObject, m_shooter_2.position, Quaternion.identity);
            bullet.transform.parent = transform.parent;
            control = bullet.GetComponent<dragonBulletController>();
            if (control)
            {
                control.SetDirect(-1, 0, 1);
                control.m_speed = 1.5f;
            }
        }
    }
}
