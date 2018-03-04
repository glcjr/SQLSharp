using System;
using Microsoft.Data.Sqlite;
using System.Data.Common;
/*********************************************************************************************************************************
Copyright and Licensing Message

This code is copyright 2018 Gary Cole Jr. 

This code is licensed by Gary Cole to others under the GPLv.3 https://opensource.org/licenses/GPL-3.0 
If you find the code useful or just feel generous a donation is appreciated.

Donate with this link: paypal.me/GColeJr
Please choose Friends and Family

Alternative Licensing Options

If you prefer to license under the LGPL for a project, https://opensource.org/licenses/LGPL-3.0
Single Developers working on their own project can do so with a donation of $20 or more. 
Small and medium companies can do so with a donation of $50 or more. 
Corporations can do so with a donation of $1000 or more.


If you prefer to license under the MS-RL for a project, https://opensource.org/licenses/MS-RL
Single Developers working on their own project can do so with a donation of $40 or more. 
Small and medium companies can do so with a donation of $100 or more.
Corporations can do so with a donation of $2000 or more.


if you prefer to license under the MS-PL for a project, https://opensource.org/licenses/MS-PL
Single Developers working on their own project can do so with a donation of $1000 or more. 
Small and medium companies can do so with a donation of $2000 or more.
Corporations can do so with a donation of $10000 or more.


If you use the code in more than one project, a separate license is required for each project.


Any modifications to this code must retain this message. 
*************************************************************************************************************************************/
namespace SQLiteSharp
{    
    public class SQLiteConnection
    {        
        SqliteConnection dbc;       
        
        /// <summary>
        /// This constructor establishes an in memory SQLite database connection
        /// </summary>
        public SQLiteConnection()   
        {
            string connectionString = "Data Source=:memory:";
            dbc = new SqliteConnection(connectionString);
        }
        /// <summary>
        /// This constructor establishes a SQLite database on the file system 
        /// </summary>
        /// <param name="filename">The name of the SQLite datafile to store the db for permanent access and updating</param>
        public SQLiteConnection(string filename)
        {
            string connectionString = $"Data Source={filename}";
            dbc = new SqliteConnection(connectionString);
        }
        /// <summary>
        /// //This returns the connection to the SQLite database for writing and reading to it
        /// </summary>
        /// <returns>DBCOnnection to the SQLite db</returns>
        public SqliteConnection GetConnection()
        {      
           
            dbc.Open();
            return dbc;
        }
        /// <summary>
        /// //This closes the connection to the SQLite database
        /// </summary>
        public void Close()
        {
            dbc.Close();
        }
        /// <summary>
        /// to do save in memory to file
        /// </summary>
        /// <param name="fileName"></param>
        //public void save(string fileName)
        //{
           
        //}
        
    }
}
