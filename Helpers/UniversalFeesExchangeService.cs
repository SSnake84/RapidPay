using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RapidPayPayment
{
    public sealed class UniversalFeesExchangeService
    {
        private static readonly UniversalFeesExchangeService instance = new UniversalFeesExchangeService();

        private static double _fee = 1;
        private const double UPDATE_TIME = 1000 * 60 * 60; // in minutes
        private static Timer _timer;
        private static Random _random = new Random();

        static UniversalFeesExchangeService()
        {
            _timer = new System.Threading.Timer(
                e => UpdateFee(),
                null,
                TimeSpan.Zero,
                TimeSpan.FromMinutes(60));

            // to start with a different value than 1 during the first hour.
            UpdateFee();
        }

        private static void UpdateFee()
        {
            _fee *= _random.NextDouble() * 2;
        }

        private UniversalFeesExchangeService()
        {
        }

        public static UniversalFeesExchangeService Instance
        {
            get
            {
                return instance;
            }
        }

        public double GetFee()
        {
            return UniversalFeesExchangeService._fee;
        }
    }
}
