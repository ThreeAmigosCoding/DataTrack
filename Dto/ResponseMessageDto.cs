namespace DataTrack.Dto;

public class ResponseMessageDto
{
    public string Message { get; set; }

    public ResponseMessageDto()
    {
    }

    public ResponseMessageDto(string message)
    {
        Message = message;
    }
}