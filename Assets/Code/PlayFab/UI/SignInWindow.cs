using PlayFab;
using PlayFab.ClientModels;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SignInWindow : AccountDataWindowBase
{
    [SerializeField]
    private Button _signInButton;

    [SerializeField]
    private Canvas _waitCanvas;
    private bool _error;

    protected override void SubscriptionElementsUI()
    {
        base.SubscriptionElementsUI();

        _signInButton.onClick.AddListener(Test);
    }

    private void Test()
    {
        AsyncSignIn();
    }

    private void SignIn()
    {
        _error = false;
        var request = new LoginWithPlayFabRequest
        {
            Username = _username,
            Password = _password,
        };

        PlayFabClientAPI.LoginWithPlayFab(request, ProcessResult, ProcessError);
    }

    async Task AsyncSignIn()
    {
        SignIn();
        DisplayWaitSign(true);
        
        while (!PlayFabClientAPI.IsClientLoggedIn() && !_error)
        {
            await Task.Yield();

        }
        DisplayWaitSign(false);

        Debug.Log($"wait is finished");
    }

    private void DisplayWaitSign(bool v)
    {
        _waitCanvas.enabled = v;
    }

    private void ProcessResult(LoginResult result)
    {
        Debug.Log($"Sign in succces: {_username}");
        EnterInGameScene();
    }

    private void ProcessError(PlayFabError error)
    {
        _error = true;
        var result = error.GenerateErrorReport();
        Debug.Log($"Error!\n{result}");

    }
}
