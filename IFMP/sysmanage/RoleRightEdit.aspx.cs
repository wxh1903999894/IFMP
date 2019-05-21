/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年7月30日 17时56分19秒
** 描    述:      用户编辑页面
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
using System.Collections;
using System.Transactions;

namespace IFMP.sysmanage
{
    public partial class RoleRightEdit : PageBase
    {
        IFMPDBContext db = new IFMPDBContext();

        #region 参数集合
        /// <summary>
        /// RoleID
        /// </summary>
        private int RoleID
        {
            get
            {
                return GetQueryString<int>("id", -1);
            }
        }
        #endregion

        #region 页面加载
        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<SysModule> modulelist = db.SysModule.Where(t => t.ParentID == -1 && t.IsRight == 1).OrderBy(t => t.ModuleOrder).ToList();
                List<object> joinlist = new List<object>();
                if (modulelist.Count > 0)
                {
                    foreach (SysModule module in modulelist)
                    {
                        string rid = "";
                        int moduleid = Convert.ToInt32(module.ID);
                        RoleRight roleright = db.RoleRight.FirstOrDefault(t => t.RoleID == RoleID && t.ModuleID == moduleid);
                        if (roleright != null)
                        {
                            rid = roleright.RoleID.ToString();
                        }
                        joinlist.Add(new
                        {
                            module.ID,
                            module.Name,
                            module.ModuleUrl,
                            module.ModuleIcon,
                            module.ParentID,
                            module.ModuleOrder,
                            module.IsRight,
                            module.ModuleButton,
                            rid
                        });
                    }
                }

                this.rpmodule.DataSource = joinlist;
                this.rpmodule.DataBind();

                Role model = db.Role.FirstOrDefault(t => t.ID == RoleID);
                if (model != null)
                {
                    this.lblrolename.Text = model.Name;
                }
            }
        }
        #endregion

        #region 一级模块绑定
        /// <summary>
        /// 一级模块绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rpmodule_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                HiddenField hfModuleID = (HiddenField)e.Item.FindControl("hffid");
                Repeater rpnextModule = (Repeater)e.Item.FindControl("rpnextModule");
                int moduleid = Convert.ToInt32(hfModuleID.Value.ToString());
                List<SysModule> modulelist = db.SysModule.Where(t => t.ParentID == moduleid && t.IsRight == 1).OrderBy(t => t.ModuleOrder).ToList();
                List<object> joinlist = new List<object>();
                if (modulelist.Count > 0)
                {
                    foreach (SysModule module in modulelist)
                    {
                        string rid = "";
                        int mid = Convert.ToInt32(module.ID);
                        RoleRight roleright = db.RoleRight.FirstOrDefault(t => t.RoleID == RoleID && t.ModuleID == mid);
                        if (roleright != null)
                        {
                            rid = roleright.RoleID.ToString();
                        }
                        joinlist.Add(new
                        {
                            module.ID,
                            module.Name,
                            module.ModuleUrl,
                            module.ModuleIcon,
                            module.ParentID,
                            module.ModuleOrder,
                            module.IsRight,
                            module.ModuleButton,
                            rid
                        });
                    }
                }
                rpnextModule.DataSource = joinlist;
                rpnextModule.DataBind();
                CheckBox chk_fid = (CheckBox)e.Item.FindControl("chk_fid");
            }
        }
        #endregion

        #region 二级模块绑定
        /// <summary>
        /// 二级模块绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rpnextModule_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                HiddenField hfModuleID = (HiddenField)e.Item.FindControl("hfsid");
                Repeater rpnextModule = (Repeater)e.Item.FindControl("rplastModule");
                int moduleid = Convert.ToInt32(hfModuleID.Value.ToString());
                List<SysModule> modulelist = db.SysModule.Where(t => t.ParentID == moduleid && t.IsRight == 1).OrderBy(t => t.ModuleOrder).ToList();
                List<object> joinlist = new List<object>();
                if (modulelist.Count > 0)
                {
                    foreach (SysModule module in modulelist)
                    {
                        string rid = "";
                        int mid = Convert.ToInt32(module.ID);
                        RoleRight roleright = db.RoleRight.FirstOrDefault(t => t.RoleID == RoleID && t.ModuleID == mid);
                        if (roleright != null)
                        {
                            rid = roleright.RoleID.ToString();
                        }
                        joinlist.Add(new
                        {
                            module.ID,
                            module.Name,
                            module.ModuleUrl,
                            module.ModuleIcon,
                            module.ParentID,
                            module.ModuleOrder,
                            module.IsRight,
                            module.ModuleButton,
                            rid
                        });
                    }
                }
                rpnextModule.DataSource = joinlist;
                rpnextModule.DataBind();
                CheckBox chk_fid = (CheckBox)e.Item.FindControl("chk_fid");
            }
        }
        #endregion

        #region 按钮列表绑定
        protected void rpbuttonModule_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                HiddenField hfModuleID = (HiddenField)e.Item.FindControl("hftid");
                CheckBox chk_fid = (CheckBox)e.Item.FindControl("chk_tid");
                CheckBoxList chklModule = (CheckBoxList)e.Item.FindControl("chkl_tid");

                int module = Convert.ToInt32(hfModuleID.Value);
                SysModule smodule = db.SysModule.FirstOrDefault(t => t.ID == module);
                List<SysButton> buttonlist = db.SysButton.Where(t => smodule.ModuleButton.Contains(t.ID.ToString())).ToList();
                chklModule.DataTextField = "Name";
                chklModule.DataValueField = "ID";
                chklModule.DataSource = buttonlist;
                chklModule.DataBind();

                RoleRight rightlist = db.RoleRight.FirstOrDefault(t => t.RoleID == RoleID && t.ModuleID == module);
                if (rightlist != null)
                {
                    string[] buttons = rightlist.Buttons.ToString().Split(',');
                    foreach (SysButton button in buttonlist)
                    {
                        for (int i = 0; i < buttons.Length; i++)
                        {
                            if (button.ID.ToString() == buttons[i].ToString() || RoleID == 1)
                            {
                                chklModule.Items[i].Selected = true;
                                //break;
                            }
                        }
                    }
                }

                if (RoleID == 1)
                {
                    chk_fid.Checked = true;
                }
            }
        }
        #endregion

        #region 保存
        protected void btn_Sumbit_Click(object sender, EventArgs e)
        {
            CheckBox chkfid;
            HiddenField hffid;
            CheckBox chksid;
            HiddenField hfsid;
            CheckBoxList chkl_sbtnids;
            CheckBox chktid;
            HiddenField hftid;
            CheckBoxList chkl_tbtnids;
            Repeater rpsid;
            Repeater rptid;

            string sids = "";
            string sbtnids = "";

            for (int i = 0; i < this.rpmodule.Items.Count; i++)
            {
                chkfid = (CheckBox)rpmodule.Items[i].FindControl("chk_fid");
                hffid = (HiddenField)rpmodule.Items[i].FindControl("hffid");
                rpsid = (Repeater)rpmodule.Items[i].FindControl("rpnextModule");
                if (chkfid.Checked)
                {
                    sids += hffid.Value + "|";
                    sbtnids += "-2" + "|";
                }
                if (rpsid != null && rpsid.Items.Count != 0)
                {
                    for (int j = 0; j < rpsid.Items.Count; j++)
                    {
                        rptid = (Repeater)rpsid.Items[j].FindControl("rplastModule");
                        chksid = (CheckBox)rpsid.Items[j].FindControl("chk_fid");
                        hfsid = (HiddenField)rpsid.Items[j].FindControl("hfsid");
                        chkl_sbtnids = (CheckBoxList)rpsid.Items[j].FindControl("chkl_tid");
                        if (chksid != null && chksid.Checked)
                        {
                            sids += hfsid.Value + "|";
                            if (chkl_sbtnids != null && chkl_sbtnids.Items.Count > 0)
                            {
                                sbtnids += GetChecked(chkl_sbtnids) + "|";
                            }
                            else
                            {
                                sbtnids += "-2" + "|";
                            }
                        }

                        if (rptid != null && rptid.Items.Count != 0)
                        {
                            for (int k = 0; k < rptid.Items.Count; k++)
                            {
                                chktid = (CheckBox)rptid.Items[k].FindControl("chk_tid");
                                hftid = (HiddenField)rptid.Items[k].FindControl("hftid");
                                chkl_tbtnids = (CheckBoxList)rptid.Items[k].FindControl("chkl_tid");
                                if (chktid != null && chktid.Checked)
                                {
                                    sids += hftid.Value + "|";
                                    if (chkl_tbtnids != null && chkl_tbtnids.Items.Count > 0)
                                    {
                                        sbtnids += GetChecked(chkl_tbtnids) + "|";
                                    }
                                    else
                                    {
                                        sbtnids += "-2" + "|";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (sids != "")
            {
                try
                {
                    using (TransactionScope ts = new TransactionScope())
                    {
                        try
                        {
                            List<RoleRight> rolerightList = db.RoleRight.Where(t => t.RoleID == RoleID).ToList();
                            if (rolerightList.Count > 0)
                            {
                                db.RoleRight.RemoveRange(rolerightList);
                            }

                            string[] moduleids = sids.ToString().TrimEnd('|').Split('|');
                            string[] btnids = sbtnids.ToString().TrimEnd('|').Split('|');
                            for (int i = 0; i < moduleids.Length; i++)
                            {
                                RoleRight model = new RoleRight();
                                model.RoleID = RoleID;
                                model.ModuleID = Convert.ToInt32(moduleids[i].ToString());
                                for (int j = 0; j < btnids.Length; j++)
                                {
                                    if (i == j)
                                    {
                                        model.Buttons = btnids[j].ToString();
                                        db.RoleRight.Add(model);
                                        db.SaveChanges();
                                        break;
                                    }
                                }
                            }
                            ShowMessage();
                            new SysLogDAO().AddLog(LogType.操作日志_添加, "成功添加权限设置信息", UserID);
                            ts.Complete();
                        }
                        catch
                        {
                            ShowMessage("提交失败");
                            ts.Dispose();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage(ex.Message);
                    new SysLogDAO().AddLog(LogType.系统日志, ex.Message, UserID);
                    return;
                }
            }
            else
            {
                ShowMessage("请选择要设置的权限");
                return;
            }
        }
        #endregion

        #region 得到CheckBoxList中选中了的值
        /// <summary>
        /// 得到CheckBoxList中选中了的值
        /// </summary>
        /// <param name="checkList">CheckBoxList</param>
        /// <returns></returns>
        private string GetChecked(CheckBoxList checkList)
        {
            string selval = "";
            for (int i = 0; i < checkList.Items.Count; i++)
            {
                if (checkList.Items[i].Selected)
                {
                    selval += checkList.Items[i].Value + ",";
                }
            }
            if (selval.TrimEnd(',') == "")
            {
                return "-2";
            }
            else
            {
                return selval.TrimEnd(',');
            }
        }
        #endregion

        #region 遍历所有节点
        //遍历所有节点
        private void SetNode(TreeNodeCollection tc, ref Hashtable ht)
        {
            foreach (TreeNode TNode in tc)
            {
                foreach (DictionaryEntry de in ht)
                {
                    if (TNode.Value == de.Key.ToString())
                    {
                        string blnode = de.Value.ToString();
                        if (blnode == "")
                            blnode = "True";
                        TNode.Expanded = bool.Parse(blnode);
                        SetNode(TNode.ChildNodes, ref ht);
                    }
                }
            }
        }
        #endregion

        #region 遍历所有节点
        //遍历所有节点
        public void GetNode(TreeNodeCollection tc, ref Hashtable ht)
        {
            foreach (TreeNode TNode in tc)
            {
                ht.Add(TNode.Value.ToString(), TNode.Expanded.ToString());
                GetNode(TNode.ChildNodes, ref ht);
            }
        }
        #endregion

        #region 选中部分节点
        //遍历所有节点
        public void SelectNode(TreeNodeCollection tc, string strdeplist)
        {
            foreach (TreeNode TNode in tc)
            {

                string[] deplist = strdeplist.Split(',');
                for (int i = 0; i < deplist.Length; i++)
                {
                    if (deplist[i] == TNode.Value)
                        TNode.Checked = true;
                }
                SelectNode(TNode.ChildNodes, strdeplist);
            }
        }
        #endregion

        #region 选中部分节点
        //遍历所有节点
        public void SelectAllNode(TreeNodeCollection tc)
        {
            foreach (TreeNode TNode in tc)
            {
                TNode.Checked = true;
                SelectAllNode(TNode.ChildNodes);
            }
        }
        #endregion
    }
}