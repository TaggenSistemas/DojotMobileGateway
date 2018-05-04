using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DojotGatewayMobile.Data.Models;
using Xamarin.Forms;

namespace DojotGatewayMobile.Data
{
    public class LocalDatabase
    {
        static object locker = new object();

        SQLiteConnection database;

        public LocalDatabase()
        {
            var conn = DependencyService.Get<ISQLite>();
            database = conn.GetConnection();

            // Criação das tabelas:
            database.CreateTable<DojotDevice>();
            database.CreateTable<DeviceRead>();
            database.CreateTable<LoginInformation>();
        }

        public List<DojotDevice> GetDevices()
        {
            lock (locker)
            {
                return database.Table<DojotDevice>().ToList();
            }
        }

        public DojotDevice GetByDojotId(string instance)
        {
            lock (locker)
            {
                return database.Table<DojotDevice>().Where(x => x.DojotId.Equals(instance)).FirstOrDefault();
            }
        }

        public DojotDevice GetByInstance(string instance)
        {
            lock (locker)
            {
                return database.Table<DojotDevice>().Where(x => x.Instance.Equals(instance.ToUpper())).FirstOrDefault();
            }
        }

        public void InsertDevice(DojotDevice device)
        {
            lock (locker)
            {
                database.Insert(device);
            }
        }

        public List<DeviceRead> GetDeviceRead()
        {
            lock (locker)
            {
                return database.Table<DeviceRead>().ToList();
            }
        }

        public DeviceRead GetDeviceReadByInstance(string instance)
        {
            lock (locker)
            {
                return database.Table<DeviceRead>().Where(x => x.Instance.Equals(instance)).FirstOrDefault();
            }
        }

        public void SaveDeviceRead(DeviceRead deviceRead)
        {
            lock (locker)
            {
                if (deviceRead.Id == 0)
                    database.Insert(deviceRead);
                else
                    database.Update(deviceRead);
            }
        }

        public void DeleteDevices()
        {
            lock (locker)
            {
                database.Execute("DELETE FROM DojotDevice");
            }
        }

        public void UpdateDojotDevice(DojotDevice device)
        {
            lock (locker)
            {
                database.Update(device);
            }

        }

        public void DeleteDeviceRead(DeviceRead device)
        {
            lock (locker)
            {
                database.Delete(device);
            }
        }

        #region dojotAddress

        public LoginInformation GetServerAddress()
        {
            lock (locker)
            {
                return database.Table<LoginInformation>().FirstOrDefault();
            }
        }

        public void SaveServerAddress(LoginInformation serverAddress)
        {
            lock (locker)
            {
                if (serverAddress.Id == 0)
                    database.Insert(serverAddress);
                else
                    database.Update(serverAddress);
            }
        }

        #endregion

    }
}
