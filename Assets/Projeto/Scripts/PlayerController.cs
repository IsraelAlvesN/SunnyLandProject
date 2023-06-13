using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator playerAnimator;
    private Rigidbody2D playerRB;
    [SerializeField] Transform groundCheck;
    [SerializeField] bool isGround = false;
    [SerializeField] public float speed;
    [SerializeField] public float touchRun = 0.0f;
    [SerializeField] public bool facingRight = true;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerRB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        touchRun = Input.GetAxisRaw("Horizontal");
        SetState();
    }

    private void FixedUpdate()
    {
        MovePlayer(touchRun);
    }

    private void MovePlayer(float moveH)
    {
        playerRB.velocity = new Vector2(moveH * speed, 0f);

        if(moveH < 0 && facingRight || (moveH > 0 && !facingRight))
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        //Vector3 scale = transform.localScale;
        //scale.x *= -1;

        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    void SetState()
    {
        playerAnimator.SetBool("bWalk", playerRB.velocity.x != 0); //if rb > 0, player is moving
    }
}
