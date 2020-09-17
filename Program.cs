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
            TicketOrderBuffer = new ItemBuffer();
            TicketAgencies = new Thread[N];

            ThemePark disneyLandThemePark = new ThemePark();
            TicketAgency iSellTicketsAgency = new TicketAgency();

            Thread themeParkWorker = new Thread(new ThreadStart(disneyLandThemePark.ThemeParkOpenForBusiness));
            themeParkWorker.Start();

            ThemePark.TicketsOnSaleEvent += new EventTicketsOnSale(iSellTicketsAgency.TicketsAreOnSale);
            TicketAgency.CreateOrderEvent += new EventCreateOrder(disneyLandThemePark.);

            Console.WriteLine("Hello World!");
        }
    }
}
 