using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.IO.Ports;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel.DataAnnotations;

namespace FuzzySpoon
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbBaud.Items.Clear();
            cmbBaud.Text = "Select a Baud Rate";
            cmbPort.Items.Clear();
            cmbPort.Text = "Select a Port";

            //Fill the port dropdown
            string[] ports = SerialPort.GetPortNames();
            // Display each port name to the console.
            foreach (string port in ports)
            {
                var thisPort = new SerialPort(port);
                if(!thisPort.IsOpen)
                {
                    cmbPort.Items.Add(port);
                }
                else
                {
                    cmbPort.Items.Add(port + " (In Use)");
                }
            }

            using (var ctx = new OLEDController())
            {
                //Fill the baud rate dropdown
                var baudResult = from row in ctx.BaudRates
                                select row;

                if(baudResult.Count() == 0)
                {
                    //Let's add dummy Values
                    ctx.BaudRates.Add(new BaudRate() { Baud = 19200 });
                    ctx.BaudRates.Add(new BaudRate() { Baud = 115200 });
                    ctx.SaveChanges();
                    cmbBaud.Items.Add("19200");
                }
                else
                {
                    foreach (var item in baudResult)
                    {
                        cmbBaud.Items.Add(item.Baud);
                    }
                }
            }
        }

        private void cmdConnect_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 frm = new Form2();
            //frm.Show();
            frm.ShowDialog();

            this.Close();
        }
    }

    public class Port
    {
        //These have no get or set because they will populate on the fly
        public int PortId;
        public string PortName;
    }

    public class BaudRate
    {
        public int BaudRateId { get; set; }
        public int Baud { get; set; }
    }

    public class Controller
    {
        public int ControllerId { get; set; }
        public string ControllerName { get; set; }

        public List<ControllerCommand> Commands { get; set; }
    }

    public class ControllerCommand
    {
        [Key]
        public int CommandId { get; set; }
        public int ControllerId { get; set; }
        public string CommandName { get; set; }
        public int CommandValue { get; set; }
        public List<CommandParameters> Parameters { get; set; }
    }

    public class CommandParameters
    {
        [Key]
        public int ParameterId { get; set; }
        public int CommandId { get; set; }
        public int ParameterIndex { get; set; }
        public int ParameterValue { get; set; }
    }

    public class OLEDController : DbContext
    {
        public DbSet<BaudRate> BaudRates { get; set; }
        public DbSet<ControllerCommand> Commands { get; set; }
        public DbSet<Controller> Controllers { get; set; }
        public DbSet<CommandParameters> Parameters {get; set;}
    }


}
