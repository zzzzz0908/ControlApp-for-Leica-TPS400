using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class spOptionsVM : BaseVM
    {
        public string portName { get; set; }
        public int baudrate { get; set; }
        public Parity parity { get; set; }
        public int dataBits { get { return (parity == Parity.None) ? 8 : 7; } }
        

        public List<Parity> parityList { get; set; }
        public List<string> portNameList { get; set; }
        public List<int> baudrateList { get; set; }


        public spOptionsVM()
        {
            parityList = new List<Parity>() { Parity.None, Parity.Even, Parity.Odd };
            
            portNameList = SerialPort.GetPortNames().ToList();

            baudrateList = new List<int>() { 19200, 9600, 4800, 2400 };
        }


    }
}
