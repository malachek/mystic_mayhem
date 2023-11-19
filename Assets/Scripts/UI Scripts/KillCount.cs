using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KillCount : MonoBehaviour
{

    //UI Text GameObjects

    public GameObject textmeshpro_kills;

    //variables

    public int kills_amt;

    // text components

    TextMeshProUGUI textmeshpro_kills_text;

    // Start is called before the first frame update
    void Start()
    {
        textmeshpro_kills_text = textmeshpro_kills.GetComponent<TextMeshProUGUI>();

    }

    // Update is called once per frame
    void Update()
    {
        //changes kills_amt in the enemyfunctionality class, whenever an enemy is killed
        textmeshpro_kills_text.text = "Kills: " + kills_amt;
    }

}
