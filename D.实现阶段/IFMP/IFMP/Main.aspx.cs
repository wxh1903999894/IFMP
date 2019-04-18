/*****************************************************************
** Copyright (c) 芜湖市高科电子有限公司
** 创 建 人:      樊紫红
** 创建日期:      2018年8月9日 15时18分19秒
** 描    述:      我的任务详情页面
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
using System.Text;

namespace IFMP
{
    public partial class Main : PageBase
    {
        IFMPDBContext db = new IFMPDBContext();

        #region 页面初始化
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.ltl_DateTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
                NoticeBind();
                //GetDate();
            }
        }
        #endregion


        #region 绑定通知消息
        private void NoticeBind()
        {
            List<Notice> noticelist = db.Notice.Where(t => t.ReciveUserID == UserID).OrderByDescending(t => t.SendDate).ToList();
            if (noticelist.Count > 0)
            {
                this.tr_null.Visible = false;
            }
            else
            {
                this.tr_null.Visible = true;
            }
            this.rp_List.DataSource = noticelist.Take(8);
            this.rp_List.DataBind();
        }
        #endregion


        #region 绑定日历信息
        private void GetDate()
        {
            //StringBuilder sb = new StringBuilder("");
            //sb.Append("<script type='text/javascript'>");
            //sb.Append("var myChart = echarts.init(document.getElementById('divclander'));");
            //sb.Append("var dateList = [");
            //DateTime dtNow = DateTime.Now;
            //int days = DateTime.DaysInMonth(dtNow.Year, dtNow.Month);
            //for (int i = 0; i < days; i++)
            //{
            //    sb.Append("['" + dtNow.Year + "-" + dtNow.Month + "-" + (i + 1) + "','勤学班', '奋进班'],");
            //}
            //sb.Append("];");
            //sb.Append("var heatmapData = [];");
            //sb.Append("var lunarData = [];");
            //sb.Append("for (var i = 0; i < dateList.length; i++) {");
            //sb.Append("    lunarData.push([");
            //sb.Append("        dateList[i][0],");
            //sb.Append("        1,");
            //sb.Append("        dateList[i][1],");
            //sb.Append("        dateList[i][2],");
            //sb.Append("        dateList[i][3],");
            //sb.Append("    ]);");
            //sb.Append("}");
            //sb.Append("var option = {");
            //sb.Append("    calendar: [{");
            //sb.Append("        left: 'center',");
            //sb.Append("        top: 'middle',");
            //sb.Append("        cellSize: [70, 100],");
            //sb.Append("        yearLabel: { show: false },");
            //sb.Append("        orient: 'vertical',");
            //sb.Append("        dayLabel: {");
            //sb.Append("            firstDay: 1,");
            //sb.Append("            nameMap: 'cn'");
            //sb.Append("        },");
            //sb.Append("        monthLabel: {");
            //sb.Append("            show: false");
            //sb.Append("        },");
            //sb.Append("        range: '" + dtNow.Year + "-" + dtNow.Month + "'");
            //sb.Append("    }],");
            //sb.Append("    series: [{");
            //sb.Append("        type: 'scatter',");
            //sb.Append("        coordinateSystem: 'calendar',");
            //sb.Append("        symbolSize: 1,");
            //sb.Append("        label: {");
            //sb.Append("            normal: {");
            //sb.Append("                show: true,");
            //sb.Append("                formatter: function (params) {");
            //sb.Append("                    var d = echarts.number.parseDate(params.value[0]);");
            //sb.Append("                    if (d.getDate() == 10) {");
            //sb.Append("                        return d.getDate() + '\\n\\n' + params.value[2] + '\\n\\n' + (params.value[3] || '') + '\\n\\n' + (params.value[4] || '');");
            //sb.Append("                    } else {");
            //sb.Append("                        return d.getDate() + '\\n\\n' + params.value[2] + '\\n\\n' + (params.value[3] || '') + '\\n\\n' + (params.value[4] || '');");
            //sb.Append("                    }");
            //sb.Append("                },");
            //sb.Append("                textStyle: {");
            //sb.Append("                    color: '#000'");
            //sb.Append("                },");
            //sb.Append("            }");
            //sb.Append("        },");
            //sb.Append("        data: lunarData");
            //sb.Append("    }]");
            //sb.Append("};");
            //sb.Append("myChart.setOption(option);");
            //sb.Append("</script>");
            //this.ltl_Content.Text = sb.ToString();
        } 
        #endregion
    }
}