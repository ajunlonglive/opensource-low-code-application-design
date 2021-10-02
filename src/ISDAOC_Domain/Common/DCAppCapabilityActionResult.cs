namespace Domain.Common
{
    public class DCAppCapabilityActionResult
    {
        private readonly bool _isSuceeded;
        private readonly string _message;

        public DCAppCapabilityActionResult(bool isSuceeded, string message)
        {
            _isSuceeded = isSuceeded;
            _message = message;
        }

        public bool IsSuceeded { get { return _isSuceeded; } }

        public string Message { get { return _message; } }
    }
}