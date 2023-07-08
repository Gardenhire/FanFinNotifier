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

/// <summary>
/// A function that can be triggered in responses to changes in Google Cloud Storage.
/// The type argument (StorageObjectData in this case) determines how the event payload is deserialized.
/// The function must be deployed so that the trigger matches the expected payload type. (For example,
/// deploying a function expecting a StorageObject payload will not work for a trigger that provides
/// a FirestoreEvent.)
/// </summary>
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

