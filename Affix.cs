 using System.Text;

namespace hunspell_tr
{
    class Affix
    {        
        public int Id { get; set; }
        public string Surface { get; set; }

        public Affix(int id, string surface)
        {
            Id = id;
            Surface = surface;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("SFX ").Append(Id).Append(" N 1").Append("\n").
               Append("SFX ").Append(Id).Append(" 0 ").Append(Surface).Append(" .");

            return sb.ToString();
        }
    }
}
