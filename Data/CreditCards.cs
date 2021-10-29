using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RapidPayPayment
{
    public interface ICreditCardRepo
    {
        void AddCreditCard(CreditCard card);
        CreditCard GetCreditCard(string cardNumber);
        bool Pay(string cardNumber, double amount);
    }

    public class CreditCardsRepo : ICreditCardRepo {
        static List<CreditCard> CreditCards = new List<CreditCard>();

        public CreditCard GetCreditCard(string cardNumber)
        {
            return CreditCards.SingleOrDefault(c => c.CardNumber == cardNumber);
        }

        public void AddCreditCard(CreditCard card) {
            CreditCards.Add(card);
        }

        public bool Pay(string cardNumber, double amount) {
            try
            {
                var card = GetCreditCard(cardNumber);
                if (card != null)
                    card.Balance += amount + UniversalFeesExchangeService.Instance.GetFee();
                return true;
            }
            catch {
                return false;
            }
        }
    }
}
