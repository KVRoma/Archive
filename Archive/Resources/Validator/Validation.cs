using Archive.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Archive.Resources.Validator
{
    public class Validation
    {
        public Validation()
        {            
        }

        public string Key;
        public string Message;
        public string Path;        

        DateTime today;  
        DateTime start;
        DateTime end;
        bool isInternet = true;
        
        public bool IsValid()
        {
            try
            {
                string decryptoToString = KeysGenerator.TripleDESImp.TripleDesDecrypt(KeysGenerator.FilesText.ReadFileText(Path), Key);
                string[] words = decryptoToString.Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries);

                if (ValidationDate(words))
                {                    
                    return true;
                }
                else
                {
                    if (isInternet)
                    {
                        Message = "      Вийшов термін дії ліцензії !" + Environment.NewLine +
                                  "Контакт для отримання ліцензії:" + Environment.NewLine +
                                  "     E-mail: kvroma83@gmail.com " + Environment.NewLine +
                                  "          tel : +38(068)104-18-24";                        
                        return false;
                    }
                    else
                    {
                        Message = "Перевірте підключення Internet...";
                        return false;
                    }
                }
                
            }
            catch
            {                
                Message = "      Вийшов термін дії ліцензії !" + Environment.NewLine +
                          "Контакт для отримання ліцензії:" + Environment.NewLine +
                          "     E-mail: kvroma83@gmail.com " + Environment.NewLine +
                          "          tel : +38(068)104-18-24";
                return false;
            }          
            
        }
        private bool ValidationDate(string[] keysString)
        {
            try
            {
                today = GetNetworkTime();
            }
            catch //(Exception ex)
            {
                isInternet = false;
            }

            if (isInternet && DateTime.TryParse(keysString[0], out start) && DateTime.TryParse(keysString[1], out end))
            {
                if (today >= start && DateTime.Today <= end)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        
        private DateTime GetNetworkTime()
        {
            const string ntpServer = "time.windows.com";
            var ntpData = new byte[48];
            ntpData[0] = 0x1B;

            var addresses = Dns.GetHostEntry(ntpServer).AddressList;
            var ipEndPoint = new IPEndPoint(addresses[0], 123);

            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                socket.Connect(ipEndPoint);
                socket.Send(ntpData);
                socket.Receive(ntpData);
                socket.Close();
            }

            var intPart = (ulong)ntpData[40] << 24 | (ulong)ntpData[41] << 16 | (ulong)ntpData[42] << 8 | ntpData[43];
            var fractPart = (ulong)ntpData[44] << 24 | (ulong)ntpData[45] << 16 | (ulong)ntpData[46] << 8 | ntpData[47];

            var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
            var networkDateTime = (new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddMilliseconds((long)milliseconds);

            return networkDateTime.ToLocalTime();
        }
    }
}
