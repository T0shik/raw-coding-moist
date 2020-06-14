using MediatR;

namespace Moist.Application
{
    public interface ICommand<T> : IRequest<Response<T>> { }
}