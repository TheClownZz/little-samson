using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearEffect : MonoBehaviour
{
    public float speed = 0.01f;
    private float m_Time = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_Time += Time.deltaTime;
        if (m_Time >= 1.5f)
           GameObject.Destroy(gameObject);
        transform.position = new Vector3(transform.position.x, transform.position.y + speed * Time.deltaTime, 0);
            
            
    }
}
