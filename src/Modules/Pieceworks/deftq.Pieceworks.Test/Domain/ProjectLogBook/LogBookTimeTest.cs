using deftq.Pieceworks.Domain.projectLogBook;
using Xunit;

namespace deftq.Pieceworks.Test.Domain.projectLogBook
{
    public class LogBookTimeTest
    {
        [Fact]
        public void InvalidTime()
        {
            Assert.Throws<ArgumentException>(() => LogBookTime.Create(LogBookHours.Create(0), LogBookMinutes.Create(-1)));
            Assert.Throws<ArgumentException>(() => LogBookTime.Create(LogBookHours.Create(-1), LogBookMinutes.Create(0)));
            Assert.Throws<ArgumentException>(() => LogBookTime.Create(LogBookHours.Create(0), LogBookMinutes.Create(60)));
        }

        [Fact]
        public void Add_Hours()
        {
            var time1 = LogBookTime.Create(LogBookHours.Create(5), LogBookMinutes.Create(0));
            var time2 = LogBookTime.Create(LogBookHours.Create(7), LogBookMinutes.Create(0));
            
            Assert.Equal(12, time1.Add(time2).GetHours().Value);
        }
        
        [Fact]
        public void Add_Minutes()
        {
            var time1 = LogBookTime.Create(LogBookHours.Create(0), LogBookMinutes.Create(12));
            var time2 = LogBookTime.Create(LogBookHours.Create(0), LogBookMinutes.Create(45));
            
            Assert.Equal(57, time1.Add(time2).GetMinutes().Value);
        }
        
        [Fact]
        public void Overflow_Minutes()
        {
            var time1 = LogBookTime.Create(LogBookHours.Create(2), LogBookMinutes.Create(42));
            var time2 = LogBookTime.Create(LogBookHours.Create(3), LogBookMinutes.Create(45));

            var result = time1.Add(time2);
            Assert.Equal(6, result.GetHours().Value);
            Assert.Equal(27, result.GetMinutes().Value);
        }
        
        [Fact]
        public void Overflow_Minutes_Full_Hours()
        {
            var time1 = LogBookTime.Create(LogBookHours.Create(2), LogBookMinutes.Create(42));
            var time2 = LogBookTime.Create(LogBookHours.Create(3), LogBookMinutes.Create(18));

            var result = time1.Add(time2);
            Assert.Equal(6, result.GetHours().Value);
            Assert.Equal(0, result.GetMinutes().Value);
        }
    }
}
