namespace Scheduler
{
    public class InvalidTimerStateException : Exception
    {
        private const string ExceptionMessage = "Invalid timer state";

        public InvalidTimerStateException() : base(ExceptionMessage)
        {
        }

        public InvalidTimerStateException(string message) : base(message)
        {
        }
    }
}
