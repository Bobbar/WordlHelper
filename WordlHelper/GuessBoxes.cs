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

        public GuessBoxes(int index)
        {
            InitializeComponent();
            Index = index;
            _defaultBackColor = this.BackColor;

            this.GotFocus += GuessBoxes_GotFocus;
            this.LostFocus += GuessBoxes_LostFocus;
            InitBoxes();
        }

        private void OnEnterKeyPressed()
        {
            EnterKeyPressed?.Invoke(this, new EventArgs());
        }

        private void GuessBoxes_LostFocus(object sender, EventArgs e)
        {
            this.BackColor = _defaultBackColor;
            this.BorderStyle = BorderStyle.None;

        }

        private void GuessBoxes_GotFocus(object sender, EventArgs e)
        {
            this.BackColor = Color.SkyBlue;
            this.BorderStyle = BorderStyle.FixedSingle;
        }

        private void InitBoxes()
        {
            for (int i = 0; i < NUM_BOXES; i++)
            {
                var box = new Label();
                box.MouseDoubleClick += Box_MouseDoubleClick;
                box.Click += Box_Click;
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

            SetColors();
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

        private void clearButton_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void GuessBoxes2_KeyDown(object sender, KeyEventArgs e)
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

        private void Box_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var state = Boxes[(int)((Control)sender).Tag].State;
            Boxes[(int)((Control)sender).Tag].State = NextState(state);
            SetColors();
        }

        private void Box_Click(object sender, EventArgs e)
        {
            this.Focus();
        }
    }

}
