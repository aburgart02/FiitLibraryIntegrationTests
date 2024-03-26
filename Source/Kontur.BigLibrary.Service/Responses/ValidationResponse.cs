namespace Kontur.BigLibrary.Service.Responses
{
    public class ValidationResponse : ErrorResponse
    {
        public override string Message => "Validation errors";

        public string[] Errors { get; set; }
    }
}