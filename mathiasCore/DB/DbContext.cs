//using System.Data.SQLite;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.IO;

//namespace mathiasCore.DB
//{
//    public class DbContext
//    {
//        private SQLiteConnection CLIENT { get; set; }
//        private String DBPATH { get; set; }
//        private String DBFILE { get; set; }
//        private String SQLCHAIN { get; set; }
//        public DbContext()
//        {
//            DBPATH = String.Format("{0}\\database", Directory.GetCurrentDirectory());
//            DBFILE = "mathias.db";
//            Init();

//            CLIENT = new SQLiteConnection(String.Format("Data Source = {0}\\{1}; Version = 3;", DBPATH, DBFILE));
//        }


//    }
//}
