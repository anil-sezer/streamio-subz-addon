namespace Subz.Models;

public class UserProfileAccessor
{
    private static readonly AsyncLocal<UserProfile> _currentUser = new();

    public UserProfile UserProfile
    {
        get => _currentUser.Value;
        set => _currentUser.Value = value;
    }
}
