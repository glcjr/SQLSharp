using System;
using System.Collections.Generic;
using System.Text;
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
namespace SQLSharp
{
    public class SQLVarsVals
    {
        private List<SQLVarVal> Variables = new List<SQLVarVal>();

        /// <summary>
        /// Constructor for creating a variable with no variables in the list
        /// </summary>
        public SQLVarsVals()
        {
            Variables = new List<SQLVarVal>();
        }
        /// <summary>
        /// Creates a new variable to host variables that already exist in another
        /// </summary>
        /// <param name="vars">The variable to be duplicated</param>
        public SQLVarsVals(SQLVarsVals vars):this()
        {           
            foreach (var v in vars.GetVars())
                Variables.Add(v);
        }
        /// <summary>
        /// Creates a new instance and adds a variable to the list
        /// </summary>
        /// <param name="var">The variable to be added to the list</param>
        public SQLVarsVals(SQLVarVal var):this()
        {            
            Variables.Add(var);
        }
        public SQLVarVal this[int index]
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
        public string GetName(int index)
        {
            return Variables[index].GetName();
        }
        public string GetValue(int index)
        {
            return Variables[index].GetRawValue();
        }
        /// <summary>
        /// Creates anew instance and adds a variable with value to the list
        /// </summary>
        /// <param name="nm">The name of the variable</param>
        /// <param name="val">The value to be stored in the variable</param>
        /// <param name="isnum">Whether or not variable is a number</param>
        public SQLVarsVals(string nm, string val, string operand = " = ",bool isnum= false)
        {
            Variables.Add(new SQLVarVal(nm, val, operand, isnum));
        }
        /// <summary>
        /// Returns the list of variables to the calling method
        /// </summary>
        /// <returns></returns>
        public List<SQLVarVal> GetVars()
        {
            return Variables;
        }
        /// <summary>
        /// Adds a variable to the list
        /// </summary>
        /// <param name="var">The variable to be added to the list</param>
        public void Add(SQLVarVal var)
        {
            Variables.Add(var);
        }
        /// <summary>
        /// Adds a variable to the list of variables with values
        /// </summary>
        /// <param name="field">Variable that contains the name of the variable</param>
        /// <param name="val">The value to be stored in the varible</param>
        /// <param name="isnum">Whether or not variable is a number</param>
        public void Add(SQLField field, string val, string operand = " = ", bool isnum = false)
        {
            Variables.Add(new SQLVarVal(field, val, operand, isnum));
        }
        /// <summary>
        /// Adds a variable to the list of variables with values
        /// </summary>
        /// <param name="nm">The name of the variable</param>
        /// <param name="val">The value to be stored in the variable</param>
        /// <param name="isnum">Whether or not variable is a number</param>
        public void Add(string nm, string val, string operand = " = ",bool isnum=false)
        {
            Variables.Add(new SQLVarVal(nm, val, operand, isnum));
        }
        /// <summary>
        /// Adds another instancees variables to this list of variables with values
        /// </summary>
        /// <param name="vars">The variable list to be added to this one</param>
        public void Add(SQLVarsVals vars)
        {
            foreach (var v in vars.GetVars())
                Variables.Add(v);
        }
        
        /// <summary>
        /// Adds a list of variables with their values
        /// The lists must be the same size
        /// </summary>
        /// <param name="Fields">List of variable names to be added</param>
        /// <param name="Values">List of values that goes with the name</param>
        public void Add(List<string>Fields, List<string> Values)
        {
            if (Fields.Count==Values.Count)
            {
                for (int index = 0; index < Fields.Count; index++)
                    Variables.Add(new SQLVarVal(Fields[index], Values[index]));
            }
        }
        /// <summary>
        /// Removes a variable from the list
        /// </summary>
        /// <param name="var">The variable to be removed</param>
        public void Remove(SQLVarVal var)
        {
            Variables.Remove(var);
        }
        /// <summary>
        /// Removes a list of variables from the list
        /// </summary>
        /// <param name="vars">the variables to be removed</param>
        public void Remove(SQLVarsVals vars)
        {
            foreach (var v in vars.GetVars())
                Variables.Remove(v);
        }
       
        /// <summary>
        /// Creates a list of the fieldnames from the list of variables with values
        /// </summary>
        /// <param name="separator">Contains the char to use as a separater</param>
        /// <returns></returns>
        public string GetFieldNames(string separator=", ")
        {
            string temp = "";
            foreach (var v in Variables)
                temp += $"{v.GetName()}{separator}";
            return Utilities.RemoveLastComma(temp);
        }
        public string GetParamFieldNames(string prefix = "@", string separator=", ")
        {
            string temp = "";
            foreach (var v in Variables)
                temp += $"{prefix}{v.GetName()}{separator}";
            return Utilities.RemoveLastComma(temp);
        }
        /// <summary>
        /// Creates a list of the fieldvalues from the list of variables with values
        /// </summary>
        /// <param name="separator"></param>
        /// <returns></returns>
        public string GetFieldValues(string separator=", ")
        {
            string temp = "";
            foreach (var v in Variables)
                temp += $"{v.GetValue()}{separator}";
            return Utilities.RemoveLastComma(temp);
        }
        ///// <summary>
        ///// Gets the list of variables with values to use in a where statement
        ///// </summary>       
        ///// <returns>returns the where statement that compares the field to the value for a where statement</returns>
        //public string GetWhereFieldEqualsValue()
        //{
        //    string temp = "";
        //    foreach (var v in Variables)
        //        temp += v.GetConstructor() + v.GetName() + v.GetOperand() + "@" + v.GetValue();
        //    return Utilities.RemoveLastComma(temp);
        //}
        /// <summary>
        /// Gets the list of variables with values to use in a where statement
        /// </summary>
        /// <param name="separator"></param>
        /// <returns>returns the where statement that compares the field to the value for a where statement</returns>
        //public string GetFieldEqualsValue(string prefix = "@", string separator = ", ")
        //{
        //    string temp = "";
        //    foreach (var v in Variables)
        //        temp += $"{v.GetName()} {v.GetOperand()} {prefix}{v.GetRawValue()}{separator}";
        //    return Utilities.RemoveLastComma(temp);
        //}
        public string GetFieldOperandValueasStringorNum(string separator = ", ")
        {
            string temp = "";
            foreach (var v in Variables)
                temp += v.GetName() + v.GetOperand() + v.GetValue() + separator;
            return Utilities.RemoveLastComma(temp);
        }
        /// <summary>
        /// Returns the number of SQLiteVarVals that are in the list
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return Variables.Count;
        }
        public string GetFieldOperandParameter(string prefix = "@",string separator = ", ")
        {
            string temp = "";
            foreach (var v in Variables)
                temp += $"{v.GetName()} {v.GetOperand()} {prefix}{v.GetName()}{separator}";
            return Utilities.RemoveLastComma(temp);
        }
        public SQLParamList GetParameterList(string prefix="@")
        {
            SQLParamList parlist = new SQLParamList();
            foreach (var v in Variables)
                parlist.Add(v.GetasParam(prefix));
            return parlist;
        }

    }
}
