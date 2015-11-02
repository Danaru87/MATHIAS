using MathiasModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using System.Configuration;

namespace DataProvider
{
    public class WordRetriever: IDisposable
    {
        private static SqlConnection _connexion;

        public WordRetriever()
        {
            _connexion = new SqlConnection(ConfigurationManager.ConnectionStrings.ToString());
            try
            {
                _connexion.Open();
            }
            catch(SqlException sqlE)
            {

            }
        }
        public List<Word> GetFullList()
        {
            List<Word> wordList = null;
            try
            {
                wordList = _connexion.Query<Word>("Select * From WORD where WOLANGSYSID = @LANGSYSID", new { LANGSYSID = 1 }).ToList();
            }
            catch
            {
                wordList = null;
            }
            return wordList;
        }

        public void Dispose()
        {
            _connexion.Close();
        }
    }
}
