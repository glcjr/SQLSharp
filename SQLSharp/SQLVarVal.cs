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
    public class SQLVarVal:SQLVar
    {
        // private string Name;
        protected SQLVal Value = new SQLVal();
        //private string Value;
        //private bool IsNum;
        protected string Operand;
        //private string Constructor = "";
        /// <summary>
        /// Constructor to create a Variable Value connection variable
        /// </summary>
        /// <param name="nm">The name of the variable</param>
        /// <param name="val">The value of the variable</param>
        /// <param name="isnum">Whether it should be seen as a number</param>
        public SQLVarVal(string nm, string val, string operand = " = ", bool isnum = false) : base(nm)
        {
            
            Value = new SQLVal(val, isnum);
            //IsNum = isnum;
            Operand = operand;

        }
        public SQLVarVal(SQLVarVal value): base(value.GetName())
        {
            Value = new SQLVal(value.GetRawValue(), value.GetIsNum());
            Operand = value.GetOperand();           
        }
        /// <summary>
        /// Constructor to create a variable value connection instance
        /// </summary>
        /// <param name="field">A SQLiteField variable to be used to name the variable</param>
        /// <param name="val">The value of the new variable</param>
        /// <param name="isnum">Whether it should be considered a number</param>
        public SQLVarVal(SQLField field, string val, string operand = " = ", bool isnum=false):this(field.GetName(), val, operand, isnum)
        {

        }
        
       
        /// <summary>
        /// Returns the variable based on whether or not it is a number
        /// </summary>
        /// <returns></returns>
        public string GetValue()
        {
            return Value.GetValue();
            //if (IsNum)
            //    return Value;
            //else
            //    return "'" + Value + "'";
        }
        /// <summary>
        /// returns the value of the variable 
        /// </summary>
        /// <returns></returns>
        public string GetRawValue()
        {
            return Value.GetRawValue();
        }
        public bool GetIsNum()
        {
            return Value.GetIsNum();
        }
        public string GetOperand()
        {
            return Operand;
        }
        public SQLParam GetasParam(string prefix = "@")
        {
            return new SQLParam($"{prefix}{Name}", Value.GetRawValue());
        }
        //public string GetConstructor()
        //{
        //    return Constructor;
        //}
    }
}
