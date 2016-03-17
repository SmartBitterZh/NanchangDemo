using RabbitMQ.Client;
using RabbitMQ.Client.Content;
using RabbitMQ.Client.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionFactory _factory = new ConnectionFactory();
            _factory.RequestedHeartbeat = 0;
            _factory.VirtualHost = "/"; //"datecenter_mq";
            _factory.Endpoint = new AmqpTcpEndpoint(new Uri("amqp://192.168.210.115:5672/"));
            //factory.Address = "192.168.18.93:5672"; url.Replace("amqp://", "");

            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //定义队列（hello为队列名）
                    channel.QueueDeclare("hello", false, false, false, null);
                    //发送到队列的消息，包含时间戳
                    string message = "Hello World!" + "_" + DateTime.Now.ToString();
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish("", "hello", null, body);
                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }
        }
    }
}
