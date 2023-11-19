using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NhenController : MonoBehaviour
{
    [Header("------------Information--------------")]
    [SerializeField] float speed;
    [SerializeField] float maxHp;
    [SerializeField] float dame;
    [SerializeField] float timeSpawn;
    [SerializeField] float timeIdle;
    float _timeIdle;
    float _timeSpawn;

    [Header("\n-----------Check Ground--------------")]
    [SerializeField] LayerMask lmGround;
    [SerializeField] Transform pointDown;
    [SerializeField] float pointDownRadius = 0.5f;
    bool isGround;

    [Header("\n-----------Check Ground 2--------------")]
    [SerializeField] LayerMask lmGround2;
    [SerializeField] Transform pointRight;
    [SerializeField] float pointRightRadius = 0.5f;

    bool facingRight;
    Rigidbody2D rg;
    Animator animator;
    BoxCollider2D boxCollider;
    CircleCollider2D circleCollider;
    // Start is called before the first frame update
    void Start()
    {
        facingRight = true;
        rg = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        boxCollider = GetComponent<BoxCollider2D>();
        circleCollider = GetComponent<CircleCollider2D>();
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
        {
            if (!isGround)
            {
                Filip();
                _timeIdle = timeIdle;
                animator.SetInteger("Idle", 0);
            }
            isGround = false;
        }
        if (Physics2D.OverlapCircle(pointRight.position, pointRightRadius, lmGround2))
            Filip();
    }
    void Filip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
    void UpdateAnimation()
    {
        if(_timeSpawn > 0)
            _timeSpawn -= Time.deltaTime;
        if(_timeIdle > 0)
        {
            _timeIdle -= Time.deltaTime;
            return;
        }
        animator.SetInteger("Idle", 1);
        if (facingRight)
        {
            rg.velocity = new Vector2(speed, 0.1f);
        }
        else
        {
            rg.velocity = new Vector2(-speed, 0.1f);
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointDown.position, pointDownRadius);
        Gizmos.DrawWireSphere(pointRight.position, pointRightRadius);
        //Gizmos.DrawLine(pointWall.position, new Vector3(pointWall.position.x + pointWallDistance, pointWall.position.y, pointWall.position.z));
        //Gizmos.DrawLine(pointClimWall.position, new Vector3(pointClimWall.position.x + pointClimWallDistance, pointClimWall.position.y, pointClimWall.position.z));
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.bounds.Intersects(boxCollider.bounds) && _timeSpawn <= 0)
            {
                animator.SetTrigger("Attack");
                _timeSpawn = timeSpawn;
            }
            else if (other.bounds.Intersects(circleCollider.bounds))
            {
                // Xử lý khi player va chạm với circle collider của boss
                Debug.Log("Player va chạm với circle collider của boss");
            }
        }
    }
    public void AddDame(float dame)
    {
        animator.SetTrigger("Hit");
    }
}
