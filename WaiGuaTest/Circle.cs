using WaiGuaTest;
using System.Drawing;
using Hook;

namespace DrawShape
{
    public class Circle
    {
        private Graphics myGra;
        private Pen myPen = new Pen(Color.White);
        public Circle(Vector2 theScreenCenter,theMainForm myForm)
        {
            myGra = myForm.CreateGraphics();
            myGra.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Rectangle myRect = new Rectangle(theScreenCenter.x - theScreenCenter.y, 0, theScreenCenter.x + theScreenCenter.y, theScreenCenter.y * 2);
            myGra.DrawEllipse(myPen, myRect);
        }
    }
}