using MediatR;

namespace Moist.Application
{
    public interface IHandler<in TIn, TOut> : IRequestHandler<TIn, Response<TOut>>
        where TIn : IRequest<Response<TOut>>
    {}
}