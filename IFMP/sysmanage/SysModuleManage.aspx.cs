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
using System.Web.UI.WebControls;

using GK.IFMP.Common;
using IFMPLibrary.DBContext;
using IFMPLibrary.Entities;

namespace IFMP.sysmanage
{
    public partial class SysModuleManage : PageBase
    {
        IFMPDBContext db = new IFMPDBContext();

        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                TreeBind();
            }
        }
        #endregion


        #region 绑定树
        /// <summary>
        /// 绑定树
        /// </summary>
        private void TreeBind()
        {
            this.tv_Meun.Nodes.Clear();
            List<SysModule> modulelist = db.SysModule.Where(t => t.ParentID == -1).OrderBy(t => t.ModuleOrder).ToList();
            if (modulelist.Count > 0)
            {
                for (int i = 0; i < modulelist.Count; i++)
                {
                    TreeNode treenode = new TreeNode();
                    int moduleid = Convert.ToInt32(modulelist[i].ID);
                    treenode.Value = moduleid.ToString();
                    treenode.Text = modulelist[i].Name.ToString();
                    tv_Meun.Nodes.Add(treenode);
                    treenode.Expanded = false;
                    if (i == 0)
                    {
                        treenode.Expanded = true;
                        framemain.Attributes["src"] = "SysModuleEdit.aspx?id=" + moduleid.ToString() + "&deep=" + treenode.Depth.ToString();
                    }
                    ChildBind(treenode, moduleid);
                }
            }
        }
        #endregion


        #region 绑定子节点
        /// <summary>
        /// 绑定子节点
        /// </summary>
        /// <param name="treenode"></param>
        /// <param name="depid"></param>
        private void ChildBind(TreeNode treenode, int moduleid)
        {
            List<SysModule> modulelist = db.SysModule.Where(t => t.ParentID == moduleid).OrderBy(t => t.ModuleOrder).ToList();
            if (modulelist.Count > 0)
            {
                for (int i = 0; i < modulelist.Count; i++)
                {
                    TreeNode childnode = new TreeNode();
                    childnode.Value = modulelist[i].ID.ToString();
                    childnode.Text = modulelist[i].Name.ToString();
                    int childid = Convert.ToInt32(modulelist[i].ID.ToString());
                    childnode = new TreeNode(childnode.Text, childnode.Value);
                    treenode.ChildNodes.Add(childnode);
                    childnode.Expanded = false;
                    ChildBind(childnode, childid);
                }
            }
        }
        #endregion


        #region 添加事件
        protected void btn_Add_Click(object sender, EventArgs e)
        {
            framemain.Attributes["src"] = "SysModuleEdit.aspx?id=" + "" + "&deep=1";
        } 
        #endregion


        #region 菜单点击事件
        protected void tv_Meun_SelectedNodeChanged(object sender, EventArgs e)
        {
            string svalue = tv_Meun.SelectedNode.Value;//获得点击的值
            framemain.Attributes["src"] = "SysModuleEdit.aspx?id=" + svalue + "&deep=" + tv_Meun.SelectedNode.Depth.ToString();
        } 
        #endregion
    }
}