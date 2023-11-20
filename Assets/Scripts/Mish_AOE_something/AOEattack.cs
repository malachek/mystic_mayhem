using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEattack : MonoBehaviour
{
    // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    [SerializeField] float cooldown = 4f;

    public float timer;

    [SerializeField] GameObject aoeCircle;
    // [SerializeField] Vector2 aoeAttackSize = new Vector2(10f,10f);
    [SerializeField] float circleRadius = 3.385f;
    // Update is called once per frame
    [SerializeField] int circleDMG = 10;

    private void OnEnable()
    {
        timer = 0;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if((timer < 0f)){
            if(Input.GetKey("space")){
                Attack();
                
            }
        }
    }

    void Attack(){
        // Debug.Log("Aoe Attack");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(aoeCircle.transform.position, circleRadius);
        timer = cooldown;
        // Debug.Log(colliders.Length);
        ApplyDamage(colliders);
    }

    private void ApplyDamage(Collider2D[] colliders){
        for(int i = 0; i<colliders.Length; i++){
            Enemy e = colliders[i].GetComponent<Enemy>();
            if (e != null){
                colliders[i].GetComponent<Enemy>().TakeDamage(circleDMG);
            }
            
        }
    }
}
