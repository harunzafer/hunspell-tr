using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace hunspell_tr
{
    static class Unigrams
    {
        public static IEnumerable<string> Read(string path)
        {
            var text = File.ReadAllText(path);
            return Get(text);
        }
     
        public static IEnumerable<string> Get(string text)
        {
            List<string> tokens = text.Split(null).ToList();//split based on whitespace
            tokens.RemoveAll(t => !string.IsNullOrEmpty(t) && !Char.IsLetter(t[0]));
            tokens.Sort();
            return tokens.Distinct();
        }

    }
}
