using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Player : MonoBehaviour
{
    public string Username;
    public string Password;

    public Player()
    { 
    
    }

    public Player(string user, string pass)
    {
        Username = name;
        Password = pass;
    }
}
