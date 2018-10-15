using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;

namespace Hook
{
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int x;
        public int y;
    }
    public struct RECT
    {
        int left, top, right, bottom;
        public RECT(int left,int top,int right,int bottom)
        {
            this.left = left;
            this.right = right;
            this.top = top;
            this.bottom = bottom;
        }
    }
    [StructLayout(LayoutKind.Sequential)]
    public class Vector2
    {
        public int x;
        public int y;
        public Vector2()
        {
            x = 0; y = 0;
        }
        public Vector2(int PosX, int PosY)
        {
            x = PosX; y = PosY;
        }
        public float Length
        {
            get
            {
                return (float)(Math.Sqrt(x * x + y * y));
            }
        }
        public static Vector2 operator +(Vector2 A, Vector2 B)
        {
            return new Vector2(A.x + B.x, A.y + B.y);
        }
        public static Vector2 operator -(Vector2 A, Vector2 B)
        {
            return new Vector2(A.x - B.x, A.y - B.y);
        }
        public static Vector2 operator *(Vector2 A, int B)
        {
            return new Vector2(A.x * B, A.y * B);
        }
        public static Vector2 operator *(int A,Vector2 B)
        {
            return B * A;
        }
        public static explicit operator Vector2(Point Loc)
        {
            return new Vector2(Loc.X, Loc.Y);
        }
        public static explicit operator Vector2(Vector2Double A)
        {
            return new Vector2((int)A.x, (int)A.y);
        }
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2Double
    {
        public double x;
        public double y;
        public Vector2Double(double X,double Y)
        {
            x = X;y = Y;
        }
        public double Length
        {
            get
            {
                return Math.Sqrt(x * x + y * y);
            }
        }
        public static explicit operator Vector2Double(Vector2 Loc)
        {
            return new Vector2Double(Loc.x, Loc.y);
        }
        public static Vector2Double operator *(Vector2Double A, double B)
        {
            return new Vector2Double(A.x * B, A.y * B);
        }
    }
    [StructLayout(LayoutKind.Sequential)]
    public class MouseHookStruct
    {
        public Vector2 pt;
        public int hwnd;
        public int wHitTestCode;
        public IntPtr dwExtraInfo;
    }
    [StructLayout(LayoutKind.Sequential)]
    public class KeyboardHookStruct
    {
        public int vkCode;      //虚拟键码,范围1至254
        public int scanCode;    //硬件扫描码
        public int flags;       //键标志
        public int time;        //时间戳
        public IntPtr dwExtraInfo; //额外信息
    }
    public class theMouseKeybdHook
    {
        public static Vector2 theScreenCenter = new Vector2(1366 / 2, 768 / 2);
        public static int radius = 120;
        public static int bigRadius = 384;
        private double theProportion = new double();
        private Vector2 theMouseCenter = new Vector2();

        private WaiGuaTest.theMainForm myForm;
        private InputSimulator mySim = new InputSimulator();

        /// <summary>
        /// 钩子相关函数
        /// </summary>
        //回调函数的委托
        private delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);
        //定义钩子句柄
        private int MouseHookID = 0;     //鼠标钩子ID
        private int KeyboardHookID = 0; //键盘钩子ID
        //定义钩子类型
        private const int WH_KEYBOARD_LL = 13;
        private const int WH_MOUSE_LL = 14;
        private HookProc theMouseProcess;
        private HookProc theKeyboardProcess;

        //钩子安装函数
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, EntryPoint = "SetWindowsHookEx")]
        private static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);
        //钩子卸载函数
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, EntryPoint = "UnhookWindowsHookEx")]
        private static extern bool UnhookWindowsHookEx(int idHook);
        //消息传递给下一个钩子
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, EntryPoint = "CallNextHookEx")]
        private static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// 鼠标相关函数
        /// </summary>
        /// <param name="lpPoint"></param>
        /// <returns></returns>
        //获取鼠标屏幕坐标
        [DllImport("user32.dll", EntryPoint = "GetCursorPos")]
        private static extern bool GetCursorPos(out POINT lpPoint);
        //限制鼠标移动范围
        [DllImport("user32.dll", EntryPoint = "ClipCursor")]
        private static extern bool ClipCursor(ref RECT lpRect);
        //隐藏显示鼠标指针
        [DllImport("user32.dll", EntryPoint = "ShowCursor", CharSet = CharSet.Auto)]
        private static extern int ShowCursor(bool bShow);

        /// <summary>
        /// 鼠标消息相应常量
        /// </summary>
        private const int WM_MOUSEMOVE = 0x200;
        private const int WM_LBUTTONDOWN = 0x201;
        private const int WM_RBUTTONDOWN = 0x204;
        private const int WM_MBUTTONDOWN = 0x207;
        private const int WM_LBUTTONUP = 0x202;
        private const int WM_RBUTTONUP = 0x205;
        private const int WM_MBUTTONUP = 0x208;

        /// <summary>
        /// 键盘消息相应常量
        /// </summary>
        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;

        private MouseHookStruct theMouseHookStruct;     //未实例化的鼠标消息结构
        private KeyboardHookStruct theKeyboardHookStruct;       //未实例化的键盘消息结构

        private int iCount = 0;
        /// <summary>
        /// 鼠标消息回调函数
        /// 功能：修改所有鼠标消息，修改为左键按下；
        /// 按下（抬起）鼠标左键->向下发送Z键按下（抬起）
        /// 按下（抬起）鼠标右键->向下发送X键按下（抬起）
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private int MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                theMouseHookStruct = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));      //定义结构体并赋值鼠标坐标
                myForm.theXYLabel.Text = "X:" + theMouseHookStruct.pt.x.ToString() + "Y:" + theMouseHookStruct.pt.y.ToString();
                Crosshair(theMouseHookStruct.pt);
                switch ((int)wParam)
                {
                    case WM_LBUTTONDOWN:
                        {
                            mySim.Keyboard.KeyDown(VirtualKeyCode.VK_Z);
                            return 1;
                        }
                    case WM_LBUTTONUP:
                        {
                            mySim.Keyboard.KeyUp(VirtualKeyCode.VK_Z);
                            return 1;
                        }
                    case WM_RBUTTONDOWN:
                        {
                            mySim.Keyboard.KeyDown(VirtualKeyCode.VK_X);
                            return 1;
                        }
                    case WM_RBUTTONUP:
                        {
                            mySim.Keyboard.KeyUp(VirtualKeyCode.VK_X);
                            return 1;
                        }
                    default:break;
                }
            }
            return CallNextHookEx(MouseHookID, nCode, wParam, lParam);
        }
        private int KeyboardHookProc(int nCode,IntPtr wParam,IntPtr lParam)
        {
            if (nCode >= 0)
            {
                theKeyboardHookStruct = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));  //将lParam数据转换成键盘消息
                if ((int)wParam == WM_KEYDOWN && theKeyboardHookStruct.vkCode == (int)(Keys.O))   //如果按键"O"，则运行下面代码
                {
                    //判断(鼠标钩子ID==0)?  Y->安装鼠标钩子  N->卸载鼠标钩子
                    if (MouseHookID == 0)
                    {
                        mySim.Mouse.LeftButtonDown();
                        if(!SetMouseHook())
                        {
                            MessageBox.Show("钩子安装失败", "请确认有管理员权限");
                            Application.Exit();
                        }
                        myForm.theCrosshair.Visible = true;
                        POINT pt = new POINT();
                        GetCursorPos(out pt);
                        theMouseCenter = new Vector2(pt.x, pt.y);
                        RECT myRECT = new RECT(pt.x - radius, pt.y - radius, pt.x + radius, pt.y + radius);
                        if (!ClipCursor(ref myRECT)) MessageBox.Show("鼠标限制失败", "Error");
                        while (iCount >= 0) iCount = ShowCursor(false);
                        myForm.theXYLabel.Text = "X:" + theMouseCenter.x.ToString() + "  Y:" + theMouseCenter.y.ToString();
                        Vector2 CrosshairLoc = theScreenCenter - new Vector2(myForm.theCrosshair.Size.Width / 2, myForm.theCrosshair.Size.Height / 2);
                        myForm.theCrosshair.Location = new Point(CrosshairLoc.x, CrosshairLoc.y);
                    }
                    else
                    {
                        myForm.theCrosshair.Visible = false;
                        RECT myRECT = new RECT(0, 0, 1365, 767);
                        if (!ClipCursor(ref myRECT)) MessageBox.Show("鼠标释放失败", "Error");
                        while (iCount < 0) iCount = ShowCursor(true);
                        if (!UnloadMouseHook())
                        {
                            MessageBox.Show("钩子卸载失败", "请确认有管理员权限");
                            Application.Exit();
                        }
                        mySim.Mouse.LeftButtonUp();
                    }
                    return 1;
                }
                //返回1，则结束消息，这个消息到此为止，不再传递；如果返回0或调用CallNextHookEx函数则消息继续往下传递
            }
            return CallNextHookEx(KeyboardHookID, nCode, wParam, lParam);
        }

        /// <summary>
        /// 鼠标钩子的安装与卸载
        /// </summary>
        /// <returns></returns>
        //设置鼠标钩子，失败返回0
        public bool SetMouseHook()
        {
            theMouseProcess = new HookProc(MouseHookProc);
            MouseHookID = SetWindowsHookEx(WH_MOUSE_LL, theMouseProcess, Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);
            if (MouseHookID == 0) return false;
            return true;
        }
        //卸载鼠标钩子
        public bool UnloadMouseHook()
        {
            bool reet = UnhookWindowsHookEx(MouseHookID);
            if (!reet) return false;       //钩子没卸载
            MouseHookID = 0;
            return true;        //钩子已卸载
        }

        /// <summary>
        /// 键盘钩子的安装与卸载
        /// </summary>
        /// <returns></returns>
        //设置键盘钩子，失败返回0
        public bool SetKeybdHook()
        {
            if (KeyboardHookID == 0)
            {
                theKeyboardProcess = new HookProc(KeyboardHookProc);
                KeyboardHookID = SetWindowsHookEx(WH_KEYBOARD_LL, theKeyboardProcess, Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);
                if (KeyboardHookID == 0) return false;
                return true;
            }
            return true;        //钩子已安装
        }
        //卸载键盘钩子
        public bool UnloadKeybdHook()
        {
            if (KeyboardHookID != 0)
            {
                bool reet = UnhookWindowsHookEx(KeyboardHookID);
                if (!reet) return false;       //钩子没卸载
                KeyboardHookID = 0;
                return true;        //钩子已卸载
            }
            return true;            //钩子未安装
        }

        /// <summary>
        /// 根据鼠标位置计算准星位置
        /// </summary>
        /// <param name="MousePoint"></param>
        private void Crosshair(Vector2 MousePoint)
        {
            if (myForm.theCrosshair.Visible)
            {
                Size theCrosshairSize = myForm.theCrosshair.Size;
                Vector2Double theCrosshairVector = (Vector2Double)(MousePoint - theMouseCenter) * theProportion;
                if (theCrosshairVector.Length > bigRadius) theCrosshairVector = new Vector2Double(theCrosshairVector.x * (bigRadius / theCrosshairVector.Length), theCrosshairVector.y * (bigRadius / theCrosshairVector.Length));
                Vector2 CrosshairPosition = (Vector2)theCrosshairVector + theScreenCenter - new Vector2(theCrosshairSize.Width / 2, theCrosshairSize.Height / 2);
                myForm.theCrosshair.Location = new Point(CrosshairPosition.x, CrosshairPosition.y);
            }
        }
        /// <summary>
        /// 这里是构造函数，初始化时把主窗体传给此函数
        /// </summary>
        /// <param name="theForm"></param>
        public theMouseKeybdHook(WaiGuaTest.theMainForm theForm)
        {
            myForm = theForm;
            theProportion = bigRadius / radius;
        }
    }
}
