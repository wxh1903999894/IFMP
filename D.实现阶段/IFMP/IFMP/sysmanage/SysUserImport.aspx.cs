/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年7月12日 8时49分19秒
** 描    述:      用户信息管理页面
** 修 改 人:      
** 修改日期:    
** 修改说明: 
**-----------------------------------------------------------------
*****************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using GK.IFMP.Common;
using IFMPLibrary.DAO;
using IFMPLibrary.Enums;
using IFMPLibrary.Entities;
using IFMPLibrary.DBContext;
using IFMPLibrary.Utils;
using System.IO;
using System.Data.OleDb;
using System.Transactions;


namespace IFMP.sysmanage
{
    public partial class SysUserImport : PageBase
    {
        IFMPDBContext db = new IFMPDBContext();

        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        #endregion


        #region 下载模板文件
        protected void lbtn_example_Click(object sender, EventArgs e)
        {
            string expath = @"~\Template\SysUserTemplate.xls";
            if (!CommonFunction.UpLoadFunciotn(expath, "职工信息导入模板"))
            {
                ShowMessage("模板文件不存在，请联系系统管理员");
                return;
            }
        }
        #endregion


        #region 上传导入的文件
        /// <summary>
        /// 上传导入的文件
        /// </summary>
        /// <returns></returns>
        protected string up()
        {
            string path = Server.MapPath("../Template/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (fuimport.HasFile)
            {
                string name = UserID.ToString().ToString() + "_TeacherTemplate_";
                string strfile = System.IO.Path.GetExtension(fuimport.FileName);
                string filename = name + strfile;
                path += filename;
                fuimport.SaveAs(path);
                return path;
            }
            else
            {
                return "";
            }
        }
        #endregion


        #region 读取Excel文件
        /// <summary>
        /// 读取Excel文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public DataTable ReadExcel(string path)
        {
            DataTable dt = new DataTable();
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source='" + path + "';" + "Extended Properties=Excel 8.0;";
            OleDbConnection conn = new OleDbConnection(strConn);
            try
            {
                conn.Open();
                //获取表名
                DataTable dtname = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string sheetName = dtname.Rows[0][2].ToString().Trim();
                //读取excel文件数据
                string strExcel = string.Format("select * from [{0}]", sheetName);
                OleDbDataAdapter myCommand = new OleDbDataAdapter(strExcel, strConn);
                myCommand.Fill(dt);
            }
            catch (Exception ex)
            {
                dt = null;
            }
            conn.Close();
            CommonFunction.delfile(path);
            return dt;
        }
        #endregion


        #region 导入事件
        protected void btn_Sumbit_Click(object sender, EventArgs e)
        {
            string path = up();
            if (path != "")
            {
                DataTable dt = ReadExcel(path);
                if (dt != null)
                {
                    string colname = "";
                    foreach (DataColumn dc in dt.Columns)
                    {
                        colname += dc.ColumnName + ",";
                    }
                    string[] needcol = { "用户名", "员工编号", "姓名", "性别", "身份证", "手机号码", "职务", "部门", "岗位", "政治面貌", "民族", "出生日期", "入职日期", "转正日期", "员工分类", "岗位类型", "状态", "家庭住址" };
                    int count = 0;
                    for (int i = 0; i < needcol.Length; i++)
                    {
                        count += colname.IndexOf(needcol[i]) == -1 ? -1 : 1;
                    }
                    if (count >= needcol.Length)
                    {
                        try
                        {
                            using (TransactionScope ts = new TransactionScope())
                            {
                                int result = 0;
                                string message = "";
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    if (dt.Rows[i]["用户名"].ToString().Trim() == "" || dt.Rows[i]["员工编号"].ToString().Trim() == "" || dt.Rows[i]["姓名"].ToString().Trim() == ""
                                    || dt.Rows[i]["性别"].ToString().Trim() == "" || dt.Rows[i]["部门"].ToString().Trim() == ""
                                    || dt.Rows[i]["岗位"].ToString().Trim() == "" || dt.Rows[i]["政治面貌"].ToString().Trim() == "" || dt.Rows[i]["民族"].ToString().Trim() == ""
                                    || dt.Rows[i]["员工分类"].ToString().Trim() == "" || dt.Rows[i]["岗位类型"].ToString().Trim() == "" || dt.Rows[i]["状态"].ToString().Trim() == "")
                                    {
                                        result = -1;
                                        message = "第【" + (i + 2) + "】行有不可为空项为空，请检查后重新导入";
                                        break;
                                    }
                                    User user = new User();
                                    UserDetails userdetails = new UserDetails();
                                    user.UserNumber = dt.Rows[i]["员工编号"].ToString().Trim();
                                    user.UserName = dt.Rows[i]["用户名"].ToString().Replace(" ", "").Trim();
                                    user.Cellphone = dt.Rows[i]["手机号码"].ToString().Trim();
                                    if (!string.IsNullOrEmpty(user.Cellphone) && new BaseUtils().GetRegex(user.Cellphone, RegexType.手机号) == false)
                                    {
                                        result = -1;
                                        message = "第【" + (i + 2) + "】行手机号码不正确，请检查后重新导入";
                                        break;
                                    }
                                    user.RealName = dt.Rows[i]["姓名"].ToString().Trim();
                                    user.Password = new BaseUtils().BuildPW(user.UserName, "888888");
                                    user.CreateDate = DateTime.Now;
                                    user.CreateUserID = UserID;
                                    user.IsDel = false;
                                    try
                                    {
                                        user.UserState = (UserState)Enum.Parse(typeof(UserState), dt.Rows[i]["状态"].ToString().Trim());
                                    }
                                    catch
                                    {
                                        result = -1;
                                        message = "第【" + (i + 2) + "】行状态信息有误，请检查后重新提交";
                                        break;
                                    }

                                    try
                                    {
                                        user.UserLeaveType = (UserLeaveType)Enum.Parse(typeof(UserLeaveType), dt.Rows[i]["岗位类型"].ToString().Trim());
                                    }
                                    catch
                                    {
                                        result = -1;
                                        message = "第【" + (i + 2) + "】行岗位类型信息有误，请检查后重新提交";
                                        break;
                                    }

                                    if (db.User.FirstOrDefault(t => t.UserName == user.UserName && t.IsDel != true) != null)
                                    {
                                        result = -1;
                                        message = "第【" + (i + 2) + "】行用户名已存在，请检查后重新提交";
                                        break;
                                    }
                                    if (db.User.FirstOrDefault(t => t.UserNumber == user.UserNumber && t.IsDel != true) != null)
                                    {
                                        result = -1;
                                        message = "第【" + (i + 2) + "】行员工编号已存在，请检查后重新提交";
                                        break;
                                    }
                                    db.User.Add(user);
                                    db.SaveChanges();

                                    //用户添加到部门
                                    string depname = dt.Rows[i]["部门"].ToString().Trim();
                                    Department dep = db.Department.FirstOrDefault(t => t.Name.Contains(depname) && t.IsDel != true);
                                    if (dep != null)
                                    {
                                        DepartmentUser DepartmentUser = new DepartmentUser();
                                        DepartmentUser.DepartmentID = dep.ID;
                                        DepartmentUser.UserID = user.ID;
                                        db.DepartmentUser.Add(DepartmentUser);
                                        //db.SaveChanges();
                                    }
                                    else
                                    {
                                        result = -1;
                                        message = "第【" + (i + 2) + "】行部门名称不存在，请检查后重新提交";
                                        break;
                                    }

                                    //用户添加到岗位
                                    string postname = dt.Rows[i]["岗位"].ToString().Trim();
                                    Post post = db.Post.FirstOrDefault(t => t.Name.Contains(postname) && t.IsDel != true);
                                    if (post != null)
                                    {
                                        PostUser postuser = new PostUser();
                                        postuser.PostID = post.ID;
                                        postuser.UserID = user.ID;
                                        db.PostUser.Add(postuser);
                                        //db.SaveChanges();
                                    }
                                    else
                                    {
                                        result = -1;
                                        message = "第【" + (i + 2) + "】行岗位名称不存在，请检查后重新提交";
                                        break;
                                    }

                                    db.SaveChanges();
                                    userdetails.UserID = user.ID;
                                    try { userdetails.Sex = (Sex)Enum.Parse(typeof(Sex), dt.Rows[i]["性别"].ToString()); }
                                    catch
                                    {
                                        result = -1;
                                        message = "第【" + (i + 2) + "】行性别信息有误，请检查后重新提交";
                                        break;
                                    }
                                    try { userdetails.Nationality = (Nationality)Enum.Parse(typeof(Nationality), dt.Rows[i]["民族"].ToString().Trim()); }
                                    catch
                                    {
                                        result = -1;
                                        message = "第【" + (i + 2) + "】行民族信息有误，请检查后重新提交";
                                        break;
                                    }
                                    try { userdetails.Polity = (Polity)Enum.Parse(typeof(Polity), dt.Rows[i]["政治面貌"].ToString().Trim()); }
                                    catch
                                    {
                                        result = -1;
                                        message = "第【" + (i + 2) + "】行政治面貌信息有误，请检查后重新提交";
                                        break;
                                    }
                                    if (!string.IsNullOrEmpty(dt.Rows[i]["出生日期"].ToString().Trim()))
                                        userdetails.BirthDate = Convert.ToDateTime(dt.Rows[i]["出生日期"].ToString().Trim());
                                    userdetails.Address = dt.Rows[i]["家庭住址"].ToString().Trim();
                                    userdetails.Job = dt.Rows[i]["职务"].ToString().Trim();

                                    try
                                    {
                                        userdetails.HireDate = Convert.ToDateTime(dt.Rows[i]["入职日期"].ToString().Trim());
                                    }

                                    catch
                                    {
                                        result = -1;
                                        message = "第【" + (i + 2) + "】行入职日期信息有误，请检查后重新提交";
                                        break;
                                    }

                                    userdetails.ProbationDays = 0;
                                    if (!string.IsNullOrEmpty(dt.Rows[i]["转正日期"].ToString().Trim()))
                                        userdetails.QualifiedDate = Convert.ToDateTime(dt.Rows[i]["转正日期"].ToString().Trim());

                                    try { userdetails.UserType = (UserType)Enum.Parse(typeof(UserType), dt.Rows[i]["员工分类"].ToString()); }
                                    catch
                                    {
                                        result = -1;
                                        message = "第【" + (i + 2) + "】行员工分类信息有误，请检查后重新提交";
                                        break;
                                    }

                                    userdetails.Identity = dt.Rows[i]["身份证"].ToString().Trim();

                                    if (!string.IsNullOrEmpty(userdetails.Identity) && new BaseUtils().GetRegex(userdetails.Identity, RegexType.身份证号码) == false)
                                    {
                                        result = -1;
                                        message = "第【" + (i + 2) + "】行身份证号码不正确，请检查后重新导入";
                                        break;
                                    }

                                    userdetails.HeaderUrl = "";
                                    userdetails.CreateDate = DateTime.Now;
                                    userdetails.CreateUserID = UserID;
                                    userdetails.IsDel = false;
                                    db.UserDetails.Add(userdetails);
                                    db.SaveChanges();
                                }
                                if (result != 0)
                                {
                                    ShowMessage(message);
                                    new SysLogDAO().AddLog(LogType.系统日志, message, UserID);
                                    ts.Dispose();
                                }
                                else
                                {
                                    ShowMessage();
                                    new SysLogDAO().AddLog(LogType.操作日志_导入, "成功导入用户及档案信息", UserID);
                                    ts.Complete();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ShowMessage(ex.Message);
                            new SysLogDAO().AddLog(LogType.系统日志, ex.Message, UserID);
                        }
                    }
                    else
                    {
                        ShowMessage("文件中缺少必要的信息，请检查后重新导入");
                        return;
                    }
                }
                else
                {
                    ShowMessage("文件读取失败，请检查文件是否已损坏");
                    return;
                }
            }
            else
            {
                ShowMessage("文件导入失败");
                return;
            }
        }
        #endregion
    }
}