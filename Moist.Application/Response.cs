namespace Moist.Application
{
    public class Response
    {
        protected Response(string message, bool error = false)
        {
            Message = message;
            Error = error;
        }

        public bool Error { get; }
        public string Message { get; }

        public static Response Ok(string message) => new Response(message);
        public static Response<T> Ok<T>(T data, string message) => new Response<T>(data, message);

        public static Response Fail(string message) => new Response(message, true);
        public static Response<T> Fail<T>(T data, string message) => new Response<T>(data, message, true);
    }
}