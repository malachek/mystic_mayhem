using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    [SerializeField] CharacterStats characterStats;

    Rigidbody2D rgb2d;
    Vector3 movementVector;

    public Animator animator;
    public SpriteRenderer spriteRenderer; 

    float timer_dash;
    public float dashDistance;
    public LayerMask ignoreRay;
    public Transform rayStartPoint;

    private Pathfinder WorldGrid;


    void Start()
    {
        timer_dash = characterStats.currentMaxDashCooldown;
        WorldGrid=GameObject.FindObjectOfType<Pathfinder>();    
        rgb2d = GetComponent<Rigidbody2D>();
        movementVector = new Vector3();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        movementVector.x = Input.GetAxisRaw("Horizontal");
        movementVector.y = Input.GetAxisRaw("Vertical");

        movementVector *= characterStats.currentMoveSpeed;

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

        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (timer_dash > 0)
            {
                timer_dash -= Time.deltaTime;
            }
        
            else
            {
                timer_dash = characterStats.currentMaxDashCooldown;
                Debug.Log("TP ACTIVATE!");

                Vector3 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
                Vector2 end  = rgb2d.transform.position + (direction * dashDistance);

                RaycastHit2D hit = Physics2D.Raycast(rayStartPoint.position, direction, dashDistance, ignoreRay);

                if (WorldGrid.isPosOk(hit.point))//Make sure we can actually be here
                    rgb2d.transform.position = hit.point;

                //set Ignore Ray to everything to tp over walls and stuff. These are the things that it will ignore.
                //Its like it shoots out a laser beam to the position your mouse is. If nothing, tp to the end. If there is a object in the way, teleport to the point of contact. HOWEVER, IT DOES NOT WORK (lol ;3)
                //Just Ignore Ray to everything.
                if (hit)
                {
                    if (WorldGrid.isPosOk(hit.point)) 
                    {
                        rgb2d.transform.position = hit.point;
                    }
                }
                else
                {
                    if (WorldGrid.isPosOk(end))
                    {
                        rgb2d.transform.position = end;
                    }
                }
                
            }
    
        }
         
        if(!WorldGrid.isPosOk(transform.position)) //Save Me if I am out of bounds.
        {
            for(int i=0;i<20;i++)
            {
                Vector2 randPos=transform.position+new Vector3(Random.Range(-4,4),Random.Range(-4,4),0);
                if(WorldGrid.isPosOk(randPos))
                {
                    transform.position = randPos;
                }
            }
        }

    }

    public Vector3 GetMovementVector()
    {
        return movementVector;
    }
}
