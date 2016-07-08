using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;

static class AzureIoTHub
{
    //
    // Note: this connection string is specific to the device "VendingMachineOne". To configure other devices,
    // see information on iothub-explorer at http://aka.ms/iothubgetstartedVSCS
    //
    const string deviceConnectionString = "HostName=rt3IoTHub.azure-devices.net;DeviceId=VendingMachineOne;SharedAccessKey=Rd9RCWIaHG9YaA3gEDLDQN2O8aIYmqodPT/Z/Oit1uo=";

    //
    // To monitor messages sent to device "VendingMachineOne" use iothub-explorer as follows:
    //    iothub-explorer HostName=rt3IoTHub.azure-devices.net;SharedAccessKeyName=service;SharedAccessKey=oANMWce9ymneEuKC4opJ6BodqBxzIlt95g5lyjbpgzQ= monitor-events "VendingMachineOne"
    //

    // Refer to http://aka.ms/azure-iot-hub-vs-cs-wiki for more information on Connected Service for Azure IoT Hub

    public static async Task SendDeviceToCloudMessageAsync(string str)
    {
        var deviceClient = DeviceClient.CreateFromConnectionString(deviceConnectionString, TransportType.Amqp);

#if WINDOWS_UWP
      //  var str = "Hello, Cloud from a UWP C# app!";
#else
      //  var str = "Hello, Cloud from a C# app!";
#endif
        var message = new Message(Encoding.ASCII.GetBytes(str));

        await deviceClient.SendEventAsync(message);
    }

    public static async Task<string> ReceiveCloudToDeviceMessageAsync()
    {
        var deviceClient = DeviceClient.CreateFromConnectionString(deviceConnectionString, TransportType.Amqp);

        while (true)
        {
            var receivedMessage = await deviceClient.ReceiveAsync();

            if (receivedMessage != null)
            {
                var messageData = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                await deviceClient.CompleteAsync(receivedMessage);
                return messageData;
            }

            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }
}
