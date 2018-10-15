using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace WaiGuaTest
{
    public partial class theMainForm : Form
    {
        private Penetrate.thePenetrateFunc myPenetrateObj;
        private Hook.theMouseKeybdHook myHookObj;
        //private DrawShape.Circle myCircle;
        public theMainForm()
        {
            InitializeComponent();
        }

        private void theMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!myHookObj.UnloadKeybdHook())
            {
                MessageBox.Show("请确认有管理员权限。", "钩子卸载失败！");
                Application.Exit();
            }
        }

        private void theMainForm_Load(object sender, EventArgs e)
        {
            try
            {
                string myPath = Application.StartupPath;
                StreamReader myReader = new StreamReader(myPath + @"\Data.txt");
                List<string> LineList = new List<string>();
                string sLine = "";
                while (sLine != null)
                {
                    sLine = myReader.ReadLine();
                    if (sLine != null && !(sLine.Equals(""))) LineList.Add(sLine);
                }
                myReader.Close();
                if (LineList.Count != 5)
                {
                    throw new IOException("文件内容不合法");
                }
                Hook.theMouseKeybdHook.theScreenCenter.x = int.Parse(LineList[0]);
                Hook.theMouseKeybdHook.theScreenCenter.y = int.Parse(LineList[1]);
                Hook.theMouseKeybdHook.radius = int.Parse(LineList[2]);
                Hook.theMouseKeybdHook.bigRadius = int.Parse(LineList[3]);
                int theCrosshairSize = int.Parse(LineList[4]);
                theCrosshair.Size = new System.Drawing.Size(theCrosshairSize, theCrosshairSize);
            }
            catch(Exception myExp)
            {
                MessageBox.Show(myExp.ToString(), "配置文件读取失败。");
            }
            finally
            {
                myHookObj = new Hook.theMouseKeybdHook(this);
            }
            if (!myHookObj.SetKeybdHook())
            {
                MessageBox.Show("请检查是否已获取管理员权限。", "钩子安装失败！");
                Application.Exit();
            }
            myPenetrateObj = new Penetrate.thePenetrateFunc(this);
            myPenetrateObj.CanPenetrate();
            //myCircle = new DrawShape.Circle(Hook.theMouseHook.theScreenCenter, this);
        }
    }
}