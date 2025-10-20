using Contracts.Events;
using MassTransit;
using Orders.Service;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdminNotification
{

    public class OrderReceivedConsumer : IConsumer<OrderReceived>
    {
        
        public async Task Consume(ConsumeContext<OrderReceived> context)
        {
            Console.WriteLine(context.ReceiveContext.InputAddress);
            AnsiConsole.Write(new Text("OrderReceived Event: I'll jump", new Style(foreground: Color.Red)));
            AnsiConsole.WriteLine();

            string jsonString = JsonSerializer.Serialize(context.Message, new JsonSerializerOptions
            {
                WriteIndented = true,

            });

            // Write the JSON string using Spectre.Console's Markup
            AnsiConsole.Write(new Text(jsonString, new Style(foreground: Color.Red)));
            AnsiConsole.WriteLine();

        }
    }
}
