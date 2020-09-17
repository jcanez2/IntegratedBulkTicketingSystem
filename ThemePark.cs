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
        private const int _maxNumberOfPriceCuts = 10;
        private int _currentNumberOfPriceCuts = 0;
        private static Random _rand = new Random();


        public void ThemeParkOpenForBusiness()
        {
            while (_currentNumberOfPriceCuts < _maxNumberOfPriceCuts)
            {
                Thread.Sleep(_rand.Next(500, 1000));
                ChangePrice();
            }
        }

        private static int GetNewPrice()
        {
            return _rand.Next(100, 200);
        }

        private static void ChangePrice()
        {
            int newPrice = GetNewPrice();
            if (TicketsOnSaleEvent == null)
            {
                if (newPrice < _currentCurrentTicketPrice)
                {
                    TicketsOnSaleEvent(Program.TicketAgencies[index].Name, newPrice);
                    index++;
                    _currentCurrentTicketPrice++;
                }
                _currentCurrentTicketPrice = newPrice;
            }
        }

        public void InitializeOrder() // Event
        {
            TicketOrder currentTicketOrder = Program.TicketOrderBuffer.getOneItem();
            Thread newThread = new Thread(() => OrderProcessing.ProcessOrder(currentTicketOrder, _currentCurrentTicketPrice)); // this should not process at the current price but at the sale price
            newThread.Start();
        }

    }
}
