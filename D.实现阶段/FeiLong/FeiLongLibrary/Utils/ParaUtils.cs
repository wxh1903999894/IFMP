using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeiLongLibrary.Utils
{
    public class ParaUtils
    {
        private static string SqlStr = "Data Source=(local);Initial Catalog=FeiLong;User ID=sa;Password=123456;";
        public static string GetSql { get { return SqlStr; } }

        public const string WXUserAddSecret = "3oaEfvBDFJPz9dP5f7B58JJ9RppFrGudBcCTImzy1UU";

        public const string WXCorpid = "ww50a89c9a5fbcc901";

        public const string WXMessageSecret = "fEELZQOmP1Js1scahtwNT8KwalscO2WXC12JuG7n5ng";

        public const string WXMessageAgentID = "1000002";
    }
}
