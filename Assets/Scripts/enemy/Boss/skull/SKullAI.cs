using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKullAI : MonoBehaviour
{
    public enum Action
    {
        Hide,Move,Shoot
    }
    public struct Goal
    {
        public int attack;
        public int defense;

        public Goal(int attack, int defense)
        {
            this.attack = attack;
            this.defense = defense;
        }
        
    }
    public FindPlayer m_Find;
    public DameController m_DameControl;
    public GameObject m_BulletObject;
    public SkullBossController m_SkullControl;
    public float m_maxTimeShoot = 1f;
    public float m_maxTimeMove = 1f;
    public float m_maxTimeHide = 1f;
    public BoxCollider2D m_Box;
    public Transform m_Pos_1, m_Pos_2;
    public Transform m_Center;
    public GameObject m_listBullet;

    private Goal m_GoalShoot = new Goal(2, 0);
    private Goal m_GoalHide = new Goal(-1, int.MaxValue);
    private Goal m_GoalMove = new Goal(1, 4);
    private Goal m_GoalPerTime = new Goal(-2, 0);

    private float m_Time = 0;
    private  float m_TimeAction = 0;
    private Vector3 m_desPos;
    private Goal m_Goal;
    private Action m_Action;
    private Action m_OldAction;
    [HideInInspector]
    public bool m_isHide = false;

    void Start()
    {
        m_desPos = m_Pos_1.position;
        m_Goal.attack = 2;
        m_Goal.defense = 0;
        m_Action = Action.Shoot;
        m_OldAction = m_Action;
    }
    int Factorial(int n)
    {
        int r = 1;
        for (int i = 2; i <= n; i++)
            r *= i;
        return r;
    }
    public void Appear()
    {
        m_Box.enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;
        m_isHide = false;
    }
    int CalculateToTalGoal(Goal action)
    {
        Goal goal = m_Goal;
        goal.attack -= action.attack;
        goal.defense -= action.defense;
        if (goal.attack < 0)
            goal.attack = 0;
        if (goal.defense < 0)
            goal.defense = 0;
        return goal.attack * goal.attack + goal.defense * goal.defense;

    }
    void Chose()
    {
        m_Action = Action.Shoot;
        int min = CalculateToTalGoal(m_GoalShoot);

        if(min > CalculateToTalGoal(m_GoalMove))
        {
            min = CalculateToTalGoal(m_GoalMove);
            m_Action = Action.Move;
        }

        if(min> CalculateToTalGoal(m_GoalHide))
        {
            min = CalculateToTalGoal(m_GoalHide);
            m_Action = Action.Hide;

        }
    }
    void ExcuteAction()
    {
        StartNewAction();
        switch (m_Action)
        {
            case Action.Shoot:
                Shoot();
                break;
            case Action.Move:
                Move();
                break;
            default:
                Hide();
                break;
        }
    }
    void FindNewPosition()
    {
        if (!m_Find.m_Player)
            return;
        if (transform.position.x < m_Find.m_Player.transform.position.x)
            m_desPos = m_Pos_2.position;
        else
            m_desPos = m_Pos_1.position;
    }
    void StartNewAction()
    {
       if(m_OldAction != m_Action)
        {
            if (m_Action == Action.Move)
                FindNewPosition();
            else
             if(m_Action== Action.Hide)
            {
                for (int i = 0; i < m_listBullet.transform.childCount; i++)
                    GameObject.Destroy(m_listBullet.transform.GetChild(i).gameObject);
            }
            m_TimeAction = 0;
            m_OldAction = m_Action;
        }
    }
    void Hide()
    {
        m_TimeAction += Time.deltaTime;
        if (!m_isHide)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            m_Box.enabled = false;
            m_isHide = true;
            transform.position = m_Center.position;
        }

        if(m_TimeAction >=m_maxTimeHide)
        {
            m_TimeAction = 0;
            m_SkullControl.SetSkullApear();
            UpdateGoal(m_GoalHide);
        }

    }
    void Move()
    {
        if (m_isHide)
            return;
        m_TimeAction += Time.deltaTime;
        float x = Mathf.Lerp(transform.position.x, m_desPos.x, Time.deltaTime);
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
        if (m_TimeAction >= m_maxTimeMove)
        {
            m_TimeAction = 0;
            UpdateGoal(m_GoalMove);
        }
    }
    void Shoot()
    {
        if (m_isHide)
            return;
        m_TimeAction += Time.deltaTime;
        if (m_TimeAction >= m_maxTimeShoot)
        {
            m_TimeAction -= m_maxTimeShoot;
            Fire();
            UpdateGoal(m_GoalShoot);
        }
    }
    void Fire()
    {
        GameObject bullet = Instantiate(m_BulletObject, transform.position, Quaternion.identity);
        SkullbulletController control = bullet.GetComponent<SkullbulletController>();
        bullet.transform.parent = m_listBullet.transform;
        if (control)
        {
            control.m_AlllowFindPlayer = true;
        }
    }
    public void UpdateGoal(Goal action)
    {
        m_Goal.attack -= action.attack;
        m_Goal.defense -= action.defense;
        if (m_Goal.attack < 0)
            m_Goal.attack = 0;
        if (m_Goal.defense < 0)
            m_Goal.defense = 0;
    }
    void UpdateTime()
    {
        m_Time += Time.deltaTime;
        if (m_Time >= 1f)
        {
            m_Time -= 1f;
            if(m_Goal.attack<=4)
                UpdateGoal(m_GoalPerTime);
        }
    }

    void Dead()
    {
        Appear();
        GameObject.Destroy(m_listBullet);
        GameObject.Destroy(this);
    }
    void Update()
    {
        if (!m_Find.m_Player)
            return;
        if (m_DameControl.m_Heal <= 0)
        {
            Dead();
            return;
        }

        UpdateTime();
        Chose();
        ExcuteAction();
    }

}
