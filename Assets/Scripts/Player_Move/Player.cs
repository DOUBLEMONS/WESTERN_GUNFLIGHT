using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    [Range(0.01f, 10f)]
    private float fadeTime;

    //Motion
    public float MovingSpeed;
    public Rigidbody2D Rigidbody;
    private Vector2 moveInput;
    private bool facingRight = true;
    SpriteRenderer spriteRenderer;
    Image Background;

    //Dash
    private float ActiveMoveSpeed;
    private float DashCounter;
    private float DashCoolCounter;
    public float DashSpeed;
    public float DashLength = .5f, DashCooldown = 1f;
    public Player_Ghost Ghost;

    //Animation States
    private Animator Animator;
    private string currentState;

    const string PLATER_RIGHT_IDLE = "Player_Right_Idle";
    const string PLATER_RIGHT_WALK = "Player_Right_Walk";

    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        Ghost = GetComponent<Player_Ghost>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Background = GetComponent<Image>();
    }

    void Start()
    {
        ActiveMoveSpeed = MovingSpeed;
        Ghost = GameObject.Find("Ghost").GetComponent<Player_Ghost>();
    }

    void ChangeAnimationState(string newState)
    {
        // stop the same animation from interrupting itself
        if (currentState == newState) return;

        // play the animation
        Animator.Play(newState);

        // reassign the current state
        currentState = newState;
    }

    void Update()
    {
        Player_Move();
    }

    void Player_Move()
    {
        // Dash
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();

        Rigidbody.velocity = moveInput * ActiveMoveSpeed;

        if (Input.GetButtonDown("Dash"))
        {
            if (DashCoolCounter <= 0 && DashCounter <= 0)
            {
                ActiveMoveSpeed = DashSpeed;
                DashCounter = DashLength;
            }
        }

        if (DashCounter > 0)
        {
            DashCounter -= Time.deltaTime;

            if (DashCounter <= 0)
            {
                ActiveMoveSpeed = MovingSpeed;
                DashCoolCounter = DashCooldown;
            }

            if (DashCoolCounter == DashCooldown)
            {
                Ghost.makeGhost = false;
            }
            else 
            {
                Ghost.makeGhost = true;
            }

        }

        if (DashCoolCounter > 0)
        {
            DashCoolCounter -= Time.deltaTime;
        }

        // Four direction movement

        Vector2 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);

        if (dir.x > 0 && !facingRight || dir.x < 0 && facingRight)
        {
            Flip();
        }

        if (moveInput.x != 0 || moveInput.y != 0)
        {
            ChangeAnimationState(PLATER_RIGHT_WALK);
        }

        else
        {
            ChangeAnimationState(PLATER_RIGHT_IDLE);
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            OnDie();
        }
    }

    public void OnDie()
    {
        //gameObject.layer = 13;
        StartCoroutine(Fade1(0, 1));

        Invoke("NextSceneWithNum", 4);
    }

    public void NextSceneWithNum()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    private IEnumerator Fade1(float start, float end)
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / fadeTime;

            Color color = Background.color;
            color.a = Mathf.Lerp(start, end, percent);
            Background.color = color;

            yield return null;
        }
    }
}

