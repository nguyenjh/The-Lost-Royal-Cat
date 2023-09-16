using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PermanentUI : MonoBehaviour
{
    //Player stats variables
    public int fish = 0;
    public int health = 3;
    public Text fishText;
    public Text healthAmount;

    public static PermanentUI perm;

    //Prevents the fish & health amount from reseting when changing to the next scene/level
    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        //Singleton
        if (!perm)
        {
            perm = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //If player falls off screen or dies, their player stats reset to these amounts
    public void Reset()
    {
        fish = 0;
        fishText.text = fish.ToString();
        health = 3;
        healthAmount.text = health.ToString();
    }
}
