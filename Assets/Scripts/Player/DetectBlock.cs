using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectBlock : MonoBehaviour
{
    public bool m_Collision = false;


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "block")
            m_Collision = true;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "block")
            m_Collision = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "block")
            m_Collision = false;
    }
}
