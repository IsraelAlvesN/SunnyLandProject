using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISlug : MonoBehaviour
{
    public  Transform       enemy;
    public  Transform[]     position;
    public  SpriteRenderer  enemySprite;
    public  float           speed;
    public  bool            isRight;

    private int             idTarget;

    void Start()
    {
        enemySprite = enemy.gameObject.GetComponent<SpriteRenderer>();
        enemy.position = position[0].position; //LimitA
        idTarget = 1;
    }

    void Update()
    {
        if(enemy != null)
        {
            enemy.position = Vector3.MoveTowards(enemy.position, position[idTarget].position, speed * Time.deltaTime);
            if(enemy.position == position[idTarget].position)
            {
                idTarget += 1; // change to other position
                if(idTarget == position.Length)
                {
                    idTarget = 0;
                }
            }

            if (position[idTarget].position.x < enemy.position.x && isRight)
            {
                Flip();
            }
            else if(position[idTarget].position.x > enemy.position.x && isRight == false)
            {
                Flip();
            }
        }
    }

    void Flip()
    {
        isRight = !isRight;
        enemySprite.flipX = !enemySprite.flipX;
    }
}
