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

    // ����ѧ�˶���ʽ : v*2 = 2as ; S = vt + 2��֮һat*2;
    public float jumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);        // ���������ʵ�൱�ڳ�ʼ�ٶ���
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
        // ���ʹ���������������Ļ�������һ��bug���ǵ���������ˮ�ܵ�ʱ�����һֱ�������������ͻȻ�����Ҽ���ʱ�򣬻�ȴ�1��
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

        // ����������Ļ�ڽ����ƶ�����ֹ������Ļ��LeftEdge�ǽ���Ļ���Ͻ�ת�����������꣬rightEdge�ǽ���Ļ���½�ת������������
        Vector2 leftEdge = camera.ScreenToWorldPoint(Vector2.zero);
        Vector2 rightEdge = camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        position.x = Mathf.Clamp(position.x, leftEdge.x + 0.5f, rightEdge.x - 0.5f);// ����һ��leftEdge��rightEdge֮�����
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
