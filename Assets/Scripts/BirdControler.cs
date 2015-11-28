using UnityEngine;
using System.Collections;

public class BirdControler : MonoBehaviour
{
    public float flapSpeed = 1000f;
    public float maxBirdSpeed = 100f;
    public float forwardSpeed = 10f;
    private Rigidbody2D rb;
    private bool didFlap;
    private bool isDead;
    private Animator animator;
    private Vector2 originalPosition;
    private GameObject startButton;
    private GameObject menu;
    private bool gameStarted;
    private int highScore = 0;



    public void Start()
    {
        // GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;// todo
        this.rb = this.GetComponent<Rigidbody2D>();
        this.animator = this.GetComponent<Animator>();
        this.startButton = GameObject.Find("GameTap");
        this.menu = GameObject.Find("Menu");
        this.originalPosition = new Vector2(this.transform.position.x, this.transform.position.y);
        this.rb.gravityScale = 0;
        this.forwardSpeed = 0;
        this.animator.enabled = false;

    }

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (!isDead)
            {
                if (!gameStarted)
                {

                    var renderer = startButton.GetComponent<SpriteRenderer>();
                    renderer.enabled = false;
                    this.forwardSpeed = 5;
                    this.rb.gravityScale = 1;
                    this.animator.enabled = true;

                }
                this.didFlap = true;
            }
            else
            {
                Application.LoadLevel("FlapyBird");
            }
        }
    }

    public void FixedUpdate()
    {
        var velocity = this.rb.velocity;
        velocity.x = forwardSpeed;
        this.rb.velocity = velocity;

        if (this.rb.velocity.y > 0)
        {
            rb.MoveRotation(30);
        }
        else if (!isDead)
        {
            var angel = velocity.y * 8;
            if (angel < -90)
            {
                angel = -90;
            }
            this.rb.MoveRotation(angel);
        }

        if (didFlap)
        {
            didFlap = false;
            this.rb.AddForce(new Vector2(0, flapSpeed), ForceMode2D.Impulse);
            var updatedVelocity = this.rb.velocity;
            if (updatedVelocity.y > this.maxBirdSpeed)
            {
                updatedVelocity.y = this.maxBirdSpeed;
                this.rb.velocity = updatedVelocity;
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.CompareTag("Floor") || collider.gameObject.CompareTag("PipeCollition"))
        {
            this.isDead = true;
            this.animator.SetBool("BirdDead", true);
            this.forwardSpeed = 0;

            var currentScore = PlayerPrefs.GetInt("HightScore", 0);

            if (this.highScore > currentScore)
            {
                PlayerPrefs.GetInt("HightScore", this.highScore);
            }
            

            var renderer = menu.GetComponent<SpriteRenderer>();
            renderer.enabled = true;
            //Time.timeScale = 0;
            var menuX = Camera.main.transform.position.x;
            var menuY = Camera.main.transform.position.y;
            var menuPosition = this.menu.transform.position;
            menuPosition.x = menuX;
            menuPosition.y = menuY;
            this.menu.transform.position = menuPosition;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pipe"))
        {
            this.highScore++;
            Debug.Log(highScore);
        }
    }
}
