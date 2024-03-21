using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lydong.Hangfire
{
    public class LydongHangfireOption
    {
        /// <summary>
        /// 时候启动定时任务
        /// </summary>
        public bool IsEnable = true;
        /// <summary>
        /// 存储类型
        /// </summary>
        public HangfireStorageType Storage = HangfireStorageType.Memory;
        /// <summary>
        /// 存储的连接字符串
        /// </summary>
        public string ConnectionString;
        /// <summary>
        /// 心跳。每隔多少秒检测一次
        /// </summary>
        public int Heartbeat = 1;
        /// <summary>
        /// 面板登陆用户名
        /// </summary>
        public string LoginUserName = "admin";
        /// <summary>
        /// 面板登陆密码
        /// </summary>
        public string LoginPassword = "password";
    }

    public enum HangfireStorageType
    {
        SqlServer,
        Redis,
        Memory
    }
}
