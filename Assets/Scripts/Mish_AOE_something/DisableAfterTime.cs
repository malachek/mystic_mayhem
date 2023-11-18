using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterTime : MonoBehaviour
{
    [SerializeField] float cooldown = 4f;
    float timer;
    

    [SerializeField] GameObject aoeCircle;

    private void OnEnable()
    {
        timer = 0;
        
    }

    private void LateUpdate(){
        timer -= Time.deltaTime;
        // Debug.Log("in circle sprite: ");
        // Debug.Log(timer);

        if (timer < 0f){
            aoeCircle.SetActive(true);
            if(Input.GetKey("space")){
                if((gameObject.activeSelf == true)){
                    aoeCircle.SetActive(false);
                    timer = cooldown;
                }
                
            }
        }
    }
}
