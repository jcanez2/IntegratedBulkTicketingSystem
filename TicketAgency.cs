using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace IntegratedBulkTicketingSystem
{
    public class TicketAgency
    {
        public static event EventCreateOrder CreateOrderEvent;
        public static Random Rand = new Random();

        public void StartWorkTicketAgency()
        {
            while (Program.ThemeParkIsOpen)
            {
                Thread.Sleep(Rand.Next(1500, 3000));
                NewOrder(Thread.CurrentThread.Name, ThemePark.CurrentTicketPrice, false);
            }
        }
         
        private void NewOrder(string id, int price, bool isSale)
        {
            int cardNumber = Rand.Next(9000, 9999);
            int numberOfTicketsRequested = Rand.Next(10, 100);

            TicketOrder newOrder = new TicketOrder(numberOfTicketsRequested, cardNumber, id, price);
            Console.WriteLine($"Ticket Agency {id} has ordered {numberOfTicketsRequested}, at {DateTime.Now:t}.");
            Program.TicketOrderBuffer.setOneItem(newOrder); // Sends order to buffer
            CreateOrderEvent?.Invoke(); // Lets Theme park know an order was created
        }

        public void OrderHasBeenProcessed(TicketOrder myOrder, int price, int amountPlusTax)
        {
            Console.WriteLine($"Ticket Agency {myOrder.Id} order has been processed, with a ticket price of ${price} for {myOrder.NumberOfTickets} tickets. The total of the transaction was ${amountPlusTax}, which includes tax.");
        }

        public void TicketsAreOnSale(string id, int price)
        {
            Console.WriteLine($"Ticket Agency {id}: Tickets are on sale for ${price}, we are placing an order.");
            NewOrder(id, price, true);
        }
    }
}
