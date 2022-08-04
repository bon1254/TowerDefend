using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;

public class DataBridge : MonoBehaviour
{
    public DatabaseReference databaseReference;

    public InputField EmailInput, PasswordInput;

    private Player data;

    private string DATA_URL = "https://fps-eed5d-default-rtdb.firebaseio.com/";

}
