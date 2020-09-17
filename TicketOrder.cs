using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace IntegratedBulkTicketingSystem
{
    public class TicketOrder
    {
        private int _price;
        private int _cardNumber;
        private string _id;

        public int Price
        {
            get => _price;
            set => _price = value;
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

        public TicketOrder(int price, int cardNumber, string id)
        {
            this._price = price;
            this._cardNumber = cardNumber;
            this._id = id;
        }

    }
}
