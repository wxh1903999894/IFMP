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
    public class FlowDAO
    {
        public FLDbContext db = new FLDbContext();
      
        public List<Flow> GetFlow(TableTypeEnums TableType)
        {
            return db.Flow.Where(t => t.TableType == TableType).ToList();
        }

    }
}
