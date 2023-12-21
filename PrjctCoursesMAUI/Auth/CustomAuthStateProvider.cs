using Microsoft.AspNetCore.Components.Authorization;
using PrjctCoursesMAUI.States;
using System.Security.Claims;

namespace PrjctCoursesMAUI.Auth
{
    public interface ICustomAuthStateProvider
    {
        Task<AuthenticationState> GetAuthenticationStateAsync();
        Task LogInAsync();
        void Logout();
    }

    public class CustomAuthStateProvider : AuthenticationStateProvider, ICustomAuthStateProvider
    {
        private ClaimsPrincipal currentUser = new ClaimsPrincipal(new ClaimsIdentity());

        AppState _appState;
        public CustomAuthStateProvider(AppState appState)
        {
            _appState = appState;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync() =>
            Task.FromResult(new AuthenticationState(currentUser));

        public Task LogInAsync()
        {
            var loginTask = LogInAsyncCore();
            NotifyAuthenticationStateChanged(loginTask);

            return loginTask;

            async Task<AuthenticationState> LogInAsyncCore()
            {
                var user = await LoginWithExternalProviderAsync();
                currentUser = user;

                _appState.SetUsername("bobby");
                return new AuthenticationState(currentUser);
            }
        }

        private Task<ClaimsPrincipal> LoginWithExternalProviderAsync()
        {
            /*
                Provide OpenID/MSAL code to authenticate the user. See your identity 
                provider's documentation for details.

                Return a new ClaimsPrincipal based on a new ClaimsIdentity.
            */
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, "Thomas"),
            new Claim(ClaimTypes.Role, "Admin")
        };

            var identity = new ClaimsIdentity(claims, "Custom");

            var authenticatedUser = new ClaimsPrincipal(identity);

            return Task.FromResult(authenticatedUser);
        }

        public void Logout()
        {
            currentUser = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(currentUser)));
        }
    }
}
