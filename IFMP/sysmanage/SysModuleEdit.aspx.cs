/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创建人:      樊紫红
** 创建日期:    2018年7月30日 14时25分
** 描 述:       模块管理页面
** 修改人:      
** 修改日期:    
** 修改说明:
**-----------------------------------------------------------------
******************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using GK.IFMP.Common;
using IFMPLibrary.DAO;
using IFMPLibrary.Enums;
using IFMPLibrary.Entities;
using IFMPLibrary.DBContext;
using IFMPLibrary.Utils;

namespace IFMP.sysmanage
{
    public partial class SysModuleEdit : PageBase
    {
        IFMPDBContext db = new IFMPDBContext();

        #region 参数集合
        /// <summary>
        /// MID
        /// </summary>
        public int MID
        {
            get
            {
                return GetQueryString<int>("id", -2);
            }
        }
        #endregion


        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cblBand();
                if (MID != -2)
                {
                    InfoBind();
                }
                else
                {
                    this.hf_PID.Value = "-1";
                    this.hf_ID.Value = "-2";
                    this.btn_Deleted.Visible = false;
                    this.btn_Adds.Visible = false;
                }
            }
        }
        #endregion


        #region 初始化数据
        /// <summary>
        /// 初始化数据
        /// </summary>
        protected void InfoBind()
        {
            SysModule model = db.SysModule.FirstOrDefault(t => t.ID == MID);
            if (model != null)
            {
                this.txt_MName.Text = model.Name;//模块名称
                this.hf_ID.Value = model.ID.ToString();
                this.hf_PID.Value = model.ParentID.ToString();
                string[] bt = model.ModuleButton.Split(',');
                //父级模块名称
                if (model.ParentID != -1)
                {
                    SysModule smodel = db.SysModule.FirstOrDefault(t => t.ID == model.ParentID);
                    if (smodel != null)
                    {
                        this.txt_PMName.Text = smodel.Name;
                    }
                }
                //按钮
                foreach (string dr in bt)
                {
                    foreach (ListItem li in this.cbl_Button.Items)
                    {
                        if (dr == li.Value)
                        {
                            li.Selected = true;
                        }
                    }
                }
                this.txt_Icon.Text = model.ModuleIcon;//图标
                this.txt_Url.Text = model.ModuleUrl;//栏目地址
                this.rbol_MType.SelectedValue = model.IsRight.ToString();
                this.txt_Order.Text = model.ModuleOrder.ToString();
            }
        }
        #endregion


        #region 按钮绑定
        /// <summary>
        /// 按钮绑定
        /// </summary>
        private void cblBand()
        {
            List<SysButton> buttonlist = db.SysButton.OrderBy(t => t.ID).ToList();
            if (buttonlist.Count > 0)
            {
                this.cbl_Button.DataSource = buttonlist;
                this.cbl_Button.DataValueField = "ID";
                this.cbl_Button.DataTextField = "Name";
                this.cbl_Button.DataBind();
            }
        }
        #endregion


        #region 提交
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Sumbit_Click(object sender, EventArgs e)
        {
            try
            {
                string button = "";
                int id = Convert.ToInt32(this.hf_ID.Value.ToString());
                SysModule module = db.SysModule.FirstOrDefault(t => t.ID == MID && id != -2);
                string message = "";
                if (module == null)
                {
                    module = new SysModule();
                    module.ParentID = int.Parse(this.hf_PID.Value);
                    module.Name = this.txt_MName.Text;
                    module.ModuleUrl = this.txt_Url.Text;
                    module.ModuleIcon = this.txt_Icon.Text;
                    module.IsRight = int.Parse(this.rbol_MType.SelectedValue);
                    if (this.txt_Order.Text == "")
                    {
                        module.ModuleOrder = 0;
                    }
                    else
                    {
                        if (new BaseUtils().GetRegex(this.txt_Order.Text, RegexType.非负整数) == false)
                        {
                            ShowMessage("排序号只能填写有效数字！！！");
                            return;
                        }
                        else
                        {
                            module.ModuleOrder = Convert.ToInt32(this.txt_Order.Text.ToString());
                        }
                    }
                    foreach (ListItem li in this.cbl_Button.Items)
                    {
                        if (li.Selected)
                        {
                            button = button + li.Value + ",";
                        }
                    }

                    if (button.Length > 0)
                    {
                        button = button.Substring(0, button.Length - 1);
                    }
                    module.ModuleButton = button;

                    if (db.SysModule.FirstOrDefault(t => t.Name == module.Name) != null)
                    {
                        ShowMessage("该模块名称已存在，请重新输入");
                        return;
                    }
                    message = "添加模块名称为【" + module.Name + "】的模块信息";
                    db.SysModule.Add(module);
                    db.SaveChanges();
                }
                else
                {
                    module.Name = this.txt_MName.Text;
                    module.ModuleUrl = this.txt_Url.Text;
                    module.ModuleIcon = this.txt_Icon.Text;
                    module.IsRight = int.Parse(this.rbol_MType.SelectedValue);
                    if (this.txt_Order.Text == "")
                    {
                        module.ModuleOrder = 0;
                    }
                    else
                    {
                        if (new BaseUtils().GetRegex(this.txt_Order.Text, RegexType.非负整数) == false)
                        {
                            ShowMessage("排序号只能填写有效数字！！！");
                            return;
                        }
                    }
                    foreach (ListItem li in this.cbl_Button.Items)
                    {
                        if (li.Selected)
                        {
                            button = button + li.Value + ",";
                        }
                    }

                    if (button.Length > 0)
                    {
                        button = button.Substring(0, button.Length - 1);
                    }
                    module.ModuleButton = button;

                    if (db.SysModule.FirstOrDefault(t => t.Name == module.Name && t.ID != MID) != null)
                    {
                        ShowMessage("该模块名称已存在，请重新输入");
                        return;
                    }
                    message = "修改模块名称为【" + module.Name + "】的模块信息";
                    db.SaveChanges();
                }
                new SysLogDAO().AddLog(LogType.操作日志_添加, message, UserID);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Message", "<script>alert('系统提示：提交成功！');succ();</script>");
            }
            catch (Exception error)
            {
                ShowMessage(error.Message);
                new SysLogDAO().AddLog(LogType.系统日志, error.Message, UserID);
                return;
            }
        }
        #endregion


        #region 添加子栏目
        /// <summary>
        /// 添加子栏目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Add_Click(object sender, EventArgs e)
        {
            this.btn_Deleted.Visible = this.btn_Adds.Visible = false;
            this.txt_PMName.Text = this.txt_MName.Text;
            this.txt_MName.Text = "";
            this.hf_PID.Value = this.hf_ID.Value;
            this.hf_ID.Value = "-2";
            this.rbol_MType.SelectedValue = "1";
            this.txt_Url.Text = "";
            this.txt_Icon.Text = "";
        }
        #endregion


        #region 删除栏目
        /// <summary>
        /// 删除栏目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Delete_Click(object sender, EventArgs e)
        {
            int mid = Convert.ToInt32(this.hf_ID.Value);

            List<SysModule> modulelist = db.SysModule.Where(t => t.ParentID == mid).ToList();
            if (modulelist.Count > 0)
            {
                ShowMessage("该模块存在子模块，无法删除");
                return;
            }
            else
            {
                SysModule model = db.SysModule.FirstOrDefault(t => t.ID == mid);
                if (model != null)
                {
                    db.SysModule.Remove(model);
                }
                else
                {
                    ShowMessage("删除失败");
                    return;
                }
            }
            db.SaveChanges();
            Page.ClientScript.RegisterStartupScript(this.GetType(), "Message", "<script>alert('系统提示：删除成功！');succ();</script>");
            new SysLogDAO().AddLog(LogType.操作日志_删除, "删除模块【" + this.txt_MName.Text + "】", UserID);
        }
        #endregion
    }
}