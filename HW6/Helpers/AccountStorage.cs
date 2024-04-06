using System.Collections.Concurrent;
using HW6.Models;

namespace HW6.Helpers
{
    public static class AccountStorage
    {
        private static readonly ConcurrentDictionary<int, User> _accounts = new();

        public static User? GetValueOrDefault(int id)
        {
            return _accounts.GetValueOrDefault(id);
        }

        public static User? GetByUserName(string userName)
        {
            return _accounts.Values.FirstOrDefault(x => x.UserName == userName);
        }

        public static void Register(string firstName, string lastName, string userName)
        {
            var userId = _accounts.Count + 1;
            _accounts.TryAdd(userId, new User { Id = userId, FirstName = firstName, LastName = lastName, UserName = userName });
        }
    }
}
