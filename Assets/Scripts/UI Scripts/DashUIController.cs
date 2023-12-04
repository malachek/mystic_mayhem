using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DashUIController : MonoBehaviour
{
    [SerializeField] PlayerMove pm;
    [SerializeField] TMP_Text textMesh;

    private float currentTimer;

    private const string DASH_READY = "Dash READY";

    // Start is called before the first frame update
    void Start()
    {
        if (pm && textMesh)
        {
            textMesh.text = DASH_READY;
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentTimer = pm.GetDashTimer();   
        if (currentTimer > 0)
        {
            textMesh.text = "Dash in " + Mathf.Ceil(currentTimer) + "...";
        }
        else
        {
            textMesh.text = DASH_READY;
        }
    }
}
