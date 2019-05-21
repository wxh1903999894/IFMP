/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年8月1日 9时08分19秒
** 描    述:      表单字段编辑页面
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

namespace IFMP.basedata
{
    public partial class FlowEdit : PageBase
    {
        IFMPDBContext db = new IFMPDBContext();

        #region 参数集合
        public int ID
        {
            get
            {
                return GetQueryString<int>("id", -1);
            }
        }

        /// <summary>
        /// 表单类型
        /// </summary>
        public int TableTypeID
        {
            get
            {
                return GetQueryString<int>("type", -1);
            }
        }
        #endregion


        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (ID != -1)
                {

                    using (IFMPDBContext db = new IFMPDBContext())
                    {
                        List<Flow> FlowList = db.Flow.Where(t => t.TableTypeID == TableTypeID).ToList();
                        FlowList = new FlowDAO().GetFlowLevel(FlowList);

                        this.ddl_ParentFlow.DataSource = FlowList;
                        this.ddl_ParentFlow.DataValueField = "ID";
                        this.ddl_ParentFlow.DataTextField = "Name";
                        this.ddl_ParentFlow.DataBind();
                        this.ddl_ParentFlow.Items.Insert(0, new ListItem("<b>无父级流程</b>", "0"));

                    }

                    InfoBind();
                }
            }
        }
        #endregion


        #region 初始化用户数据
        private void InfoBind()
        {
            using (IFMPDBContext db = new IFMPDBContext())
            {
                Flow Flow = db.Flow.FirstOrDefault(t => t.ID == ID);
                if (Flow != null)
                {
                    txt_FlowName.Text = Flow.Name;
                    ddl_ParentFlow.SelectedValue = Flow.ParentID.ToString();
                    rdo_IsAudit.SelectedValue = Flow.IsAudit ? "1" : "0";
                }
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
                    Flow Flow = db.Flow.FirstOrDefault(t => t.ID == ID);
                    if (Flow == null)
                    {
                        Flow = new Flow();
                        Flow.TableTypeID = TableTypeID;
                        Flow.Name = txt_FlowName.Text;
                        Flow.ParentID = Convert.ToInt32(ddl_ParentFlow.SelectedValue);
                        if (rdo_IsAudit.SelectedValue == "1")
                        {
                            Flow.IsAudit = true;
                        }
                        else
                        {
                            Flow.IsAudit = false;
                        }
                        db.Flow.Add(Flow);
                    }
                    else
                    {
                        Flow.TableTypeID = TableTypeID;
                        Flow.Name = txt_FlowName.Text;
                        Flow.ParentID = Convert.ToInt32(ddl_ParentFlow.SelectedValue);
                        if (rdo_IsAudit.SelectedValue == "1")
                        {
                            Flow.IsAudit = true;
                        }
                        else
                        {
                            Flow.IsAudit = false;
                        }
                        db.Flow.Add(Flow);
                    }
                    db.SaveChanges();

                    ShowMessage();
                    LogType log = (ID == -1 ? LogType.操作日志_添加 : LogType.操作日志_修改);
                    new SysLogDAO().AddLog(log, (ID == -1 ? "增加" : "修改") + "流程信息");
                    ts.Complete();
                   
                }
                catch (Exception ex)
                {
                    ShowMessage(ex.Message);
                    new SysLogDAO().AddLog(LogType.系统日志, ex.Message, UserID);
                    ts.Dispose();
                    return;
                }
            }
        }
        #endregion



    }
}