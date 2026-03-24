namespace TrookApi.DTOs;

public class CreateStringRequest
{
    public required string Key { get; init; }
    
    public required string Localized { get; init; }
}