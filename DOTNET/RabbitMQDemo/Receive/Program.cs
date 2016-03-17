using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Receive
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionFactory _factory = new ConnectionFactory();
            _factory.RequestedHeartbeat = 0;
            _factory.VirtualHost = "/"; //"datecenter_mq";
            _factory.Endpoint = new AmqpTcpEndpoint(new Uri("amqp://192.168.210.115:5672/"));
            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //定义队列（hello为队列名）
                    channel.QueueDeclare("hello", false, false, false, null);

                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume("hello", true, consumer);

                    Console.WriteLine(" [*] Waiting for messages." +
                                             "To exit press CTRL+C");
                    while (true)
                    {
                        //接受客户端发送的消息并打印出来
                        var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(" [x] Received {0}", message);
                    }
                }
            }
        }
    }
}
