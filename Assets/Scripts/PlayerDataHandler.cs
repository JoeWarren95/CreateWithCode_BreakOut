using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataHandler : MonoBehaviour
{
    //the point of this class is just to store the data we want to
    //save from the player, in this case their name and their score
    public static PlayerDataHandler Instance;

    public string PlayerName;
    public int Score;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

}
