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
    public class SQLJoinFields
    {
        private string Field1;
        private string Field2;
        private string Operand;
        private string JoinType;
        /// <summary>
        /// This constructur creates the instance of the Join Fields to be used in the SQLJoinTable class
        /// </summary>
        /// <param name="one">This is a field from a table that will be matched to a second field</param>
        /// <param name="two">This is the second field that will match the one to create the join</param>
        /// <param name="operand">This is an operand for the join. Usually = </param>
        public SQLJoinFields(string one, string two, string jointype="", string operand = " = ")
        {
            Field1 = one;
            Field2 = two;
            Operand = operand;
            JoinType = jointype;
        }
        /// <summary>
        /// Returns the sql for the fields that are being joined.
        /// </summary>
        /// <returns></returns>
        public string GetJoinSQL()
        {
            return Field1 + Operand + Field2;
        }
        public string GetJoinType()
        {
            return JoinType;
        }
        public void MakeLeftJoin()
        {
            JoinType = "LEFT";
        }
        public void MakeInnerJoin()
        {
            JoinType = "INNER";
        }
        public void MakeCrossJoin()
        {
            JoinType = "CROSS";
        }
        public void MakeFullOuterJoin()
        {
            JoinType = "FULL OUTER";
        }
    }
}
