using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;

using IFMPLibrary.Entities;
using IFMPLibrary.Enums;
using IFMPLibrary.DBContext;
using IFMPLibrary.Utils;
using IFMPLibrary.DAO;
using Newtonsoft.Json.Linq;


namespace IFMP.ashx
{
    /// <summary>
    /// GetBaseDate 的摘要说明
    /// </summary>
    public class GetBaseDate : IHttpHandler
    {
        IFMPDBContext db = new IFMPDBContext();
        public void ProcessRequest(HttpContext context)
        {
            string method = context.Request.Params["method"];
            switch (method)
            {
                case "GetUserTxt":
                    GetUserTxt(context);
                    break;
                case "GetUserByDepLayUI":
                    GetUserByDepLayUI(context);
                    break;
                case "GetName":
                    GetName(context);
                    break;
            }
        }


        #region 获取部门用户联动信息（剔除部门没人分组TextBox选择）
        /// <summary>
        /// 获取用户列表（剔除部门没人分组TextBox选择）
        /// </summary>
        /// <param name="context"></param>
        public void GetUserTxt(HttpContext context)
        {
            StringBuilder sb = new StringBuilder("");
            //string deptype = context.Request.Params["deptype"];
            string name = "";
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    List<Department> DepartmentList = db.Department.Where(t => t.IsDel != true && t.ParentID == -1 && t.IsAdmin).ToList();
                    List<DepartmentUser> DepartmentUserList = db.DepartmentUser.ToList();
                    List<User> UserList = db.User.Where(t => t.IsDel != true && t.UserState != UserState.离职).ToList();
                    if (DepartmentList.Count > 0)
                    {
                        foreach (Department Department in DepartmentList)
                        {

                            string a = InitChildUser(Department.ID, UserList.Where(t => DepartmentUserList.Where(m => m.DepartmentID == Department.ID).Select(m => m.UserID).Contains(t.ID)).ToList());
                            if (a == "")
                            { }
                            else
                            {
                                name += "{\"id\":\"" + "dep" + Department.ID +
                                   "\",\"text\":\"" + Department.Name + "\",";
                                name += a;//调用递归方法
                                name += ",\"state\":\"closed\"},";
                            }
                        }
                    }
                    else
                    {
                        name = "[]";
                    }
                    sb.Append("[");
                    sb.Append(name.ToString().TrimEnd(','));
                    sb.Append("]");
                }
            }
            catch (Exception error)
            {
                sb.Append("{\"result\":\"" + error.Message + "\"}");
            }
            context.Response.Clear();
            context.Response.Write(sb.ToString());
            context.Response.End();
        }

        /// <summary>
        /// 根据部门id获取用户信息
        /// </summary>
        /// <param name="usertype">用户类别</param>
        /// <param name="parentID">部门id</param>
        /// <returns></returns>
        public string InitChildUser(int parentID, List<User> UserList)
        {
            string name = "";
            StringBuilder sb = new StringBuilder();
            using (IFMPDBContext db = new IFMPDBContext())
            {
                if (UserList.Count > 0)
                {
                    foreach (User User in UserList)
                    {
                        name += "{\"id\":\"" + User.ID +
                        "\",\"text\":\"" + User.RealName + "\"},";
                    }
                    sb.Append("\"children\":[");
                    sb.Append(name.ToString().TrimEnd(','));
                    sb.Append("]");
                }
                else
                {
                    return "";
                }
            }
            return sb.ToString();
        }
        #endregion


        #region 获取部门用户联动信息（LayUI用）
        /// <summary>
        /// 获取用户列表（剔除部门没人分组TextBox选择）
        /// </summary>
        /// <param name="context"></param>
        public void GetUserByDepLayUI(HttpContext context)
        {
            JArray jarray = new JArray();
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    List<Department> DepartmentList = db.Department.Where(t => t.IsDel != true && t.ParentID == -1 && t.IsAdmin).ToList();
                    List<DepartmentUser> DepartmentUserList = db.DepartmentUser.ToList();
                    List<User> UserList = db.User.Where(t => t.IsDel != true && t.UserState != UserState.离职).ToList();

                    foreach (Department Department in DepartmentList)
                    {
                        JObject depobject = new JObject();
                        depobject.Add("name", Department.Name);
                        depobject.Add("type", "optgroup");
                        jarray.Add(depobject);
                        foreach (DepartmentUser DepartmentUser in DepartmentUserList.Where(t => t.DepartmentID == Department.ID && UserList.Select(m => m.ID).Contains(t.UserID)))
                        {
                            JObject userobject = new JObject();
                            userobject.Add("name", UserList.FirstOrDefault(t => t.ID == DepartmentUser.UserID).RealName);
                            userobject.Add("value", DepartmentUser.UserID);
                            jarray.Add(userobject);
                        }
                    }
                }
            }
            catch (Exception error)
            {
                //sb.Append("{\"result\":\"" + error.Message + "\"}");
            }
            context.Response.Clear();
            context.Response.Write(jarray);
            context.Response.End();
        }

        #endregion

        #region 获取名称
        /// <summary>
        /// 获取名称
        /// </summary>
        /// <param name="context"></param>
        private void GetName(HttpContext context)
        {
            StringBuilder sb = new StringBuilder("");
            try
            {
                string ids = context.Request.Params["id"];
                string name = "";
                string[] id = ids.ToString().Split(',');
                if (id.Length > 0)
                {
                    for (int i = 0; i < id.Length; i++)
                    {
                        int uid = Convert.ToInt32(id[i].ToString());
                        User user = db.User.FirstOrDefault(t => t.ID == uid && t.IsDel != true);
                        name += user.RealName.ToString() + ",";
                    }
                    name = "{\"name\":\"" + name.TrimEnd(',') + "\"}";
                    sb.Append("{\"result\":\"true\",\"data\":[");
                    sb.Append(name.TrimEnd(','));
                    sb.Append("]}");
                }
                else
                {
                    sb.Append("{\"result\":\"false\"}");
                }
            }
            catch (Exception error)
            {

            }
            context.Response.Clear();
            context.Response.Write(sb.ToString());
            context.Response.End();
        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}