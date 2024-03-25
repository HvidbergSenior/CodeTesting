using deftq.Pieceworks.Application.CreateProject;
using Marten;
using Npgsql;

namespace deftq.Pieceworks.Infrastructure
{
    public class SequenceProjectNumberGenerator : IProjectNumberGenerator
    {
        private readonly IDocumentSession _documentSession;

        public SequenceProjectNumberGenerator(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }
        
        public async Task<long> GetNextProjectNumber(CancellationToken cancellationToken)
        {
            await using var sequenceCommand = new NpgsqlCommand("CREATE SEQUENCE IF NOT EXISTS PROJECT_NUMBER_SEQUENCE");
            await _documentSession.ExecuteAsync(sequenceCommand, cancellationToken);
            await using var nextValCommand = new NpgsqlCommand("SELECT nextval('PROJECT_NUMBER_SEQUENCE');");
            await using var reader = await _documentSession.ExecuteReaderAsync(nextValCommand, cancellationToken);
            if (await reader.ReadAsync(cancellationToken))
            {
                return reader.GetInt64(0);
            }

            throw new InvalidOperationException("Not able to generate next project number");
        }
    }
}
