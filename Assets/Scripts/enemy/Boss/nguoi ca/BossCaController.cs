using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCaController : MonoBehaviour
{

    public GameObject m_buttletObject;
    public Transform m_posLeft, m_posRight, m_posHide;
    public Transform m_ShooterLeft, m_ShooterRight;
    public FindPlayer m_Find;
    public float m_MaxTimeHide = 4f;
    public float m_Speed = 1f;
    public float m_MaxTimeAttack = 3.25f;

    bool m_AllowHide = false;
    bool m_isHide = false;
    float m_TimeHide = 0;
    public float  m_TimeAttack = 0;
    bool m_AllowAttack = true;
    bool m_isAttack = false;

    Transform m_currentPos;
    private GameObject m_bullet_1, m_bullet_2;
    private DameController m_DameControl;

    // Start is called before the first frame update
    void Start()
    {
        m_currentPos = m_posRight;
        m_DameControl = GetComponent<DameController>();
    }

    void UpdateTime()
    {
        if (m_isHide)
            m_TimeHide += Time.deltaTime;

        if (m_isAttack)
            m_TimeAttack += Time.deltaTime;
    }

    void Hide()
    {
        if (m_AllowHide)
        {
            GoDown();
        }

        if (m_isHide)
            GoUp();
    }

    void GoUp()
    {
        if (m_TimeHide >= m_MaxTimeHide)
            transform.position = new Vector3(transform.position.x, transform.position.y + m_Speed * Time.deltaTime, 0);
        if (transform.position.y > m_currentPos.position.y)
        {
            transform.position = new Vector3(transform.position.x, m_currentPos.position.y, 0);
            m_isHide = false;
            m_TimeHide = 0;
            m_AllowAttack = true;
        }
    }
    void GoDown()
    {
        if (transform.position.y > m_posHide.position.y)
            transform.position = new Vector3(transform.position.x, transform.position.y - m_Speed * Time.deltaTime, 0);
        else
        {
            {
                if (m_currentPos == m_posRight)
                    m_currentPos = m_posLeft;
                else
                    m_currentPos = m_posRight;
            }
            transform.position = new Vector3(m_currentPos.position.x, m_posHide.position.y, 0);
            m_isHide = true;
            m_AllowHide = false;

        }
    }
    void Flip()
    {
        if (!m_Find.m_Player)
            return;
        if (m_Find.m_Player.transform.position.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }
    void Attack()
    {
        if (m_TimeAttack >= m_MaxTimeAttack)
        {
            m_TimeAttack = 0;
            m_AllowHide = true;
            m_isAttack = false;
            return;
        }
        if (!m_AllowAttack || !m_Find.m_Player)
            return;
        if (!m_isAttack)
        {
            m_AllowAttack = false;
            m_isAttack = true;
            if (m_DameControl.m_Heal <= m_DameControl.m_MaxHeal / 2)
            {
                m_bullet_1 = Instantiate(m_buttletObject, m_ShooterRight.position, Quaternion.identity);
                m_bullet_1.transform.parent = transform.parent;
                BossCaBulletController control = m_bullet_1.GetComponent<BossCaBulletController>();
                control.SetScale(2f);
                control.SetDirect((int)transform.localScale.x);
                control.SetSpeed(2.5f);

                m_bullet_2 = Instantiate(m_buttletObject, m_ShooterLeft.position, Quaternion.identity);
                m_bullet_2.transform.parent = transform.parent;
                control = m_bullet_2.GetComponent<BossCaBulletController>();
                control.SetScale(2f);
                control.SetDirect((int)transform.localScale.x);
                control.SetSpeed(2.5f);

                return;
            }

            {
                m_bullet_1 = Instantiate(m_buttletObject, m_ShooterRight.position, Quaternion.identity);
                m_bullet_1.transform.parent = transform.parent;
                BossCaBulletController control = m_bullet_1.GetComponent<BossCaBulletController>();
                control.SetScale(2f);
                control.SetDirect((int)transform.localScale.x);
                control.SetSpeed(2);
                return;
            }

        }

    }
    // Update is called once per frame
    void Update()
    {
        if (m_DameControl.m_Heal <= 0)
        {
            if(m_bullet_1)
                GameObject.Destroy(m_bullet_1);
            if(m_bullet_2)
                GameObject.Destroy(m_bullet_2);
            return;
        }
        Flip();
        UpdateTime();
        Attack();
        Hide();
    }
}
