namespace EnablerApp
{
    partial class SfwSelectWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SfwSelectWindow));
            this.radioButtonSfw = new System.Windows.Forms.RadioButton();
            this.radioButtonNsfw = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonAcc = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.buttonCanc = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioButtonSfw
            // 
            this.radioButtonSfw.AutoSize = true;
            this.radioButtonSfw.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonSfw.Location = new System.Drawing.Point(3, 3);
            this.radioButtonSfw.Name = "radioButtonSfw";
            this.radioButtonSfw.Size = new System.Drawing.Size(655, 28);
            this.radioButtonSfw.TabIndex = 0;
            this.radioButtonSfw.Text = "Use safe mode (SFW, disable all* R18 content, for content creation)";
            this.radioButtonSfw.UseVisualStyleBackColor = true;
            this.radioButtonSfw.CheckedChanged += new System.EventHandler(this.radioButtonSfw_CheckedChanged);
            // 
            // radioButtonNsfw
            // 
            this.radioButtonNsfw.AutoSize = true;
            this.radioButtonNsfw.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonNsfw.Location = new System.Drawing.Point(3, 116);
            this.radioButtonNsfw.Name = "radioButtonNsfw";
            this.radioButtonNsfw.Size = new System.Drawing.Size(491, 28);
            this.radioButtonNsfw.TabIndex = 1;
            this.radioButtonNsfw.Text = "Use adult mode (NSFW, all features stay enabled)";
            this.radioButtonNsfw.UseVisualStyleBackColor = true;
            this.radioButtonNsfw.CheckedChanged += new System.EventHandler(this.radioButtonNsfw_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.flowLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(12, 4, 12, 12);
            this.panel1.Size = new System.Drawing.Size(800, 429);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.buttonAcc);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.buttonCanc);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(12, 378);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(776, 39);
            this.panel2.TabIndex = 4;
            // 
            // buttonAcc
            // 
            this.buttonAcc.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonAcc.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAcc.Location = new System.Drawing.Point(488, 0);
            this.buttonAcc.Name = "buttonAcc";
            this.buttonAcc.Size = new System.Drawing.Size(131, 39);
            this.buttonAcc.TabIndex = 3;
            this.buttonAcc.Text = "Accept";
            this.buttonAcc.UseVisualStyleBackColor = true;
            this.buttonAcc.Click += new System.EventHandler(this.buttonAcc_Click);
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(619, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(26, 39);
            this.panel3.TabIndex = 4;
            // 
            // buttonCanc
            // 
            this.buttonCanc.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCanc.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonCanc.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCanc.Location = new System.Drawing.Point(645, 0);
            this.buttonCanc.Name = "buttonCanc";
            this.buttonCanc.Size = new System.Drawing.Size(131, 39);
            this.buttonCanc.TabIndex = 3;
            this.buttonCanc.Text = "Cancel";
            this.buttonCanc.UseVisualStyleBackColor = true;
            this.buttonCanc.Click += new System.EventHandler(this.buttonCanc_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.radioButtonSfw);
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.label3);
            this.flowLayoutPanel1.Controls.Add(this.radioButtonNsfw);
            this.flowLayoutPanel1.Controls.Add(this.label2);
            this.flowLayoutPanel1.Controls.Add(this.label4);
            this.flowLayoutPanel1.Controls.Add(this.label6);
            this.flowLayoutPanel1.Controls.Add(this.label5);
            this.flowLayoutPanel1.Controls.Add(this.radioButton1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 4);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(776, 413);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 34);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.label1.Size = new System.Drawing.Size(767, 29);
            this.label1.TabIndex = 2;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 63);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(0, 0, 0, 11);
            this.label3.Size = new System.Drawing.Size(769, 50);
            this.label3.TabIndex = 4;
            this.label3.Text = resources.GetString("label3.Text");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 147);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.label2.Size = new System.Drawing.Size(741, 29);
            this.label2.TabIndex = 3;
            this.label2.Text = "Keep all of the game\'s content enabled, including adult content. The content crea" +
    "tion tools (character maker and studio), as well as all other available game mod" +
    "es will be fully unlocked.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 176);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(0, 0, 0, 11);
            this.label4.Size = new System.Drawing.Size(765, 76);
            this.label4.TabIndex = 5;
            this.label4.Text = resources.GetString("label4.Text");
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(3, 252);
            this.label6.Name = "label6";
            this.label6.Padding = new System.Windows.Forms.Padding(0, 8, 0, 3);
            this.label6.Size = new System.Drawing.Size(132, 31);
            this.label6.TabIndex = 6;
            this.label6.Text = "User agreements";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 283);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.label5.Size = new System.Drawing.Size(769, 86);
            this.label5.TabIndex = 5;
            this.label5.Text = resources.GetString("label5.Text");
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(3, 372);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(85, 17);
            this.radioButton1.TabIndex = 7;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "radioButton1";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.Visible = false;
            // 
            // label7
            // 
            this.label7.Dock = System.Windows.Forms.DockStyle.Left;
            this.label7.Location = new System.Drawing.Point(0, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(421, 39);
            this.label7.TabIndex = 5;
            this.label7.Text = "Your choice can be changed later by running this tool from your game\'s root.\r\nThi" +
    "s mod is entirely free. If you paid for it you got scammed.";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SfwSelectWindow
            // 
            this.AcceptButton = this.buttonAcc;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.CancelButton = this.buttonCanc;
            this.ClientSize = new System.Drawing.Size(800, 429);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SfwSelectWindow";
            this.Text = "SFW / NSFW mode selector";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButtonSfw;
        private System.Windows.Forms.RadioButton radioButtonNsfw;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonAcc;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button buttonCanc;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Label label7;
    }
}

