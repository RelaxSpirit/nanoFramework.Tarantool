// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Device.I2c;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using Iot.Device.Bmxx80;
using nanoFramework.Hardware.Esp32;
using nanoFramework.Networking;
using nanoFramework.Tarantool;
using nanoFramework.Tarantool.Model;
using nanoFramework.Tarantool.Model.Requests;

namespace nanoFramework.WeatherTracker
{
    /// <summary>
    /// Program class.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main method.
        /// </summary>
        public static void Main()
        {
            ////const string Ssid = "YourSSID";
            ////const string Password = "YourWifiPassword";
            const string Ssid = "Keenetic-0711";
            const string Password = "UBztXG8x";

            //// ESP-WROOM-32
            Configuration.SetPinFunction(21, DeviceFunction.I2C1_DATA);
            Configuration.SetPinFunction(22, DeviceFunction.I2C1_CLOCK);

            //// XIAO ESP32C3
            ////Configuration.SetPinFunction(6, DeviceFunction.I2C1_DATA);
            ////Configuration.SetPinFunction(7, DeviceFunction.I2C1_CLOCK);

            //// XIAO ESP32C6
            ////Configuration.SetPinFunction(22, DeviceFunction.I2C1_DATA);
            ////Configuration.SetPinFunction(23, DeviceFunction.I2C1_CLOCK);

            CancellationTokenSource cs = new CancellationTokenSource(60000);

            var success = WifiNetworkHelper.ConnectDhcp(Ssid, Password, requiresDateTime: true, token: cs.Token);
            if (!success)
            {
                Debug.WriteLine($"Can't connect to the network, error: {WifiNetworkHelper.Status}");
                if (WifiNetworkHelper.HelperException != null)
                {
                    Debug.WriteLine($"ex: {WifiNetworkHelper.HelperException}");
                }
            }
            else
            {
                // bus id on the MCU
                const int BusId = 1;
                ////const string TarantoolHostIp = "YourTarantoolIpAddress";
                const string TarantoolHostIp = "192.168.1.116";

                var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

                if (networkInterfaces.Length > 0)
                {
                    var physicalAddressBytes = networkInterfaces[0].PhysicalAddress;
                    StringBuilder sb = new StringBuilder(18);
                    foreach (byte b in physicalAddressBytes)
                    {
                        sb.Append($"{b:X2}:");
                    }

                    if (sb.Length > 0)
                    {
                        sb.Remove(sb.Length - 1, 1);
                    }

                    string macAddress = sb.ToString();

                    try
                    {
                        ClientOptions clientOptions = new ClientOptions($"{TarantoolHostIp}:3301");
                        clientOptions.ConnectionOptions.ReadSchemaOnConnect = false;
                        clientOptions.ConnectionOptions.ReadBoxInfoOnConnect = false;
                        clientOptions.ConnectionOptions.WriteStreamBufferSize = 512;
                        clientOptions.ConnectionOptions.ReadStreamBufferSize = 512;

                        using (var box = TarantoolContext.Connect(clientOptions))
                        {
                            var space = box.Schema["weatherTracker"];

                            I2cConnectionSettings i2cSettings = new I2cConnectionSettings(BusId, Bmp280.SecondaryI2cAddress);
                            I2cDevice i2cDevice = I2cDevice.Create(i2cSettings);

                            using (var i2CBmp280 = new Bmp280(i2cDevice))
                            {
                                i2CBmp280.TemperatureSampling = Sampling.LowPower;
                                i2CBmp280.PressureSampling = Sampling.UltraHighResolution;

                                while (true)
                                {
                                    try
                                    {
                                        var readResult = i2CBmp280.Read();
                                        var temperature = readResult.Temperature.DegreesCelsius;
                                        var pressure = readResult.Pressure.Hectopascals;
                                        //// Insert data in to Tarantool
                                        space.Insert(TarantoolTuple.Create(InsertRequest.IncrementKey, macAddress, DateTime.UtcNow, temperature, pressure));
                                    }
                                    catch (Exception ex)
                                    {
                                        Debug.WriteLine($"Error write data result, ex:\n{ex}");
                                    }
                                    finally
                                    {
                                        //// Measured every 10 minutes
                                        Thread.Sleep(60 * 10 * 1000);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"ex: {ex}");
                    }
                }
                else
                {
                    Debug.WriteLine($"Error getting the MAC address");
                }
            }

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
