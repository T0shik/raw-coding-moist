using MediatR;

namespace Moist.Application
{
    public interface IQuery<T> : IRequest<Response<T>> { }
}