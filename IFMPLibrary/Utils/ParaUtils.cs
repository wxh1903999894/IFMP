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

        //这个是真实的
        public static string Corpid = "dingeb41d8c01edf1eb835c2f4657eb6378f";
        public static string CorpSecret = "4swaPVoBBWCfWcIv_Jw2GxJRBCP16be_xqjlynxPEUyxBMJci77EQSe-bKp2KWFQ";
        public static string AgentID = "189695335";
        public static string DormitoryAgentID = "255478748";
        //@lADPBY0V4-OQbIDNAorNAoo
        public static string AlertImageID = "@lADPBY0V4-OQbIDNAorNAoo";
        public static string RemindImageID = "@lADPBY0V4-PVAtnNAa_NAoo";

        public static string DormitroyKey = "dingw2bbdgwnzczlvjej";
        public static string DormitroySecret = "LSvIJdx7hgK9QyNMqPE9xiz540zcVeenxWKc9TygGR8BkdpUoUCvkwzgS51eo4OB";

        public static string LoginAppID = "dingoaquq1djxkzqol3ujs";
        public static string LoginAppSecret = "pL-GpuwFsHvRj4L-y7yXz-uuCEN93buzAVkdYyIDcmmsUQ-5cZGCHhBLhw-Jtfc9";
        //测试
        //public static string AlertImageID = "@lADPBY0V49xzrPjNAorNAoo"
        
        //这个是测试的
        //public static string Corpid = "ding622179de41ce4b65";
        //public static string CorpSecret = "4swaPVoBBWCfWcIv_Jw2GxJRBCP16be_xqjlynxPEUyxBMJci77EQSe-bKp2KWFQ";
        //public static string AgentID = "189695335";
        //public static string DormitoryAgentID = "255478748";
        ////@lADPBY0V4-OQbIDNAorNAoo
        //public static string AlertImageID = "@lADPBY0V4-OQbIDNAorNAoo";
        //public static string RemindImageID = "@lADPBY0V4-PVAtnNAa_NAoo";

        //public static string DormitroyKey = "ding4mbc6dbi8uvswfjx";
        //public static string DormitroySecret = "IgAtSwRui8rgdKtnqBAMv8s2pGMKS1ftoL1B1H7GkFIyxCap1G0pf7Y46DxGDgJx";

        //public static string LoginAppID = "dingoaquq1djxkzqol3ujs";
        //public static string LoginAppSecret = "pL-GpuwFsHvRj4L-y7yXz-uuCEN93buzAVkdYyIDcmmsUQ-5cZGCHhBLhw-Jtfc9";
    }
}
