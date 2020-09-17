using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace IntegratedBulkTicketingSystem
{
    public class ThemePark
    {
        enum Level
        {
            Somewhat, Highly, Very
        }

        private static int business = 1;
        private static int index = 0;
        private static string _howBusy = "Somewhat";
        private static int _currentCurrentTicketPrice = 150;
        private static int _previousTicketPrice = 150;
        private static int _totalNumberOfTicketsLeft = 2000;
        private static int _totalNumberOfOrders = 0;
        public static event EventTicketsOnSale TicketsOnSaleEvent;

        
        public static int PreviousTicketPrice
        {
            get => _previousTicketPrice;
            set => _previousTicketPrice = value;
        }

        public static int CurrentTicketPrice
        {
            get => _currentCurrentTicketPrice;
            set => _currentCurrentTicketPrice = value;
        }
        private const int MaxNumberOfPriceCuts = 20;
        private static int DayOfTheWeek = 1;
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

        private static int GetNewPrice() // (1) 
        {
            return PriceModel();
        }

        private static int PriceModel()
        {
            int newPrice = 0;
            // Need to implement pricing model
            if (_howBusy == "Very")
            {
                newPrice += 100;
            }
            else if(_howBusy == "Highly")
            {
                newPrice += 75;
            }
            else
            {
                newPrice += 40;
            }

            if (DayOfTheWeek == 6 || DayOfTheWeek == 7)
            {
                newPrice += 100;
            }
            else if (DayOfTheWeek == 4)
            {
                newPrice += 75;
            }
            else
            {
                newPrice += 20;
            }

            if (_totalNumberOfTicketsLeft < 800)
            {
                newPrice += 100;
            }
            else if (_totalNumberOfTicketsLeft < 1400)
            {
                newPrice += 75;
            }
            else
            {
                newPrice += 20;
            }


            ChangeDayOfTheWeek();
            return newPrice;
        }

        

        private static void ChangeDayOfTheWeek()
        {
            CheckIfItIsBusy();
            if (DayOfTheWeek == 7)
            {
                DayOfTheWeek = 1;
            }
            else
            {
                DayOfTheWeek++;
            }
        }

        

        private static void ChangePrice()
        {
            int newPrice = GetNewPrice();
            Console.WriteLine($"\n***There is a price change from ${_currentCurrentTicketPrice} to ${newPrice}, place orders only {_totalNumberOfTicketsLeft} tickets are left at this price and it is {_howBusy} Busy!\n");

            if (index == Program.TicketAgencies.Length) // resets thread index when all are used
            {
                index = 0;
            }

            if (TicketsOnSaleEvent != null) //(1) : if there are any TicketAgencies subscribed  then we will send and event.
            {
                if (newPrice < _currentCurrentTicketPrice)
                {
                    TicketsOnSaleEvent(Program.TicketAgencies[index].Name, newPrice);
                    index++;
                    _currentNumberOfPriceCuts++;
                }
            }
            
            _previousTicketPrice = _currentCurrentTicketPrice;
            _currentCurrentTicketPrice = newPrice;
            IncreaseTicketInventory();
        }

        private static void CheckIfItIsBusy()
        {
            if (_totalNumberOfOrders > 3)
            {
                business = 3;
                _howBusy = "Very";
            }
            else if (_totalNumberOfOrders > 2)
            {
                business = 2;
                _howBusy = "Highly";
            }
            else
            {
                business = 1;
                _howBusy = "Somewhat";
            }

            _totalNumberOfOrders = 0;

        }

        private static void IncreaseTicketInventory()
        {
            if (_totalNumberOfTicketsLeft < 1800)
            {
                _totalNumberOfTicketsLeft += 60;
            }
            else
            {
                _totalNumberOfTicketsLeft = 2000;
            }
        }

        public void InitializeOrder() // Event (5)
        {
            _totalNumberOfOrders++;
            TicketOrder currentTicketOrder = Program.TicketOrderBuffer.getOneItem(); // (4)
            Thread newThread = new Thread(() => OrderProcessing.ProcessOrder(currentTicketOrder, _currentCurrentTicketPrice)); // this should not process at the current price but at the sale price
            newThread.Start();
        }

        public static bool RequestTicketsFromParkInventory(int numberOfTicketsNeeded)
        {
            if (numberOfTicketsNeeded < _totalNumberOfTicketsLeft)
            {
                _totalNumberOfTicketsLeft -= numberOfTicketsNeeded;
                return true;
            }
            else
            {
                return false; // not enough tickets available to compete order.
            }
        }
    }
}
