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
    public class FlowDAO
    {
        public List<Flow> GetFlowLevel(List<Flow> FlowList)
        {
            int level = 0;
            FlowList.Where(t => t.ParentID == 0).ToList().ForEach(t => t.Level = level);
            List<Flow> SelectList = FlowList.Where(t => t.Level == level).ToList();
            while (SelectList.Count > 0)
            {
                level = level + 1;
                FlowList.Where(t => SelectList.Select(m => m.ID).Contains(t.ParentID)).ToList().ForEach(t => t.Level = level);
                SelectList = FlowList.Where(t => t.Level == level).ToList();
            }

            return FlowList.OrderBy(t => t.Level).ToList();
        }
    }
}
