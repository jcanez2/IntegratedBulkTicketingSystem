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
                //int amountPlusTax = (int)((Price * myOrder.NumberOfTickets ) * taxAmount);// (6)
                //ProcessOrderEvent?.Invoke(myOrder, Price, amountPlusTax); // (7) 
                //Console.WriteLine($"Printed Order: \n{myOrder.Id} order has been place for {myOrder.NumberOfTickets} at a Price of {Price} amount after tax is {amountPlusTax}.");// (7)
                int amountPlusTaxAndFees = CalculateTotalAfterTaxAndFees(price, myOrder.NumberOfTickets, 25); //(6)
                if (RemoveTicketsFromParkInventory(myOrder.NumberOfTickets))
                {
                    SendOrderConfirmation(myOrder, price, amountPlusTaxAndFees); // (7)
                    PrintOrder(myOrder, price, amountPlusTaxAndFees);
                }
                else
                {
                    PrintCancelOrder(myOrder, price, amountPlusTaxAndFees);
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

        private static int CalculateTotalAfterTaxAndFees(int price, int numberOfTickets, int fee)
        {
            double taxAmount = 1.08;
            int amountAfterTax = (int)((price *numberOfTickets) * taxAmount);
            int amountAfterTaxAndFee = amountAfterTax + fee;
            return amountAfterTaxAndFee;
        }

        private static void SendOrderConfirmation(TicketOrder myOrder, int price, int totalAmount)
        {
            ProcessedOrderEvent?.Invoke(myOrder, price, totalAmount);
        }

        private static void PrintOrder(TicketOrder myOrder, int price, int totalAmount)
        {
            Console.WriteLine($"Order Process Thread Printed Order: \n{myOrder.Id} order has been place for {myOrder.NumberOfTickets} tickets at a Price of ${price} amount after tax and fees is ${totalAmount}.");// (7)
        }
    }
}
