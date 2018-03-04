##SQL Sharp

This library makes it possible to interact with SQL like most c# objects. It was originally going to only be SQLite but I split it off so that the SQL is separate in case it gets expanded into MYSQL and SQLServer.

It builds off the Microsft SQLite project: https://github.com/aspnet/Microsoft.Data.Sqlite

If you wanted to create a table

'''csharp
SQLCreateTable table = new SQLCreateTable("Customers");
            table.AddField("ID", "INTEGER", 0, false, true, true);
            table.AddField("Name", "VARCHAR", 30);
            table.AddField("Address", "VARCHAR", 30);
            table.AddField("City", "VARCHAR", 30);
            table.AddField("State", "VARCHAR", 2);
            table.AddField("Zip", "VARCHAR", 7);
			SQLiteCreate create = new SQLiteCreate(db, table);
'''

If you wanted to add multiple rows of data you could do so like this:

'''csharp
 SQLInsertTableMultiValues insertvalues = new SQLInsertTableMultiValues("Customers");
            insertvalues.SetFields("Name", "Address", "City", "State", "Zip");
            insertvalues.AddValues("Bob Moore", "123 No where", "NoneTown", "GH", "14538-3213");
            insertvalues.AddValues("Jack Friar", "342 Main St", "Hopetown", "JI", "46753-3234");
			SQLiteInsert insert = new SQLiteInsert(db, insertvalues);
'''

if you want to do a query
'''csharp
SQLiteSelect select = new SQLiteSelect(db);
            select.DoQuery("Customers", "*");
'''

To Access the data from the query
'''csharp
 SqliteDataReader reader = select.GetReader();
            while (reader.Read())
                Console.WriteLine($"{reader["ID"]} {reader["Name"]} {reader["Address"]} {reader["City"]} {reader["State"]} {reader["Zip"]}");
'''

If you find the library useful, a donation would be appeciated
Donate with this link: paypal.me/GColeJr
Please choose Friends and Family
