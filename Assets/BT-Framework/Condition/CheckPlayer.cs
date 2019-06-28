using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;
public class CheckPlayer : BTPrecondition
{
    private FindPlayer m_Find;
    private bool m_isFind;
    private bool m_isNotUpdate;
    public CheckPlayer(FindPlayer find, bool isNotUpdate = false)
    {
        m_Find = find;
        m_isFind = false;
        m_isNotUpdate = isNotUpdate;
    }
    public override void Activate(Database database)
    {
        base.Activate(database);
    }
    public override bool Check()
    {
        if (!m_Find)
            return false;
        if(m_isNotUpdate && !m_isFind)
        {
            m_isFind = m_Find.m_Player != null;
        }
        return m_Find.m_Player != null || m_isFind;
    }

}
