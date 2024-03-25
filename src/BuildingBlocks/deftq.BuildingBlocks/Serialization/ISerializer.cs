namespace deftq.BuildingBlocks.Serialization
{
    public interface ISerializer<T>
    {
        string Serialize(T obj);
        T? Deserialize(string json);
    }
}