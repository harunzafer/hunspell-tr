using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace hunspell_tr
{
    internal static class DictionaryGenerator
    {
        public static void Generate(IDictionary<string, string> wordStemMap, string path, string langCode)
        {
            var affixDictionary = new AffixDictionary();
            var wordDictionary = new WordDictionary();

            foreach (var pair in wordStemMap)
            {
                var word = pair.Key;
                var stem = pair.Value;

                if (word == stem)
                {
                    wordDictionary.AddWord(word);
                }
                else if (word.StartsWith(stem))
                {
                    var suffix = word.Substring(stem.Length);
                    int affixId = affixDictionary.AddAffix(suffix);
                    wordDictionary.AddWord(stem);
                    wordDictionary.AddAffixToWord(stem, affixId);
                }
                else
                {
                    wordDictionary.AddWord(word);
                }
            }          

            var dic = wordDictionary.Generate();
            var aff = affixDictionary.GenerateDictFile();            

            File.WriteAllText(path + "/" + langCode + "-" + System.DateTime.Now + ".dic", dic, new UTF8Encoding(false));
            File.WriteAllText(path + "/" + langCode + "-" + System.DateTime.Now + ".aff", aff, new UTF8Encoding(false));

        }


    }
}