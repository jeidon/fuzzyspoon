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
    public partial class frmMain : Form
    {
        SerialPort _serialPort = new SerialPort();

        public frmMain()
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
                if (!thisPort.IsOpen)
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

                if (baudResult.Count() == 0)
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
                //Fill the controllers dropdown
                var controllerResult = from row in ctx.Controllers
                                       select row;

                if (controllerResult.Count() == 0)
                {
                    //Add some sample values
                    //Add a controller
                    Controller sampleController;
                    sampleController = new Controller() { ControllerName = "SSD1805" };
                    ctx.Controllers.Add(sampleController);
                    ctx.SaveChanges();

                    //Add a command
                    ControllerCommand sampleCommand = new ControllerCommand() { ControllerId = sampleController.ControllerId, CommandName = "GetID", CommandValue = 0x01 };
                    ctx.Commands.Add(sampleCommand);
                    ctx.SaveChanges();

                    //Add a parameter
                    CommandParameters sampleParameters;
                    sampleParameters = new CommandParameters() { CommandId = sampleCommand.CommandId, ParameterIndex = 0, ParameterValue = 15 };
                    ctx.Parameters.Add(sampleParameters);
                    ctx.SaveChanges();

                    sampleParameters = new CommandParameters() { CommandId = sampleCommand.CommandId, ParameterIndex = 1, ParameterValue = 26 };
                    ctx.Parameters.Add(sampleParameters);
                    ctx.SaveChanges();

                    sampleController = new Controller() { ControllerName = "SSD1322" };
                    ctx.Controllers.Add(sampleController);
                    ctx.SaveChanges();
                }

                //Configure the dropdown boxes
                cmbController.DataSource = ctx.Controllers.ToList();
                cmbController.ValueMember = "ControllerId";
                cmbController.DisplayMember = "ControllerName";

                //                lbCommands.DataSource = ctx.Commands.ToList();
                //                lbCommands.ValueMember = "CommandId";
                //                lbCommands.DisplayMember = "CommandName";
            }
        }

        private void cmdConnect_Click(object sender, EventArgs e)
        {
            if (cmbPort.Text != "" && cmbBaud.Text != "")
            {
                if (cmdConnect.Text == "Connect")
                {
                    _serialPort.PortName = cmbPort.Text;
                    _serialPort.BaudRate = Convert.ToInt32(cmbBaud.Text);
                    _serialPort.Parity = Parity.None;
                    _serialPort.DataBits = 8;
                    _serialPort.StopBits = StopBits.One;

                    try
                    {
                        _serialPort.Open();
                        groupBox2.Enabled = true;
                    }
                    catch
                    {
                        groupBox2.Enabled = false;
                    }

                    cmdConnect.Text = "Disconnect";
                    cmdConnect.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    if (_serialPort.IsOpen)
                    {
                        _serialPort.Close();
                    }
                    groupBox2.Enabled = false;
                    cmdConnect.Text = "Connect";
                    cmdConnect.ForeColor = System.Drawing.Color.Black;
                }
            }
        }

        private void cmdInit_Click(object sender, EventArgs e)
        {
            PACKET serial = new PACKET();

            //Send the following Commands
            // Copied from CFA Bring Up
            
            //Write_Command(0xFD);
            //Write_Data(0x12);
            serial.command = 0xFD;
            serial.data.Add(0x12);
            Transmit(serial);

            //Write_Command(0xAE);
            serial.command = 0xAE;
            Transmit(serial);

            //Write_Command(0x15);
            //Write_Data(0x1c);
            //Write_Data(0x5b);
            serial.command = 0x15;
            serial.data.Add(0x1C);
            serial.data.Add(0x5B);
            Transmit(serial);

            //Write_Command(0x75);
            //Write_Data(0x00);
            //Write_Data(0x3f);
            serial.command = 0x75;
            serial.data.Add(0x00);
            serial.data.Add(0x3F);
            Transmit(serial);
            
            //Write_Command(0xB3);
            //Write_Data(0x91);
            serial.command = 0xB3;
            serial.data.Add(0x91);
            Transmit(serial);

            //Write_Command(0xCA);
            //Write_Data(0x3F);
            serial.command = 0xCA;
            serial.data.Add(0x3F);
            Transmit(serial);

            //Write_Command(0xA2);
            //Write_Data(0x00);
            serial.command = 0xA2;
            serial.data.Add(0x00);
            Transmit(serial);

            //Write_Command(0xA1);
            //Write_Data(0x00);
            serial.command = 0xA1;
            serial.data.Add(0x00);
            Transmit(serial);

            //Write_Command(0xA0);
            //Write_Data(0x14);
            //Write_Data(0x11);
            serial.command = 0xA0;
            serial.data.Add(0x14);
            serial.data.Add(0x11);
            Transmit(serial);

            //Write_Command(0xB5);
            //Write_Data(0x00);
            serial.command = 0xB5;
            serial.data.Add(0x00);
            Transmit(serial);
            
            //Write_Command(0xAB);
            //Write_Data(0x01);
            serial.command = 0xAB;
            serial.data.Add(0x01);
            Transmit(serial);

            //Write_Command(0xB4);
            //Write_Data(0xA0);
            //Write_Data(0xFD);
            serial.command = 0xB4;
            serial.data.Add(0xA0);
            serial.data.Add(0xFD);
            Transmit(serial);

            //Write_Command(0xC1);
            //Write_Data(0x9F);
            serial.command = 0xC1;
            serial.data.Add(0x9F);
            Transmit(serial);

            //Write_Command(0xC7);
            //Write_Data(0x0F);
            serial.command = 0xC7;
            serial.data.Add(0x0F);
            Transmit(serial);

            //Write_Command(0xb8);
            //Write_Data(0x0c);
            //Write_Data(0x18);
            //Write_Data(0x24);
            //Write_Data(0x30);
            //Write_Data(0x3c);
            //Write_Data(0x48);
            //Write_Data(0x54);
            //Write_Data(0x60);
            //Write_Data(0x6c);
            //Write_Data(0x78);
            //Write_Data(0x84);
            //Write_Data(0x90);
            //Write_Data(0x9c);
            //Write_Data(0xa8);
            //Write_Data(0xb4);
            serial.command = 0xB8;
            serial.data.Add(0x0C);
            serial.data.Add(0x18);
            serial.data.Add(0x24);
            serial.data.Add(0x30);
            serial.data.Add(0x3C);
            serial.data.Add(0x48);
            serial.data.Add(0x54);
            serial.data.Add(0x60);
            serial.data.Add(0x6C);
            serial.data.Add(0x78);
            serial.data.Add(0x84);
            serial.data.Add(0x90);
            serial.data.Add(0x9C);
            serial.data.Add(0xA8);
            serial.data.Add(0xB4);
            Transmit(serial);

            //Write_Command(0x00);
            serial.command = 0x00;
            Transmit(serial);

            //Write_Command(0xB1);
            //Write_Data(0xE2);
            serial.command = 0xB1;
            serial.data.Add(0xE2);
            Transmit(serial);

            //Write_Command(0xD1);
            //Write_Data(0x82);
            //Write_Data(0x20);
            serial.command = 0xD1;
            serial.data.Add(0x82);
            serial.data.Add(0x20);
            Transmit(serial);

            //Write_Command(0xBB);
            //Write_Data(0x1F);
            serial.command = 0xBB;
            serial.data.Add(0x1F);
            Transmit(serial);

            //Write_Command(0xB6);
            //Write_Data(0x08);
            serial.command = 0xB6;
            serial.data.Add(0x08);
            Transmit(serial);

            //Write_Command(0xBE);
            //Write_Data(0x07);
            serial.command = 0xBE;
            serial.data.Add(0x07);
            Transmit(serial);

            //Write_Command(0xA6);
            serial.command = 0xA6;
            Transmit(serial);

            //Write_Command(0xA9);
            serial.command = 0xA9;
            Transmit(serial);

            //Write_Command(0x5C);
            serial.command = 0x5C;
            Transmit(serial);

            //Write_Command(0xAF);
            serial.command = 0xAF;
            Transmit(serial);
        }

        private void cmdEdit_Click(object sender, EventArgs e)
        {
            frmCommands frm = new frmCommands();
            frm.ShowDialog();
        }

        private void cmbPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBaud.Text != "")
            {
                cmdConnect.Enabled = true;
            }
        }

        private void cmbBaud_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPort.Text != "")
            {
                cmdConnect.Enabled = true;
            }
        }

        private void cmbController_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var ctx = new OLEDController())
            {
//                cmbCommands.DataBindings.Clear();
                cmbCommands.DataSource = null;
                var commandsResult = (from row in ctx.Commands
                                      where row.ControllerId == cmbController.SelectedIndex + 1
                                      select row);

                cmbCommands.DataSource = commandsResult.ToList();
                cmbCommands.ValueMember = "CommandId";
                cmbCommands.DisplayMember = "CommandName";
            }
        }

        private void Transmit(PACKET packet)
        {
            //Make a big array to hold the command and its parameters
            String intString = "";

            intString = packet.command.ToString();
            intString += packet.data.Count;

            for (int i = 0; i < packet.data.Count; i++)
            {
                intString += packet.data[i];
            }
            _serialPort.Write(intString);

            packet.command = 0;
            packet.data.Clear();
        }
    }

    public class PACKET
    {
        public int command { get; set; }
        public List<int> data = new List<int>();
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

//        public List<ControllerCommand> Commands { get; set; }
    }

    public class ControllerCommand
    {
        [Key]
        public int CommandId { get; set; }
        public int ControllerId { get; set; }
        public string CommandName { get; set; }
        public int CommandValue { get; set; }
//        public List<CommandParameters> Parameters { get; set; }
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
