using Microsoft.Win32;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    public class MainVM : BaseVM
    {
        private ObservableCollection<Measurment> _measurments;
        private string data;
        private ACADconector ACADcon;
        private bool _recordRS232;
        private bool _exportCivil;


        public SerialPort serialPort { get; set; }

        public string PointName { get; set; }
        public double StationNorthing { get; set; }
        public double StationEasting { get; set; }
        public double StationHeight { get; set; }

        public double InstrHeight { get; set; }
        public double ReflectorHeight { get; set; }

        public bool ExportCivil
        {
            get => _exportCivil;
            set
            {
                _exportCivil = value;
                RaisePropertyChanged(nameof(ExportCivil));
            }
        }


        public bool RecordRS232
        {
            get => _recordRS232;
            set
            {
                _recordRS232 = value;
                RaisePropertyChanged(nameof(RecordRS232));
            }
        }





        public ObservableCollection<Measurment> Measurments
        {
            get => _measurments;
            set
            {
                _measurments = value;
                RaisePropertyChanged(nameof(Measurments));
            }
        }







        public MainVM()
        {
            SendValue = new DelegateCommand<string>(str =>
            {
                switch (str)
                {
                    case "11":
                        serialPort.WriteLine($"PUT/11....+" + PointName.PadLeft(8, '0') + " ");
                        serialPort.WriteLine("?");
                        break;
                    case "87":
                        serialPort.WriteLine($"PUT/87...0" + (ReflectorHeight * 1000).ToString("+00000000;-00000000") + " ");
                        serialPort.WriteLine("?");
                        break;
                    case "88":
                        serialPort.WriteLine($"PUT/88...0" + (InstrHeight * 1000).ToString("+00000000;-00000000") + " ");
                        serialPort.WriteLine("?");
                        break;
                    case "85":                        
                        serialPort.WriteLine($"PUT/85...0" + (StationNorthing * 1000).ToString("+00000000;-00000000") + " ");
                        serialPort.WriteLine("?");
                        break;
                    case "84":
                        serialPort.WriteLine($"PUT/84...0" + (StationEasting * 1000).ToString("+00000000;-00000000") + " ");
                        serialPort.WriteLine("?");
                        break;
                    case "86":
                        serialPort.WriteLine($"PUT/86...0" + (StationHeight * 1000).ToString("+00000000;-00000000") + " ");
                        serialPort.WriteLine("?");
                        break;
                }

            });
            

            Show_spOptions = new DelegateCommand<string>(str =>
            {
                var window = new SerialPortOptionsWindow();
                
                window.ShowDialog();



                spOptionsVM spOptions = (spOptionsVM)window.DataContext;

                try
                {
                    if (serialPort != null && serialPort.IsOpen) serialPort.Close();
                    serialPort = new SerialPort(spOptions.portName, spOptions.baudrate, spOptions.parity, spOptions.dataBits, StopBits.One);
                    serialPort.NewLine = "\r\n";
                    serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                    serialPort.Open();
                    serialPort.WriteLine("SET/76/1");
                    serialPort.WriteLine("");
                    RecordRS232 = true;

                }
                catch (Exception)
                {
                    MessageBox.Show("Не удалось установить соединение");
                }

            });



            Show_Settings = new DelegateCommand<string>(str =>
            {                
                int[] settingsValues = new int[5];

                data = "";
                string temp = "";
                serialPort.WriteLine("CONF/161"); //EDMmode
                Thread.Sleep(500);
                temp = data;
                
                settingsValues[0] = Int32.Parse(temp.Substring(5, 4));
                temp = "";


                data = "";
                serialPort.WriteLine("CONF/40"); //angleUnit
                Thread.Sleep(500);
                temp = data;

                settingsValues[1] = Int32.Parse(temp.Substring(5, 4));
                temp = "";
                                              


                data = "";
                serialPort.WriteLine("CONF/31"); //DisplayLight
                Thread.Sleep(500);
                temp = data;

                settingsValues[3] = Int32.Parse(temp.Substring(5, 4));
                temp = "";


                data = "";
                serialPort.WriteLine("CONF/36"); //LaserPointer
                Thread.Sleep(500);
                temp = data;

                settingsValues[4] = Int32.Parse(temp.Substring(5, 4));
                temp = "";



                SettingsVM settings = new SettingsVM()
                {
                    EDMmode = settingsValues[0],
                    AngleUnit = settingsValues[1],
                    DisplayLight = settingsValues[3],
                    LaserPointer = settingsValues[4]
                };

                var window = new SetingsWindow()
                { DataContext = settings };
                
                window.ShowDialog();

                if (window.DialogResult == true)
                {
                    //запись изменений
                    serialPort.WriteLine($"SET/161/{settings.EDMmode}");
                    serialPort.WriteLine($"SET/40/{settings.AngleUnit}");
                    serialPort.WriteLine($"SET/31/{settings.DisplayLight}");
                    serialPort.WriteLine($"SET/36/{settings.LaserPointer}");
                }
                data = "";
            });



            ConnectCivil = new DelegateCommand<string>(str =>
            {
                bool val = ExportCivil;
                if (val)
                {
                    ACADcon = new ACADconector();
                    if (!ACADcon.isAvailable)
                    {
                        ExportCivil = false;
                    }
                }
                else
                {
                    ACADcon = null;
                }

            });


            SetRS232Mode = new DelegateCommand<string>(str =>
            {
                if (RecordRS232)
                {
                    serialPort.WriteLine("SET/76/1");
                    serialPort.WriteLine("");
                }
                else
                {
                    serialPort.WriteLine("SET/76/0");
                    serialPort.WriteLine("");
                }

            });


            SaveData = new DelegateCommand<string>(str =>
            {
                SaveFileDialog sd = new SaveFileDialog();
                sd.Filter = "GSI files (*.gsi)|*.gsi|All files (*.*)|*.*";
                sd.FilterIndex = 0;
                sd.AddExtension = true;
                sd.DefaultExt = "gsi";

                if (sd.ShowDialog() == true)
                {
                    string path = sd.FileName;

                    using (StreamWriter sw = new StreamWriter(path))
                    {
                        foreach (Measurment m in Measurments)
                        {
                            sw.WriteLine(m.data);
                        }
                    }
                }

            });

        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            data += indata;

            if (indata.Contains("w\r\n"))
            {
                sp.WriteLine("?");
                int end = data.IndexOf("w\r\n") + 3;
                string temp = data.Substring(0, end);
                data = data.Remove(0, end);
                temp = temp.TrimStart('\r', '\n', '?');
                AddMeasurment(temp);
            }

            
            
        }

        private void AddMeasurment(string input)
        {
            Measurment meas = new Measurment(input);
            Application.Current.Dispatcher.BeginInvoke(new Action(() => Measurments.Add(meas)));
            // export to Civil
            if (ExportCivil)
            {
                ACADcon.Send(meas);
            }
        }

        public DelegateCommand<string> Show_spOptions { get; }
        public DelegateCommand<string> Show_Settings { get; }
        public DelegateCommand<string> SendValue { get; }
        public DelegateCommand<string> ConnectCivil { get; }
        public DelegateCommand<string> SetRS232Mode { get; }
        public DelegateCommand<string> SaveData { get; }



    }
}
