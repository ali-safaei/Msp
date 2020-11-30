using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class Settings
    {}

    public class JwtSettings
    {
        public string SecretKey { get; } = "very-secret-key-16";
        public string Encryptkey { get; } = "very-secret-_key";
        public string Issuer { get; } = "webapp";
        public string Audience { get; } = "webapp";
        public int NotBeforeMinutes { get; } = 0;
        public int ExpirationMinutes { get; } = 1440;
    }
}
