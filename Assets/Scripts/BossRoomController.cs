using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomController : MonoBehaviour
{
    public CameraController m_CamControl;
    public PlayerController m_playerControl;
    public GameObject m_boss;
    public GameObject m_healUi;
    public GameObject m_object;

    public float m_timeAppear = 3f;
    [HideInInspector]
    public bool m_isStart = false;

    private float m_time = 0;
    void Start()
    {
        m_object.gameObject.SetActive(false);
        m_healUi.gameObject.SetActive(false);
    }

    bool BossAppear()
    {
        if (m_time >= m_timeAppear)
        {
            m_boss.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            return true;
        }
        m_object.gameObject.SetActive(false);
        m_healUi.gameObject.SetActive(false);
        SpriteRenderer render = m_boss.GetComponent<SpriteRenderer>();
        Color r = render.color;
        r.a = m_time / m_timeAppear;
        render.color = new Color(1, 1, 1, r.a);
        return false;
    }
    void Update()
    {
        if (!m_isStart)
            return;
        m_time += Time.deltaTime;
        if(BossAppear())
        {
            m_object.gameObject.SetActive(true);
            m_healUi.gameObject.SetActive(true);
            m_playerControl.m_isDisableInput = false;
            if (m_CamControl)
                m_CamControl.enabled = false;
            GameObject.Destroy(this);
        }
    }

}
