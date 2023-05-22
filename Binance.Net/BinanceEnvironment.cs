using Binance.Net.Objects;
using CryptoExchange.Net.Objects;
using System.Collections.Generic;

namespace Binance.Net
{
    /// <summary>
    /// Binance environments
    /// </summary>
    public class BinanceEnvironment : TradeEnvironment
    {
        /// <summary>
        /// Spot Rest API address
        /// </summary>
        public string SpotRestAddress => Addresses["SpotRest"]!;

        /// <summary>
        /// Spot Socket API address
        /// </summary>
        public string SpotSocketAddress => Addresses["SpotSocket"]!;

        /// <summary>
        /// Blvt Socket API address
        /// </summary>
        public string? BlvtSocketAddress => Addresses["BlvtSocket"];

        /// <summary>
        /// Usd futures Rest address
        /// </summary>
        public string? UsdFuturesRestAddress => Addresses["UsdFuturesRest"];

        /// <summary>
        /// Usd futures Socket address
        /// </summary>
        public string? UsdFuturesSocketAddress => Addresses["UsdFuturesSocket"];

        /// <summary>
        /// Coin futures Rest address
        /// </summary>
        public string? CoinFuturesRestAddress => Addresses["CoinFuturesRest"];

        /// <summary>
        /// Coin futures Socket address
        /// </summary>
        public string? CoinFuturesSocketAddress => Addresses["CoinFuturesSocket"];

        internal BinanceEnvironment(
            string name, 
            string spotRestAddress, 
            string spotSocketAddress, 
            string? blvtSocketAddress, 
            string? usdFuturesRestAddress, 
            string? usdFuturesSocketAddress,
            string? coinFuturesRestAddress,
            string? coinFuturesSocketAddress) :
            base(name, new Dictionary<string, string?>
                {
                    { "SpotRest", spotRestAddress },
                    { "SpotSocket", spotSocketAddress },
                    { "BlvtSocket", blvtSocketAddress },
                    { "UsdFuturesRest", usdFuturesRestAddress },
                    { "UsdFuturesSocket", usdFuturesSocketAddress },
                    { "CoinFuturesRest", coinFuturesRestAddress },
                    { "CoinFuturesSocket", coinFuturesSocketAddress },
                })
        {
        }

        /// <summary>
        /// Live environment
        /// </summary>
        public static TradeEnvironment Live { get; } 
            = new BinanceEnvironment(TradeEnvironmentNames.Live, 
                                     BinanceApiAddresses.Default.RestClientAddress,
                                     BinanceApiAddresses.Default.SocketClientAddress,
                                     BinanceApiAddresses.Default.BlvtSocketClientAddress,
                                     BinanceApiAddresses.Default.UsdFuturesRestClientAddress,
                                     BinanceApiAddresses.Default.UsdFuturesSocketClientAddress,
                                     BinanceApiAddresses.Default.CoinFuturesRestClientAddress,
                                     BinanceApiAddresses.Default.CoinFuturesSocketClientAddress);

        /// <summary>
        /// Testnet environment
        /// </summary>
        public static TradeEnvironment TestNet { get; }
            = new BinanceEnvironment(TradeEnvironmentNames.Testnet,
                                     BinanceApiAddresses.TestNet.RestClientAddress,
                                     BinanceApiAddresses.TestNet.SocketClientAddress,
                                     BinanceApiAddresses.TestNet.BlvtSocketClientAddress,
                                     BinanceApiAddresses.TestNet.UsdFuturesRestClientAddress,
                                     BinanceApiAddresses.TestNet.UsdFuturesSocketClientAddress,
                                     BinanceApiAddresses.TestNet.CoinFuturesRestClientAddress,
                                     BinanceApiAddresses.TestNet.CoinFuturesSocketClientAddress);

        /// <summary>
        /// Binance.us environment
        /// </summary>
        public static TradeEnvironment Us { get; }
            = new BinanceEnvironment("Us",
                                     BinanceApiAddresses.Us.RestClientAddress,
                                     BinanceApiAddresses.Us.SocketClientAddress,
                                     null,
                                     null,
                                     null,
                                     null,
                                     null);

        /// <summary>
        /// Create a custom environment
        /// </summary>
        /// <param name="name"></param>
        /// <param name="spotRestAddress"></param>
        /// <param name="spotSocketAddress"></param>
        /// <param name="blvtSocketAddress"></param>
        /// <param name="usdFuturesRestAddress"></param>
        /// <param name="usdFuturesSocketAddress"></param>
        /// <param name="coinFuturesRestAddress"></param>
        /// <param name="coinFuturesSocketAddress"></param>
        /// <returns></returns>
        public static TradeEnvironment CreateCustom(
                        string name,
                        string spotRestAddress,
                        string spotSocketAddress,
                        string? blvtSocketAddress,
                        string? usdFuturesRestAddress,
                        string? usdFuturesSocketAddress,
                        string? coinFuturesRestAddress,
                        string? coinFuturesSocketAddress)
            => new BinanceEnvironment(name, spotRestAddress, spotSocketAddress, blvtSocketAddress, usdFuturesRestAddress, usdFuturesSocketAddress, coinFuturesRestAddress, coinFuturesSocketAddress);
    }
}
