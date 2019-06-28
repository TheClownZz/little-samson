using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update
    // Update is called once per frame
    private bool m_isReady = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !m_isReady)
        {
            m_isReady = true;
            Rigidbody2D r2d = GetComponent<Rigidbody2D>();
            r2d.velocity = new Vector2(1, 0);
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag=="gate")
        {
            Rigidbody2D r2d = GetComponent<Rigidbody2D>();
            r2d.velocity = new Vector2(0, 0);
        }
    }
}
