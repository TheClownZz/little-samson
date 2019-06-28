using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuaController : MonoBehaviour
{

    public float m_Speed = 1f;

    Rigidbody2D m_r2d;
    void Start()
    {
        m_r2d = GetComponent<Rigidbody2D>();
       
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
            m_r2d.velocity = new Vector2(m_Speed, 0);
    }
}
