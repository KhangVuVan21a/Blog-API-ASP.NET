namespace ASPNet_CoreAPI_Blog.Models.DTO
{
    public class BaseResponseDTO<T>
    {
        private int code { get; set; }
        private bool status { get; set; }
        private string message { get; set; }
        private T data { get; set; }
    }
}
