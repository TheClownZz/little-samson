using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;

public class CheckCollision : BTPrecondition
{
    private int m_CheckCollisionID;
    public CheckCollision(int id=-1)
    {
        m_CheckCollisionID = id;
    }

    public override bool Check()
    {
        if (m_CheckCollisionID == -1)
            return false;
        return database.GetData<bool>(m_CheckCollisionID);
    }
}
