using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RongDatController : MonoBehaviour
{
    public FindPlayer m_Find;
    public float m_MaxTimeAttack = 1f;
    public float m_MaxTimeHide = 4f;
    public float m_Speed = 1f;
    public GameObject m_Effect;
    public GameObject m_BulletObject;
    public Transform pos1, pos2, pos3, posMixY, posMaxY;
    public Transform m_shooter;

    private DameController m_DameControl;
    bool m_AllowHide = false;
    bool m_isHide = false;
    bool m_isAttack = false;
    bool m_AllowAttack = true;
    float m_TimeAttack = 0;
    float m_TimeHide = 0;
    int m_maxNumAttack = 1;
    int m_numAttack = 0;

    Animator m_Anim;
    // Start is called before the first frame update
    void Start()
    {
        m_Anim = GetComponent<Animator>();
        m_DameControl = GetComponent<DameController>();
    }

    void UpdateTime()
    {
        if (m_isHide)
            m_TimeHide += Time.deltaTime;
        if (m_AllowAttack)
            m_TimeAttack += Time.deltaTime;
    }

    void Flip()
    {
        if (!m_Find.m_Player)
            return;
        if (m_Find.m_Player.transform.position.x > transform.position.x)
            transform.localScale = new Vector3(-0.75f, 0.75f, 1);
        else
            transform.localScale = new Vector3(0.75f, 0.75f, 1);
    }

    void Fire()
    {
        GameObject bullet = Instantiate(m_BulletObject, m_shooter.position, Quaternion.identity);
        NormanBulletController control = bullet.GetComponent<NormanBulletController>();
        if (control)
            control.SetDirect(-transform.localScale.x, 0);

        bullet = Instantiate(m_BulletObject, m_shooter.position, Quaternion.identity);
        control = bullet.GetComponent<NormanBulletController>();
        if (control)
            control.SetDirect(-transform.localScale.x*0.8f , 0.15f);

        bullet = Instantiate(m_BulletObject, m_shooter.position, Quaternion.identity);
        control = bullet.GetComponent<NormanBulletController>();
        if (control)
            control.SetDirect(-transform.localScale.x *0.8f, -0.15f);

    }

    void Hide()
    {
        if(m_AllowHide)
        {
            GoDown();
        }

        if (m_isHide)
            GoUp();
    }


    void GoUp()
    {
        if(m_TimeHide>=m_MaxTimeHide)
            transform.position = new Vector3(transform.position.x, transform.position.y + m_Speed * Time.deltaTime, 0);

        if (transform.position.y > posMaxY.position.y)
        {
            transform.position = new Vector3(transform.position.x, posMaxY.position.y, 0);
            m_isHide = false;
            m_AllowAttack = true;
            m_TimeHide = 0;
        }
    }
    void GoDown()
    {
        if (transform.position.y > posMixY.position.y)
            transform.position = new Vector3(transform.position.x, transform.position.y - m_Speed * Time.deltaTime, 0);
        else
        {
            transform.position = new Vector3(transform.position.x, posMixY.position.y, 0);
            m_isHide = true;
            m_AllowHide = false;

            int r = Random.Range(0, 100);
            switch(r%3)
            {
                case 0:
                    Instantiate(m_Effect, pos1.position, Quaternion.identity);
                    transform.position = new Vector3(pos1.position.x, transform.position.y, 0);
                    break;
                case 1:
                    Instantiate(m_Effect, pos2.position, Quaternion.identity);
                    transform.position = new Vector3(pos2.position.x, transform.position.y, 0);
                    break;
                default:
                    Instantiate(m_Effect, pos3.position, Quaternion.identity);
                    transform.position = new Vector3(pos3.position.x, transform.position.y, 0);
                    break;
                    
            }
        }
    }

    void Attack()
    {
        if (m_TimeAttack >= m_MaxTimeAttack && m_Find.m_Player)
        {
            m_isAttack = true;
            m_TimeAttack = 0;
            m_Anim.ResetTrigger("stand");
            m_Anim.SetTrigger("attack");
        }

        if(m_TimeAttack >0.6 && m_isAttack)
        {
            Fire();
            m_isAttack = false;
            m_TimeAttack = 0;
            m_Anim.ResetTrigger("attack");
            m_Anim.SetTrigger("stand");
            m_numAttack++;
            if (m_numAttack >= m_maxNumAttack)
            {
                m_numAttack = 0;
                m_AllowHide = true;
                m_AllowAttack = false;
            }
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (m_DameControl.m_Heal <= 0)
            return;
        if (m_DameControl.m_Heal <= m_DameControl.m_MaxHeal / 2)
        {
            m_MaxTimeAttack = 0.65f;
            m_maxNumAttack = 2;
        }
        if (m_DameControl.m_Heal <= 2)
        {
            m_maxNumAttack = 3;
        }

        UpdateTime();
        Flip();
        Attack();
        Hide();

    }
}
