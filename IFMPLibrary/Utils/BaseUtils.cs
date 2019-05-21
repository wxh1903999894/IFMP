using IFMPLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Xml;

namespace IFMPLibrary.Utils
{
    public class BaseUtils
    {
        public string BuildPW(string username, string pw)
        {
            return GetMD5Hash(GetMD5Hash(username + pw) + "gkdz");
        }

        private string GetMD5Hash(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return "";
            }
            else
            {
                string output = string.Empty;
                MD5 md5Hasher = MD5.Create();
                byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
                StringBuilder result = new StringBuilder();

                foreach (byte @byte in data)
                {
                    result.Append(@byte.ToString("x2"));
                }
                return result.ToString();
            }
        }

        public static void DeepCopy(object _oldobject, object _object)
        {
            Type T = _object.GetType();
            PropertyInfo[] PI = T.GetProperties();
            for (int i = 0; i < PI.Length; i++)
            {
                PropertyInfo P = PI[i];
                P.SetValue(_oldobject, P.GetValue(_object));
            }
        }

        public DateTime InitDate(DateTime? Date, bool isbegin = true)
        {
            if (Date == null)
            {
                if (isbegin)
                {
                    Date = Convert.ToDateTime("00:00:00");
                }
                else
                {
                    Date = Convert.ToDateTime("23:59:59");
                }
            }
            else
            {
                Date = Convert.ToDateTime(Date.Value.Date.ToString("HH:mm:ss"));
            }
            return Date.Value;
        }

        public DateTime GetSelectDate(DateTime Date, bool isbegin = true)
        {
            if (Convert.ToDateTime(Date.ToString("yyyy-MM-dd")) != Date)
            {
                return Date;
            }
            else
            {
                if (isbegin)
                {
                    Date = Convert.ToDateTime(Date.ToString("yyyy-MM-dd") + " 00:00:00");
                }
                else
                {
                    Date = Convert.ToDateTime(Date.ToString("yyyy-MM-dd") + " 23:59:59");
                }
                return Date;
            }
        }

        public DateTime GetTodayDate(DateTime TargetDate, DateTime SourceDate)
        {
            DateTime returndate = Convert.ToDateTime(TargetDate.Date.AddHours(SourceDate.Hour).AddMinutes(SourceDate.Minute).AddSeconds(SourceDate.Second));

            return returndate;
        }

        public bool GetRegex(string data, RegexType RegexType, string array = null)
        {
            string pattern = "";
            switch (RegexType)
            {
                case RegexType.英文字母:
                    pattern = @"^[a-zA-Z]+$";
                    break;
                case RegexType.大写英文字母:
                    pattern = @"^[A-Z]+$";
                    break;
                case RegexType.小写英文字母:
                    pattern = @"^[a-z]+$";
                    break;
                case RegexType.整数:
                    pattern = @"^(-)?[0-9]+$";
                    break;
                case RegexType.实数:
                    pattern = @"^(-)?[0-9]+(\.[0-9]+)?$";
                    break;
                case RegexType.非负整数:
                    pattern = @"^[0-9]+$";
                    break;
                case RegexType.正整数:
                    pattern = @"^[1-9][0-9]*$";
                    break;
                case RegexType.有范围的数字:
                    try
                    {
                        double begindata = Convert.ToDouble(array.Split('|')[0]);
                        double enddata = Convert.ToDouble(array.Split('|')[1]);
                        if (begindata > enddata)
                        {
                            begindata = begindata + enddata;
                            enddata = begindata - enddata;
                            begindata = begindata - enddata;
                        }
                        if (Convert.ToDouble(data) >= begindata && Convert.ToDouble(data) <= enddata)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch
                    {
                        return false;
                    }
                case RegexType.度数:
                    pattern = @"^(-)?[0-9]+(\.[0-9]+)?[°]$";
                    break;
                case RegexType.有范围的度数:
                    try
                    {
                        double begindata = Convert.ToDouble(array.Split('|')[0].Split('°')[0]);
                        double enddata = Convert.ToDouble(array.Split('|')[1].Split('°')[0]);
                        if (begindata > enddata)
                        {
                            begindata = begindata + enddata;
                            enddata = begindata - enddata;
                            begindata = begindata - enddata;
                        }
                        if (Convert.ToDouble(data) >= begindata && Convert.ToDouble(data) <= enddata)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch
                    {
                        return false;
                    }
                case RegexType.手机号:
                    pattern = @"^[1][345789][0-9]{9}$";
                    break;
                case RegexType.邮箱:
                    pattern = @"^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.[a-zA-Z0-9]{2,6}$";
                    break;
                case RegexType.身份证号码:
                    pattern = @"^[1-9]\d{5}(18|19|([23]\d))\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{3}[0-9Xx]$";
                    break;
                case RegexType.邮政编码:
                    pattern = @"^[1-9][0-9]{5}$";
                    break;
                case RegexType.英文字母与数字:
                    pattern = @"^[a-zA-Z0-9]+$";
                    break;
                case RegexType.特殊的一组字符:
                    if (array.Length > 0)
                    {
                        foreach (char str in array)
                        {
                            if (str != '|')
                                pattern = pattern + str.ToString();
                        }
                        pattern = @"^[" + pattern + "]+$";
                    }
                    break;

                default:
                    return false;
            }

            return new Regex(pattern).Match(data).Success;
        }

        public List<object> GetEnumList(Type EnumType)
        {
            List<object> objectlist = new List<object>();
            foreach (int item in Enum.GetValues(EnumType))
            {
                objectlist.Add(new
                {
                    Name = Enum.GetName(EnumType, item),
                    ID = item
                });
            }
            return objectlist;
        }


        public static List<T> ConvertToModel<T>(DataTable dt) where T : new()
        {
            // 定义的集合      
            List<T> ts = new List<T>();

            // 获得此模型的类型     
            Type type = typeof(T);
            string tempName = "";

            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                // 获得此模型的公共属性        
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;  // 检查DataTable是否包含此列
                    if (dt.Columns.Contains(tempName))
                    {
                        if (!pi.CanWrite) continue;
                        object value = dr[tempName];
                        if (value != DBNull.Value)
                            pi.SetValue(t, value, null);
                    }
                }
                ts.Add(t);
            }
            return ts;
        }


        #region 导出excel
        /// <summary>
        /// 导出excel
        /// </summary>
        /// <param name="page"></param>
        /// <param name="fileName"></param>
        /// <param name="text"></param>
        public void ExportExcel(string fileName, string text)
        {
            try
            {
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Charset = "GB2312";//设置字符集，解决中文乱码问题
                HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
                HttpContext.Current.Response.Write("<meta http-equiv=Content-Type content=\"text/html;charset=GB2312\">");//解决乱码问题
                //解决HTTP头中文乱码问题
                string strExcelText = fileName + DateTime.Now.ToString("yyyyMMddHHmmss");//Excel显示的内容
                string strEncode = System.Web.HttpUtility.UrlEncode(strExcelText, System.Text.Encoding.UTF8);//进行编码的格式,用gb2312出错
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=\"" + strEncode + ".xls\"");//对保存标题进行编码
                HttpContext.Current.Response.ContentType = "application/vnd.xls";//设置输出格式
                HttpContext.Current.Response.Write(@"<html><head><style>.content {border-top: 1pt solid #2e5ac5; border-right: 1pt solid #9DB3C5;}
                                    .content th {height:30px;line-height:30px;font-weight:bold;border-bottom: 1pt solid #9DB3C5;border-left: 1pt solid #2e5ac5;background:#fff;}
                                    .content td {line-height:26px;color:#333;border-left: 1pt solid #9DB3C5;border-bottom: 1pt solid #9DB3C5;background:#fff;}
                                    .content tr {text-align:center;}
                                    </style></head><body>");
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                htw.WriteLine(text);//将数据输出
                HttpContext.Current.Response.Write(sw.ToString());
                HttpContext.Current.Response.Write("</body></html>");
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
            catch
            {
                return;
            }
        }
        #endregion


        public string GetXML(string path)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;//忽略文档里面的注释
                string filepath = "";

                filepath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ParaUtils.XmlPath);

                XmlReader reader = XmlReader.Create(filepath, settings);
                doc.Load(reader);
                reader.Close();
                XmlNode data = doc.SelectSingleNode(path);

                if (data != null)
                {
                    return data.InnerText;
                }
            }
            catch
            {

            }
            return null;
        }

        public void SetXML(string path, string message)
        {
            XmlDocument doc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;//忽略文档里面的注释
            string filepath = "";

            filepath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ParaUtils.XmlPath);

            XmlReader reader = XmlReader.Create(filepath, settings);
            doc.Load(reader);
            reader.Close();
            XmlNode data = doc.SelectSingleNode(path);

            if (data != null)
            {
                data.InnerText = message;
            }

            doc.Save(filepath);
        }

    }
}
