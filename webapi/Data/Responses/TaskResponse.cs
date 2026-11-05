namespace TaskApi.Data.Responses;

public class TaskResponse : BaseResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public DateTime Ts { get; set; }
}
