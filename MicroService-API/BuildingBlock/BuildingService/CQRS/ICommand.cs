using MediatR;

namespace BuildingService.CQRS;

/// <summary>
/// ICommand is a shortcut for ICommand<Unit>, meaning it does not return any meaningful result—just like a "fire-and-forget" command.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface ICommand : ICommand<Unit>
{

}

/// <summary>
/// ICommand<TResponse> extends IRequest<TResponse> from MediatR, which means any 
/// ICommand<T> is a request that expects a response of type T
/// </summary>
public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
