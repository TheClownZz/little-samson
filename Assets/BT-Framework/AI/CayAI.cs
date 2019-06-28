using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;

public class CayAI : BTTree
{
    public GameObject m_bulletObject;
    public Transform m_Shooter;
    public FindPlayer m_Find;
    public float m_TimeAttackCooldown = 3f;
    public float m_MaxTimeAttack = 0.7f;
    public float m_distaceBullet = 0.2f;
    private float m_dx, m_dy;
    private bool m_IsAttack = false;
    private int m_timeWaitId, m_timeAttackId;
    protected override void Init()
    {
        base.Init();
        float time = 0;
        float timeAttack = m_MaxTimeAttack + 1;
        m_timeWaitId = database.GetDataId("timeWait");
        database.SetData<float>(m_timeWaitId, time);

        m_timeAttackId = database.GetDataId("timeAttack");
        database.SetData<float>(m_timeAttackId, timeAttack);

        _root = new BTPrioritySelector();
        CheckPlayer _checkPlayer = new CheckPlayer(m_Find);
        CheckAllowAttack _checkAllowAttack = new CheckAllowAttack(m_timeWaitId, m_TimeAttackCooldown);
        CheckAttacking _checkAttacking = new CheckAttacking(m_timeAttackId, m_MaxTimeAttack);

        BTPrecondition2 con1 = new BTPrecondition2(_checkAllowAttack, _checkAttacking, BTPrecondition2.BTPrecondition2Param.Or);
        BTPrecondition2 con2 = new BTPrecondition2(_checkPlayer, con1, BTPrecondition2.BTPrecondition2Param.And);

        _root.AddChild(new PlayAnimation("attack", con2));
        _root.AddChild(new PlayAnimation("stand"));

    }
    void UpdateTime(int id)
    {
        float time = database.GetData<float>(id);
        time += Time.deltaTime;
        database.SetData<float>(id, time);
    }
    protected override void DoUpdate()
    {
        Flip();
        UpdateTime(m_timeWaitId);
        if (database.GetData<float>(m_timeWaitId) >= m_TimeAttackCooldown + 0.5f)
            database.SetData<float>(m_timeWaitId, 0);
        if (m_IsAttack)
        {
            UpdateTime(m_timeAttackId);
            if (database.GetData<float>(m_timeAttackId) >= m_MaxTimeAttack)
                m_IsAttack = false;
        }
        if (m_Find.m_Player)
        {
            m_dx = m_Find.m_Player.transform.position.x - transform.position.x;
            m_dy = m_Find.m_Player.transform.position.y - transform.position.y;
        }
    }
    void Flip()
    {
        if (m_Find.m_Player)
        {
            if (m_Find.m_Player.transform.position.x > transform.position.x)
                transform.localScale = new Vector3(1, 1, 1);
            else
                transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    void Fire(Vector2 direct)
    {
        GameObject bullet = Instantiate(m_bulletObject, m_Shooter.position, Quaternion.identity);
        bullet.transform.parent = transform.parent;
        NormanBulletController control = bullet.GetComponent<NormanBulletController>();
        if (control)
        {
            control.SetDirect(direct.x, direct.y);
        }
    }
    void Attack()
    {
        m_IsAttack = true;
        database.SetData<float>(m_timeWaitId, 0);
        database.SetData<float>(m_timeAttackId, 0);
    }
    void Shoot()
    {
        Vector2 direct = new Vector2(m_dx, m_dy);
        direct.Normalize();
        Fire(direct);
        //

        Vector2 top = direct;
        top.y += m_distaceBullet;
        Fire(top);

        Vector2 bot = direct;
        bot.y -= m_distaceBullet;
        Fire(bot);

    }

}
