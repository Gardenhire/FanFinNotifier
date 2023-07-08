using CloudNative.CloudEvents;
using Google.Cloud.Functions.Framework;
using Google.Events.Protobuf.Cloud.PubSub.V1;
using System;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using FirebaseAdmin.Messaging;
using FirebaseAdmin;

namespace FanFinNotifier;

public class Function : ICloudEventFunction<MessagePublishedData>
{
    public async Task HandleAsync(CloudEvent cloudEvent, MessagePublishedData data, CancellationToken cancellationToken)
    {
        if (FirebaseApp.DefaultInstance == null)
            FirebaseApp.Create();
        // Construct the message payload
        var message = new Message()
        {
            Notification = new Notification
            {
                Title = data.Message?.Attributes[AttributeConstants.Title],
                Body = data.Message?.Attributes[AttributeConstants.Body]
            },
            Token = data.Message?.Attributes[AttributeConstants.NotificationToken]
    };
        // Send the message
        var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
        Console.WriteLine($"Successfully sent message: {response}");

        return;
    }
}

