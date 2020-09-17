using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace IntegratedBulkTicketingSystem
{
    class TicketAgency
    {
        public static event EventCreateOrder CreateOrder;
        public static Random Rand = new Random();

        public void StartWorkTicketAgency()
        {
            while (Program.ThemeParkIsOpen)
            {
                Thread.Sleep(Rand.Next(1500, 3000));
                NewOrder(Thread.CurrentThread.Name);
            }
        }

        private void NewOrder(string id)
        {
            int cardNumber = Rand.Next(9000, 9999);
            int numberOfTicketsRequested = Rand.Next(10, 100);

            // todo:  Create order object
            Console.WriteLine($"Ticket Agency {id} has ordered {numberOfTicketsRequested}, at {DateTime.Now:hh:mm:ss t z}.");
             //Program.TicketOrderBuffer.setOneCell("order"); // Sends order to buffer
             CreateOrder?.Invoke(); // Lets Theme park know an order was created
        }

        public void OrderHasBeenProcessed(int id, int totalAmount, int priceOfTickets, int numberOfTickets)
        {
            Console.WriteLine($"Ticket Agency {id} order has been processed, with a ticket price of ${priceOfTickets} for {numberOfTickets} tickets. The total of the transaction was {totalAmount}.");
        }

        public void TicketsAreOnSale(string id, int price)
        {
            Console.WriteLine($"Ticket Agency {id}: Tickets are on sale for ${price}, we are placing an order.");
            NewOrder(id);
        }
    }
}
