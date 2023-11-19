using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OngController : MonoBehaviour
{
    [Header("------------Information--------------")]
    [SerializeField] float speed;
    [SerializeField] float maxHp;
    [SerializeField] float dame;
    [SerializeField] float timeSpawn;
    [SerializeField] float timeIdle;
    float _timeIdle;
    float _timeSpawn;

    [Header("\n------------Check limit-----------")]
    [SerializeField] Transform pointLimit;
    [SerializeField] float pointRadius = 0.5f;

    [Header("\n-----------Point list ----------")]
    [SerializeField] Transform[] pointLine;
    int index;
    bool facingRight;
    Rigidbody2D rg;
    Animator animator;
    BoxCollider2D boxCollider;
    CircleCollider2D circleCollider;

    // Start is called before the first frame update
    void Start()
    {
        index = (int)Random.Range(0, pointLine.Length);
        facingRight = true;
        if ((transform.position.x > pointLine[index].position.x && facingRight) || (transform.position.x <= pointLine[index].position.x && !facingRight))
            Flip();
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
        if (Vector2.Distance(transform.position, pointLine[index].position) >= 0.1f)
        {
            float t = Mathf.PingPong(Time.time * speed, 1f);
            float yOffset = Mathf.Sin(t * Mathf.PI * 2) * 1f; // Điều chỉnh khoảng cách bay lên xuống tại đây
            Vector3 targetPosition = new Vector3(pointLine[index].position.x, pointLine[index].position.y + yOffset, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
        }
        else
        {
            index = (int)Random.Range(0, pointLine.Length);
            if ((transform.position.x >= pointLine[index].position.x && facingRight) || (transform.position.x < pointLine[index].position.x && !facingRight))
                Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    void UpdateAnimation()
    {
        /*if (_timeSpawn > 0)
            _timeSpawn -= Time.deltaTime;
        if (_timeIdle > 0)
        {
            _timeIdle -= Time.deltaTime;
            return;
        }*/

        // Add your animation logic here
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointLimit.position, pointRadius);
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