using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;

public class Shoot : BTAction
{
    private GameObject m_BulletObject;
    private Transform m_shooter;
    private Vector2 m_direct;
    private int m_timeWaitId;

    public Shoot(GameObject bullet, Transform shooter, Vector2 direct, int id =-1, BTPrecondition precondition = null) : base(precondition)
    {
        m_BulletObject = bullet;
        m_shooter = shooter;
        m_direct = direct;
        m_timeWaitId = id;
    }

    protected override void Enter()
    {
        Fire();
    }

    void Fire()
    {
        database.SetData<float>(m_timeWaitId, 0);
        GameObject bullet = GameObject.Instantiate(m_BulletObject, m_shooter.position, Quaternion.identity);
        NormanBulletController control = bullet.GetComponent<NormanBulletController>();
        control.SetDirect(0, -1);
    }
}
