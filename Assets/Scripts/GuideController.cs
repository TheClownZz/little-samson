using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideController : MonoBehaviour
{
    public static bool m_isHide = false;
    public GameObject m_guide;
    public GameObject[] listGuide;
   // private bool m_isTouch = false;
    // Start is called before the first frame update

    void Start()
    {
        if (m_isHide)
            GameObject.Destroy(gameObject);
    }

    void Update()
    {
        if (m_isHide)
            GameObject.Destroy(gameObject);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_isHide = true;
                
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            for (int i = 0; i < listGuide.Length; i++)
                listGuide[i].SetActive(false);
            m_guide.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" )
        {
            m_guide.SetActive(false);
        }
    }
}
