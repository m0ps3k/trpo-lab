namespace TaskApi.Data.Requests;

public class TaskRequest
{
    public string Name { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
}
