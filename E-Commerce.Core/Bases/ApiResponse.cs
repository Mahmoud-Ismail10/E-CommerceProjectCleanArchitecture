using System.Net;

namespace E_Commerce.Core.Bases
{
    public class ApiResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public object Meta { get; set; }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public T Data { get; set; }

        public ApiResponse()
        {

        }
        public ApiResponse(T data, string message = null!)
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }
        public ApiResponse(string message)
        {
            Succeeded = true;
            Message = message;
        }
        public ApiResponse(string message, bool succeeded)
        {
            Succeeded = succeeded;
            Message = message;
        }
    }
}
