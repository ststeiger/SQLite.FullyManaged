
namespace TestFullyManaged
{


    using Connection = SQLite.FullyManaged.SqliteConnection;
    using ConnectionStringBuilder = SQLite.FullyManaged.SqliteConnectionStringBuilder;


    internal class Program
    {


        public static byte[]? StreamToByteArray(System.IO.Stream stream)
        {
            byte[]? retData = null;

            if (stream != null)
            {
                // Assuming the stream size is relatively small
                int bufferSize = 4096; // Adjust buffer size as needed
                byte[] buffer = new byte[bufferSize];
                using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                {
                    int bytesRead;
                    while ((bytesRead = stream.Read(buffer, 0, bufferSize)) > 0)
                    {
                        memoryStream.Write(buffer, 0, bytesRead);
                    } // Whend 

                    retData = memoryStream.ToArray();
                } // End Using memoryStream 

            } // End if (stream != null) 

            return retData;
        } // End Function StreamToByteArray 


        public static async System.Threading.Tasks.Task ChangeNamespace()
        {
            string targetDirectory = "D:\\username\\Documents\\Visual Studio 2022\\Projects\\TestAccess\\SQLite.FullyManaged";

            foreach (string thisFile in System.IO.Directory.GetFiles(targetDirectory, "*.cs", System.IO.SearchOption.AllDirectories))
            {
                string fileContent = await System.IO.File.ReadAllTextAsync(thisFile);
                fileContent = fileContent.Replace("namespace Community.CsharpSqlite.SQLiteClient", "namespace SQLite.FullyManaged");
                fileContent = fileContent.Replace("namespace Community.CsharpSqlite", "namespace SQLite.FullyManaged.Engine");
                fileContent = fileContent.Replace("using Community.CsharpSqlite.SQLiteClient", "using SQLite.FullyManaged.Engine");
                await System.IO.File.WriteAllTextAsync(thisFile, fileContent, System.Text.Encoding.UTF8);
            } // Next thisFile 

        } // End Task ChangeNamespace 


        public static async System.Threading.Tasks.Task<int> Main(string[] args)
        {
            System.IO.Stream stream = null;
            int x = 132; int y = 164; int zoom = 8;
            string cs = "Data Source=D:\\username\\Downloads\\COR_switzerland.mbtiles;Version=3; Read Only=True;";
            cs = "Data Source=file:///D:/username/Downloads/COR_switzerland.mbtiles;Version=3;Read Only=True;";

            ConnectionStringBuilder csb = new ConnectionStringBuilder();
            // csb.ConnectionString = "Data Source=D:\\username\\Downloads\\COR_switzerland.mbtiles;Version=3; Read Only=True;"; // BAD
            // csb.Uri = "file:///D:/username/Downloads/COR_switzerland.mbtiles";
            // csb.FileName = "D:/username/Downloads/COR_switzerland.mbtiles";
            // csb.FileName = @"D:\aa bb;/COR_switzerland.mbtiles";
            // csb.FileName = @"D:\aa bb;äöü/COR_switzerland.mbtiles";
            // csb.FileName = @"COR_switzerland.mbtiles";
            csb.FileName = "D:/username/Documents/Visual Studio 2022/gitlab/VectorTileServer/VectorTileServer/App_Data/COR_switzerland.mbtiles";


            // Not a valid URI form (should be file:///D:/...), but S

            csb.Version = 3;
            csb.ReadOnly = true;
            cs = csb.ConnectionString;
            System.Console.WriteLine(cs);



            using (System.Data.Common.DbConnection conn = new Connection(cs))
            {
                if (conn.State != System.Data.ConnectionState.Open)
                    conn.Open();

                using (System.Data.Common.DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT * FROM tiles WHERE tile_column = {0} and tile_row = {1} and zoom_level = {2}", x, y, zoom);

                    using (System.Data.Common.DbDataReader reader = cmd.ExecuteReader())
                    {
                        int ord = reader.GetOrdinal("tile_data");

                        if (reader.Read())
                        {
                            stream = reader.GetStream(ord);
                        } // End if (reader.Read()) 

                    } // End Using reader 

                    if (conn.State != System.Data.ConnectionState.Open)
                        conn.Close();
                } // End Using cmd 

            } // End Using conn 

            if (stream != null)
            {
                byte[] buffer = StreamToByteArray(stream)!;
                string base64 = System.Convert.ToBase64String(buffer);
                System.Console.WriteLine("base64: " + base64);
            } // End if (stream != null) 

            System.Console.WriteLine(" --- Press any key to continue --- ");
            System.Console.ReadKey();
            return await System.Threading.Tasks.Task.FromResult(0);
        } // End Task Main 


    } // End Class Program 


} // End Namespace 
