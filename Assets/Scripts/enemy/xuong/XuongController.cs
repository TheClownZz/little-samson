using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XuongController : MonoBehaviour
{
    enum state
    {
        stand,run,attack
    }
    public float m_Speed = 1.5f;
    public FindPlayer m_Find;
    public FindPlayer m_FIndAttack;

    public float m_TimeAttack = 1.5f;

    private float m_Time;
    private Animator m_Anim;
    private Rigidbody2D m_r2d;
    private state m_State = state.stand;
    void Start()
    {
        m_Anim = GetComponent<Animator>();
        m_r2d = GetComponent<Rigidbody2D>();
        m_Time = m_TimeAttack;
    }


    void UpdateState()
    {
        m_Anim.ResetTrigger(m_State.ToString());

        switch(m_State)
        {
            case state.stand:
                if(m_FIndAttack.m_Player)
                {
                    if (m_Time > m_TimeAttack)
                    {
                        m_Time = 0;
                        m_State = state.attack;
                    }
                    break;
                }
                if (m_Find.m_Player)
                {
                    m_State = state.run;
                } 
                break;
            case state.run:
                if(m_FIndAttack.m_Player)
                {
                    if (m_Time > m_TimeAttack)
                    {
                        m_Time = 0;
                        m_State = state.attack;
                        break;
                    }
                    m_State = state.stand;
                    break;
                }

                if (!m_Find.m_Player)
                {
                    m_State = state.stand;
                }
                break;
            case state.attack:
                if(m_Time>0.5f)
                {
                    if(!m_FIndAttack.m_Player && m_Find.m_Player)
                    {
                        m_State = state.run;
                        break;
                    }
                    m_State = state.stand;
                }
                break;
            default:break;
        }

        m_Anim.SetTrigger(m_State.ToString());
    }
    // Update is called once per frame

    void Flip()
    {
        if(m_Find.m_Player)
        {
            if (m_Find.m_Player.transform.position.x > transform.position.x)
                transform.localScale = new Vector3(1, 1, 1);
            else
                transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void Update()
    {
        m_Time += Time.deltaTime;
        Flip();
        UpdateState();
        if (m_State == state.run)
            m_r2d.velocity = new Vector2(m_Speed * transform.localScale.x, 0);
        else
            m_r2d.velocity = new Vector2(0, 0);
    }
}
