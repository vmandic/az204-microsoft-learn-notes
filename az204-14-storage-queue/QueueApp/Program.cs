using System;
using System.Threading.Tasks;
using Azure.Storage.Queues;

namespace QueueApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (!System.IO.File.Exists("queueConnectionString"))
            {
                throw new System.IO.FileNotFoundException("Could not find required file 'queueConnectionString'");
            }

            var queueConnectionString = System.IO.File.ReadAllText("queueConnectionString");
            Console.WriteLine("Using queue connection string: " + queueConnectionString);

            var queue = new QueueClient(queueConnectionString, "mystoragequeue");

            await InsertMessageAsync(queue, "hello world!");

            Console.WriteLine("Message sent away...");

            var message = await queue.ReceiveMessageAsync();

            if (message is not null)
            {
                Console.WriteLine("Popped the latest message from a queue: " + message.Value.Body);
            }

            var messages = await queue.ReceiveMessagesAsync();

            foreach (var m in messages.Value)
            {
                Console.WriteLine("Reading multiple messages returned: " + m.Body);
            }
        }

        private static async Task InsertMessageAsync(QueueClient theQueue, string newMessage)
        {
            if ((await theQueue.CreateIfNotExistsAsync()) is not null)
            {
                Console.WriteLine("The queue was created.");
            }

            await theQueue.SendMessageAsync(newMessage);
        }
    }
}