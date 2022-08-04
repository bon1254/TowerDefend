using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase.Database;
using Firebase;
using System.Linq;
using System;

public class FirebaseManager : MonoBehaviour
{
    public string SceneToload = "UserData";
    public static FirebaseManager instance;
    public SceneFader sceneFader;

    [Header("FireBase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth Auth;
    public FirebaseUser User;
    public DatabaseReference DBreference;
    public Text MessegeText;

    [Header("Login")]
    public InputField EmailLoginInput;
    public InputField PasswordLoginInput;
    public bool IsLogin;

    [Header("Register")]
    public InputField UsernameRegisterInput;
    public InputField EmailRegisterInput;
    public InputField PasswordRegisterInput;
    public InputField PasswordRegisterVerifyInput;

    [Header("UserData")]
    public Text UsernameText;
    public Text ExpText;
    public Text KillsText;
    public Text LossText;
    public Text HighestLevelText;
    public GameObject ScoreElement;
    public Transform ScoreboardContent;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }

        //Check that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are avalible Initialize Firebase
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });

        DontDestroyOnLoad(gameObject);
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        //Set the authentication instance object
        Auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    void LoadPlayerData()
    {
        StartCoroutine(LoadUserData());
        Debug.Log("Load Player Local data success!");
    }

    public void ClearLoginFeilds()
    {
        EmailLoginInput.text = "";
        PasswordLoginInput.text = "";
    }
    public void ClearRegisterFeilds()
    {
        UsernameRegisterInput.text = "";
        EmailRegisterInput.text = "";
        PasswordRegisterInput.text = "";
        PasswordRegisterVerifyInput.text = "";
    }

    //Function for the login button
    public void LoginButton()
    {
        StartCoroutine(Login(EmailLoginInput.text, PasswordLoginInput.text));
    }
    //Function for the register button
    public void RegisterButton()
    {
        StartCoroutine(Register(EmailRegisterInput.text, PasswordRegisterInput.text, UsernameRegisterInput.text));
    }

    public void SignOutButton()
    {
        Auth.SignOut();
        MessegeText.text = "你已登出";
        UIManager.instance.SwitchUIScreen("Login");
        UIManager.instance.OpenMessegeUI();
        ClearRegisterFeilds();
        ClearLoginFeilds();
    }

    public void SaveData(int _exp, int _kills, int _loss, int _highestlevel)
    {
        //StartCoroutine(UpdateUsernameAuth(UsernameText.text));
        StartCoroutine(UpdateExp(int.Parse(ExpText.text)));
        StartCoroutine(UpdateKills(int.Parse(KillsText.text)));
        StartCoroutine(UpdateLoss(int.Parse(LossText.text)));
        StartCoroutine(UpdateHighestLevel(int.Parse(HighestLevelText.text)));
        Debug.Log("Sava Data on Cloud successful!");
    }

    //Function for the scoreboard button
    public void ScoreboardButton()
    {
        StartCoroutine(LoadScoreboardData());
    }

    private IEnumerator Login(string _email, string _password)
    {
        //Call the Firebase auth signin function passing the email and password
        var LoginTask = Auth.SignInWithEmailAndPasswordAsync(_email, _password);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "請輸入信箱";
                    break;
                case AuthError.MissingPassword:
                    message = "請輸入密碼";
                    break;
                case AuthError.WrongPassword:
                    message = "密碼錯誤";
                    break;
                case AuthError.InvalidEmail:
                    message = "無效的信箱";
                    break;
                case AuthError.UserNotFound:
                    message = "沒有這個信箱使用者";
                    break;
            }
            UIManager.instance.OpenMessegeUI();
            MessegeText.text = message;
        }
        else
        {
            //User is now logged in
            //Now get the result
            User = LoginTask.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);

            UIManager.instance.OpenMessegeUI();
            MessegeText.text = "已登入";
            IsLogin = true;

            yield return new WaitForSeconds(0.5f);

            UsernameText.text = User.DisplayName;
            LoadPlayerData();
            UIManager.instance.SwitchUIScreen("UserData");
            ClearLoginFeilds();
            ClearRegisterFeilds();
        }
    }

    private IEnumerator Register(string _email, string _password, string _username)
    {
        if (_username == "")
        {
            //If the username field is blank show a warning
            UIManager.instance.OpenMessegeUI();
            MessegeText.text = "請輸入使用者名稱";
        }
        else if (PasswordRegisterInput.text != PasswordRegisterVerifyInput.text)
        {
            //If the password does not match show a warning
            UIManager.instance.OpenMessegeUI();
            MessegeText.text = "兩次密碼輸入不同";
        }
        else
        {
            //Call the Firebase auth signin function passing the email and password
            var RegisterTask = Auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            //Wait until the task completes
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "註冊失敗";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        UIManager.instance.OpenMessegeUI();
                        message = "請輸入信箱";
                        break;
                    case AuthError.MissingPassword:
                        UIManager.instance.OpenMessegeUI();
                        message = "請輸入密碼";
                        break;
                    case AuthError.WeakPassword:
                        UIManager.instance.OpenMessegeUI();
                        message = "密碼強度不足";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        UIManager.instance.OpenMessegeUI();
                        message = "這信箱有人用過了";
                        break;
                }
                MessegeText.text = message;
            }
            else
            {
                //User has now been created
                //Now get the result
                User = RegisterTask.Result;

                if (User != null)
                {
                    //Create a user profile and set the username
                    UserProfile profile = new UserProfile { DisplayName = _username };

                    //Call the Firebase auth update user profile function passing the profile with the username
                    var ProfileTask = User.UpdateUserProfileAsync(profile);
                    //Wait until the task completes
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        //If there are errors handle them
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        UIManager.instance.OpenMessegeUI();
                        MessegeText.text = "使用者名稱設定錯誤!";
                    }
                    else
                    {
                        //Username is now set
                        //Now return to login screen
                        UIManager.instance.SwitchUIScreen("Login");
                        UIManager.instance.OpenMessegeUI();
                        MessegeText.text = "註冊成功!";
                        ClearRegisterFeilds();
                        ClearLoginFeilds();
                    }
                }
            }
        }
    }

    public IEnumerator UpdateUsernameAuth(string _username)
    {
        //Create a user profile and set the username
        UserProfile profile = new UserProfile { DisplayName = _username };

        //Call the Firebase auth update user profile function passing the profile with the username
        var ProfileTask = User.UpdateUserProfileAsync(profile);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

        if (ProfileTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
        }
        else
        {
            //Auth username is now updated
            Debug.Log("Auth username is now updated");
        }
    }

    //玩家暱稱
    public IEnumerator UpdateUsernameDatabase(string _username)
    {
        //Set the currently logged in user username in the database
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("Username").SetValueAsync(_username);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Database username is now updated
            Debug.Log("Database username is now updated");
        }
    }

    //經驗值
    public IEnumerator UpdateExp(int _exp)
    {      
        //Set the currently logged in user Exp
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("Exp").SetValueAsync(_exp);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Exp is now updated
            Debug.Log("Exp is now updated");
        }
    }

    //擊殺數
    public IEnumerator UpdateKills(int _kills)
    {
        //Set the currently logged in user kills
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("Kills").SetValueAsync(_kills);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Kills are now updated
            Debug.Log("Kills are now updated");
        }
    }

    //輸掉的場次
    public IEnumerator UpdateLoss(int _Loss)
    {
        //Set the currently logged in user Loss
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("Loss").SetValueAsync(_Loss);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Loss are now updated
            Debug.Log("Loss are now updated");
        }
    }

    //最高關卡
    public IEnumerator UpdateHighestLevel(int _HighestLevel)
    {
        //Set the currently logged in user Loss
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("HighestLevel").SetValueAsync(_HighestLevel);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //HighestLevel are now updated
            Debug.Log("HighestLevel are now updated");
        }
    }

    private IEnumerator LoadUserData()
    {
        //Get the currently logged in user data
        var DBTask = DBreference.Child("users").Child(User.UserId).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null)
        {
            //No data exists yet
            ExpText.text = "0";
            KillsText.text = "0";
            LossText.text = "0";
            HighestLevelText.text = "0";

            PlayerPrefs.SetInt("Exp", int.Parse(ExpText.text.ToString()));
            PlayerPrefs.SetInt("Kills", int.Parse(KillsText.text.ToString()));
            PlayerPrefs.SetInt("Loss", int.Parse(LossText.text.ToString()));
            PlayerPrefs.SetInt("HighestLevel", int.Parse(HighestLevelText.text.ToString()));
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;

            ExpText.text = snapshot.Child("Exp").Value.ToString();
            KillsText.text = snapshot.Child("Kills").Value.ToString();
            LossText.text = snapshot.Child("Loss").Value.ToString();
            HighestLevelText.text = snapshot.Child("HighestLevel").Value.ToString();

            PlayerPrefs.SetInt("Exp", int.Parse(snapshot.Child("Exp").Value.ToString()));
            PlayerPrefs.SetInt("Kills", int.Parse(snapshot.Child("Kills").Value.ToString()));
            PlayerPrefs.SetInt("Loss", int.Parse(snapshot.Child("Loss").Value.ToString()));
            PlayerPrefs.SetInt("HighestLevel", int.Parse(snapshot.Child("HighestLevel").Value.ToString()));
        }
    }

    private IEnumerator LoadScoreboardData()
    {
        //Get all the users data ordered by kills amount
        var DBTask = DBreference.Child("users").OrderByChild("Kills").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;

            //Destroy any existing scoreboard elements
            foreach (Transform child in ScoreboardContent.transform)
            {
                if (child != null)
                {
                    Destroy(child.gameObject);
                }
            }

            //Loop through every users UID
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse())
            {
                Debug.Log(childSnapshot);

                string Username = childSnapshot.Child("Username").Value.ToString();
                int Kills = int.Parse(childSnapshot.Child("Kills").Value.ToString());
                int Loss = int.Parse(childSnapshot.Child("Loss").Value.ToString());
                int Exp = int.Parse(childSnapshot.Child("Exp").Value.ToString());
                int HighestLevel = int.Parse(childSnapshot.Child("HighestLevel").Value.ToString());

                //Instantiate new scoreboard elements
                GameObject scoreboardElement = Instantiate(ScoreElement, ScoreboardContent);
                scoreboardElement.GetComponent<ScoreElement>().NewScoreElement(Username, Kills, Loss, Exp, HighestLevel);
            }

            //Go to scoareboard screen
            UIManager.instance.SwitchUIScreen("ScoreBoard");
        }
    }
}
