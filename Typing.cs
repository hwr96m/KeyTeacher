using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Media;
using System.Text.RegularExpressions;
using System.Linq;
using System.Diagnostics;

namespace KeyTeacher {
    class Typing {
        #region ---- КОНСТАНТЫ ------------------------------
        readonly string RuWords_FILE = $"{Environment.CurrentDirectory}\\dictionary\\ru.txt";
        readonly string EnWords_FILE = $"{Environment.CurrentDirectory}\\dictionary\\en.txt";
        readonly string BEEP_FILE = $"{Environment.CurrentDirectory}\\beep_3.wav";
        readonly string TICK_FILE = $"{Environment.CurrentDirectory}\\metronome.wav";
        const string alphabetRU = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";//абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ0123456789
        const string alphabetEN = "abcdefghijklmnopqrstuvwxyz";
        const string numbers = "0123456789";
        const string punctuation = ",.;:?!\"'[]{}()<>@#$%^&*-+=_|\\/~`";
        readonly Color DefaultColor = Color.White;
        readonly Color SuccessColor = Color.LightGreen;
        readonly Color FailColor = Color.LightPink;
        const int ShortTextShadowLen = 10;
        const int ShortTextMaxLen = 20;
        const int MaxPressDelay = 1000;     //максимальное время нажатия клавиши
        const int MinPressDelay = 50;     //минимальное время нажатия клавиши
        #endregion
        #region ---- ПЕРЕМЕННЫЕ --------------------------------
        string Words;
        int ShortTextCurrentSymbol = 0;
        List<ShortText_t> ShortText;                      //бегущая строка с печатаемым текстом
        public Config_t config;                                //конфиг
        Random random = new Random();
        Symbol_t[] Symbols;
        RichTextBox ShortTextFormObj;   //элементы формы
        RichTextBox FullTextFormObj;
        TabPage StatisticFormObj;
        Button StartButton;
        Stopwatch timer = new Stopwatch();
        public Metronome metronome;
        #endregion
        #region ---- ФЛАГИ -----------------------------
        bool CurrentSymbolIsHandled = false;        //текущий символ уже обработан
        bool TypingStart = false;         //флаг старта процесса печати

        #endregion
        #region ---- СТРУКТУРЫ   ------------------------------
        public struct Symbol_t {
            public char Name;         //символ
            public int SuccessPress;    //счётчик успешных нажатий
            public int FailPress;       //счетчик ошибок
            public List<int> PressDelay;      //время затраченное на нажатие клавиши
            public Label Lbl;           //лабел для вывода информации
            public ProgressBar PBar;     //статистика успешных нажатий
        }
        public struct ShortText_t {
            public char Symbol;
            public Color Color;
        }
        public struct Config_t {
            public bool WaitSuccessPress;    //ждать успешное нажатие
            public bool EN;
            public bool RU;
            public bool PunctuationMarks;    //занки препинания
            public bool Numbers;     //числа
            public bool Upper;       //заглавные
            public byte GenMode;    //режим генерации
            public bool MetronomeEnabled;
            public int MetronomePeriod;
        }
        #endregion
        public Typing(RichTextBox ShortTextFormObj, RichTextBox FullTextFormObj, TabPage StatisticFormObj, Button StartButton) {
            this.ShortTextFormObj = ShortTextFormObj;
            this.FullTextFormObj = FullTextFormObj;
            this.StatisticFormObj = StatisticFormObj;
            this.StartButton = StartButton;
            config = new Config_t { RU = true };
            Init_Symbols(out Symbols);
            Init_StatisticForm();
            metronome = new Metronome(ref config.MetronomeEnabled, ref config.MetronomePeriod);
        }
        public void KeyPressHandler(char key)//обработчик нажатий на кнопки
        {
            if (TypingStart) {
                if (key == GetCurrentSymbol() && ((!metronome.Early && !metronome.Late) || !metronome.isStarted)) {
                    AddSuccessPress(key);
                } else {
                    AddFailPress(key);
                }
            }
        }
        public void Start()//запустить процесс печати
        {
            TypingStart = true;
            Init_Symbols(out Symbols);
            Init_Dictionary();
            Init_ShortText();
            ShortTextShow();
            Init_StatisticForm();
            if (metronome.Enabled) {
                metronome.Start();
            }
        }
        public void Stop()//остановить процесс печати
        {
            TypingStart = false;
            metronome.Stop();
        }
        void AddSuccessPress(char symbol)//обновляет переменные при успешном нажатии
        {
            if (!CurrentSymbolIsHandled)//текущий символ еще не обработан
            {
                for (int i = 0; i < Symbols.Length; i++)    //ищем нужный символ, прибавляем к счётчику
                {
                    if (Symbols[i].Name == symbol) {
                        Symbols[i].SuccessPress++;
                        timer.Stop();
                        PressDelayAppend(Symbols[i], (int)timer.ElapsedMilliseconds);
                        break;
                    }
                }
                UpdateStatistic();
                ShortTextCurrentSymbolSetColor(SuccessColor);
                FullTextAddCurrentSymbol();
            }
            CurrentSymbolIsHandled = true;
            ShortTextShift();
            ShortTextAdd();
            ShortTextShow();
        }
        void AddFailPress(char symbol)//обновляет переменные при неверном нажатии
        {
            if (!CurrentSymbolIsHandled)    //текущий символ еще не обработан
            {
                for (int i = 0; i < Symbols.Length; i++) {
                    if (Symbols[i].Name == symbol) {
                        Symbols[i].FailPress++;
                        PressDelayAppend(Symbols[i], MaxPressDelay);
                        break;
                    }
                }
                UpdateStatistic();
                ShortTextCurrentSymbolSetColor(FailColor);
                FullTextAddCurrentSymbol();
            }
            CurrentSymbolIsHandled = true;
            if (!config.WaitSuccessPress)    //если в конфиге задано не ждать правильное нажатие
            {
                ShortTextShift();
                ShortTextShow();
            }
            Beep();
        }
        void Init_StatisticForm()//создаёт элементы отображения статистики на форме
        {
            var s = Symbols;
            for (int i = 0; i < s.Length; i++) {
                //-- Label -----------
                s[i].Lbl = new Label();
                s[i].Lbl.Font = new Font("Arial", 8);
                s[i].Lbl.Location = new Point(10, 22 * i);
                s[i].Lbl.Size = new Size(30, 20);
                s[i].Lbl.Margin = new Padding(0);
                s[i].Lbl.Text = $"{s[i].Name}:";

                //-- ProgressBar -----------
                s[i].PBar = new ProgressBar();
                s[i].PBar.Location = new Point(40, 22 * i);
                s[i].PBar.Size = new Size(50, 20);
                s[i].PBar.Margin = new Padding(0);
                s[i].PBar.Maximum = MaxPressDelay;
                s[i].PBar.Minimum = MinPressDelay;
                s[i].PBar.Value = MinPressDelay;
                //--- записываем в Symbols -----------------
                StatisticFormObj.Controls.Add(s[i].Lbl);
                StatisticFormObj.Controls.Add(s[i].PBar);
            }
        }
        void UpdateStatistic() //обновление статистики
        {
            for (int i = 0; i < Symbols.Length; i++) {
                //Symbols[i].PBar.Value = (Symbols[i].FailPress * 100 + 1) / (Symbols[i].SuccessPress + Symbols[i].FailPress + 1);
                Symbols[i].PBar.Value = Between((int)Symbols[i].PressDelay.Average(), MinPressDelay, MaxPressDelay);//усреднённое время нескольких последних нажатий
            }
        }
        string GetWord() {
            int index = 0;
            int count = 0;
            int rand;
            string word;
            //--- выбираем нужный символ с учётом статистики ошибок ----------
            rand = random.Next(Symbols.Sum(x => (int)x.PressDelay.Average()));
            for (int i = 0; i < Symbols.Length; i++) {
                if (count > rand)
                    break;
                count += (int)Symbols[i].PressDelay.Average();
                index = i;
            }
            //--- ищем в словаре слово с нужным символом -------
            if (Char.IsLetter(Symbols[index].Name)) {
                var MatchWords = Regex.Matches(Words, $"\\w*{Symbols[index].Name.ToString().ToLower()}+\\w*");  //
                if (MatchWords.Count > 0)
                    word = MatchWords[random.Next(MatchWords.Count)].Value;
                else
                    word = Symbols[index].Name.ToString();
            } else {
                word = Symbols[index].Name.ToString();
            }
            //--- приводим к соответствующему регистру -------
            if (Char.IsUpper(Symbols[index].Name))
                return word.ToUpper();
            else
                return word;
        }
        string GetSymbol() {
            int index = 0;
            int count = 0;
            int rand;
            //--- выбираем нужный символ с учётом статистики ошибок ----------
            rand = random.Next(Symbols.Sum(x => (int)x.PressDelay.Average()));
            for (int i = 0; i < Symbols.Length; i++) {
                if (count > rand)
                    break;
                count += (int)Symbols[i].PressDelay.Average();
                index = i;
            }
            return Symbols[index].Name.ToString();

        }
        void Init_Dictionary() {
            if (config.RU)
                Words += File.ReadAllText(RuWords_FILE).Replace(" ", "") + ",";
            if (config.EN)
                Words += File.ReadAllText(EnWords_FILE).Replace(" ", "") + ",";
        }
        #region --- Symbols ----------
        void FullTextAddCurrentSymbol()   //добавляет строку в FullTextFormObj
        {
            var str = ShortText[ShortTextCurrentSymbol].Symbol.ToString();
            var color = ShortText[ShortTextCurrentSymbol].Color;
            var obj = FullTextFormObj;
            obj.AppendText(str);
            obj.SelectionStart = obj.TextLength - str.Length;
            obj.SelectionLength = str.Length;
            obj.SelectionBackColor = color;
        }
        char GetCurrentSymbol() {
            return ShortText[ShortTextCurrentSymbol].Symbol;
        }
        void Init_Symbols(out Symbol_t[] k) {
            string s = "";
            if (config.RU) {
                s += alphabetRU;
                if (config.Upper) {
                    s += alphabetRU.ToUpper();
                }

            }
            if (config.EN) {
                s += alphabetEN;
                if (config.Upper) {
                    s += alphabetEN.ToUpper();
                }

            }
            if (config.Numbers)
                s += numbers;
            if (config.PunctuationMarks)
                s += punctuation;


            k = new Symbol_t[s.Length];
            for (int i = 0; i < s.Length; i++) {
                k[i].Name = s[i];
                k[i].SuccessPress = 1;
                k[i].FailPress = 1;
                k[i].PressDelay = Enumerable.Repeat(MinPressDelay, 10).ToList();
            }
        }
        void PressDelayAppend(Symbol_t s, int delay)//добавляет delay в Symbols
        {
            var item = s.PressDelay;
            for (int i = 1; i < item.Count; i++) {
                item[i - 1] = item[i];
            }
            item[^1] = Between(delay, MinPressDelay, MaxPressDelay);
        }

        #endregion
        #region ---- ShortText ----------------------
        void Init_ShortText() {
            ShortText = new List<ShortText_t>(ShortTextMaxLen * 2);
            for (int i = 0; i < ShortTextShadowLen; i++)        //добавляем пробелы в начале строки
            {
                ShortTextAdd(" ");
            }
            while (ShortText.Count < ShortTextMaxLen) {
                ShortTextAdd();
            }
            ShortTextCurrentSymbol = ShortTextShadowLen;
        }
        void ShortTextAdd() //добавляет текст в переменную ShortText
        {
            if (ShortText.Count < ShortTextMaxLen) {
                var item = new ShortText_t { };
                string str = "";
                if (config.GenMode == 0) {
                    str = GetWord() + " ";
                }
                if (config.GenMode == 1) {
                    str = GetSymbol();
                }
                for (int i = 0; i < str.Length; i++) {
                    item.Symbol = str[i];
                    item.Color = DefaultColor;
                    ShortText.Add(item);
                }
            }
        }
        void ShortTextAdd(string str) //добавляет текст в переменную ShortText
        {
            if (ShortText.Count < ShortTextMaxLen) {
                var item = new ShortText_t { };
                for (int i = 0; i < str.Length; i++) {
                    item.Symbol = str[i];
                    item.Color = DefaultColor;
                    ShortText.Add(item);
                }
            }
        }
        void ShortTextCurrentSymbolSetColor(Color color) {
            ShortText[ShortTextCurrentSymbol] = new ShortText_t { Symbol = ShortText[ShortTextCurrentSymbol].Symbol, Color = color };
        }
        void ShortTextShift() {
            if (ShortTextCurrentSymbol >= ShortTextShadowLen)//если тень стремится за пределы допустимого значения, то удаляем 0 элемент
            {
                ShortText.RemoveAt(0);
            } else//если не, то передвигаем ShortTextCurrentSymbol
              {
                ShortTextCurrentSymbol++;
            }
            CurrentSymbolIsHandled = false;     //текущий символ обновлен - сбрасываем флаг
            ShortTextAdd();
            timer.Reset();
            timer.Start();
        }
        void ShortTextShow()//печатаем ShortText в текстбокс
        {
            var obj = ShortTextFormObj;
            obj.Clear();
            for (int i = 0; i < ShortText.Count && i < ShortTextMaxLen; i++) {
                obj.AppendText(ShortText[i].Symbol.ToString());
                obj.SelectionStart = obj.TextLength - 1;
                obj.SelectionLength = 1;
                obj.SelectionBackColor = ShortText[i].Color;
            }
        }
        #endregion
        #region --- ДОПОЛНИТЕЛЬНО ------------------
        void Beep() {
            var beep = new SoundPlayer(BEEP_FILE);
            beep.Play();
            Thread.Sleep(100);
        }
        int Between(int x, int min, int max)//возвращает значение ограниченное диапазоном
        {
            if (x < min)
                return min;
            if (x > max)
                return max;
            return x;
        }
        #endregion
    }

    class Metronome {
        #region ---- ПЕРЕМЕННЫЕ --------------------------------
        readonly string TICK_FILE = $"{Environment.CurrentDirectory}\\metronome.wav";
        int diff = 150;
        public bool Early { get; private set; }
        public bool Late { get; private set; }
        public bool isStarted { get; private set; }
        public int Period;
        public bool Enabled;

        #endregion
        public Metronome(ref bool enabled, ref int period) {
            Period = period;
            Enabled = enabled;
        }
        public void Start() {
            if (Enabled) {
                isStarted = true;
                Task.Run(() => {
                    while (isStarted) {
                        Early = true;
                        Late = false;
                        Thread.Sleep(Period / 2 - diff);
                        Early = false;
                        Thread.Sleep(diff);
                        Tick();
                        Thread.Sleep(diff);
                        Late = true;
                        Thread.Sleep(Period / 2 - diff);
                    }
                    Early = false;
                    Late = false;
                });
            }
        }
        public void Stop() {
            isStarted = false;
            Early = false;
            Late = false;
        }
        void Tick() {
            var beep = new SoundPlayer(TICK_FILE);
            beep.Play();
            Thread.Sleep(33);
        }
    }
}
