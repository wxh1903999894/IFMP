using IFMPLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IFMPLibrary.Utils
{
    public class ParaUtils
    {
        public const string SiteURL = "";
        public const string XmlPath = "Setting.xml";

        //这个是测试用的，是公司的钉钉账号
        public static string Corpid = "dingeb41d8c01edf1eb835c2f4657eb6378f";
        public static string CorpSecret = "4swaPVoBBWCfWcIv_Jw2GxJRBCP16be_xqjlynxPEUyxBMJci77EQSe-bKp2KWFQ";
        public static string AgentID = "189695335";
        //@lADPBY0V4-OQbIDNAorNAoo
        public static string AlertImageID = "@lADPBY0V4-OQbIDNAorNAoo";
        public static string RemindImageID = "@lADPBY0V4-PVAtnNAa_NAoo";
        //测试
        //public static string AlertImageID = "@lADPBY0V49xzrPjNAorNAoo";
    }
}
