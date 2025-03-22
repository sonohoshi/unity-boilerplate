using System;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

namespace Util
{
    public class JwtUtility
    {
        private static string Base64UrlEncode(byte[] bytes)
        {
            return Convert.ToBase64String(bytes)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }

        [Serializable]
        private class Header
        {
            public string alg;
            public string typ;
        }

        [Serializable]
        private class Payload
        {
            public string nonce;
            public long iat;
            public string iss;
            public string aud;
            public long exp;
            public string uid;
        }

        public static string CreateJwt(string secretKey, string nonce, string uid)
        {
            var header = new Header { alg = "HS256", typ = "JWT" };
            var utcNow = DateTimeOffset.UtcNow;
            var exp = utcNow.AddSeconds(5);
            var payload = new Payload
            {
                nonce = nonce,
                iat = utcNow.ToUnixTimeSeconds(),
                iss = "PetPoP_C",
                aud = "PetPoP_S",
                exp = exp.ToUnixTimeSeconds(),
                uid = uid,
            };
            var headerJson = JsonConvert.SerializeObject(header, typeof(Header), Formatting.Indented, new JsonSerializerSettings());
            var payloadJson = JsonConvert.SerializeObject(payload, typeof(Payload), Formatting.Indented, new JsonSerializerSettings());
            var headerEncoded = Base64UrlEncode(Encoding.UTF8.GetBytes(headerJson));
            var payloadEncoded = Base64UrlEncode(Encoding.UTF8.GetBytes(payloadJson));

            var unsignedToken = $"{headerEncoded}.{payloadEncoded}";

            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            using var hmac = new HMACSHA256(keyBytes);
            var signatureBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(unsignedToken));
            var signatureEncoded = Base64UrlEncode(signatureBytes);

            return $"{unsignedToken}.{signatureEncoded}";
        }

        public static string GenerateHmacSignature(string secret, string message)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
            return BitConverter.ToString(hash).Replace("-", "").ToLower(); // HEX 문자열 변환
        }
    }
}