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
            typing.config.Upper = confUpper.Checked;
            typing.config.Numbers = confNumbers.Checked;
            typing.config.PunctuationMarks = confPunctuationMarks.Checked;
            typing.config.RU = confRU.Checked;
            typing.config.EN = confEN.Checked;
            typing.config.WaitSuccessPress = confWaitSuccessPress.Checked;
            typing.config.GenMode = (byte)comboGenMode.SelectedIndex;
            typing.metronome.Enabled = checkMetronome.Checked;
            typing.metronome.Period = trackMetronomeDelay.Value;
        }
        public Form1() {
            InitializeComponent();
            typing = new Typing(tboxShortText, tboxFullText, tabStatistic, bStartStop);
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
            typing.config.Upper = confUpper.Checked;
        }
        private void confNumbers_Click(object sender, EventArgs e) {
            typing.config.Numbers = confNumbers.Checked;
        }
        private void confPunctuationMarks_Click(object sender, EventArgs e) {
            typing.config.PunctuationMarks = confPunctuationMarks.Checked;
        }
        private void confWaitSuccessPress_Click(object sender, EventArgs e) {
            typing.config.WaitSuccessPress = confWaitSuccessPress.Checked;
        }
        private void confRU_Click(object sender, EventArgs e) {
            typing.config.RU = confRU.Checked;
            if (!confRU.Checked && !confEN.Checked) {
                confEN.Checked = true;
                typing.config.EN = true;
            }
        }
        private void confEN_Click(object sender, EventArgs e) {
            typing.config.EN = confEN.Checked;
            if (!confRU.Checked && !confEN.Checked) {
                confRU.Checked = true;
                typing.config.RU = true;
            }

        }
        private void comboGenMode_SelectedIndexChanged(object sender, EventArgs e) {
            typing.config.GenMode = (byte)comboGenMode.SelectedIndex;
        }
        private void checkMetronome_Click(object sender, EventArgs e) {
            if (checkMetronome.Checked) {
                trackMetronomeDelay.Enabled = true;
                typing.metronome.Enabled = true;
                typing.metronome.Period = trackMetronomeDelay.Value;
            } else {
                trackMetronomeDelay.Enabled = false;
                typing.metronome.Enabled = false;
            }
        }
        private void trackMetronomeDelay_ValueChanged(object sender, EventArgs e) {
            lblMetronomeDelay.Text = trackMetronomeDelay.Value.ToString();
            typing.config.MetronomePeriod = trackMetronomeDelay.Value;
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
