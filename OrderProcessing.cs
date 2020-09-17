using System;
using System.Collections.Generic;
using System.Text;

namespace IntegratedBulkTicketingSystem
{
    public class OrderProcessing
    {
        public static event EventOrderProcessing ProcessedOrderEvent;

        private static bool CheckCardNumberIsValid(int cardNumber)// (6)
        {
            return cardNumber < 7001 && cardNumber > 4999;
        }

        public static bool ProcessOrder(TicketOrder myOrder, int price)
        {
            if (CheckCardNumberIsValid(myOrder.CardNumber)) // (6)
            {
                //double taxAmount = 1.08;
                //int amountPlusTax = (int)((price * myOrder.NumberOfTickets ) * taxAmount);// (6)
                //ProcessOrderEvent?.Invoke(myOrder, price, amountPlusTax); // (7) 
                //Console.WriteLine($"Printed Order: \n{myOrder.Id} order has been place for {myOrder.NumberOfTickets} at a price of {price} amount after tax is {amountPlusTax}.");// (7)
                int amountPlusTax = CalculateTotalAfterTax(price, myOrder.NumberOfTickets); //(6)
                if (RemoveTicketsFromParkInventory(myOrder.NumberOfTickets))
                {
                    SendOrderConfirmation(myOrder, price, amountPlusTax); // (7)
                    PrintOrder(myOrder, price, amountPlusTax);
                }
                else
                {
                    PrintCancelOrder(myOrder, price, amountPlusTax);
                }
                return true;
            }
            else
            {
                Console.WriteLine($"The card number {myOrder.CardNumber} is not valid, the order for {myOrder.Id} is cancelled.");
                return false;
            }
        }

        private static void PrintCancelOrder(TicketOrder myOrder, int price, int totalAmount)
        {
            Console.WriteLine($"{myOrder.Id} ORDER CANCELLED:\nNot enough tickets available at time of purchase!");
        }

        private static bool RemoveTicketsFromParkInventory(int numberOfTickets)
        {
            return ThemePark.RequestTicketsFromParkInventory(numberOfTickets);
        }

        private static int CalculateTotalAfterTax(int price, int numberOfTickets)
        {
            double taxAmount = 1.08;
            int amountAfterTax = (int)((price *numberOfTickets) * taxAmount);
            return amountAfterTax;
        }

        private static void SendOrderConfirmation(TicketOrder myOrder, int price, int totalAmount)
        {
            ProcessedOrderEvent?.Invoke(myOrder, price, totalAmount);
        }

        private static void PrintOrder(TicketOrder myOrder, int price, int totalAmount)
        {
            Console.WriteLine($"Order Process Thread Printed Order: \n{myOrder.Id} order has been place for {myOrder.NumberOfTickets} tickets at a price of ${price} amount after tax is ${totalAmount}.");// (7)
        }
    }
}
