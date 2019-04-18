using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using IFMPLibrary.DAO;
using IFMPLibrary.Enums;
using IFMPLibrary.Entities;
using IFMPLibrary.DBContext;
using IFMPLibrary.Utils;

using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IFMP.ashx
{
    /// <summary>
    /// Statistics 的摘要说明
    /// </summary>
    public class Statistics : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string method = context.Request.Params["method"];
            switch (method)
            {
                case "GetTask":
                    GetTask(context);
                    break;
                case "GetStatisticsTableType":
                    GetStatisticsTableType(context);
                    break;
                case "GetStaticColumn":
                    GetStaticColumn(context);
                    break;
                case "GetNotStaticColumn":
                    GetNotStaticColumn(context);
                    break;
                case "GetCompareDateByTask":
                    GetCompareDateByTask(context);
                    break;
                case "GetCompareDateByUser":
                    GetCompareDateByUser(context);
                    break;
                case "GetStatisticsByDate":
                    GetStatisticsByDate(context);
                    break;
                case "GetClassStatistics":
                    GetClassStatistics(context);
                    break;
                case "GetDateStatistics":
                    GetDateStatistics(context);
                    break;
                case "GetStatisticsDeviceDataType":
                    GetStatisticsDeviceDataType(context);
                    break;
                case "GetStatisticsDevice":
                    GetStatisticsDevice(context);
                    break;
                case "GetStatisticsDeviceData":
                    GetStatisticsDeviceData(context);
                    break;
                case "GetAlert":
                    GetAlert(context);
                    break;
                default:
                    break;
            }
        }

        public void GetTask(HttpContext context)
        {
            JObject returnobj = new JObject();
            JArray jarray = new JArray();
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    int TableType = Convert.ToInt32(context.Request["tabletype"]);
                    DateTime BeginDate = new BaseUtils().GetSelectDate(string.IsNullOrEmpty(context.Request["begin"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(context.Request["begin"]), true);
                    DateTime EndDate = new BaseUtils().GetSelectDate(string.IsNullOrEmpty(context.Request["end"].ToString()) ? DateTime.MaxValue : Convert.ToDateTime(context.Request["end"]), false);

                    int ClassType = Convert.ToInt32(context.Request["classtype"]);
                    List<Task> TaskList = db.Task.Where(t => (ClassType == 0 || t.ClassType == (ClassTypeEnums)ClassType)
                        && t.TableTypeID == TableType && t.CreateDate > BeginDate
                        && t.CreateDate < EndDate).OrderBy(t => t.CreateDate).ToList();

                    foreach (Task Task in TaskList)
                    {
                        JObject jobject = new JObject();
                        jobject.Add("value", Task.ID);
                        jobject.Add("name", Task.TaskName);
                        jarray.Add(jobject);
                    }
                    returnobj.Add("result", "success");
                    returnobj.Add("List", jarray);
                }
            }
            catch
            {
                returnobj.Add("result", "failed");
            }
            context.Response.Clear();
            context.Response.Write(returnobj);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        public void GetStatisticsTableType(HttpContext context)
        {
            JObject returnobj = new JObject();
            JArray jarray = new JArray();
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    //var list = db.TableColumn.Where(t => t.IsStats).Select(t => t.TableTypeID).Distinct().ToList();
                    List<TableType> TableTypeList = db.TableType.Where(t => t.IsDel != true && db.TableColumn.Where(m => m.IsStats).Select(m => m.TableTypeID).Contains(t.ID)).ToList();
                    foreach (TableType TableType in TableTypeList)
                    {

                        JObject jobject = new JObject();
                        jobject.Add("ID", TableType.ID);
                        jobject.Add("Name", TableType.Name);
                        jarray.Add(jobject);

                    }

                    returnobj.Add("result", "success");
                    returnobj.Add("List", jarray);
                }
            }
            catch
            {
                returnobj.Add("result", "failed");
            }
            context.Response.Clear();
            context.Response.Write(returnobj);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        public void GetStaticColumn(HttpContext context)
        {
            JObject returnobj = new JObject();
            JArray jarray = new JArray();

            try
            {
                int TableTypeID = Convert.ToInt32(context.Request["tabletype"]);

                using (IFMPDBContext db = new IFMPDBContext())
                {
                    List<TableColumn> TableColumnList = db.TableColumn.Where(t => t.TableTypeID == TableTypeID && t.IsStats).OrderBy(t => t.Order).ToList();
                    foreach (TableColumn TableColumn in TableColumnList)
                    {
                        JObject jobject = new JObject();
                        jobject.Add("ID", TableColumn.ID);
                        jobject.Add("Name", TableColumn.ColumnName);
                        jarray.Add(jobject);
                    }
                    returnobj.Add("result", "success");
                    returnobj.Add("List", jarray);
                }
            }
            catch
            {
                returnobj.Add("result", "failed");
            }
            context.Response.Clear();
            context.Response.Write(returnobj);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        public void GetNotStaticColumn(HttpContext context)
        {
            JObject returnobj = new JObject();
            JArray jarray = new JArray();

            try
            {
                int TableTypeID = Convert.ToInt32(context.Request["tabletype"]);

                using (IFMPDBContext db = new IFMPDBContext())
                {
                    List<TableColumn> TableColumnList = db.TableColumn.Where(t => t.TableTypeID == TableTypeID
                        && t.IsStats != true
                        && t.DictionaryID != null
                        && db.Dictionary.FirstOrDefault(m => m.ID == t.DictionaryID).DisplayType == DictionaryTypeEnums.单选).OrderBy(t => t.Order).ToList();
                    foreach (TableColumn TableColumn in TableColumnList)
                    {
                        JObject jobject = new JObject();
                        jobject.Add("ID", TableColumn.ID);
                        jobject.Add("Name", TableColumn.ColumnName);
                        jarray.Add(jobject);
                    }
                    returnobj.Add("result", "success");
                    returnobj.Add("List", jarray);
                }
            }
            catch
            {
                returnobj.Add("result", "failed");
            }
            context.Response.Clear();
            context.Response.Write(returnobj);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        public void GetUser(HttpContext context)
        {
            JObject returnobj = new JObject();
            JArray jarray = new JArray();

            try
            {
                JArray TaskArray = (JArray)context.Request["task"];
                int ColumnID = Convert.ToInt32(context.Request["column"]);
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    List<User> UserList = db.User.ToList();
                    List<TableData> TableDataList = db.TableData.Where(t => t.TableColumnID == ColumnID).ToList();
                    List<Table> TableList = db.Table.ToList();

                    TableColumn TableColumn = db.TableColumn.FirstOrDefault(t => t.ID == ColumnID && t.IsStats);
                    if (TableColumn != null)
                    {
                        foreach (JObject TaskObject in TaskArray)
                        {
                            JArray DataArray = new JArray();
                            int TaskID = Convert.ToInt32(TaskObject["val"]);
                            Task Task = db.Task.FirstOrDefault(t => t.ID == TaskID);
                            if (Task != null)
                            {
                                List<TableData> SelTableDataList = TableDataList.Where(t => TableList.Where(m => m.TaskID == TaskID).Select(m => m.ID).Contains(t.TableID)).ToList();
                                //还没做

                            }
                        }
                        returnobj.Add("result", "success");
                        returnobj.Add("List", jarray);
                    }
                    else
                    {
                        returnobj.Add("result", "failed");
                    }
                }
            }
            catch
            {
                returnobj.Add("result", "failed");
            }
            context.Response.Clear();
            context.Response.Write(returnobj);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        public void GetCompareDateByTask(HttpContext context)
        {
            JObject returnobj = new JObject();
            JArray jarray = new JArray();

            try
            {
                JArray TaskArray = (JArray)JsonConvert.DeserializeObject(context.Request["task"].ToString());
                int ColumnID = Convert.ToInt32(context.Request["column"]);
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    TableColumn TableColumn = db.TableColumn.FirstOrDefault(t => t.ID == ColumnID && t.IsStats);
                    List<DictionaryData> DictionaryDataList = null;
                    if (TableColumn != null)
                    {
                        //判断字典
                        if (TableColumn.DictionaryID != null)
                        {
                            Dictionary Dictionary = db.Dictionary.FirstOrDefault(t => t.ID == TableColumn.ID && t.DisplayType == DictionaryTypeEnums.单选);
                            if (Dictionary != null)
                            {
                                DictionaryDataList = db.DictionaryData.Where(t => t.DictionaryID == Dictionary.ID).ToList();
                            }
                        }

                        List<User> UserList = db.User.ToList();
                        List<TableData> TableDataList = db.TableData.Where(t => t.TableColumnID == ColumnID).ToList();
                        List<Table> TableList = db.Table.ToList();
                        foreach (JObject TaskObject in TaskArray)
                        {
                            JArray DataArray = new JArray();
                            int TaskID = Convert.ToInt32(TaskObject["val"]);
                            Task Task = db.Task.FirstOrDefault(t => t.ID == TaskID);
                            if (Task != null)
                            {
                                List<TableData> SelTableDataList = TableDataList.Where(t => TableList.Where(m => m.TaskID == TaskID).Select(m => m.ID).Contains(t.TableID)).ToList();

                                foreach (TableData SelTableData in SelTableDataList)
                                {
                                    JObject jobject = new JObject();
                                    jobject.Add("ID", SelTableData.ID);
                                    jobject.Add("UserName", UserList.FirstOrDefault(t => t.ID == SelTableData.CreateUserID).RealName);
                                    jobject.Add("IsAlert", SelTableData.IsAlert == null ? false : SelTableData.IsAlert.Value);
                                    if (DictionaryDataList == null)
                                    {
                                        jobject.Add("Data", SelTableData.Data);
                                    }
                                    else
                                    {
                                        double seldata = Convert.ToInt32(SelTableData.Data);
                                        jobject.Add("Data", DictionaryDataList.FirstOrDefault(t => t.ID == seldata).Data);
                                    }
                                    DataArray.Add(jobject);
                                }

                                JObject TaskJO = new JObject();
                                TaskJO.Add("ID", Task.ID);
                                TaskJO.Add("Name", Task.TaskName);
                                TaskJO.Add("List", DataArray);
                                jarray.Add(TaskJO);
                            }
                        }

                        JObject MarkLine = new JObject();
                        //判断是否有提示字典
                        if (TableColumn.HintDictionaryID != null)
                        {
                            Dictionary HintDictionary = db.Dictionary.FirstOrDefault(t => t.ID == TableColumn.HintDictionaryID);
                            if (HintDictionary.RegexType == RegexType.有范围的数字)
                            {
                                double max = Convert.ToDouble(HintDictionary.RegexData.Split('|')[0]);
                                double min = Convert.ToDouble(HintDictionary.RegexData.Split('|')[1]);
                                if (max < min)
                                {
                                    max = max + min;
                                    min = max - min;
                                    max = max - min;
                                }

                                MarkLine.Add("Max", max);
                                MarkLine.Add("Min", min);

                            }

                        }

                        returnobj.Add("result", "success");
                        returnobj.Add("List", jarray);
                        returnobj.Add("MarkLine", MarkLine);
                    }
                    else
                    {
                        returnobj.Add("result", "failed");
                        returnobj.Add("List", jarray);
                    }

                }
            }
            catch
            {
                returnobj.Add("result", "failed");
            }
            context.Response.Clear();
            context.Response.Write(returnobj);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        public void GetCompareDateByUser(HttpContext context)
        {
            JObject returnobj = new JObject();
            JArray jarray = new JArray();

            try
            {
                JArray TaskArray = (JArray)context.Request["task"];
                JArray UserArray = (JArray)context.Request["user"];
                List<int> SelUserList = new List<int>();
                foreach (JObject SelUser in SelUserList)
                {
                    int SelUserID = Convert.ToInt32(SelUser["val"]);
                    if (SelUserList.FirstOrDefault(t => t == SelUserID) == null)
                        SelUserList.Add(SelUserID);
                }

                int ColumnID = Convert.ToInt32(context.Request["column"]);
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    TableColumn TableColumn = db.TableColumn.FirstOrDefault(t => t.ID == ColumnID && t.IsStats);
                    List<DictionaryData> DictionaryDataList = null;
                    if (TableColumn != null)
                    {
                        //判断字典
                        if (TableColumn.DictionaryID != null)
                        {
                            Dictionary Dictionary = db.Dictionary.FirstOrDefault(t => t.ID == TableColumn.ID && t.DisplayType == DictionaryTypeEnums.单选);
                            if (Dictionary != null)
                            {
                                DictionaryDataList = db.DictionaryData.Where(t => t.DictionaryID == Dictionary.ID).ToList();
                            }
                        }

                        List<User> UserList = db.User.ToList();
                        List<TableData> TableDataList = db.TableData.Where(t => t.TableColumnID == ColumnID).ToList();
                        List<Table> TableList = db.Table.ToList();
                        foreach (JObject TaskObject in TaskArray)
                        {
                            JArray DataArray = new JArray();
                            int TaskID = Convert.ToInt32(TaskObject["val"]);
                            Task Task = db.Task.FirstOrDefault(t => t.ID == TaskID);
                            if (Task != null)
                            {
                                List<TableData> SelTableDataList = TableDataList.Where(t => SelUserList.Contains(t.CreateUserID.Value) && TableList.Where(m => m.TaskID == TaskID).Select(m => m.ID).Contains(t.TableID)).ToList();

                                foreach (TableData SelTableData in SelTableDataList)
                                {
                                    JObject jobject = new JObject();
                                    jobject.Add("ID", SelTableData.ID);
                                    jobject.Add("UserName", UserList.FirstOrDefault(t => t.ID == SelTableData.CreateUserID).RealName);

                                    if (DictionaryDataList != null)
                                    {
                                        jobject.Add("Data", SelTableData.Data);
                                    }
                                    else
                                    {
                                        int seldata = Convert.ToInt32(SelTableData.Data);
                                        jobject.Add("Data", DictionaryDataList.FirstOrDefault(t => t.ID == seldata).Data);
                                    }
                                    DataArray.Add(jobject);
                                }

                                JObject TaskJO = new JObject();
                                TaskJO.Add("ID", Task.ID);
                                TaskJO.Add("Name", Task.TaskName);
                                TaskJO.Add("List", DataArray);
                                jarray.Add(TaskJO);
                            }

                        }
                    }
                    else
                    {
                        returnobj.Add("result", "failed");
                        returnobj.Add("List", jarray);
                    }

                    returnobj.Add("result", "success");
                    returnobj.Add("List", jarray);
                }
            }
            catch
            {
                returnobj.Add("result", "failed");
            }
            context.Response.Clear();
            context.Response.Write(returnobj);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }




        private static List<int> SelectedColumnList = new List<int>();
        public void GetStatisticsByDate(HttpContext context)
        {
            JObject returnobj = new JObject();
            JArray jarray = new JArray();
            Random ran = new Random();
            try
            {

                using (IFMPDBContext db = new IFMPDBContext())
                {
                    int lineid = Convert.ToInt32(context.Request["flag"]);
                    List<TableColumn> TableColumnList = db.TableColumn.Where(t => t.IsStats && db.TableLine.Where(m => m.LineID == lineid).Select(m => m.TableTypeID).Contains(t.TableTypeID)).ToList();


                    //var re = from t in db.TableColumn join d in db.TableLine on t.TableTypeID equals d.TableTypeID                            
                    //         where d.LineID=
                    //         select new
                    //         {
                    //             OrderCode = o.Code,
                    //             ProductName = p.Name,
                    //             SalePrice = p.SalePrice,
                    //             ProductQuantity = d.Quantity,
                    //             Amount = d.Amount
                    //         };


                    int ColumnID = 0;
                    TableColumn StatColunm = null;
                    int statcolumn = Convert.ToInt32(context.Request["statitem"]);
                    ColumnStatType? ColumnStatType = null;
                    string selcolumn = context.Request["selcolumn"];

                    foreach (string selcol in selcolumn.Split(','))
                    {
                        if (!string.IsNullOrEmpty(selcol))
                        {
                            int selcolid = Convert.ToInt32(selcol);
                            TableColumnList.Remove(TableColumnList.FirstOrDefault(t => t.ID == selcolid));
                        }
                    }

                    if (string.IsNullOrEmpty(context.Request["column"]))
                    {
                        //考虑读取对应chart类型的数据
                        //TableColumnList = TableColumnList.Where(t => !SelectedColumnList.Contains(t.ID)).ToList();
                        ColumnID = TableColumnList[ran.Next(0, TableColumnList.Count)].ID;
                        //if ()
                        TableColumn SelTableColumn = TableColumnList.FirstOrDefault(t => t.ID == ColumnID);
                        ColumnStatType = SelTableColumn.ColumnStatType;
                        if (SelTableColumn.StatTableColumnID != null)
                        {
                            StatColunm = db.TableColumn.FirstOrDefault(t => t.ID == SelTableColumn.StatTableColumnID);
                            //statcolumn = SelTableColumn.StatTableColumnID.Value;
                        }
                    }
                    else
                    {
                        ColumnID = Convert.ToInt32(context.Request["column"]);
                        if (statcolumn != 0)
                        {
                            StatColunm = db.TableColumn.FirstOrDefault(t => t.ID == statcolumn);
                        }
                    }



                    DateTime BeginDate = string.IsNullOrEmpty(context.Request["begin"].ToString()) ? DateTime.Now.Date.AddDays(-7) : Convert.ToDateTime(context.Request["begin"].ToString());
                    DateTime EndDate = string.IsNullOrEmpty(context.Request["end"].ToString()) ? DateTime.Now.Date.AddDays(1) : Convert.ToDateTime(context.Request["end"].ToString());


                    TableColumn TableColumn = db.TableColumn.FirstOrDefault(t => t.ID == ColumnID && t.IsStats);
                    List<DictionaryData> DictionaryDataList = null;
                    List<DictionaryData> FullDictionaryDataList = db.DictionaryData.ToList();
                    if (TableColumn != null)
                    {
                        List<TableData> TableDataList = db.TableData.Where(t =>
                            t.CreateDate >= BeginDate
                            && t.CreateDate <= EndDate
                            && t.TableColumnID == ColumnID).OrderBy(t => t.CreateDate).ToList();

                        if (TableColumn.DictionaryID != null)
                        {
                            Dictionary Dictionary = db.Dictionary.FirstOrDefault(t => t.ID == TableColumn.ID && t.DisplayType == DictionaryTypeEnums.单选);
                            if (Dictionary != null)
                            {
                                DictionaryDataList = db.DictionaryData.Where(t => t.DictionaryID == Dictionary.ID).ToList();
                            }
                        }
                        List<User> UserList = db.User.ToList();
                        List<Table> TableList = db.Table.ToList();

                        List<TableData> FullList = new List<TableData>();
                        if (StatColunm != null)
                        {
                            FullList = db.TableData.Where(t => t.TableColumnID == StatColunm.ID).ToList();
                        }

                        foreach (TableData TableData in TableDataList)
                        {
                            JObject jobject = new JObject();
                            jobject.Add("ID", TableData.ID);
                            jobject.Add("UserName", UserList.FirstOrDefault(t => t.ID == TableData.CreateUserID).RealName);
                            if (StatColunm != null)
                            {
                                TableData StatData = FullList.FirstOrDefault(t => t.TableID == TableData.TableID);
                                Dictionary StatDictionary = db.Dictionary.FirstOrDefault(t => t.ID == StatColunm.DictionaryID && t.DisplayType == DictionaryTypeEnums.单选);
                                if (StatDictionary == null)
                                {
                                    jobject.Add("StatData", StatData.Data);
                                }
                                else
                                {
                                    int StatDictionaryDataID = Convert.ToInt32(StatData.Data);
                                    jobject.Add("StatData", FullDictionaryDataList.FirstOrDefault(t => t.ID == StatDictionaryDataID).Data);
                                }
                            }
                            jobject.Add("IsAlert", TableData.IsAlert == null ? false : TableData.IsAlert.Value);
                            jobject.Add("CreateDate", TableData.CreateDate.Value);
                            if (DictionaryDataList == null)
                            {
                                jobject.Add("Data", TableData.Data);
                            }
                            else
                            {
                                double seldata = Convert.ToInt32(TableData.Data);
                                jobject.Add("Data", DictionaryDataList.FirstOrDefault(t => t.ID == seldata).Data);
                            }
                            jarray.Add(jobject);
                        }

                        JObject MarkLine = new JObject();
                        //判断是否有提示字典
                        if (TableColumn.HintDictionaryID != null)
                        {
                            Dictionary HintDictionary = db.Dictionary.FirstOrDefault(t => t.ID == TableColumn.HintDictionaryID);
                            if (HintDictionary.RegexType == RegexType.有范围的数字)
                            {
                                double max = Convert.ToDouble(HintDictionary.RegexData.Split('|')[0]);
                                double min = Convert.ToDouble(HintDictionary.RegexData.Split('|')[1]);
                                if (max < min)
                                {
                                    double temp = max;
                                    max = min;
                                    min = temp;
                                }

                                MarkLine.Add("Max", max);
                                MarkLine.Add("Min", min);
                            }

                        }

                        returnobj.Add("result", "success");
                        returnobj.Add("List", jarray);
                        returnobj.Add("MarkLine", MarkLine);
                        returnobj.Add("ColumnID", TableColumn.ID);
                        returnobj.Add("ColumnName", db.TableType.FirstOrDefault(t => t.ID == TableColumn.TableTypeID).Name + "-" + TableColumn.ColumnName);
                        returnobj.Add("ColumnStatType", ColumnStatType == null ? null : Enum.GetName(typeof(ColumnStatType), ColumnStatType));
                        returnobj.Add("StatColumn", StatColunm == null ? 0 : StatColunm.ID);
                    }
                    else
                    {
                        returnobj.Add("result", "failed");
                        returnobj.Add("List", jarray);
                    }

                }
            }
            catch
            {
                returnobj.Add("result", "failed");
            }
            context.Response.Clear();
            context.Response.Write(returnobj);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        public void GetClassStatistics(HttpContext context)
        {
            JObject returnobj = new JObject();
            JArray jarray = new JArray();

            try
            {
                int ClassType = Convert.ToInt32(context.Request["classtype"]);
                DateTime BeginDate = new BaseUtils().GetSelectDate(string.IsNullOrEmpty(context.Request["begin"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(context.Request["begin"]), true);
                DateTime EndDate = new BaseUtils().GetSelectDate(string.IsNullOrEmpty(context.Request["end"].ToString()) ? DateTime.MaxValue : Convert.ToDateTime(context.Request["end"]), false);

                using (IFMPDBContext db = new IFMPDBContext())
                {
                    List<BaseClass> BaseClassList = db.BaseClass.Where(t => t.IsDel != true && (ClassType == 0 || t.ClassType == (ClassTypeEnums)ClassType)).ToList();
                    List<TaskFlow> TaskFlowList = db.TaskFlow.Where(t => t.ApplyDate > BeginDate && t.ApplyDate < EndDate
                        && t.BaseClassID != null && t.ApplyType != ApplyTypeEnums.未交 && db.Flow.Where(m => m.IsAudit != true).Select(m => m.ID).Contains(t.ID)
                        ).ToList();
                    var BascClassName = BaseClassList.Select(t => t.Name).Distinct().ToList();
                    foreach (string Name in BascClassName)
                    {
                        List<BaseClass> SelBaseClassList = BaseClassList.Where(t => t.Name == Name).OrderBy(t => t.ClassType).ToList();
                        if (SelBaseClassList.Count > 0)
                        {
                            JObject BaseClassObject = new JObject();
                            BaseClassObject.Add("name", Name);
                            //BaseClassObject.Add("value",TaskFlowList.Count(t=>SelBaseClassList.wh));
                            JArray ChildArray = new JArray();
                            int total = 0;
                            foreach (BaseClass SelBaseClass in SelBaseClassList)
                            {
                                JObject SelBaseClassObject = new JObject();
                                SelBaseClassObject.Add("name", Name + "(" + Enum.GetName(typeof(ClassTypeEnums), SelBaseClass.ClassType) + ")");
                                int count = TaskFlowList.Where(t => t.BaseClassID == SelBaseClass.ID).Select(t => t.TaskID).Distinct().Count();
                                SelBaseClassObject.Add("value", count);
                                total = total + count;
                                ChildArray.Add(SelBaseClassObject);
                            }
                            BaseClassObject.Add("value", total);
                            BaseClassObject.Add("children", ChildArray);

                            jarray.Add(BaseClassObject);
                        }
                    }
                    returnobj.Add("result", "success");
                    returnobj.Add("List", jarray);
                }
            }
            catch
            {
                returnobj.Add("result", "failed");
            }
            context.Response.Clear();
            context.Response.Write(returnobj);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }


        public void GetDateStatistics(HttpContext context)
        {
            JObject returnobj = new JObject();
            JArray jarray = new JArray();

            try
            {
                int ClassType = Convert.ToInt32(context.Request["classtype"]);
                int TableColumnID = Convert.ToInt32(context.Request["tablecolumn"]);
                int TableTypeID = Convert.ToInt32(context.Request["tabletype"]);

                DateTime BeginDate = new BaseUtils().GetSelectDate(string.IsNullOrEmpty(context.Request["begin"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(context.Request["begin"]), true);
                DateTime EndDate = new BaseUtils().GetSelectDate(string.IsNullOrEmpty(context.Request["end"].ToString()) ? DateTime.MaxValue : Convert.ToDateTime(context.Request["end"]), false);

                if (BeginDate > EndDate)
                {
                    DateTime Temp = BeginDate;
                    BeginDate = EndDate;
                    EndDate = Temp;
                }

                if ((EndDate - BeginDate).TotalDays < 15)
                {
                    using (IFMPDBContext db = new IFMPDBContext())
                    {
                        List<TableData> TableDataList = db.TableData.Where(t => t.CreateDate >= BeginDate && t.CreateDate <= EndDate
                            && t.TableColumnID == TableColumnID
                            && db.Table.Where(m => m.TableTypeID == TableTypeID && (ClassType == 0 || db.Task.FirstOrDefault(k => k.ID == m.TaskID).ClassType == (ClassTypeEnums)ClassType)).Select(m => m.ID).Contains(t.TableID)
                            ).ToList();


                        List<User> UserList = db.User.ToList();

                        List<TaskFlow> TaskFlowList = db.TaskFlow.Where(t => t.ApplyDate >= BeginDate
                            && t.ApplyDate <= EndDate
                            && db.Flow.Where(m => m.IsAudit != true).Select(m => m.ID).Contains(t.ID)
                            ).ToList();

                        //这个以天为单位统计
                        while (BeginDate <= EndDate)
                        {
                            JArray SelDataArray = new JArray();
                            List<TableData> SelTableDataList = TableDataList.Where(t => t.CreateDate >= BeginDate && t.CreateDate <= BeginDate.AddDays(1)).ToList();

                            if (SelTableDataList.Count > 0)
                            {
                                foreach (TableData SelTableData in SelTableDataList)
                                {
                                    JObject SelJObject = new JObject();
                                    SelJObject.Add("Name", UserList.FirstOrDefault(t => t.ID == SelTableData.CreateUserID).RealName);
                                    SelJObject.Add("Count", SelTableData.Data);
                                    SelDataArray.Add(SelJObject);
                                }
                                JObject DayJObject = new JObject();
                                DayJObject.Add("Date", BeginDate.ToString("yyyy-MM-dd"));
                                DayJObject.Add("List", SelDataArray);
                                jarray.Add(DayJObject);
                            }
                            BeginDate = BeginDate.AddDays(1);
                        }

                        returnobj.Add("result", "success");
                        returnobj.Add("List", jarray);
                    }
                }
                else
                {
                    returnobj.Add("result", "failed");
                    returnobj.Add("message", "请选择最多相差14天的数据");
                }
            }
            catch
            {
                returnobj.Add("result", "failed");
            }
            context.Response.Clear();
            context.Response.Write(returnobj);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        public void GetStatisticsDeviceDataType(HttpContext context)
        {
            JObject returnobj = new JObject();
            JArray jarray = new JArray();
            try
            {
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    foreach (int item in Enum.GetValues(typeof(DeviceDataType)))
                    {
                        JObject jobject = new JObject();
                        jobject.Add("ID", item);
                        jobject.Add("Name", Enum.GetName(typeof(DeviceDataType), item));
                        jarray.Add(jobject);
                    }
                    returnobj.Add("result", "success");
                    returnobj.Add("List", jarray);
                }
            }
            catch
            {
                returnobj.Add("result", "failed");
            }
            context.Response.Clear();
            context.Response.Write(returnobj);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        public void GetStatisticsDevice(HttpContext context)
        {
            JObject returnobj = new JObject();
            JArray jarray = new JArray();
            try
            {
                DeviceType DeviceType = (DeviceType)Convert.ToInt32(context.Request["devicetype"]);
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    List<IntelligentDevice> IntelligentDeviceList = db.IntelligentDevice.Where(t => t.DeviceType == DeviceType).ToList();
                    foreach (IntelligentDevice IntelligentDevice in IntelligentDeviceList)
                    {
                        JObject jobject = new JObject();
                        jobject.Add("ID", IntelligentDevice.ID);
                        jobject.Add("Name", IntelligentDevice.Name);
                        jarray.Add(jobject);
                    }
                    returnobj.Add("result", "success");
                    returnobj.Add("List", jarray);
                }
            }
            catch
            {
                returnobj.Add("result", "failed");
            }
            context.Response.Clear();
            context.Response.Write(returnobj);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }


        public void GetStatisticsDeviceData(HttpContext context)
        {
            JObject returnobj = new JObject();
            try
            {
                int DeviceID = Convert.ToInt32(context.Request["device"]);

                DateTime BeginDate = new BaseUtils().GetSelectDate(string.IsNullOrEmpty(context.Request["begin"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(context.Request["begin"]), true);
                DateTime EndDate = new BaseUtils().GetSelectDate(string.IsNullOrEmpty(context.Request["end"].ToString()) ? DateTime.MaxValue : Convert.ToDateTime(context.Request["end"]), false);

                using (IFMPDBContext db = new IFMPDBContext())
                {
                    List<IntelligentDeviceData> IntelligentDeviceDataList = db.IntelligentDeviceData.Where(t => t.IntelligentDeviceID == DeviceID && t.CreateDate > BeginDate && t.CreateDate < EndDate).OrderBy(t => t.CreateDate).ToList();
                    JArray DateArray = new JArray();
                    JArray DataArray = new JArray();
                    if (IntelligentDeviceDataList.Count > 0)
                    {
                        foreach (IntelligentDeviceData IntelligentDeviceData in IntelligentDeviceDataList)
                        {
                            DateArray.Add(IntelligentDeviceData.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"));
                            DataArray.Add(IntelligentDeviceData.Data);
                        }
                    }
                    returnobj.Add("Date", DateArray);
                    returnobj.Add("Data", DataArray);
                    returnobj.Add("result", "success");
                }
            }
            catch
            {
                returnobj.Add("result", "failed");
            }
            context.Response.Clear();
            context.Response.Write(returnobj);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }


        public void GetAlert(HttpContext context)
        {
            JObject returnobj = new JObject();
            try
            {
                //先只考虑整体情况
                //int ClassType = Convert.ToInt32(context.Request["classtype"]);
                int TableColumnID = Convert.ToInt32(context.Request["tablecolumn"]);

                DateTime BeginDate = new BaseUtils().GetSelectDate(string.IsNullOrEmpty(context.Request["begin"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(context.Request["begin"]), true);
                DateTime EndDate = new BaseUtils().GetSelectDate(string.IsNullOrEmpty(context.Request["end"].ToString()) ? DateTime.MaxValue : Convert.ToDateTime(context.Request["end"]), false);
                using (IFMPDBContext db = new IFMPDBContext())
                {
                    List<TableData> TableDataList = db.TableData.Where(t => (TableColumnID == 0 || t.TableColumnID == TableColumnID)
                        && t.CreateDate >= BeginDate && t.CreateDate <= EndDate
                        ).OrderBy(t => t.CreateDate).ToList();
                    JArray jarray = new JArray();
                    if (TableDataList.Count > 0)
                    {
                        if (BeginDate.Date < TableDataList.FirstOrDefault().CreateDate.Value.Date)
                        {
                            BeginDate = TableDataList[0].CreateDate.Value.Date;
                        }

                        if (EndDate.Date > TableDataList.LastOrDefault().CreateDate.Value.Date)
                        {
                            EndDate = new BaseUtils().GetSelectDate(TableDataList.LastOrDefault().CreateDate.Value.Date, false);
                        }

                        while (BeginDate < EndDate)
                        {
                            JObject jobject = new JObject();
                            jobject.Add("Date", BeginDate.ToString("yyyy-MM-dd"));
                            jobject.Add("Count", TableDataList.Count(t => t.CreateDate.Value.Date == BeginDate.Date && t.IsAlert == true));
                            jarray.Add(jobject);

                            BeginDate = BeginDate.AddDays(1);
                        }

                        returnobj.Add("List", jarray);
                        returnobj.Add("result", "success");
                    }
                }
            }
            catch
            {
                returnobj.Add("result", "failed");
            }
            context.Response.Clear();
            context.Response.Write(returnobj);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}