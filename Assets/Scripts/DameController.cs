using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DameController : MonoBehaviour
{
    [HideInInspector]
    public enum type
    {
        player, enemy, boss, none
    }

    public GameObject m_DieEffect;
    public type m_Type = type.none;
    public int m_Heal = 1;
    public int m_MaxHeal = 1;
    public int m_Dame = 1;


    private PlayerController m_PlayerControl;
    private BossController m_BossControl;
    private SoundController m_soundControl;

    void Start()
    {
        if (m_Type == type.player)
        {
            m_MaxHeal = GameController.m_MaxHealPlayer;
            m_Heal = m_MaxHeal;
            m_PlayerControl = GetComponentInParent<PlayerController>();
            if (m_PlayerControl.m_HealManager)
            {
                m_PlayerControl.m_HealManager.Innit();
                m_PlayerControl.m_HealManager.DrawUI(m_MaxHeal, m_Heal);
            }

        }

        if (m_Type == type.boss)
        {
            m_BossControl = GetComponent<BossController>();
            if (m_BossControl.m_HealManager)
            {
                m_BossControl.m_HealManager.Innit();
                m_BossControl.m_HealManager.DrawUI(m_MaxHeal, m_Heal);
            }
        }

        GameObject soundManager = GameObject.FindGameObjectWithTag("sound");
        if (soundManager)
        {
            m_soundControl = soundManager.GetComponent<SoundController>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Dead()
    {
        if (m_DieEffect)
            Instantiate(m_DieEffect, transform.position, Quaternion.identity);
        if (m_Type == type.player)
        {
            if (!GameController.m_isDead)
            {
                GameObject theme = GameObject.FindGameObjectWithTag("theme");
                if (theme)
                {
                    ThemeController control = theme.GetComponent<ThemeController>();
                    if (control)
                        control.ChangeGameOverTheme();
                }
                if (m_soundControl)
                    m_soundControl.PlayerClip(m_soundControl.m_playDeadClip);
                GameController.m_isDead = true;
                GameController.m_currentMaxHeal = GameController.m_MaxHealPlayer;
                transform.parent.gameObject.SetActive(false);
                Invoke("LoadMenu", 2f);
                return;
            }
            else
                return;
        }

        if (m_Type == type.boss)
        {
            if (m_BossControl)
            {
                m_BossControl.Dead();
                GameObject theme = GameObject.FindGameObjectWithTag("theme");
                if (theme)
                {
                    ThemeController control = theme.GetComponent<ThemeController>();
                    if (control)
                        control.ChangeWinTheme();
                }
            }
            GameController.Destroy(this);
            return;
        }

        GameObject.Destroy(gameObject);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (m_Type == type.player)
        {
            if ((other.tag == "enemy" || other.tag == "enemyWeapon") && m_PlayerControl)
            {
                if (m_PlayerControl.m_TimeHurt > 2f)
                {

                    if (m_soundControl)
                        m_soundControl.PlayerClip(m_soundControl.m_PlayerHurtClip);

                    m_PlayerControl.m_TimeHurt = 0;
                    if (other.gameObject.GetComponent<DameController>())
                        m_Heal -= other.gameObject.GetComponent<DameController>().m_Dame;

                    if (m_PlayerControl.m_HealManager)
                        m_PlayerControl.m_HealManager.DrawUI(m_MaxHeal, m_Heal);

                    if (m_Heal <= 0)
                        Dead();

                    if (other.tag == "enemyWeapon" && other.name != "melee")
                        GameObject.Destroy(other.gameObject);
                }
            }

            if (other.tag == "item" && m_PlayerControl)
            {
                if (other.name == "heal")
                    m_Heal += 2;
                if (other.name == "bigHeal")
                    m_Heal += 4;
                if (other.name == "maxHeal")
                {
                    m_MaxHeal = (m_MaxHeal < 8 ? m_MaxHeal + 4 : m_MaxHeal + 2);
                    GameController.m_currentMaxHeal = m_MaxHeal;
                }
                if (m_Heal > m_MaxHeal)
                    m_Heal = m_MaxHeal;
                if (m_PlayerControl.m_HealManager)
                    m_PlayerControl.m_HealManager.DrawUI(m_MaxHeal, m_Heal);
                GameObject.Destroy(other.gameObject);
                if (m_soundControl)
                    m_soundControl.ItemClip();
            }

            if (other.tag == "dead" && m_PlayerControl)
            {
                m_Heal = 0;
                if (m_PlayerControl.m_HealManager)
                    m_PlayerControl.m_HealManager.DrawUI(m_MaxHeal, m_Heal);
                Dead();
            }
        }

        if (m_Type == type.enemy)
        {
            if (other.tag == "playerWeapon")
            {
                m_Heal -= other.gameObject.GetComponent<DameController>().m_Dame;
                if (m_Heal <= 0)
                {
                    if (m_soundControl)
                        m_soundControl.EnemyClip(m_soundControl.m_explosionClip);
                    Dead();
                }
                else
                {
                    if (m_soundControl)
                        m_soundControl.EnemyClip(m_soundControl.m_EnemyHurtClip);
                }
            }
        }


        if (m_Type == type.boss)
        {
            if (other.tag == "playerWeapon" && !m_BossControl.m_IsHurt)
            {
                if (m_soundControl)
                    m_soundControl.EnemyClip(m_soundControl.m_EnemyHurtClip);
                m_Heal -= other.gameObject.GetComponent<DameController>().m_Dame;
                if (m_BossControl)
                {
                    m_BossControl.Hurt();
                    if (m_BossControl.m_HealManager)
                        m_BossControl.m_HealManager.DrawUI(m_MaxHeal, m_Heal);
                }
                if (m_Heal <= 0)
                    Dead();
                if (gameObject.name == "skull")
                {
                    int lost = m_MaxHeal - m_Heal;
                    int goal = 4 - lost % 4;
                    if (goal % 2 == 0)
                    {
                        SKullAI skuAI = gameObject.GetComponent<SKullAI>();
                        skuAI.UpdateGoal(new SKullAI.Goal(0, -goal * goal));
                    }
                }
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (m_Type == type.player)
        {
            if ((other.tag == "enemy" || other.tag == "enemyWeapon") && m_PlayerControl)
            {
                if (m_PlayerControl.m_TimeHurt > 2f)
                {

                    if (m_soundControl)
                        m_soundControl.PlayerClip(m_soundControl.m_PlayerHurtClip);

                    m_PlayerControl.m_TimeHurt = 0;
                    if (other.gameObject.GetComponent<DameController>())
                        m_Heal -= other.gameObject.GetComponent<DameController>().m_Dame;

                    if (m_PlayerControl.m_HealManager)
                        m_PlayerControl.m_HealManager.DrawUI(m_MaxHeal, m_Heal);

                    if (m_Heal <= 0)
                        Dead();

                    if (other.tag == "enemyWeapon" && other.name != "melee")
                        GameObject.Destroy(other.gameObject);
                }
            }

            if (other.tag == "item" && m_PlayerControl)
            {
                if (other.name == "heal")
                    m_Heal += 2;
                if (other.name == "bigHeal")
                    m_Heal += 4;
                if (other.name == "maxHeal")
                {
                    m_MaxHeal = (m_MaxHeal < 8 ? m_MaxHeal + 4 : m_MaxHeal + 2);
                    GameController.m_currentMaxHeal = m_MaxHeal;
                }
                if (m_Heal > m_MaxHeal)
                    m_Heal = m_MaxHeal;
                if (m_PlayerControl.m_HealManager)
                    m_PlayerControl.m_HealManager.DrawUI(m_MaxHeal, m_Heal);
                GameObject.Destroy(other.gameObject);
                if (m_soundControl)
                    m_soundControl.ItemClip();
            }

            if (other.tag == "dead" && m_PlayerControl)
            {
                m_Heal = 0;
                if (m_PlayerControl.m_HealManager)
                    m_PlayerControl.m_HealManager.DrawUI(m_MaxHeal, m_Heal);
                Dead();
            }
        }

        if (m_Type == type.enemy)
        {
            if (other.tag == "playerWeapon")
            {
                m_Heal -= other.gameObject.GetComponent<DameController>().m_Dame;
                if (m_Heal <= 0)
                {
                    if (m_soundControl)
                        m_soundControl.EnemyClip(m_soundControl.m_explosionClip);
                    Dead();
                }
                else
                {
                    if (m_soundControl)
                        m_soundControl.EnemyClip(m_soundControl.m_EnemyHurtClip);
                }
            }
        }


        if (m_Type == type.boss)
        {
            if (other.tag == "playerWeapon" && !m_BossControl.m_IsHurt)
            {
                if (m_soundControl)
                    m_soundControl.EnemyClip(m_soundControl.m_EnemyHurtClip);
                m_Heal -= other.gameObject.GetComponent<DameController>().m_Dame;
                if (m_BossControl)
                {
                    m_BossControl.Hurt();
                    if (m_BossControl.m_HealManager)
                        m_BossControl.m_HealManager.DrawUI(m_MaxHeal, m_Heal);
                }
                if (m_Heal <= 0)
                    Dead();
                if (gameObject.name == "skull")
                {
                    int lost = m_MaxHeal - m_Heal;
                    int goal = 4 - lost % 4;
                    if (goal % 2 == 0)
                    {
                        SKullAI skuAI = gameObject.GetComponent<SKullAI>();
                        skuAI.UpdateGoal(new SKullAI.Goal(0, -goal * goal));
                    }
                }
            }
        }
    }
}
