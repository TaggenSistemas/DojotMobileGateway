using Plugin.BluetoothLE;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DojotGatewayMobile.Client;
using DojotGatewayMobile.Data.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DojotGatewayMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RunningPage : ContentPage
    {
        private Plugin.Geolocator.Abstractions.IGeolocator locator;
        private bool shouldRun;
        private string _dojotAddr;
        private string _user;
        private bool gotLocation;
        private Position position;
        private object logLocker;
        public RunningPage(string dojotAddr, string user)
        {
            _dojotAddr = dojotAddr;
            _user = user;
            gotLocation = false;
            position = new Position();
            logLocker = new object();
            this.Disappearing += WhenClosed;
            this.Appearing += WhenOpened;

            locator = CrossGeolocator.Current;

            InitializeComponent();



        }

        private async Task LocatorThread()
        {
            while (shouldRun)
            {
                position = await locator.GetPositionAsync();
                gotLocation = true;

                await Task.Delay(TimeSpan.FromSeconds(30));
            }
        }


        private void WhenOpened(object sender, EventArgs e)
        {

            this.shouldRun = true;

            CrossBleAdapter.Current.Scan().Subscribe(OnScanResult);
            Task.Run(LocatorThread);
            Task.Run(BeaconsThread);
        }
        private void WhenClosed(object sender, EventArgs e)
        {
            this.shouldRun = false;
        }

        private async Task BeaconsThread()
        {
            while (shouldRun)
            {
                try
                {
                    var beacons = App.Database.GetDeviceRead();
                    foreach (var beacon in beacons)
                    {
                        // Envia ao dojot:

                        var currPos = new Position();

                        currPos = new Position(position.Latitude, position.Longitude);

                        if (gotLocation)
                        {
                            var stretcher = App.Database.GetByInstance(beacon.Instance);

                            var attribs = new StretcherAttribs(currPos.Latitude.ToString().Replace(',', '.') + "," + currPos.Longitude.ToString().Replace(',', '.'), stretcher.IsInAmbulance, beacon.RSSI.ToString());

                            var dojotClient = new DojotMqtt(_dojotAddr, 1883, 10);
                            await dojotClient.MQTTPublish(stretcher.DojotId, attribs, _user);


                            App.Database.DeleteDeviceRead(beacon);
                        }
                    }
                }
                catch (Exception)
                {
                    //TODO - exibir erro
                }

                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }

        void OnScanResult(IScanResult result)
        {
            //Guid a;
            if (result.Device.Name == "TaggenBeacon")
            {
                var frameData = result.AdvertisementData.ServiceData.FirstOrDefault();

                if (frameData == null)
                    return;

                if (frameData.Count() != 22)
                    return;

                if ((frameData[0] != 0xAA) && (frameData[1] != 0xFE) && (frameData[2] != 0x00))
                    return;

                StringBuilder eddystoneNamespace = new StringBuilder(20);

                for (int i = 4; i < 14; i++)
                {
                    eddystoneNamespace.AppendFormat("{0:x2}", frameData[i]);
                }

                StringBuilder eddystoneInstance = new StringBuilder(12);

                for (int i = 14; i < 20; i++)
                {
                    eddystoneInstance.AppendFormat("{0:x2}", frameData[i]);
                }
                var instanceStr = eddystoneInstance.ToString();

                var stretcher = App.Database.GetByInstance(instanceStr);

                if (stretcher != null) // Apenas salva uma leitura caso esse dispositivo exista no Dojot
                {
                    // Atualiza a maca e a ambulancia:

                    App.Database.UpdateDojotDevice(stretcher);

                    DeviceRead deviceRead = App.Database.GetDeviceReadByInstance(instanceStr);
                    if (deviceRead == null)
                    {
                        deviceRead = new DeviceRead();
                        deviceRead.Instance = instanceStr;
                    }

                    deviceRead.RSSI = result.Rssi;
                    deviceRead.LastReadOn = DateTime.Now;

                    Log($"Leu o beacon { deviceRead.Instance }, em { deviceRead.LastReadOn.ToString() }");

                    App.Database.SaveDeviceRead(deviceRead);
                }
            }

        }

        private void Log(string msg)
        {
            lock (logLocker)
            {

                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    lbLog.Text += msg + Environment.NewLine;
                });


            }

        }
    }
}