// (6) The OrderProcessingThread process the order, e.g., checks the credit card number and calculates the total amount.
// (7) The OrderProcessingThread process the order, e.g., sends confirmation to the ticket agency and prints the order (on screen)



using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace IntegratedBulkTicketingSystem
{
    public delegate void EventCreateOrder();

    public delegate void EventOrderProcessing(TicketOrder myOrder, int priceOfTicket, int totalDue);

    public delegate void EventTicketsOnSale(string name, int price);
    
    class Program
    {
        private const int N = 5;
        public static ItemBuffer TicketOrderBuffer;
        public static bool _themeParkIsOpen = true;
        
        public static bool ThemeParkIsOpen
        {
            get => _themeParkIsOpen;
            set => _themeParkIsOpen = value;
        }

        public static Thread[] TicketAgencies;
        

        static void Main(string[] args)
        {
            Console.WriteLine("Start Program:=============================================================");
            // Setup
            TicketOrderBuffer = new ItemBuffer();
            TicketAgencies = new Thread[N];
            ThemePark disneyLandThemePark = new ThemePark();
            TicketAgency iSellTicketsAgency = new TicketAgency();
            // Setup

            // Open Park(s)
            Thread themeParkWorker = new Thread(new ThreadStart(disneyLandThemePark.ThemeParkOpenForBusiness));
            themeParkWorker.Start();

            // Connect Delegates and events
            ThemePark.TicketsOnSaleEvent += new EventTicketsOnSale(iSellTicketsAgency.TicketsAreOnSale);
            TicketAgency.CreateOrderEvent += new EventCreateOrder(disneyLandThemePark.InitializeOrder);
            OrderProcessing.ProcessedOrderEvent += new EventOrderProcessing(iSellTicketsAgency.OrderHasBeenProcessed); //(7)

            // 
            for (int i = 0; i < N; i++)
            {
                TicketAgencies[i] = new Thread(new ThreadStart(iSellTicketsAgency.StartWorkTicketAgency));
                TicketAgencies[i].Name = $"Ticket Agency:{i + 1}";
                TicketAgencies[i].Start();
            }

        }
    }
}
 