using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaController : MonoBehaviour
{
    public float m_Speed = 0.35f;
    public FindPlayer m_Find;

    private Animator m_Anim;
    private float m_Direct = 1;
    void Start()
    {
        m_Anim = GetComponent<Animator>();
        m_Anim.SetTrigger("stand");
    }
    void Run()
    {
        if (m_Find.m_Player.transform.position.y > transform.position.y)
        {
            m_Direct = 1;
            transform.localScale = new Vector3(1, -m_Direct, 1);
        }
        else
        {
            m_Direct = -1;
            transform.localScale = new Vector3(1, -m_Direct, 1);
        }

        transform.position = new Vector3(transform.position.x, transform.position.y + m_Direct * m_Speed * Time.deltaTime, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(m_Find.m_Player)
        {
            m_Anim.ResetTrigger("stand");
            m_Anim.SetTrigger("run");
            Run();
        }
        else
        {
            m_Anim.ResetTrigger("run");
            m_Anim.SetTrigger("stand");
        }
    }
}
