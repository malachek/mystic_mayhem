using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveItemScriptableObject", menuName = "ScriptableObjects/Passive Item")]
public class PassiveItemScriptableObject : ScriptableObject
{
    [SerializeField]
    string description;
    [SerializeField]
    float multiplier;
    public float Multiplier { get => multiplier; set => multiplier = value; }

    [SerializeField]
    GameObject nextLevelPrefab; // The prefab of the next level i.e. what the object becomes when it levels up
    public GameObject NextLevelPrefab { get => nextLevelPrefab; private set => nextLevelPrefab = value; }

    [SerializeField]
    int level;
    public int Level { get => level; set => level = value; }

    [SerializeField]
    Sprite icon; // Not meant to be modified in game [Only in Editor]
    public Sprite Icon { get => icon; private set => icon = value; }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
