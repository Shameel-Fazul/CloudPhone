using System;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;

using Amazon.Lambda.Core;
using System.Threading.Tasks;
using System.Collections.Generic;
using CloudPhone.models;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace CloudPhone
{
    public class MessageFunction
    {
        public async Task<bool> MessageHandler(MessageModel input, ILambdaContext context)
        {
            string number = input.Number;
            string message = input.Message;

            var client = new AmazonSimpleNotificationServiceClient(region: Amazon.RegionEndpoint.USEast1);
            var request = new PublishRequest
            {
                MessageAttributes = new Dictionary<string, MessageAttributeValue>
                {
                    {"AWS.SNS.SMS.SMSType", new MessageAttributeValue { DataType = "String", StringValue = "Transactional" } }
                },
                Message = message,
                PhoneNumber = number
            };

            try
            {
                var response = await client.PublishAsync(request);
                context.Logger.Log($"[{message}] has been sent to {number}");
                return true;
            }
            catch (Exception ex)
            {
                context.Logger.Log($"There was an error while sending [{message}] to {number}: {ex.Message}");
                return false;
            }
        }
    }
}
