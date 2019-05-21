using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IFMPLibrary.Entities;
using IFMPLibrary.Enums;
using IFMPLibrary.DBContext;
using System.Web;
using IFMPLibrary.Utils;

namespace IFMPLibrary.DAO
{
    public class ColumnDAO
    {
        //验证合法性
        public bool ValidateColumnRange(string ids, ColumnShowType ColumnShowType, int tabletypeid)
        {
            bool result = true;
            using (IFMPDBContext db = new IFMPDBContext())
            {
                List<int> idlist = new List<int>(ids.Split(',').Select(t => { int m = Convert.ToInt32(t); return m; }));
                List<TableColumn> TableColumnList = db.TableColumn.Where(t => idlist.Contains(t.ID)).ToList();
                List<Dictionary> DictionaryList = db.Dictionary.ToList();
                if (ColumnShowType == ColumnShowType.极值 || ColumnShowType == ColumnShowType.平均值 || ColumnShowType == ColumnShowType.最大值 || ColumnShowType == ColumnShowType.最小值 || ColumnShowType == ColumnShowType.求和)
                {
                    DictionaryList = DictionaryList.Where(t => t.RegexType == RegexType.非负整数
                        || t.RegexType == RegexType.实数
                         || t.RegexType == RegexType.有范围的数字
                          || t.RegexType == RegexType.整数
                           || t.RegexType == RegexType.正整数
                        ).ToList();

                    if (TableColumnList.Count != TableColumnList.Where(t => DictionaryList.Select(m => m.ID).Contains(t.DictionaryID.Value)).Count())
                    {
                        result = false;
                    }
                }
            }
            return result;
        }


        public object GetData(int tableid, TableColumn tablecolumn)
        {
            object returndata = null;
            using (IFMPDBContext db = new IFMPDBContext())
            {
                List<TableData> SelList = db.TableData.Where(t => t.TableID == tableid
                           && db.TableColumnRange.Where(m => m.TableColumnID == tablecolumn.ID).Select(m => m.SourceID).Contains(t.TableColumnID)).ToList();

                switch (tablecolumn.ColumnShowType.Value)
                {
                    case ColumnShowType.极值:
                        returndata = SelList.Select(t => { decimal m = Convert.ToDecimal(t.Data); return m; }).OrderByDescending(t => t).FirstOrDefault() - SelList.Select(t => { decimal m = Convert.ToDecimal(t.Data); return m; }).OrderBy(t => t).FirstOrDefault();
                        break;
                    case ColumnShowType.平均值:
                        returndata = System.Decimal.Round(SelList.Select(t => { decimal m = Convert.ToDecimal(t.Data); return m; }).Sum() / SelList.Count, 4);
                        break;
                    case ColumnShowType.最大值:
                        returndata = SelList.Select(t => { decimal m = Convert.ToDecimal(t.Data); return m; }).OrderByDescending(t => t).FirstOrDefault();
                        break;
                    case ColumnShowType.最小值:
                        returndata = SelList.Select(t => { decimal m = Convert.ToDecimal(t.Data); return m; }).OrderBy(t => t).FirstOrDefault();
                        break;
                    case ColumnShowType.求和:
                        returndata = SelList.Select(t => { decimal m = Convert.ToDecimal(t.Data); return m; }).Sum();
                        break;
                    default:
                        break;
                }

            }

            return returndata;
        }

    }
}
