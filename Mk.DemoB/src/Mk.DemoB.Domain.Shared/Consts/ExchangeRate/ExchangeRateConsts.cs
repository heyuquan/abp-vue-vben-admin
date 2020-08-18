using Leopard.Abp.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mk.DemoB.Consts
{
    public class ExchangeRateConsts
    {
        public const int MaxCurrencyCodeFromLength = StringLengthConsts.MaxCurrencyCodeLength;

        public const int MaxCurrencyCodeToLength = StringLengthConsts.MaxCurrencyCodeLength;

        public const int MaxCaptureBatchNumberLength = ExchangeRateCaptureBatchConsts.MaxCaptureBatchNumberLength;

        public const int MaxDataFromUrlLength = StringLengthConsts.MaxUrlLength;
    }
}
