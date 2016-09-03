using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
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
            //Fill the controllers dropdown
            try
            {
                cmbController.Items.Clear();
                cmbController.Text = "Select a controller";

                Controller controller = new Controller();
                if (controller.Commands != null)
                {
                    var controllers = from d in controller.ControllerName
                                      select d;
                    foreach (var item in controllers)
                    {
                        cmbController.Items.Add(item);
                    }
                }
            }
            catch
            {
                MessageBox.Show("An error has occured while loading the controller list");
            }

            //Fill the port dropdown

            //Fill the baud rate dropdown
            try
            {
                cmbBaud.Items.Clear();
                cmbBaud.Text = "Select a Baud Rate";

                BaudRate baudRate = new BaudRate();
                if (baudRate.Baud != null)
                {

                    var BaudRates = from d in baudRate.Baud
                                    select d;
                    foreach (var item in BaudRates)
                    {
                        cmbController.Items.Add(item);
                    }
                }
            }
            catch
            {
                MessageBox.Show("An error has occured while loading the controller list");
            }

        }
    }

    public class Port
    {
        //These have no get or set because they will populate on the fly
        public int PortID;
        public string PortName;
    }

    public class BaudRate
    {
        public int BaudID { get; set; }
        public string Baud { get; set; }
    }

    public class Controller
    {
        public int ControllerID { get; set; }
        public string ControllerName { get; set; }

        public ICollection<Command> Commands { get; set; }
    }

    public class Command
    {
        public int CommandID { get; set; }
        public string CommandName { get; set; }
        public string CommandValue { get; set; }
        public ICollection<int> Parameters { get; set; }

        public virtual Controller Controller { get; set; }
    }

    public class ControllerCommands : DbContext
    {
        public DbSet<BaudRate> BaudRates { get; set; }
        public DbSet<Controller> Controllers { get; set; }
        public DbSet<Command> Commands { get; set; }
    }
}
