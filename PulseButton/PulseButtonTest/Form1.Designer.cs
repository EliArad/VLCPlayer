namespace PulseButtonTest
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
            this.pulseButton5 = new PulseButton.PulseButton();
            this.pulseButton3 = new PulseButton.PulseButton();
            this.pulseButton4 = new PulseButton.PulseButton();
            this.pulseButton2 = new PulseButton.PulseButton();
            this.pulseButton1 = new PulseButton.PulseButton();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // pulseButton5
            // 
            this.pulseButton5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pulseButton5.Image = global::PulseButtonTest.Properties.Resources.Symbol_Plus;
            this.pulseButton5.Location = new System.Drawing.Point(128, 250);
            this.pulseButton5.Name = "pulseButton5";
            this.pulseButton5.Size = new System.Drawing.Size(40, 40);
            this.pulseButton5.TabIndex = 5;
            this.pulseButton5.UseVisualStyleBackColor = true;
            this.pulseButton5.PulseSpeed = 0.3F;
            this.pulseButton5.Click += new System.EventHandler(this.button_Click);
            // 
            // pulseButton3
            // 
            this.pulseButton3.ButtonColorBottom = System.Drawing.Color.Navy;
            this.pulseButton3.ButtonColorTop = System.Drawing.Color.DeepSkyBlue;
            this.pulseButton3.CornerRadius = 5;
            this.pulseButton3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pulseButton3.Location = new System.Drawing.Point(14, 12);
            this.pulseButton3.Name = "pulseButton3";
            this.pulseButton3.NumberOfPulses = 2;
            this.pulseButton3.PulseColor = System.Drawing.Color.Navy;
            this.pulseButton3.PulseWidth = 20;
            this.pulseButton3.Size = new System.Drawing.Size(130, 87);
            this.pulseButton3.TabIndex = 3;
            this.pulseButton3.Text = "pulseButton3";
            this.pulseButton3.PulseSpeed = 1F;
            this.pulseButton3.Click += new System.EventHandler(this.button_Click);
            // 
            // pulseButton4
            // 
            this.pulseButton4.ButtonColorTop = System.Drawing.Color.AliceBlue;
            this.pulseButton4.FocusColor = System.Drawing.Color.Lime;
            this.pulseButton4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pulseButton4.Location = new System.Drawing.Point(194, 172);
            this.pulseButton4.Name = "pulseButton4";
            this.pulseButton4.NumberOfPulses = 1;
            this.pulseButton4.PulseColor = System.Drawing.Color.Orange;
            this.pulseButton4.PulseWidth = 5;
            this.pulseButton4.ShapeType = PulseButton.PulseButton.Shape.Rectangle;
            this.pulseButton4.Size = new System.Drawing.Size(111, 99);
            this.pulseButton4.TabIndex = 4;
            this.pulseButton4.Text = "&pulseButton4";
            this.pulseButton4.PulseSpeed = 0.0F;
            this.pulseButton4.Click += new System.EventHandler(this.button_Click);
            // 
            // pulseButton2
            // 
            this.pulseButton2.ButtonColorBottom = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.pulseButton2.ButtonColorTop = System.Drawing.Color.Lime;
            this.pulseButton2.CornerRadius = 5;
            this.pulseButton2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pulseButton2.Location = new System.Drawing.Point(183, 12);
            this.pulseButton2.Name = "pulseButton2";
            this.pulseButton2.NumberOfPulses = 2;
            this.pulseButton2.PulseWidth = 20;
            this.pulseButton2.ShapeType = PulseButton.PulseButton.Shape.Rectangle;
            this.pulseButton2.Size = new System.Drawing.Size(122, 143);
            this.pulseButton2.TabIndex = 2;
            this.pulseButton2.Text = "pulseButton2";
            this.pulseButton2.PulseSpeed = 0.6F;
            this.pulseButton2.Click += new System.EventHandler(this.button_Click);
            // 
            // pulseButton1
            // 
            this.pulseButton1.ButtonColorBottom = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.pulseButton1.ButtonColorTop = System.Drawing.Color.Maroon;
            this.pulseButton1.CornerRadius = 5;
            this.pulseButton1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.pulseButton1.Image = global::PulseButtonTest.Properties.Resources.Symbol_Plus;
            this.pulseButton1.Location = new System.Drawing.Point(12, 105);
            this.pulseButton1.Name = "pulseButton1";
            this.pulseButton1.PulseColor = System.Drawing.Color.Red;
            this.pulseButton1.PulseWidth = 20;
            this.pulseButton1.Size = new System.Drawing.Size(138, 139);
            this.pulseButton1.TabIndex = 1;
            this.pulseButton1.PulseSpeed = 0.6F;
            this.pulseButton1.Click += new System.EventHandler(this.button_Click);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Right;
            this.propertyGrid1.Location = new System.Drawing.Point(363, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.SelectedObject = this.pulseButton1;
            this.propertyGrid1.Size = new System.Drawing.Size(232, 310);
            this.propertyGrid1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(595, 310);
            this.Controls.Add(this.pulseButton5);
            this.Controls.Add(this.pulseButton3);
            this.Controls.Add(this.pulseButton4);
            this.Controls.Add(this.pulseButton2);
            this.Controls.Add(this.pulseButton1);
            this.Controls.Add(this.propertyGrid1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private PulseButton.PulseButton pulseButton1;
        private PulseButton.PulseButton pulseButton2;
        private PulseButton.PulseButton pulseButton3;
        private PulseButton.PulseButton pulseButton4;
        private PulseButton.PulseButton pulseButton5;
    }
}

