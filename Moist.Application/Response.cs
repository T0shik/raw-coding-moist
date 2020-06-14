namespace Moist.Application
{
    public static class Response
    {
        public static Response<Empty> Empty(bool success) => new Response<Empty>(new Empty(), "", !success);
        public static Response<T> Ok<T>(T data) => new Response<T>(data);
        public static Response<Empty> Ok(string message) => new Response<Empty>(new Empty(), message);
        public static Response<T> Ok<T>(T data, string message) => new Response<T>(data, message);
        public static Response<Empty> Fail(string message) => new Response<Empty>(new Empty(), message, true) ;
        public static Response<T> Fail<T>(string message) => new Response<T>(default, message, true) ;
        public static Response<T> Fail<T>(T data, string message) => new Response<T>(data, message, true);
    }
    public class Response<T>
    {
        public Response(T data, bool error = false)
            : this(data, "", error) {}

        public Response(T data, string message, bool error = false)
        {
            Data = data;
            Message = message;
            Error = error;
        }

        public T Data { get; }
        public bool Error { get; }
        public string Message { get; }
    }
}