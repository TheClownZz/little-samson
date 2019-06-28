using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SauController : MonoBehaviour
{
    public FindPlayer m_Find;
    public GameObject bulletObject;
    public Transform min, max, shooter;
    public float speed;
    public float timeAttack = 3f;

    float time = 0;

    int direct = 1;

    void Start()
    {
        
    }

    void Move()
    {
        if (transform.position.x > max.position.x)
        {
            direct = -1;
            transform.localScale = new Vector3(direct, 1, 1);
        }
        else if (transform.position.x < min.position.x)
        {
            direct = 1;
            transform.localScale = new Vector3(direct, 1, 1);

        }
        transform.position = new Vector3(transform.position.x + direct * speed * Time.deltaTime, transform.position.y, 0);
    }

    void Attack()
    {
        GameObject bullet = Instantiate(bulletObject, shooter.position, Quaternion.identity);
        NormanBulletController control = bullet.GetComponent<NormanBulletController>();
        control.SetDirect(0, -1);
    }
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= timeAttack)
        {
            time -= timeAttack;
            if (m_Find.m_Player)
            {
                Attack();
            }
        }
        Move();
    }
}
