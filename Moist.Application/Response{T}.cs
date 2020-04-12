namespace Moist.Application
{
    public class Response<T> : Response
    {
        public Response(
            T data,
            string message,
            bool error = false)
            : base(message, error)
        {
            Data = data;
        }

        public T Data { get; }
    }
}