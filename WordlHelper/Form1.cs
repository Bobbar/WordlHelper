using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace WordlHelper
{
    public partial class Form1 : Form
    {
        private const int NUM_GUESSES = 6;

        private List<string> _allWords;
        private List<string> _possibleWords;
        private GuessBoxes[] _guessBoxes = new GuessBoxes[NUM_GUESSES];
        private string _selectedWord = string.Empty;
        private bool _isPlaying = false;
        private bool _debug = true;

        public Form1()
        {
            InitializeComponent();
            InitGuessBoxes();
            InitWords();
        }

        private void StartPlay()
        {
            Reset();
            _isPlaying = true;

            var rnd = new Random();
            _selectedWord = _allWords[rnd.Next(_allWords.Count)];

            selectedWordLabel.Text = $"Word: {_selectedWord}";
            wordSelectedLabel.Visible = true;
        }

        private void SetBoxStates(bool updateUI = true)
        {
            if (!_isPlaying)
                return;

            foreach (var boxes in _guessBoxes)
            {
                if (boxes.IsValid)
                {
                    for (int i = 0; i < boxes.Word.Length; i++)
                    {
                        var boxLetter = boxes.Word[i].ToString();
                        var selectLetter = _selectedWord[i].ToString();

                        if (selectLetter == boxLetter)
                        {
                            boxes.Boxes[i].State = GuessState.CorrectPosition;
                        }
                        else if (selectLetter != boxLetter && _selectedWord.Contains(boxLetter))
                        {
                            boxes.Boxes[i].State = GuessState.IncorrectPosition;
                        }
                        else
                        {
                            boxes.Boxes[i].State = GuessState.NotInWord;
                        }
                    }
                }

                if (updateUI)
                    boxes.SetColors();
            }
        }

        private void AnalyzeGuesses(List<string> allwords, bool updateUI = true)
        {
            var incorrectPos = new string[5];
            var correctPos = new string[5];
            var notInWord = new HashSet<string>();

            var boxWords = new List<string>();

            int guesses = 0;

            foreach (var boxes in _guessBoxes)
            {
                boxWords.Add(boxes.Word);

                if (boxes.IsValid)
                    guesses++;

                for (int i = 0; i < boxes.Word.Length; i++)
                {
                    var state = boxes.Boxes[i];
                    var boxLetter = boxes.Word[i].ToString();

                    if (!string.IsNullOrEmpty(boxLetter))
                    {
                        switch (state.State)
                        {
                            case GuessState.CorrectPosition:
                                correctPos[i] = boxLetter;
                                break;
                            case GuessState.IncorrectPosition:
                                incorrectPos[i] = boxLetter;
                                break;
                            case GuessState.NotInWord:
                                if (!correctPos.Contains(boxLetter))
                                    notInWord.Add(boxLetter);
                                break;
                        }
                    }
                }
            }

            notInWord = notInWord.Except(correctPos.ToList()).ToHashSet();

            _possibleWords = GetPossibleWords(notInWord, incorrectPos, correctPos, _allWords.ToList());
            _possibleWords = _possibleWords.Except(boxWords).ToList();

            if (updateUI)
                UpdatePossibleList();
        }

        private void UpdatePossibleList()
        {
            matchesCountLabel.Text = $"Count: {_possibleWords.Count}";

            possibleWordsBox.SuspendLayout();
            possibleWordsBox.Clear();

            var sb = new StringBuilder();
            foreach (var word in _possibleWords)
            {
                sb.AppendLine(word);
            }

            possibleWordsBox.Text = sb.ToString();

            possibleWordsBox.ResumeLayout();
        }

        private static List<string> GetPossibleWords(HashSet<string> notInWord, string[] incorrectPos, string[] correctPos, List<string> allWords)
        {
            var possibles = allWords.ToList();
            var removes = new HashSet<string>();

            // First filter words by known correct letters.
            var mustContain = incorrectPos.Where(i => i != null).ToList();
            mustContain.AddRange(correctPos.Where(c => c != null).ToList());
            possibles = possibles.FindAll(w => mustContain.All(m => w.Contains(m)));

            // Remove words without letter in correct position.
            for (int i = 0; i < correctPos.Length; i++)
            {
                if (correctPos[i] != null)
                {
                    for (int w = 0; w < possibles.Count; w++)
                    {
                        var word = possibles[w];
                        if (word[i].ToString() != correctPos[i])
                            removes.Add(word);
                    }
                }
            }

            // Remove words with letters not in the answer.
            foreach (var noMatch in notInWord)
            {
                for (int w = 0; w < possibles.Count; w++)
                {
                    var word = possibles[w];

                    if (word.Contains(noMatch))
                        removes.Add(word);
                }
            }

            // Remove words with letters in known wrong positions.
            for (int w = 0; w < possibles.Count; w++)
            {
                var word = possibles[w];

                for (int i = 0; i < incorrectPos.Length; i++)
                {
                    if (word[i].ToString() == incorrectPos[i])
                        removes.Add(word);
                }
            }

            possibles = possibles.Except(removes).ToList();

            return possibles;
        }

        private void InitGuessBoxes()
        {
            for (int i = 0; i < NUM_GUESSES; i++)
            {
                var guessBox = new GuessBoxes(i);
                guessBox.EnterKeyPressed += GuessBox_EnterKeyPressed;
                guessBoxesPanel.Controls.Add(guessBox, 0, i);
                _guessBoxes[i] = guessBox;
            }
        }

        private void InitWords()
        {
            var inputPath = $@"{Environment.CurrentDirectory}\words.txt";
            var inputText = File.ReadAllLines(inputPath).ToList();
            var cleaned = Regex.Replace(inputText.First(), @"\s+", "");
            cleaned = cleaned.Replace("\"", "");
            var words = cleaned.Split(',');

            for (int i = 0; i < words.Length; i++)
            {
                words[i] = words[i].ToUpper();
            }

            _allWords = words.ToList();
            _possibleWords = words.ToList();
        }

        private void SaveState()
        {
            using (var saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "CSV | *.csv";
                saveDialog.Title = "Save State";
                var res = saveDialog.ShowDialog();

                if (res == DialogResult.OK)
                {
                    var path = saveDialog.FileName;
                    string output = string.Empty;

                    output += "line,boxIdx,state,value\n";
                    for (int i = 0; i < _guessBoxes.Length; i++)
                    {
                        var boxes = _guessBoxes[i];
                        for (int j = 0; j < boxes.Boxes.Length; j++)
                        {
                            var box = boxes.Boxes[j];
                            output += $"{i},{j},{(int)box.State},{box.Box.Text}\n";
                        }
                    }

                    File.WriteAllText(path, output);
                }
            }
        }

        private void LoadState()
        {
            using (var openDialog = new OpenFileDialog())
            {
                openDialog.Filter = "CSV | *.csv";
                openDialog.Title = "Load State";
                var res = openDialog.ShowDialog();

                if (res == DialogResult.OK)
                {
                    var stateData = File.ReadAllLines(openDialog.FileName).ToList();

                    stateData.RemoveAt(0);

                    foreach (var line in stateData)
                    {
                        var values = line.Split(',');
                        int lineIdx = int.Parse(values[0]);
                        int boxIdx = int.Parse(values[1]);
                        GuessState state = (GuessState)int.Parse(values[2]);
                        string value = values[3];

                        _guessBoxes[lineIdx].Boxes[boxIdx].Box.Text = value;
                        _guessBoxes[lineIdx].Boxes[boxIdx].State = state;
                        _guessBoxes[lineIdx].SetColors();
                    }
                }
            }
        }

        private void Reset()
        {
            _isPlaying = false;
            _selectedWord = string.Empty;
            _possibleWords = _allWords.ToList();

            possibleWordsBox.Clear();
            selectedWordLabel.Text = "";
            wordSelectedLabel.Visible = false;


            foreach (var box in _guessBoxes)
            {
                box.Clear();
            }

            UpdatePossibleList();

            _guessBoxes.First().Focus();
        }

        private void Test()
        {
            var rnd = new Random(0);
            int wins = 0;
            int games = _allWords.Count;
            var failed = new List<string>();
            var usedWords = new List<string>();
            this.SuspendLayout();

            var timer = new System.Diagnostics.Stopwatch();
            timer.Restart();


            for (int i = 0; i < games; i++)
            {
                _isPlaying = true;
                _possibleWords = _allWords.ToList();

                _selectedWord = _allWords[rnd.Next(_allWords.Count)];

                while (usedWords.Contains(_selectedWord))
                    _selectedWord = _allWords[rnd.Next(_allWords.Count)];

                usedWords.Add(_selectedWord);

                int guesses = 0;

                while (guesses < NUM_GUESSES)
                {
                    AnalyzeGuesses(_possibleWords, false);

                    var guessWord = _possibleWords.First();
                    _guessBoxes[guesses].Word = guessWord;

                    SetBoxStates(false);

                    if (_guessBoxes[guesses].IsMatch)
                        break;

                    guesses++;
                }

                if (guesses < NUM_GUESSES)
                    wins++;
                else
                    failed.Add(_selectedWord);

                if (i % 100 == 0)
                    Log($"[{i}] Word: {_selectedWord}  Guesses: {guesses}  Result: { (guesses < NUM_GUESSES) }");

                foreach (var box in _guessBoxes)
                {
                    box.Word = "";
                    foreach (var s in box.Boxes)
                    {
                        s.State = GuessState.NotInWord;
                        //s.Box.Text = "";
                    }
                }
            }


            timer.Stop();
            Log(string.Format("Timer: {0} ms  {1} ticks", timer.Elapsed.TotalMilliseconds, timer.Elapsed.Ticks));

            this.ResumeLayout();

            Log($"Wins: {wins} of {games}");


        }

        private void Log(string message)
        {
            if (_debug)
                Debug.WriteLine(message);
            else
                File.AppendAllText($@"{Environment.CurrentDirectory}/log.txt", message + "\n");
        }

        private void GuessBox_EnterKeyPressed(object sender, EventArgs e)
        {
            if (_isPlaying)
                SetBoxStates();

            AnalyzeGuesses(_allWords.ToList());

            var box = sender as GuessBoxes;
            if (box.Index + 1 < _guessBoxes.Length)
                _guessBoxes[box.Index + 1].Focus();
        }

        private void analyzeButton_Click(object sender, EventArgs e)
        {
            if (_isPlaying)
                SetBoxStates();

            AnalyzeGuesses(_allWords.ToList());
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveState();
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            LoadState();
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            StartPlay();
        }

        private void testButton_Click(object sender, EventArgs e)
        {
            Test();
        }

        private void showWordCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            selectedWordLabel.Visible = showWordCheckBox.Checked;
        }
    }
}
