using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TheyAreComing : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    private bool fadingAlpha = false;

    [SerializeField] private float fadeSpeed;

    // Start is called before the first frame update
    private void OnEnable()
    {
        StartCoroutine("FadeFont");
    }

    // Update is called once per frame
    void Update()
    {
        if (fadingAlpha)
        {
            text.alpha -= fadeSpeed * Time.deltaTime;
            Debug.Log(text.alpha);
            if (text.alpha <= 0)
            {
                Debug.Log("Done fading text");
                fadingAlpha = false;
                gameObject.SetActive(false);
            }
        }
    }

    IEnumerator FadeFont()
    {
        yield return new WaitForSeconds(2);

        fadingAlpha = true;
    }
}
