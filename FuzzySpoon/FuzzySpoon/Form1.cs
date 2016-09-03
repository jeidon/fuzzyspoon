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
            cmbController.Items.Clear();
            cmbController.Text = "Select a controller";
            cmbPort.Items.Clear();
            cmbPort.Text = "Select a Port";

            //Fill the controllers dropdown
            //Controller controller = new Controller();
            //if (controller.Commands != null)
            //{
            //    var controllers = from d in controller.ControllerName
            //                        select d;
            //    foreach (var item in controllers)
            //    {
            //        cmbController.Items.Add(item);
            //    }
            //}
            //else
            //{
            //    //Let's add a dummy value
            //    controller.ControllerName = "SSD1322";
            //    Command command = new Command();
            //    command.CommandName = "GetID";
            //    command.CommandValue = 0x30;
            //    command.Parameters.Add(0x26);
            //    controller.Commands.Add(command);
            //}

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

            //Fill the baud rate dropdown
            using (var ctx = new ControllerCommands())
            {
                var result = from row in ctx.BaudRates
                                select row;

                if(result.Count() == 0)
                {
                    //Let's add dummy Values
                    ctx.BaudRates.Add(new BaudRate() { Baud = 19200 });
                    ctx.BaudRates.Add(new BaudRate() { Baud = 115200 });
                    ctx.SaveChanges();
                }
                else
                {
                    foreach (var item in result)
                    {
                        cmbBaud.Items.Add(item.Baud);
                    }
                }
            }
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

        public ICollection<Command> Commands { get; set; }
    }

    public class Command
    {
        public int CommandId { get; set; }
        public string CommandName { get; set; }
        public int CommandValue { get; set; }
        public List<int> Parameters { get; set; }

        public virtual Controller Controller { get; set; }
    }

    public class ControllerCommands : DbContext
    {
        public DbSet<BaudRate> BaudRates { get; set; }
        public DbSet<Controller> Controllers { get; set; }
        public DbSet<Command> Commands { get; set; }
    }
}
