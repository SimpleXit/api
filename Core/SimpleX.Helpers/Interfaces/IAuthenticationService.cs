using System.Collections.Generic;

namespace SimpleX.Helpers.Interfaces
{
    public interface IAuthenticationService
    {
        //void Login();
        //void Logout();
        bool IsAuthenticated { get; }
        string CurrentUsername { get; }
        string CurrentUserLanguageCode { get; }
        byte? CurrentUserStoreID { get; }
        long? CurrentUserId { get; }
        bool UserHasRole(string role);
    }
}