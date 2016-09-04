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
            
//            cmbController.Items.Clear();
//            cmbController.Text = "Select a controller";

            using (var ctx = new OLEDController())
            {
                cmbController.DataSource = ctx.Controllers.ToList();
                cmbController.ValueMember = "ControllerId";
                cmbController.DisplayMember = "ControllerName";

/*                //Fill the controllers dropdown
                var controllerResult = from row in ctx.Controllers
                                       select row;

                if (controllerResult.Count() == 0)
                {
                    List<int> sampleParameters = new List<int>();
                    sampleParameters.Add(26);

                    Controller sampleController = new Controller() { ControllerName = "SSD1322" };
                    ctx.Controllers.Add(sampleController);

                    ControllerCommand sampleCommand = new ControllerCommand() { ControllerId = 0, CommandName = "GetID", CommandValue = 0x01, Parameters = sampleParameters };
                    ctx.Commands.Add(sampleCommand);

                    ctx.SaveChanges();
                    cmbController.Items.Add("SSD1322");
                }
                else
                {
                    foreach (var item in controllerResult)
                    {
                        cmbController.Items.Add(item.ControllerName);
                    }
                }*/
            }
        }

        private void cmbController_SelectedIndexChanged(object sender, EventArgs e)
        {
//            lbCommands.Items.Clear();
            using (var ctx = new OLEDController())
            {
                lbCommands.DataSource = ctx.Commands.ToList();
                lbCommands.ValueMember = "CommandId";
                lbCommands.DisplayMember = "CommandName";

/*                var result = ctx.Commands.Find(cmbController.SelectedIndex + 1);

                if(result == null)
                {
                    return;
                }

                var controllerResult = from row in ctx.Commands
                                       where row.ControllerId == result.ControllerId
                                       select row;
                if(controllerResult.Count() > 0)
                {
                    foreach (var item in controllerResult)
                    {
                        lbCommands.Items.Add(item.CommandName);
                    }
                }
                else
                {
                    lbCommands.Items.Add("This Controller has no Commands associated with it");
                }*/
            }
        }

        private void cmdUpdate_Click(object sender, EventArgs e)
        {
            using (var ctx = new OLEDController())
            {
                var original = ctx.Commands.Find(txtCommand.Tag);

                if (original != null)
                {
                    original.CommandName = txtName.Text;
                    original.CommandValue = Convert.ToInt32(txtCommand.Text);
                    ctx.SaveChanges();
                    lbCommands.Items[(int)txtCommand.Tag - 1] = txtName.Text;
                }
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
                commandResult = commandResult.Where(p => p.CommandName == selectedCommand.CommandName);
                if (commandResult.Count() > 0)
                {
                    foreach (var item in commandResult)
                    {
                        txtName.Text = item.CommandName;
                        txtCommand.Tag = item.CommandId;
                        txtCommand.Text = item.CommandValue.ToString();
                        if(item.Parameters != null)
                        {
                            txtParameters.Text = item.Parameters.ToArray().ToString();
                        }
                    }
                }
            }
        }

        private void cmdInsert_Click(object sender, EventArgs e)
        {
            using (var ctx = new OLEDController())
            {
                ctx.Commands.Add(new ControllerCommand() { CommandName = txtName.Text, CommandValue = Convert.ToInt32(txtCommand.Text), ControllerId = cmbController.SelectedIndex + 1 });
                ctx.SaveChanges();
                cmbController_SelectedIndexChanged(sender, e);
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
                cmbController_SelectedIndexChanged(sender, e);
            }
        }
    }
}
