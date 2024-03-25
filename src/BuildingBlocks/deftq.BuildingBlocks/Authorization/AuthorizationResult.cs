namespace deftq.BuildingBlocks.Authorization
{
    public class AuthorizationResult
    {
        public bool IsAuthorized { get; }
        public string FailureMessage { get; }

        public AuthorizationResult()
        {
            IsAuthorized = false;
            FailureMessage = string.Empty;
        }

        private AuthorizationResult(bool isAuthorized, string failureMessage)
        {
            IsAuthorized = isAuthorized;
            FailureMessage = failureMessage;
        }

        public static AuthorizationResult Fail()
        {
            return new AuthorizationResult(false, string.Empty);
        }

        public static AuthorizationResult Fail(string failureMessage)
        {
            return new AuthorizationResult(false, failureMessage);
        }

        public static AuthorizationResult Succeed()
        {
            return new AuthorizationResult(true, string.Empty);
        }
    }
}
