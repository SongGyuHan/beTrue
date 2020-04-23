using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    public float maxSpeed;
    public float jumpPower;
    Animator anim;
    Physics2D gravity;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
             Physics2D.gravity = new Vector2(9.8f, 0);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Physics2D.gravity = new Vector2(0, -9.8f);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            Physics2D.gravity = new Vector2(-9.8f,0);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            Physics2D.gravity = new Vector2(0, 9.8f);
        }


        //jump
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
        }

        //when stop
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
            Debug.Log("get button up horizontal");
        }

        // character direction
        if (Input.GetButtonDown("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        //walking animation
        if ( Mathf.Abs(rigid.velocity.x) < 0.3)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);
        
    }


    public void FixedUpdate()
    {
        //move speed
        float h = Input.GetAxisRaw("Horizontal");

        Debug.Log("input getAxisRaw horizontal " + h);
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        //Max speed
        if (rigid.velocity.x > maxSpeed)
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        if (rigid.velocity.x < (-1)*maxSpeed)
            rigid.velocity = new Vector2(maxSpeed*(-1), rigid.velocity.y);

       


        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("platform"));
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.6f)
                    anim.SetBool("isJumping", false);
                Debug.Log(rayHit.collider.name);
            }
        }   
    }
}
