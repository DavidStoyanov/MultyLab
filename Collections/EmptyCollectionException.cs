namespace Collections
{
    public class EmptyCollectionException : Exception
    {
        private const string ExceptionMessage = "Collection is empty";

        public EmptyCollectionException() : base(ExceptionMessage)
        {
        }

        public EmptyCollectionException(string message) : base(message)
        {
        }
    }
}
