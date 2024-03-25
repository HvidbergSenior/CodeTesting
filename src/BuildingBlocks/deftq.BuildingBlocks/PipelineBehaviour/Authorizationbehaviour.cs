using deftq.BuildingBlocks.Authorization;
using deftq.BuildingBlocks.Exceptions;
using MediatR;

namespace deftq.BuildingBlocks.PipelineBehaviour
{
    public class Authorizationbehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IAuthorizer<TRequest>> authorizers;

        public Authorizationbehaviour(IEnumerable<IAuthorizer<TRequest>> authorizers)
        {
            this.authorizers = authorizers;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            foreach (var authorizer in authorizers)
            {
                var result = await authorizer.Authorize(request, cancellationToken);
                if (!result.IsAuthorized)
                {
                    throw new UnauthorizedException(result.FailureMessage);
                }
            }
            return await next();
        }
    }
}
