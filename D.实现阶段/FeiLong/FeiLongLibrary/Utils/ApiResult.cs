using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using FeiLongLibrary.DAO;
using FeiLongLibrary.Enums;

namespace FeiLongLibrary.Utils
{
    /// <summary>
    /// Api 返回的结果
    /// </summary>
    public class ApiResult
    {
        public ApiResultCodeType Status { get; set; }

        /// <summary>
        /// 用来存操作信息
        /// eg：登陆成功/添加成功/删除失败
        /// </summary>
        protected List<string> Messages { get; set; }

        public ApiResult()
        {
            this.Status = ApiResultCodeType.Success;
            this.Messages = new List<string>();
        }

        public ApiResult(ApiResultCodeType status)
        {
            this.Status = status;
            this.Messages = new List<string>();
        }

        public ApiResult(ApiResultCodeType status, List<string> messages)
        {
            this.Status = status;
            this.Messages = messages;
        }

        /// <summary>
        /// 用于保存方法数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="args"></param>
        public void AddMessage(string msg, params object[] args)
        {
            if (this.Messages == null)
            {
                this.Messages = new List<string>();
            }
            if (String.IsNullOrEmpty(msg) || string.IsNullOrWhiteSpace(msg)) { }
            else
            {
                this.Messages.Add(String.Format(msg, args));
            }
        }

        /// <summary>
        /// 添加异常消息
        /// </summary>
        /// <param name="ex">异常</param>
        public void AddMessage(Exception ex)
        {
            if (ex != null)
            {
                this.AddMessage("Message:{0}{1}StackTrace:{2}", ex.Message, Environment.NewLine, ex.StackTrace);
            }
        }

        public void AddMessage(List<string> msgs)
        {
            if (this.Messages == null)
            {
                this.Messages = new List<string>();
            }
            this.Messages.AddRange(msgs);
        }

        /// <summary>
        /// 获取消息
        /// </summary>
        /// <param name="split">消息分隔符,默认以<br/>分隔</param>
        /// <returns></returns>
        public string GetMessageString()
        {
            if (this.Messages != null && this.Messages.Count > 0)
            {
                string result = null;
                foreach (string msg in this.Messages)
                {
                    result += String.Format("{0} ", msg);
                }
                return result;
            }
            return null;
        }

        public static ApiResult NewErrorJson(string message)
        {
            new SysLogDAO().AddLog(LogType.Failure, message: "发生错误:" + message);
            return new ApiResult()
            {
                Status = ApiResultCodeType.Failure,
                Data = message,
            };
        }


        public static ApiResult NewSuccessJson(object Data)
        {
            return new ApiResult()
            {
                Status = ApiResultCodeType.Success,
                Data = Data,
            };
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }

}