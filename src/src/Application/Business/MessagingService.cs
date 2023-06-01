using Application.Interfaces;
using JamaaTech.Smpp.Net.Client;
using JamaaTech.Smpp.Net.Lib;
using Microsoft.Extensions.Logging;

namespace Application.Business;
public class MessagingService: IMessagingService
{
    readonly ILogger<MessagingService> _logger;
    public MessagingService(ILogger<MessagingService> logger)
    {
        _logger = logger;
    }
    public Task<(bool, string)> SendMessage(string receiptNumber)
    {
        try
        {
            var client = new SmppClient();
            client.Properties.SystemID = "xxxx";
            client.Properties.Password = "YYYY";
            client.Properties.Port = 1234; //IP or port to use
            client.Properties.Host ="1.2.3.4"; //SMSC host name or IP Address;
            client.Properties.DefaultEncoding = DataCoding.SMSCDefault;//"EXT_SME";
            client.Properties.AddressNpi = NumberingPlanIndicator.Unknown;
            client.Properties.AddressTon = TypeOfNumber.Unknown;
            //Resume a lost connection after 30 seconds
            client.AutoReconnectDelay = 3000;
            TextMessage send_msg = new TextMessage();
            send_msg.DestinationAddress = receiptNumber; //Receipient number
            send_msg.SourceAddress = "1234"; //Originating number
            send_msg.Text = "Hi brothers";
            send_msg.RegisterDeliveryNotification = true; 
            client.SendMessage(send_msg);
            return Task.FromResult((true,"Success"));
        }
        catch (Exception e)
        {
              EventId Send_Message = new EventId(201, "Error whilst sending sms");
            _logger.LogError(Send_Message, e, "An error occurred whilst sending sms");
            return Task.FromResult((false, e.Message));
        }
   
    }
}