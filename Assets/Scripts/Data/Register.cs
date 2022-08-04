using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    public InputField UsernameInput;
    public InputField PasswordInput;
    public InputField ConfirmPasswordInput;
    public InputField EmailInput;
    public Button SubmitButton;

    public GameObject LoginUI;
    public GameObject RegisterUI;

    // Start is called before the first frame update
    void Start()
    {
        SubmitButton.onClick.AddListener(() =>
        {
            StartCoroutine(Main.Instance.web.RegisterUser(UsernameInput.text, PasswordInput.text, ConfirmPasswordInput.text, EmailInput.text));
        });
    }


    public void RegisterActiveUI()
    {
        RegisterUI.SetActive(false);
        LoginUI.SetActive(true);
    }

}