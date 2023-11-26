using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CayControllerAnimation : MonoBehaviour
{
    [Header("-------------Setting----------")]
    [SerializeField] Sprite[] spritesIdle1;
    [SerializeField] Sprite[] spritesIdle2;
    [SerializeField] Sprite[] spritesAttack;
    [SerializeField] float deltaFrame = 0.05f;
    int action;
    int index;
    float _deltaFrame;
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        action = 0;
        index = 0;
        _deltaFrame = deltaFrame;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_deltaFrame > 0)
        {
            _deltaFrame -= Time.deltaTime;
        }
        else
        {
            _deltaFrame = deltaFrame;
            index++;
            switch( action )
            {
                case 0:
                    {
                        if (spritesIdle1.Length == 0)
                            break;
                        if( index == spritesIdle1.Length) {
                            index = 0;
                        }
                        spriteRenderer.sprite = spritesIdle1[index];
                        break;
                    }
                case 1:
                    {
                        if (spritesIdle2.Length == 0)
                            break;
                        if (index == spritesIdle2.Length)
                        {
                            index = 0;
                        }
                        spriteRenderer.sprite = spritesIdle2[index];
                        break;
                    }
                case 2:
                    {
                        if (spritesAttack.Length == 0)
                            break;
                        if (index == spritesAttack.Length)
                        {
                            index = 0;
                            action = 0;
                        }
                        spriteRenderer.sprite = spritesAttack[index];
                        break;
                    }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && action != 2)
        {
            action = 1;
            index = 0;
        }else if (collision.CompareTag("EffectAttackPlayer"))
        {
            action = 2;
            index = 0;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && action != 2)
        {
            action = 0;
            index = 0;
        }
    }
}
