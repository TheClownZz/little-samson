using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;

public class EnemyDragonAi : BTTree
{
    public GameObject m_bulletObject;
    public Transform m_Shooter;
    public FindPlayer m_Find;
    public float m_TimeAttackCooldown = 3f;
    public float m_MaxTimeAttack = 0.5f;
    private int m_timeWaitId, m_timeAttackId;

    private bool m_IsAttack = false;
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
        _root.AddChild(new PlayAnimation("fly"));
        
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
    }

    void Fire()
    {
        database.SetData<float>(m_timeWaitId, 0);
        database.SetData<float>(m_timeAttackId, 0);
        m_IsAttack = true;
        Vector2 direct;
        direct.x = m_Find.m_Player.transform.position.x - transform.position.x;
        direct.y = m_Find.m_Player.transform.position.y - transform.position.y;

        direct.Normalize();

        GameObject bullet = Instantiate(m_bulletObject, m_Shooter.position, Quaternion.identity);
        bullet.transform.parent = transform.parent;
        NormanBulletController control = bullet.GetComponent<NormanBulletController>();
        control.SetDirect(direct.x, direct.y);
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
}
