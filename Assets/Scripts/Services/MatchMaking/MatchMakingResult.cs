public class MatchMakingResult
{

    public MatchMakingType Type { get;  set; }
    public MatchMakingStatus Status { get;  set; }
    public string RequestId { get;  set; }

    public string ServerAddress { get; set; }
    public string ServerPort { get; set; }
}