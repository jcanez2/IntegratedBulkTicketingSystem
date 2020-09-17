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

        public TicketOrder(int numberOfTickets, int cardNumber, string id)
        {
            this._numberOfTickets = numberOfTickets;
            this._cardNumber = cardNumber;
            this._id = id;
        }

    }
}
