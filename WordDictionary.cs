using System.Collections.Generic;
using System.Text;

namespace hunspell_tr
{
    internal class WordDictionary
    {
     
        private readonly IDictionary<string, Word> _wordDictionary;

        public WordDictionary()
        {
            _wordDictionary = new Dictionary<string, Word>();
        }

        public WordDictionary(IEnumerable<string> words)
        {
        }

        public void AddWord(string word)
        {
            if (!_wordDictionary.ContainsKey(word))
            {
                var w = new Word(word);
                _wordDictionary.Add(word, w);
            }
        }

        public void AddAffixToWord(string word, int affixId)
        {
            if (_wordDictionary.ContainsKey(word))
            {
                _wordDictionary[word].AddAffix(affixId);
            }
        }

        public string Generate()
        {
            var sb = new StringBuilder();
            sb.Append(_wordDictionary.Count).Append("\n");
            foreach (var word in _wordDictionary)
            {
                sb.Append(word.Value).Append("\n");
            }

            return sb.ToString();
        }
    }
}