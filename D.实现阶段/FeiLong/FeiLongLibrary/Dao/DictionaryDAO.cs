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
    public class DictionaryDAO
    {
        public void AddDictionaryData(Dictionary Dictionary)
        {
            DateTime dt = DateTime.Now;
            FLDbContext db = new FLDbContext();
            foreach (DictionaryData Data in Dictionary.DataList)
            {
                Data.CreateDate = dt;
                Data.CreateUserID = LoginHelper.CurrentUser.ID;
                Data.DictionaryID = Dictionary.ID;
                Data.IsDel = false;
                db.DictionaryData.Add(Data);
            }
            db.SaveChanges();
        }
    }
}
