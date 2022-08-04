using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int Money;
    public int StartMoney = 400;

    public static int Lives;
    public int StartLive = 20;

    public static int Exp;
    public static int Kills;

    public static int Rounds;

    public void Start()
    {
        Money = StartMoney;
        Lives = StartLive;

        
        Exp = 0;
        Kills = 0;
        Rounds = 0;
    }
}
