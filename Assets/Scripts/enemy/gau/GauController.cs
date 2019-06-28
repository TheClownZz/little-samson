using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GauController : MonoBehaviour
{
    public GameObject m_bulletObject;
    public FindPlayer m_Find;
    public float m_powerJump = 180f;
    public float m_timeAttack = 4f;

    private Animator m_Anim;
    private Rigidbody2D m_R2d;
    private float m_time = 0;
    private bool m_isAttack = false;
    private bool m_AllowAttack = false;
    void Start()
    {
        m_Anim = GetComponent<Animator>();
        m_R2d = GetComponent<Rigidbody2D>();
        m_Anim.SetTrigger("stand");
    }

    // Update is called once per frame

    void Flip()
    {
        if (m_Find.m_Player)
        {
            if (m_Find.m_Player.transform.position.x > transform.position.x)
                transform.localScale = new Vector3(1, 1, 1);
            else
                transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void Attack()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject bullet = Instantiate(m_bulletObject, transform.position, Quaternion.identity);
            bullet.transform.parent = transform.parent;
            NormanBulletController control = bullet.GetComponent<NormanBulletController>();

            switch (i)
            {
                case 0:
                    control.SetDirect(1, 0);
                    break;
                case 1:
                    control.SetDirect(1, 1);
                    break;
                case 2:
                    control.SetDirect(-1, 1);
                    break;
                default:
                    control.SetDirect(-1, 0);
                    break;
            }
        }

    }
    void Update()
    {
        Flip();
        m_time += Time.deltaTime;
        if(m_AllowAttack && m_time>0.75f)
        {
            Attack();
            m_AllowAttack = false;
        }
        if(m_isAttack && m_time>1.5f)
        {
            m_Anim.ResetTrigger("attack");
            m_Anim.SetTrigger("stand");
            m_isAttack = false;
        }

        if (m_time >= m_timeAttack && m_Find.m_Player)
        {
            m_isAttack = true;
            m_AllowAttack = true;
            m_time = 0;
            m_Anim.ResetTrigger("stand");
            m_Anim.SetTrigger("attack");
            m_R2d.AddForce(new Vector2(0, m_powerJump));
        }
       
    }
}
