using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using FeiLongLibrary.DAO;
using FeiLongLibrary.Enums;
using System.Security.Cryptography;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Data;

namespace FeiLongLibrary.Utils
{
    /// <summary>
    /// Api 返回的结果
    /// </summary>
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

        public bool GetRegex(string data, RegexType RegexType, char[] array = null)
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

    }

}