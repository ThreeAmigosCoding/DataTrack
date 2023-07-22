namespace DataTrack.Dto;

public class ErrorDto
{
    public string Message { get; set; }

    public ErrorDto()
    {
    }

    public ErrorDto(string message)
    {
        Message = message;
    }
}