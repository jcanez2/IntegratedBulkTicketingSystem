using System;
using System.Security.Cryptography.X509Certificates;

namespace IntegratedBulkTicketingSystem
{
    public delegate void EventCreateOrder();

    public delegate void EventOrderProcessing(TicketOrder myOrder, int priceOfTicket, int totalDue);

    public delegate void EventTicketsOnSale(string id, int price);
    
    class Program
    {
        public static ItemBuffer TicketOrderBuffer;
        private static bool _themeParkIsOpen = true;
        public static bool ThemeParkIsOpen
        {
            get => _themeParkIsOpen;
            set => _themeParkIsOpen = value;
        }



        static void Main(string[] args)
        {
            TicketOrderBuffer = new ItemBuffer();


            Console.WriteLine("Hello World!");
        }
    }
}
 