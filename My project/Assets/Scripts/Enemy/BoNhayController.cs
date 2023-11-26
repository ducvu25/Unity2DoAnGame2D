using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoNhayController : MonoBehaviour
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
    bool attack;
    Rigidbody2D rg;
    Animator animator;
    EnemyInformation enemyInformation;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        facingRight = true;
        attack = false;
        _timeIdle = 0;
        _timeSpawn = 0;
        _delayFlip = 0;
        rg = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemyInformation = GetComponent<EnemyInformation>();
        player = FindObjectOfType<PlayerController>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.TransformPoint(Vector3.zero), player.transform.TransformPoint(Vector3.zero)) > enemyInformation.distance)
        {
            return;
        }
        else if ((transform.TransformPoint(Vector3.zero).x > player.transform.TransformPoint(Vector3.zero).x && facingRight)
                || (transform.TransformPoint(Vector3.zero).x < player.transform.TransformPoint(Vector3.zero).x && !facingRight))
        {
            Flip();
        }
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
                animator.SetInteger("Idle", 0);
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
        if (_delayFlip > 0)
        {
            _delayFlip -= Time.deltaTime;
        }
        if (_timeSpawn > 0)
            _timeSpawn -= Time.deltaTime;
        if (_timeIdle > 0)
        {
            _timeIdle -= Time.deltaTime;
            return;
        }
        if (_delayHit*2 > 0)
        {
            _delayHit -= Time.deltaTime;
            return;
        }
        animator.SetInteger("Idle", 1);
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
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EffectAttackPlayer") && _delayHit <= 0)
        {
            _delayHit = enemyInformation.delayHit;
            if (enemyInformation.Hit(other.gameObject.GetComponent<DameEnemyController>().dame))
            {
                animator.SetTrigger("Die");
                Destroy(gameObject, 2f);
            }
            
            animator.SetTrigger("Hit");
            rg.velocity = new Vector2(0, 0);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("LimitEnemy") && other.transform == transform.parent)
        {
            Flip();
            attack = false;
        }
    }
}
