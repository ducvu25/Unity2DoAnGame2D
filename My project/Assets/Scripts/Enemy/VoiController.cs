using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiController : MonoBehaviour
{
    float _delayHit = 0;
    float _delayFlip;
    float _timeIdle;
    float _timeSpawn;

    [Header("\n-----------Check Ground--------------")]
    [SerializeField] LayerMask lmGround;
    [SerializeField] LayerMask lmPlayer;
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
    EnemyInformation enemyInformation;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        facingRight = true;
        rg = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        enemyInformation = GetComponent<EnemyInformation>();
        player = FindObjectOfType<PlayerController>().gameObject;
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
                Flip();
                _timeIdle = enemyInformation.timeIdle;
                animator.SetInteger("Run", 0);
            }
            isGround = false;
        }
        if (Physics2D.OverlapCircle(pointRight.position, pointRightRadius, lmGround2))
            Flip();
        if (Physics2D.OverlapCircle(pointDown.position, pointDownRadius, lmPlayer) && _timeSpawn <= 0)
        {
            animator.SetTrigger("Attack");
            _timeSpawn = enemyInformation.timeSpawn;
        }
    }
    void Flip()
    {
        if (_delayFlip > 0)
        {
            return;
        }
        _delayFlip = enemyInformation.delayFilip;
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
    void UpdateAnimation()
    {
        if (_timeSpawn > 0)
            _timeSpawn -= Time.deltaTime;
        if (_timeIdle > 0)
        {
            _timeIdle -= Time.deltaTime;
            return;
        }
        animator.SetInteger("Run", 1);
        if (facingRight)
        {
            rg.velocity = new Vector2(enemyInformation.speed, 0.1f);
        }
        else
        {
            rg.velocity = new Vector2(-enemyInformation.speed, 0.1f);
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
            if (other.CompareTag("EffectAttackPlayer") && _delayHit <= 0)
            {
                _delayHit = enemyInformation.delayHit;
                if (enemyInformation.Hit(other.gameObject.GetComponent<DameEnemyController>().dame))
                {
                    animator.SetTrigger("Die");
                    Invoke("Destroy", 2);
                }

                animator.SetTrigger("Hit");
                rg.velocity = new Vector2(0, 0);
            }
        }
    }
    private void OnDestroy()
    {
        gameObject.SetActive(false);
    }
}
