using FluentValidation.Results;

namespace deftq.BuildingBlocks.Validation.Webapi.Errors
{
    public sealed class ErrorDetail
    {
        /// <summary>
        ///
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        ///
        /// </summary>
        public string Field { get; set; }
        /// <summary>
        ///
        /// </summary>
        public object AttemptedValue { get; set; }
        /// <summary>
        ///
        /// </summary>
        public string Message { get; set; }

        public ErrorDetail()
        {
            Code = string.Empty;
            Field = string.Empty;
            Message = string.Empty;
            AttemptedValue = string.Empty;
        }

        public ErrorDetail(string code, string message)
        {
            Code = code;
            Message = message;
            Field = string.Empty;
            AttemptedValue = string.Empty;
        }

        public ErrorDetail(ValidationFailure validationFailure)
        {
            if (validationFailure == null)
            {
                throw new ArgumentNullException(nameof(validationFailure));
            }

            Code = validationFailure.ErrorCode;
            Field = validationFailure.PropertyName;
            AttemptedValue = validationFailure.AttemptedValue;
            Message = validationFailure.ErrorMessage;
        }
    }
}
