using deftq.Pieceworks.Domain;
using Xunit;

namespace deftq.Pieceworks.Test.Domain.BaseRateAndSupplement
{
    public class BaseRateAndSupplementsTests
    {
        [Fact]
        public void GetsBaseRateInterval()
        {
            var baseRateAndSupplement = Pieceworks.Domain.BaseRateAndSupplement.Create(IndirectTimeSupplement.Create(64), SiteSpecificTimeSupplement.Create(2), new List<PersonalTimeSupplementInterval>(){PersonalTimeSupplementInterval.Create(PersonalTimeSupplement.Create(6), DateInterval.Always() )},
                new List<BaseRateInterval>(){BaseRateInterval.Create(BaseRate.Create(214.74m), DateInterval.Create(DateOnly.FromDayNumber(20), DateOnly.FromDayNumber(30)) ), BaseRateInterval.Create(BaseRate.Create(216.56m), DateInterval.Create(DateOnly.FromDayNumber(55), DateOnly.FromDayNumber(80)) ), BaseRateInterval.Create(BaseRate.Create(218.64m), DateInterval.Create(DateOnly.FromDayNumber(55), DateOnly.FromDayNumber(80)) )}, BaseRateRegulation.Create(0));
            baseRateAndSupplement.GetBaseRateInterval(DateOnly.FromDayNumber(20));
            Assert.Equal(214.74m, baseRateAndSupplement.GetBaseRateInterval(DateOnly.FromDayNumber(20)).BaseRate.Value);
        }

        [Fact]
        public void GetsPersonalSupplementInterval()
        {
            var baseRateAndSupplement = Pieceworks.Domain.BaseRateAndSupplement.Create(IndirectTimeSupplement.Create(64), SiteSpecificTimeSupplement.Create(2), new List<PersonalTimeSupplementInterval>(){PersonalTimeSupplementInterval.Create(PersonalTimeSupplement.Create(6), DateInterval.Create(DateOnly.FromDayNumber(10), DateOnly.FromDayNumber(15)) ), PersonalTimeSupplementInterval.Create(PersonalTimeSupplement.Create(8), DateInterval.Create(DateOnly.FromDayNumber(55), DateOnly.FromDayNumber(80)) ), PersonalTimeSupplementInterval.Create(PersonalTimeSupplement.Create(10), DateInterval.Create(DateOnly.FromDayNumber(80), DateOnly.FromDayNumber(90)))},
                new List<BaseRateInterval>(){BaseRateInterval.Create(BaseRate.Create(214.74m), DateInterval.Create(DateOnly.FromDayNumber(20), DateOnly.FromDayNumber(30)) )}, BaseRateRegulation.Create(0));
            baseRateAndSupplement.GetPersonalTimeSupplementInterval(DateOnly.FromDayNumber(10));
            Assert.Equal(6 , baseRateAndSupplement.GetPersonalTimeSupplementInterval(DateOnly.FromDayNumber(10)).PersonalTimeSupplement.Value);
        }
    }
}
