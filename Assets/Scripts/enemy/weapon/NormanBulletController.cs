using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormanBulletController : MonoBehaviour
{
    // Start is called before the first frame update

    public float m_TimeAlive = 3f;
    public float m_speedRotation = 100f;
    public float m_speed = 1f;
    private float r_z = 0;
    private float dx = 1, dy = 0;
    private float m_Time = 0;
    // Update is called once per frame

    public void SetDirect(float x,float y)
    {
        dx = x;
        dy = y;
    }

    public void SetSpeed(float speed)
    {
        m_speed = speed;
    }
    void Update()
    {
        m_Time += Time.deltaTime;
        if (m_Time >= m_TimeAlive)
            GameObject.Destroy(gameObject);
        r_z += Time.deltaTime* m_speedRotation;
        if (r_z >= 360f)
            r_z -= 360f;
        transform.rotation = Quaternion.Euler(0, 0, r_z);
        transform.position = new Vector3(transform.position.x + dx * m_speed*Time.deltaTime, transform.position.y + dy * m_speed*Time.deltaTime, transform.position.z);
    }

    void  OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "MainCamera")
            GameObject.Destroy(gameObject);
    }

}
