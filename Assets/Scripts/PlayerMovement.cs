using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private new Camera camera;
    private new  Rigidbody2D rigidbody;
    private new CapsuleCollider2D collider;

    private Vector2 velocity;
    private float inputAxis;

    public float speed = 8f;
    public float maxJumpHeight = 5f;
    public float maxJumpTime = 1f;

    // 物理学运动公式 : v*2 = 2as ; S = vt + 2分之一at*2;
    public float jumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);        // 这里的力其实相当于初始速度了
    public float gravity => (-2f * maxJumpTime) / Mathf.Pow((maxJumpTime / 2f), 2);

    public bool grounded { get; private set; }
    public bool jumping { get; private set; }

    public bool running => Mathf.Abs(velocity.x) > 0.25f ;
    public bool sliding => (inputAxis > 0 && velocity.x < 0) || (inputAxis < 0 && velocity.x > 0);


    private void Awake()
    {
        camera = Camera.main;
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        Movement();
        grounded = rigidbody.Raycast(Vector2.down);

        if (grounded)
        {
            GroundedMovement();
        }

        ApplyGravity();
    }

    private void OnEnable()
    {
        rigidbody.isKinematic = false;
        collider.enabled = true;
        velocity = Vector2.zero;
        jumping = false;
    }

    private void OnDisable()
    {
        rigidbody.isKinematic = true;
        collider.enabled = false;
        velocity = Vector2.zero;
        jumping = false;
    }

    private void Movement()
    {
        inputAxis = Input.GetAxis("Horizontal");
        // 如果使用下面的这个方法的话，会有一个bug就是当人物碰到水管的时候，如果一直按着左键，当你突然按着右键的时候，会等待1秒
        velocity.x = Mathf.MoveTowards(velocity.x, inputAxis * speed, speed * Time.deltaTime);
        //velocity.x = inputAxis * speed;
        if (rigidbody.Raycast(Vector2.right * velocity.x))
        {
            velocity.x = 0;
        }
        if(velocity.x > 0)
        {
            transform.eulerAngles = Vector3.zero;
        }else if(velocity.x < 0)
        {
            transform.eulerAngles = new Vector3(0, -180f, 0);
        }
    }

    private void GroundedMovement()
    {
        velocity.y = Mathf.Max(velocity.y, 0);
        jumping = velocity.y > 0f;
        if (Input.GetButtonDown("Jump"))
        {
            velocity.y = jumpForce;
            jumping = true;
        }
    }

    private void ApplyGravity()
    {
        bool falling = velocity.y < 0f || !Input.GetButton("Jump");
        float multiplier = falling ? 2f : 1f;

        velocity.y += gravity * multiplier *  Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, gravity / 2f);
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody.position;
        position += velocity * Time.fixedDeltaTime;

        // 让人物在屏幕内进行移动，防止超出屏幕。LeftEdge是将屏幕左上角转换成世界坐标，rightEdge是将屏幕右下角转换成世界坐标
        Vector2 leftEdge = camera.ScreenToWorldPoint(Vector2.zero);
        Vector2 rightEdge = camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        position.x = Mathf.Clamp(position.x, leftEdge.x + 0.5f, rightEdge.x - 0.5f);// 返回一个leftEdge和rightEdge之间的数
        rigidbody.MovePosition(position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (transform.DotTest(collision.transform, Vector2.down))
            {
                velocity.y = jumpForce / 2;
                jumping = true;
            }
        }else if (collision.gameObject.layer != LayerMask.NameToLayer("PowerUp"))
        {
            if (transform.DotTest(collision.transform, Vector2.up))
            {
                velocity.y = 0; 
            }
        }
    }
}
