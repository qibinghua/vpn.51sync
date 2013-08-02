using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace VPN51SYNC
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool runone;
            System.Threading.Mutex run = new System.Threading.Mutex(true, "xinbiao_a_test", out runone);
            if (runone)
            {
                run.ReleaseMutex();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new TinyVpnUI());
            }
            else
            {
                //MessageBox.Show("已经运行了一个实例了。");
                Application.Exit();//退出程
            }
        }
    }
}
