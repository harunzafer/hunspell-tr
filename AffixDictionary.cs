using System.Collections.Generic;
using System.Text;

namespace hunspell_tr
{
    class AffixDictionary
    {
        private readonly IDictionary<string, int> _affixIds;
        private readonly IDictionary<int, Affix> _affixDictionary;
        private const string Header = 
            "LANG tr_TR\n" +
            "SET UTF-8\n" +
            "TRY İiIıŞşÇçĞğÜüÖö-qwertyuopasdfghjklzxcvbnmQWERTYUOPASDFGHJKLZXCVBNM'\n" +
            "FLAG num\n\n";

        public AffixDictionary()
        {
            _affixIds = new Dictionary<string, int>();
            _affixDictionary = new Dictionary<int, Affix>();
        }

        public int AddAffix(string surface)
        {            
            if (_affixIds.ContainsKey(surface))
            {
                return _affixIds[surface];
            }

            int affixId = _affixDictionary.Count;
            _affixIds.Add(surface, affixId);
            _affixDictionary.Add(affixId, new Affix(affixId ,surface));

            return affixId;
        }

        public string GenerateDictFile()
        {
            var sb = new StringBuilder(Header);
            foreach (var affix in _affixDictionary)
            {
                sb.Append(affix.Value).Append("\n\n");
            }
            return sb.ToString();
        }
    }
}
