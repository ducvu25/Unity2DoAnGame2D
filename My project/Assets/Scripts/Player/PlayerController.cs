using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public enum action
{
    idle,
    run,
    jump,
    fell,
    touchWall
}
public class PlayerController : MonoBehaviour
{
    [Header("-------Information-------")]
    //[SerializeField] PlayerSO playerInformation;
    [SerializeField][Range(1, 3)] int numberJump;
    public float delayHit = 1f;
    [SerializeField] float speed = 2;
    [SerializeField]  float maxHp = 1000;
    float hp;
    [SerializeField] float jumpFoce;
    [SerializeField] float time_spawn;
    [SerializeField] float delayCombo;
    float _time_spawn = 0;
    float _delayCombo = 0;
    float _delayHit = 0;    

    [Header("-------Check collider-------\n")]
    [Header("------Ground-----")]
    [SerializeField] LayerMask lmGround;
    [SerializeField] Transform pointDown;
    [SerializeField] float pointDownRadius = 0.5f;
    float _speed;
    bool isGround;
    bool canJump;
    int m_numberJump;
    int inputMovement;

    [Header("------Wall-----")]
    [SerializeField] LayerMask lmWall;
    [SerializeField] Transform pointWall;
    [SerializeField] float pointWallDistance = 0.1f;
    [SerializeField] float wallSpeed;
    bool isTochingWall;

    bool facingRight;

    [SerializeField] float forceAir = 10f;

    Rigidbody2D rg;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
        facingRight = false;
        rg = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(m_numberJump);
        if (hp <= 0) return;
        this.CheckCollider();
        this.CheckInput();
        this.UpdateAnimation();
    }
    void CheckCollider()
    {
        isTochingWall = Physics2D.Raycast(pointWall.position, transform.right* (facingRight ? 1 : -1) , pointWallDistance, lmWall);

        if (Physics2D.OverlapCircle(pointDown.position, pointDownRadius, lmGround))
            isGround = true;
        else
            isGround = false;

        if (isGround || isTochingWall)
        {
            canJump = true;
            m_numberJump = numberJump;
        }
        else
        {
            if (m_numberJump > 0)
                canJump = true;
            else
                canJump = false;
        }
    }
    void CheckInput()
    {
        inputMovement = 0;
        _time_spawn -= Time.deltaTime;
        _delayCombo -= Time.deltaTime;
        if (_delayHit > 0)
        {
            _delayHit -= Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (facingRight)
                Filip();
            inputMovement = -1;
            this.Run();
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (!facingRight)
                Filip();
            inputMovement = 1;
            this.Run();
        }
        else
        {
            rg.velocity = new Vector2(0, rg.velocity.y);
            _speed = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.UpArrow))
            this.Jump();

        if (Input.GetKeyDown(KeyCode.Q))
            Attack();
    }
    void Run()
    {
        this.AddMovement(inputMovement);
        if(isTochingWall && rg.velocity.y < -wallSpeed)
        {
            rg.velocity = new Vector2(rg.velocity.x, -wallSpeed);
        }
    }
    void AddMovement(int value)
    {
        if (isGround)
        {
            rg.velocity = new Vector2(value*_speed, rg.velocity.y);
            if (_speed < 2)
                _speed += speed;//playerInformation.Speed / 8;
        }
        else if(!isGround && !isTochingWall)
        {
            Vector2 forceToAdd = new Vector2(value * forceAir, 0);
            if(Mathf.Abs(rg.velocity.x) < forceAir)
                rg.AddForce(forceToAdd);
        }
    }
    void Jump()
    {
        if (!canJump) return;
        if (!isTochingWall)
        {
            rg.velocity = new Vector2(rg.velocity.x, jumpFoce);
            m_numberJump--;
        }else 
        {
            Filip();
            // rg.velocity = Vector2.zero;
            rg.velocity = new Vector2(rg.velocity.x, jumpFoce);
        }
    }
    void Attack()
    {
       // Debug.Log(_delayCombo);
        if (_time_spawn <= 0)
        {
            animator.SetTrigger("Attack");
            if(_delayCombo > 0)
            {
                animator.SetFloat("Skill", (animator.GetFloat("Skill") + 1) % 3);
            }
            else
            {
                animator.SetFloat("Skill", 0);
            }
            _delayCombo = delayCombo;
            _time_spawn = time_spawn;
        }
    }
    void Filip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
   void UpdateAnimation()
    {
        action state = action.idle;
        if(math.abs(rg.velocity.x) >= 0.1f)
        {
            state = action.run;
        }
        if (!isGround)
        {
            state = action.fell;
        }
        if (rg.velocity.y > 0.1f && !isGround)
        {
            state = action.jump;
        }

        animator.SetInteger("State", (int)state);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyDame") && _delayHit <= 0 && hp > 0)
        {
            _delayHit = delayHit;
            hp -= collision.gameObject.GetComponent<DameEnemyController>().dame;
            if(hp < 0)
            {
                animator.SetTrigger("Die");
                return;
            }
            animator.SetTrigger("Hit");
            rg.AddForce(new Vector2(facingRight ? 1 : -1, 3));
            animator.SetFloat("TypeHit", (int)UnityEngine.Random.Range(0, 2));
            _delayHit = delayHit;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointDown.position, pointDownRadius);
        Gizmos.DrawLine(pointWall.position, new Vector3(pointWall.position.x + pointWallDistance, pointWall.position.y, pointWall.position.z));
        //Gizmos.DrawLine(pointClimWall.position, new Vector3(pointClimWall.position.x + pointClimWallDistance, pointClimWall.position.y, pointClimWall.position.z));
    }
}
