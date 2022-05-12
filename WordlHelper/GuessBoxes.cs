using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace WordlHelper
{
    public partial class GuessBoxes : UserControl
    {
        private const int NUM_BOXES = 5;

        public BoxState[] Boxes = new BoxState[NUM_BOXES];

        private string _currentWord = string.Empty;
        private Color _defaultBackColor = Color.Gray;
        private bool _updateUI = true;

        public int Index { get; set; }

        public bool IsValid
        {
            get
            {

                return !string.IsNullOrEmpty(_currentWord);
            }
        }

        public string Word
        {
            set
            {
                _currentWord = value;
                if (_updateUI)
                    SetBoxValues();
            }

            get
            {
                return _currentWord;
            }
        }

        public bool IsMatch
        {
            get
            {
                return Boxes.All(b => b.State == GuessState.CorrectPosition);
            }

        }

        public event EventHandler EnterKeyPressed;
        public event EventHandler<int> BoxSelected;


        public GuessBoxes(int index, bool updateUI = true)
        {
            InitializeComponent();
            _updateUI = updateUI;
            Index = index;
            _defaultBackColor = this.BackColor;

            InitBoxes();
        }

        private void OnEnterKeyPressed()
        {
            EnterKeyPressed?.Invoke(this, new EventArgs());
        }

        private void OnBoxSelected()
        {
            BoxSelected?.Invoke(this, Index);
        }

        private void InitBoxes()
        {
            for (int i = 0; i < NUM_BOXES; i++)
            {
                var box = new Label();
                box.MouseDoubleClick += Box_MouseDoubleClick;
                box.MouseClick += Box_MouseClick;
                box.Font = new Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                box.TextAlign = ContentAlignment.MiddleCenter;
                box.AutoSize = false;
                box.Dock = DockStyle.Fill;
                box.Tag = i;
                Boxes[i] = new BoxState(box);
                boxLayoutPanel.Controls.Add(box, i, 0);
            }

            SetColors();
        }

        private void SetBoxValues()
        {
            for (int i = 0; i < NUM_BOXES; i++)
            {
                var box = Boxes[i];

                if (i < _currentWord.Length)
                    box.Box.Text = _currentWord[i].ToString();
                else
                    box.Box.Text = string.Empty;
            }
        }

        public void Clear()
        {
            _currentWord = string.Empty;
            foreach (var box in Boxes)
            {
                box.State = GuessState.NotInWord;
                box.Box.Text = string.Empty;
            }

            clearButton.Enabled = false;
            Application.DoEvents();
            SetColors();
            this.Focus();
            clearButton.Enabled = true;
        }

        private GuessState NextState(GuessState current)
        {
            var cnt = Enum.GetValues(typeof(GuessState)).Length;
            var curIdx = (int)current;

            if (curIdx + 1 < cnt)
                return (GuessState)curIdx + 1;
            else if (curIdx + 1 >= cnt)
                return (GuessState)0;

            return GuessState.NotInWord;
        }

        public void SetColors()
        {
            foreach (var box in Boxes)
            {
                switch (box.State)
                {
                    case GuessState.CorrectPosition:
                        box.Box.BackColor = Color.Green;
                        break;
                    case GuessState.IncorrectPosition:
                        box.Box.BackColor = Color.Goldenrod;
                        break;
                    case GuessState.NotInWord:
                        box.Box.BackColor = Color.Gray;
                        break;
                }
            }
        }

        public void HighlightBox()
        {
            this.BackColor = Color.SkyBlue;
            this.BorderStyle = BorderStyle.FixedSingle;
        }

        public void UnHighlightBox()
        {
            this.BackColor = _defaultBackColor;
            this.BorderStyle = BorderStyle.None;
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            OnBoxSelected();
            Clear();

        }

        private void Box_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var state = Boxes[(int)((Control)sender).Tag].State;
            Boxes[(int)((Control)sender).Tag].State = NextState(state);
            SetColors();
        }

        private void Box_MouseClick(object sender, MouseEventArgs e)
        {
            OnBoxSelected();
        }

        private void boxLayoutPanel_MouseClick(object sender, MouseEventArgs e)
        {
            OnBoxSelected();
        }

        private void GuessBoxes_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                if (_currentWord.Length > 0)
                    _currentWord = _currentWord.Remove(_currentWord.Length - 1);
            }
            else if (e.KeyCode == Keys.Enter)
            {
                OnEnterKeyPressed();
            }
            else
            {
                var kc = new KeysConverter();
                if (_currentWord.Length < NUM_BOXES)
                    _currentWord += kc.ConvertToString(e.KeyData);
            }

            SetBoxValues();
        }

        public override string ToString()
        {
            return Word;
        }
    }
}
