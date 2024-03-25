namespace deftq.BuildingBlocks.Validation.Webapi.Errors
{
    public static class ValidationErrorCodes
    {
        /// <summary>
        /// Indicates a field is missing 
        /// </summary>
        public const string MissingField = "missing_field_error";
        /// <summary>
        /// Indicates that a value faild to validate 
        /// </summary>
        public const string BadFormat = "bad_format";
    }
}
