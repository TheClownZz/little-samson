using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCuoiController : MonoBehaviour
{
    public GameObject m_armor;
    public DameController m_dame;
    public FindPlayer m_find;
    public float m_speed = 1f;
    public float m_timeAttack = 3f;
    public GameObject m_bulletObject_1;
    public GameObject m_bulletObject_2;
    public GameObject m_effect;
    public Transform m_shooter_1, m_shooter_2;
    public Transform m_pos1, m_pos2;
    private GameObject m_currentBullet;
    private List<GameObject> m_list;
    private Rigidbody2D m_r2d;
    private Animator m_anim;
    private float m_time = 2;
    private bool m_isAttack = false;
    public float m_scale;

    void Start()
    {
        m_r2d = GetComponent<Rigidbody2D>();
        m_anim = GetComponent<Animator>();
        m_anim.SetTrigger("stand");
        m_list = new List<GameObject>();

    }

    void RemoveList()
    {
        for (int i = 0; i < m_list.Count; i++)
            m_list.RemoveAt(i);
    }

    void Attack_2()
    {
        if (m_dame.m_Heal > m_dame.m_MaxHeal / 2)
            return;
        RemoveList();
        GameObject effect = Instantiate(m_effect, m_shooter_2.position, Quaternion.identity);
        GameObject.Destroy(effect, 0.2f);
        Invoke("Shoot_2", 0.25f);
        Invoke("Shoot_2", 0.5f);
        Invoke("Shoot_2", 0.75f);
        
    }
    void Shoot_2()
    {
        GameObject bullet= Instantiate(m_bulletObject_2, m_shooter_2.position, Quaternion.identity);
        m_list.Add(bullet);
    }

    void Shoot_1()
    {
        m_currentBullet = Instantiate(m_bulletObject_1, m_shooter_1.position, Quaternion.identity);
    }

    void Attack_1()
    {
        if (m_currentBullet)
            m_currentBullet.GetComponent<Bullet_1_Controller>().m_AllowShoot = true;
    }


    void Flip()
    {
        if (m_find.m_Player.transform.position.x > transform.position.x)
            transform.localScale = new Vector3(-m_scale, m_scale, 1);
        else
            transform.localScale = new Vector3(m_scale, m_scale, 1);
    }

    void ResetAnim()
    {
        m_anim.ResetTrigger("stand");
        m_anim.ResetTrigger("run");
        m_anim.ResetTrigger("attack");
    }
    // Update is called once per frame
    public int num = 0;
    void Update()
    {
        if (!m_find.m_Player || m_dame.m_Heal <= 0)
        {
            if(m_currentBullet)
                GameObject.Destroy(m_currentBullet);
            for(int i=0;i<m_list.Count;i++)
            {
                GameObject bullet = m_list[i];
                m_list.RemoveAt(i);
                GameObject.Destroy(bullet);
            }
            ResetAnim();
            m_anim.SetTrigger("stand");
            return;
        }
        Flip();
        m_armor.transform.position = transform.position;
        m_time += Time.deltaTime;
        if (m_isAttack && m_time >= 1f)
        {
            m_isAttack = false;
            ResetAnim();
            m_anim.SetTrigger("stand");
            m_time = 0;
        }

        if (m_time >= m_timeAttack)
        {
            ResetAnim();
            m_anim.SetTrigger("attack");
            m_isAttack = true;
            m_time = 0;
            num++;
        }

        if (!m_isAttack && num >= 2)
        {
            float distance = (m_find.m_Player.transform.position.x - transform.position.x);
            float speed = (distance > 0 ? m_speed : -m_speed);
            if ((transform.position.x > m_pos2.position.x && speed > 0)
                || (transform.position.x < m_pos1.position.x && speed < 0) || Mathf.Abs(distance) < 0.35f)
            {
                ResetAnim();
                m_anim.SetTrigger("stand");
                m_r2d.velocity = new Vector2(0, 0);
                num = 0;
            }
            else
            {
                ResetAnim();
                m_anim.SetTrigger("run");
                m_r2d.velocity = new Vector2(speed, 0);
            }
        }
    }
}
