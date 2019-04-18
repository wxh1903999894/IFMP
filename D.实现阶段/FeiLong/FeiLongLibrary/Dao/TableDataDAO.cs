using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeiLongLibrary.Entities;
using FeiLongLibrary.Enums;
using FeiLongLibrary.DBContext;
using FeiLongLibrary.Utils;

namespace FeiLongLibrary.DAO
{
    public class TableDataDAO
    {
        public bool DictionaryValidate(List<TableData> tabledatalist)
        {
            bool result = true;
            using (FLDbContext db = new FLDbContext())
            {
                foreach (TableData tabledata in tabledatalist)
                {
                    TableColumn tc = db.TableColumn.FirstOrDefault(t => t.ID == tabledata.TableColumnID);
                    if (tc.DictionaryID == null)
                        continue;

                    Dictionary dictionary = db.Dictionary.FirstOrDefault(t => t.ID == tc.DictionaryID);
                    if (dictionary.DisplayType == DictionaryTypeEnums.单选)
                    {
                        //判断是否在所选的值之中
                        if (db.DictionaryData.FirstOrDefault(t => t.RexgexData == tabledata.Data || t.ID == Convert.ToInt32(tabledata.Data)) == null)
                            return false;
                    }
                    else
                    {
                        //判断是否满足
                        DictionaryData dictionarydata = db.DictionaryData.FirstOrDefault(t => t.DictionaryID == dictionary.ID);
                        if (dictionarydata.RegexType == RegexType.特殊的一组字符)
                        {
                            result = new BaseUtils().GetRegex(tabledata.Data, dictionarydata.RegexType, dictionarydata.RexgexData.ToCharArray());
                            if (!result)
                                break;
                        }
                        else
                        {
                            result = new BaseUtils().GetRegex(tabledata.Data, dictionarydata.RegexType);
                            if (!result)
                                break;
                        }
                    }

                }
            }
            return result;
        }
    }
}
