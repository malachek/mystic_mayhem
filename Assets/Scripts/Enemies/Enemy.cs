using RoyT.AStar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathfinderAgent))]
public class Enemy : MonoBehaviour
{
    public GameObject PlayerTarget;

    [SerializeField] SpriteRenderer sprite; // to turn red when taking damage
    private Color currentNoDamageColor = Color.white; // when enemy is damaged, they briefly turn red, this is to know what color to go back to afterwards

    [HideInInspector]
    public float LiveHP; // The real amount of health this enemy has.
    [Min(0)]
    public float MaxHP; // The maximum amount of health this enemy has.

    [SerializeField] private bool isBoss;
    [Min(0)]
    public float StoppingDistance = 1;
    
    [SerializeField] int EXP = 1;
    [SerializeField] int strength = 1;
    [SerializeField] KillCount killCounter;
    public CharacterStats player; //declare whatever the permanent character class is here later
    //permanent character class must have method TakeDamage() for attack to work
    private PathfinderAgent pathfinderAgent;

    private float _nextShot = 0.25f;
    private float _fireDelay = 0.5f;
    SpriteRenderer spriteRenderer;
    private void Start()
    {
        sprite.color = Color.white;
        LiveHP = MaxHP;
        pathfinderAgent = GetComponent<PathfinderAgent>();
        spriteRenderer = GetComponent<SpriteRenderer>();


        lastPos = transform.position;
    }
    Vector3 lastPos;
    private void FixedUpdate()
    {

        Vector3 movementVector = transform.position - lastPos;
        if (movementVector.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (movementVector.x < 0)
        {
            spriteRenderer.flipX =false;
        }
        //Dont change when we stop moving ^^^


        lastPos = transform.position;

        if (LiveHP<= 0)
        {
            Die();
        }
        else
        {
            if (PlayerTarget != null&&!pathfinderAgent.ListensForMouseDebug)
            {
                if (Vector2.Distance(transform.position, PlayerTarget.transform.position) > StoppingDistance)
                {
                    Follow();
                }
                else
                {
                    Attack();
                }
            }
        }
    }
    public void TakeDamage(float damage)
    {
        StartCoroutine(ShowRedOnHit());
        LiveHP -= damage;
        // Debug.Log("Monster takes a hit.\n");
        // Debug.Log(hp);
        if (LiveHP < 1)
        {
            Die();
        }
        
    }

    private IEnumerator ShowRedOnHit()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(.15f);
        sprite.color = currentNoDamageColor;
    }

    public void SlowBy(float slow, float slowDuration)
    {
        StartCoroutine(Slow(slow, slowDuration));
    }

    private IEnumerator Slow(float slow, float slowDuration)
    {
        float OGMovementSpeed = pathfinderAgent.MovementSpeed;
        pathfinderAgent.MovementSpeed *= slow;

        currentNoDamageColor = new Color(.5f, .9f, 1f);
        sprite.color = currentNoDamageColor;
        
        yield return new WaitForSeconds(slowDuration);

        pathfinderAgent.MovementSpeed = OGMovementSpeed;
        
        currentNoDamageColor = Color.white;
        sprite.color = currentNoDamageColor;
    }

    public void Die()
    {
        if (isBoss)
        {
            DeathMenu menu = GameObject.FindObjectOfType<DeathMenu>();
            menu.OpenWinMenu();
            gameObject.SetActive(false);
            // switch scenes here...
            Debug.Log("yeah");
        } 
        else
        {
            Destroy(this.gameObject);
            DropEXP();
        }
    }
    public void Despawn()
    {
        if(!isBoss)
        {
            Monster1Manager.MonsterDespawnedDebt++;
            Destroy(gameObject);
        }
    }
    private const int MAX_PATHTENSION = int.MaxValue; // The max amount of temporary extensions allowed before a full recacluation is needed.
    private const int SHORTPATH_CUTOFF = 10; //No Tension Optimization will be performed if the enemy is closer than this from the player.

    public void Follow()
    {
        pathfinderAgent.WhenIdleGoTo = PlayerTarget;
        if(pathfinderAgent.CanSmallRefine(transform.position,PlayerTarget.transform.position))
        {
            if (pathfinderAgent.isCalculatingPath)
            {
                pathfinderAgent.AbortPathRequest();
            }
                pathfinderAgent.movementTargets = new List<Vector2>() { PlayerTarget.transform.position };
        }

        if(!pathfinderAgent.isPathing())
        {
            pathfinderAgent.PathfindTo(PlayerTarget.transform.position);
        }
        else if(!pathfinderAgent.isCalculatingPath)
        {
            Vector2 targPos=  pathfinderAgent.movementTargets[pathfinderAgent.movementTargets.Count - 1];


            if (Vector2.Distance(targPos, PlayerTarget.transform.position) > StoppingDistance)
            {
                if (pathfinderAgent.pathTension >= MAX_PATHTENSION || Vector2.Distance(transform.position, PlayerTarget.transform.position) < SHORTPATH_CUTOFF)
                {
                    pathfinderAgent.PathfindTo(PlayerTarget.transform.position);
                }
                else
                {
                    pathfinderAgent.UnionPathfindTo(PlayerTarget.transform.position);
                }
            }
            
        }
    }
    private void Attack(){
        
        if (Time.time > _nextShot)
        {
            _nextShot = Time.time + _fireDelay;
            player.TakeDamage(strength);
        }
    }

    public void DropEXP(){
        killCounter.kills_amt += EXP;
        player.GainExperience(EXP);
        // Debug.Log("DROP XP");
    }
}
