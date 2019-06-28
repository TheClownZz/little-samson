using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    [HideInInspector]
    public enum BigState
    {
        norman, climb_1, climb_2, none
    }

    [HideInInspector]
    public enum PlayerState
    {
        norman,
        climb_1,
        climb_2,
        stand,
        move,
        lay,
        attack,
        layAttack,
        look,
        lookAttack,
        jump,
        hurt,
        xoay,
        none
    }

    [HideInInspector]
    public Collider2D m_CurrentRoom = null;

    //  public TutorialInput m_TutInput;
    public float m_minVy = -3f;
    public CameraController m_CamControl;
    public HealManager m_HealManager;
    public float m_ForceHurt = 100f;
    public float m_addPower = 30;
    public float m_Speed = 1;
    public float m_JumpPower = 185;
    public float m_Step = 0.1f;
    public float m_MaxTimeFly = 3;
    public float m_MaxTimeHurt = 2f;
    public float m_SpeedHurt = 100f;
    public DetectCollision m_BackDetect, m_FontDetect, m_FootDetect, m_HeadDetect;
    public DetectBlock m_FontBlock;
    public Transform m_Shooter;
    public GameObject[] m_ListBullet;

    private Rigidbody2D m_R2D;
    private Animator m_Anim;
    private SpriteRenderer m_Render;
    public GameObject m_currentLoc = null;
    private GameObject m_currentCua = null;
    private Vector3 m_oldPosCurrentLoc;
    private Vector3 m_oldPosCurrentCua;

    private bool m_IsHoldJump = false;
    private bool m_IsPressJump = false;
    private bool m_IsPressLay = false;
    private bool m_IsPressAttack = false;
    private bool m_IsPressUp = false;
    private bool m_IsPressDown = false;
    private bool m_IsPressLeft = false;
    private bool m_IsPressRight = false;

    private bool m_IsLay = false;
    private bool m_IsJump = false;
    private bool m_IsAttack = false;
    private bool m_IsClimb_1 = false;
    private bool m_IsClimb_2 = false;
    private bool m_AllowAttack = true;

    private SoundController m_soundControl;

    [HideInInspector]
    public bool m_isDisableInput = false;

    private float m_TimeAttack = 100;

    [HideInInspector]
    public float m_TimeHurt = 100;
    private int m_DirectX = 0;
    private int m_DirectY = 0;
    private int m_IndexBulletReady = -1;
    private Vector3 m_oldPos;

    [HideInInspector]
    public PlayerState m_State;
    [HideInInspector]
    public BigState m_BigState;

    void Start()
    {
        m_R2D = GetComponent<Rigidbody2D>();
        m_Anim = GetComponent<Animator>();
        m_Render = GetComponent<SpriteRenderer>();
        m_State = PlayerState.norman;
        m_BigState = BigState.norman;
        m_oldPos = transform.position;
        GameObject soundManager = GameObject.FindGameObjectWithTag("sound");
        if (soundManager)
        {
            m_soundControl = soundManager.GetComponent<SoundController>();
        }
    }
    void GetInput()
    {
        if (m_isDisableInput)
        {
            m_IsPressAttack = m_IsHoldJump = m_IsPressJump = m_IsPressLay = m_IsPressUp = m_IsPressDown = false;
            m_DirectX = m_DirectY = 0;
            return;
        }


        m_IsPressAttack = Input.GetKey(KeyCode.C);
        m_IsHoldJump = Input.GetKey(KeyCode.X);
        m_IsPressJump = Input.GetKeyDown(KeyCode.X);
        m_IsPressLay = Input.GetKey(KeyCode.DownArrow);
        m_IsPressUp = Input.GetKey(KeyCode.UpArrow);
        m_IsPressDown = Input.GetKey(KeyCode.DownArrow);
        m_IsPressLeft = Input.GetKey(KeyCode.LeftArrow);
        m_IsPressRight = Input.GetKey(KeyCode.RightArrow);
        /* if (m_TutInput)
         {
             m_IsPressAttack = m_IsPressAttack && m_TutInput.m_Allow_C;
             m_IsHoldJump = m_IsHoldJump && m_TutInput.m_Allow_X;
             m_IsPressJump = m_IsPressJump && m_TutInput.m_Allow_X;
             m_IsPressLay = m_IsPressLay && m_TutInput.m_Allow_Down;
             m_IsPressUp = m_IsPressUp && m_TutInput.m_Allow_Up;
             m_IsPressDown = m_IsPressDown && m_TutInput.m_Allow_Down;
             m_IsPressLeft = m_IsPressLeft && m_TutInput.m_Allow_Left;
             m_IsPressRight = m_IsPressRight && m_TutInput.m_Allow_Right;
         }*/
        if (m_IsPressLeft)
            m_DirectX = -1;
        else if (m_IsPressRight)
            m_DirectX = 1;
        else
            m_DirectX = 0;

        if (m_IsPressDown)
            m_DirectY = -1;
        else if (m_IsPressUp)
            m_DirectY = 1;
        else
            m_DirectY = 0;

        if (m_currentLoc)
        {
            m_DirectX = 0;
            m_DirectY = 0;
        }
    }
    //
    void FixPosition(float x, float y)
    {
        float _x, _y;
        _x = transform.position.x + x;
        _y = transform.position.y + y;
        transform.position = new Vector3(_x, _y, transform.position.z);

    }
    bool UpdateBigState()
    {
        switch (m_BigState)
        {
            case BigState.norman:
                //climb_1
                if (m_HeadDetect.m_Collision && !m_IsClimb_1 && m_R2D.velocity.y > 0 && m_IsPressUp)
                {
                    m_BigState = BigState.climb_1;
                    m_State = PlayerState.climb_1;
                    m_Anim.SetTrigger(m_State.ToString());
                    m_IsClimb_1 = true;
                    FixPosition(0, m_Step);
                    HoldEnter();
                    return true;
                }
                //climb_2
                if (m_FontDetect.m_Collision && m_IsPressJump && !m_IsClimb_2 && m_DirectX !=0)
                {
                    if (m_FootDetect.m_Collision)
                        FixPosition(0, m_Step);
                    m_BigState = BigState.climb_2;
                    m_State = PlayerState.climb_2;
                    m_Anim.SetTrigger(m_State.ToString());
                    m_IsClimb_2 = true;
                    FixPosition(m_Step * transform.localScale.x, 0);
                    HoldEnter();
                    return true;
                }
                return false;
            case BigState.climb_1:
                if (m_IsClimb_1)
                {
                    //norman
                    if (m_IsPressJump)
                    {
                        m_BigState = BigState.norman;
                        m_State = PlayerState.norman;
                        m_Anim.SetTrigger(m_State.ToString());
                        m_IsClimb_1 = false;
                        HoldExit();
                        return true;
                    }

                    //climb_2
                    if ((!m_HeadDetect.m_Collision && !m_FontDetect.m_Collision && m_DirectY == 0)
                        || (m_FontDetect.m_Collision && m_DirectX * transform.localScale.x > 0 && !m_IsPressUp))
                    {
                        if (!m_FontDetect.m_Collision)
                        {
                            FixPosition(0, 2 * m_Step);
                            transform.localScale = new Vector3(-1 * transform.localScale.x, 1, 1);
                        }
                        else
                        {
                            FixPosition(2 * m_Step * transform.localScale.x, -m_Step);
                        }
                        m_BigState = BigState.climb_2;
                        m_State = PlayerState.climb_2;
                        m_Anim.SetTrigger(m_State.ToString());
                        m_IsClimb_1 = false;
                        HoldExit();
                        m_IsClimb_2 = true;
                        HoldEnter();
                        return true;
                    }

                    //fix bug tu dong roi
                    if (!m_HeadDetect.m_Collision && !m_BackDetect.m_Collision)
                    {
                        transform.position = m_oldPos;
                        return false;
                    }
                }
                return false;
            case BigState.climb_2:
                if (m_IsClimb_2)
                {
                    //climb_1
                    if ((m_HeadDetect.m_Collision && m_DirectX == 0 && m_IsPressUp) || (!m_FontDetect.m_Collision && m_IsPressDown && !m_FootDetect.m_Collision))
                    {
                        if (!m_FontDetect.m_Collision && m_IsPressDown && !m_FootDetect.m_Collision)
                        {
                            FixPosition(m_Step * transform.localScale.x, 0);
                        }
                        else
                        {
                            transform.localScale = new Vector3(-1 * transform.localScale.x, 1, 1);
                            FixPosition(0, m_Step);
                        }
                        m_BigState = BigState.climb_1;
                        m_State = PlayerState.climb_1;
                        m_Anim.SetTrigger(m_State.ToString());
                        m_IsClimb_2 = false;
                        HoldExit();
                        m_IsClimb_1 = true;
                        HoldEnter();
                        return true;
                    }
                    //norman
                    if (m_IsPressJump || (!m_HeadDetect.m_Collision && !m_FontDetect.m_Collision && m_IsPressUp) || m_FootDetect.m_Collision)
                    {
                        if (!m_IsPressJump)
                            FixPosition(m_Step * transform.localScale.x, 0);
                        m_BigState = BigState.norman;
                        m_State = PlayerState.norman;
                        m_Anim.SetTrigger(m_State.ToString());
                        m_IsClimb_2 = false;
                        HoldExit();
                        return true;
                    }

                }
                return false;
            default:
                return false;
        }
    }
    bool CheckHurt()
    {
        if (m_IsClimb_1 || m_IsClimb_2)
            return false;
        if (m_TimeHurt <= 0.25f)
        {
            m_State = PlayerState.hurt;
            m_Anim.SetTrigger(m_State.ToString());
            return true;
        }
        else if (m_State == PlayerState.hurt)
        {
            switch (m_BigState)
            {
                case BigState.norman:
                    m_State = PlayerState.norman;
                    break;
                case BigState.climb_1:
                    m_State = PlayerState.climb_1;
                    break;
                case BigState.climb_2:
                    m_State = PlayerState.climb_2;
                    break;
                default:
                    m_State = PlayerState.norman;
                    break;
            }
            m_Anim.SetTrigger(m_State.ToString());
            return true;
        }
        return false;
    }
    bool CheckXoay()
    {
        if (m_currentLoc)
        {
            m_State = PlayerState.xoay;
            m_Anim.SetTrigger(m_State.ToString());
            if (m_BigState != BigState.norman)
            {
                m_IsClimb_1 = false;
                m_IsClimb_2 = false;
                HoldExit();
                m_BigState = BigState.norman;
            }

            return true;
        }
        return false;
    }
    void UpdateState()
    {
        m_Anim.ResetTrigger(m_State.ToString());
        if (CheckHurt())
            return;

        if (CheckXoay())
            return;

        if (UpdateBigState())
            return;
        switch (m_State)
        {
            case PlayerState.norman:
            case PlayerState.climb_1:
            case PlayerState.climb_2:
            case PlayerState.xoay:
                m_State = PlayerState.stand;
                break;

            case PlayerState.stand:
                if (m_IsJump && m_BigState == BigState.norman)
                {
                    m_State = PlayerState.jump;
                    break;
                }

                if (m_IsLay && m_BigState == BigState.norman)
                {
                    m_State = PlayerState.lay;
                    break;
                }
                if (m_BigState == BigState.climb_2 && m_DirectX * transform.localScale.x == -1)
                {
                    m_State = PlayerState.look;
                    break;
                }
                if ((m_DirectX != 0 && !m_IsClimb_2) || (m_DirectY != 0 && m_IsClimb_2))
                {
                    m_State = PlayerState.move;
                    break;
                }
                if (m_IsAttack)
                {
                    m_State = PlayerState.attack;
                    break;
                }
                break;

            case PlayerState.move:
                if (m_IsJump && m_BigState == BigState.norman)
                {
                    m_State = PlayerState.jump;
                    break;
                }

                if (m_IsLay && m_BigState == BigState.norman)
                {
                    m_State = PlayerState.lay;
                    break;
                }
                if ((m_DirectX == 0 && !m_IsClimb_2) || (m_IsClimb_2 && m_DirectY == 0))
                    m_State = PlayerState.stand;
                break;

            case PlayerState.jump:
                if (m_IsJump)
                    break;
                if (m_DirectX != 0 && !m_IsLay)
                    m_State = PlayerState.move;
                else
                    m_State = PlayerState.stand;
                break;

            case PlayerState.lay:
                if (m_IsJump)
                {
                    m_State = PlayerState.jump;
                    break;
                }
                if (m_IsAttack)
                {
                    m_State = PlayerState.layAttack;
                    break;
                }

                if (!m_IsLay)
                    m_State = PlayerState.stand;
                break;

            case PlayerState.layAttack:
                if (m_IsJump)
                {
                    m_State = PlayerState.jump;
                    break;
                }
                if (!m_IsAttack)
                    m_State = PlayerState.lay;
                break;

            case PlayerState.attack:
                if (m_IsJump && m_BigState == BigState.norman)
                {
                    m_State = PlayerState.jump;
                    break;
                }

                if (m_IsLay && m_BigState == BigState.norman)
                {
                    m_State = PlayerState.lay;
                    break;
                }
                if (m_DirectX != 0)
                {
                    m_State = PlayerState.move;
                    break;
                }
                if (!m_IsAttack)
                    m_State = PlayerState.stand;
                break;
            case PlayerState.look:
                if (m_IsJump && m_BigState == BigState.norman)
                {
                    m_State = PlayerState.jump;
                    break;
                }
                if (m_DirectX * transform.localScale.x != -1)
                {
                    m_State = PlayerState.stand;
                    break;
                }
                if (m_IsAttack)
                {
                    m_State = PlayerState.lookAttack;
                    break;
                }
                break;
            case PlayerState.lookAttack:
                if (m_IsJump && m_BigState == BigState.norman)
                {
                    m_State = PlayerState.jump;
                    break;
                }
                if (!m_IsAttack)
                    m_State = PlayerState.look;
                break;
            default:
                break;
        }
        m_Anim.SetTrigger(m_State.ToString());

    }
    void Flip()
    {
        if (m_DirectX == 0 || m_IsClimb_2)
            return;
        transform.localScale = new Vector3(m_DirectX, 1, 1);
    }
    void Jump()
    {
        if (m_IsPressJump && (m_FootDetect.m_Collision || m_IsClimb_2 || (m_currentLoc && m_R2D.velocity.y <= 0)))
        {
            m_R2D.AddForce(new Vector2(0, m_JumpPower));
            m_IsJump = true;
        }
        m_IsJump = !m_FootDetect.m_Collision;
    }
    void Lay()
    {
        if (m_IsPressLay && (m_FootDetect.m_Collision || m_State == PlayerState.lay))
        {
            // m_R2D.velocity = new Vector2(0, 0);
            m_IsLay = true;
        }
        else
            m_IsLay = false;
    }
    void Move()
    {

        if (!m_IsLay && !m_IsClimb_2 && !m_IsClimb_1)
            m_R2D.velocity = new Vector2(m_DirectX * m_Speed, m_R2D.velocity.y);
        if ((m_FontDetect.m_Collision || m_FontBlock.m_Collision) && !m_IsClimb_1)
            m_R2D.velocity = new Vector2(0, m_R2D.velocity.y);
    }
    void HoldEnter()
    {
        m_R2D.gravityScale = 0;
        m_R2D.angularVelocity = 0;
    }
    void HoldExit()
    {
        m_R2D.gravityScale = 1;
    }
    void Climb_1_Stay()
    {
        if (m_IsClimb_1)
        {
            if (m_DirectY != 0)
                m_R2D.velocity = new Vector2(0, 0);
            else
                m_R2D.velocity = new Vector2(0.5f * m_Speed * m_DirectX, 0);
        }

    }
    void Climb_2_Stay()
    {
        if (m_IsClimb_2 && m_State != PlayerState.look && m_State != PlayerState.lookAttack)
        {
            if (m_DirectX != 0)
                m_R2D.velocity = new Vector2(0, 0);
            else
                m_R2D.velocity = new Vector2(0, 0.5f * m_Speed * m_DirectY);
        }

    }
    void Fire()
    {
        bulletController control = m_ListBullet[m_IndexBulletReady].GetComponent<bulletController>();
        int direct = (int)transform.localScale.x;
        if (m_State == PlayerState.look || m_State == PlayerState.lookAttack)
            direct *= -1;
        control.StartFire(m_Shooter.transform.position);
        control.FindDirect(direct);
        control.SetType(0);
        control.m_IsReady = false;

        if (m_soundControl)
            m_soundControl.PlayerClip(m_soundControl.m_PlayerShootClip);
    }
    void Attack()
    {
        FindReadyBullet();
        if (m_IsPressAttack && m_AllowAttack && !m_IsAttack)
        {
            m_IsAttack = true;
            m_TimeAttack = 0;
            Fire();
        }

        if (m_TimeAttack > 0.1f)
            m_IsAttack = false;
    }
    void FindReadyBullet()
    {
        m_IndexBulletReady = -1;
        for (int i = 0; i < m_ListBullet.Length; i++)
        {
            if (m_ListBullet[i].GetComponent<bulletController>().m_IsReady)
            {
                m_IndexBulletReady = i;
                break;
            }
        }

        m_AllowAttack = (m_IndexBulletReady == -1) ? false : true;
    }
    public void ForceHurt(int direct)
    {
        if (m_BigState == BigState.climb_2 || m_BigState == BigState.climb_1 || m_IsLay)
            return;
        m_R2D.velocity = new Vector2(0, 0);
        m_R2D.angularVelocity = 0;
        m_R2D.AddForce(new Vector2(m_ForceHurt * direct, 0));
    }
    void Hurt()
    {
        if (m_TimeHurt <= 2f)
        {
            Color c = m_Render.color;
            c.a -= Time.deltaTime;
            if (c.a <= 0)
                c.a = 1;
            m_Render.color = new Color(1f, 1f, 1f, c.a);
        }
        else if (m_Render.color.a != 1f)
        {
            m_Render.color = new Color(1f, 1f, 1f, 1f);
        }

    }

    void Loc()
    {
        if (m_currentLoc)
        {
            if (m_R2D.velocity.y <= 0)
            {
                Vector2 direct = m_currentLoc.GetComponent<LocController>().m_direct;
                float dx = 0, dy = 0;

                // if ((direct.x > 0 && !m_FontBlock.m_Collision && !m_FontDetect) || (direct.x < 0 && !m_BackBlock.m_Collision))
                dx = m_currentLoc.transform.position.x - m_oldPosCurrentLoc.x;

                // if (direct.y > 0 && !m_HeadBlock.m_Collision)
                dy = m_currentLoc.transform.position.y - m_oldPosCurrentLoc.y;
                transform.position = new Vector3(transform.position.x + dx, transform.position.y + dy, transform.position.z);
            }
            m_oldPosCurrentLoc = m_currentLoc.transform.position;

        }
    }

    void Cua()
    {
        if (m_currentCua)
        {
            if (!m_IsLay)
            {
                float dx = 0;

                dx = m_currentCua.transform.position.x - m_oldPosCurrentCua.x;

                transform.position = new Vector3(transform.position.x + dx, transform.position.y, transform.position.z);
            }
            m_oldPosCurrentCua = m_currentCua.transform.position;

        }
    }


    //
    void UpdateTime()
    {
        m_TimeAttack += Time.deltaTime;
        m_TimeHurt += Time.deltaTime;
    }
    void Update()
    {
        UpdateTime();
        GetInput();
        Jump();
        UpdateState();
        Climb_1_Stay();
        Climb_2_Stay();
        Lay();
        Move();
        Attack();
        Flip();
        Hurt();
        Loc();
        Cua();
        m_oldPos = transform.position;
        if (m_R2D.velocity.y < m_minVy)
        {
            m_R2D.velocity = new Vector2(m_R2D.velocity.x, m_minVy);
        }
    }
    //
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "loc" && !m_currentLoc && !m_IsLay)
        {
            m_currentLoc = other.gameObject;
            m_oldPosCurrentLoc = m_currentLoc.transform.position;
            HoldEnter();
            float y = m_R2D.velocity.y;
            y = (y > 0) ? y : 0;
            m_R2D.velocity = new Vector2(0, y);
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "room" && !m_CurrentRoom && other.isTrigger)
        {
            m_CurrentRoom = other;
            other.transform.GetChild(0).gameObject.SetActive(true);
            if (other.gameObject.GetComponent<BossRoomController>())
            {
                GameObject theme = GameObject.FindGameObjectWithTag("theme");
                if(theme)
                {
                    ThemeController control = theme.GetComponent<ThemeController>();
                    if (control)
                        control.ChangeBossTheme();
                }
                other.gameObject.GetComponent<BossRoomController>().m_isStart = true;
                m_isDisableInput = true;
            }
            m_CamControl.SetLimit();
            m_CamControl.m_isChange = true;
            m_CamControl.m_isFindPos = false;
        }
        if (other.tag == "loc" && !m_currentLoc && !m_IsLay)
        {
            m_currentLoc = other.gameObject;
            m_oldPosCurrentLoc = m_currentLoc.transform.position;
            HoldEnter();
            float y = m_R2D.velocity.y;
            y = (y > 0) ? y : 0;
            m_R2D.velocity = new Vector2(0, y);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "room" && m_CurrentRoom == other)
        {
            other.isTrigger = false;
            other.tag = "block";
            if (m_currentLoc)
            {
                m_currentLoc.transform.parent = transform.parent;
                m_currentLoc.GetComponent<LocController>().m_isKill = true;
            }
            other.transform.GetChild(0).gameObject.SetActive(false);
            m_CurrentRoom = null;
        }
        if (other.tag == "loc")
        {
            m_currentLoc = null;
            HoldExit();
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "cua")
        {
            m_currentCua = other.gameObject;
            m_oldPosCurrentCua = m_currentCua.transform.position;
        }

    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.name == "cua")
        {
            m_currentCua = null;
        }
    }

}
