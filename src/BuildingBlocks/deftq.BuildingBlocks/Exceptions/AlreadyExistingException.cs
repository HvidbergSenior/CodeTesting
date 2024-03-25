namespace deftq.BuildingBlocks.Exceptions
{
    [Serializable]
    public class AlreadyExistingException : Exception
    {
        public AlreadyExistingException() { }
        public AlreadyExistingException(string message) : base(message) { }
        public AlreadyExistingException(string message, Exception inner) : base(message, inner) { }
        protected AlreadyExistingException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
