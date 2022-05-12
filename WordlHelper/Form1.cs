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
using Fastenshtein;

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
        private int _lastSelectedBoxIdx = -1;
        private Dictionary<string, string> _distinctLetters = new Dictionary<string, string>();

        public Form1()
        {
            InitializeComponent();
            InitGuessBoxes();
            InitWords();

            this.Show();
            this.Invalidate();
            var prevText = this.Text;
            this.Invalidate();
            this.Refresh();

            SortByLetterCounts(_allWords);

            AnalyzeGuesses(_allWords.ToList());
        }

        private void StartPlay()
        {
            Reset();
            _isPlaying = true;

            var rnd = new Random();

            testWordTextBox.Text = testWordTextBox.Text.Trim();

            if (string.IsNullOrEmpty(testWordTextBox.Text))
                _selectedWord = _allWords[rnd.Next(_allWords.Count)];
            else
                _selectedWord = testWordTextBox.Text;

            //_selectedWord = "WACKE";

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
            notInWord = notInWord.Except(incorrectPos.ToList()).ToHashSet();

            _possibleWords = GetPossibleWords(notInWord, incorrectPos, correctPos, allwords);
            _possibleWords = _possibleWords.Except(boxWords).ToList();
            _possibleWords = SortByLetterCounts(_possibleWords);

            if (updateUI)
                UpdatePossibleList();
        }

        private void UpdatePossibleList()
        {
            matchesCountLabel.Text = $"Count: {_possibleWords.Count}";

            possibleWordsList.SuspendLayout();

            possibleWordsList.DataSource = _possibleWords;

            possibleWordsList.ResumeLayout();
        }

        private static List<string> GetPossibleWords(HashSet<string> notInWord, string[] incorrectPos, string[] correctPos, List<string> allWords)
        {
            var possibles = allWords.ToList();
            var removes = new HashSet<string>();

            // First filter words by known correct letters.
            var mustContain = incorrectPos.Where(i => i != null).ToList();
            mustContain.AddRange(correctPos.Where(c => c != null).ToList());
            mustContain = mustContain.Distinct().ToList();
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

        private void InitGuessBoxes(bool updateUI = true)
        {
            guessBoxesPanel.Controls.Clear();

            for (int i = 0; i < NUM_GUESSES; i++)
            {
                if (_guessBoxes[i] != null)
                {
                    _guessBoxes[i].EnterKeyPressed -= GuessBox_EnterKeyPressed;
                    _guessBoxes[i].BoxSelected -= GuessBox_BoxSelected;
                }

                var guessBox = new GuessBoxes(i, updateUI);
                guessBox.EnterKeyPressed += GuessBox_EnterKeyPressed;
                guessBox.BoxSelected += GuessBox_BoxSelected;

                guessBoxesPanel.Controls.Add(guessBox, 0, i);
                _guessBoxes[i] = guessBox;
            }
        }

        private void HighlightBox(int idx)
        {
            _lastSelectedBoxIdx = idx;

            for (int i = 0; i < _guessBoxes.Length; i++)
            {
                if (i == idx)
                    _guessBoxes[i].HighlightBox();
                else
                    _guessBoxes[i].UnHighlightBox();
            }
        }

        private void InitWords()
        {
            // Parse the word list to a string array.
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

            selectedWordLabel.Text = "";
            wordSelectedLabel.Visible = false;

            foreach (var box in _guessBoxes)
            {
                box.Clear();
            }

            AnalyzeGuesses(_allWords, false);

            UpdatePossibleList();

            InitGuessBoxes();

            _guessBoxes.First().Focus();
        }

        /// <summary>
        /// Sorts the word list by number of unique letter occurrences.
        /// </summary>
        /// <param name="words"></param>
        /// <returns></returns>
        /// <remarks>
        /// The idea is to prioritize words that will eliminate the maximum number of possible words.
        /// </remarks>
        private List<string> SortByLetterCounts(List<string> words)
        {
            // Record the distinct letters for each word.
            // We don't want duplicate letters to contribute to the rank.
            // Cached for speed.
            if (_distinctLetters.Count == 0)
            {
                foreach (var word in words)
                {
                    var letters = new string(word.Distinct().ToArray());
                    _distinctLetters.Add(word, letters);
                }
            }

            // Accumulate the number of letter occurrences for each word in the list.
            var counts = new Dictionary<char, int>();
            foreach (var word in words)
            {
                var letters = _distinctLetters[word];

                foreach (var letter in letters)
                {
                    if (!counts.ContainsKey(letter))
                        counts.Add(letter, 1);
                    else
                        counts[letter] += 1;
                }
            }

            // Now use the letter counts and rank each word.
            var rWords = new string[words.Count];
            var rRanks = new int[words.Count];
            for (int idx = 0; idx < words.Count; idx++)
            {
                var word = words[idx];
                var letters = _distinctLetters[word];
                int rank = 0;
                foreach (var letter in letters)
                {
                    rank += counts[letter];
                }

                rWords[idx] = word;
                rRanks[idx] = rank;
            }

            // Sort the words by rank in descending order.
            Array.Sort(rRanks, rWords);
            Array.Reverse(rWords);

            return rWords.ToList();
        }

        private void Test()
        {
            InitGuessBoxes(false);

            var rnd = new Random(5464);
            int wins = 0;
            int games = 500;//_allWords.Count;
            var failed = new List<string>();
            var usedWords = new List<string>();
            var failGuesses = new Dictionary<string, List<string>>();

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

                var guessWords = new List<string>();

                int guesses = 0;

                while (guesses < NUM_GUESSES)
                {
                    //AnalyzeGuesses(_possibleWords, false);
                    AnalyzeGuesses(_allWords.ToList(), false);

                    var guessWord = _possibleWords.First();
                    _guessBoxes[guesses].Word = guessWord;

                    guessWords.Add(guessWord);

                    SetBoxStates(false);

                    if (_guessBoxes[guesses].IsMatch)
                        break;

                    guesses++;
                }

                if (guesses < NUM_GUESSES)
                    wins++;
                else
                {
                    failed.Add(_selectedWord);
                    failGuesses.Add(_selectedWord, guessWords);
                }

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

            Log("Failed words:");

            failed.ForEach(w =>
            {
                Log(w);
                failGuesses[w].ForEach(g => Log($"  {g}"));
            });


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

        private void GuessBox_BoxSelected(object sender, int e)
        {
            HighlightBox(e);
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

        private void possibleWordsList_DoubleClick(object sender, EventArgs e)
        {
            if (_lastSelectedBoxIdx > -1)
            {
                _guessBoxes[_lastSelectedBoxIdx].Word = possibleWordsList.SelectedItem.ToString();

                if (_isPlaying)
                {
                    SetBoxStates();
                    AnalyzeGuesses(_allWords.ToList());
                }
            }
        }
    }
}
