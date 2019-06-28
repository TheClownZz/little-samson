using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;

public class DiaAI : BTTree
{
    public float m_Speed = 0.25f;
    public FindPlayer m_Find;
    private int  m_DirectID;

    protected override void Init()
    {
        base.Init();
        m_DirectID = database.GetDataId("direct");
        database.SetData<Vector2>(m_DirectID, new Vector2(0, 0));
        CheckPlayer _checkPlayer = new CheckPlayer(m_Find);
        _root = new BTPrioritySelector();

        BTParallel run = new BTParallel(BTParallel.ParallelFunction.Or,_checkPlayer);
        run.AddChild(new Move(m_Speed, new Vector2(0, 0), transform.position, 999, 999, m_DirectID));
        run.AddChild(new PlayAnimation("run"));
        _root.AddChild(run);
        _root.AddChild(new PlayAnimation("stand"));

    }
    void UpdateDirect()
    {
        if(m_Find.m_Player)
        {
            Vector2 direct = database.GetData<Vector2>(m_DirectID);
            if(m_Find.m_Player.transform.position.y > transform.position.y)
            {
                direct.y = 1;
            }
            else
            {
                direct.y = -1;
            }
            transform.localScale = new Vector3(1, -direct.y, 1);
            database.SetData<Vector2>(m_DirectID, direct);

        }
    }
    protected override void DoUpdate()
    {
        UpdateDirect();
    }
}
