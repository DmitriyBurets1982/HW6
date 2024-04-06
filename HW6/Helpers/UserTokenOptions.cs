using System.ComponentModel.DataAnnotations;

namespace HW6.Helpers
{
    internal sealed class UserTokenOptions
    {
        public const string SettingsSectionName = "TokenOptions";

        [Required]
        internal string PublicKey { get; init; } = null!;

        [Required]
        internal string Issuer { get; init; } = null!;
    }
}
