using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BT;

public class NuocAI : BTTree
{
    public GameObject m_bulletObject;
    public FindPlayer m_Find;
    private Rigidbody2D m_r2d;
    private int m_CheckCollisionId;
    protected override void Init()
    {
        base.Init();
        m_r2d = GetComponent<Rigidbody2D>();
        m_CheckCollisionId = database.GetDataId("checkCollision");
        database.SetData<bool>(m_CheckCollisionId, false);

        CheckPlayer _checkPlayer = new CheckPlayer(m_Find, true);
        CheckCollision _checkCollision = new CheckCollision(m_CheckCollisionId);

        _root = new BTPrioritySelector();
        _root.AddChild(new PlayAnimation("exploision", _checkCollision));
        _root.AddChild(new PlayAnimation("fall", _checkPlayer));
        _root.AddChild(new PlayAnimation("hang"));
    }

    void Fall()
    {
        m_r2d.gravityScale = 2f;
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
            database.SetData<bool>(m_CheckCollisionId, true);
        }
    }
}
