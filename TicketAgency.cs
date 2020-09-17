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
            int cardNumber = Rand.Next(5000, 7000);
            int numberOfTicketsRequested = CalculateNumberOfTicketsToBuy(); // (2)
            TicketOrder newOrder = new TicketOrder(numberOfTicketsRequested, cardNumber, id, price); 
            newOrder.Sent = DateTime.Now;
            Console.WriteLine($"Ticket Agency {id} has ordered {numberOfTicketsRequested} tickets, at {newOrder.Sent}.");
            Program.TicketOrderBuffer.setOneItem(newOrder); // (3) Sends order to buffer
            CreateOrderEvent?.Invoke(); // Lets Theme park know an order was created
        }

        public void OrderHasBeenProcessed(TicketOrder myOrder, int price, int amountPlusTax) //(7)
        {
            myOrder.Confirmed = DateTime.Now;
            myOrder.TotalTime = myOrder.Confirmed - myOrder.Sent;
            Console.WriteLine($"{myOrder.Id} has received confirmation at {myOrder.Confirmed:t} order took {myOrder.TotalTime.TotalMilliseconds} Milliseconds to process:\nOrder has been processed, with a ticket Price of ${price} for {myOrder.NumberOfTickets} tickets. The total of the transaction was ${amountPlusTax}.");
        }

        public void TicketsAreOnSale(string id, int price)
        {
            Console.WriteLine($"Ticket Agency {id}: Tickets are on sale for ${price}, we are placing an order.");
            NewOrder(id, price, true);
        }

        private int CalculateNumberOfTicketsToBuy() // (2)
        {
            int ticketPriceDifference = ThemePark.PreviousTicketPrice - ThemePark.CurrentTicketPrice;
            if (ticketPriceDifference < 0)
            {
                return Rand.Next(10, 25);
            }
            else if(ticketPriceDifference < 25)
            {
                return Rand.Next(25, 50);
            }
            else if(ticketPriceDifference < 50)
            {
                return Rand.Next(50, 75);
            }
            else
            {
                return Rand.Next(75, 100);
            }
        }
    }
}
