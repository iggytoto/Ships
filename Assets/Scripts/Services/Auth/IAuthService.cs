using System;

public interface IAuthService
{
    public AuthData GetAuthData();

    void RegisterNewUser(string login, string password, Action<String> onSuccessHandler,
        Action<String> OnErrorHandler);

    void Login(string login, string password, Action<String> onSuccessHandler,
        Action<String> OnErrorHandler);
}