using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace IntegratedBulkTicketingSystem
{
    public class ThemePark
    {
        private static int index = 0;
        private static int _currentCurrentTicketPrice = 150;
        public static event EventTicketsOnSale TicketsOnSaleEvent;
        public static int CurrentTicketPrice
        {
            get => _currentCurrentTicketPrice;
            set => _currentCurrentTicketPrice = value;
        }
        private const int MaxNumberOfPriceCuts = 5;
        private static int _currentNumberOfPriceCuts = 0;
        private static Random _rand = new Random();


        public void ThemeParkOpenForBusiness()
        {
            while (_currentNumberOfPriceCuts < MaxNumberOfPriceCuts)
            {
                Thread.Sleep(_rand.Next(500, 1000));
                ChangePrice();
            }

            Program.ThemeParkIsOpen = false;
        }

        private static int GetNewPrice()
        {
            return _rand.Next(100, 200);
        }

        private static void ChangePrice()
        {
            int newPrice = GetNewPrice();
            if (index == Program.TicketAgencies.Length)
            {
                index = 0;
            }
            if (TicketsOnSaleEvent != null)
            {
                if (newPrice < _currentCurrentTicketPrice)
                {
                    TicketsOnSaleEvent(Program.TicketAgencies[index].Name, newPrice);
                    index++;
                    _currentNumberOfPriceCuts++;
                }
            }
            Console.WriteLine($"There is a price change from ${_currentCurrentTicketPrice} to ${newPrice}");
            _currentCurrentTicketPrice = newPrice;
            
        }

        public void InitializeOrder() // Event
        {
            TicketOrder currentTicketOrder = Program.TicketOrderBuffer.getOneItem();
            Thread newThread = new Thread(() => OrderProcessing.ProcessOrder(currentTicketOrder, _currentCurrentTicketPrice)); // this should not process at the current price but at the sale price
            newThread.Start();
        }


        private int myPrices()
        {
            int price = _rand.Next(100, 200);
            return price;
        }
    }
}
