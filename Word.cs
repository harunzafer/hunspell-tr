using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hunspell_tr
{
    class Word
    {
        private readonly string _surface;
        private readonly ISet<int> _affixSet;

        public Word(string surface)
        {
            _surface = surface;
            _affixSet = new HashSet<int>();
        }

        public void AddAffix(int affixId)
        {
            _affixSet.Add(affixId);
        }

        public override string ToString()
        {            
            var sb = new StringBuilder();
            sb.Append(_surface);
            if (_affixSet.Count > 0)
            {
                var affixes = _affixSet.ToList();
                affixes.Sort();

                sb.Append("/");
                sb.Append(affixes[0]);
                for (int i = 1; i < affixes.Count; i++)
                {
                    sb.Append(",").Append(affixes[i]);
                }
            }
            return sb.ToString();
        }
    }
}
