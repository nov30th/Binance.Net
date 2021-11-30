﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Binance.Net.Converters;
using Binance.Net.Enums;
using Binance.Net.Interfaces.Clients.Rest.Spot;
using Binance.Net.Objects.Models.Spot.Blvt;
using CryptoExchange.Net;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Logging;
using CryptoExchange.Net.Objects;
using Newtonsoft.Json;

namespace Binance.Net.Clients.Rest.Spot
{
    /// <summary>
    /// Spot system endpoints
    /// </summary>
    public class BinanceClientSpotLeveragedTokens: IBinanceClientSpotMarketLeveragedTokens
    {
        private const string BlvtApi = "sapi";
        private const string blvtVersion = "1";

        private const string blvtInfoEndpoint = "blvt/tokenInfo";
        private const string blvtSubscribeEndpoint = "blvt/subscribe";
        private const string blvtRedeemEndpoint = "blvt/redeem";
        private const string blvtSubscriptionRecordsEndpoint = "blvt/subscribe/record";
        private const string blvtRedeemRecordsEndpoint = "blvt/redeem/record";

        private const string blvtHistoricalKlinesEndpoint = "lvtKlines";

        private readonly BinanceClientSpotMarket _baseClient;

        internal BinanceClientSpotLeveragedTokens(BinanceClientSpotMarket baseClient)
        {
            _baseClient = baseClient;
        }


        #region Get info

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BinanceBlvtInfo>>> GetBlvtInfoAsync(int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<IEnumerable<BinanceBlvtInfo>>(_baseClient.GetUrl(blvtInfoEndpoint, BlvtApi, blvtVersion), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        #endregion

        #region Subscribe

        /// <inheritdoc />
        public async Task<WebCallResult<BinanceBlvtSubscribeResult>> SubscribeBlvtAsync(string tokenName, decimal cost, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "tokenName", tokenName },
                { "cost", cost.ToString(CultureInfo.InvariantCulture) }
            };
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<BinanceBlvtSubscribeResult>(_baseClient.GetUrl(blvtSubscribeEndpoint, BlvtApi, blvtVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Get subscription records

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BinanecBlvtSubscription>>> GetSubscriptionRecordsAsync(string? tokenName = null, long? id = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            limit?.ValidateIntBetween(nameof(limit), 1, 1000);

            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("tokenName", tokenName);
            parameters.AddOptionalParameter("id", id?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("startTime",  DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime",  DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<IEnumerable<BinanecBlvtSubscription>>(_baseClient.GetUrl(blvtSubscriptionRecordsEndpoint, BlvtApi, blvtVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Redeem 

        /// <inheritdoc />
        public async Task<WebCallResult<BinanceBlvtRedeemResult>> RedeemBlvtAsync(string tokenName, decimal quantity, int? receiveWindow = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "tokenName", tokenName },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
            };
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<BinanceBlvtRedeemResult>(_baseClient.GetUrl(blvtRedeemEndpoint, BlvtApi, blvtVersion), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Get redemption records

        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BinanceBlvtRedemption>>> GetRedemptionRecordsAsync(string? tokenName = null, long? id = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            limit?.ValidateIntBetween(nameof(limit), 1, 1000);

            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("tokenName", tokenName);
            parameters.AddOptionalParameter("id", id?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("startTime",  DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime",  DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<IEnumerable<BinanceBlvtRedemption>>(_baseClient.GetUrl(blvtRedeemRecordsEndpoint, BlvtApi, blvtVersion), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

        #endregion

        #region Get historical klines
        /// <inheritdoc />
        public async Task<WebCallResult<IEnumerable<BinanceBlvtKline>>> GetHistoricalBlvtKlinesAsync(string symbol, KlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
        {
            // TODO check if URL works
            limit?.ValidateIntBetween(nameof(limit), 1, 1000);

            var parameters = new Dictionary<string, object>
            {
                { "symbol", symbol },
                { "interval", JsonConvert.SerializeObject(interval, new KlineIntervalConverter(false)) }
            };
            parameters.AddOptionalParameter("startTime",  DateTimeConverter.ConvertToMilliseconds(startTime));
            parameters.AddOptionalParameter("endTime",  DateTimeConverter.ConvertToMilliseconds(endTime));
            parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

            return await _baseClient.SendRequestInternal<IEnumerable<BinanceBlvtKline>>(_baseClient.GetUrl(blvtHistoricalKlinesEndpoint, "fapi", blvtVersion), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        #endregion
    }
}