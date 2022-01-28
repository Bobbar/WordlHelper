using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WordlHelper
{
    public class BoxState
    {
        public Control Box { get; set; }
        public GuessState State { get; set; }

        public BoxState(Control box)
        {
            Box = box;
            State = GuessState.NotInWord;
        }
    }
}
