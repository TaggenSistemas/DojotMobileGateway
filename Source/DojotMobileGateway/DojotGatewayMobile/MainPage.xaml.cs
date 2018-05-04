using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using OpenNETCF;
using OpenNETCF.MQTT;
using Plugin.BluetoothLE;
using DojotGatewayMobile.Client;
using DojotGatewayMobile.Data.Models;
using DojotGatewayMobile.Data;

namespace DojotGatewayMobile
{
    public partial class MainPage : ContentPage
    {
        private LoginInformation _serverAddressItem;

        public MainPage()
        {
            InitializeComponent();

            _serverAddressItem = App.Database.GetServerAddress();

            if (_serverAddressItem == null)
            {
                _serverAddressItem = new LoginInformation();
                TxtDojot.Text = String.Empty;
            }
            else
            {
                TxtDojot.Text = _serverAddressItem.Address;
                if (_serverAddressItem.Username != null && _serverAddressItem.Password != null)
                {
                    TxtUser.Text = _serverAddressItem.Username;
                    TxtPasswd.Text = _serverAddressItem.Password;
                    swRemember.IsToggled = true;
                }
            }
        }

        private async void OnButtonClicked(object sender, EventArgs e)
        {
            _serverAddressItem.Address = TxtDojot.Text.Trim();

            if (swRemember.IsToggled)
            {
                _serverAddressItem.Username = TxtUser.Text;
                _serverAddressItem.Password = TxtPasswd.Text;
            }
            else
            {
                _serverAddressItem.Username = null;
                _serverAddressItem.Password = null;
            }

            var synced = await SyncData(_serverAddressItem.Address, TxtUser.Text, TxtPasswd.Text);

            if (synced)
            {
                App.Database.SaveServerAddress(_serverAddressItem);

                await this.Navigation.PushAsync(new RunningPage(_serverAddressItem.Address, TxtUser.Text));
            }
            else
            {
                await DisplayAlert("Erro", "Erro ao conectar ao Dojot", "OK");
            }


        }

        private async Task<bool> SyncData(string address, string user, string passwd)
        {
            try
            {
                var client = new DojotRestapi(address);

                var token = await client.LoginAsync(user, passwd);
                var devices = await client.GetDevices(token.Jwt);

                App.Database.DeleteDevices();

                foreach (var device in devices.Devices)
                {
                    var existing = App.Database.GetByDojotId(device.Id);
                    if (existing == null)
                    {
                        var attribs = new List<Attrib>();

                        device.Attribs.TryGetValue(device.Templates.FirstOrDefault().ToString(), out attribs);

                        var instanceObj = attribs.FirstOrDefault(x => x.Label.Equals("Instance"));

                        var instance = (instanceObj == null) ? "" : instanceObj.StaticValue;

                        App.Database.InsertDevice(new DojotDevice()
                        {
                            Label = device.Label,
                            DojotId = device.Id,
                            Instance = instance,
                            isSelected = false
                        });
                    }
                }
            }
            catch (Exception)
            {
                return false; // TODO: Tratamento de erro.
            }
            return true;

        }

    }
}
