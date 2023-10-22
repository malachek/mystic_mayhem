using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rgb2d;
    Vector3 movementVector;
    [SerializeField]float player_speed = 10f;

    void Start()
    {
        rgb2d = GetComponent<Rigidbody2D>();
        movementVector = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        movementVector.x = Input.GetAxisRaw("Horizontal");
        movementVector.y = Input.GetAxisRaw("Vertical");

        movementVector *= player_speed;

        rgb2d.velocity = movementVector;
    }
}
