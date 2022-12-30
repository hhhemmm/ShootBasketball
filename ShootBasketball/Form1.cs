using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ShootBasketball.BaseClass;
using System.Runtime.InteropServices;


namespace ShootBasketball
{
    public partial class FrmSB : Form
    {
        public FrmSB()
        {
            InitializeComponent();
        }
        [DllImport("winmm")]
        public static extern bool PlaySound(string szSound, int Mod, int i);


        //加载并显示元素
        Bitmap bmpBackground;                                                           //背景位图变量
        static readonly Rectangle GameBasketballRect = new Rectangle(0, 0, 1025, 1025);             //篮球矩形
        Bitmap bmpBasketball;                                                           //篮球
        Point centerOfBasketball;                                                       //篮球在窗口中的坐标
        static readonly Rectangle GameBackoardRect = new Rectangle(0, 0, 100, 100);               //篮板矩形
        Bitmap bmpBackboard;                                                            //篮板
        static readonly Rectangle GameBasketryRect = new Rectangle(0, 0, 100, 100);               //篮筐矩形
        Bitmap bmpBasketry;                                                             //篮筐
        int score = 0;

        private void LoadPictures()
        {
            //加载篮筐
            Bitmap bmpBackboard_all = LoadBitmap.LoadBmp("Backboard");
            bmpBackboard = bmpBackboard_all.Clone(GameBackoardRect, bmpBackboard_all.PixelFormat);        //裁切篮板
            //加载背景
            bmpBackground = LoadBitmap.LoadBmp("Background");
            //加载篮球
            Bitmap bmpBasketball_all = LoadBitmap.LoadBmp("Basketball");
            bmpBasketball = bmpBasketball_all.Clone(GameBasketballRect, bmpBasketball_all.PixelFormat);     //裁切篮球
            //加载篮筐
            Bitmap bmpBasketry_all = LoadBitmap.LoadBmp("Basketry");
            bmpBasketry = bmpBasketry_all.Clone(GameBasketryRect, bmpBasketry_all.PixelFormat);
        }
        int time_remain = 100;
        private void FrmSB_Load(object sender, EventArgs e)
        {
            this.LoadPictures();
            //初始化篮球位置
            centerOfBasketball.X = 10;
            centerOfBasketball.Y = this.ClientRectangle.Height / 2;
            BallClass.BallRect = new Rectangle(centerOfBasketball.X - 25, centerOfBasketball.Y - 25, 50, 50);
            BallClass.BallDirection = new Vector2(0f, 0f);
            MessageBox.Show("来测测你的投篮技术吧！\n 鼠标左键抓球，通过拖拽将篮球投入篮筐 \n只有黑线下才能抓球", "提示");
            this.TimerRefreshScreen.Start();
            time_remain = 100;
            this.timer1.Start();
        }
        Rectangle rectboard;
        Rectangle rectbasketry;
        private void DrawGame(Graphics graphic)
        {
            
            graphic.DrawImage(bmpBackground, new Rectangle(0, 0, this.ClientRectangle.Width - 1, this.ClientRectangle.Height - 1));
            graphic.DrawImage(bmpBasketball, BallClass.BallRect);
            rectboard = new Rectangle(this.ClientRectangle.Width - 50, 30, 10, 150);
            graphic.DrawImage(bmpBackboard, rectboard);
            rectbasketry = new Rectangle(this.ClientRectangle.Width - 120, 130, 70, 10);
            graphic.DrawImage(bmpBasketry, rectbasketry);
            graphic.DrawString("Score " + score.ToString(),new Font("Arial Black", 24),new SolidBrush(Color.Black),new Point(5, 5));
            graphic.DrawString("time:"+time_remain.ToString(), new Font("Arial Black", 24), new SolidBrush(Color.Black), new Point(250, 5));
            if (time_remain <= 0)
            {
                graphic.DrawString("Game Over", new Font("Arial Black", 30), new SolidBrush(Color.White), new Point(200, 200));
                if(score >= 15)
                {
                    graphic.DrawString("您是投篮大神！", new Font("Arial Black", 30), new SolidBrush(Color.White), new Point(200, 250));
                }
                else if(score >= 10)
                {
                    graphic.DrawString("您是投篮高手！", new Font("Arial Black", 30), new SolidBrush(Color.White), new Point(200, 250));
                }
                else if(score >= 7)
                {
                    graphic.DrawString("您投得还行！", new Font("Arial Black", 30), new SolidBrush(Color.White), new Point(200, 250));
                }
                else if(score >= 4)
                {
                    graphic.DrawString("您投得一般。。。", new Font("Arial Black", 30), new SolidBrush(Color.White), new Point(200, 250));
                }
                else
                {
                    graphic.DrawString("您是投篮菜鸟。。。", new Font("Arial Black", 30), new SolidBrush(Color.White), new Point(200, 250));
                }
            }
               
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Bitmap bufferBmp = new Bitmap(this.ClientRectangle.Width - 1, this.ClientRectangle.Height - 1);
            Graphics g = Graphics.FromImage(bufferBmp);
            this.DrawGame(g);
            e.Graphics.DrawImage(bufferBmp, 0, 0);
            g.Dispose();
            base.OnPaint(e);
        }


        //碰撞检测
        private bool IsHit()
        {
            if (BallClass.BallRect.X < -10) //左边框
            {
                BallClass.BallRect.X = -10;
                BallClass.BallDirection.X = - (float)0.7*BallClass.BallDirection.X;
                return true;
            }
            if (BallClass.BallRect.X > this.ClientRectangle.Width - 50)//右边框
            {
                BallClass.BallRect.X = this.ClientRectangle.Width - 50;
                BallClass.BallDirection.X = -(float)0.7*BallClass.BallDirection.X;
                return true;
            }
            if (BallClass.BallRect.Y > this.ClientRectangle.Height - 50) //下边框
            {
                BallClass.BallRect.Y = this.ClientRectangle.Height - 50;
                BallClass.BallDirection.Y = -(float)0.7 * BallClass.BallDirection.Y;
                if(Math.Abs(BallClass.BallDirection.Y) < 1)
                {
                    BallClass.BallDirection.Y = 0;
                }
                return true;
            }
  
            return false;
        }
        private bool IsHit_board()
        {
            if (BallClass.BallRect.IntersectsWith(rectboard))
            {
                if (Math.Abs(rectboard.X - (BallClass.BallRect.X + 50)) < 20)
                {
                    BallClass.BallRect.X = rectboard.X - 50;
                    BallClass.BallDirection.X = -BallClass.BallDirection.X;
                }
                else
                {
                    if (BallClass.BallDirection.Y < 0)
                    {
                        BallClass.BallDirection.Y = -BallClass.BallDirection.Y;
                        BallClass.BallRect.Y = 180;
                    }
                    else
                    {
                        BallClass.BallDirection.Y = -BallClass.BallDirection.Y;
                    }
                }
                return true;
            }
            return false;
        }
        private bool IsHit_basketry()
        {
            Rectangle rectbasketry1 = new Rectangle(this.ClientRectangle.Width - 120, 130, 10, 10);
            if (BallClass.BallRect.IntersectsWith(rectbasketry1))
            {
                if(Math.Abs(rectbasketry.X - (BallClass.BallRect.X + 50)) < 20)
                {
                    BallClass.BallRect.X = rectbasketry.X - 50;
                    BallClass.BallDirection.X = (float)-0.7*BallClass.BallDirection.X;
                }
                else if (Math.Abs(rectbasketry.X - BallClass.BallRect.X) < 20)
                {
                    BallClass.BallRect.X = rectbasketry.X + 10;
                    BallClass.BallDirection.X = (float)-0.7*BallClass.BallDirection.X;
                }
                else
                {
                   BallClass.BallDirection.Y = (float)-0.7 * BallClass.BallDirection.Y;
                    if( BallClass.BallRect.Y >= rectbasketry.Y)
                    {
                        BallClass.BallRect.Y = rectbasketry.Y + 10;
                    }
                    else
                    {
                        BallClass.BallRect.Y = rectbasketry.Y - 50;
                    }
                   
                }
                return true;
            }
            return false;
        }
        int flagup = 0;         //向上抛球的标志
        private bool Is_score()
        {
            Rectangle rectscore = new Rectangle(this.ClientRectangle.Width - 90, 140, 10, 10);

            if (BallClass.BallRect.IntersectsWith(rectscore)  )
            {
                if (BallClass.BallDirection.Y > 0)
                {
                    return true;
                }
                else
                {
                    flagup = 1;
                    return false; 
                }
            }
            return false;
        }


        bool bSounding = false;
        //声音播放
        private void Play(string waveName)
        {
            PlaySound(Application.StartupPath + "\\GameSounds\\" + waveName + ".wav",0, 1);
        }

        int timer = 100;
        int flag_stop = 0;
        //计时器
        private void TimerRefreshScreen_Tick(object sender, EventArgs e)
        {
            if(flag_stop == 1)
            {
                this.Invalidate();
                return;
            }

            //模拟重力
            BallClass.BallDirection.Y += (float)0.16;

            //是否和边框碰撞
            if (IsHit())
            {
                Play("hit");
            }

            //是否和篮板碰撞
            if (IsHit_board() && bSounding == false)
            {
                Play("hit_board");
                bSounding = true;
                this.timer2.Start();
            }

            //是否和篮筐碰撞
            if (IsHit_basketry()&&bSounding == false)
            {
                Play("hit_basketry");
                bSounding = true;
                this.timer2.Start();
            }
            timer++;
            //是否进球
            if (Is_score() && timer > 200)
            {
                if(flagup == 1)
                {
                    flagup = 0;
                }
                else
                {
                    score++;
                }
                timer = 0;
                Play("score");
                bSounding = true;
                this.timer2.Start();
            }

            //模拟摩擦力
            if (BallClass.BallDirection.X > 0)
            {
                //地面摩擦
          
                if(BallClass.BallRect.Y == this.ClientRectangle.Height - 50)
                {
                    BallClass.BallDirection.X -= (float)0.05;
                }
                else
                {
                    //空气摩擦
                    BallClass.BallDirection.X -= (float)0.004;
                }
            }
            else if(BallClass.BallDirection.X < 0)
            {
                if (BallClass.BallRect.Y == this.ClientRectangle.Height - 50)
                {
                    BallClass.BallDirection.X += (float)0.05;
                }
                else
                {
                    //空气摩擦
                    BallClass.BallDirection.X += (float)0.004;
                }
            }

            //判断停止
            if(Math.Abs(BallClass.BallDirection.X) < 1 && Math.Abs(BallClass.BallDirection.Y) < 1 && BallClass.BallRect.Y == this.ClientRectangle.Height - 50)
            {
                flag_stop = 1;
            }

            BallClass.BallRect.X += (int)(BallClass.BallDirection.X);
            BallClass.BallRect.Y += (int)(BallClass.BallDirection.Y);

            this.Invalidate();
        }
        //对鼠标进行捕捉
        private void FrmSB_MouseMove(object sender, MouseEventArgs e)
        {
        
        }
        int mousedown_x, mousedown_y;
        int flag = 0;
        private void FrmSB_MouseDown(object sender, MouseEventArgs e)
        {
            if(Math.Abs(BallClass.BallRect.X + 25 - e.X) <= 25 && Math.Abs(BallClass.BallRect.Y + 25 - e.Y) <= 25 && BallClass.BallRect.Y >= 300)
            {
                flag_stop = 1;
                flag = 1;
                //抓球
                BallClass.BallRect.X = e.X - 25;
                BallClass.BallRect.Y = e.Y - 25;
                BallClass.BallDirection.X = 0;
                BallClass.BallDirection.Y = 0;
                //记录抓球的位置
                mousedown_x = e.X;
                mousedown_y = e.Y;
            }
        }
        private void FrmSB_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphic = e.Graphics;
            Pen pen = new Pen(Color.Black, 2.0f);

            graphic.DrawLine(pen, this.ClientRectangle.Width, 300, 0, 300);


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(time_remain == 0)
            {
                this.TimerRefreshScreen.Stop();
            }
            time_remain--;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            bSounding = false;
            this.timer2.Stop();
        }

        private void FrmSB_MouseUp(object sender, MouseEventArgs e)
        {
            if(flag == 1)
            {
                flag_stop = 0;                  //开始运动
                int vector_x = e.X - mousedown_x;
                int vector_y = e.Y - mousedown_y;
                BallClass.BallDirection.X = vector_x * (float)0.04;
                BallClass.BallDirection.Y = vector_y * (float)0.04;
                flag = 0;
            }

        }
    }
}
