using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionPlayer
{
    idle,
    walk,
    attack,
    hit,
    die
}

public class Player : MonoBehaviour
{
    [Header("------------Information--------------")]
    [SerializeField] float speed;
    [SerializeField] float maxHp;
    [SerializeField] float dame;
    [SerializeField] float timeSpawn;

    [Header("\n-----------Check Ground--------------")]
    [SerializeField] LayerMask lmGround;
    [SerializeField] Transform pointDown;
    [SerializeField] float pointDownRadius = 0.5f;
    bool isGround;

    bool facingRight;
    Rigidbody2D rg;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        facingRight = true;
        rg = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        this.CheckCollider();
        this.UpdateAnimation();
    }
    void CheckCollider()
    {
        if (Physics2D.OverlapCircle(pointDown.position, pointDownRadius, lmGround))
            isGround = true;
        else
            isGround = false;
    }
    void Filip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
    void UpdateAnimation()
    {
        /*action state = action.idle;
        if (speed > 0.1f)
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
        animator.SetInteger("State", (int)state);*/
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointDown.position, pointDownRadius);
        //Gizmos.DrawLine(pointWall.position, new Vector3(pointWall.position.x + pointWallDistance, pointWall.position.y, pointWall.position.z));
        //Gizmos.DrawLine(pointClimWall.position, new Vector3(pointClimWall.position.x + pointClimWallDistance, pointClimWall.position.y, pointClimWall.position.z));
    }
}

