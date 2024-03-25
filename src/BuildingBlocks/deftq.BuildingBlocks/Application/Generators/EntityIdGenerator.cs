﻿namespace deftq.BuildingBlocks.Application.Generators
{
    public sealed class GuidIdGenerator : IIdGenerator<Guid>
    {
        public Guid Generate()
        {
            return Guid.NewGuid();
        }
    }
}
