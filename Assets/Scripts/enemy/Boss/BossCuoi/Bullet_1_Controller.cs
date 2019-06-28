using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_1_Controller : MonoBehaviour
{
    //  [HideInInspector]
    public float m_maxTime = 4f;
    public bool m_AllowShoot = false;
    public float m_speed = 0.5f;
    private float m_time = 0;

    Transform m_Player;
    void Start()
    {
        if(GameObject.FindGameObjectWithTag("Player"))
            m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        m_time += Time.deltaTime;
        if (m_time >= m_maxTime || !m_Player)
            GameObject.Destroy(gameObject);
        if(m_AllowShoot )
        {
            Vector2 direct = (Vector2)(m_Player.position - transform.position).normalized;
            transform.position = new Vector3(transform.position.x + m_speed * direct.x * Time.deltaTime,
                transform.position.y + m_speed * direct.y * Time.deltaTime, transform.position.z);
        }
        float scale = Mathf.Lerp(0, 1, m_time / 0.35f);
        transform.localScale = new Vector3(scale, scale, 1);
    }

    void TriggerEnter2D(Collider2D other)
    {
        if(other.tag=="Player")
            GameObject.Destroy(gameObject);
    }
}
