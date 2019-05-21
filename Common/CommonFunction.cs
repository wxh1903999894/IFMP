/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年07月11日 10时4分16秒
** 描    述:      公共方法类
** 修 改 人:      
** 修改日期:    
** 修改说明: 
**-----------------------------------------------------------------
******************************************************************/
using System;
using System.Text;
using System.Data;
using System.IO;
using System.Web;
using NPOI.HPSF;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using System.Security.Cryptography;
using System.Web.UI.WebControls;

using GK.IFMP.DAL;
using GK.IFMP.Entities;
using System.Configuration;
using System.Net;


namespace GK.IFMP.Common
{
    public class CommonFunction
    {
        public static DepartmentDAL departDAL = new DepartmentDAL();


        #region SQL关键字
        /// <summary>
        /// SQL关键字
        /// </summary>
        /// <param name="charString">关键字</param>
        /// <returns></returns>
        public static string GetCommoneString(string charString)
        {
            if (charString.IndexOf('%') >= 0)
            {
                return charString.Replace("%", "[%]").Replace("[", "[[]").Replace("_", "[_]");
            }

            return charString;
        }
        #endregion

        #region 检查枚举绑定
        /// <summary>
        /// 检查枚举绑定
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="checkedValue">被检查的值</param>
        /// <returns></returns>
        public static string CheckEnum<T>(object checkedValue)
        {
            try
            {
                return Enum.Parse(typeof(T), checkedValue.ToString().Trim()).ToString();
            }
            catch
            {
                return string.Empty;
            }
        }


        /// <summary>
        /// 绑定下拉列表枚举数据源
        /// </summary>
        /// <typeparam name="T">枚举源</typeparam>
        /// <param name="ddlControl">被绑定的控件</param>
        public static void BindEnum<T>(DropDownList ddlControl, string firstvalue)
        {
            Type enumSource = typeof(T);
            if (firstvalue == "-99")
            {

            }
            else
            {
                ddlControl.Items.Add(new ListItem("--请选择--", firstvalue));
            }
            foreach (int itemValue in Enum.GetValues(enumSource))
            {
                ddlControl.Items.Add(new ListItem(Enum.GetName(enumSource, itemValue), itemValue.ToString()));
            }
        }

        /// <summary>
        /// 绑定单选列表枚举数据源
        /// </summary>
        /// <typeparam name="T">枚举源</typeparam>
        /// <param name="rblControl">被绑定的控件</param>
        public static void BindEnum<T>(RadioButtonList rblControl)
        {
            Type enumSource = typeof(T);

            foreach (int itemValue in Enum.GetValues(enumSource))
            {
                rblControl.Items.Add(new ListItem(Enum.GetName(enumSource, itemValue), itemValue.ToString()));
            }
        }
        #endregion

        #region 加密、解密
        private static readonly string KEY = "gk.sismp";

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="encryptString">被加密的字符串</param>
        /// <returns></returns>
        public static string Encrypt(string encryptString)
        {
            try
            {
                using (MemoryStream encryptStream = new MemoryStream())
                {
                    DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();

                    desProvider.IV = ASCIIEncoding.UTF8.GetBytes(KEY);
                    desProvider.Key = ASCIIEncoding.UTF8.GetBytes(KEY);

                    using (CryptoStream cryptoStream = new CryptoStream(encryptStream, desProvider.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        byte[] encryptBuffer = Encoding.UTF8.GetBytes(encryptString);

                        cryptoStream.Write(encryptBuffer, 0, encryptBuffer.Length);
                        cryptoStream.FlushFinalBlock();
                        cryptoStream.Close();
                    }

                    StringBuilder encryptBuilder = new StringBuilder();

                    foreach (byte encryptByte in encryptStream.ToArray())
                    {
                        encryptBuilder.AppendFormat("{0:X2}", encryptByte);
                    }

                    encryptStream.Close();
                    return encryptBuilder.ToString();
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="decryptString">被解密的字符串</param>
        /// <returns></returns>
        public static string Decrypt(string decryptString)
        {
            try
            {
                using (MemoryStream decryptStream = new MemoryStream())
                {
                    DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();

                    desProvider.IV = ASCIIEncoding.ASCII.GetBytes(KEY);
                    desProvider.Key = ASCIIEncoding.ASCII.GetBytes(KEY);

                    using (CryptoStream cryptoStream = new CryptoStream(decryptStream, desProvider.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        int decryptStringCount = decryptString.Length / 2;
                        byte[] decryptBuffer = new byte[decryptStringCount];

                        for (int x = 0; x < decryptStringCount; x++)
                        {
                            int i = (Convert.ToInt32(decryptString.Substring(x * 2, 2), 16));
                            decryptBuffer[x] = (byte)i;
                        }

                        cryptoStream.Write(decryptBuffer, 0, decryptBuffer.Length);
                        cryptoStream.FlushFinalBlock();
                        cryptoStream.Close();
                    }

                    decryptStream.Close();
                    return Encoding.UTF8.GetString(decryptStream.ToArray());
                }
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion

        #region 绑定下拉框数据源（含请选择）
        /// <summary>
        /// 绑定下拉框数据源
        /// </summary>
        /// <param name="ddlControl"></param>
        /// <param name="dv">数据源</param>
        /// <param name="valuename">绑定值</param>
        /// <param name="textname">显示字段</param>
        /// <param name="firstvalue">请选择默认值</param>
        public static void DDlTypeBind(DropDownList ddlControl, DataTable dv, string valuename, string textname, string firstvalue)
        {
            ddlControl.Items.Clear();
            if (firstvalue != "-999")
            {
                ddlControl.Items.Add(new ListItem("--请选择--", firstvalue));
            }
            if (dv != null && dv.Rows.Count > 0)
            {
                foreach (DataRow row in dv.Rows)
                {
                    //ddlControl.Items.Add(new ListItem(row[textname].ToString().Length > 10 ? row[textname].ToString().Substring(0, 10).TrimEnd(',') + "..." : row[textname].ToString(), row[valuename].ToString()));
                    ddlControl.Items.Add(new ListItem(row[textname].ToString(), row[valuename].ToString()));
                }
            }

        }


        /// <summary>
        /// 绑定多级基础数据
        /// </summary>
        /// <param name="ddlControl"></param>
        /// <param name="flag"></param>
        //public static void DDlPidTypeBind(DropDownList ddlControl, int flag)
        //{
        //    //DataTable dv = sysDataDAL.GetList((int)CommonEnum.Deleted.未删除, flag, -2);
        //    //ddlControl.Items.Clear();
        //    //ddlControl.Items.Add(new ListItem("--请选择--", "-2"));

        //    //if (dv != null && dv.Rows.Count > 0)
        //    //{
        //    //    foreach (DataRow row in dv.Rows)
        //    //    {
        //    //        //ddlControl.Items.Add(new ListItem(row["DataName"].ToString().Length > 10 ? row["DataName"].ToString().Substring(0, 10).TrimEnd(',') + "..." : row["DataName"].ToString(), row["SDID"].ToString()));
        //    //        ddlControl.Items.Add(new ListItem(row["DataName"].ToString(), row["SDID"].ToString()));
        //    //        DataTable dv1 = sysDataDAL.GetList((int)CommonEnum.Deleted.未删除, flag, Convert.ToInt32(row["SDID"].ToString()));
        //    //        if (dv1 != null && dv1.Rows.Count > 0)
        //    //        {
        //    //            foreach (DataRow row1 in dv1.Rows)
        //    //            {
        //    //                ddlControl.Items.Add(new ListItem("--" + row1["DataName"].ToString(), row1["SDID"].ToString()));
        //    //            }
        //    //        }
        //    //    }
        //    //}
        //}


        /// <summary>
        /// 绑定部门数据
        /// </summary>
        /// <param name="ddlControl"></param>
        /// <param name="flag"></param>
        public static void DepInfoBind(DropDownList ddlControl, int deptype)
        {
            DataTable dv = departDAL.GetTable((int)CommonEnum.Deleted.未删除, -1, deptype);
            ddlControl.Items.Clear();
            ddlControl.Items.Add(new ListItem("--请选择--", "-2"));

            if (dv != null && dv.Rows.Count > 0)
            {
                foreach (DataRow row in dv.Rows)
                {
                    ddlControl.Items.Add(new ListItem(row["DepName"].ToString(), row["DID"].ToString()));
                    DataTable dv1 = departDAL.GetTable((int)CommonEnum.Deleted.未删除, Convert.ToInt32(row["DID"].ToString()), deptype);
                    if (dv1 != null && dv1.Rows.Count > 0)
                    {
                        foreach (DataRow row1 in dv1.Rows)
                        {
                            ddlControl.Items.Add(new ListItem("--" + row1["DepName"].ToString(), row1["DID"].ToString()));
                        }
                    }
                }
            }
        }
        #endregion

        #region 使用npoi导出excel
        #region Excel导出方法 ExportByWeb(dtSource,strHeaderText,strFileName)
        /// <summary>
        /// Excel导出方法 ExportByWeb()
        /// </summary>
        /// <param name="dtSource">DataTable数据源</param>
        /// <param name="strHeaderText">Excel表头文本（例如：车辆列表）</param>
        /// <param name="strFileName">Excel文件名（例如：车辆列表.xls）</param>
        public static void ExportByWeb(DataTable dtSource, string strHeaderText, string strFileName)
        {
            HttpContext curContext = HttpContext.Current;
            // 设置编码和附件格式
            curContext.Response.ContentType = "application/ms-excel";
            curContext.Response.ContentEncoding = Encoding.UTF8;
            curContext.Response.Charset = "";
            curContext.Response.AppendHeader("Content-Disposition",
                "attachment;filename=" + HttpUtility.UrlEncode(strFileName, Encoding.UTF8));
            //调用导出具体方法Export()
            curContext.Response.BinaryWrite(Export(dtSource, strHeaderText).GetBuffer());
            curContext.Response.End();
        }
        #endregion

        #region DataTable导出到Excel文件 Export(dtSource,strHeaderText,strFileName)
        /// <summary>
        /// DataTable导出到Excel文件 Export()
        /// </summary>
        /// <param name="dtSource">DataTable数据源</param>
        /// <param name="strHeaderText">Excel表头文本（例如：车辆列表）</param>
        /// <param name="strFileName">保存位置</param>
        public static void Export(DataTable dtSource, string strHeaderText, string strFileName)
        {
            using (MemoryStream ms = Export(dtSource, strHeaderText))
            {
                using (FileStream fs = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
                {
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
            }
        }
        #endregion

        #region DataTable导出到Excel的MemoryStream Export(dtSource,strHeaderText)
        /// <summary>
        /// DataTable导出到Excel的MemoryStream Export()
        /// </summary>
        /// <param name="dtSource">DataTable数据源</param>
        /// <param name="strHeaderText">Excel表头文本（例如：车辆列表）</param>
        public static MemoryStream Export(DataTable dtSource, string strHeaderText)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();

            #region 右击文件 属性信息
            {
                DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                dsi.Company = "NPOI";
                workbook.DocumentSummaryInformation = dsi;

                SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
                si.Author = "文件作者信息"; //填加xls文件作者信息
                si.ApplicationName = "创建程序信息"; //填加xls文件创建程序信息
                si.LastAuthor = "最后保存者信息"; //填加xls文件最后保存者信息
                si.Comments = "作者信息"; //填加xls文件作者信息
                si.Title = "标题信息"; //填加xls文件标题信息
                si.Subject = "主题信息";//填加文件主题信息
                si.CreateDateTime = System.DateTime.Now;
                workbook.SummaryInformation = si;
            }
            #endregion

            ICellStyle dateStyle = workbook.CreateCellStyle();
            IDataFormat format = workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

            //取得列宽
            int[] arrColWidth = new int[dtSource.Columns.Count];
            foreach (DataColumn item in dtSource.Columns)
            {
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length + 5;
            }
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                for (int j = 0; j < dtSource.Columns.Count; j++)
                {
                    int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
                    if (intTemp > arrColWidth[j])
                    {
                        arrColWidth[j] = intTemp + 3;
                    }
                }
            }
            int rowIndex = 0;
            foreach (DataRow row in dtSource.Rows)
            {
                #region 新建表，填充表头，填充列头，样式
                if (rowIndex == 65535 || rowIndex == 0)
                {
                    if (rowIndex != 0)
                    {
                        sheet = workbook.CreateSheet();
                    }

                    #region 表头及样式
                    if (strHeaderText != "")
                    {
                        IRow headerRow = sheet.CreateRow(0);
                        headerRow.HeightInPoints = 25;
                        headerRow.CreateCell(0).SetCellValue(strHeaderText);

                        ICellStyle headStyle = workbook.CreateCellStyle();
                        headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER; // ------------------
                        IFont font = workbook.CreateFont();
                        font.FontHeightInPoints = 20;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);
                        headerRow.GetCell(0).CellStyle = headStyle;
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, dtSource.Columns.Count - 1)); // ------------------
                    }
                    #endregion

                    #region 列头及样式
                    {
                        IRow headerRow;
                        if (strHeaderText != "")
                            headerRow = sheet.CreateRow(1);
                        else
                            headerRow = sheet.CreateRow(0);
                        ICellStyle headStyle = workbook.CreateCellStyle();
                        headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER; // ------------------
                        IFont font = workbook.CreateFont();
                        font.FontHeightInPoints = 12;
                        font.FontName = "宋体";
                        font.Boldweight = 700;
                        headStyle.SetFont(font);
                        foreach (DataColumn column in dtSource.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle;

                            //设置列宽
                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
                        }
                    }


                    #endregion
                    if (strHeaderText != "")
                        rowIndex = 2;
                    else
                        rowIndex = 1;
                }
                #endregion

                #region 填充内容
                IRow dataRow = sheet.CreateRow(rowIndex);
                ICellStyle contentStyle = workbook.CreateCellStyle();
                {
                    contentStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER; // ------------------
                    IFont font = workbook.CreateFont();
                    font.FontHeightInPoints = 12;
                    font.FontName = "宋体";
                    //  font.Boldweight = 700;
                    contentStyle.SetFont(font);
                }
                foreach (DataColumn column in dtSource.Columns)
                {
                    ICell newCell = dataRow.CreateCell(column.Ordinal);
                    newCell.CellStyle = contentStyle;
                    string drValue = row[column].ToString();

                    switch (column.DataType.ToString())
                    {
                        case "System.String"://字符串类型
                            newCell.SetCellValue(drValue);
                            break;
                        case "System.DateTime"://日期类型
                            System.DateTime dateV;
                            System.DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);

                            newCell.CellStyle = dateStyle;//格式化显示
                            break;
                        case "System.Boolean"://布尔型
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;
                        case "System.Int16"://整型
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal"://浮点型
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;
                        case "System.DBNull"://空值处理
                            newCell.SetCellValue("");
                            break;
                        default:
                            newCell.SetCellValue("");
                            break;
                    }
                }
                #endregion

                rowIndex++;
            }
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                ms.Dispose();
                return ms;
            }
        }
        #endregion

        #region 读取excel ,默认第一行为标头Import()
        /// <summary>
        /// 读取excel ,默认第一行为标头
        /// </summary>
        /// <param name="strFileName">excel文档路径</param>
        /// <returns></returns>
        public static DataTable Import(string strFileName)
        {
            DataTable dt = new DataTable();

            HSSFWorkbook hssfworkbook;
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }
            ISheet sheet = hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;

            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = headerRow.GetCell(j);
                dt.Columns.Add(cell.ToString());
            }

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = dt.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();
                }

                dt.Rows.Add(dataRow);
            }
            return dt;
        }
        #endregion
        #endregion

        #region 下载
        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool UpLoadFunciotn(string path, string filename)
        {
            try
            {
                string filekind = Path.GetExtension(path);
                string name = filename + filekind;
                //提供下载的文件并且编码
                string fileName = HttpContext.Current.Server.UrlEncode(name);
                fileName = fileName.Replace("+", "%20");
                string filePath = HttpContext.Current.Server.MapPath(path);
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    byte[] bytes = new byte[(int)fs.Length];
                    fs.Read(bytes, 0, bytes.Length);
                    fs.Close();
                    HttpContext.Current.Response.ContentType = "application/octet-stream";
                    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                    HttpContext.Current.Response.BinaryWrite(bytes);
                    HttpContext.Current.Response.Flush();
                    //HttpContext.Current.Response.End();
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region 删除上传的文件
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path">路径</param>
        public static void delfile(string path)
        {
            string[] upfile = path.Split('$');
            for (int i = 0; i < upfile.Length; i++)
            {
                if (System.IO.File.Exists(upfile[i].ToString()))
                {
                    System.IO.File.Delete(upfile[i].ToString());
                }
            }
        }
        #endregion

        #region 上传文件
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file">文件名</param>
        /// <param name="mark">标识</param>
        /// <param name="filepath">文件存放路径</param>
        /// <returns></returns>
        //public static AccessoryEntity upfile(int start, int end, HiddenField hfcount, string uploadpath)
        //{
        //    int upsize = 140000000;
        //    try
        //    {
        //        upsize = Convert.ToInt32(ConfigurationManager.AppSettings["upsize"].ToString());
        //    }
        //    catch (Exception) { }

        //    //清空隐藏控件的值，用于存放路径，以便数据保存失败时删除文件
        //    if (hfcount != null)
        //    {
        //        hfcount.Value = "";
        //    }
        //    //设置文件夹的名称
        //    string attaname = "";
        //    string attaurl = "";
        //    //设置上传路径
        //    //string pname = "~/webupload/" + System.DateTime.Now.ToString("yyyyMM") + "/";
        //    string pname = "/webupload/" + uploadpath + "/";
        //    string path = System.Web.HttpContext.Current.Server.MapPath(pname);

        //    AccessoryEntity attainfo = null;

        //    //判断上传文件夹是否存在，若不存在，则创建
        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }
        //    //遍历页面中的上传控件
        //    HttpFileCollection files = HttpContext.Current.Request.Files;
        //    for (int i = start; i < end; i++)
        //    {
        //        HttpPostedFile postedFile = files[i];
        //        if (postedFile.FileName != "")
        //        {
        //            string a = Path.GetFileName(postedFile.FileName);

        //            string[] name = postedFile.FileName.ToString().Split('\\');
        //            string[] filename = name[name.Length - 1].ToString().Split('.');
        //            //获取上传文件的名称
        //            string oglname = filename[0].ToString();
        //            for (int o = 1; o < filename.Length - 1; o++)
        //            {
        //                oglname += "." + filename[o].ToString();
        //            }
        //            attaname += oglname + ",";
        //            //为上传的文件设置新的名称，防止重名
        //            string newname = System.DateTime.Now.ToString("yyyyMMddHHmmssffff") + i.ToString();
        //            newname = newname + "." + filename[filename.Length - 1];
        //            //设置完整的文件上传路径
        //            string filepath = path + newname;

        //            if (postedFile.ContentLength < upsize)
        //            {
        //                postedFile.SaveAs(filepath);
        //                if (hfcount != null)
        //                {
        //                    hfcount.Value += filepath + "$";
        //                }

        //                int j = filepath.IndexOf("webupload");
        //                string str = filepath.Substring(j - 1);
        //                attaurl += str + ",";
        //            }
        //            else
        //            {
        //                attainfo = new AccessoryEntity();
        //                attainfo.AccessID = "-2";
        //                attainfo.AccessName = "上传失败，上传文件不能大于" + upsize / 1000000 + "M！";
        //                return attainfo;
        //            }
        //        }
        //    }

        //    attaname = (attaname + "$").Replace(",$", "");
        //    attaurl = (attaurl + "$").Replace(",$", "");
        //    if (attaname != "$" && attaurl != "$" && attainfo == null)
        //    {
        //        attainfo = new AccessoryEntity(attaname, attaurl);
        //    }
        //    else
        //    {
        //        attainfo = new AccessoryEntity();
        //    }
        //    return attainfo;
        //}

        //public static AccessoryEntity upfile(int start, int end, HiddenField hfcount)
        //{
        //    int upsize = 30000000;
        //    try
        //    {
        //        upsize = Convert.ToInt32(ConfigurationManager.AppSettings["upsize"].ToString());
        //    }
        //    catch (Exception) { }

        //    //清空隐藏控件的值，用于存放路径，以便数据保存失败时删除文件
        //    if (hfcount != null)
        //    {
        //        hfcount.Value = "";
        //    }
        //    //设置文件夹的名称
        //    string attaname = "";
        //    string attaurl = "";
        //    //设置上传路径
        //    string pname = "/uploadfile/" + System.DateTime.Now.ToString("yyyyMM") + "/";
        //    // string pname = "/webupload/" + uploadpath + "/";
        //    string path = System.Web.HttpContext.Current.Server.MapPath(pname);

        //    AccessoryEntity attainfo = null;

        //    //判断上传文件夹是否存在，若不存在，则创建
        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }
        //    //遍历页面中的上传控件
        //    HttpFileCollection files = HttpContext.Current.Request.Files;
        //    for (int i = start; i < end; i++)
        //    {
        //        HttpPostedFile postedFile = files[i];
        //        if (postedFile.FileName != "")
        //        {
        //            string a = Path.GetFileName(postedFile.FileName);

        //            string[] name = postedFile.FileName.ToString().Split('\\');
        //            string[] filename = name[name.Length - 1].ToString().Split('.');
        //            //获取上传文件的名称
        //            string oglname = filename[0].ToString();
        //            for (int o = 1; o < filename.Length - 1; o++)
        //            {
        //                oglname += "." + filename[o].ToString();
        //            }
        //            attaname += oglname + ",";
        //            //为上传的文件设置新的名称，防止重名
        //            string newname = System.DateTime.Now.ToString("yyyyMMddHHmmssffff") + i.ToString();
        //            newname = newname + "." + filename[filename.Length - 1];
        //            //设置完整的文件上传路径
        //            string filepath = path + newname;

        //            if (postedFile.ContentLength < upsize)
        //            {
        //                postedFile.SaveAs(filepath);
        //                if (hfcount != null)
        //                {
        //                    hfcount.Value += filepath + "$";
        //                }

        //                int j = filepath.IndexOf("uploadfile");
        //                string str = filepath.Substring(j - 1);
        //                attaurl += str + ",";
        //            }
        //            else
        //            {
        //                attainfo = new AccessoryEntity();
        //                attainfo.AccessID = "-2";
        //                attainfo.AccessName = "上传失败，上传文件不能大于" + upsize / 1000000 + "M！";
        //                return attainfo;
        //            }
        //        }
        //    }

        //    attaname = (attaname + "$").Replace(",$", "");
        //    attaurl = (attaurl + "$").Replace(",$", "");
        //    if (attaname != "$" && attaurl != "$" && attainfo == null)
        //    {
        //        attainfo = new AccessoryEntity(attaname, attaurl);
        //    }
        //    else
        //    {
        //        attainfo = new AccessoryEntity();
        //    }
        //    return attainfo;
        //}

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file">文件名</param>
        /// <param name="mark">标识</param>
        /// <returns></returns>
        //public static AccessoryEntity upImage(int start, int end, HiddenField hfcount)
        //{
        //    int upsize = 30000000;
        //    try
        //    {
        //        upsize = Convert.ToInt32(ConfigurationManager.AppSettings["upimgsize"].ToString());
        //    }
        //    catch (Exception) { }

        //    //清空隐藏控件的值，用于存放路径，以便数据保存失败时删除文件
        //    if (hfcount != null)
        //    {
        //        hfcount.Value = "";
        //    }
        //    //设置文件夹的名称
        //    string attaname = "";
        //    string attaurl = "";
        //    //设置上传路径
        //    string pname = "~/UpFile/" + System.DateTime.Now.ToString("yyyyMM") + "/";
        //    string path = System.Web.HttpContext.Current.Server.MapPath(pname);

        //    AccessoryEntity attainfo = null;

        //    //判断上传文件夹是否存在，若不存在，则创建
        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }
        //    //遍历页面中的上传控件
        //    HttpFileCollection files = HttpContext.Current.Request.Files;
        //    for (int i = start; i < end; i++)
        //    {
        //        HttpPostedFile postedFile = files[i];
        //        if (postedFile.FileName != "")
        //        {
        //            string a = Path.GetFileName(postedFile.FileName);

        //            string[] name = postedFile.FileName.ToString().Split('\\');
        //            string[] filename = name[name.Length - 1].ToString().Split('.');
        //            //获取上传文件的名称
        //            string oglname = filename[0].ToString();
        //            for (int o = 1; o < filename.Length - 1; o++)
        //            {
        //                oglname += "." + filename[o].ToString();
        //            }
        //            attaname += oglname + ",";
        //            //为上传的文件设置新的名称，防止重名
        //            string newname = System.DateTime.Now.ToString("yyyyMMddHHmmssffff") + i.ToString();
        //            newname = newname + "." + filename[filename.Length - 1];
        //            //设置完整的文件上传路径
        //            string filepath = path + newname;

        //            if (postedFile.ContentLength < upsize)
        //            {
        //                postedFile.SaveAs(filepath);
        //                if (hfcount != null)
        //                {
        //                    hfcount.Value += filepath + "$";
        //                }

        //                int j = filepath.IndexOf("UpFile");
        //                string str = filepath.Substring(j - 1);
        //                attaurl += "~" + str + ",";
        //            }
        //            else
        //            {
        //                attainfo = new AccessoryEntity();
        //                attainfo.AccessID = "-2";
        //                attainfo.AccessName = "上传失败，上传文件不能大于" + upsize / 1000000 + "M！";
        //                return attainfo;
        //            }
        //        }
        //    }

        //    attaname = (attaname + "$").Replace(",$", "");
        //    attaurl = (attaurl + "$").Replace(",$", "");
        //    if (attaname != "$" && attaurl != "$" && attainfo == null)
        //    {
        //        attainfo = new AccessoryEntity(attaname, attaurl);
        //    }
        //    else
        //    {
        //        attainfo = new AccessoryEntity();
        //    }
        //    return attainfo;
        //}

        /// <summary>
        /// 共享文件上传
        /// </summary>
        /// <param name="file">文件名</param>
        /// <param name="mark">标识</param>
        /// <returns></returns>
        //public static ShareFileEntity upsharefile(int start, int end, HiddenField hfcount)
        //{
        //    int upsize = 30000000;
        //    try
        //    {
        //        upsize = Convert.ToInt32(ConfigurationManager.AppSettings["upsize"].ToString());
        //    }
        //    catch (Exception) { }

        //    //清空隐藏控件的值，用于存放路径，以便数据保存失败时删除文件
        //    if (hfcount != null)
        //    {
        //        hfcount.Value = "";
        //    }
        //    //设置文件夹的名称
        //    string attaname = "";
        //    string attalastname = "";
        //    string attaurl = "";
        //    //设置上传路径
        //    string pname = "~/ShareFile/";
        //    string path = System.Web.HttpContext.Current.Server.MapPath(pname);

        //    ShareFileEntity attainfo = null;

        //    //判断上传文件夹是否存在，若不存在，则创建
        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }
        //    //遍历页面中的上传控件
        //    HttpFileCollection files = HttpContext.Current.Request.Files;
        //    for (int i = start; i < end; i++)
        //    {
        //        HttpPostedFile postedFile = files[i];
        //        if (postedFile.FileName != "")
        //        {
        //            string a = Path.GetFileName(postedFile.FileName);

        //            string[] name = postedFile.FileName.ToString().Split('\\');
        //            string[] filename = name[name.Length - 1].ToString().Split('.');
        //            //获取上传文件的名称
        //            string oglname = filename[0].ToString();
        //            string lastname = filename[filename.Length - 1].ToString();
        //            for (int o = 1; o < filename.Length - 1; o++)
        //            {
        //                oglname += "." + filename[o].ToString();
        //            }
        //            attaname += oglname + ",";
        //            attalastname += lastname + ",";
        //            //为上传的文件设置新的名称，防止重名
        //            string newname = System.DateTime.Now.ToString("yyyyMMddHHmmssffff") + i.ToString();
        //            newname = newname + "." + filename[filename.Length - 1];
        //            //设置完整的文件上传路径
        //            string filepath = path + newname;

        //            if (postedFile.ContentLength < upsize)
        //            {
        //                postedFile.SaveAs(filepath);
        //                if (hfcount != null)
        //                {
        //                    hfcount.Value += filepath + "$";
        //                }

        //                int j = filepath.IndexOf("Share");
        //                string str = filepath.Substring(j - 1);
        //                attaurl += "~" + str + ",";
        //            }
        //            else
        //            {
        //                attainfo = new ShareFileEntity();
        //                attainfo.FID = "-2";
        //                attainfo.LastName = "";
        //                attainfo.AccessName = "上传失败，上传文件不能大于" + upsize / 1000000 + "M！";
        //                return attainfo;
        //            }
        //        }
        //    }

        //    attaname = (attaname + "$").Replace(",$", "");
        //    attaurl = (attaurl + "$").Replace(",$", "");
        //    attalastname = (attalastname + "$").Replace(",$", "");
        //    if (attaname != "$" && attaurl != "$" && attainfo == null)
        //    {
        //        attainfo = new ShareFileEntity(attaname, attaurl);

        //        attainfo.LastName = attalastname;
        //    }
        //    else
        //    {
        //        attainfo = new ShareFileEntity();
        //    }
        //    return attainfo;
        //}
        #endregion

        #region 截取Json字符串
        /// <summary>
        /// 截取Json字符串
        /// </summary>
        /// <param name="json"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Json(string json, string key)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(json))
            {
                key = "\"" + key.Trim('"') + "\"";
                int index = json.IndexOf(key) + key.Length + 1;
                if (index > key.Length + 1)
                {
                    //先截逗号，若是最后一个，截“｝”号，取最小值
                    int end = json.IndexOf(',', index);
                    if (end == -1)
                    {
                        end = json.IndexOf('}', index);
                    }

                    result = json.Substring(index, end - index);
                    result = result.Trim(new char[] { '"', ' ', '\'' }); //过滤引号或空格
                }
            }
            return result;
        }
        #endregion

        #region get事件
        /// <summary>
        /// get事件
        /// </summary>
        /// <param name="urlString"></param>
        /// <returns></returns>
        public static string RequestUrl(string urlString)
        {
            //定义局部变量
            HttpWebRequest httpWebRequest = null;
            HttpWebResponse httpWebRespones = null;
            Stream stream = null;
            string htmlString = string.Empty;
            //请求页面
            try
            {
                httpWebRequest = WebRequest.Create(urlString) as HttpWebRequest;
            }
            //处理异常
            catch (Exception ex)
            {
                throw new Exception("建立页面请求时发生错误！", ex);
            }
            httpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; Maxthon 2.0)";
            //获取服务器的返回信息
            try
            {
                httpWebRespones = (HttpWebResponse)httpWebRequest.GetResponse();
                stream = httpWebRespones.GetResponseStream();
            }
            //处理异常
            catch (Exception ex)
            {
                throw new Exception("接受服务器返回页面时发生错误！", ex);
            }
            StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
            //读取返回页面
            try
            {
                htmlString = streamReader.ReadToEnd();
            }
            //处理异常
            catch (Exception ex)
            {
                throw new Exception("读取页面数据时发生错误！", ex);
            }
            //释放资源返回结果
            streamReader.Close();
            stream.Close();
            return htmlString;
        }
        #endregion

    }
}
