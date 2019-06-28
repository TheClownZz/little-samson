using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float dampTime = 0.15f;
    public bool m_FreezeX = false;
    public bool m_FreezeY = false;
    public float m_TimeChange = 2f;
    private Vector3 velocity = Vector3.zero;
    public Transform target;

   // [HideInInspector]
    public bool m_isChange = false;
    [HideInInspector]
    public bool m_isFindPos = true;
    private float m_Time = 5;
    private  float top, bot, left, right;
    private Vector3 m_StartPos, m_EndPos;
    void Start()
    {
        top = right = float.MaxValue;
        bot = left = float.MinValue;
        m_StartPos = new Vector3();
        m_EndPos = new Vector3();
    }


    void Update()
    {
        if (m_isChange)
        {
            if(!m_isFindPos)
            {
                m_isFindPos = true;
                m_StartPos = transform.position;
            }
            m_Time += Time.deltaTime;
            if (m_Time>=m_TimeChange)
            {
                m_isChange = false;
                m_Time = 0;
            }
        }
    }
    void FixedUpdate()
    {
        if (target)
        {
            Vector3 point = Camera.main.WorldToViewportPoint(target.position);
            Vector3 delta = target.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;

            Vector3 newPos = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
            float x, y, h, w;
            h = 2f * Camera.main.orthographicSize;
            w = h * Camera.main.aspect;
            if (!m_FreezeX)
            {
                if (newPos.x + w / 2 > right)
                    x = right - w / 2;
                else if (newPos.x - w / 2 < left)
                    x = left + w / 2;
                else
                    x = newPos.x;
            }
            else
                x = transform.position.x;

            if (!m_FreezeY)
            {
                if (newPos.y + h / 2 > top)
                    y = top - h / 2;
                else if (newPos.y - h / 2 < bot)
                    y = bot + h / 2;
                else
                    y = newPos.y;
            }
            else
                y = transform.position.y;
            if(m_isChange)
            {
                m_EndPos = new Vector3(x, y, transform.position.z);
                transform.position = Vector3.Lerp(m_StartPos, m_EndPos, m_Time / m_TimeChange);
            }
            else
                transform.position = new Vector3(x, y, transform.position.z);
        }

    }

   public void SetLimit()
    {
        PlayerController control = target.gameObject.GetComponent<PlayerController>();
        if(control.m_CurrentRoom)
        {
            Bounds _bound = control.m_CurrentRoom.bounds;
            top = _bound.max.y;
            bot = _bound.min.y;
            left = _bound.min.x;
            right = _bound.max.x;       
        }
        
    }

}
