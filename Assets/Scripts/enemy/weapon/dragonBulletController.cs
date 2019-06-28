using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dragonBulletController : MonoBehaviour
{
    public float m_TimeAlive = 3f;
    public float m_speed = 2f;
    private float dx = -1, dy = 0;
    private float m_Time = 0;


    public void SetRotation(float r)
    {
        transform.rotation = Quaternion.Euler(0, 0, r);
    }
    public void SetDirect(float x, float y, float scale)
    {
        dx = x;
        dy = y;

        if(x<0)
            transform.localScale = new Vector3(-scale, scale, 1);   
        else
            transform.localScale = new Vector3(scale, scale, 1);

    }



    // Update is called once per frame
    void Update()
    {
        m_Time += Time.deltaTime;
        if (m_Time >= m_TimeAlive)
            GameObject.Destroy(gameObject);
        transform.position = new Vector3(transform.position.x + dx * m_speed * Time.deltaTime, transform.position.y + dy * m_speed * Time.deltaTime, transform.position.z);
    }
}
