using DoubleBalls.Model;
using DoubleBalls.Other;
using DoubleBalls.Style;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DoubleBalls
{
    public partial class LoopDataAnalyse : SkinMain
    {
        DoubleBallView _doubleBallView;

        public static int colorIndex = 0;
        //深蓝、灰色、天空蓝、水彩粉
        public static int[] colorArgb = { -2892833 , -1382941, -2232323, -270873 };
        
        private LoopDataSummarizing _loopDataSummarizing;
        /// <summary>
        /// 存储几等奖对应得那期公共开奖号码
        /// </summary>
        public SimplexDoubleBallNumber[] _LotteryDoubleBallNumbers = new SimplexDoubleBallNumber[6];

        public LoopDataAnalyse(DoubleBallView doubleBallView, LoopDataSummarizing loopDataSummarizing) 
        {
            InitializeComponent();
            _doubleBallView = doubleBallView;
            _loopDataSummarizing = loopDataSummarizing;
        }

        private void LoopDataAnalyse_Load(object sender, EventArgs e)
        {
            Height = _doubleBallView.Height;

            Top = _doubleBallView.Top;

            if (_doubleBallView.Left == 0)
            {
                Left = Screen.PrimaryScreen.WorkingArea.Width - Width;
            }
            else
            {
                Left = _doubleBallView.Left + (_doubleBallView.Width - Width);
            }

            BackColor = Color.FromArgb(colorArgb[colorIndex]);
            panBottom.BackColor = Color.White;

            SetWindowRegion(panBottom, 50);

            Win32.AnimateWindow(Handle, 200, Win32.AW_ACTIVATE | Win32.AW_HOR_NEGATIVE | Win32.AW_SLIDE);
            Opacity = 0.95;

            colorIndex++;
            if (colorIndex == colorArgb.Length)
            {
                colorIndex = 0;
            }

            ShowLoopData();
        }

        #region ShowLoopData

        private void ShowLoopData()
        {
            _loopDataSummarizing.LoopNotLotteryTotalZhu = _loopDataSummarizing.buyTotalZhu - _loopDataSummarizing.LoopWinPrizeLotteryTotalZhu;

            int onePrizeCount = _loopDataSummarizing.OnePrizeCount;
            string formatOnePrizeCount = DoubleBallView.FormatNumber(onePrizeCount, 5);
            lblOnePrize.Text = string.Format(Info.LotteryZhuCountMessage, Info.GetNumberMaxUnit(onePrizeCount), formatOnePrizeCount);

            int twoPrizeCount = _loopDataSummarizing.TwoPrizeCount;
            string formatTwoPrizeCount = DoubleBallView.FormatNumber(twoPrizeCount, 5);
            lblTwoPrize.Text = string.Format(Info.LotteryZhuCountMessage, Info.GetNumberMaxUnit(twoPrizeCount), formatTwoPrizeCount);

            int threePrizeCount = _loopDataSummarizing.ThreePrizeCount;
            string formatThreePrizeCount = DoubleBallView.FormatNumber(threePrizeCount, 5);
            lblThreePrize.Text = string.Format(Info.LotteryZhuCountMessage, Info.GetNumberMaxUnit(threePrizeCount), formatThreePrizeCount);

            int fourPrizeCount = _loopDataSummarizing.FourPrizeCount;
            string formatFourPrizeCount = DoubleBallView.FormatNumber(fourPrizeCount, 5);
            lblFourPrize.Text = string.Format(Info.LotteryZhuCountMessage, Info.GetNumberMaxUnit(fourPrizeCount), formatFourPrizeCount);

            int fivePrizeCount = _loopDataSummarizing.FivePrizeCount;
            string formatFivePrizeCount = DoubleBallView.FormatNumber(fivePrizeCount, 5);
            lblFivePrize.Text = string.Format(Info.LotteryZhuCountMessage, Info.GetNumberMaxUnit(fivePrizeCount), formatFivePrizeCount);

            int sixPrizeCount = _loopDataSummarizing.SixPrizeCount;
            string formatSixPrizeCount = DoubleBallView.FormatNumber(sixPrizeCount, 5);
            lblSixPrize.Text = string.Format(Info.LotteryZhuCountMessage, Info.GetNumberMaxUnit(sixPrizeCount), formatSixPrizeCount);

            long loopWinPrizeLotteryTotalZhu = _loopDataSummarizing.LoopWinPrizeLotteryTotalZhu;
            string formatLoopWinPrizeLotteryTotalZhu = DoubleBallView.FormatNumber(loopWinPrizeLotteryTotalZhu.ToString(), 5);
            lblLotteryAwardTotal.Text = string.Format(Info.LotteryZhuCountMessage, Info.GetNumberMaxUnit(loopWinPrizeLotteryTotalZhu), formatLoopWinPrizeLotteryTotalZhu);

            long loopNotLotteryTotalZhu = _loopDataSummarizing.LoopNotLotteryTotalZhu;
            string formatLoopNotLotteryTotalZhu = DoubleBallView.FormatNumber(loopNotLotteryTotalZhu.ToString(), 5);
            lblNotAwardTotal.Text = string.Format(Info.LotteryZhuCountMessage, Info.GetNumberMaxUnit(loopNotLotteryTotalZhu), formatLoopNotLotteryTotalZhu);

            long buyTotalZhu = _loopDataSummarizing.buyTotalZhu;
            string formatBuyTotalZhu = DoubleBallView.FormatNumber(buyTotalZhu.ToString(), 5);
            lblBuyTotalZhu.Text = string.Format(Info.LotteryZhuCountMessage, Info.GetNumberMaxUnit(buyTotalZhu), formatBuyTotalZhu);

            long loopWinPrizeLotteryTotalMoney = _loopDataSummarizing.LoopWinPrizeLotteryTotalMoney;
            string LoopWinPrizeLotteryTotalMoney = DoubleBallView.FormatNumber(loopWinPrizeLotteryTotalMoney.ToString(), 5);
            lblLotteryTotalAwardMoney.Text = string.Format(Info.LotteryMoneyConutMessage, Info.GetNumberMaxUnit(loopWinPrizeLotteryTotalMoney), LoopWinPrizeLotteryTotalMoney);

            long loopTotalConsumptionMoney = _loopDataSummarizing.LoopTotalConsumptionMoney;
            string formatLoopTotalConsumptionMoney = DoubleBallView.FormatNumber(loopTotalConsumptionMoney.ToString(), 5);
            lblBuyTotalMoney.Text = string.Format(Info.LotteryMoneyConutMessage, Info.GetNumberMaxUnit(loopTotalConsumptionMoney), formatLoopTotalConsumptionMoney);


            lblProfitTotalMoney.Text = _loopDataSummarizing.GetProfitTotalMoney();
            lblCharityTotalMoney.Text = string.Format(Info.PublicBenefitTotalMoney, _loopDataSummarizing.GetPublicBenefitMoney());

            lblStopCondition.Text = Info.GetLotteryChineseMessage(_doubleBallView.GetLoopStopCondition());

            lblMultiple.Text = DoubleBallView.FormatNumber(_doubleBallView.GetMulttple(), 3) + Info.Multiple;

            long loopPeriodsCount = _loopDataSummarizing.LoopPeriodsCount;
            string formatLoopPeriodsCount = DoubleBallView.FormatNumber(loopPeriodsCount.ToString(), 5);
            lblLoopPeriods.Text = string.Format(Info.LotteryPeriodsCountMessage, Info.GetNumberMaxUnit(loopPeriodsCount), formatLoopPeriodsCount);

            lblLoopZhu.Text = DoubleBallView.FormatNumber(_doubleBallView.GetDoubleBallConsumeZhu(), 3);
            lblLoopMoney.Text = DoubleBallView.FormatNumber(_doubleBallView.GetRandomMoney(), 3);

            int prizeIndex = 0;
            foreach (Control item in panBottom.Controls)
            {
                if (item is Panel panPrize && "Prize".Equals(panPrize.Tag.ToString()))
                {
                    //开奖号为空进入下次循环、并将奖得下标++
                    if(_LotteryDoubleBallNumbers[prizeIndex] == null)
                    {
                        prizeIndex++;
                        continue;
                    }
                    int ballIndex = 0;
                    foreach (Label labelBall in panPrize.Controls)
                    {
                        SimplexDoubleBallNumber ballNumber = _loopDataSummarizing.GetSimplexBallByIndex(prizeIndex);
                        if (ballIndex == 6)
                        {
                            //最后一个篮球、复制完跳出本次循环
                            labelBall.Text = DoubleBallView.FormatNumber(ballNumber.blueBall, 2);
                            break;
                        }
                        //赋值6个红球
                        labelBall.Text = DoubleBallView.FormatNumber(ballNumber.redBalls[ballIndex], 2);
                        ballIndex++;
                    }
                    prizeIndex++;
                }
            }

            bool expectationReward = false;
            AwardType myRewardAwardType = AwardType.NotAward;
            AwardType stopExpectationReward = _doubleBallView.GetLoopStopCondition();

            if (_loopDataSummarizing.SixPrizeSimplexDoubleBall != null)
            {
                if (_loopDataSummarizing.SixPrizeSimplexDoubleBall.awardType >= stopExpectationReward)
                {
                    expectationReward = true;
                    myRewardAwardType = _loopDataSummarizing.SixPrizeSimplexDoubleBall.awardType;
                }
                _doubleBallView.DrawingSimplexColorLotteryNumber(panSixPrize, _loopDataSummarizing.SixPrizeSimplexDoubleBall, _LotteryDoubleBallNumbers[5]);
            }

            if (_loopDataSummarizing.FivePrizeSimplexDoubleBall != null)
            {
                if (_loopDataSummarizing.FivePrizeSimplexDoubleBall.awardType >= stopExpectationReward)
                {
                    expectationReward = true;
                    myRewardAwardType = _loopDataSummarizing.FivePrizeSimplexDoubleBall.awardType;
                }
                _doubleBallView.DrawingSimplexColorLotteryNumber(panFivePrize, _loopDataSummarizing.FivePrizeSimplexDoubleBall, _LotteryDoubleBallNumbers[4]);
            }

            if (_loopDataSummarizing.FourPrizeSimplexDoubleBall != null)
            {
                if (_loopDataSummarizing.FourPrizeSimplexDoubleBall.awardType >= stopExpectationReward)
                {
                    expectationReward = true;
                    myRewardAwardType = _loopDataSummarizing.FourPrizeSimplexDoubleBall.awardType;
                }
                _doubleBallView.DrawingSimplexColorLotteryNumber(panFourPrize, _loopDataSummarizing.FourPrizeSimplexDoubleBall, _LotteryDoubleBallNumbers[3]);
            }

            if (_loopDataSummarizing.ThreePrizeSimplexDoubleBall != null)
            {
                if (_loopDataSummarizing.ThreePrizeSimplexDoubleBall.awardType >= stopExpectationReward)
                {
                    expectationReward = true;
                    myRewardAwardType = _loopDataSummarizing.ThreePrizeSimplexDoubleBall.awardType;
                }
                _doubleBallView.DrawingSimplexColorLotteryNumber(panThreePrize, _loopDataSummarizing.ThreePrizeSimplexDoubleBall, _LotteryDoubleBallNumbers[2]);
            }

            if (_loopDataSummarizing.TwoPrizeSimplexDoubleBall != null)
            {
                if (_loopDataSummarizing.TwoPrizeSimplexDoubleBall.awardType >= stopExpectationReward)
                {
                    expectationReward = true;
                    myRewardAwardType = _loopDataSummarizing.TwoPrizeSimplexDoubleBall.awardType;
                }
                _doubleBallView.DrawingSimplexColorLotteryNumber(panTwoPrize, _loopDataSummarizing.TwoPrizeSimplexDoubleBall, _LotteryDoubleBallNumbers[1]);
            }

            if (_loopDataSummarizing.OnePrizeSimplexDoubleBall != null)
            {
                if(_loopDataSummarizing.OnePrizeSimplexDoubleBall.awardType >= stopExpectationReward)
                {
                    expectationReward = true;
                    myRewardAwardType = _loopDataSummarizing.OnePrizeSimplexDoubleBall.awardType;
                }
                _doubleBallView.DrawingSimplexColorLotteryNumber(panOnePrize, _loopDataSummarizing.OnePrizeSimplexDoubleBall, _LotteryDoubleBallNumbers[0]);
            }

            decimal loopYear = _loopDataSummarizing.GetLoopPeriodsMessage(2);

            if (!expectationReward)
            {
                lblMessage.Visible = true;
                picLoop.Image = Properties.Resources.regret;
                lblMessage.Text = string.Format(Info.NotExpectationReward, loopYear, Info.GetLotteryChineseMessage(_doubleBallView.GetLoopStopCondition()));
            }
            else
            {
                lblMessage1.Text = Info.GetLoopDataAnalyseInfo(Config.Setting);

                lblLoopYear.Text = loopYear.ToString();
                lblYear1.Text = string.Format(Info.LoopYearMessage, Info.GetNumberMaxUnit((int)loopYear));

                panMessage.Size = new Size(
                    lblMessage1.Width + lblLoopZhu.Width + lblMessage2.Width + lblLoopMoney.Width + lblMessage3.Width + 40, panMessage.Height);
                panMessage.Left = (Width - panMessage.Width) / 2;

                panLoopYear.Size = new Size(lblYear1.Width + lblLoopYear.Width + lblYear2.Width + 20, panLoopYear.Height);
                panLoopYear.Left = (Width - panLoopYear.Width) / 2;

                panMessage.Visible = true;
                panLoopYear.Visible = true;

                //中奖总额大于消费总额时，并且能在有生之年赚到钱就替换开心表情
                if (_loopDataSummarizing.LoopWinPrizeLotteryTotalMoney > _loopDataSummarizing.LoopTotalConsumptionMoney && loopYear <= 100)
                {
                    picLoop.Image = Properties.Resources.earned;
                }

                if (myRewardAwardType > stopExpectationReward)
                {
                    //MessageBox.Show("运气爆棚了、这是顺便中一个" + Info.GetLotteryChineseMessage(myRewardAwardType) + "");
                }
            }

            picLoop.Left = (Width - picLoop.Width) / 2;

            picLoop.Visible = true;
        }

        #endregion

        #region unimportance code

        public static void SetWindowRegion(Control control, int radius)
        {
            Rectangle rect = new Rectangle(0, 0, control.Width, control.Height);
            var formPath = GetRoundedRectPath(rect, radius);
            control.Region = new Region(formPath);
        }

        /// <summary>
        /// 绘制圆角窗体
        /// </summary>
        /// <param name="rect">绘制大小</param>
        private static GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            Rectangle arcRect = new Rectangle(rect.Location, new Size(radius, radius));
            GraphicsPath path = new GraphicsPath();
            //   左上角
            path.AddArc(arcRect, 180, 90);
            //   右上角
            arcRect.X = rect.Right - radius;
            path.AddArc(arcRect, 270, 90);
            //   右下角
            arcRect.Y = rect.Bottom /*- radius*/;
            path.AddArc(arcRect, 0, 90);
            //   左下角
            arcRect.X = rect.Left;
            path.AddArc(arcRect, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void btnRandom_Click(object sender, EventArgs e)
        {
            base.ThisVisible(false);
            Win32.AnimateWindow(Handle, 200, Win32.AW_HIDE | Win32.AW_HOR_POSITIVE | Win32.AW_SLIDE);

            this.Close();
        }

        #endregion
    }
}
