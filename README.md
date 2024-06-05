# SQLite.FullyManaged

A fully managed SQLite3 port to C# using NetStandard 2.0. <br />
This contains unsafe code. <br />
Probably not safe for production use. <br />
License as with SQLite and Community.CsharpSqlite and 2008 Noah B Hart SQLite3 port to C#-SQLite. <br />
No warranty whatsovever. 


**Note:** 

    Data Source=file:///D:/username/Downloads/COR_switzerland.mbtiles;Version=3;Read Only=True;

Note that the data source needs to start with file:///. <br />
But then again, you can't just do 

    new System.Uri("your_filename_here", System.UriKind.RelativeOrAbsolute).AbsoluteUri

because it's not actually an Uri - instead it's just the filename prefixed by "file:///". 


## Usage:

Add the package reference: 

    dotnet add package SQLite.FullyManaged 

And then use the library:     


    namespace TestFullyManaged
    {
    
    
        using Connection = SQLite.FullyManaged.SqliteConnection;
        using ConnectionStringBuilder = SQLite.FullyManaged.SqliteConnectionStringBuilder;
    
    
        internal class Program
        {
    
    
            public static async System.Threading.Tasks.Task<int> Main(string[] args)
            {
                System.IO.Stream stream = null;
                int x = 132; int y = 164; int zoom = 8;
                string cs = "Data Source=D:\\username\\Downloads\\switzerland.mbtiles;Version=3; Read Only=True;"; // this doesn't work 
                cs = "Data Source=file:///D:/username/Downloads/witzerland.mbtiles;Version=3;Read Only=True;"; // Note: no %20 for whitespace etc. - it's not an actual Uri
    
                ConnectionStringBuilder csb = new ConnectionStringBuilder();
                // csb.ConnectionString = "Data Source=D:\\username\\Downloads\\COR_switzerland.mbtiles;Version=3; Read Only=True;"; // BAD
                // csb.Uri = "file:///D:/username/Downloads/COR_switzerland.mbtiles";
                csb.FileName = "D:/username/Downloads/COR_switzerland.mbtiles";
    
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
                return await System.Threading.Tasks.Task.FromResult(0);
            } // End Task Main 

            
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

    
        } // End Class Program 
    
    
    } // End Namespace 

