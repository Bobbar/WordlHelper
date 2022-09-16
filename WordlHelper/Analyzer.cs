using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace WordlHelper
{
    public class Analyzer
    {
        private List<string> _allWords;
        private int[] _wordFlags;
        private int[] _flags;
        private int[] _validIdxs;

        public Analyzer(List<string> words)
        {
            _allWords = new List<string>(words);

            Init();
        }

        private void Init()
        {
            _validIdxs = new int[_allWords.Count];

            // Cache flag values for all letters.
            // Indexed & padded into an array for direct lookups via the OG char value.
            _flags = new int[92];
            int start = 64; // Starting ASCII decimal value. (Capitol letter A)
            int len = 27; // How many letters to cache. (Stop at Z)
            int flag = 1;
            for (int i = 1; i < len; i++)
            {
                _flags[start + i] = flag;
                flag = 1 << i;
            }

            // Cache summed flag values for all words.
            // Indexed to match the all words list.
            _wordFlags = new int[_allWords.Count];
            for (int i = 0; i < _allWords.Count; i++)
            {
                _wordFlags[i] = WordToFlag(_allWords[i]);
            }

            var test = _wordFlags.ToList().Distinct().ToList();
        }

        public List<string> Filter(IEnumerable<char> notInWord, IEnumerable<char> mustContain, ICollection<CharPos> correctPos, IEnumerable<CharPos> incorrectPos)
        {
            if (notInWord.Count() == 0)
                return new List<string>(_allWords);

            int idx = 0;
            int notInWordFlags = 0;
            int mustContainFlags = 0;

            // Compute the flags for not-in-word and must-contain characters.
            foreach (var n in notInWord)
                    notInWordFlags += _flags[n];

            foreach (var m in mustContain)
                    mustContainFlags += _flags[m];


            for (int j = 0; j < _allWords.Count; j++)
            {
                var flags = _wordFlags[j];

                // Filter out words with letters known to NOT exists in the answer.
                if ((flags & notInWordFlags) == 0)
                {
                    // Filter words with letters that are known to be in the answer.
                    if ((flags & mustContainFlags) == mustContainFlags)
                    {
                        bool isValid = true;

                        // Filter by known correct and incorrect positions.
                        foreach (var corPos in correctPos)
                        {
                            if (_allWords[j][corPos.Idx] != corPos.Char)
                                isValid = false;
                        }

                        foreach (var inPos in incorrectPos)
                        {
                            if (_allWords[j][inPos.Idx] == inPos.Char)
                                isValid = false;
                        }

                        // Record the index of the valid word.
                        if (isValid)
                            _validIdxs[idx++] = j;
                    }
                }

            }

            // Build the new list of possible words.
            var res = new List<string>();
            for (int i = 0; i < idx; i++)
            {
                res.Add(_allWords[_validIdxs[i]]);
            }

            return res;
        }

        private int WordToFlag(string word)
        {
            int flag = 0;

            for (int i = 0; i < word.Length; i++)
            {
                var cf = _flags[word[i]];

                if ((flag & cf) == 0) // Make sure the computed flag doesn't already contain the next flag. (Don't add duplicate characters to the result)
                    flag += cf;
            }

            return flag;
        }
    }

    public struct CharPos
    {
        public char Char;
        public int Idx;

        public CharPos(char chr, int idx)
        {
            Char = chr;
            Idx = idx;
        }
    }
}
