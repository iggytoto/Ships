using System;
using UnityEngine;

public class PlayFabAuthService : MonoBehaviour, IAuthService
{
    public void RegisterNewUser(
        string login,
        string password,
        Action<string> onSuccessHandler,
        Action<string> onErrorHandler)
    {
        onSuccessHandler?.Invoke("register "+ login + " " + password);
    }

    public void Login(
        string login,
        string password,
        Action<string> onSuccessHandler,
        Action<string> onErrorHandler)
    {
        onErrorHandler?.Invoke("login "+ login + " " + password);
    }
}