using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;

public class RuoiAI : BTTree
{
    public DameController m_Dame;
    public float m_speed;
    public FindPlayer m_Find;
    private int m_DirectID;

    protected override void Init()
    {
        base.Init();
        m_DirectID = database.GetDataId("direct");
        database.SetData<Vector2>(m_DirectID, new Vector2(0, 0));
        CheckPlayer _checkPlayer = new CheckPlayer(m_Find, true);
        _root = new BTPrioritySelector();
        _root.AddChild(new Move(m_speed, new Vector2(0, 0), transform.position, 999, 999, m_DirectID, _checkPlayer));

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            m_Dame.Dead();
    }
    protected override void DoUpdate()
    {
        if (m_Find.m_Player)
        {
            Vector2 direct = database.GetData<Vector2>(m_DirectID);
            if (m_Find.m_Player.transform.position.x > transform.position.x)
            {
                direct.x = 1;
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
                direct.x = -1;
            }

            if (m_Find.m_Player.transform.position.y > transform.position.y)
                direct.y = 1;
            else
                direct.y = -1;
            database.SetData<Vector2>(m_DirectID, direct);
        }
    }
}
