using System;
using System.Collections.Generic;
using System.Text;

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
namespace SQLSharp
{
    public class SQLVals
    {
        List<SQLVal> Fields = new List<SQLVal>();

        public SQLVals()
        {
            Fields = new List<SQLVal>();
        }
        public SQLVals(SQLVal[] fields)
        {
            foreach (var f in fields)
                Fields.Add(f);
        }
        public void Add(SQLVals vars)
        {
            foreach (var v in vars.GetFields())
                Fields.Add(v);
        }
        public void Add(params string[] fields)
        {
            foreach (var f in fields)
                Fields.Add(new SQLVal(f));
        }
        public void Add(SQLVal field)
        {
            Fields.Add(field);
        }

        public List<SQLVal> GetFields()
        {
            return Fields;
        }
        public string GetFieldValues(string separator = ", ")
        {
            string temp = "";
            foreach (var v in Fields)
                temp += v.GetValue() + separator;
            return Utilities.RemoveLastComma(temp);
        }
        public SQLVal GetFieldValues(int index)
        {
            return Fields[index];
        }
        public int Count()
        {
            return Fields.Count;
        }
    }
}
