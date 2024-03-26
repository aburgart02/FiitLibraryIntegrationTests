using Kontur.BigLibrary.Service.Services.BookService;
using UnidecodeSharpFork;

namespace Kontur.BigLibrary.Service.Services.SynonimMaker
{
    public class SynonymMaker : ISynonymMaker
    {
        public string Create(string text)
        {
            var syn = text.Unidecode().Replace(' ', '_');
            var synonym = "";
            foreach (var letter in syn)
            {
                if (char.IsLetter(letter) || char.IsDigit(letter) || letter == '_')
                {
                    synonym += letter;
                }
            }
            return synonym;
        }
    }
  
}