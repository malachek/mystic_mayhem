using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterTime : MonoBehaviour
{
    [SerializeField] float cooldown = 4f;
    float timer;
    

    [SerializeField] GameObject aoeCircle;

    // private float opacity_fun;
    private void OnEnable()
    {
        timer = 0;
        // opacity_fun = 0f;
    }

    private void LateUpdate(){
        timer -= Time.deltaTime;
        // Debug.Log("in circle sprite: ");
        // Debug.Log(timer);
        // opacity_fun = opacity_fun + 0.001f;
        // GetComponent<Renderer>().material.color.a = opacity_fun;
        if (timer < 0f){
            // opacity_fun = 0;
            // GetComponent<Renderer>().material.color.a = 1f;

            aoeCircle.SetActive(true);
            if(Input.GetKey("space")){
                if((gameObject.activeSelf == true)){

                    // aoeCircle.SetActive(false);
                    timer = cooldown;
                }
                
            }
        }
    }


}
