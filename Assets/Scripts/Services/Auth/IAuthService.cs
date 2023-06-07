using System;

public interface IAuthService
{
    public ConnectionState ConnectionState { get; }

    public AuthData GetAuthData();

    void RegisterNewUser(string login, string password, Action<String> onSuccessHandler,
        Action<string> onErrorHandler);

    void Login(string login, string password, Action<String> onSuccessHandler,
        Action<string> onErrorHandler);

    void Logout();
}