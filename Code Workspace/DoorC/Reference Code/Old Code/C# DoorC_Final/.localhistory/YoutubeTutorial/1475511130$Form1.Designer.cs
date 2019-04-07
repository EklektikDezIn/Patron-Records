namespace WindowsFormsApplication1
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.bClear = new System.Windows.Forms.Button();
            this.bSend = new System.Windows.Forms.Button();
            this.tbTX = new System.Windows.Forms.RichTextBox();
            this.tbRX = new System.Windows.Forms.RichTextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.mySerialPort = new System.IO.Ports.SerialPort(this.components);
            this.SuspendLayout();
            // 
            // bClear
            // 
            this.bClear.Location = new System.Drawing.Point(643, 227);
            this.bClear.Name = "bClear";
            this.bClear.Size = new System.Drawing.Size(75, 23);
            this.bClear.TabIndex = 0;
            this.bClear.Text = "Clear";
            this.bClear.UseVisualStyleBackColor = true;
            this.bClear.Click += new System.EventHandler(this.bClear_Click);
            // 
            // bSend
            // 
            this.bSend.Location = new System.Drawing.Point(643, 12);
            this.bSend.Name = "bSend";
            this.bSend.Size = new System.Drawing.Size(75, 23);
            this.bSend.TabIndex = 1;
            this.bSend.Text = "Send";
            this.bSend.UseVisualStyleBackColor = true;
            this.bSend.Click += new System.EventHandler(this.bSend_Click);
            // 
            // tbTX
            // 
            this.tbTX.Location = new System.Drawing.Point(12, 12);
            this.tbTX.Name = "tbTX";
            this.tbTX.Size = new System.Drawing.Size(615, 39);
            this.tbTX.TabIndex = 2;
            this.tbTX.Text = "";
            this.tbTX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbTX_KeyPress);
            // 
            // tbRX
            // 
            this.tbRX.Location = new System.Drawing.Point(12, 57);
            this.tbRX.Name = "tbRX";
            this.tbRX.Size = new System.Drawing.Size(616, 193);
            this.tbRX.TabIndex = 3;
            this.tbRX.Text = "";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(643, 41);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(38, 17);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "IM";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // mySerialPort
            // 
            this.mySerialPort.PortName = "COM9";
            this.mySerialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.mySerialPort_DataReceived);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 262);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.tbRX);
            this.Controls.Add(this.tbTX);
            this.Controls.Add(this.bSend);
            this.Controls.Add(this.bClear);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bClear;
        private System.Windows.Forms.Button bSend;
        private System.Windows.Forms.RichTextBox tbTX;
        private System.Windows.Forms.RichTextBox tbRX;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.IO.Ports.SerialPort mySerialPort;
    }
}

