using System.Collections.Generic;
using System.Net;
using FluentValidation.Results;

namespace deftq.BuildingBlocks.Validation.Webapi.Errors
{
    public sealed class ValidationError : Error
    {
        private const string DefaultErrorMessage = "Validation error";
        /// <summary>
        /// List of validation errors
        /// </summary>
        public IList<ErrorDetail> Errors { get; private set; }

        public ValidationError()
        {
            Errors = new List<ErrorDetail>();
        }

        public ValidationError(IEnumerable<ValidationFailure> ValidationFailures, string traceId, string instance) : base(HttpStatusCode.BadRequest, traceId, instance)
        {
            if (ValidationFailures == null)
            {
                throw new ArgumentNullException(nameof(ValidationFailures));
            }

            Detail = DefaultErrorMessage;

            Errors = new List<ErrorDetail>();
            foreach (ValidationFailure validationFailure in ValidationFailures)
            {
                Errors.Add(new ErrorDetail(validationFailure));
            }
        }
    }
}
