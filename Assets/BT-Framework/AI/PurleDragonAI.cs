using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;

public class PurleDragonAI : BTTree
{
    public FindPlayer m_Find;
    public float m_Speed = 2f;
    protected override void Init()
    {
        base.Init();
        CheckPlayer _checkPlayer = new CheckPlayer(m_Find, true);
        _root = new BTPrioritySelector();
        BTPrioritySelector selector = new BTPrioritySelector();
        selector.AddChild(new Move(m_Speed, new Vector2(-1, 0), transform.position, 10, 0, -1, _checkPlayer));
        selector.AddChild(new PlayAnimation("fly"));
        _root.AddChild(selector);
    }
}
