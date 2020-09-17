using System;
using System.Collections.Generic;
using System.Text;

namespace IntegratedBulkTicketingSystem
{
    public class OrderProcessing
    {
        public static event EventOrderProcessing ProcessOrderEvent;

        private static bool CheckCardNumberIsValid(int cardNumber)
        {
            return cardNumber < 10000 && cardNumber > 8999;
        }

        public static bool ProcessOrder(TicketOrder myOrder, int price)
        {
            if (CheckCardNumberIsValid(myOrder.CardNumber))
            {
                double taxAmount = 1.08;
                int amountPlusTax = (int)((price * myOrder.NumberOfTickets ) * taxAmount);
                ProcessOrderEvent?.Invoke(myOrder, price, amountPlusTax);
                return true;
            }
            else
            {
                Console.WriteLine($"The card number {myOrder.CardNumber} is not valid, the order for {myOrder.Id} is cancelled.");
                return false;
            }
        }
    }
}
