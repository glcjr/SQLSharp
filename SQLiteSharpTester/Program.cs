using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.Sqlite;
using SQLiteSharp;
using SQLSharp;

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
namespace SQLiteSharpTester
{
    class Program
    {
        static void Main(string[] args)
        {
            //create a sqiite data connection
            SQLiteConnection db = new SQLiteConnection();

            //create a table
            SQLCreateTable table = new SQLCreateTable("Customers");
            table.AddField("ID", "INTEGER", 0, false, true, true);
            table.AddField("Name", "VARCHAR", 30);
            table.AddField("Address", "VARCHAR", 30);
            table.AddField("City", "VARCHAR", 30);
            table.AddField("State", "VARCHAR", 2);
            table.AddField("Zip", "VARCHAR", 7);
            //See the SQL that will be generated
            Console.WriteLine("Creating Customers Table");            
            //Perform the Create
            SQLiteCreate create = new SQLiteCreate(db, table);
            Console.WriteLine(create.GetSuccess());
            Console.WriteLine(create.GetCommmandPerformed());

            //Create values to insert into the Customers table
            SQLInsertTableMultiValues insertvalues = new SQLInsertTableMultiValues("Customers");
            insertvalues.SetFields("Name", "Address", "City", "State", "Zip");
            insertvalues.AddValues("Bob Moore", "123 No where", "NoneTown", "GH", "14538-3213");
            insertvalues.AddValues("Jack Friar", "342 Main St", "Hopetown", "JI", "46753-3234");
            
            // See the SQL that will be generated
            Console.WriteLine("Inserting Values into the Customers Table");        
           
            //Perform the insert into the SQL db
            SQLiteInsert insert = new SQLiteInsert(db, insertvalues);
            Console.WriteLine(insert.GetSuccess());
            Console.WriteLine(insert.GetCommmandPerformed());
            Console.WriteLine(insert.GetSQLPerfomed());

            //Select all rows in the Customers table
            SQLiteSelect select = new SQLiteSelect(db);
            select.DoQuery("Customers", "*");

            // Access the fields selected in the query
            Console.WriteLine($"Fields: {select.GetFields()}");

            //Access the results of the query
            SqliteDataReader reader = select.GetReader();
            while (reader.Read())
                Console.WriteLine($"{reader["ID"]} {reader["Name"]} {reader["Address"]} {reader["City"]} {reader["State"]} {reader["Zip"]}");
            
            //Get the number of rows in the query
            Console.WriteLine($"Total Rows: {select.Count()}");

            //insert a single row with SQLInsertTable
            SQLInsertTable inserttable = new SQLInsertTable("Customers");
            inserttable.Add("Name", "Jack Waler");
            inserttable.Add("Address", "545 Tracker Street");
            inserttable.Add("City", "Walpum");
            inserttable.Add("State", "BO");
            inserttable.Add("Zip", "13422-222");
            Console.WriteLine("Inserting another record into the customers table");         
            insert = new SQLiteInsert(db, inserttable);

            //Insert a single row more compact with SQLInsertTable
            inserttable = new SQLInsertTable("Customers");
            inserttable.Add(new SQLVarVal("Name", "Henry Thomas"), new SQLVarVal("Address", "34 Downing Street"), new SQLVarVal("City", "Kiltown"), new SQLVarVal("State", "YI"), new SQLVarVal("Zip", "45430-2232"));
            Console.WriteLine("Inserting another record into the customers table");
            insert = new SQLiteInsert(db, inserttable);

            //Get result in the form of a DataTable
            DataTable dt = select.GetTable();
            Console.WriteLine(dt.Rows.Count);

            //update a field with SQLUpdateTable and SQLiteUpdate
            SQLUpdateTable updatetable = new SQLUpdateTable("Customers", new SQLVarVal("State", "HI"), new SQLWhereVar("State", "JI"));
            Console.WriteLine("Changing state in customers table");
            Console.WriteLine(updatetable.GetSQL());            
            SQLiteUpdate update = new SQLiteUpdate(db, updatetable);

            //passing normal SQL rather than building it in code is possible
            SQLiteSelect select2 = new SQLiteSelect(db);
            select2.DoQuery("Select ID, Name, State FROM Customers");

            //Check fields of the query
            Console.WriteLine($"Fields: {select2.GetFields()}");
            Console.WriteLine();
            //Access the results of the query
            reader = select2.GetReader();

            //Display the results
            while (reader.Read())
                Console.WriteLine($"{reader["ID"]} {reader["Name"]} {reader["State"]}");

            //get the row count
            Console.WriteLine($"Total Rows: {select2.Count()}");

            //How to do a Special function with Select like Count, or Sum, or ect...
            SQLSpecialSelectTable selecttable = new SQLSpecialSelectTable("Customers");
            selecttable.AddSelectField("Name");
            selecttable.AddWhereField(new SQLWhereVar("","Name", "B%", "LIKE"));
            selecttable.MakeCount();
            select2 = new SQLiteSelect(db);
            Console.WriteLine(selecttable.GetSql());
            select2.DoQuery(selecttable);
            reader = select2.GetReader();
            reader.Read();
            Console.WriteLine($"The number of matching records is {reader[0]}");

            //Can also get the count with this method
            SQLSelectTable selecttable2 = new SQLSelectTable("Customers");
            selecttable2.AddSelectField("Name");
            selecttable2.AddWhereField(new SQLWhereVar("","Name", "B%", "LIKE"));
            Console.WriteLine(selecttable2.GetSql());
            select2 = new SQLiteSelect(db);
            select2.DoQuery(selecttable2);
            int count = select2.Count();
            Console.WriteLine($"The number of matching records is {count}");

            //SQLSelectIntoTable tab = new SQLSelectIntoTable(selecttable2, "destin", "extdb");
            //Console.WriteLine(tab.GetSql());

            //close the db connection
            db.Close();
        }
    }
}
