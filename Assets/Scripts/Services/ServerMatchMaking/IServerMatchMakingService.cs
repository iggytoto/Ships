using System;

public interface IServerMatchMakingService
{
    public void ApplyAsServer(string host, string port, Action<MatchInstance> onSuccessHandler,
        Action<string> onError);
}