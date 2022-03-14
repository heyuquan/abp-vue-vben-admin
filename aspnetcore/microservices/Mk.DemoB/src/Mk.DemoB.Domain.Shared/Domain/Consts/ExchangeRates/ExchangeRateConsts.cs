using Leopard.Buiness.Consts;

namespace Mk.DemoB.Domain.Consts.ExchangeRates
{
    public class ExchangeRateConsts
    {
        public const int MaxCurrencyCodeFromLength = StringLengthConvention.MaxCurrencyCodeLength;

        public const int MaxCurrencyCodeToLength = StringLengthConvention.MaxCurrencyCodeLength;

        public const int MaxCaptureBatchNumberLength = ExchangeRateCaptureBatchConsts.MaxCaptureBatchNumberLength;

        public const int MaxDataFromUrlLength = StringLengthConvention.MaxUrlLength;
    }
}
