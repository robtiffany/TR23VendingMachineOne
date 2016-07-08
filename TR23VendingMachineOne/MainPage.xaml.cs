using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Newtonsoft.Json;
using Windows.UI.Popups;
using System.Threading.Tasks;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TR23VendingMachineOne
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        string productOneName = string.Empty;
        int productOneQty;
        double productOnePrice;

        string productTwoName = string.Empty;
        int productTwoQty;
        double productTwoPrice;

        bool exactChangeAlarm;
        double cash;
        int spiralCapacity;

        string command = string.Empty;


        public MainPage()
        {
            this.InitializeComponent();

            InitializeVendingMachine();
        }

        private async void InitializeVendingMachine()
        {

            productOneName = "Jalepeno Chips";
            productOneQty = 10;
            productOnePrice = 1.00;

            productTwoName = "BBQ Chips";
            productTwoQty = 10;
            productTwoPrice = 1.00;

            exactChangeAlarm = false;
            cash = 0;
            spiralCapacity = 10;

            while (true)
            {
                command = await AzureIoTHub.ReceiveCloudToDeviceMessageAsync();
                if(command != "")
                {
                    productOnePrice = Convert.ToDouble(command);
                    txtProductOnePrice.Text = Convert.ToString(productOnePrice);
                    listBoxCommands.Items.Add(productOnePrice);
                }

                await Task.Delay(TimeSpan.FromSeconds(1));
            }


        }

        private async void btnSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                VendingMachine vending = new VendingMachine();
                vending.LastUpdated = DateTime.Now;
                vending.SpiralCapacity = spiralCapacity;
                vending.ProductOneName = productOneName;
                vending.ProductOneQty = productOneQty;
                vending.ProductOnePrice = productOnePrice;
                vending.ProductTwoName = productTwoName;
                vending.ProductTwoQty = productTwoQty;
                vending.ProductTwoPrice = productTwoPrice;
                vending.Cash = cash;
                vending.ExactChangeAlarm = exactChangeAlarm;

                var messageString = JsonConvert.SerializeObject(vending);
                await AzureIoTHub.SendDeviceToCloudMessageAsync(messageString);

                var messageDialog = new MessageDialog("Message Sent Successfully");
                await messageDialog.ShowAsync();


            }
            catch (Exception ex)
            {
                var messageDialog = new MessageDialog(ex.Message, "Error Receiving Commands");
                await messageDialog.ShowAsync();
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            productOneQty = 10;
            txtProductOneQty.Text = Convert.ToString(productOneQty);

            productTwoQty = 10;
            txtProductTwoQty.Text = Convert.ToString(productTwoQty);

            cash = 0;
            txtCashQty.Text = Convert.ToString(cash);
        }

        private void checkBoxChange_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)checkBoxChange.IsChecked)
            {
                exactChangeAlarm = true;
            }
            else
            {
                exactChangeAlarm = false;
            }
        }

        private void btnPurchaseOne_Click(object sender, RoutedEventArgs e)
        {
            if (productOneQty > 0)
            {
                productOneQty -= 1;
                txtProductOneQty.Text = Convert.ToString(productOneQty);

                cash += productOnePrice;
                txtCashQty.Text = Convert.ToString(cash);
            }
        }

        private void btnPurchaseTwo_Click(object sender, RoutedEventArgs e)
        {
            if (productTwoQty > 0)
            {
                productTwoQty -= 1;
                txtProductTwoQty.Text = Convert.ToString(productTwoQty);

                cash += productTwoPrice;
                txtCashQty.Text = Convert.ToString(cash);
            }
        }







    }




}
