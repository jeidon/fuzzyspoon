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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            using (var ctx = new OLEDController())
            {

                //Fill the controllers dropdown
                var controllerResult = from row in ctx.Controllers
                                       select row;

                if (controllerResult.Count() == 0)
                {
                    //Add some sample values
                    Controller sampleController = new Controller() { ControllerName = "SSD1322" };
                    ctx.Controllers.Add(sampleController);

                    ControllerCommand sampleCommand = new ControllerCommand() { ControllerId = 0, CommandName = "GetID", CommandValue = 0x01};
                    ctx.Commands.Add(sampleCommand);

                    CommandParameters sampleParameters = new CommandParameters() { CommandId = 0, ParameterId = 0, ParameterIndex = 0, ParameterValue = 0 };
                    ctx.Parameters.Add(sampleParameters);

                    ctx.SaveChanges();
                }
                //Configure the dropdown boxes
                cmbController.DataSource = ctx.Controllers.ToList();
                cmbController.ValueMember = "ControllerId";
                cmbController.DisplayMember = "ControllerName";

                lbCommands.DataSource = ctx.Commands.ToList();
                lbCommands.ValueMember = "CommandId";
                lbCommands.DisplayMember = "CommandName";
            }
        }

        private void cmdUpdate_Click(object sender, EventArgs e)
        {
            using (var ctx = new OLEDController())
            {
                //Find the entry based on the tag stored in txtCommand
                var originalCommand = ctx.Commands.Find(txtCommand.Tag);

                if (originalCommand != null)
                {
                    // Update the command table with the new values
                    originalCommand.CommandName = txtName.Text;
                    originalCommand.CommandValue = Convert.ToInt32(txtCommand.Text);

                    //Delete any of the current parameters
                    var originalParameters = from row in ctx.Parameters
                                           where row.CommandId == originalCommand.CommandId
                                           select row;
                    if (originalParameters.Count() > 0)
                    {
                        foreach (var row in originalParameters)
                        {
                            ctx.Parameters.Remove(row);
                        }
                    }
                    ctx.SaveChanges();

                    if (txtParameters.Text != "")
                    {
                        //Add each parameter, if listed
                        var parameterString = txtParameters.Text;

                        //Strip out spaces if any
                        parameterString.Replace(" ", string.Empty);

                        //Split the string into it's component parts
                        String[] splitStrings = parameterString.Split(',');

                        int currentIndex = 0;
                        foreach (var entry in splitStrings)
                        {
                            ctx.Parameters.Add(new CommandParameters() { CommandId = originalCommand.CommandId, ParameterIndex = currentIndex++, ParameterValue = Convert.ToInt32(entry) });
                        }
                        ctx.SaveChanges();
                    }
                }
                lbCommands.DataBindings.Clear();
                lbCommands.DataSource = ctx.Commands.ToList();
                lbCommands.ValueMember = "CommandId";
                lbCommands.DisplayMember = "CommandName";
            }
        }

        private void lbCommands_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var ctx = new OLEDController())
            {
                var selectedCommand = (ControllerCommand)lbCommands.SelectedItem;
                var commandResult = from row in ctx.Commands
                                       select row;
                commandResult = commandResult.Where(p => p.ControllerId == cmbController.SelectedIndex + 1);
                commandResult = commandResult.Where(p => p.CommandId == selectedCommand.CommandId);
                if (commandResult.Count() > 0)
                {
                    foreach (var item in commandResult)
                    {
                        txtName.Text = item.CommandName;
                        txtCommand.Tag = item.CommandId;
                        txtCommand.Text = item.CommandValue.ToString();

                        var parameterResult = from row in ctx.Parameters
                                              select row;
                        parameterResult = parameterResult.Where(p => p.CommandId == item.CommandId);
                        txtParameters.Text = "";
                        foreach (var parameter in parameterResult)
                        {
                            if (parameter.ParameterIndex != 0)
                            {
                                txtParameters.Text += "," + parameter.ParameterValue;
                            }
                            else
                            {
                                txtParameters.Text = parameter.ParameterValue.ToString();
                            }
                        }
                    }
                }
            }
        }

        private void cmdInsert_Click(object sender, EventArgs e)
        {
            using (var ctx = new OLEDController())
            {
                ControllerCommand newCommand = new ControllerCommand() { CommandName = txtName.Text, CommandValue = Convert.ToInt32(txtCommand.Text), ControllerId = cmbController.SelectedIndex + 1 };
                ctx.Commands.Add(newCommand);
                ctx.SaveChanges();

                if (txtParameters.Text != "")
                {
                    //Add each parameter, if listed
                    var parameterString = txtParameters.Text;

                    //Strip out spaces if any
                    parameterString.Replace(" ", string.Empty);

                    //Split the string into it's component parts
                    String[] splitStrings = parameterString.Split(',');

                    int currentIndex = 0;
                    foreach (var entry in splitStrings)
                    {
                        ctx.Parameters.Add(new CommandParameters() { CommandId = newCommand.CommandId, ParameterIndex = currentIndex++, ParameterValue = Convert.ToInt32(entry) });
                    }
                    ctx.SaveChanges();
                }

                lbCommands.DataBindings.Clear();
                lbCommands.DataSource = ctx.Commands.ToList();
                lbCommands.ValueMember = "CommandId";
                lbCommands.DisplayMember = "CommandName";
            }
        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            using (var ctx = new OLEDController())
            {
                var commandResult = (from row in ctx.Commands
                                    where row.ControllerId == cmbController.SelectedIndex + 1
                                    where row.CommandId == lbCommands.SelectedIndex + 1
                                    select row).FirstOrDefault();
                
                if (commandResult != null)
                {
                    //Delete it from memory
                    ctx.Commands.Remove(commandResult);
                    //Save to database
                    ctx.SaveChanges();
                }
                lbCommands.DataBindings.Clear();
                lbCommands.DataSource = ctx.Commands.ToList();
                lbCommands.ValueMember = "CommandId";
                lbCommands.DisplayMember = "CommandName";
            }
        }
    }
}
