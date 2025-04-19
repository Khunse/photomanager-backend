using System.ComponentModel.DataAnnotations;

namespace imageuploadandmanagementsystem.Model
{
    public class RequestModel<T>
    {
        public T Req { get; set; } = default!;
    }

    public class ResponseModel<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = default!;
        public T Resp { get; set; } = default!;
    }
}