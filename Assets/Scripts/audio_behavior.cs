using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audio_behavior : MonoBehaviour
{
    [SerializeField] AudioSource audioSourceIntro;
    [SerializeField] AudioSource audioSourceLoop;
    private bool startedLoop;


    void Update()
    {
        if (!audioSourceIntro.isPlaying && !startedLoop)
        {
            audioSourceLoop.Play();
            Debug.Log("Done playing");
            startedLoop = true;
        }
    }

}
