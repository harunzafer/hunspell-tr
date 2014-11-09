using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Nuve.Lang;

namespace hunspell_tr
{
    internal static class Stems
    {
        /// <summary>
        /// Reads the word-stem pairs from the file at given path.
        /// </summary>
        /// <param name="stemDictionaryPath">path for the dictionary file</param>
        /// <returns>A map which contains word-stem pairs for every stemmable word in the given parameter.</returns>
        public static IDictionary<string, string> FromDictionary(string stemDictionaryPath)
        {
            var map = new Dictionary<string, string>();
            string[] lines = File.ReadAllLines(stemDictionaryPath);
            foreach (var line in lines)
            {
                var row = line.Split('\t');
                var word = row[0];
                var stem = row[1];

                if (!map.ContainsKey(word))
                {
                    map.Add(word, stem);
                }
            }
            return map;
        }

        /// <summary>
        /// Tries to stem everyword in given parameter and returns a map contains word-stem pairs.
        /// If a word can not be stemmed, it is ignored and does not exist in the map.
        /// </summary>
        /// <param name="words">words to be stemmed</param>
        /// <returns>A map which contains word-stem pairs for every stemmable word in the given parameter.</returns>
        public static IDictionary<string, string> Stem(IEnumerable<string> words)
        {
            var map = new Dictionary<string, string>();
            foreach (var word in words)
            {
                string stem;
                if (!map.ContainsKey(word) && TryStem(word, out stem))
                {
                    map.Add(word, stem);
                }
            }
            return map;
        }

        public static bool TryStem(string word, out string stem)
        {
            stem = null;
            var analyzer = new WordAnalyzer(Language.Turkish);
            var solutions = analyzer.Analyze(word);

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