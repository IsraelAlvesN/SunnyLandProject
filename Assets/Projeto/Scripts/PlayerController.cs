using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator playerAnimator;
    private Rigidbody2D playerRB;
    private GameManager _gameManager;
    [SerializeField] Transform groundCheck;
    [SerializeField] bool isGround = false;
    [SerializeField] public float speed;
    [SerializeField] public float touchRun = 0.0f;
    [SerializeField] public bool facingRight = true;
    [SerializeField] public bool jump = false;
    [SerializeField] public int numberJumps = 0;
    [SerializeField] public int maxJump = 2;
    [SerializeField] public float jumpForce;

    //Audio
    public AudioSource fxGame;
    public AudioClip fxJump;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerRB = GetComponent<Rigidbody2D>();
        _gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;
    }

    void Update()
    {
        isGround = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground")); //line between player and groundCheck
        playerAnimator.SetBool("bIsGrounded", isGround);

        touchRun = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            
        }

        SetState();
    }

    private void FixedUpdate()
    {
        MovePlayer(touchRun);

        if (jump)
        {
            JumpPlayer();
        }
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

    void JumpPlayer()
    {
        if (isGround)
        {
            numberJumps = 0;
        }

        if (isGround || numberJumps < maxJump)
        {
            playerRB.AddForce(new Vector2(0f, jumpForce));
            isGround = false;
            numberJumps++;

            //som do pulo
            fxGame.PlayOneShot(fxJump);
        }

        jump = false;
    }

    void SetState()
    {
        playerAnimator.SetBool("bWalk", playerRB.velocity.x != 0 && isGround); //if rb > 0, player is moving and isGrounnd true
        playerAnimator.SetBool("bJump", !isGround);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Collectable":
                _gameManager.Points(1);
                Destroy(collision.gameObject);
                break;
        }
    }
}
