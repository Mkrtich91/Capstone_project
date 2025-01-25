using GameStore.BusinessLayer.Interfaces.Services;

namespace GameStore.BusinessLayer.Services
{
    public class BanService : IBanService
    {
        private readonly Dictionary<string, (TimeSpan Duration, DateTime BanEnd)> _bans = new Dictionary<string, (TimeSpan Duration, DateTime BanEnd)>();

        public void BanUser(string userName, string duration)
        {

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(duration))
            {
                throw new ArgumentException("User and duration are required.");
            }

            TimeSpan banDuration;

            switch (duration.ToLower())
            {
                case "1 hour":
                    banDuration = TimeSpan.FromHours(1);
                    break;
                case "1 day":
                    banDuration = TimeSpan.FromDays(1);
                    break;
                case "1 week":
                    banDuration = TimeSpan.FromDays(7);
                    break;
                case "1 month":
                    banDuration = TimeSpan.FromDays(30);
                    break;
                case "permanent":
                    banDuration = TimeSpan.MaxValue;
                    break;
                default:
                    throw new ArgumentException("Invalid ban duration.");
            }

            if (_bans.ContainsKey(userName))
            {
                _bans.Remove(userName);
            }

            _bans[userName] = (banDuration, DateTime.UtcNow.Add(banDuration));
        }

        public bool IsUserBanned(string userName)
        {
            if (!_bans.TryGetValue(userName, out var ban))
                return false;

            if (ban.Duration == TimeSpan.MaxValue)
                return true;

            if (DateTime.UtcNow >= ban.BanEnd)
            {
                _bans.Remove(userName);
                return false;
            }

            return true;
        }

        public IEnumerable<string> GetBanDurations()
        {
            return new[]
            {
            "1 hour",
            "1 day",
            "1 week",
            "1 month",
            "permanent",
            };
        }
    }
}
