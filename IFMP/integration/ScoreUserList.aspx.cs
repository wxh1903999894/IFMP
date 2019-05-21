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
using Aspose.Words;
using System.Data;
using System.IO;


namespace IFMP.integration
{
    public partial class ScoreUserList : PageBase
    {
        #region 参数集合 2:我的奖票 1:全部奖票
        public int SFlag
        {
            get
            {
                return GetQueryString<int>("sflag", 0);
            }
        }
        #endregion


        #region 页面初始化
        /// <summary>
        /// 页面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.hf_sflag.Value = SFlag.ToString();
                if (SFlag == 1)
                {
                    //this.btn_OutPutFull.Visible = false;
                }
                if (SFlag == 2)
                {
                    this.lbl_Menuname.Text = "我的奖票";
                    btn_OutPut.Visible = false;
                    btn_OutPutFull.Visible = false;
                }
                GetCondition();
                DataBindList();
            }
        }
        #endregion


        #region 获取查询条件
        public void GetCondition()
        {
            ViewState["begin"] = this.txt_Begin.Text.Trim() == "" ? "1900-01-01" : this.txt_Begin.Text.Trim();
            ViewState["end"] = this.txt_End.Text.Trim() == "" ? "9999-12-31" : this.txt_End.Text.Trim();
        }
        #endregion

        #region 数据绑定
        /// <summary>
        /// 数据绑定
        /// </summary>
        private void DataBindList()
        {
            int myheight = GetCookie<int>("ScreenH");
            //this.txt_RealName.Text = myheight.ToString();
            if (myheight > 800)
            {
                Pager.PageSize = 15;
            }

            //DateTime begindate = Convert.ToDateTime(ViewState["begin"]);
            //DateTime enddate = Convert.ToDateTime(ViewState["end"]);

            DateTime begindate = new BaseUtils().GetSelectDate(Convert.ToDateTime(ViewState["begin"]));
            DateTime enddate = new BaseUtils().GetSelectDate(Convert.ToDateTime(ViewState["end"]), false);

            using (IFMPDBContext db = new IFMPDBContext())
            {
                var list = from scoreuser in db.ScoreUser.Where(t => t.IsDel != true)
                           join score in db.Score.Where(t => t.IsDel != true && t.AuditState == AuditState.通过 && t.IsReward) on scoreuser.ScoreID equals score.ID
                           join user in db.User.Where(t => t.IsDel != true) on scoreuser.UserID equals user.ID
                           join recorduser in db.User.Where(t => t.IsDel != true) on score.CreateUserID equals recorduser.ID
                           join firstaudituser in db.User.Where(t => t.IsDel != true) on score.FirstAuditUserID equals firstaudituser.ID
                           join lastaudituser in db.User.Where(t => t.IsDel != true) on score.LastAuditUserID equals lastaudituser.ID
                           join scoreevent in db.ScoreEvent.Where(t => t.IsDel != true) on score.ScoreEventID equals scoreevent.ID
                           where score.CreateDate >= begindate && score.CreateDate <= enddate && (SFlag == 1 || (SFlag == 2 && scoreuser.UserID == UserID))
                           orderby score.CreateDate descending
                           select new
                                  {
                                      scoreuser.ID,
                                      RealName = user.RealName,
                                      RecordUserName = recorduser.RealName,
                                      FirstAuditUserName = firstaudituser.RealName,
                                      LastAuditUserName = lastaudituser.RealName,
                                      scoreuser.BScore,
                                      score.Title,
                                      score.Content,
                                      EventName = scoreevent.Name,
                                      score.CreateDate,
                                      scoreuser.IsPrint
                                  };


                int total = list.Count();
                if (total > 0)
                {
                    this.tr_null.Visible = false;
                }
                else
                {
                    this.tr_null.Visible = true;
                }
                this.rp_List.DataSource = list.Skip((Pager.CurrentPageIndex - 1) * Pager.PageSize).Take(Pager.PageSize).ToList();
                Pager.RecordCount = total;
                rp_List.DataBind();
                this.hf_CheckIDS.Value = "";
            }
        }
        #endregion


        #region 分页事件
        /// <summary>
        /// 分页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Pager_PageChanged(object sender, EventArgs e)
        {
            DataBindList();
        }
        #endregion


        #region 查询事件
        /// <summary>
        /// 查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Query_Click(object sender, EventArgs e)
        {
            Pager.CurrentPageIndex = 1;
            GetCondition();
            DataBindList();
        }
        #endregion


        #region 导出事件,这个没做
        /// <summary>
        /// 导出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Out_Click(object sender, EventArgs e)
        {
            string ids = this.hf_CheckIDS.Value.ToString();
            try
            {
                if (string.IsNullOrEmpty(ids))
                {
                    ShowMessage("请至少选择一项");
                    return;
                }
                List<int> idlist = new List<int>();
                foreach (string id in ids.TrimEnd(',').Split(','))
                {
                    idlist.Add(Convert.ToInt32(id));
                }
                string PhysicalAddress = System.AppDomain.CurrentDomain.BaseDirectory.ToString();
                Document backdoc = new Document(PhysicalAddress + "/Template/ScoreBack.docx");
                Document frontdoc = new Document(PhysicalAddress + "/Template/ScoreFront.docx");
                Document doc = new Document(PhysicalAddress + "/Template/ScoreBack.docx");
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    var list = (from scoreuser in db.ScoreUser
                                join score in db.Score on scoreuser.ScoreID equals score.ID
                                join rewarduser in db.User on scoreuser.UserID equals rewarduser.ID
                                join firstaudituser in db.User on score.FirstAuditUserID equals firstaudituser.ID
                                join lastaudituser in db.User on score.FirstAuditUserID equals lastaudituser.ID
                                where score.IsDel != true && scoreuser.IsDel != true
                                && scoreuser.IsPrint != true
                               && idlist.Contains(scoreuser.ID)
                               && rewarduser.IsDel != true
                                && score.AuditState == AuditState.通过
                                select new
                                {
                                    RewardUserName = rewarduser.RealName,
                                    FirstAuditUserName = firstaudituser.RealName,
                                    LastAuditUserName = lastaudituser.RealName,
                                    score.Title,
                                    scoreuser.BScore,
                                    RewardDate = score.CreateDate,
                                    score.Content
                                }).ToList();


                    for (int i = 0; i < list.Count; i = i + 4)
                    {
                        if (i + 4 >= list.Count)
                        {
                            //最后一组
                            for (int j = i; j < list.Count; j++)
                            {
                                if (j == i)
                                {
                                    if (j != 0)
                                    {
                                        appendDoc(doc, backdoc, true);
                                    }
                                }
                                else
                                {
                                    appendDoc(doc, backdoc);
                                }

                            }

                            for (int j = i; j < list.Count; j++)
                            {
                                frontdoc = new Document(PhysicalAddress + "/Template/ScoreFront.docx");
                                frontdoc.Range.Replace("@UserName", list[j].RewardUserName, false, false);
                                frontdoc.Range.Replace("@DateTime", list[j].RewardDate.Value.ToString("yyyy-MM-dd"), false, false);
                                frontdoc.Range.Replace("@STitle", list[j].Title, false, false);
                                frontdoc.Range.Replace("@BSCore", list[j].BScore.ToString(), false, false);
                                frontdoc.Range.Replace("@FirstAduitUserName", list[j].FirstAuditUserName, false, false);
                                frontdoc.Range.Replace("@LastAduitUserName", list[j].LastAuditUserName, false, false);
                                frontdoc.Range.Replace("@EventMark", list[j].Content, false, false);

                                frontdoc.Range.Replace("@Year", DateTime.Now.ToString("yyyy"), false, false);
                                frontdoc.Range.Replace("@Month", DateTime.Now.ToString("MM"), false, false);
                                frontdoc.Range.Replace("@Day", DateTime.Now.ToString("dd"), false, false);

                                if (j == i)
                                {
                                    appendDoc(doc, frontdoc, true);
                                }
                                else
                                {
                                    appendDoc(doc, frontdoc);
                                }

                            }

                        }
                        else
                        {
                            //前面的组
                            if (i % 4 == 0)
                            {
                                for (int j = i; j < i + 4; j++)
                                {
                                    if (j == i)
                                    {
                                        if (j != 0)
                                        {
                                            appendDoc(doc, backdoc, true);
                                        }
                                    }
                                    else
                                    {
                                        appendDoc(doc, backdoc);
                                    }
                                }

                                for (int j = i; j < i + 4; j++)
                                {
                                    frontdoc = new Document(PhysicalAddress + "/Template/ScoreFront.docx");
                                    frontdoc.Range.Replace("@UserName", list[j].RewardUserName, false, false);
                                    frontdoc.Range.Replace("@DateTime", list[j].RewardDate.Value.ToString("yyyy-MM-dd"), false, false);
                                    frontdoc.Range.Replace("@STitle", list[j].Title, false, false);
                                    frontdoc.Range.Replace("@BSCore", list[j].BScore.ToString(), false, false);
                                    frontdoc.Range.Replace("@FirstAduitUserName", list[j].FirstAuditUserName, false, false);
                                    frontdoc.Range.Replace("@LastAduitUserName", list[j].LastAuditUserName, false, false);
                                    frontdoc.Range.Replace("@EventMark", list[j].Content, false, false);

                                    frontdoc.Range.Replace("@Year", DateTime.Now.ToString("yyyy"), false, false);
                                    frontdoc.Range.Replace("@Month", DateTime.Now.ToString("MM"), false, false);
                                    frontdoc.Range.Replace("@Day", DateTime.Now.ToString("dd"), false, false);

                                    if (j == i)
                                    {
                                        appendDoc(doc, frontdoc, true);
                                    }
                                    else
                                    {
                                        appendDoc(doc, frontdoc);
                                    }
                                }
                            }

                        }
                    }


                    doc.MailMerge.DeleteFields();

                    if (File.Exists(PhysicalAddress + "/Template/Score.docx"))
                    {
                        File.Delete(PhysicalAddress + "/Template/Score.docx");
                    }

                    doc.Save(PhysicalAddress + "/Template/Score.docx", Aspose.Words.SaveFormat.Docx);

                    //设为已打印
                    try
                    {
                        foreach (ScoreUser scoreuser in db.ScoreUser.Where(t => idlist.Contains(t.ID) && t.IsPrint != true).ToList())
                        {
                            scoreuser.IsPrint = true;
                        }

                        new SysLogDAO().AddLog(LogType.操作日志_导出, "导出奖票", UserID);
                        db.SaveChanges();

                        FileInfo fileInfo = new FileInfo(PhysicalAddress + "/Template/Score.docx");
                        Response.Clear();
                        Response.ClearContent();
                        Response.ClearHeaders();
                        Response.AddHeader("Content-Disposition", "attachment;filename=积分制奖票.docx");
                        Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                        Response.AddHeader("Content-Transfer-Encoding", "binary");
                        Response.ContentType = "application/octet-stream";
                        Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
                        Response.WriteFile(fileInfo.FullName);

                        //Response.Redirect("ScoreUserList.aspx", false);
                        Response.Flush();

                        Context.ApplicationInstance.CompleteRequest();


                    }
                    catch
                    {

                    }
                }


            }
            catch (Exception ex)
            {

                return;
            }
        }
        #endregion



        #region 导出事件
        /// <summary>
        /// 导出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_OutFull_Click(object sender, EventArgs e)
        {
            try
            {
                string PhysicalAddress = System.AppDomain.CurrentDomain.BaseDirectory.ToString();
                Document backdoc = new Document(PhysicalAddress + "/Template/ScoreBack.docx");
                Document frontdoc = new Document(PhysicalAddress + "/Template/ScoreFront.docx");
                Document doc = new Document(PhysicalAddress + "/Template/ScoreBack.docx");
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    var list = (from scoreuser in db.ScoreUser
                                join score in db.Score on scoreuser.ScoreID equals score.ID
                                join rewarduser in db.User on scoreuser.UserID equals rewarduser.ID
                                join firstaudituser in db.User on score.FirstAuditUserID equals firstaudituser.ID
                                join lastaudituser in db.User on score.FirstAuditUserID equals lastaudituser.ID
                                where score.IsDel != true
                                && score.IsReward
                                && scoreuser.IsDel != true
                                && scoreuser.IsPrint != true
                               && rewarduser.IsDel != true
                               && score.AuditState == AuditState.通过
                                select new
                                {
                                    RewardUserName = rewarduser.RealName,
                                    FirstAuditUserName = firstaudituser.RealName,
                                    LastAuditUserName = lastaudituser.RealName,
                                    score.Title,
                                    scoreuser.BScore,
                                    RewardDate = score.CreateDate,
                                    score.Content
                                }).ToList();


                    for (int i = 0; i < list.Count; i = i + 4)
                    {
                        if (i + 4 >= list.Count)
                        {
                            //最后一组
                            for (int j = i; j < list.Count; j++)
                            {
                                if (j == i)
                                {
                                    if (j != 0)
                                    {
                                        appendDoc(doc, backdoc, true);
                                    }
                                }
                                else
                                {
                                    appendDoc(doc, backdoc);
                                }

                            }

                            for (int j = i; j < list.Count; j++)
                            {
                                frontdoc = new Document(PhysicalAddress + "/Template/ScoreFront.docx");
                                frontdoc.Range.Replace("@UserName", list[j].RewardUserName, false, false);
                                frontdoc.Range.Replace("@DateTime", list[j].RewardDate.Value.ToString("yyyy-MM-dd"), false, false);
                                frontdoc.Range.Replace("@STitle", list[j].Title, false, false);
                                frontdoc.Range.Replace("@BSCore", list[j].BScore.ToString(), false, false);
                                frontdoc.Range.Replace("@FirstAduitUserName", list[j].FirstAuditUserName, false, false);
                                frontdoc.Range.Replace("@LastAduitUserName", list[j].LastAuditUserName, false, false);
                                frontdoc.Range.Replace("@EventMark", list[j].Content, false, false);

                                frontdoc.Range.Replace("@Year", DateTime.Now.ToString("yyyy"), false, false);
                                frontdoc.Range.Replace("@Month", DateTime.Now.ToString("MM"), false, false);
                                frontdoc.Range.Replace("@Day", DateTime.Now.ToString("dd"), false, false);

                                if (j == i)
                                {
                                    appendDoc(doc, frontdoc, true);
                                }
                                else
                                {
                                    appendDoc(doc, frontdoc);
                                }

                            }

                        }
                        else
                        {
                            //前面的组
                            if (i % 4 == 0)
                            {
                                for (int j = i; j < i + 4; j++)
                                {
                                    if (j == i)
                                    {
                                        if (j != 0)
                                        {
                                            appendDoc(doc, backdoc, true);
                                        }
                                    }
                                    else
                                    {
                                        appendDoc(doc, backdoc);
                                    }
                                }

                                for (int j = i; j < i + 4; j++)
                                {
                                    frontdoc = new Document(PhysicalAddress + "/Template/ScoreFront.docx");
                                    frontdoc.Range.Replace("@UserName", list[j].RewardUserName, false, false);
                                    frontdoc.Range.Replace("@DateTime", list[j].RewardDate.Value.ToString("yyyy-MM-dd"), false, false);
                                    frontdoc.Range.Replace("@STitle", list[j].Title, false, false);
                                    frontdoc.Range.Replace("@BSCore", list[j].BScore.ToString(), false, false);
                                    frontdoc.Range.Replace("@FirstAduitUserName", list[j].FirstAuditUserName, false, false);
                                    frontdoc.Range.Replace("@LastAduitUserName", list[j].LastAuditUserName, false, false);
                                    frontdoc.Range.Replace("@EventMark", list[j].Content, false, false);

                                    frontdoc.Range.Replace("@Year", DateTime.Now.ToString("yyyy"), false, false);
                                    frontdoc.Range.Replace("@Month", DateTime.Now.ToString("MM"), false, false);
                                    frontdoc.Range.Replace("@Day", DateTime.Now.ToString("dd"), false, false);

                                    if (j == i)
                                    {
                                        appendDoc(doc, frontdoc, true);
                                    }
                                    else
                                    {
                                        appendDoc(doc, frontdoc);
                                    }
                                }
                            }

                        }
                    }


                    doc.MailMerge.DeleteFields();

                    if (File.Exists(PhysicalAddress + "/Template/Score.docx"))
                    {
                        File.Delete(PhysicalAddress + "/Template/Score.docx");
                    }

                    doc.Save(PhysicalAddress + "/Template/Score.docx", Aspose.Words.SaveFormat.Docx);

                    //设为已打印
                    try
                    {
                        foreach (ScoreUser scoreuser in db.ScoreUser.Where(t => t.IsPrint != true
                            && t.IsDel != true
                            && db.Score.Where(m => m.IsDel != true && m.AuditState == AuditState.通过).Select(m => m.ID).Contains(t.ScoreID)).ToList())
                        {
                            scoreuser.IsPrint = true;
                        }

                        new SysLogDAO().AddLog(LogType.操作日志_导出, "导出奖票", UserID);
                        db.SaveChanges();
                        FileInfo fileInfo = new FileInfo(PhysicalAddress + "/Template/Score.docx");
                        Response.Clear();
                        Response.ClearContent();
                        Response.ClearHeaders();
                        Response.AddHeader("Content-Disposition", "attachment;filename=积分制奖票.docx");
                        Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                        Response.AddHeader("Content-Transfer-Encoding", "binary");
                        Response.ContentType = "application/octet-stream";
                        Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
                        Response.WriteFile(fileInfo.FullName);

                        //Response.Redirect("ScoreUserList.aspx", false);
                        Response.Flush();

                        Context.ApplicationInstance.CompleteRequest();


                    }
                    catch
                    {

                    }
                }
            }
            catch (Exception ex)
            {

                return;
            }


        }
        #endregion


        public void DownLoad()
        {
            string PhysicalAddress = System.AppDomain.CurrentDomain.BaseDirectory.ToString();
            FileInfo fileInfo = new FileInfo(PhysicalAddress + "/Resource/Score.docx");
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "attachment;filename=积分制奖票.docx");
            Response.AddHeader("Content-Length", fileInfo.Length.ToString());
            Response.AddHeader("Content-Transfer-Encoding", "binary");
            Response.ContentType = "multipart/form-data";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            Response.WriteFile(fileInfo.FullName);

            //Response.Redirect("ScoreUserList.aspx");
            Response.Flush();
            Context.ApplicationInstance.CompleteRequest();
        }

        public static void insertDocumentAfterNode(Node insertAfterNode, Document mainDoc, Document srcDoc)
        {

            // Make sure that the node is either a pargraph or table.
            if ((insertAfterNode.NodeType != NodeType.Paragraph)
            & (insertAfterNode.NodeType != NodeType.Table))
                throw new Exception("The destination node should be either a paragraph or table.");

            //We will be inserting into the parent of the destination paragraph.
            CompositeNode dstStory = insertAfterNode.ParentNode;
            //Remove empty paragraphs from the end of document
            while (null != srcDoc.LastSection.Body.LastParagraph && !srcDoc.LastSection.Body.LastParagraph.HasChildNodes)
            {
                srcDoc.LastSection.Body.LastParagraph.Remove();
            }

            NodeImporter importer = new NodeImporter(srcDoc, mainDoc, ImportFormatMode.KeepSourceFormatting);
            //Loop through all sections in the source document.
            int sectCount = srcDoc.Sections.Count;
            for (int sectIndex = 0; sectIndex < sectCount; sectIndex++)
            {
                Section srcSection = srcDoc.Sections[sectIndex];
                //Loop through all block level nodes (paragraphs and tables) in the body of the section.
                int nodeCount = srcSection.Body.ChildNodes.Count;
                for (int nodeIndex = 0; nodeIndex < nodeCount; nodeIndex++)
                {
                    Node srcNode = srcSection.Body.ChildNodes[nodeIndex];
                    Node newNode = importer.ImportNode(srcNode, true);
                    dstStory.InsertAfter(newNode, insertAfterNode);
                    insertAfterNode = newNode;
                }
            }
        }


        public static void appendDoc(Document dstDoc, Document srcDoc, bool includeSection = false)
        {
            // Loop through all sections in the source document.
            // Section nodes are immediate children of the Document node so we can
            // just enumerate the Document.
            if (includeSection)
            {
                foreach (Section srcSection in srcDoc.Sections)
                {
                    Node dstNode = dstDoc.ImportNode(srcSection, true, ImportFormatMode.UseDestinationStyles);
                    dstDoc.AppendChild(dstNode);
                }
            }
            else
            {
                //find the last paragraph of the last section
                Node node = dstDoc.LastSection.Body.LastParagraph;
                if (node == null)
                {
                    node = new Paragraph(srcDoc);
                    dstDoc.LastSection.Body.AppendChild(node);
                }
                if ((node.NodeType != NodeType.Paragraph) & (node.NodeType != NodeType.Table))
                {
                    throw new Exception("Use appendDoc(dstDoc, srcDoc, true) instead of appendDoc(dstDoc, srcDoc, false)");
                }
                insertDocumentAfterNode(node, dstDoc, srcDoc);
            }
        }
    }
}