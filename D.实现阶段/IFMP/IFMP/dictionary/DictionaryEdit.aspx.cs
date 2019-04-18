/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年7月27日 14时36分19秒
** 描    述:      字典信息管理页面
** 修 改 人:      
** 修改日期:    
** 修改说明: 
**-----------------------------------------------------------------
*****************************************************************/
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

namespace IFMP.dictionary
{
    public partial class DictionaryEdit : PageBase
    {
        IFMPDBContext db = new IFMPDBContext();

        #region 参数集合
        public int DID
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
                CommonFunction.BindEnum<DictionaryTypeEnums>(this.ddl_DisplayType, "-99");
                CommonFunction.BindEnum<RegexType>(this.ddl_RegexType, "-2");
                if (DID != -1)
                {
                    this.ddl_DisplayType.Enabled = false;
                    InfoBind();
                }
            }
        }
        #endregion


        #region 初始化用户数据
        private void InfoBind()
        {
            Dictionary model = db.Dictionary.FirstOrDefault(t => t.ID == DID);
            if (model != null)
            {
                this.txt_Name.Text = model.Name;
                this.txt_RegexData.Text = model.RegexData;
                this.ddl_DisplayType.SelectedValue = Convert.ToInt32(model.DisplayType).ToString();
                this.ddl_RegexType.SelectedValue = Convert.ToInt32(model.RegexType).ToString();
                if (model.DisplayType == DictionaryTypeEnums.单选)
                {
                    this.trinfo.Visible = true;
                    this.trvis.Visible = true;
                    DataBindList();
                }
            }
        }
        #endregion


        #region 选项绑定事件
        private void DataBindList()
        {
            List<DictionaryData> datalist = db.DictionaryData.Where(t => t.IsDel != true && t.DictionaryID == DID).ToList();
            if (datalist.Count > 0)
            {
                this.tr_null.Visible = false;
            }
            else
            {
                this.tr_null.Visible = true;
            }
            this.rp_List.DataSource = datalist;
            this.rp_List.DataBind();
        }
        #endregion


        #region 提交事件
        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            try
            {
                if (((DictionaryTypeEnums)Convert.ToInt32(this.ddl_DisplayType.SelectedValue) == DictionaryTypeEnums.填写) && this.ddl_RegexType.SelectedValue == "-2")
                {
                    ShowMessage("字典类型为【填写】时，请选择验证内容信息");
                    return;
                }
                string message = "";
                Dictionary model = db.Dictionary.FirstOrDefault(t => t.ID == DID && t.IsDel != null);
                if (model == null)
                {
                    model = new Dictionary();
                    model.Name = this.txt_Name.Text.ToString().Trim();
                    RegexType RegexType = new RegexType();
                    if (Enum.TryParse(this.ddl_RegexType.SelectedValue, out  RegexType))
                    {
                        if (Enum.IsDefined(typeof(RegexType), RegexType))
                            model.RegexType = RegexType;
                    }
                    model.DisplayType = (DictionaryTypeEnums)Convert.ToInt32(this.ddl_DisplayType.SelectedValue);
                    if (!string.IsNullOrEmpty(this.txt_RegexData.Text.ToString().Trim()))
                        model.RegexData = this.txt_RegexData.Text.ToString().Trim();
                    model.IsDel = false;
                    model.CreateDate = DateTime.Now;
                    model.CreateUserID = UserID;

                    if ((model.RegexType == RegexType.有范围的数字 || model.RegexType == RegexType.特殊的一组字符) && this.txt_RegexData.Text == "")
                    {
                        ShowMessage("验证内容为【有范围的数据】或【特殊的一组字符】时，请填写范围数据信息");
                        return;
                    }

                    if (db.Dictionary.FirstOrDefault(t => t.Name == model.Name) != null)
                    {
                        ShowMessage("字典名称已存在，请修改后重新添加");
                        return;
                    }
                    db.Dictionary.Add(model);
                    db.SaveChanges();

                    if (model.DisplayType == DictionaryTypeEnums.单选)
                    {
                        DictionaryData dmodel = new DictionaryData();
                        dmodel.DictionaryID = model.ID;
                        dmodel.CreateDate = DateTime.Now;
                        dmodel.CreateUserID = UserID;
                        dmodel.IsDel = false;
                        string[] data = Request.Form.GetValues("option");
                        for (int i = 0; i < data.Length; i++)
                        {
                            dmodel.Data = data[i].ToString();
                            if (dmodel.Data == "")
                            {
                                ShowMessage("选项内容不能为空");
                                return;
                            }
                            else
                            {
                                db.DictionaryData.Add(dmodel);
                                db.SaveChanges();
                            }
                        }
                    }

                    message = "添加名称为【" + model.Name + "】的字典信息";
                    new SysLogDAO().AddLog(LogType.操作日志_添加, message, UserID);
                }
                else
                {
                    model.Name = this.txt_Name.Text.ToString().Trim();
                    RegexType RegexType = new RegexType();
                    if (Enum.TryParse(this.ddl_RegexType.SelectedValue, out  RegexType))
                    {
                        if (Enum.IsDefined(typeof(RegexType), RegexType))
                            model.RegexType = RegexType;
                    }
                    model.DisplayType = (DictionaryTypeEnums)Convert.ToInt32(this.ddl_DisplayType.SelectedValue);
                    model.RegexData = this.txt_RegexData.Text.ToString().Trim();

                    if ((model.RegexType == RegexType.有范围的数字 || model.RegexType == RegexType.特殊的一组字符) && this.txt_RegexData.Text == "")
                    {
                        ShowMessage("验证内容为【有范围的数据】或【特殊的一组字符】时，请填写范围数据信息");
                        return;
                    }

                    if (db.Dictionary.FirstOrDefault(t => t.Name == model.Name && t.ID != DID) != null)
                    {
                        ShowMessage("字典名称已存在，请修改后重新添加");
                        return;
                    }

                    if (model.DisplayType == DictionaryTypeEnums.单选)
                    {
                        DictionaryData dmodel = new DictionaryData();
                        dmodel.DictionaryID = model.ID;
                        dmodel.CreateDate = DateTime.Now;
                        dmodel.CreateUserID = UserID;
                        dmodel.IsDel = false;
                        string[] data = Request.Form.GetValues("option");
                        for (int i = 0; i < data.Length; i++)
                        {
                            dmodel.Data = data[i].ToString();
                            if (dmodel.Data == "")
                            {
                                ShowMessage("选项内容不能为空");
                                return;
                            }
                            else
                            {
                                db.DictionaryData.Add(dmodel);
                                db.SaveChanges();
                            }
                        }
                    }

                    message = "修改名称为【" + model.Name + "】的字典信息";
                    new SysLogDAO().AddLog(LogType.操作日志_修改, message, UserID);
                    db.SaveChanges();
                }
                ShowMessage();
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
                new SysLogDAO().AddLog(LogType.系统日志, ex.Message, UserID);
            }
        }
        #endregion


        protected void ddl_DisplayType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(this.ddl_DisplayType.SelectedValue) == Convert.ToInt32(DictionaryTypeEnums.单选))
            {
                this.trvis.Visible = true;
                this.trtitle.Visible = true;
            }
            else
            {
                this.trvis.Visible = false;
                this.trinfo.Visible = false;
                this.trtitle.Visible = false;
            }
        }

        #region 选项删除事件
        protected void ImageButton3_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton imgbtn = (ImageButton)sender;
                int id = Convert.ToInt32(imgbtn.CommandArgument.ToString());

                DictionaryData ddate = db.DictionaryData.FirstOrDefault(t => t.ID == id);
                if (ddate != null)
                {
                    ddate.IsDel = true;
                }
                else
                {
                    ShowMessage("删除失败");
                    return;
                }
                db.SaveChanges();
                ShowMessage("删除成功");
                DataBindList();
                new SysLogDAO().AddLog(LogType.操作日志_删除, "删除字典信息", UserID);
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
                new SysLogDAO().AddLog(LogType.系统日志, ex.Message, UserID);
                return;
            }
        }
        #endregion
    }
}