
namespace TestSystemDataSQLite
{


    internal class Program
    {


        public static void WriteStreamToFileBase64(System.IO.Stream? stream, string filePath)
        {
            if (stream == null || string.IsNullOrEmpty(filePath))
                throw new System.ArgumentNullException(stream == null ? nameof(stream) : nameof(filePath));

            using (System.IO.FileStream fileStream = System.IO.File.OpenWrite(filePath))
            {
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(fileStream))
                {
                    // Convert stream to byte array
                    byte[] data = ReadStreamToByteArray(stream);

                    // Encode byte array to base64 string
                    string base64String = System.Convert.ToBase64String(data);

                    // Write base64 string to file
                    writer.Write(base64String);
                } // End Using writer

            } // End Using fileStream 

        } // End Sub WriteStreamToFileBase64 


        private static byte[] ReadStreamToByteArray(System.IO.Stream stream)
        {

            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            } // End Using memoryStream 

        } // End Function ReadStreamToByteArray 


        public static async System.Threading.Tasks.Task<int> Main(string[] args)
        {
            System.IO.Stream stream = null;
            int x = 132; int y = 164; int zoom = 8;
            string csb = "Data Source=D:\\username\\Downloads\\COR_switzerland.mbtiles;Version=3; Read Only=True;";


            using (System.Data.Common.DbConnection conn = new System.Data.SQLite.SQLiteConnection(csb))
            {
                if (conn.State != System.Data.ConnectionState.Open)
                    conn.Open();

                using (System.Data.Common.DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT * FROM tiles WHERE tile_column = {0} and tile_row = {1} and zoom_level = {2}", x, y, zoom);

                    using (System.Data.Common.DbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) stream =
                            reader.GetStream(reader.GetOrdinal("tile_data"));

                        WriteStreamToFileBase64(stream, @"D:\sqlite_netcore.txt");
                    } // End Using reader 

                    if (conn.State != System.Data.ConnectionState.Open)
                        conn.Close();
                } // End Using cmd 

            } // End Using conn 

            System.Console.WriteLine(" --- Press any key to continue --- ");
            System.Console.ReadKey();
            return await System.Threading.Tasks.Task.FromResult(0);
        } // End Task Main 


    } // End Class Program 


} // End Namespace 
