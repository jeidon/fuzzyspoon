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
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel.DataAnnotations;

namespace FuzzySpoon
{
    public partial class frmMain : Form
    {
        static SerialPort _serialPort = new SerialPort();

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
                        cmdConnect.Text = "Disconnect";
                        cmdConnect.ForeColor = System.Drawing.Color.Red;
                    }
                    catch
                    {
                        groupBox2.Enabled = false;
                    }
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
            serial.CD = 1;
            serial.command = 0xFD;
            serial.data[0] = 0x12;
            Transmit(serial);

            //Write_Command(0xAE);
            serial.command = 0xAE;
            Transmit(serial);

            //Write_Command(0x15);
            //Write_Data(0x1c);
            //Write_Data(0x5b);
            serial.command = 0x15;
            serial.dataLength = 2;
            serial.data[0] = 0x1C;
            serial.data[1] = 0x5B;
            Transmit(serial);

            //Write_Command(0x75);
            //Write_Data(0x00);
            //Write_Data(0x3f);
            serial.command = 0x75;
            serial.dataLength = 2;
            serial.data[0] = 0x00;
            serial.data[1] = 0x3F;
            Transmit(serial);

            //Write_Command(0xB3);
            //Write_Data(0x91);
            serial.command = 0xB3;
            serial.dataLength = 1;
            serial.data[0] = 0x91;
            Transmit(serial);

            //Write_Command(0xCA);
            //Write_Data(0x3F);
            serial.command = 0xCA;
            serial.dataLength = 1;
            serial.data[0] = 0x3F;
            Transmit(serial);

            //Write_Command(0xA2);
            //Write_Data(0x00);
            serial.command = 0xA2;
            serial.dataLength = 1;
            serial.data[0] = 0x00;
            Transmit(serial);

            //Write_Command(0xA1);
            //Write_Data(0x00);
            serial.command = 0xA1;
            serial.dataLength = 1;
            serial.data[0] = 0x00;
            Transmit(serial);

            //Write_Command(0xA0);
            //Write_Data(0x14);
            //Write_Data(0x11);
            serial.command = 0xA0;
            serial.dataLength = 2;
            serial.data[0] = 0x14;
            serial.data[1] = 0x11;
            Transmit(serial);

            //Write_Command(0xB5);
            //Write_Data(0x00);
            serial.command = 0xB5;
            serial.dataLength = 1;
            serial.data[0] = 0x00;
            Transmit(serial);

            //Write_Command(0xAB);
            //Write_Data(0x01);
            serial.command = 0xAB;
            serial.dataLength = 1;
            serial.data[0] = 0x01;
            Transmit(serial);

            //Write_Command(0xB4);
            //Write_Data(0xA0);
            //Write_Data(0xFD);
            serial.command = 0xB4;
            serial.dataLength = 2;
            serial.data[0] = 0xA0;
            serial.data[1] = 0xFD;
            Transmit(serial);

            //Write_Command(0xC1);
            //Write_Data(0x9F);
            serial.command = 0xC1;
            serial.dataLength = 1;
            serial.data[0] = 0x9F;
            Transmit(serial);

            //Write_Command(0xC7);
            //Write_Data(0x0F);
            serial.command = 0xC7;
            serial.dataLength = 1;
            serial.data[0] = 0x0F;
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
            serial.dataLength = 15;
            serial.data[0] = 0x0C;
            serial.data[1] = 0x18;
            serial.data[2] = 0x24;
            serial.data[3] = 0x30;
            serial.data[4] = 0x3C;
            serial.data[5] = 0x48;
            serial.data[6] = 0x54;
            serial.data[7] = 0x60;
            serial.data[8] = 0x6C;
            serial.data[9] = 0x78;
            serial.data[10] = 0x84;
            serial.data[11] = 0x90;
            serial.data[12] = 0x9C;
            serial.data[13] = 0xA8;
            serial.data[14] = 0xB4;
            Transmit(serial);

            //Write_Command(0x00);
            serial.command = 0x00;
            Transmit(serial);

            //Write_Command(0xB1);
            //Write_Data(0xE2);
            serial.command = 0xB1;
            serial.dataLength = 1;
            serial.data[0] = 0xE2;
            Transmit(serial);

            //Write_Command(0xD1);
            //Write_Data(0x82);
            //Write_Data(0x20);
            serial.command = 0xD1;
            serial.dataLength = 2;
            serial.data[0] = 0x82;
            serial.data[0] = 0x20;
            Transmit(serial);

            //Write_Command(0xBB);
            //Write_Data(0x1F);
            serial.command = 0xBB;
            serial.dataLength = 1;
            serial.data[0] = 0x1F;
            Transmit(serial);

            //Write_Command(0xB6);
            //Write_Data(0x08);
            serial.command = 0xB6;
            serial.dataLength = 1;
            serial.data[0] = 0x08;
            Transmit(serial);

            //Write_Command(0xBE);
            //Write_Data(0x07);
            serial.command = 0xBE;
            serial.dataLength = 1;
            serial.data[0] = 0x07;
            Transmit(serial);

            //Write_Command(0xA6);
            serial.command = 0xA6;
            serial.dataLength = 0;
            Transmit(serial);

            //Write_Command(0xA9);
            serial.command = 0xA9;
            serial.dataLength = 0;
            Transmit(serial);

            //Write_Command(0x5C);
            serial.command = 0x5C;
            serial.dataLength = 0;
            Transmit(serial);

            //Write_Command(0xAF);
            serial.command = 0xAF;
            serial.dataLength = 0;
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

        static public void Transmit(PACKET packet)
        {
            //Send out signals
            _serialPort.Write(BitConverter.GetBytes(0x55), 0, 1);
            _serialPort.Write(BitConverter.GetBytes(0xAA), 0, 1);

            // Send the C/D bit
            _serialPort.Write(BitConverter.GetBytes(packet.CD), 0, 1);

            // Send the Command -OR- a zero
            if (packet.CD == 1)
            {
                _serialPort.Write(BitConverter.GetBytes(packet.command), 0, 1);
            }

            // Send the dataLength
            _serialPort.Write(BitConverter.GetBytes(packet.dataLength), 0, 1);

            // Send the data
            for (int i = 0; i < packet.dataLength; i++)
            {
                _serialPort.Write(BitConverter.GetBytes(packet.data[i]), 0, 1);
            }

        }

        private void cmbFill1_Click(object sender, EventArgs e)
        {
            // Layout some variables
            byte[] bigArray = new byte[8192];
            int progress = 0;
            int dat;

            //Spin up a screen
            for (int y = 0; y < 64; y++)
            {
                dat = 0;

                for (int x = 0; x < 64; x++)
                {
                    bigArray[progress++] = (byte)dat;
                    bigArray[progress++] = (byte)dat;
                    if ((x > 0) && (x % 8 == 0))
                    {
                        dat += 0x22;
                    }
                }
            }
            groupBox2.Enabled = false;
            sendImageArray(bigArray);
            groupBox2.Enabled = true;
        }

        private void btnFill2_Click(object sender, EventArgs e)
        {
            byte[] bigArray = new byte[8192];
            int progress = 0;
            int dat;

            for (int y = 0; y < 64; y++)
            {
                dat = 0xEE;

                for (int x = 0; x < 64; x++)
                {
                    bigArray[progress++] = (byte)dat;
                    bigArray[progress++] = (byte)dat;
                    if ((x > 0) && (x % 8 == 0))
                    {
                        dat -= 0x22;
                    }
                }
            }
            groupBox2.Enabled = false;
            sendImageArray(bigArray);
            groupBox2.Enabled = true;
        }

        private void btnNormal_Click(object sender, EventArgs e)
        {
            PACKET packet = new PACKET();
            //Display_Mode(0xA6);                //Set Display Normal Mode
            packet.CD = 1;
            packet.command = 0xA6;
            packet.dataLength = 0;
            Transmit(packet);
        }

        private void btnInvert_Click(object sender, EventArgs e)
        {
            PACKET packet = new PACKET();
            //Display_Mode(0xA7);                //Set Display Inverted Mode
            packet.CD = 1;
            packet.command = 0xA7;
            packet.dataLength = 0;
            Transmit(packet);
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (_serialPort.IsOpen)
            {
                groupBox2.Enabled = false;
                OpenFileDialog openFileDialog1 = new OpenFileDialog();

                openFileDialog1.InitialDirectory = "c:\\";
                openFileDialog1.Filter = "24-Bit Bitmap file (*.bmp)|*.bmp|All files (*.*)|*.*";
                openFileDialog1.FilterIndex = 2;
                openFileDialog1.RestoreDirectory = true;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    PACKET packet = new PACKET();
                    try
                    {
                        //  Set Column Address
                        //  Column_Address(0x1C, 0x5B);
                        packet.CD = 1;
                        packet.command = 0x15;
                        packet.dataLength = 2;
                        packet.data[0] = 0x1C;
                        packet.data[1] = 0x5B;
                        Transmit(packet);

                        //  Set Row Address
                        //  Row_Address(0x00, 0x3F);
                        packet.CD = 1;
                        packet.command = 0x75;
                        packet.dataLength = 2;
                        packet.data[0] = 0x00;
                        packet.data[1] = 0x3F;
                        Transmit(packet);

                        //  Write_Command(0x5c);
                        packet.CD = 1;
                        packet.command = 0x5C;
                        packet.dataLength = 0;
                        Transmit(packet);

                        System.Drawing.Bitmap image = (Bitmap)Bitmap.FromFile(openFileDialog1.FileName);
                        _bmpImage imgArray = new _bmpImage();
                        imgArray.sourceImage = image;

                        // Start sending bytes
                        imgArray.sendImageArray(8);

                        //TODO
                        //if the arrayProgress < 255
                        // Wrap it up and send it
                        //TODO
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: Could not open Bitmap: " + ex.Message);
                    }
                }
                groupBox2.Enabled = true;
            }
        }

        private void btnSendImgArray1_Click(object sender, EventArgs e)
        {
            Screens screens = new Screens();
            sendImageArray(screens.logo_screen_16_level);
        }

        private void btnSendImgArray2_Click(object sender, EventArgs e)
        {
            Screens screens = new Screens();
            sendImageArray(screens.text_description);
        }

        public void sendImageArray(byte[] sourceImageArray)
        {
            if (_serialPort.IsOpen)
            {
                PACKET packet = new PACKET();

                //  Set Column Address
                //  Column_Address(0x1C, 0x5B);
                packet.CD = 1;
                packet.command = 0x15;
                packet.dataLength = 2;
                packet.data[0] = 0x1C;
                packet.data[1] = 0x5B;
                Transmit(packet);
                Thread.Sleep(1);

                //  Set Row Address
                //  Row_Address(0x00, 0x3F);
                packet.CD = 1;
                packet.command = 0x75;
                packet.dataLength = 2;
                packet.data[0] = 0x00;
                packet.data[1] = 0x3F;
                Transmit(packet);
                Thread.Sleep(1);

                //  Write_Command(0x5c);
                packet.CD = 1;
                packet.command = 0x5C;
                packet.dataLength = 0;
                Transmit(packet);
                Thread.Sleep(1);

                int arrayProgress = 0;
                int bytesRemaining = sourceImageArray.Length;
                for (int i = 0; i < sourceImageArray.Length; i++)
                {
                    packet.data[arrayProgress++] = sourceImageArray[i];

                    if (arrayProgress == 251)
                    {
                        packet.CD = 0;
                        packet.dataLength = (byte)arrayProgress;
                        frmMain.Transmit(packet);
                        arrayProgress = 0;
                        bytesRemaining -= 251;
                    }
                }
                if (bytesRemaining > 0)
                {
                    packet.CD = 0;
                    packet.dataLength = (byte)bytesRemaining;
                    frmMain.Transmit(packet);
                }
            }
        }
    }

    public class PACKET
    {
        public byte CD { get; set; }
        public byte command { get; set; }
        public byte dataLength { get; set; }
        public byte[] data = new byte[256];
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
