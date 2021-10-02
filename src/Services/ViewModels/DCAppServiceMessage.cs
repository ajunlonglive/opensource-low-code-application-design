namespace Presentation.ViewModels
{
    public enum DCAppServiceResult
    {
        failed,
        suceeded
    }

    public class DCAppServiceMessage
    {
        public DCAppServiceMessage(string message, DCAppServiceResult result, object data = null)
        {
            Message = message;
            Result = result;
            Data = data;
        }

        public string Message { get; set; }
        public object Data { get; set; }
        public DCAppServiceResult Result { get; set; }
    }
}