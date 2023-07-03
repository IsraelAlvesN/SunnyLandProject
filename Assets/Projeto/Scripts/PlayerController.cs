using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Animator playerAnimator;
    private Rigidbody2D playerRB;
    private GameManager _gameManager;
    private SpriteRenderer srPlayer;
    private bool isInvincible = false;
    public GameObject playerDie;
    public Transform groundCheck;
    bool isGround = false;
    public float speed;
    public float touchRun = 0.0f;
    public bool facingRight = true;
    public int lives = 3;
    public Color hitColor;
    public Color nohitColor;
    public ParticleSystem _dust;
    //jump
    public bool jump = false;
    public int numberJumps = 0;
    public int maxJump = 2;
    public float jumpForce;
    

    //Audio
    public AudioSource fxGame;
    public AudioClip fxJump;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerRB = GetComponent<Rigidbody2D>();
        srPlayer = GetComponent<SpriteRenderer>();
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
        createDust();
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
            createDust();
        }

        if (isGround || numberJumps < maxJump)
        {
            playerRB.AddForce(new Vector2(0f, jumpForce));
            isGround = false;
            numberJumps++;

            //som do pulo
            fxGame.PlayOneShot(fxJump);
            createDust();
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
            case "Enemy":
                //explosion animation
                GameObject tempExplosion = Instantiate(_gameManager.hitPrefab, transform.position, transform.localRotation);
                Destroy(tempExplosion, 0.4f);

                //add force to jump when enemy dies
                Rigidbody2D rb = GetComponentInParent<Rigidbody2D>();
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(new Vector2(0, 600));
                //explosion sound
                _gameManager.fxGame.PlayOneShot(_gameManager.fxExplosion);

                Destroy(collision.gameObject);
                break;

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                Hurt();
                break;
            case "Platform":
                //to make player a platform children
                this.transform.parent = collision.transform;
                break;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Platform":
                this.transform.parent = null;
                break;
        }
    }

    void Hurt()
    {
        if (!isInvincible)
        {
            isInvincible = true;

            lives--;
            StartCoroutine("Damage");
            _gameManager.LifeBar(lives);

            if(lives < 1)  
            {
                //jumping die effect
                GameObject pDieTenp = Instantiate(playerDie, transform.position, Quaternion.identity);
                Rigidbody2D rbDie = pDieTenp.GetComponent<Rigidbody2D>();
                rbDie.AddForce(new Vector2(150f, 500f));
                _gameManager.fxGame.PlayOneShot(_gameManager.fxDead);

                Invoke("LoadGame", 4f);
                gameObject.SetActive(false);
            }
        }
        
    }

    //when player suffers damage
    IEnumerator Damage()
    {
        srPlayer.color = nohitColor;
        yield return new WaitForSeconds(0.1f);

        for (float i = 0; i < 1; i += 0.1f)
        {
            srPlayer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            srPlayer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
        srPlayer.color = Color.white;
        isInvincible = false;
    }

    void LoadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void createDust()
    {
        _dust.Play();
    }
}
