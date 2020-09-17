using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace IntegratedBulkTicketingSystem
{
    public class ItemBuffer
    {
        private const int MaxNumberOfOrders = 3;
        private TicketOrder[] orderBuffer;
        private int _numberOfOrders = 0;
        private static Semaphore _writeSemaphores; // pool of writing resources
        private static Semaphore _readSemaphores; // pool of reading resources

        public ItemBuffer()
        {
            lock (this)
            {
                try
                {
                    _writeSemaphores = new Semaphore(MaxNumberOfOrders, MaxNumberOfOrders);
                    _readSemaphores = new Semaphore(MaxNumberOfOrders, MaxNumberOfOrders);
                    orderBuffer = new TicketOrder[MaxNumberOfOrders];
                    InitiateBuffer();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e} \n Error occurred when constructing ItemBuffer" );
                    throw;
                }
            }
        }

        private void InitiateBuffer()
        {
            for (int i = 0; i < MaxNumberOfOrders; i++)
            {
                orderBuffer[i] = new TicketOrder(0, 0, "empty",0); // set all values to "empty"
            }
        }

        public TicketOrder getOneItem()
        {
            _readSemaphores.WaitOne();
            TicketOrder retrievedOrder = new TicketOrder(0,0,"not a valid order", 0);

            lock (this)
            {
                while (_numberOfOrders <= 0) // wait for an order to an order to appear
                {
                    Monitor.Wait(this);
                }

                for (int i = 0; i < MaxNumberOfOrders; i++)
                {
                    if (orderBuffer[i].Id != "empty") // make sure the order is not empty
                    {
                        retrievedOrder = orderBuffer[i];
                        orderBuffer[i] = new TicketOrder(0,0,"empty", 0);
                        _numberOfOrders--;
                        break;
                    }
                }

                _readSemaphores.Release();
                Monitor.Pulse(this);
            }

            return retrievedOrder;
        }
        
        public void setOneItem(TicketOrder newOrder)
        {
            _writeSemaphores.WaitOne();

            lock (this)
            {
                while (_numberOfOrders >= MaxNumberOfOrders) // thread waits for buffer to have an empty spot fill before processing orders
                {
                    Monitor.Wait(this);
                }

                for (int i = 0; i < MaxNumberOfOrders; i++)
                {
                    if (orderBuffer[i].Id == "empty") // do not overwrite existing order
                    {
                        orderBuffer[i] = newOrder;
                        _numberOfOrders++;
                        break;
                    }
                }

                _writeSemaphores.Release();
                Monitor.Pulse(this);
            }
        }

    }
}
