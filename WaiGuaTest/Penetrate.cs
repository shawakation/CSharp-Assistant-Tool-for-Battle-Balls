using System;
using System.Runtime.InteropServices;

namespace Penetrate
{
    public class thePenetrateFunc
    {
        private WaiGuaTest.theMainForm myForm;
        /// <summary>
        /// 窗体穿透函数
        /// </summary>
        private const uint WS_EX_LAYERED = 0x80000;
        private const int WS_EX_TRANSPARENT = 0x20;
        private const int GWL_STYLE = (-16);
        private const int GWL_EXSTYLE = (-20);
        private const int LWA_COLORKEY = 0x1;
        private const int LWA_ALPHA = 0x2;
        [DllImport("user32", EntryPoint = "SetWindowLong")]
        private static extern uint SetWindowLong(IntPtr hwnd, int nIndex, uint dwNewLong);
        [DllImport("user32", EntryPoint = "GetWindowLong")]
        private static extern uint GetWindowLong(IntPtr hwnd, int nIndex);
        [DllImport("user32", EntryPoint = "SetLayeredWindowAttributes")]
        private static extern int SetLayeredWindowAttributes(IntPtr hwnd, int crKey, int bAlpha, int dwFlags);

        public thePenetrateFunc(WaiGuaTest.theMainForm theForm)
        {
            myForm = theForm;
        }
        /// <summary>
        /// 使窗口有鼠标穿透功能  
        /// </summary>
        public void CanPenetrate()
        {
            SetWindowLong(myForm.Handle , GWL_EXSTYLE, WS_EX_LAYERED);
            SetLayeredWindowAttributes(myForm.Handle, (int)(0x010101), 0, LWA_COLORKEY);
        }
    }
}
