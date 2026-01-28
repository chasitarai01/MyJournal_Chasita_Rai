using Microsoft.Maui.Storage;
using System.Security.Cryptography;
using System.Text;

namespace MyJournal.Services
{
    public class SettingsService
    {
        // ---------------- THEME ----------------
        private const string ThemeKey = "theme";

        public Task<string> GetThemeAsync()
        {
            var t = Preferences.Default.Get(ThemeKey, "light");
            return Task.FromResult(string.IsNullOrWhiteSpace(t) ? "light" : t);
        }

        public Task SetThemeAsync(string theme)
        {
            theme = theme == "dark" ? "dark" : "light";
            Preferences.Default.Set(ThemeKey, theme);
            return Task.CompletedTask;
        }

        // ---------------- PIN LOCK ----------------
        private const string PinHashKey = "pin_hash";
        private const string PinSaltKey = "pin_salt";

        public Task<bool> IsPinSetAsync()
        {
            var hash = Preferences.Default.Get(PinHashKey, "");
            var salt = Preferences.Default.Get(PinSaltKey, "");
            return Task.FromResult(!string.IsNullOrWhiteSpace(hash) && !string.IsNullOrWhiteSpace(salt));
        }

        public Task SetPinAsync(string pin)
        {
            pin = (pin ?? "").Trim();

            // you can change min length if teacher wants
            if (pin.Length < 4)
                throw new ArgumentException("PIN must be at least 4 digits.");

            // generate random salt
            var saltBytes = RandomNumberGenerator.GetBytes(16);
            var salt = Convert.ToBase64String(saltBytes);

            // hash(pin + salt)
            var hash = Hash(pin, salt);

            Preferences.Default.Set(PinSaltKey, salt);
            Preferences.Default.Set(PinHashKey, hash);

            return Task.CompletedTask;
        }

        public Task ClearPinAsync()
        {
            Preferences.Default.Remove(PinSaltKey);
            Preferences.Default.Remove(PinHashKey);
            return Task.CompletedTask;
        }

        public Task<bool> VerifyPinAsync(string pin)
        {
            pin = (pin ?? "").Trim();

            var salt = Preferences.Default.Get(PinSaltKey, "");
            var storedHash = Preferences.Default.Get(PinHashKey, "");

            // no pin set => always unlocked
            if (string.IsNullOrWhiteSpace(salt) || string.IsNullOrWhiteSpace(storedHash))
                return Task.FromResult(true);

            var computedHash = Hash(pin, salt);
            return Task.FromResult(SlowEquals(storedHash, computedHash));
        }

        private static string Hash(string pin, string salt)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(pin + "|" + salt);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        // constant-time compare (better practice)
        private static bool SlowEquals(string a, string b)
        {
            if (a.Length != b.Length) return false;

            var diff = 0;
            for (int i = 0; i < a.Length; i++)
                diff |= a[i] ^ b[i];

            return diff == 0;
        }
        public Task<(string Hash, string Salt)> GetPinDebugAsync()
        {
            var hash = Preferences.Default.Get("pin_hash", "");
            var salt = Preferences.Default.Get("pin_salt", "");
            return Task.FromResult((hash, salt));
        }
        private const string LockEnabledKey = "lock_enabled";

        public Task<bool> IsLockEnabledAsync()
        {
            var enabled = Preferences.Default.Get(LockEnabledKey, false);
            return Task.FromResult(enabled);
        }

        public Task SetLockEnabledAsync(bool enabled)
        {
            Preferences.Default.Set(LockEnabledKey, enabled);
            return Task.CompletedTask;
        }


    }
}
