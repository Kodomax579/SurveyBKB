using System;

namespace SchulFunk_Webprojekt.Feature.UserHandling
{
    public class UserStateService
    {
        private UserModel? currentUser;

        public event Action? OnChange;

        public UserModel? GetUser()
        {
            return currentUser;
        }

        public void SetUser(UserModel user)
        {
            currentUser = user;
            NotifyStateChanged();
        }

        public void UpdateUser(UserModel updatedUser)
        {
            if (currentUser == null) return;

            currentUser = updatedUser;
            NotifyStateChanged();
        }

        public void ClearUser()
        {
            currentUser = null;
            NotifyStateChanged();
        }

        public bool IsLoggedIn()
        {
            return currentUser != null;
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}