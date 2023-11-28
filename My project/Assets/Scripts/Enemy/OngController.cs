using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OngController : MonoBehaviour
{
    float _delayHit = 0;
    float _delayFlip;
    float _timeIdle;
    float _timeSpawn;

    [Header("\n------------Check limit-----------")]
    [SerializeField] Transform pointLimit;
    [SerializeField] float pointRadius = 0.5f;

    [Header("\n-----------Point list ----------")]
    [SerializeField] Transform[] pointLine;
    int index;
    bool facingRight;
    bool attack = false;
    Rigidbody2D rg;
    Animator animator;
    EnemyInformation enemyInformation;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        index = (int)Random.Range(0, pointLine.Length);
        facingRight = true;
        attack = false;
        _timeIdle = 0;
        _timeSpawn = 0;
        _delayFlip = 0;
        if ((transform.position.x > pointLine[index].position.x && facingRight) || (transform.position.x <= pointLine[index].position.x && !facingRight))
            Flip();
        rg = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemyInformation = GetComponent<EnemyInformation>();
        player = FindObjectOfType<PlayerController>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.TransformPoint(Vector3.zero), player.transform.TransformPoint(Vector3.zero)) < enemyInformation.distance)
        {
            attack = true;
            if (Vector2.Distance(transform.TransformPoint(Vector3.zero), transform.parent.TransformPoint(Vector3.zero)) > 1.5f * enemyInformation.distance)
            {
                attack = false;
            }
        }
        else if ((transform.TransformPoint(Vector3.zero).x > player.transform.TransformPoint(Vector3.zero).x && facingRight)
                || (transform.TransformPoint(Vector3.zero).x < player.transform.TransformPoint(Vector3.zero).x && !facingRight))
        {
            Flip();
            attack = false;
        }
        this.CheckCollider();
        this.UpdateAnimation();
    }

    void CheckCollider()
    {
        Vector3 pos = attack ? player.transform.TransformPoint(Vector3.zero) : pointLine[index].TransformPoint(Vector3.zero);
        if (Vector2.Distance(transform.TransformPoint(Vector3.zero), pos) >= 0.1f)
        {
            float t = Mathf.PingPong(Time.time * enemyInformation.speed, 1f);
            float yOffset = Mathf.Sin(t * Mathf.PI * 2) * 1f; // Điều chỉnh khoảng cách bay lên xuống tại đây
            Vector3 targetPosition = new Vector3(pos.x, pos.y + yOffset, pos.z);
            transform.position = Vector3.Lerp(transform.TransformPoint(Vector3.zero), targetPosition, enemyInformation.speed * Time.deltaTime);
        }
        else
        {
            if (!attack)
            {
                index = (int)Random.Range(0, pointLine.Length);
                if ((transform.position.x >= pointLine[index].position.x && facingRight) || (transform.position.x < pointLine[index].position.x && !facingRight))
                    Flip();
            }
            else
            {
                if ((transform.TransformPoint(Vector3.zero).x >= pos.x && facingRight) || (transform.TransformPoint(Vector3.zero).x < pos.x && !facingRight))
                    Flip();
            }
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
        if (_delayHit * 2 > 0)
        {
            _delayHit -= Time.deltaTime;
            return;
        }
        animator.SetInteger("Idle", 1);
        
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointLimit.position, pointRadius);
        Gizmos.DrawLine(transform.position, player.transform.position);
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
}