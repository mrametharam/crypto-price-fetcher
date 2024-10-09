﻿using CryptoPriceFetcher.Domain.Entities;

namespace CryptoPriceFetcher.Domain.Interfaces;

public interface ICryptoPricesRepository
{
    Task SaveCryptoPrices(IAsyncEnumerable<CryptoPriceRec> cryptoPrices);
    IAsyncEnumerable<CryptoSymbol> GetCryptoSymbols();
    IAsyncEnumerable<CryptoPriceRec> GetCryptoPrice(string cryptoSymbol);
}
