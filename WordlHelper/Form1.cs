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

        private string[] _allWords;
        private List<string> _possibleWords;
        private GuessBoxes[] _guessBoxes = new GuessBoxes[NUM_GUESSES];

        private string _selectedWord = string.Empty;
        private bool _isPlaying = false;

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
            _selectedWord = _allWords[rnd.Next(_allWords.Length)];
            selectedWordLabel.Text = $"Word: {_selectedWord}";
        }

        private void SetStates()
        {
            if (!_isPlaying)
                return;

            foreach (var boxes in _guessBoxes)
            {
                if (boxes.IsValid)
                {
                    var mismatches2 = boxes.Word.Except(_selectedWord).ToList();
                    var matches = boxes.Word.Intersect(_selectedWord).ToList();
                    var boxLetterCounts = boxes.Word.GroupBy(c => c).ToDictionary(k => k.Key, e => e.Count());
                    var selectedLetterCounts = _selectedWord.GroupBy(c => c).ToDictionary(k => k.Key, e => e.Count());


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

                boxes.SetColors();
            }
        }

        private void AnalyzeGuesses()
        {
            var incorrectPos = new string[5];
            var correctPos = new string[5];
            var notInWord = new HashSet<string>();

            var boxWords = new List<string>();
            foreach (var boxes in _guessBoxes)
            {
                boxWords.Add(boxes.Word);

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
            _possibleWords = SortByDistribution(_possibleWords);
            _possibleWords = _possibleWords.Except(boxWords).ToList();

            UpdatePossibleList();
        }

        private List<string> SortByDistribution(List<string> words)
        {
            var dists = new Dictionary<char, int>();

            foreach (var word in words)
            {
                foreach (var letter in word)
                {
                    if (!dists.ContainsKey(letter))
                        dists.Add(letter, 1);
                    else
                        dists[letter]++;
                }
            }

            var ranks = new Dictionary<string, int>();

            foreach (var word in words)
            {
                if (!ranks.ContainsKey(word))
                    ranks.Add(word, 0);

                char lastLet = ' ';
                foreach (var letter in word)
                {
                    if (letter != lastLet)
                    {
                        ranks[word] += dists[letter];
                    }
                    lastLet = letter;
                }
            }

            var rankSort = ranks.ToList();
            rankSort = rankSort.OrderByDescending(r => r.Value).ToList();

            var ret = new List<string>();
            rankSort.ForEach(r => ret.Add(r.Key));
            return ret;
        }

        private void UpdatePossibleList()
        {
            matchesCountLabel.Text = $"Matches: {_possibleWords.Count}";

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
            var possibles = allWords.ToList<string>();
            var removes = new HashSet<string>();

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

            // Remove words not containing letters in wrong position.
            for (int w = 0; w < possibles.Count; w++)
            {
                var word = possibles[w];

                bool hasAll = true;
                foreach (var let in incorrectPos)
                {
                    if (let != null && !word.Contains(let))
                        hasAll = false;

                }

                if (!hasAll)
                    removes.Add(word);

                for (int i = 0; i < incorrectPos.Length; i++)
                {
                    if (word[i].ToString() == incorrectPos[i])
                        removes.Add(word);
                }
            }

            possibles = possibles.Except(removes).ToList();

            return possibles.ToList();
        }

        private void InitGuessBoxes()
        {
            for (int i = 0; i < NUM_GUESSES; i++)
            {
                var guessBox = new GuessBoxes();
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

            _allWords = words;
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

            possibleWordsBox.Clear();
            selectedWordLabel.Text = "";

            foreach (var box in _guessBoxes)
            {
                box.Clear();
            }

            UpdatePossibleList();

            _guessBoxes.First().Focus();
        }

        private void analyzeButton_Click(object sender, EventArgs e)
        {
            if (_isPlaying)
                SetStates();

            AnalyzeGuesses();
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
    }
}
