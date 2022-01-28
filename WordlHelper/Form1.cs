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

        public Form1()
        {
            InitializeComponent();
            InitGuessBoxes();
            InitWords();
        }

        private void AnalyzeGuesses()
        {
            var incorrectPos = new string[5];
            var correctPos = new string[5];
            var notInWord = new HashSet<string>();

            foreach (var boxes in _guessBoxes)
            {
                for (int i = 0; i < boxes.Boxes.Length; i++)
                {
                    var state = boxes.Boxes[i];
                    state.Box.Text = state.Box.Text.Trim();

                    if (!string.IsNullOrEmpty(state.Box.Text))
                    {
                        switch (state.State)
                        {
                            case GuessState.CorrectPosition:
                                correctPos[i] = state.Box.Text;
                                break;
                            case GuessState.IncorrectPosition:
                                incorrectPos[i] = state.Box.Text;
                                break;
                            case GuessState.NotInWord:
                                if (!correctPos.Contains(state.Box.Text))
                                    notInWord.Add(state.Box.Text);
                                break;
                        }
                    }
                }
            }

            _possibleWords = GetPossibleWords(notInWord, incorrectPos, correctPos, _allWords.ToList());

            UpdatePossibleList();
        }

        private void UpdatePossibleList()
        {
            matchesCountLabel.Text = $"Matches: {_possibleWords.Count}";

            possibleWordsBox.SuspendLayout();
            possibleWordsBox.Clear();
            foreach (var word in _possibleWords)
            {
                possibleWordsBox.AppendText(word + "\n");
            }
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
            possibleWordsBox.Clear();

            foreach (var box in _guessBoxes)
            {
                box.Clear();
            }

            _guessBoxes.First().Boxes.First().Box.Focus();
        }

        private void analyzeButton_Click(object sender, EventArgs e)
        {
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
    }
}
