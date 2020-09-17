using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace IntegratedBulkTicketingSystem
{
    public class TicketOrder
    {
        private int _numberOfTickets;
        private int _cardNumber;
        private string _id;
        private int _price;

        public int price
        {
            get => _price;
            set => _price = value;
        }

        public int NumberOfTickets
        {
            get => _numberOfTickets;
            set => _numberOfTickets = value;
        }

        public int CardNumber
        {
            get => _cardNumber;
            set => _cardNumber = value;
        }

        public string Id
        {
            get => _id;
            set => _id = value;
        }

        public TicketOrder(int numberOfTickets, int cardNumber, string id, int price)
        {
            this._numberOfTickets = numberOfTickets;
            this._cardNumber = cardNumber;
            this._id = id;
            this._price = price;
        }

    }
}
