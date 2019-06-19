using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AutoCAD;


namespace WpfApp1
{
    public class ACADconector
    {
        private AcadApplication acApp = null;

        public bool isAvailable
        {
            get
            {
                return (acApp != null);
            }
        }

        public ACADconector()
        {
            const string progID = "AutoCAD.Application";         

            try
            {
                acApp = (AcadApplication)Marshal.GetActiveObject(progID);
            }
            catch
            {
                MessageBox.Show("Cannot create object of type \"" + progID + "\"");
                //try
                //{
                //    Type acType = Type.GetTypeFromProgID(progID);

                //    acApp = (AcadApplication)Activator.CreateInstance(acType, true);
                //}
                //catch
                //{
                //    MessageBox.Show("Cannot create object of type \"" + progID + "\"");
                //}
            }

            //if (acApp != null)
            //{
            //    acApp.Visible = true;
            //    acApp.WindowState = AcWindowState.acMax;

            //    double n = 10.8;
            //    double east = 20.7;
            //    double h = 15.5;

            //    string command = n.ToString(CultureInfo.InvariantCulture) + "," + east.ToString(CultureInfo.InvariantCulture) + "," + h.ToString(CultureInfo.InvariantCulture) + "," + "descxmpl";
            //    acApp.ActiveDocument.SendCommand("AddCOGOPoint " + command + " ");

            //}
        }


        public void Send(Measurment meas)
        {
            double north = meas.Northing;
            double east = meas.Easting;
            double height = meas.Height;
            string name = meas.Id;

            string output = north.ToString(CultureInfo.InvariantCulture) + "," 
                + east.ToString(CultureInfo.InvariantCulture) + "," 
                + height.ToString(CultureInfo.InvariantCulture) + "," 
                + name;

            acApp.ActiveDocument.SendCommand("AddCOGOPoint " + output + " ");
        }
    }
}
