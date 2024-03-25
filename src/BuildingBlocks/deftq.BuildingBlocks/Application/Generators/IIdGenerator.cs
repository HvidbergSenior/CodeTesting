namespace deftq.BuildingBlocks.Application.Generators
{
    public interface IIdGenerator<out T>
    {
        T Generate();
    }
}
