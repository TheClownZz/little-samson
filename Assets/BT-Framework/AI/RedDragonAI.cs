using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;

public class RedDragonAI : BTTree
{
    public GameObject m_bulletObject;
    public Transform m_Shooter;
    public FindPlayer m_Find;
    public float m_Speed = 0.5f;
    public float m_TimeAttackCooldown = 3f;
    public float m_MaxTimeAttack = 0.5f;
    private int m_timeWaitId, m_timeAttackId, m_DirectID;
    private bool m_IsAttack = false;
    public float m_MaxTimeChange = 3.5f;
    private float m_timeChange = 0;
    protected override void Init()
    {
        base.Init();
        float time = 0;
        float timeAttack = m_MaxTimeAttack + 1;
        Vector2 direct = new Vector2(0, 1);

        m_DirectID = database.GetDataId("direct");
        database.SetData<Vector2>(m_DirectID, direct);

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

        BTParallel moveAttack = new BTParallel(BT.BTParallel.ParallelFunction.Or);
        moveAttack.AddChild(new Move(m_Speed, direct, transform.position, 1000f, 0.2f, m_DirectID));

        BTPrioritySelector fight = new BTPrioritySelector();
        fight.AddChild(new PlayAnimation("attack", con2));
        fight.AddChild(new PlayAnimation("fly"));

        moveAttack.AddChild(fight);
        _root.AddChild(moveAttack);
    }
    void UpdateTime(int id)
    {
        float time = database.GetData<float>(id);
        time += Time.deltaTime;
        database.SetData<float>(id, time);
    }

    void ChangeDirect()
    {
        Vector2 direct = database.GetData<Vector2>(m_DirectID);
        direct.y *= -1;
        database.SetData<Vector2>(m_DirectID, direct);
    }
    void UpdateTimeChange()
    {
        m_timeChange += Time.deltaTime;
        if(m_timeChange>=m_MaxTimeChange)
        {
            ChangeDirect();
            m_timeChange -= m_MaxTimeChange;
        }
    }
    protected override void DoUpdate()
    {
        Flip();
        UpdateTimeChange();
        UpdateTime(m_timeWaitId);
        if (database.GetData<float>(m_timeWaitId) >= m_TimeAttackCooldown + 0.5f)
        {
            database.SetData<float>(m_timeWaitId, 0);
        }
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
