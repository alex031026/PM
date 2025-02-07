using ErrorOr;

namespace PM.Application.Common.Errors
{
    public static partial class Errors
    {
        public static class User
        {
            public static Error DuplicateEmail => Error.Conflict(
                "User.DuplicateEmail",
                "User with same email already exists.");

            public static Error UnavailableProvince => Error.Conflict(
                "User.UnavailableProvince",
                "The Province is not available for the user.");
        }
    }
}
