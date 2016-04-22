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
        public static Context SYSCONTEXT { get; set; }

        /*
        public static Choices GetGrammar(string contextname = null)
        {
            if (String.IsNullOrEmpty(contextname))
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
            return null;
        }*/

        public static Context GetContext(string ContextName = null)
        {
            using (SQLiteConnection sqlite = new SQLiteConnection(GlobalManager.SQLCHAIN))
            {
                string sql = "select * from SENTENCES INNER JOIN COMMANDS on COMMANDS.ID in (select CMDID from TRIGGERCMD where SENTENCES.SENID = TRIGGERCMD.SENID) AND INNER JOIN MODULES on MODULES.NAME = COMMANDS.MODULENAME";
                SYSCONTEXT.SENTENCESLIST = sqlite.Query<SENTENCES, COMMANDS, MODULES, SENTENCES>
                    (sql, (sentence, command, module) => { command.MODULE = module; sentence.CMD = command; return sentence; }).ToList<SENTENCES>();
            }
            return SYSCONTEXT;
        }

        private static Context MergeContexts(Context mathiasContext)
        {
            mathiasContext.SENTENCESLIST = mathiasContext.SENTENCESLIST.Concat(SYSCONTEXT.SENTENCESLIST).ToList();
            return mathiasContext;
        }

        /// <summary>
        /// Load SYSTEM Context
        /// </summary>
        private static void LoadSysContext()
        {
            using (SQLiteConnection sqlite = new SQLiteConnection(GlobalManager.SQLCHAIN))
            {
                string sql = "select * from SENTENCES INNER JOIN COMMANDS on COMMANDS.ID in (select CMDID from TRIGGERCMD where SENTENCES.SENID = TRIGGERCMD.SENID)AND COMMANDS.MODULENAME = 'SYSTEM' INNER JOIN MODULES on MODULES.NAME = COMMANDS.MODULENAME";
                SYSCONTEXT.SENTENCESLIST = sqlite.Query<SENTENCES, COMMANDS, MODULES, SENTENCES>
                    (sql, (sentence, command, module) => {
                        command.MODULE = module;
                        sentence.CMD = command;
                        return sentence;
                    }).ToList<SENTENCES>();
                SYSCONTEXT.NAME = "System";
            }
        }
    }
}
