using System.Data.SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using mathiasModels;
using Dapper;
using Microsoft.Speech.Recognition;

namespace mathiasCore.DB
{
    public static class DBContext
    {

        public static Choices GetGrammar()
        {
            var commands = new Choices();
            using (SQLiteConnection sqlite = new SQLiteConnection(GlobalManager.SQLCHAIN))
            {
                
                List<SENTENCES> sentence = sqlite.Query<SENTENCES>("SELECT * from SENTENCES").ToList();
                foreach (SENTENCES sen in sentence)
                {
                    sen.CMD = sqlite.Query<COMMANDS>(String.Format("SELECT * FROM COMMANDS where COMMANDS.ID in (select CMDID from TRIGGERCMD where SENID = {0})", sen.SENID)).Single();
                    commands.Add(new SemanticResultValue(sen.SENTENCE, sen.CMD.CMD));
                }
            }
            
            return commands;
        }


    }
}
