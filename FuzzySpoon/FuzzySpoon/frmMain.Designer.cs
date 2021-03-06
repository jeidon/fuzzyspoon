﻿namespace FuzzySpoon
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmdConnect = new System.Windows.Forms.Button();
            this.cmbBaud = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbPort = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSendImgArray2 = new System.Windows.Forms.Button();
            this.btnSendImgArray1 = new System.Windows.Forms.Button();
            this.btnInvert = new System.Windows.Forms.Button();
            this.btnNormal = new System.Windows.Forms.Button();
            this.btnFill2 = new System.Windows.Forms.Button();
            this.cmbFill1 = new System.Windows.Forms.Button();
            this.cmdSend = new System.Windows.Forms.Button();
            this.cmbCommands = new System.Windows.Forms.ComboBox();
            this.cmdEdit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbController = new System.Windows.Forms.ComboBox();
            this.cmdInit = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmdConnect);
            this.groupBox1.Controls.Add(this.cmbBaud);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cmbPort);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(379, 127);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select a Port and Baud Rate to connect";
            // 
            // cmdConnect
            // 
            this.cmdConnect.Enabled = false;
            this.cmdConnect.Location = new System.Drawing.Point(246, 34);
            this.cmdConnect.Name = "cmdConnect";
            this.cmdConnect.Size = new System.Drawing.Size(121, 61);
            this.cmdConnect.TabIndex = 30;
            this.cmdConnect.Text = "Connect";
            this.cmdConnect.UseVisualStyleBackColor = true;
            this.cmdConnect.Click += new System.EventHandler(this.cmdConnect_Click);
            // 
            // cmbBaud
            // 
            this.cmbBaud.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBaud.FormattingEnabled = true;
            this.cmbBaud.Location = new System.Drawing.Point(6, 74);
            this.cmbBaud.Name = "cmbBaud";
            this.cmbBaud.Size = new System.Drawing.Size(121, 21);
            this.cmbBaud.TabIndex = 29;
            this.cmbBaud.SelectedIndexChanged += new System.EventHandler(this.cmbBaud_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 26;
            this.label2.Text = "Port";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 28;
            this.label3.Text = "Baud";
            // 
            // cmbPort
            // 
            this.cmbPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPort.FormattingEnabled = true;
            this.cmbPort.Location = new System.Drawing.Point(6, 34);
            this.cmbPort.Name = "cmbPort";
            this.cmbPort.Size = new System.Drawing.Size(121, 21);
            this.cmbPort.TabIndex = 27;
            this.cmbPort.SelectedIndexChanged += new System.EventHandler(this.cmbPort_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSendImgArray2);
            this.groupBox2.Controls.Add(this.btnSendImgArray1);
            this.groupBox2.Controls.Add(this.btnInvert);
            this.groupBox2.Controls.Add(this.btnNormal);
            this.groupBox2.Controls.Add(this.btnFill2);
            this.groupBox2.Controls.Add(this.cmbFill1);
            this.groupBox2.Controls.Add(this.cmdSend);
            this.groupBox2.Controls.Add(this.cmbCommands);
            this.groupBox2.Controls.Add(this.cmdEdit);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cmbController);
            this.groupBox2.Controls.Add(this.cmdInit);
            this.groupBox2.Enabled = false;
            this.groupBox2.Location = new System.Drawing.Point(12, 145);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(379, 210);
            this.groupBox2.TabIndex = 31;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Controller Interface";
            // 
            // btnSendImgArray2
            // 
            this.btnSendImgArray2.Location = new System.Drawing.Point(272, 152);
            this.btnSendImgArray2.Name = "btnSendImgArray2";
            this.btnSendImgArray2.Size = new System.Drawing.Size(95, 52);
            this.btnSendImgArray2.TabIndex = 42;
            this.btnSendImgArray2.Text = "Send Picture2";
            this.btnSendImgArray2.UseVisualStyleBackColor = true;
            this.btnSendImgArray2.Click += new System.EventHandler(this.btnSendImgArray2_Click);
            // 
            // btnSendImgArray1
            // 
            this.btnSendImgArray1.Location = new System.Drawing.Point(171, 152);
            this.btnSendImgArray1.Name = "btnSendImgArray1";
            this.btnSendImgArray1.Size = new System.Drawing.Size(95, 52);
            this.btnSendImgArray1.TabIndex = 41;
            this.btnSendImgArray1.Text = "Send Picture1";
            this.btnSendImgArray1.UseVisualStyleBackColor = true;
            this.btnSendImgArray1.Click += new System.EventHandler(this.btnSendImgArray1_Click);
            // 
            // btnInvert
            // 
            this.btnInvert.Location = new System.Drawing.Point(90, 152);
            this.btnInvert.Name = "btnInvert";
            this.btnInvert.Size = new System.Drawing.Size(75, 23);
            this.btnInvert.TabIndex = 40;
            this.btnInvert.Text = "Invert";
            this.btnInvert.UseVisualStyleBackColor = true;
            this.btnInvert.Click += new System.EventHandler(this.btnInvert_Click);
            // 
            // btnNormal
            // 
            this.btnNormal.Location = new System.Drawing.Point(9, 152);
            this.btnNormal.Name = "btnNormal";
            this.btnNormal.Size = new System.Drawing.Size(75, 23);
            this.btnNormal.TabIndex = 39;
            this.btnNormal.Text = "Normal";
            this.btnNormal.UseVisualStyleBackColor = true;
            this.btnNormal.Click += new System.EventHandler(this.btnNormal_Click);
            // 
            // btnFill2
            // 
            this.btnFill2.Location = new System.Drawing.Point(90, 181);
            this.btnFill2.Name = "btnFill2";
            this.btnFill2.Size = new System.Drawing.Size(75, 23);
            this.btnFill2.TabIndex = 38;
            this.btnFill2.Text = "Fill 2";
            this.btnFill2.UseVisualStyleBackColor = true;
            this.btnFill2.Click += new System.EventHandler(this.btnFill2_Click);
            // 
            // cmbFill1
            // 
            this.cmbFill1.Location = new System.Drawing.Point(9, 181);
            this.cmbFill1.Name = "cmbFill1";
            this.cmbFill1.Size = new System.Drawing.Size(75, 23);
            this.cmbFill1.TabIndex = 37;
            this.cmbFill1.Text = "Fill 1";
            this.cmbFill1.UseVisualStyleBackColor = true;
            this.cmbFill1.Click += new System.EventHandler(this.cmbFill1_Click);
            // 
            // cmdSend
            // 
            this.cmdSend.Location = new System.Drawing.Point(292, 115);
            this.cmdSend.Name = "cmdSend";
            this.cmdSend.Size = new System.Drawing.Size(75, 23);
            this.cmdSend.TabIndex = 36;
            this.cmdSend.Text = "Send";
            this.cmdSend.UseVisualStyleBackColor = true;
            // 
            // cmbCommands
            // 
            this.cmbCommands.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCommands.FormattingEnabled = true;
            this.cmbCommands.Location = new System.Drawing.Point(9, 88);
            this.cmbCommands.Name = "cmbCommands";
            this.cmbCommands.Size = new System.Drawing.Size(358, 21);
            this.cmbCommands.TabIndex = 35;
            // 
            // cmdEdit
            // 
            this.cmdEdit.Location = new System.Drawing.Point(292, 59);
            this.cmdEdit.Name = "cmdEdit";
            this.cmdEdit.Size = new System.Drawing.Size(75, 23);
            this.cmdEdit.TabIndex = 34;
            this.cmdEdit.Text = "Edit";
            this.cmdEdit.UseVisualStyleBackColor = true;
            this.cmdEdit.Click += new System.EventHandler(this.cmdEdit_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 33;
            this.label1.Text = "Controller";
            // 
            // cmbController
            // 
            this.cmbController.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbController.FormattingEnabled = true;
            this.cmbController.Location = new System.Drawing.Point(9, 32);
            this.cmbController.Name = "cmbController";
            this.cmbController.Size = new System.Drawing.Size(358, 21);
            this.cmbController.TabIndex = 32;
            this.cmbController.SelectedIndexChanged += new System.EventHandler(this.cmbController_SelectedIndexChanged);
            // 
            // cmdInit
            // 
            this.cmdInit.Location = new System.Drawing.Point(9, 59);
            this.cmdInit.Name = "cmdInit";
            this.cmdInit.Size = new System.Drawing.Size(75, 23);
            this.cmdInit.TabIndex = 31;
            this.cmdInit.Text = "Initialize";
            this.cmdInit.UseVisualStyleBackColor = false;
            this.cmdInit.Click += new System.EventHandler(this.cmdInit_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 366);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmMain";
            this.Text = "Windows Based OLED Controll Panel";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button cmdConnect;
        private System.Windows.Forms.ComboBox cmbBaud;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbPort;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbController;
        private System.Windows.Forms.Button cmdInit;
        private System.Windows.Forms.Button cmdEdit;
        private System.Windows.Forms.Button cmdSend;
        private System.Windows.Forms.ComboBox cmbCommands;
        private System.Windows.Forms.Button cmbFill1;
        private System.Windows.Forms.Button btnFill2;
        private System.Windows.Forms.Button btnNormal;
        private System.Windows.Forms.Button btnInvert;
//        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Button btnSendImgArray1;
        private System.Windows.Forms.Button btnSendImgArray2;
    }
}

