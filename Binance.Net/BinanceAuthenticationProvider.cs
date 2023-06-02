using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using Binance.Net.Objects;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Objects;

namespace Binance.Net
{
    internal class BinanceAuthenticationProvider : AuthenticationProvider<BinanceApiCredentials>
    {
        public string GetApiKey() => _credentials.Key!.GetString();

        public BinanceAuthenticationProvider(BinanceApiCredentials credentials) : base(credentials)
        {
        }

        public override void AuthenticateRequest(RestApiClient apiClient, Uri uri, HttpMethod method, Dictionary<string, object> providedParameters, bool auth, ArrayParametersSerialization arraySerialization, HttpMethodParameterPosition parameterPosition, out SortedDictionary<string, object> uriParameters, out SortedDictionary<string, object> bodyParameters, out Dictionary<string, string> headers)
        {
            uriParameters = parameterPosition == HttpMethodParameterPosition.InUri ? new SortedDictionary<string, object>(providedParameters) : new SortedDictionary<string, object>();
            bodyParameters = parameterPosition == HttpMethodParameterPosition.InBody ? new SortedDictionary<string, object>(providedParameters) : new SortedDictionary<string, object>();
            headers = new Dictionary<string, string>() { { "X-MBX-APIKEY", _credentials.Key!.GetString() } };

            if (!auth)
                return;

            var parameters = parameterPosition == HttpMethodParameterPosition.InUri ? uriParameters : bodyParameters;
            var timestamp = GetMillisecondTimestamp(apiClient);
            parameters.Add("timestamp", timestamp);

            if (_credentials.Type == ApiCredentialsType.Hmac)
            {
                uri = uri.SetParameters(uriParameters, arraySerialization);
                parameters.Add("signature", SignHMACSHA256(parameterPosition == HttpMethodParameterPosition.InUri ? uri.Query.Replace("?", "") : parameters.ToFormData()));
            }
            else
            {
                var parameterString = parameters.ToFormData();
                var data = SignSHA256Bytes(parameterString);

                using var rsa = RSA.Create();
                if (_credentials.Type == ApiCredentialsType.RsaPem)
                {
#if NETSTANDARD2_1_OR_GREATER
                    // Read from pem private key
                    rsa.ImportPkcs8PrivateKey(Convert.FromBase64String(_credentials.Secret!.GetString()), out _);
#else
                    throw new Exception("Pem format not supported when running from .NetStandard2.0. Convert the private key to xml format.");
#endif
                }
                else
                {
                    // Read from xml private key format
                    rsa.FromXmlString(_credentials.Secret!.GetString());
                }

                var rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
                rsaFormatter.SetHashAlgorithm("SHA256");
                var sign = rsaFormatter.CreateSignature(data);

                parameters.Add("signature", Convert.ToBase64String(sign));
            }
        }

        public Dictionary<string, object> AuthenticateSocketParameters(Dictionary<string, object> providedParameters)
        {
            var sortedParameters = new SortedDictionary<string, object>(providedParameters)
            {
                { "apiKey", _credentials.Key!.GetString() },
                { "timestamp", DateTimeConverter.ConvertToMilliseconds(DateTime.UtcNow) }
            };
            var paramString = string.Join("&", sortedParameters.Select(p => p.Key + "=" + Convert.ToString(p.Value, CultureInfo.InvariantCulture)));

            if (_credentials.Type == ApiCredentialsType.Hmac)
            {
                var sign = SignHMACSHA256(paramString);
                var result = sortedParameters.ToDictionary(p => p.Key, p => p.Value);
                result.Add("signature", sign);
                return result;
            }
            else
            {
                var data = SignSHA256Bytes(paramString);

                using var rsa = RSA.Create();
                if (_credentials.Type == ApiCredentialsType.RsaPem)
                {
#if NETSTANDARD2_1_OR_GREATER
                    // Read from pem private key
                    rsa.ImportPkcs8PrivateKey(Convert.FromBase64String(_credentials.Secret!.GetString()), out _);
#else
                    throw new Exception("Pem format not supported when running from .NetStandard2.0. Convert the private key to xml format.");
#endif
                }
                else
                {
                    // Read from xml private key format
                    rsa.FromXmlString(_credentials.Secret!.GetString());
                }

                var rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
                rsaFormatter.SetHashAlgorithm("SHA256");
                var sign = rsaFormatter.CreateSignature(data);

                var result = sortedParameters.ToDictionary(p => p.Key, p => p.Value);
                result.Add("signature", Convert.ToBase64String(sign));
                return result;
            }
        }
    }
}
