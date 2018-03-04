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
    public class SQLHavingVars
    {
        List<SQLHavingVar> Variables = new List<SQLHavingVar>();

        public SQLHavingVars()
        {
            Variables = new List<SQLHavingVar>();
        }
        public SQLHavingVars(List<SQLHavingVar> vars)
        {
            Variables = vars;
        }
        public SQLHavingVars(string function, string nm, string value, string operand = "= ", bool isnum = false)
        {
            Variables.Add(new SQLHavingVar(function,"", nm, value, operand, isnum));
        }
        public SQLHavingVars(string function, string con, string nm, string value, string operand = "= ", bool isnum = false)
        {
            Variables.Add(new SQLHavingVar(function, con, nm, value, operand, isnum));
        }
        public SQLHavingVar this[int index]
        {
            get
            {
                if ((index >= 0) && (Variables.Count > 0))
                {
                    if (index < Variables.Count)
                        return Variables[index];
                    else
                        return Variables[Variables.Count - 1];
                }
                else
                    return null;
            }
            set
            {
                try
                {
                    Variables[index] = value;
                }
                catch
                {
                    Variables.Add(value);
                }
            }
        }
        public void Add(string function, SQLVarVal field)
        {
            Variables.Add(new SQLHavingVar(function,"", field));
        }
        public void Add(string function,string con, SQLVarVal field)
        {
            Variables.Add(new SQLHavingVar(function, con, field));
        }
        public void Add(string function, string con, string field1, string field2, string operand = " = ", bool isnum = false)
        {
            Variables.Add(new SQLHavingVar(function, con, field1, field2, operand, isnum));
        }
        public void Add(string function, SQLWhereVar field)
        {
            Add(function, field.GetConstructor(), field.GetName(), field.GetValue(), field.GetOperand(), field.GetIsNum());
        }
        public string GetHavingStatements()
        {
            string temp = "";
            foreach (var h in Variables)
                temp += h.GetStatement();
            return temp;
        }
        public string GetHavingParamStatements(string prefix="@")
        {
            string temp = "";
            foreach (var h in Variables)
                temp += h.GetParamStatement(prefix);
            return temp;
        }
        public int Count()
        {
            return Variables.Count;
        }

        public string GetFieldNames(string separator = ", ")
        {
            string temp = "";
            foreach (var v in Variables)
                temp += v.GetName() + separator;
            return Utilities.RemoveLastComma(temp);
        }
        public string GetFieldValues(string separator = ", ")
        {
            string temp = "";
            foreach (var v in Variables)
                temp += v.GetRawValue() + separator;
            return Utilities.RemoveLastComma(temp);
        }
        public SQLParamList GetParameterList(string prefix = "@")
        {
            SQLParamList parlist = new SQLParamList();
            foreach (var v in Variables)
                parlist.Add(v.GetasParam(prefix));
            return parlist;
        }
        public string GetParameterListString(string prefix = "@", string separator = ", ")
        {
            string temp = "";
            foreach (var v in Variables)
                temp += $"{v.GetAsParameter(prefix)}{separator}";
            return Utilities.RemoveLastComma(temp);
        }
    }
}
