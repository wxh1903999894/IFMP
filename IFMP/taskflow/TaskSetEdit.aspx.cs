/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年8月28日 17时00分19秒
** 描    述:      任务设定信息
** 修 改 人:      
** 修改日期:    
** 修改说明: 
**-----------------------------------------------------------------
*****************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using GK.IFMP.Common;
using IFMPLibrary.DAO;
using IFMPLibrary.Enums;
using IFMPLibrary.Entities;
using IFMPLibrary.DBContext;
using IFMPLibrary.Utils;
using System.Transactions;

namespace IFMP.taskflow
{
    public partial class TaskSetEdit : PageBase
    {
        IFMPDBContext db = new IFMPDBContext();

        #region 参数集合
        public int TaskID
        {
            get
            {
                return GetQueryString<int>("id", -1);
            }
        }
        #endregion


        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CommonFunction.BindEnum<ClassTypeEnums>(this.ddl_ClassType, "-2");

                List<TableType> TableTypeList = db.TableType.Where(t => t.IsDel != true).ToList();
                foreach (TableType TableType in TableTypeList)
                {
                    cbl_TableType.Items.Add(new ListItem(TableType.Name, TableType.ID.ToString()));
                }
              
                for (int i = 0; i < this.cbl_TableType.Items.Count; i++)
                {
                    cbl_TableType.Items[i].Selected = true;
                }
            }
        }
        #endregion


        #region 绑定基础班次信息
        protected void ddl_ClassType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int clatype = Convert.ToInt32(this.ddl_ClassType.SelectedValue.ToString());//班次类型
            List<BaseClass> classlist = db.BaseClass.Where(t => t.ClassType == (ClassTypeEnums)clatype && t.IsDel != true).ToList();
            if (classlist.Count > 0)
            {
                cbl_BaseClassID.DataTextField = "Name";
                cbl_BaseClassID.DataValueField = "ID";
                cbl_BaseClassID.DataSource = classlist;
                cbl_BaseClassID.DataBind();
            }
        }
        #endregion


        #region 提交事件
        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    TaskSet model = db.TaskSet.FirstOrDefault(t => t.ID == TaskID && t.IsDel != true);
                    if (model == null)
                    {
                        for (int w = 0; w < this.ck_Weeks.Items.Count; w++)
                        {
                            if (this.ck_Weeks.Items[w].Selected)
                            {
                                for (int i = 0; i < this.cbl_TableType.Items.Count; i++)
                                {
                                    if (this.cbl_TableType.Items[i].Selected)
                                    {
                                        for (int j = 0; j < this.cbl_BaseClassID.Items.Count; j++)
                                        {
                                            model = new TaskSet();
                                            model.TaskName = this.txt_TaskName.Text;
                                            model.ClassType = (ClassTypeEnums)Convert.ToInt32(this.ddl_ClassType.SelectedValue);
                                            model.TableTypeID = Convert.ToInt32(this.cbl_TableType.Items[i].Value);
                                            model.IsDel = false;
                                            model.CreateDate = DateTime.Now;
                                            model.CreateUserID = UserID;
                                            model.BaseClassID = Convert.ToInt32(this.cbl_BaseClassID.Items[j].Value);
                                            model.Weeks = this.ck_Weeks.Items[w].Value.ToString();
                                            db.TaskSet.Add(model);
                                            db.SaveChanges();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        model.TaskName = this.txt_TaskName.Text;
                        model.ClassType = (ClassTypeEnums)Convert.ToInt32(this.ddl_ClassType.SelectedValue);
                        for (int i = 0; i < this.cbl_TableType.Items.Count; i++)
                        {
                            if (this.cbl_TableType.Items[i].Selected)
                            {
                                model.TableTypeID = Convert.ToInt32(cbl_TableType.Items[i].Value);
                                for (int j = 0; j < this.cbl_BaseClassID.Items.Count; j++)
                                {
                                    model.BaseClassID = Convert.ToInt32(this.cbl_BaseClassID.Items[j].Value);
                                }
                            }
                        }

                        db.SaveChanges();
                    }
                    ts.Complete();
                    ShowMessage();
                }
                catch (Exception ex)
                {
                    ShowMessage("提交失败请检查填写数据");
                    ts.Dispose();
                    new SysLogDAO().AddLog(LogType.系统日志, ex.Message, UserID);
                }
            }
        }
        #endregion
    }
}