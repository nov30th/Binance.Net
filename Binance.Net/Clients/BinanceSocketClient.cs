using Binance.Net.Clients.CoinFuturesApi;
using Binance.Net.Clients.SpotApi;
using Binance.Net.Clients.UsdFuturesApi;
using Binance.Net.Interfaces.Clients;
using Binance.Net.Interfaces.Clients.CoinFuturesApi;
using Binance.Net.Interfaces.Clients.SpotApi;
using Binance.Net.Interfaces.Clients.UsdFuturesApi;
using Binance.Net.Objects;
using Binance.Net.Objects.Options;
using CryptoExchange.Net;
using Microsoft.Extensions.Logging;
using System;

namespace Binance.Net.Clients
{
    /// <inheritdoc cref="IBinanceSocketClient" />
    public class BinanceSocketClient : BaseSocketClient, IBinanceSocketClient
    {
        #region fields
        #endregion

        #region Api clients

        /// <inheritdoc />
        public IBinanceSocketClientSpotApi SpotApi { get; set; }

        /// <inheritdoc />
        public IBinanceSocketClientUsdFuturesApi UsdFuturesApi { get; set; }

        /// <inheritdoc />
        public IBinanceSocketClientCoinFuturesApi CoinFuturesApi { get; set; }

        #endregion

        #region constructor/destructor
        /// <summary>
        /// Create a new instance of BinanceSocketClient
        /// </summary>
        /// <param name="logger">The logger</param>
        public BinanceSocketClient(ILogger<BinanceSocketClient>? logger = null) : this((x) => { }, logger)
        {
        }

        /// <summary>
        /// Create a new instance of BinanceSocketClient
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public BinanceSocketClient(Action<BinanceSocketOptions> optionsDelegate) : this(optionsDelegate, null)
        {
        }

        /// <summary>
        /// Create a new instance of BinanceSocketClient
        /// </summary>
        /// <param name="logger">The logger</param>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public BinanceSocketClient(Action<BinanceSocketOptions> optionsDelegate, ILogger<BinanceSocketClient>? logger = null) : base(logger, "Binance")
        {
            var options = BinanceSocketOptions.Default.Copy();
            optionsDelegate(options);
            Initialize(options);

            SpotApi = AddApiClient(new BinanceSocketClientSpotApi(_logger, options));
            UsdFuturesApi = AddApiClient(new BinanceSocketClientUsdFuturesApi(_logger, options));
            CoinFuturesApi = AddApiClient(new BinanceSocketClientCoinFuturesApi(_logger, options));
        }
        #endregion

        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public static void SetDefaultOptions(Action<BinanceSocketOptions> optionsDelegate)
        {
            var options = BinanceSocketOptions.Default.Copy();
            optionsDelegate(options);
            BinanceSocketOptions.Default = options;
        }

        /// <inheritdoc />
        public void SetApiCredentials(BinanceApiCredentials credentials)
        {
            SpotApi.SetApiCredentials(credentials);
            UsdFuturesApi.SetApiCredentials(credentials);
            CoinFuturesApi.SetApiCredentials(credentials);
        }
    }
}
