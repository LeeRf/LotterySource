using SuperLotto.Model;
using SuperLotto.Other;
using SuperLotto.Style;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SuperLotto
{
    public partial class LoopDataAnalyse : SkinMain
    {
        SuperLottoView _superLottoView;

        public static int colorIndex = 0;
        //深蓝、灰色、天空蓝、水彩粉
        public static int[] colorArgb = { -2892833, -1382941, -2232323, -270873 };
        
        private LoopDataSummarizing _loopDataSummarizing;
        /// <summary>
        /// 存储几等奖对应得那期公共开奖号码
        /// </summary>
        public SimplexSuperLottoNumber[] _LotterySuperLottoNumbers = new SimplexSuperLottoNumber[9];

        public LoopDataAnalyse(SuperLottoView superLottoView, LoopDataSummarizing loopDataSummarizing) 
        {
            InitializeComponent();
            _superLottoView = superLottoView;
            _loopDataSummarizing = loopDataSummarizing;
        }

        private void LoopDataAnalyse_Load(object sender, EventArgs e)
        {
            Height = _superLottoView.Height;

            Top = _superLottoView.Top;

            if (_superLottoView.Left == 0)
            {
                Left = Screen.PrimaryScreen.WorkingArea.Width - Width;
            }
            else
            {
                Left = _superLottoView.Left + (_superLottoView.Width - Width);
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

            //9个奖项中奖注数
            int onePrizeCount = _loopDataSummarizing.OnePrizeCount;
            string formatOnePrizeCount = SuperLottoView.FormatNumber(onePrizeCount, 5);
            lblOnePrize.Text = string.Format(Info.LotteryZhuCountMessage, Info.GetNumberMaxUnit(onePrizeCount), formatOnePrizeCount);

            int twoPrizeCount = _loopDataSummarizing.TwoPrizeCount;
            string formatTwoPrizeCount = SuperLottoView.FormatNumber(twoPrizeCount, 5);
            lblTwoPrize.Text = string.Format(Info.LotteryZhuCountMessage, Info.GetNumberMaxUnit(twoPrizeCount), formatTwoPrizeCount);

            int threePrizeCount = _loopDataSummarizing.ThreePrizeCount;
            string formatThreePrizeCount = SuperLottoView.FormatNumber(threePrizeCount, 5);
            lblThreePrize.Text = string.Format(Info.LotteryZhuCountMessage, Info.GetNumberMaxUnit(threePrizeCount), formatThreePrizeCount);

            int fourPrizeCount = _loopDataSummarizing.FourPrizeCount;
            string formatFourPrizeCount = SuperLottoView.FormatNumber(fourPrizeCount, 5);
            lblFourPrize.Text = string.Format(Info.LotteryZhuCountMessage, Info.GetNumberMaxUnit(fourPrizeCount), formatFourPrizeCount);

            int fivePrizeCount = _loopDataSummarizing.FivePrizeCount;
            string formatFivePrizeCount = SuperLottoView.FormatNumber(fivePrizeCount, 5);
            lblFivePrize.Text = string.Format(Info.LotteryZhuCountMessage, Info.GetNumberMaxUnit(fivePrizeCount), formatFivePrizeCount);

            int sixPrizeCount = _loopDataSummarizing.SixPrizeCount;
            string formatSixPrizeCount = SuperLottoView.FormatNumber(sixPrizeCount, 5);
            lblSixPrize.Text = string.Format(Info.LotteryZhuCountMessage, Info.GetNumberMaxUnit(sixPrizeCount), formatSixPrizeCount);

            int sevenPrizeCount = _loopDataSummarizing.SevenPrizeCount;
            string formatSevenPrizeCount = SuperLottoView.FormatNumber(sevenPrizeCount, 5);
            lblSevenPrize.Text = string.Format(Info.LotteryZhuCountMessage, Info.GetNumberMaxUnit(sevenPrizeCount), formatSevenPrizeCount);

            int eightPrizeCount = _loopDataSummarizing.EightPrizeCount;
            string formatEightPrizeCount = SuperLottoView.FormatNumber(eightPrizeCount, 5);
            lblEightPrize.Text = string.Format(Info.LotteryZhuCountMessage, Info.GetNumberMaxUnit(eightPrizeCount), formatEightPrizeCount);

            int ninePrizeCount = _loopDataSummarizing.NinePrizeCount;
            string formatNinePrizeCount = SuperLottoView.FormatNumber(ninePrizeCount, 5);
            lblNinePrize.Text = string.Format(Info.LotteryZhuCountMessage, Info.GetNumberMaxUnit(ninePrizeCount), formatNinePrizeCount);

            //中奖情况展示
            long loopWinPrizeLotteryTotalZhu = _loopDataSummarizing.LoopWinPrizeLotteryTotalZhu;
            string formatLoopWinPrizeLotteryTotalZhu = SuperLottoView.FormatNumber(loopWinPrizeLotteryTotalZhu.ToString(), 5);
            lblLotteryAwardTotal.Text = string.Format(Info.LotteryZhuCountMessage, Info.GetNumberMaxUnit(loopWinPrizeLotteryTotalZhu), formatLoopWinPrizeLotteryTotalZhu);

            long loopNotLotteryTotalZhu = _loopDataSummarizing.LoopNotLotteryTotalZhu;
            string formatLoopNotLotteryTotalZhu = SuperLottoView.FormatNumber(loopNotLotteryTotalZhu.ToString(), 5);
            lblNotAwardTotal.Text = string.Format(Info.LotteryZhuCountMessage, Info.GetNumberMaxUnit(loopNotLotteryTotalZhu), formatLoopNotLotteryTotalZhu);

            long buyTotalZhu = _loopDataSummarizing.buyTotalZhu;
            string formatBuyTotalZhu = SuperLottoView.FormatNumber(buyTotalZhu.ToString(), 5);
            lblBuyTotalZhu.Text = string.Format(Info.LotteryZhuCountMessage, Info.GetNumberMaxUnit(buyTotalZhu), formatBuyTotalZhu);

            long loopWinPrizeLotteryTotalMoney = _loopDataSummarizing.LoopWinPrizeLotteryTotalMoney;
            string LoopWinPrizeLotteryTotalMoney = SuperLottoView.FormatNumber(loopWinPrizeLotteryTotalMoney.ToString(), 5);
            lblLotteryTotalAwardMoney.Text = string.Format(Info.LotteryMoneyConutMessage, Info.GetNumberMaxUnit(loopWinPrizeLotteryTotalMoney), LoopWinPrizeLotteryTotalMoney);

            long loopTotalConsumptionMoney = _loopDataSummarizing.LoopTotalConsumptionMoney;
            string formatLoopTotalConsumptionMoney = SuperLottoView.FormatNumber(loopTotalConsumptionMoney.ToString(), 5);
            lblBuyTotalMoney.Text = string.Format(Info.LotteryMoneyConutMessage, Info.GetNumberMaxUnit(loopTotalConsumptionMoney), formatLoopTotalConsumptionMoney);


            lblProfitTotalMoney.Text = _loopDataSummarizing.GetProfitTotalMoney();
            lblCharityTotalMoney.Text = string.Format(Info.PublicBenefitTotalMoney, _loopDataSummarizing.GetPublicBenefitMoney());

            lblStopCondition.Text = Info.GetLotteryChineseMessage(_superLottoView.GetLoopStopCondition());

            lblMultiple.Text = SuperLottoView.FormatNumber(_superLottoView.GetMulttple(), 3) + Info.Multiple;

            long loopPeriodsCount = _loopDataSummarizing.LoopPeriodsCount;
            string formatLoopPeriodsCount = SuperLottoView.FormatNumber(loopPeriodsCount.ToString(), 5);
            lblLoopPeriods.Text = string.Format(Info.LotteryPeriodsCountMessage, Info.GetNumberMaxUnit(loopPeriodsCount), formatLoopPeriodsCount);

            lblLoopZhu.Text = SuperLottoView.FormatNumber(_superLottoView.GetSuperLottoConsumeZhu(), 3);
            lblLoopMoney.Text = SuperLottoView.FormatNumber(_superLottoView.GetRandomMoney(), 3);

            int prizeIndex = 0;
            foreach (Control item in panBottom.Controls)
            {
                if (item is Panel panPrize && "Prize".Equals(panPrize.Tag.ToString()))
                {
                    //开奖号为空进入下次循环、并将奖得下标++
                    if(_LotterySuperLottoNumbers[prizeIndex] == null)
                    {
                        prizeIndex++;
                        continue;
                    }

                    int ballIndex = 0;
                    foreach (Label labelBall in panPrize.Controls)
                    {
                        SimplexSuperLottoNumber ballNumber = _loopDataSummarizing.GetSimplexBallByIndex(prizeIndex);

                        if (ballIndex >= 5)
                        {
                            int blueIndex = ballIndex - SuperLottos.REDCOUNT;
                            labelBall.Text = SuperLottoView.FormatNumber(ballNumber.blueBalls[blueIndex], 2);

                            //最后两个篮球、复制完跳出本次循环
                            if (ballIndex == 6) break;
                        }
                        else
                        {
                            //赋值5个红球
                            labelBall.Text = SuperLottoView.FormatNumber(ballNumber.redBalls[ballIndex], 2);
                        }
                        ballIndex++;
                    }
                    prizeIndex++;
                }
            }

            bool expectationReward = false;
            AwardType myRewardAwardType = AwardType.NotAward;
            AwardType stopExpectationReward = _superLottoView.GetLoopStopCondition();

            if (_loopDataSummarizing.NinePrizeSimplexSuperLotto != null)
            {
                if (_loopDataSummarizing.NinePrizeSimplexSuperLotto.awardType >= stopExpectationReward)
                {
                    expectationReward = true;
                    myRewardAwardType = _loopDataSummarizing.NinePrizeSimplexSuperLotto.awardType;
                }
                _superLottoView.DrawingSimplexColorLotteryNumber(panNinePrize, _loopDataSummarizing.NinePrizeSimplexSuperLotto, _LotterySuperLottoNumbers[8]);
            }

            if (_loopDataSummarizing.EightPrizeSimplexSuperLotto != null)
            {
                if (_loopDataSummarizing.EightPrizeSimplexSuperLotto.awardType >= stopExpectationReward)
                {
                    expectationReward = true;
                    myRewardAwardType = _loopDataSummarizing.EightPrizeSimplexSuperLotto.awardType;
                }
                _superLottoView.DrawingSimplexColorLotteryNumber(panEightPrize, _loopDataSummarizing.EightPrizeSimplexSuperLotto, _LotterySuperLottoNumbers[7]);
            }

            if (_loopDataSummarizing.SevenPrizeSimplexSuperLotto != null)
            {
                if (_loopDataSummarizing.SevenPrizeSimplexSuperLotto.awardType >= stopExpectationReward)
                {
                    expectationReward = true;
                    myRewardAwardType = _loopDataSummarizing.SevenPrizeSimplexSuperLotto.awardType;
                }
                _superLottoView.DrawingSimplexColorLotteryNumber(panSevenPrize, _loopDataSummarizing.SevenPrizeSimplexSuperLotto, _LotterySuperLottoNumbers[6]);
            }

            if (_loopDataSummarizing.SixPrizeSimplexSuperLotto != null)
            {
                if (_loopDataSummarizing.SixPrizeSimplexSuperLotto.awardType >= stopExpectationReward)
                {
                    expectationReward = true;
                    myRewardAwardType = _loopDataSummarizing.SixPrizeSimplexSuperLotto.awardType;
                }
                _superLottoView.DrawingSimplexColorLotteryNumber(panSixPrize, _loopDataSummarizing.SixPrizeSimplexSuperLotto, _LotterySuperLottoNumbers[5]);
            }

            if (_loopDataSummarizing.FivePrizeSimplexSuperLotto != null)
            {
                if (_loopDataSummarizing.FivePrizeSimplexSuperLotto.awardType >= stopExpectationReward)
                {
                    expectationReward = true;
                    myRewardAwardType = _loopDataSummarizing.FivePrizeSimplexSuperLotto.awardType;
                }
                _superLottoView.DrawingSimplexColorLotteryNumber(panFivePrize, _loopDataSummarizing.FivePrizeSimplexSuperLotto, _LotterySuperLottoNumbers[4]);
            }

            if (_loopDataSummarizing.FourPrizeSimplexSuperLotto != null)
            {
                if (_loopDataSummarizing.FourPrizeSimplexSuperLotto.awardType >= stopExpectationReward)
                {
                    expectationReward = true;
                    myRewardAwardType = _loopDataSummarizing.FourPrizeSimplexSuperLotto.awardType;
                }
                _superLottoView.DrawingSimplexColorLotteryNumber(panFourPrize, _loopDataSummarizing.FourPrizeSimplexSuperLotto, _LotterySuperLottoNumbers[3]);
            }

            if (_loopDataSummarizing.ThreePrizeSimplexSuperLotto != null)
            {
                if (_loopDataSummarizing.ThreePrizeSimplexSuperLotto.awardType >= stopExpectationReward)
                {
                    expectationReward = true;
                    myRewardAwardType = _loopDataSummarizing.ThreePrizeSimplexSuperLotto.awardType;
                }
                _superLottoView.DrawingSimplexColorLotteryNumber(panThreePrize, _loopDataSummarizing.ThreePrizeSimplexSuperLotto, _LotterySuperLottoNumbers[2]);
            }

            if (_loopDataSummarizing.TwoPrizeSimplexSuperLotto != null)
            {
                if (_loopDataSummarizing.TwoPrizeSimplexSuperLotto.awardType >= stopExpectationReward)
                {
                    expectationReward = true;
                    myRewardAwardType = _loopDataSummarizing.TwoPrizeSimplexSuperLotto.awardType;
                }
                _superLottoView.DrawingSimplexColorLotteryNumber(panTwoPrize, _loopDataSummarizing.TwoPrizeSimplexSuperLotto, _LotterySuperLottoNumbers[1]);
            }

            if (_loopDataSummarizing.OnePrizeSimplexSuperLotto != null)
            {
                if(_loopDataSummarizing.OnePrizeSimplexSuperLotto.awardType >= stopExpectationReward)
                {
                    expectationReward = true;
                    myRewardAwardType = _loopDataSummarizing.OnePrizeSimplexSuperLotto.awardType;
                }
                _superLottoView.DrawingSimplexColorLotteryNumber(panOnePrize, _loopDataSummarizing.OnePrizeSimplexSuperLotto, _LotterySuperLottoNumbers[0]);
            }

            decimal loopYear = _loopDataSummarizing.GetLoopPeriodsMessage(2);

            if (!expectationReward)
            {
                lblMessage.Visible = true;
                picLoop.Image = Properties.Resources.regret;
                lblMessage.Text = string.Format(Info.NotExpectationReward, loopYear, Info.GetLotteryChineseMessage(_superLottoView.GetLoopStopCondition()));
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
