using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;

public class SauAI : BTTree
{
    public FindPlayer m_Find;
    public GameObject bulletObject;
    public Transform distance, shooter;
    public float speed;
    public float timeAttack = 3f;

    private int m_timeWaitId, m_DirectID;

    protected override void Init()
    {
        base.Init();
        m_DirectID = database.GetDataId("direct");
        database.SetData<Vector2>(m_DirectID, new Vector2(1, 0));

        m_timeWaitId = database.GetDataId("timeWait");
        database.SetData<float>(m_timeWaitId, 0);

        CheckPlayer _checkPlayer = new CheckPlayer(m_Find);
        CheckAllowAttack _checkAllowAttack = new CheckAllowAttack(m_timeWaitId, timeAttack);

        BTPrecondition2 con = new BTPrecondition2(_checkPlayer, _checkAllowAttack, BTPrecondition2.BTPrecondition2Param.And);

        _root = new BTPrioritySelector();
        BTParallel moveAttack = new BTParallel(BT.BTParallel.ParallelFunction.Or, con);
        float maxX, maxY;
        maxX = distance.position.x - transform.position.x;
        maxY = 0;
        moveAttack.AddChild(new Move(speed, new Vector2(1, 0), transform.position, maxX, maxY, m_DirectID, null, true));
        moveAttack.AddChild(new Shoot(bulletObject, shooter, new Vector2(0, -1), m_timeWaitId));

        _root.AddChild(moveAttack);
        _root.AddChild(new Move(speed, new Vector2(1, 0), transform.position, maxX, maxY, m_DirectID, null, true));
    }
    void UpdateTime(int id)
    {
        float time = database.GetData<float>(id);
        time += Time.deltaTime;
        database.SetData<float>(id, time);
    }

    void Flip()
    {
        float dx = database.GetData<Vector2>(m_DirectID).x;
        if (dx != 0)
            transform.localScale = new Vector3(dx, 1, 1);
    }

    protected override void DoUpdate()
    {
        Flip();
        UpdateTime(m_timeWaitId);
        if (database.GetData<float>(m_timeWaitId) >= timeAttack + 0.5f)
        {
            database.SetData<float>(m_timeWaitId, 0);
        }
    }
}
