using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_2_Controller : MonoBehaviour
{
    public float m_speed = 1f;
    public float m_maxTime = 3f;
    private Vector2 direct = new Vector2(0, 0);
    private Transform m_player;
    private float m_time = 0;
    void Start()
    {
        if(GameObject.FindGameObjectWithTag("Player"))
            m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        if(m_player)
        {
            direct = (m_player.position - transform.position).normalized;
            float rotation = Mathf.Rad2Deg * Mathf.Atan2(direct.y, direct.x) + 180;
            transform.rotation = Quaternion.Euler(0, 0, rotation);

        }
    }

    // Update is called once per frame
    void Update()
    {
        m_time += Time.deltaTime;
        if (m_time > m_maxTime || !m_player)
            GameObject.Destroy(gameObject);
        transform.position = new Vector3(transform.position.x + m_speed * direct.x * Time.deltaTime,
                        transform.position.y + m_speed * direct.y * Time.deltaTime, transform.position.z);
    }

    void TriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            GameObject.Destroy(gameObject);
    }
}
