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
    public class Typing {
        #region ---- Struct ---------------------------------------
        public struct FormConfig_t {
            public RichTextBox tShortText;
            public RichTextBox tFullText;
            public TabPage tabStatistic;
            public TabPage tabConfig;
            public Button bStartStop;
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
        #endregion -------------------------------------------------
        #region ---- ПЕРЕМЕННЫЕ ------------------------------
        const int MaxPressDelay = 1000;     //максимальное время нажатия клавиши
        const int MinPressDelay = 50;     //минимальное время нажатия клавиши
        public FormConfig_t FormConfig;        //конфиграция настроек из формы
        Symbols symbols;
        Generator generator;
        Metronome metronome;
        Printer printer;
        bool TypingStart = false;         //флаг старта процесса печати
        #endregion

        public Typing() {
            FormConfig = new FormConfig_t { RU = true };
        }
        public void KeyPressHandler(char key)//обработчик нажатий на кнопки
        {
            if (TypingStart) {
                printer.KeyPress(key);
            }
        }
        public void Start()//запустить процесс печати
        {
            TypingStart = true;
            symbols = new Symbols(FormConfig, MaxPressDelay, MinPressDelay);
            generator = new Generator(FormConfig, symbols);
            metronome = new Metronome(FormConfig);
            printer = new Printer(FormConfig, generator, metronome, symbols, MaxPressDelay);
            if (metronome.Enabled) {
                metronome.Start();
            }
        }
        public void Stop()//остановить процесс печати
        {
            TypingStart = false;
            metronome.Stop();
        }
    }
    public class Metronome {
        #region ---- ПЕРЕМЕННЫЕ --------------------------------
        int diff = 150;
        public bool Early { get; private set; }
        public bool Late { get; private set; }
        public bool isStarted { get; private set; }
        public int Period;
        public bool Enabled;
        #endregion --------------------------------------------
        //------ Функции ---------------------------------------
        public Metronome(Typing.FormConfig_t config) {
            Period = config.MetronomePeriod;
            Enabled = config.MetronomeEnabled;
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
                        Sound.Tick();
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
        //-----------------------------------------------------
    }
    public class Printer {
        #region ---- Данные --------------------------------
        //--- Переменные ---
        readonly Color DefaultColor = Color.White;
        readonly Color SuccessColor = Color.LightGreen;
        readonly Color FailColor = Color.LightPink;
        const int ShadowLen = 10;
        const int MaxLen = 20;
        Typing.FormConfig_t config;
        Generator generator;
        Metronome metronome;
        Symbols symbols;
        int MaxPressDelay;
        List<Text_t> Text;    //бегущая строка с печатаемым текстом
        int CurrentSymbol = 0;
        public bool CurrentSymbolIsHandled = false;        //текущий символ уже обработан
        Stopwatch timer = new Stopwatch();

        //--- Структуры ----
        struct Text_t {
            public char Symbol;
            public Color Color;
        }
        #endregion --------------------------------------------

        //------ Функции ---------------------------------------
        public Printer(Typing.FormConfig_t config, Generator generator, Metronome metronome, Symbols symbols, int MaxPressDelay) {
            this.config = config;
            this.generator = generator;
            this.metronome = metronome;
            this.symbols = symbols;
            this.MaxPressDelay = MaxPressDelay;
            TextInit();
            Show();
            timer.Restart();
        }
        void TextInit() {
            Text = new List<Text_t>(MaxLen * 2);
            for (int i = 0; i < ShadowLen; i++)        //добавляем пробелы в начале строки
            {
                AddText(" ");
            }
            while (Text.Count < MaxLen) {
                AddText();
            }
            CurrentSymbol = ShadowLen;
        }
        void AddText() //добавляет текст в переменную Text
        {
            if (Text.Count < MaxLen) {
                AddText(generator.GetWord());
            }
        }
        void AddText(string str) //добавляет текст в переменную Text
        {
            if (Text.Count < MaxLen) {
                var item = new Text_t { };
                for (int i = 0; i < str.Length; i++) {
                    item.Symbol = str[i];
                    item.Color = DefaultColor;
                    Text.Add(item);
                }
            }
        }
        public void KeyPress(char key) {
            var symbol = GetCurrentSymbol();
            if (key == symbol && ((!metronome.Early && !metronome.Late) || !metronome.isStarted)) {
                SuccessPress(symbol);
            } else {
                FailPress(symbol);
            }
        }
        void SuccessPress(char symbol) {
            if (!CurrentSymbolIsHandled)//текущий символ еще не обработан
            {
                symbols.AddSuccessPress(symbol, (int)timer.ElapsedMilliseconds);
                symbols.StatisticUpdate();
                CurrentSymbolSetColor(SuccessColor);
                FullTextAddSymbol(Text[CurrentSymbol]);
            }
            timer.Restart();
            CurrentSymbolIsHandled = true;
            Shift();
            AddText();
            Show();
        }
        void FailPress(char symbol) {
            if (!CurrentSymbolIsHandled)    //текущий символ еще не обработан
            {
                symbols.AddFailPress(symbol);
                symbols.StatisticUpdate();
                CurrentSymbolSetColor(FailColor);
                FullTextAddSymbol(Text[CurrentSymbol]);
            }
            Sound.Beep();
            CurrentSymbolIsHandled = true;
            if (!config.WaitSuccessPress)    //если в конфиге задано не ждать правильное нажатие
            {
                timer.Restart();
                Shift();
                Show();
            }
        }
        void CurrentSymbolSetColor(Color color) {
            Text[CurrentSymbol] = new Text_t { Symbol = Text[CurrentSymbol].Symbol, Color = color };
        }
        void Shift() {
            if (CurrentSymbol >= ShadowLen)//если тень стремится за пределы допустимого значения, то удаляем 0 элемент
            {
                Text.RemoveAt(0);
            } else//если не, то передвигаем ShortTextCurrentSymbol
              {
                CurrentSymbol++;
            }
            CurrentSymbolIsHandled = false;     //текущий символ обновлен - сбрасываем флаг
            AddText();
        }
        char GetCurrentSymbol() {
            return Text[CurrentSymbol].Symbol;
        }
        void FormObjAppendColorText(RichTextBox obj, Text_t symbol) {
            obj.AppendText(symbol.Symbol.ToString());
            obj.SelectionStart = obj.TextLength - 1;
            obj.SelectionLength = 1;
            obj.SelectionBackColor = symbol.Color;
        }
        void Show()//печатаем Text в текстбокс
        {
            config.tShortText.Clear();
            for (int i = 0; i < Text.Count && i < MaxLen; i++) {
                FormObjAppendColorText(config.tShortText, Text[i]);
            }
        }
        void FullTextAddSymbol(Text_t symbol) {
            FormObjAppendColorText(config.tFullText, symbol);
        }
        //-----------------------------------------------------
    }
    public class Generator {
        #region --- Данные -------------------------------------------------------------------
        readonly string RuWords_FILE = $"{Environment.CurrentDirectory}\\dictionary\\ru.txt";
        readonly string EnWords_FILE = $"{Environment.CurrentDirectory}\\dictionary\\en.txt";
        string Words;
        Random random = new Random();
        Symbols symbols;
        Typing.FormConfig_t config;            
        #endregion ---------------------------------------------------------------------------
        public Generator(Typing.FormConfig_t config, Symbols symbols) {
            this.symbols = symbols;
            this.config = config;
            Init_Dictionary();
        }
        void Init_Dictionary() {
            if (config.RU)
                Words += File.ReadAllText(RuWords_FILE).Replace(" ", "") + ",";
            if (config.EN)
                Words += File.ReadAllText(EnWords_FILE).Replace(" ", "") + ",";
        }
        public string GetWord() {
            switch (config.GenMode) {
                case 0: //режим Word
                    return GetWordByStatistic()+" ";
                case 1: //режим Symbol
                    return GetSymbolByStatistic().ToString();
                default:
                    return GetSymbolByStatistic().ToString();
            }
        }
        string GetWordByStatistic() {
            string word;
            var s = symbols.SymbolList;
            var Sym = GetSymbolByStatistic();   //выбираем нужный символ с учётом статистики ошибок
                                                //--- ищем в словаре слово с нужным символом -------
            if (Char.IsLetter(Sym)) {
                var MatchWords = Regex.Matches(Words, $"\\w*{Sym.ToString().ToLower()}+\\w*");  //
                if (MatchWords.Count > 0)
                    word = MatchWords[random.Next(MatchWords.Count)].Value;
                else
                    word = Sym.ToString();
            } else {
                word = Sym.ToString();
            }
            //--- приводим к соответствующему регистру -------
            if (Char.IsUpper(Sym))
                return word.ToUpper();
            else
                return word;
        }
        char GetSymbolByStatistic() {
            int index = 0;
            int count = 0;
            int rand;
            var s = symbols.SymbolList;
            rand = random.Next(s.Sum(x => (int)x.PressDelay.Average()));
            for (int i = 0; i < s.Length; i++) {
                if (count > rand)
                    break;
                count += (int)s[i].PressDelay.Average();
                index = i;
            }
            return s[index].Name;
        }
    }
    public class Symbols {
        #region ---- ПЕРЕМЕННЫЕ --------------------------------
        public struct Symbol_t {
            public char Name;         //символ
            public int SuccessPress;    //счётчик успешных нажатий
            public int FailPress;       //счетчик ошибок
            public List<int> PressDelay;      //время затраченное на нажатие клавиши
            public Label Lbl;           //лабел для вывода информации
            public ProgressBar PBar;     //статистика успешных нажатий
        }
        const string alphabetRU = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";//абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ0123456789
        const string alphabetEN = "abcdefghijklmnopqrstuvwxyz";
        const string numbers = "0123456789";
        const string punctuation = ",.;:?!\"'[]{}()<>@#$%^&*-+=_|\\/~`";
        Typing.FormConfig_t config;
        int MaxPressDelay;
        int MinPressDelay;
        public Symbol_t[] SymbolList;
        #endregion ---------------------------------------------

        public Symbols(Typing.FormConfig_t config, int MaxPressDelay, int MinPressDelay) {
            this.config = config;
            this.MaxPressDelay = MaxPressDelay;
            this.MinPressDelay = MinPressDelay;
            SymbolListInit();
            FormInit();
        }
        void SymbolListInit() {
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
            SymbolList = new Symbol_t[s.Length];
            for (int i = 0; i < s.Length; i++) {
                SymbolList[i].Name = s[i];
                SymbolList[i].SuccessPress = 1;
                SymbolList[i].FailPress = 1;
                SymbolList[i].PressDelay = Enumerable.Repeat(MinPressDelay, 10).ToList();
            }

        }
        void FormInit() {
            config.tabStatistic.Controls.Clear();
            var s = SymbolList;
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
                config.tabStatistic.Controls.Add(s[i].Lbl);
                config.tabStatistic.Controls.Add(s[i].PBar);
            }
        }
        public void StatisticUpdate() //обновление статистики
         {
            for (int i = 0; i < SymbolList.Length; i++) {
                //Symbols[i].PBar.Value = (SymbolList[i].FailPress * 100 + 1) / (SymbolList[i].SuccessPress + Symbols[i].FailPress + 1);
                SymbolList[i].PBar.Value = StatisticCalc(SymbolList[i]);  //усреднённое время нескольких последних нажатий
            }
        }
        public void AddSuccessPress(char symbol, int delay) {
            var s = GetSymbolByName(symbol);
            if (s.Name == symbol) {
                s.SuccessPress++;
                SetPressDelay(symbol, delay);
            }
        }
        public void AddFailPress(char symbol) {
            var s = GetSymbolByName(symbol);
            if (s.Name == symbol) {
                s.FailPress++;
                SetPressDelay(symbol, MaxPressDelay);
            }
        }
        int StatisticCalc(Symbol_t symbol) {
            return Between((int)symbol.PressDelay.Average(), MinPressDelay, MaxPressDelay);
        }
        public void SetPressDelay(char symbol, int delay)//добавляет delay в Symbols        
        {
            var s = GetSymbolByName(symbol);
            if (s.Name == symbol) {
                var item = s.PressDelay;
                for (int i = 1; i < item.Count; i++) {
                    item[i - 1] = item[i];
                }
                item[^1] = Between(delay, MinPressDelay, MaxPressDelay);
            }
        }
        Symbol_t GetSymbolByName(char symbol) {
            for (int i = 0; i < SymbolList.Length; i++)    //ищем нужный символ, прибавляем к счётчику
                        {
                if (SymbolList[i].Name == symbol) {
                    return SymbolList[i];
                }
            }
            return new Symbol_t();
        }
        int Between(int x, int min, int max)//возвращает значение ограниченное диапазоном
        {
            if (x < min)
                return min;
            if (x > max)
                return max;
            return x;
        }
    }
    static class Sound {
        static readonly string TICK_FILE = $"{Environment.CurrentDirectory}\\metronome.wav";
        static readonly string BEEP_FILE = $"{Environment.CurrentDirectory}\\beep_3.wav";
        public static void Beep() {
            var beep = new SoundPlayer(BEEP_FILE);
            beep.Play();
            Thread.Sleep(100);
        }
        public static void Tick() {
            var beep = new SoundPlayer(TICK_FILE);
            beep.Play();
            Thread.Sleep(33);
        }
    }
}
