using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;

public class Move : BTAction
{
    private float m_Speed;
    private Vector2 m_Direct;
    private Vector3 m_posStart;
    private float m_MaxDistanceX, m_MaxDistanceY;
    private int m_directID;
    private Transform _trans;
    private bool m_allowChangeDirect;
    public Move(float speed, Vector2 direct, Vector3 posStart, float distanceX, float distanceY, 
        int id =-1, BTPrecondition precondition = null, bool allowChangeDirect = false) : base(precondition)
    {
        m_Speed = speed;
        m_Direct = direct;
        m_posStart = posStart;
        m_MaxDistanceX = distanceX;
        m_MaxDistanceY = distanceY;
        m_directID = id;
        m_allowChangeDirect = allowChangeDirect;
    }

    public override void Activate(Database database)
    {
        base.Activate(database);

        _trans = database.transform;
    }
    protected override BTResult Execute()
    {
        UpdateDirect();
        Moving();
        return BTResult.Running;
    }

    void UpdateDirect()
    {
        if(m_directID!=-1)
            m_Direct = database.GetData<Vector2>(m_directID);
    }
    void Moving()
    {
        float x, y;
        x = _trans.position.x + m_Speed * Time.deltaTime * m_Direct.x;
        if(Mathf.Abs(x-m_posStart.x) > m_MaxDistanceX)
        {
            x = (x - m_posStart.x) / Mathf.Abs(x - m_posStart.x) * m_MaxDistanceX + m_posStart.x;
            if(m_allowChangeDirect)
            {
                Vector2 direct = database.GetData<Vector2>(m_directID);
                direct.x *= -1;
                database.SetData<Vector2>(m_directID, direct);
            }
        }

        y = _trans.position.y + m_Speed * Time.deltaTime * m_Direct.y;
        if (Mathf.Abs(y - m_posStart.y) > m_MaxDistanceY)
        {
            y = (y - m_posStart.y) / Mathf.Abs(y - m_posStart.y) * m_MaxDistanceY + m_posStart.y;
        }
        Vector3 des = new Vector3(x, y, _trans.position.z);
        _trans.position = des;
    }
}
