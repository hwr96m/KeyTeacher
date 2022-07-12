

namespace KeyTeacher {
    partial class Form1 {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabStatistic = new System.Windows.Forms.TabPage();
            this.tabConfig = new System.Windows.Forms.TabPage();
            this.lblMetronomeDelay = new System.Windows.Forms.Label();
            this.trackMetronomeDelay = new System.Windows.Forms.TrackBar();
            this.checkMetronome = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboGenMode = new System.Windows.Forms.ComboBox();
            this.confWaitSuccessPress = new System.Windows.Forms.CheckBox();
            this.confEN = new System.Windows.Forms.CheckBox();
            this.confRU = new System.Windows.Forms.CheckBox();
            this.confPunctuationMarks = new System.Windows.Forms.CheckBox();
            this.confNumbers = new System.Windows.Forms.CheckBox();
            this.confUpper = new System.Windows.Forms.CheckBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tboxShortText = new System.Windows.Forms.RichTextBox();
            this.bStartStop = new System.Windows.Forms.Button();
            this.tboxFullText = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabConfig.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackMetronomeDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Cursor = System.Windows.Forms.Cursors.VSplit;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            this.splitContainer1.Panel1.Cursor = System.Windows.Forms.Cursors.Default;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1998, 965);
            this.splitContainer1.SplitterDistance = 386;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabStatistic);
            this.tabControl1.Controls.Add(this.tabConfig);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(382, 961);
            this.tabControl1.TabIndex = 0;
            // 
            // tabStatistic
            // 
            this.tabStatistic.AutoScroll = true;
            this.tabStatistic.Location = new System.Drawing.Point(4, 39);
            this.tabStatistic.Name = "tabStatistic";
            this.tabStatistic.Padding = new System.Windows.Forms.Padding(3);
            this.tabStatistic.Size = new System.Drawing.Size(374, 918);
            this.tabStatistic.TabIndex = 0;
            this.tabStatistic.Text = "Статистика";
            this.tabStatistic.UseVisualStyleBackColor = true;
            // 
            // tabConfig
            // 
            this.tabConfig.AutoScroll = true;
            this.tabConfig.Controls.Add(this.lblMetronomeDelay);
            this.tabConfig.Controls.Add(this.trackMetronomeDelay);
            this.tabConfig.Controls.Add(this.checkMetronome);
            this.tabConfig.Controls.Add(this.label1);
            this.tabConfig.Controls.Add(this.comboGenMode);
            this.tabConfig.Controls.Add(this.confWaitSuccessPress);
            this.tabConfig.Controls.Add(this.confEN);
            this.tabConfig.Controls.Add(this.confRU);
            this.tabConfig.Controls.Add(this.confPunctuationMarks);
            this.tabConfig.Controls.Add(this.confNumbers);
            this.tabConfig.Controls.Add(this.confUpper);
            this.tabConfig.Location = new System.Drawing.Point(4, 39);
            this.tabConfig.Name = "tabConfig";
            this.tabConfig.Padding = new System.Windows.Forms.Padding(3);
            this.tabConfig.Size = new System.Drawing.Size(374, 918);
            this.tabConfig.TabIndex = 1;
            this.tabConfig.Text = "Настройки";
            this.tabConfig.UseVisualStyleBackColor = true;
            // 
            // lblMetronomeDelay
            // 
            this.lblMetronomeDelay.AutoSize = true;
            this.lblMetronomeDelay.Location = new System.Drawing.Point(159, 359);
            this.lblMetronomeDelay.Name = "lblMetronomeDelay";
            this.lblMetronomeDelay.Size = new System.Drawing.Size(95, 30);
            this.lblMetronomeDelay.TabIndex = 10;
            this.lblMetronomeDelay.Text = "1000 ms";
            // 
            // trackMetronomeDelay
            // 
            this.trackMetronomeDelay.Enabled = false;
            this.trackMetronomeDelay.Location = new System.Drawing.Point(6, 398);
            this.trackMetronomeDelay.Maximum = 2000;
            this.trackMetronomeDelay.Minimum = 300;
            this.trackMetronomeDelay.Name = "trackMetronomeDelay";
            this.trackMetronomeDelay.Size = new System.Drawing.Size(362, 69);
            this.trackMetronomeDelay.TabIndex = 9;
            this.trackMetronomeDelay.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackMetronomeDelay.Value = 1000;
            this.trackMetronomeDelay.ValueChanged += new System.EventHandler(this.trackMetronomeDelay_ValueChanged);
            // 
            // checkMetronome
            // 
            this.checkMetronome.AutoSize = true;
            this.checkMetronome.Location = new System.Drawing.Point(6, 358);
            this.checkMetronome.Name = "checkMetronome";
            this.checkMetronome.Size = new System.Drawing.Size(147, 34);
            this.checkMetronome.TabIndex = 8;
            this.checkMetronome.Text = "Метроном";
            this.checkMetronome.UseVisualStyleBackColor = true;
            this.checkMetronome.Click += new System.EventHandler(this.checkMetronome_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(263, 30);
            this.label1.TabIndex = 7;
            this.label1.Text = "Режим генерации текста";
            // 
            // comboGenMode
            // 
            this.comboGenMode.FormattingEnabled = true;
            this.comboGenMode.Items.AddRange(new object[] {
            "Word",
            "Symbol"});
            this.comboGenMode.Location = new System.Drawing.Point(6, 61);
            this.comboGenMode.Name = "comboGenMode";
            this.comboGenMode.Size = new System.Drawing.Size(321, 38);
            this.comboGenMode.TabIndex = 6;
            this.comboGenMode.Tag = "";
            this.comboGenMode.SelectedIndexChanged += new System.EventHandler(this.comboGenMode_SelectedIndexChanged);
            // 
            // confWaitSuccessPress
            // 
            this.confWaitSuccessPress.AutoSize = true;
            this.confWaitSuccessPress.Location = new System.Drawing.Point(6, 308);
            this.confWaitSuccessPress.Name = "confWaitSuccessPress";
            this.confWaitSuccessPress.Size = new System.Drawing.Size(321, 34);
            this.confWaitSuccessPress.TabIndex = 5;
            this.confWaitSuccessPress.Text = "Ждать правильное нажатие";
            this.confWaitSuccessPress.UseVisualStyleBackColor = true;
            this.confWaitSuccessPress.Click += new System.EventHandler(this.confWaitSuccessPress_Click);
            // 
            // confEN
            // 
            this.confEN.AutoSize = true;
            this.confEN.Location = new System.Drawing.Point(6, 265);
            this.confEN.Name = "confEN";
            this.confEN.Size = new System.Drawing.Size(66, 34);
            this.confEN.TabIndex = 4;
            this.confEN.Text = "EN";
            this.confEN.UseVisualStyleBackColor = true;
            this.confEN.Click += new System.EventHandler(this.confEN_Click);
            // 
            // confRU
            // 
            this.confRU.AutoSize = true;
            this.confRU.Checked = true;
            this.confRU.CheckState = System.Windows.Forms.CheckState.Checked;
            this.confRU.Location = new System.Drawing.Point(6, 225);
            this.confRU.Name = "confRU";
            this.confRU.Size = new System.Drawing.Size(67, 34);
            this.confRU.TabIndex = 3;
            this.confRU.Text = "RU";
            this.confRU.UseVisualStyleBackColor = true;
            this.confRU.Click += new System.EventHandler(this.confRU_Click);
            // 
            // confPunctuationMarks
            // 
            this.confPunctuationMarks.AutoSize = true;
            this.confPunctuationMarks.Location = new System.Drawing.Point(6, 185);
            this.confPunctuationMarks.Name = "confPunctuationMarks";
            this.confPunctuationMarks.Size = new System.Drawing.Size(230, 34);
            this.confPunctuationMarks.TabIndex = 2;
            this.confPunctuationMarks.Text = "Знаки препинания";
            this.confPunctuationMarks.UseVisualStyleBackColor = true;
            this.confPunctuationMarks.Click += new System.EventHandler(this.confPunctuationMarks_Click);
            // 
            // confNumbers
            // 
            this.confNumbers.AutoSize = true;
            this.confNumbers.Location = new System.Drawing.Point(6, 145);
            this.confNumbers.Name = "confNumbers";
            this.confNumbers.Size = new System.Drawing.Size(112, 34);
            this.confNumbers.TabIndex = 1;
            this.confNumbers.Text = "Цифры";
            this.confNumbers.UseVisualStyleBackColor = true;
            this.confNumbers.Click += new System.EventHandler(this.confNumbers_Click);
            // 
            // confUpper
            // 
            this.confUpper.AutoSize = true;
            this.confUpper.Location = new System.Drawing.Point(6, 105);
            this.confUpper.Name = "confUpper";
            this.confUpper.Size = new System.Drawing.Size(215, 34);
            this.confUpper.TabIndex = 0;
            this.confUpper.Text = "Заглавные буквы";
            this.confUpper.UseVisualStyleBackColor = true;
            this.confUpper.Click += new System.EventHandler(this.confUpper_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer2.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tboxShortText);
            this.splitContainer2.Panel1.Controls.Add(this.bStartStop);
            this.splitContainer2.Panel1.Cursor = System.Windows.Forms.Cursors.Default;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tboxFullText);
            this.splitContainer2.Size = new System.Drawing.Size(1608, 965);
            this.splitContainer2.SplitterDistance = 261;
            this.splitContainer2.TabIndex = 1;
            // 
            // tboxShortText
            // 
            this.tboxShortText.Enabled = false;
            this.tboxShortText.Font = new System.Drawing.Font("Consolas", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tboxShortText.Location = new System.Drawing.Point(15, 29);
            this.tboxShortText.Name = "tboxShortText";
            this.tboxShortText.Size = new System.Drawing.Size(1427, 168);
            this.tboxShortText.TabIndex = 3;
            this.tboxShortText.Text = "";
            // 
            // bStartStop
            // 
            this.bStartStop.Location = new System.Drawing.Point(1463, 29);
            this.bStartStop.Name = "bStartStop";
            this.bStartStop.Size = new System.Drawing.Size(112, 34);
            this.bStartStop.TabIndex = 1;
            this.bStartStop.Text = "Start";
            this.bStartStop.UseVisualStyleBackColor = true;
            this.bStartStop.Click += new System.EventHandler(this.bStartStop_Click);
            // 
            // tboxFullText
            // 
            this.tboxFullText.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tboxFullText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tboxFullText.Enabled = false;
            this.tboxFullText.Location = new System.Drawing.Point(0, 0);
            this.tboxFullText.Name = "tboxFullText";
            this.tboxFullText.Size = new System.Drawing.Size(1604, 696);
            this.tboxFullText.TabIndex = 0;
            this.tboxFullText.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1998, 965);
            this.Controls.Add(this.splitContainer1);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Form1";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabConfig.ResumeLayout(false);
            this.tabConfig.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackMetronomeDelay)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button bStartStop;
        private System.Windows.Forms.RichTextBox tboxFullText;
        private System.Windows.Forms.RichTextBox tboxShortText;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabStatistic;
        private System.Windows.Forms.TabPage tabConfig;
        private System.Windows.Forms.CheckBox confWaitSuccessPress;
        private System.Windows.Forms.CheckBox confEN;
        private System.Windows.Forms.CheckBox confRU;
        private System.Windows.Forms.CheckBox confPunctuationMarks;
        private System.Windows.Forms.CheckBox confNumbers;
        private System.Windows.Forms.CheckBox confUpper;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboGenMode;
        private System.Windows.Forms.TrackBar trackMetronomeDelay;
        private System.Windows.Forms.CheckBox checkMetronome;
        private System.Windows.Forms.Label lblMetronomeDelay;
    }
}



