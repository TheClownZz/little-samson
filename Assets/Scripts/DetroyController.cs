using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetroyController : MonoBehaviour
{
    public float timeAlive = 0.5f;
    public float m_Time = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_Time += Time.deltaTime;
        if(m_Time>=timeAlive)
        {
            GameObject.Destroy(gameObject);
        }

    }
}
