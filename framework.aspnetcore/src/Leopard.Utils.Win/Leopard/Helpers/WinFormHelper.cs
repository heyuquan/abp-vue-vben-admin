using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Leopard.Helpers
{

    //// 案例
    //static class Program
    //{
    //    /// <summary>
    //    /// 应用程序的主入口点。
    //    /// </summary>
    //    [STAThread]
    //    static void Main()
    //    {
    //        string formUniqueName = "xxx服务"; // 窗体标题（form.Text属性值），最好起一个不会和别的程序重名的标题
    //        WinFormHelper.OpenUniqueProcess(formUniqueName, () =>
    //        {
    //            Application.EnableVisualStyles();
    //            Application.SetCompatibleTextRenderingDefault(false);
    //            Application.Run(new FrmClient());
    //        });
    //    }
    //}

    /// <summary>
    /// windows窗体程序的相关帮助类
    /// </summary>
    public class WinFormHelper
    {
        #region 打开窗体程序相关
        [DllImport("user32.dll ")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        //根据任务栏应用程序显示的名称找窗口的名称
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        private const int SW_RESTORE = 9;

        /// <summary>
        /// 打开指定程序，如果指定程序已经运行，则把窗口show出来
        /// </summary>
        /// <param name="formName">窗口名称（Form.Text属性）</param>
        ///  <param name="processStartInfo">要运行的程序</param>
        public static void OpenUniqueProcess(string formName, ProcessStartInfo processStartInfo)
        {
            OpenUniqueProcess(formName, () =>
            {
                Process.Start(processStartInfo);
            });
        }

        /// <summary>
        /// 打开指定程序，如果指定程序已经运行，则把窗口show出来
        /// </summary>
        /// <param name="formName">窗口名称（Form.Text属性）</param>
        ///  <param name="startProcessAction">要运行的程序的委托(eg：在启动程序自身时，传入Application.Run(new FrmClient())。)</param>
        public static void OpenUniqueProcess(string formName, Action startProcessAction)
        {
            //查找状态中的窗口名称来查看目标程序是否在运行运行则前置否则打开
            IntPtr findPtr = FindWindow(null, formName);
            if (findPtr.ToInt32() != 0)
            {
                ShowWindow(findPtr, SW_RESTORE); //将窗口还原，如果不用此方法，缩小的窗口不能显示出来
                SetForegroundWindow(findPtr);//将指定的窗口选中(**)
            }
            else
            {
                startProcessAction();
            }
        }
        #endregion
    }
}
