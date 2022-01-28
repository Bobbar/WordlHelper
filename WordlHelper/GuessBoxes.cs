using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WordlHelper
{
    public partial class GuessBoxes : UserControl
    {
        private const int NUM_BOXES = 5;

        public BoxState[] Boxes = new BoxState[NUM_BOXES];

        public GuessBoxes()
        {
            InitializeComponent();
            InitBoxes();
        }

        private void InitBoxes()
        {
            for (int i = 0; i < NUM_BOXES; i++)
            {
                var box = new TextBox();
                box.MouseDoubleClick += Box_MouseDoubleClick;
                box.KeyUp += Box_KeyUp;
                box.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                box.TextAlign = HorizontalAlignment.Center;
                box.MaxLength = 1;
                box.Tag = i;
                Boxes[i] = new BoxState(box);
                boxLayoutPanel.Controls.Add(box, i, 0);
            }

            SetColors();
        }

        public void Clear()
        {
            foreach (var box in Boxes)
            {
                box.State = GuessState.NotInWord;
                box.Box.Clear();
            }

            SetColors();
        }

        private void Box_KeyUp(object sender, KeyEventArgs e)
        {
            var senderBox = sender as TextBox;
            var nextIdx = (int)senderBox.Tag;


            if (e.KeyCode == Keys.Back)
            {
                if (nextIdx - 1 >= 0)
                    nextIdx--;
            }
            else
            {
                if (!string.IsNullOrEmpty(senderBox.Text.Trim()))
                {
                    if (nextIdx + 1 < NUM_BOXES)
                        nextIdx++;
                }
            }

            Boxes[nextIdx].Box.Focus();

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

        private void Box_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var state = Boxes[(int)((Control)sender).Tag].State;
            Boxes[(int)((Control)sender).Tag].State = NextState(state);
            SetColors();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            Clear();
        }
    }

    public class BoxState
    {
        public TextBox Box { get; set; }
        public GuessState State { get; set; }

        public BoxState(TextBox box)
        {
            Box = box;
            State = GuessState.NotInWord;
        }
    }

    public enum GuessState
    {
        NotInWord,
        CorrectPosition,
        IncorrectPosition
    }
}
