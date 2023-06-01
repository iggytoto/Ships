using System;
using PlayFab;
using PlayFab.ClientModels;
using Unity.VisualScripting;
using UnityEngine;

public class PlayFabAuthService : MonoBehaviour, IAuthService
{
    private static string _entityId;
    private static string _entityToken;

    public AuthData GetAuthData()
    {
        return _entityId == null ? null : new AuthData(_entityId, _entityToken);
    }

    public void RegisterNewUser(
        string login,
        string password,
        Action<string> onSuccessHandler,
        Action<string> onErrorHandler)
    {
        var request = new RegisterPlayFabUserRequest()
        {
            Password = password,
            Email = login,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(
            request,
            success => OnRegisterSuccessInternal(onSuccessHandler, success),
            error => OnErrorInternal(onErrorHandler, error));
    }

    public void Login(
        string login,
        string password,
        Action<string> onSuccessHandler,
        Action<string> onErrorHandler)
    {
        var request = new LoginWithEmailAddressRequest()
        {
            Email = login,
            Password = password
        };
        PlayFabClientAPI.LoginWithEmailAddress(
            request,
            success => OnLoginSuccessInternal(onSuccessHandler, success),
            error => OnErrorInternal(onErrorHandler, error));
    }

    private static void OnErrorInternal(Action<string> onErrorHandler, PlayFabError error)
    {
        onErrorHandler?.Invoke(error.ErrorMessage);
    }

    private static void OnRegisterSuccessInternal(Action<string> onSuccessHandler, RegisterPlayFabUserResult success)
    {
        onSuccessHandler?.Invoke("Successfully registered user: " + success.Username);
    }

    private static void OnLoginSuccessInternal(Action<string> onSuccessHandler, LoginResult success)
    {
        _entityId = success.EntityToken.Entity.Id;
        _entityToken = success.EntityToken.EntityToken;
        onSuccessHandler?.Invoke("Login successful with id: " + _entityId);
    }
}