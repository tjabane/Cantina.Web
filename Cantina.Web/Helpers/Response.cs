using FluentResults;
using FluentValidation.Results;

namespace Cantina.Web.Helpers
{
    public class Response
    {
        public Response(string message)
        {
            Message = message;
            Desciption = string.Empty;
        }

        public Response(List<ValidationFailure> errors)
        {
            Message = "Invalid Request Parameters";
            Desciption = ParseErrors(errors);
        }

        private static string ParseErrors(List<ValidationFailure> errors)
        {
            if(errors is null) return string.Empty;
            return string.Join(", ", errors.Select(error => error.ErrorMessage));
        }
        public string Message { get; set; }
        public string Desciption { get; set; }
    }
}
