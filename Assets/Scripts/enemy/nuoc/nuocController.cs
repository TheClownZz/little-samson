using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nuocController : MonoBehaviour
{

    public GameObject m_bulletObject;
    public FindPlayer m_Find;
    private Animator m_Anim;
    private Rigidbody2D m_r2d;
    void Start()
    {
        m_Anim = GetComponent<Animator>();
        m_r2d = GetComponent<Rigidbody2D>();
        m_Anim.SetTrigger("hang");
    }

    // Update is called once per frame

    void Fall()
    {
        m_r2d.gravityScale = 2f;
    }
    void Update()
    {
        if (m_Find.m_Player && m_r2d.gravityScale == 0)
        {
            Fall();
            m_Anim.ResetTrigger("hang");
            m_Anim.SetTrigger("fall");
        }
    }

    void SelfDestruct()
    {
        GameObject bullet = Instantiate(m_bulletObject, transform.position, Quaternion.identity);
        bullet.transform.parent = transform.parent;
        NormanBulletController control = bullet.GetComponent<NormanBulletController>();
        if (control)
            control.SetDirect(1, 0);
        //

        bullet = Instantiate(m_bulletObject, transform.position, Quaternion.identity);
        bullet.transform.parent = transform.parent;
        control = bullet.GetComponent<NormanBulletController>();
        if (control)
            control.SetDirect(-1, 0);
        GameObject.Destroy(gameObject);

    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if ((other.gameObject.tag == "ground" || other.gameObject.tag == "dead") && m_r2d.gravityScale != 0)
        {
            m_Anim.ResetTrigger("fall");
            m_Anim.ResetTrigger("hang");
            m_Anim.SetTrigger("explosion");
            Invoke("SelfDestruct", 0.5f);
        }
    }
}
