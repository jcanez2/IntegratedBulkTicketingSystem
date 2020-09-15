using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace IntegratedBulkTicketingSystem
{
    class Buffer
    {
        public string[] orderBuffer;
        private const int maxBufferSize = 3;
        private int bufferSize = 0;
        private int numberOfOrders;
        private static Semaphore writeSemaphores; // pool of writing resources
        private static Semaphore readSemaphores; // pool of reading resources

        public Buffer(int neededBufferSize)
        {
            lock (this)
            {
                numberOfOrders = 0;

                if (neededBufferSize <= maxBufferSize)
                {
                    bufferSize = neededBufferSize;
                    writeSemaphores = new Semaphore(bufferSize, bufferSize);
                    readSemaphores = new Semaphore(bufferSize, bufferSize);
                    orderBuffer = new string[bufferSize];

                    for (int i = 0; i < bufferSize; i++)
                    {
                        orderBuffer[i] = "empty"; // Initialize all buffer values with "empty"
                    }
                }
                else
                {
                    Console.WriteLine("Requested buffer size {0} is larger then available resources, max buffer size is {1}.", neededBufferSize, maxBufferSize);
                }

            }
        }

        public string getOneCell()
        {
            readSemaphores.WaitOne();
            string retrievedOrder = "empty Order";

            lock (this)
            {
                while (numberOfOrders == 0) // wait for an order to an order to appear
                {
                    Monitor.Wait(this);
                }

                for (int i = 0; i < bufferSize; i++)
                {
                    if (orderBuffer[i] != "empty") // make sure the order is not empty
                    {
                        retrievedOrder = orderBuffer[i];
                        orderBuffer[i] = "empty";
                        numberOfOrders--;
                        break;
                    }
                }

                readSemaphores.Release();
                Monitor.Pulse(this);
            }

            return retrievedOrder;
        }
        
        public void setOneCell(string newOrder)
        {
            writeSemaphores.WaitOne();

            lock (this)
            {
                while (numberOfOrders == bufferSize) // thread waits for buffer to fill before processing orders
                {
                    Monitor.Wait(this);
                }

                for (int i = 0; i < bufferSize; i++)
                {
                    if (orderBuffer[i] == "empty") // do not overwrite existing order
                    {
                        orderBuffer[i] = newOrder;
                        numberOfOrders++;
                        break;
                    }
                }

                writeSemaphores.Release();
                Monitor.Pulse(this);
            }
        }

    }
}
