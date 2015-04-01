using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuve.Lang;

namespace hunspell_tr
{
    public static class Nuve
    {
        private static readonly WordAnalyzer Analyzer = new WordAnalyzer(Language.Turkish);

        public static bool HasSolution(string word)
        {
            var solutions = Analyzer.Analyze(word);
            return solutions.Count > 0;
        }

        public static bool AreAllWordsValid(IEnumerable<string> words)
        {
            bool areAllWordsValid = true;
            foreach (var word in words)
            {
                if (!HasSolution(word))
                {
                    areAllWordsValid = false;
                    Console.WriteLine(@"No solution for the new word :" + word );
                }
            }

            return areAllWordsValid;
        }

        public static bool TryStem(string word, out string stem)
        {
            stem = null;
            
            var solutions = Analyzer.Analyze(word);

            if (solutions.Count == 0)
            {
                return false;
            }

            int index = 0;
            for (int j = 0; j < solutions.Count; j++)
            {
                if (solutions[j].HasSuffix("IC_COGUL_lAr"))
                {
                    index = j;
                    break;
                }
            }

            stem = solutions[index].GetStem().GetSurface();
            return true;
        }
    }
}
