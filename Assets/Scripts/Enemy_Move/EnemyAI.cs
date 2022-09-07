using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public GameObject target;

    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public int Life = 10;

    public Transform Thief;

    Path Path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker Seeker;
    Rigidbody2D Rigidbody;
    private Vector2 moveInput;

    //Animation States
    private Animator Animator;
    private string currentState;

    const string ENEMY_RIGHT_IDLE = "Enemy_Right_Idle";
    const string ENEMY_RIGHT_WALK = "Enemy_Right_Walk";

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player");
        Seeker = GetComponent<Seeker>();
        Rigidbody = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();

        InvokeRepeating("UpdatePath", 0f, .5f);
        Seeker.StartPath(Rigidbody.position, target.transform.position, OnPathComplete);
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

    private void Update()
    {
        Vector3 Direction = target.transform.position - transform.position;
        Direction.Normalize();
        moveInput = Direction;
        Player_Move();
    }

    void UpdatePath()
    {
        if (Seeker.IsDone())
        {
            Seeker.StartPath(Rigidbody.position, target.transform.position, OnPathComplete);
        }
    }

    void Player_Move()
    {
        // Four direction movement
        if (moveInput.x != 0 || moveInput.y != 0)
        {
            ChangeAnimationState(ENEMY_RIGHT_WALK);
        }

        else
        {
            ChangeAnimationState(ENEMY_RIGHT_IDLE);
        }
    }

    void OnPathComplete(Path P)
    {
        if (!P.error)
        {
            Path = P;
            currentWaypoint = 0;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Life--;

            if (Life == 0)
            {
                Destroy(this.gameObject);
                return;
            }
        }
    }

    void FixedUpdate()
    {
        if (Path == null)
        {
            return;
        }

        if(currentWaypoint >= Path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)Path.vectorPath[currentWaypoint] - Rigidbody.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        Rigidbody.AddForce(force);

        float distance = Vector2.Distance(Rigidbody.position, Path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (force.x >= 0.01f)
        {
            Thief.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (force.x <= -0.01f)
        {
            Thief.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
