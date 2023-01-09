using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentData : MonoBehaviour
{

    public int money;
    public int potatoesHarvested;
    public bool potatoInstructionsBought;
    public int numSeasons;


    // Start is called before the first frame update
    void Start()
    {
        money = 100;
        potatoesHarvested = 0;
        potatoInstructionsBought = false;
        numSeasons = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
