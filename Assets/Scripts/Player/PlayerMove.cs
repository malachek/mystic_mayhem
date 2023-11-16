using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rgb2d;
    Vector3 movementVector;
    [SerializeField]float player_speed = 10f;

    public Animator animator;
    public SpriteRenderer spriteRenderer; 

    void Start()
    {
        rgb2d = GetComponent<Rigidbody2D>();
        movementVector = new Vector3();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        movementVector.x = Input.GetAxisRaw("Horizontal");
        movementVector.y = Input.GetAxisRaw("Vertical");

        movementVector *= player_speed;

        rgb2d.velocity = movementVector;

        // Animator control
        animator.SetFloat("Speed", rgb2d.velocity.magnitude);
        // Flip animations horizontally based on the last player movement command
        if (movementVector.x < 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (movementVector.x > 0)
        {
            spriteRenderer.flipX = true;
        }
    }
}
