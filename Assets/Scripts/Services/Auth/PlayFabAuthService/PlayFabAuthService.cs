using System;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayFabAuthService : MonoBehaviour, IAuthService
{
    private static string _entityId;
    private static string _entityToken;
    private static string _login;

    public ConnectionState ConnectionState { get; private set; }

    public AuthData GetAuthData()
    {
        return _entityId == null
            ? null
            : new AuthData
            {
                Id = _entityId,
                Token = _entityToken,
                Login = _login
            };
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
            error => OnErrorRegisterInternal(onErrorHandler, error));
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
            success => OnLoginSuccessInternal(onSuccessHandler, success, login),
            error => OnErrorLoginInternal(onErrorHandler, error));
        ConnectionState = ConnectionState.Connecting;
    }

    public void Logout()
    {
        _entityId = null;
        _entityToken = null;
        ConnectionState = ConnectionState.Disconnected;
    }

    private void OnErrorRegisterInternal(Action<string> onErrorHandler, PlayFabError error)
    {
        onErrorHandler?.Invoke(error.ErrorMessage);
    }

    private void OnErrorLoginInternal(Action<string> onErrorHandler, PlayFabError error)
    {
        onErrorHandler?.Invoke(error.ErrorMessage);
        ConnectionState = ConnectionState.Disconnected;
    }

    private static void OnRegisterSuccessInternal(Action<string> onSuccessHandler, RegisterPlayFabUserResult success)
    {
        onSuccessHandler?.Invoke("Successfully registered user: " + success.Username);
    }

    private void OnLoginSuccessInternal(Action<string> onSuccessHandler, LoginResult success, string login)
    {
        _entityId = success.EntityToken.Entity.Id;
        _entityToken = success.EntityToken.EntityToken;
        _login = login;
        onSuccessHandler?.Invoke("Login successful with id: " + _entityId);
        ConnectionState = ConnectionState.Connected;
    }
}