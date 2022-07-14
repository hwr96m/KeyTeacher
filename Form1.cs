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

namespace KeyTeacher {
    public partial class Form1 : Form {
        Typing typing;
        void SetConfig() {
            typing.FormConfig.tShortText = tboxShortText;
            typing.FormConfig.tFullText = tboxFullText;
            typing.FormConfig.tabStatistic = tabStatistic;
            typing.FormConfig.tabConfig = tabConfig;
            typing.FormConfig.bStartStop = bStartStop;

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
            comboGenMode.SelectedIndex = 0;
            SetConfig();
        }
        void Form1_KeyPress(object sender, KeyPressEventArgs e) {
            typing.KeyPressHandler(e.KeyChar);
        }
        private void bStartStop_Click(object sender, EventArgs e) {
            if (bStartStop.Text == "Start") {
                bStartStop.Text = "Stop";
                typing.Start();
                SetConfig();
                return;
            }
            if (bStartStop.Text == "Stop") {
                bStartStop.Text = "Start";
                typing.Stop();
                return;
            }
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

    }
}

/*
 Task.Run(() =>
             {
                while (StartFlag)
                {
                     char CurSymbol = typing.GetCurrentSymbol();
                    if (key != (char)0 && key == CurSymbol)
                    {
                        this.Invoke(new Action(() =>
                        {
                            AddText(tboxFullText, CurSymbol.ToString(), typing.SuccessColor);
                            typing.AddSuccessPress(tboxShortText, CurSymbol);
                        }));
                        key = (char)0;
                    }
                    if (key != (char)0 && key != CurSymbol)
                    {
                        this.Invoke(new Action(() =>
                        {
                            AddText(tboxFullText, CurSymbol.ToString(), typing.FailColor);
                            typing.AddFailPress(tboxShortText, CurSymbol);
                        }));
                        key = (char)0;
                    }
                    Thread.Sleep(50);
                }
            });

*/
