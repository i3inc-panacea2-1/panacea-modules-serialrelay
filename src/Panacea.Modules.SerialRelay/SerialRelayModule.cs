using System;
using System.Threading.Tasks;
using Panacea.Modularity.Relays;
using System.IO.Ports;
using System.Management;
using System.Linq;
using System.Diagnostics;

namespace Panacea.Modules.SerialRelay
{
    class SerialRelayModule : IRelayModule
    {
        private string portName;
        const string DEVICE_NAME = "USB-SERIAL CH340 ";
        const string ON  = "A00101A2";
        const string OFF = "A00100A1";
        public SerialRelayModule()
        {
            using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_PnPEntity"))
            {
                string[] portnames = SerialPort.GetPortNames();
                var ports = searcher.Get().Cast<ManagementBaseObject>().ToList();
                var dev = ports.FirstOrDefault(p => p["Name"]?.ToString().StartsWith(DEVICE_NAME) == true);
                if(dev!= null)
                {
                    Debug.WriteLine(dev["Name"]);
                    portName = dev["Name"].ToString().Substring(DEVICE_NAME.Length + 1, dev["Name"].ToString().Length - DEVICE_NAME.Length - 2);
                }
            }
        }
        public Task<bool> SetStatusAsync(bool on, int port)
        {
            using (var sp = new SerialPort(portName))
            {
                if (sp == null)
                {
                    return Task.FromResult(false);
                }
                sp.Open();
                var hex = on ? ON : OFF;
                var bytes = StringToByteArray(hex);
                sp.Write(bytes, 0, bytes.Length);
                sp.Close();
                return Task.FromResult(true);
            }
        }

        private byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}
