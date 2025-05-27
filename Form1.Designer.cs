namespace SerialPortMonitor
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.comboBoxPorts = new System.Windows.Forms.ComboBox();
            this.comboBoxBaudRate = new System.Windows.Forms.ComboBox();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelBufferSize = new System.Windows.Forms.Label();
            this.textBoxBufferSize = new System.Windows.Forms.TextBox();
            this.labelMissedHeaders = new System.Windows.Forms.Label();
            this.textBoxMissedHeaders = new System.Windows.Forms.TextBox();
            this.checkBoxShowData = new System.Windows.Forms.CheckBox();
            this.timerBufferCheck = new System.Windows.Forms.Timer(this.components);
            this.labelSaveCount = new System.Windows.Forms.Label();
            this.textBoxSaveCount = new System.Windows.Forms.TextBox();
            this.labelBytesToRead = new System.Windows.Forms.Label();
            this.textBoxBytesToRead = new System.Windows.Forms.TextBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.SuspendLayout();
            // 
            // comboBoxPorts
            // 
            this.comboBoxPorts.FormattingEnabled = true;
            this.comboBoxPorts.Location = new System.Drawing.Point(12, 23);
            this.comboBoxPorts.Name = "comboBoxPorts";
            this.comboBoxPorts.Size = new System.Drawing.Size(70, 20);
            this.comboBoxPorts.TabIndex = 0;
            // 
            // comboBoxBaudRate
            // 
            this.comboBoxBaudRate.FormattingEnabled = true;
            this.comboBoxBaudRate.Location = new System.Drawing.Point(87, 23);
            this.comboBoxBaudRate.Name = "comboBoxBaudRate";
            this.comboBoxBaudRate.Size = new System.Drawing.Size(70, 20);
            this.comboBoxBaudRate.TabIndex = 1;
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(244, 11);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(70, 32);
            this.buttonConnect.TabIndex = 2;
            this.buttonConnect.Text = "連接";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // textBoxLog
            // 
            this.textBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLog.Location = new System.Drawing.Point(12, 50);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLog.Size = new System.Drawing.Size(379, 184);
            this.textBoxLog.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "串列埠：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(84, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "鮑率：";
            // 
            // labelBufferSize
            // 
            this.labelBufferSize.AutoSize = true;
            this.labelBufferSize.Location = new System.Drawing.Point(397, 126);
            this.labelBufferSize.Name = "labelBufferSize";
            this.labelBufferSize.Size = new System.Drawing.Size(77, 12);
            this.labelBufferSize.TabIndex = 6;
            this.labelBufferSize.Text = "待處理字元數";
            // 
            // textBoxBufferSize
            // 
            this.textBoxBufferSize.Location = new System.Drawing.Point(399, 141);
            this.textBoxBufferSize.Name = "textBoxBufferSize";
            this.textBoxBufferSize.ReadOnly = true;
            this.textBoxBufferSize.Size = new System.Drawing.Size(72, 22);
            this.textBoxBufferSize.TabIndex = 7;
            this.textBoxBufferSize.Text = "0";
            // 
            // labelMissedHeaders
            // 
            this.labelMissedHeaders.AutoSize = true;
            this.labelMissedHeaders.Location = new System.Drawing.Point(397, 180);
            this.labelMissedHeaders.Name = "labelMissedHeaders";
            this.labelMissedHeaders.Size = new System.Drawing.Size(89, 12);
            this.labelMissedHeaders.TabIndex = 8;
            this.labelMissedHeaders.Text = "找不到標頭次數";
            // 
            // textBoxMissedHeaders
            // 
            this.textBoxMissedHeaders.Location = new System.Drawing.Point(399, 195);
            this.textBoxMissedHeaders.Name = "textBoxMissedHeaders";
            this.textBoxMissedHeaders.ReadOnly = true;
            this.textBoxMissedHeaders.Size = new System.Drawing.Size(72, 22);
            this.textBoxMissedHeaders.TabIndex = 9;
            this.textBoxMissedHeaders.Text = "0";
            // 
            // checkBoxShowData
            // 
            this.checkBoxShowData.AutoSize = true;
            this.checkBoxShowData.Location = new System.Drawing.Point(10, 240);
            this.checkBoxShowData.Name = "checkBoxShowData";
            this.checkBoxShowData.Size = new System.Drawing.Size(72, 16);
            this.checkBoxShowData.TabIndex = 10;
            this.checkBoxShowData.Text = "顯示數據";
            this.checkBoxShowData.UseVisualStyleBackColor = true;
           
            // 
            // labelSaveCount
            // 
            this.labelSaveCount.AutoSize = true;
            this.labelSaveCount.Location = new System.Drawing.Point(170, 6);
            this.labelSaveCount.Name = "labelSaveCount";
            this.labelSaveCount.Size = new System.Drawing.Size(53, 12);
            this.labelSaveCount.TabIndex = 11;
            this.labelSaveCount.Text = "存檔筆數";
            // 
            // textBoxSaveCount
            // 
            this.textBoxSaveCount.Location = new System.Drawing.Point(172, 21);
            this.textBoxSaveCount.Name = "textBoxSaveCount";
            this.textBoxSaveCount.Size = new System.Drawing.Size(50, 22);
            this.textBoxSaveCount.TabIndex = 12;
            this.textBoxSaveCount.Text = "10000";
            // 
            // labelBytesToRead
            // 
            this.labelBytesToRead.AutoSize = true;
            this.labelBytesToRead.Location = new System.Drawing.Point(397, 75);
            this.labelBytesToRead.Name = "labelBytesToRead";
            this.labelBytesToRead.Size = new System.Drawing.Size(77, 12);
            this.labelBytesToRead.TabIndex = 13;
            this.labelBytesToRead.Text = "暫存器字元數";
            // 
            // textBoxBytesToRead
            // 
            this.textBoxBytesToRead.Location = new System.Drawing.Point(399, 90);
            this.textBoxBytesToRead.Name = "textBoxBytesToRead";
            this.textBoxBytesToRead.ReadOnly = true;
            this.textBoxBytesToRead.Size = new System.Drawing.Size(72, 22);
            this.textBoxBytesToRead.TabIndex = 14;
            this.textBoxBytesToRead.Text = "0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 264);
            this.Controls.Add(this.textBoxBytesToRead);
            this.Controls.Add(this.labelBytesToRead);
            this.Controls.Add(this.textBoxSaveCount);
            this.Controls.Add(this.labelSaveCount);
            this.Controls.Add(this.checkBoxShowData);
            this.Controls.Add(this.textBoxMissedHeaders);
            this.Controls.Add(this.labelMissedHeaders);
            this.Controls.Add(this.textBoxBufferSize);
            this.Controls.Add(this.labelBufferSize);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxLog);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.comboBoxBaudRate);
            this.Controls.Add(this.comboBoxPorts);
            this.Name = "Form1";
            this.Text = "串列埠監控程式";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.ColorDialog colorDialog1;

        private System.Windows.Forms.ComboBox comboBoxPorts;
        private System.Windows.Forms.ComboBox comboBoxBaudRate;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelBufferSize;
        private System.Windows.Forms.TextBox textBoxBufferSize;
        private System.Windows.Forms.Label labelMissedHeaders;
        private System.Windows.Forms.TextBox textBoxMissedHeaders;
        private System.Windows.Forms.CheckBox checkBoxShowData;
        private System.Windows.Forms.Timer timerBufferCheck;
        private System.Windows.Forms.Label labelSaveCount;
        private System.Windows.Forms.TextBox textBoxSaveCount;
        private System.Windows.Forms.Label labelBytesToRead;
        private System.Windows.Forms.TextBox textBoxBytesToRead;
    }
} 