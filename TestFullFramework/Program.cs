
#if MONO
using Mono.Data.Sqlite;
using Connection = Mono.Data.Sqlite.SqliteConnection;
using Command = Mono.Data.Sqlite.SqliteCommand;
using DataReader = Mono.Data.Sqlite.SqliteDataReader; // SQLiteDataReader

#else 
// using Connection = System.Data.SQLite.SQLiteConnection;
// using Command = System.Data.SQLite.SqliteCommand;
// using DataReader = System.Data.SQLite.SqliteDataReader; // SQLiteDataReader

using Connection = System.Data.SqlClient.SqlConnection;
#endif 

namespace TestFullFramework
{
    internal class Program
    {
        static int Main(string[] args)
        {

            
            System.IO.Stream stream = null;
            int x = 132; int y = 164; int zoom = 8;
            string csb = "Data Source=D:\\username\\Downloads\\COR_switzerland.mbtiles;Version=3; Read Only=True;";
            using (System.Data.Common.DbConnection conn = new Connection(csb))
            {
                if (conn.State != System.Data.ConnectionState.Open)
                    conn.Open();

                using (System.Data.Common.DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT * FROM tiles WHERE tile_column = {0} and tile_row = {1} and zoom_level = {2}", x, y, zoom);

                    System.Data.Common.DbDataReader reader = cmd.ExecuteReader();
                    /*
                    if (reader.Read())
                    {
                        var aaa = reader.GetBlob(reader.GetOrdinal("tile_data"), true);
                        int numBytes = aaa.GetCount();
                        byte[] buffer = new byte[numBytes];


                        aaa.Read(buffer, numBytes, 0);
                        stream = new MemoryStream(buffer);
                        stream.Position = 0;
                    }*/
                    if (reader.Read())
                    {
                        int ord = reader.GetOrdinal("tile_data");
                        // stream = reader.GetBytes(ord);
                        long dataLength = reader.GetBytes(ord, 0, null, 0, 0); // get the length 
                        byte[] buffer = new byte[dataLength];
                        reader.GetBytes(ord, 0, buffer, 0, (int)dataLength); // get the data 
                    }

                    if (conn.State != System.Data.ConnectionState.Open)
                        conn.Close();
                } // End Using cmd 

            } // End Using conn 

            return 0;
        }
    }
}
