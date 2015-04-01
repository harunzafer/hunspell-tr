using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
            int count = 0;
            foreach (var word in words)
            {
                
                string stem;
                if (!map.ContainsKey(word) && Nuve.TryStem(word, out stem))
                {
                    map.Add(word, stem);
                }
                else
                {
                    count++;
                }
            }
            Console.WriteLine(count + " of " + words.Count() + " words can not be stemmed" );
            return map;
        }

        

      
    }
}