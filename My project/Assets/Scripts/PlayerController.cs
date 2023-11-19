using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public enum action
{
    idle,
    walk,
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

    float speed;
    float hp;
    float mp;
    float dame;
    [SerializeField] float jumpFoce;
    float[] time_spawn;

    [Header("-------Check collider-------\n")]
    [Header("------Ground-----")]
    [SerializeField] LayerMask lmGround;
    [SerializeField] Transform pointDown;
    [SerializeField] float pointDownRadius = 0.5f;
    bool isGround;
    bool isWater;
    bool canJump;
    int m_numberJump;
    int inputMovement;

    [Header("------Wall-----")]
    [SerializeField] LayerMask lmWall;
    [SerializeField] Transform pointWall;
    [SerializeField] float pointWallDistance = 0.1f;
    [SerializeField] float wallSpeed;
    bool isTochingWall;

    [Header("------Clim Wall-----")]
    [SerializeField] Transform pointClimWall;
    [SerializeField] float pointClimWallDistance = 0.1f;

    bool facingRight;

    [SerializeField] float forceAir = 10f;

    Rigidbody2D rg;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
       /* hp = playerInformation.Hp;
        mp = playerInformation.Mp;
        dame = playerInformation.Dame;
        jumpFoce = playerInformation.JumpFoce;*/
        speed = 0;
        /*time_spawn = new float[playerInformation.TimeSpawn.Length];
        for (int i = 0; i < playerInformation.TimeSpawn.Length; i++)
            time_spawn[i] = 0;
        */
        facingRight = true;
        rg = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        this.CheckCollider();
        this.CheckInput();
        //this.UpdateAnimation();
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
            speed = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.UpArrow))
            this.Jump();

        /*KeyCode[] input = { KeyCode.Q, KeyCode.W };
        for (int i = 0; i < time_spawn.Length; i++)
        {
            if (time_spawn[i] > 0)
                time_spawn[i] -= Time.deltaTime;
            if (Input.GetKeyDown(input[i]))
            {
                Attack(i);
            }
        }*/
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
            rg.velocity = new Vector2(value*speed, rg.velocity.y);
            if (speed < 10f)//playerInformation.Speed * 1.3f)
                speed += 10 / 8;//playerInformation.Speed / 8;
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
    void Attack(int type)
    {
        int[] attack = { -1, 1 };
        if (time_spawn[type] <= 0)
        {
            animator.SetTrigger("Attack");
            animator.SetFloat("Attack Type", attack[type]);
            //time_spawn[type] = playerInformation.TimeSpawn[type];
        }
    }
    void Filip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
   /* void UpdateAnimation()
    {
        action state = action.idle;
        if(speed > 0.1f)
        {
            if (speed > playerInformation.Speed * 0.75f)
                state = action.run;
            else
                state = action.walk;
        }
        if (rg.velocity.y > 0.1f)
        {
            state = action.jump;
            if (isTochingWall)
                state = action.touchWall;
        }
        else if (rg.velocity.y < -0.1f)
        {
            state = action.fell;
            if (isTochingWall)
                state = action.touchWall;
        }
        animator.SetInteger("State", (int)state);
    }*/
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointDown.position, pointDownRadius);
        Gizmos.DrawLine(pointWall.position, new Vector3(pointWall.position.x + pointWallDistance, pointWall.position.y, pointWall.position.z));
        Gizmos.DrawLine(pointClimWall.position, new Vector3(pointClimWall.position.x + pointClimWallDistance, pointClimWall.position.y, pointClimWall.position.z));
    }
}
