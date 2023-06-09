using System;

[Serializable]
public class MatchInstance
{
    public long id;
    public string matchId;
    public string host;
    public string port;
    public MatchInstanceStatus status;
    public MatchType type;  
}