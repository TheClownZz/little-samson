using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;

public class CheckAttacking : BTPrecondition
{
    private int m_TimeAttackId;
    private float m_MaxTimeAttack;

    public CheckAttacking( int timeAttackID, float maxTimeAttack)
    {
        m_TimeAttackId = timeAttackID;
        m_MaxTimeAttack = maxTimeAttack;
    }
    public override bool Check()
    {
        return database.GetData<float>(m_TimeAttackId) < m_MaxTimeAttack;
    }
}
