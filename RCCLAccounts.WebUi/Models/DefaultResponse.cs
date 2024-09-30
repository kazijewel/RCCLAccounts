namespace RCCLAccounts.WebUi.Models
{
    public class DefaultResponse
    {
        public DefaultResponse(string message = "")
        {
            Message = message;
        }
        public string Message { get; set; }
    }
}
