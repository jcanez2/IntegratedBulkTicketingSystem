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
        private double _price;
        private DateTime _sent;
        private DateTime _confirmed;
        private TimeSpan _totalTime;

        public DateTime Sent
        {
            get => _sent;
            set => _sent = value;
        }


        public DateTime Confirmed
        {
            get => _confirmed;
            set => _confirmed = value;
        }

        public TimeSpan TotalTime
        {
            get => _totalTime;
            set => _totalTime = value;
        }

        public double Price
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

        public TicketOrder(int numberOfTickets, int cardNumber, string id, double price)
        {
            this._numberOfTickets = numberOfTickets;
            this._cardNumber = cardNumber;
            this._id = id;
            this._price = price;
        }

    }
}
