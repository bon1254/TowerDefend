using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public InputField UsernameInput;
    public InputField PasswordInput;
    public Button LoginButton;

    public GameObject LoginUI;
    public GameObject RegisterUI;

    // Start is called before the first frame update
    void Start()
    {
        LoginButton.onClick.AddListener(() =>
        {
            StartCoroutine(Main.Instance.web.Login(UsernameInput.text, PasswordInput.text));
        });
    }

    public void LoginActiveUI()
    {
        RegisterUI.SetActive(true);
        LoginUI.SetActive(false);
    }
}
