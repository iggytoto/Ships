public class MatchMakingResult
{
    public MatchMakingResult(MatchMakingType type, MatchMakingStatus status)
    {
        Type = type;
        Status = status;
    }

    private MatchMakingType Type { get; }
    private MatchMakingStatus Status { get; }
}