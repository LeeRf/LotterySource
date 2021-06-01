using DoubleBalls.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleBalls.Model
{
    /// <summary>
    /// 循环摇奖数据汇总类
    /// </summary>
    public class LoopDataSummarizing
    {
        /// <summary>
        /// 序列号
        /// </summary>
        public int SerialId { get; set; }
        /// <summary>
        /// 摇奖期数
        /// </summary>
        public long LoopPeriodsCount { get; set; }

        /// <summary>
        /// 中奖总注
        /// </summary>
        public long LoopWinPrizeLotteryTotalZhu { get; set; }
        /// <summary>
        /// 未中奖注
        /// </summary>
        public long LoopNotLotteryTotalZhu { get; set; }
        /// <summary>
        /// 购买总注
        /// </summary>
        public long buyTotalZhu { get; set; }

        /// <summary>
        /// 中奖总额
        /// </summary>
        public long LoopWinPrizeLotteryTotalMoney { get; set; }
        /// <summary>
        /// 消费总额
        /// </summary>
        public long LoopTotalConsumptionMoney { get; set; }


        /// <summary>
        /// 一等奖注
        /// </summary>
        public int OnePrizeCount { get; set; }

        public SimplexDoubleBallNumber OnePrizeSimplexDoubleBall { get; set; }

        /// <summary>
        /// 二等奖注
        /// </summary>
        public int TwoPrizeCount { get; set; }

        public SimplexDoubleBallNumber TwoPrizeSimplexDoubleBall { get; set; }

        /// <summary>
        /// 三等奖注
        /// </summary>
        public int ThreePrizeCount { get; set; }

        public SimplexDoubleBallNumber ThreePrizeSimplexDoubleBall { get; set; }

        /// <summary>
        /// 四等奖注
        /// </summary>
        public int FourPrizeCount { get; set; }

        public SimplexDoubleBallNumber FourPrizeSimplexDoubleBall { get; set; }

        /// <summary>
        /// 五等奖注
        /// </summary>
        public int FivePrizeCount { get; set; }

        public SimplexDoubleBallNumber FivePrizeSimplexDoubleBall { get; set; }

        /// <summary>
        /// 六等奖注
        /// </summary>
        public int SixPrizeCount { get; set; }

        public SimplexDoubleBallNumber SixPrizeSimplexDoubleBall { get; set; }

        /// <summary>
        /// 获取战损情况
        /// </summary>
        /// <returns></returns>
        public string GetProfitTotalMoney()
        {
            string profitText;
            long profitTotalMoney = 0;


            if (LoopTotalConsumptionMoney > LoopWinPrizeLotteryTotalMoney)
            {
                profitText = "亏";
                profitTotalMoney = LoopTotalConsumptionMoney - LoopWinPrizeLotteryTotalMoney;
            }
            else
            {
                profitText = "赚";
                profitTotalMoney = LoopWinPrizeLotteryTotalMoney - LoopTotalConsumptionMoney;
            }

            return string.Format(Info.LotteryMoneyConutMessage, profitText + " "
                + Info.GetNumberMaxUnit(profitTotalMoney), DoubleBallView.FormatNumber(profitTotalMoney.ToString(), 5));
        }

        /// <summary>
        /// 根据下标获取双色球对象
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public SimplexDoubleBallNumber GetSimplexBallByIndex(int index)
        {
            if (index == 0)
            {
                return OnePrizeSimplexDoubleBall;
            }
            else if(index == 1)
            {
                return TwoPrizeSimplexDoubleBall;
            }
            else if (index == 2)
            {
                return ThreePrizeSimplexDoubleBall;
            }
            else if (index == 3)
            {
                return FourPrizeSimplexDoubleBall;
            }
            else if (index == 4)
            {
                return FivePrizeSimplexDoubleBall;
            }
            else if (index == 5)
            {
                return SixPrizeSimplexDoubleBall;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取该循环期数摇了多少年
        /// </summary>
        /// <param name="zhu">单期购买注数</param>
        /// <param name="money">单期花费金额</param>
        public decimal GetLoopPeriodsMessage(int round)
        {
            decimal yearPeriodsDecimal = new decimal(52 * 3);
            decimal loopPeriodsDecimal = new decimal(LoopPeriodsCount);

            return decimal.Round(loopPeriodsDecimal / yearPeriodsDecimal, round);
        }

        /// <summary>
        /// 获取做公益的金额
        /// </summary>
        public decimal GetPublicBenefitMoney()
        {
            decimal publicBenefitPercentum = new decimal(0.36);
            decimal publicBenefitMoney = new decimal(LoopTotalConsumptionMoney);
            return Math.Round(publicBenefitPercentum * publicBenefitMoney, 2);
        }
    }
}
