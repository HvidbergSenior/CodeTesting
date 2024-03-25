﻿using MediatR;

namespace deftq.BuildingBlocks.Application.Queries
{

    public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
           where TQuery : IQuery<TResponse>
    {
    }
}
