using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;

public class CheckAllowAttack : BTPrecondition
{
    private int m_timeWaitID;
    private float m_TimeAttack;
    public CheckAllowAttack(int timeWaitID, float timeAttack)
    {
        m_timeWaitID = timeWaitID;
        m_TimeAttack = timeAttack;
    }

    public override bool Check()
    {
        if(database.GetData<float>(m_timeWaitID) >= m_TimeAttack)
        {
            return true;
        }
        return false;
    }
}
