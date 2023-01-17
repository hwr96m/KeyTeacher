using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace KeyTeacher {
    public partial class Form1 : Form {        
        Typing typing; 

        readonly string CustomCollection_FILE = $"{Environment.CurrentDirectory}\\dictionary\\custom_collection.txt";
        void SetConfig() {
            typing.FormConfig.tShortText = tboxShortText;
            typing.FormConfig.tFullText = tboxFullText;
            typing.FormConfig.tabStatistic = tabStatistic;
            typing.FormConfig.tabConfig = tabConfig;
            typing.FormConfig.bStart = bStart;
            typing.FormConfig.bStop = bStop;

            typing.FormConfig.Upper = confUpper.Checked;
            typing.FormConfig.Numbers = confNumbers.Checked;
            typing.FormConfig.PunctuationMarks = confPunctuationMarks.Checked;
            typing.FormConfig.RU = confRU.Checked;
            typing.FormConfig.EN = confEN.Checked;
            typing.FormConfig.WaitSuccessPress = confWaitSuccessPress.Checked;
            typing.FormConfig.GenMode = (byte)comboGenMode.SelectedIndex;
            typing.FormConfig.MetronomeEnabled = checkMetronome.Checked;
            typing.FormConfig.MetronomePeriod = trackMetronomeDelay.Value;
        }
        public Form1() {
            InitializeComponent();
            typing = new Typing();
            comboGenMode.SelectedIndex = 1;
            CustomCollectionCombo.Items.AddRange(File.ReadAllText(CustomCollection_FILE).Replace("\r","").Split("\n"));  //добавляем свой набор символов в комбобокс
            CustomCollectionCombo.SelectedIndex = 0;    
            SetConfig();
        }
        void Form1_KeyPress(object sender, KeyPressEventArgs e) {
            typing.KeyPressHandler(e.KeyChar);
        }
        private void bStart_Click(object sender, EventArgs e) {
            bStart.Enabled = false;
            bStop.Enabled = true;
            typing.Start();
            label2.Focus();
        }
        private void bStop_Click(object sender, EventArgs e) {
            bStart.Enabled = true;
            bStop.Enabled = false;
            typing.Stop();
            label2.Focus();
        }

        #region ---- вкладка Config ---------------
        private void confUpper_Click(object sender, EventArgs e) {
            typing.FormConfig.Upper = confUpper.Checked;
        }
        private void confNumbers_Click(object sender, EventArgs e) {
            typing.FormConfig.Numbers = confNumbers.Checked;
        }
        private void confPunctuationMarks_Click(object sender, EventArgs e) {
            typing.FormConfig.PunctuationMarks = confPunctuationMarks.Checked;
        }
        private void confWaitSuccessPress_Click(object sender, EventArgs e) {
            typing.FormConfig.WaitSuccessPress = confWaitSuccessPress.Checked;
        }
        private void confRU_Click(object sender, EventArgs e) {
            typing.FormConfig.RU = confRU.Checked;
            if (!confRU.Checked && !confEN.Checked) {
                confEN.Checked = true;
                typing.FormConfig.EN = true;
            }
        }
        private void confEN_Click(object sender, EventArgs e) {
            typing.FormConfig.EN = confEN.Checked;
            if (!confRU.Checked && !confEN.Checked) {
                confRU.Checked = true;
                typing.FormConfig.RU = true;
            }

        }
        private void comboGenMode_SelectedIndexChanged(object sender, EventArgs e) {
            typing.FormConfig.GenMode = (byte)comboGenMode.SelectedIndex;
        }
        private void checkMetronome_Click(object sender, EventArgs e) {
            if (checkMetronome.Checked) {
                trackMetronomeDelay.Enabled = true;
                typing.FormConfig.MetronomeEnabled = true;
                typing.FormConfig.MetronomePeriod = trackMetronomeDelay.Value;
            } else {
                trackMetronomeDelay.Enabled = false;
                typing.FormConfig.MetronomeEnabled = false;
            }
        }
        private void trackMetronomeDelay_ValueChanged(object sender, EventArgs e) {
            lblMetronomeDelay.Text = trackMetronomeDelay.Value.ToString();
            typing.FormConfig.MetronomePeriod = trackMetronomeDelay.Value;
        }
        #endregion

        private void CustomCollectionCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            typing.FormConfig.CustomCollection = CustomCollectionCombo.Text;
        }
    }
}

