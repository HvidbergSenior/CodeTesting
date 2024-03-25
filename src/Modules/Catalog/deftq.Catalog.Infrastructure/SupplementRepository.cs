using System.Collections.ObjectModel;
using deftq.BuildingBlocks.DataAccess.Marten;
using deftq.BuildingBlocks.Events;
using deftq.Catalog.Domain.SupplementCatalog;
using Marten;

namespace deftq.Catalog.Infrastructure
{
    public class SupplementRepository : MartenDocumentRepository<Supplement>, ISupplementRepository
    {
        private IList<Supplement> _data = new List<Supplement>(); 
        
        public SupplementRepository(IDocumentSession documentSession, IEntityEventsPublisher aggregateEventsPublisher) : base(documentSession, aggregateEventsPublisher)
        {
            _data.Add(Create("C0C64FCC-D855-43ED-B808-9094C3D60646", "bah25", "Begrænset arbejdshøjde fra 100 t.o.m. 150cm v/ fremspring over 50cm 25%", 25));
            _data.Add(Create("28AD88AD-C66B-49A8-88DF-CAFAB2A87290", "bah75", "Begrænset arbejdshøjde fra 50 t.o.m. 100cm v/ fremspring over 20cm 75%", 75));
            _data.Add(Create("D7C155D2-FD39-49E7-819F-F6AB49006405", "bmi30", "Beton med min. 15mm isolationslag 30%", 30));
            _data.Add(Create("14FE0200-F0AE-4610-80BF-CD08A83DA3AE", "ht10", "Højdetillæg 3,5 mtr. indtil 5 mtr. 10%", 10));
            _data.Add(Create("5274F918-DDA9-416D-890A-A40D2413E8CE", "ht25", "Højdetillæg 5mtr. indtil 7mtr. 25%", 25));
            _data.Add(Create("CBBF2708-68F3-4B80-ABD7-0ACC3E910EC5", "ht40", "Højdetillæg 7mtr. indtil 10mtr. 40%", 40));
            _data.Add(Create("B4FB06DC-C628-4FBF-850F-5B3421EB0C3C", "lht20", "Lavhøjdetillæg fra 125 t.o.m. 150cm 20%", 20));
            _data.Add(Create("2A122516-EDD0-421D-B069-9C5900E4CDF7", "lht30", "Lavhøjdetillæg fra 100 t.o.m. 125cm 30%", 30));
            _data.Add(Create("3DD00F5D-35F8-4D0A-9943-3ECA2C0BB95E", "lht50", "Lavhøjdetillæg fra 75 t.o.m. 100cm 50%", 50));
            _data.Add(Create("2D8DEE0D-FA76-4C68-A13C-22E168D29E42", "lht75", "Lavhøjdetillæg fra 50 t.o.m. 75cm 75%", 75));
        }

        private Supplement Create(string guid, string number, string text, decimal value)
        {
            return Supplement.Create(SupplementId.Create(Guid.Parse(guid)), SupplementNumber.Create(number), SupplementText.Create(text),
                SupplementValue.Create(value));
        }
        
        public Task<IReadOnlyList<Supplement>> GetAllAsync(CancellationToken cancellationToken)
        {
            IReadOnlyList<Supplement> result = new ReadOnlyCollection<Supplement>(_data);
            return Task.FromResult(result);
        }
    }
}
