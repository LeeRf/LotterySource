using DoubleBalls.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleBalls.Model
{
    /// <summary>
    /// 综合数据汇总类
    /// </summary>
    public class SyntheticDataVariate
    {
        /// <summary>
        /// 序列号
        /// </summary>
        public int serialId { get; set; }
        /// <summary>
        /// 中奖总注
        /// </summary>
        public long winPrizeAwardTotalZhu { get; set; }
        /// <summary>
        /// 购买总注
        /// </summary>
        public long buyTotalZhu { get; set; }
        /// <summary>
        /// 中奖总额
        /// </summary>
        public long winPrizeAwardTotalMoney { get; set; }
        /// <summary>
        /// 消费总额
        /// </summary>
        public long totalConsumptionMoney { get; set; }

        /// <summary>
        /// 获取战损情况
        /// </summary>
        /// <returns></returns>
        public string GetProfitTotalMoney()
        {
            string profitText;
            long profitTotalMoney = 0;


            if (totalConsumptionMoney > winPrizeAwardTotalMoney)
            {
                profitText = "亏";
                profitTotalMoney = totalConsumptionMoney - winPrizeAwardTotalMoney;
            }
            else
            {
                profitText = "赚";
                profitTotalMoney = winPrizeAwardTotalMoney - totalConsumptionMoney;
            }

            return string.Format(Info.LotteryMoneyConutMessage, profitText + " " 
                + Info.GetNumberMaxUnit(profitTotalMoney), DoubleBallView.FormatNumber(profitTotalMoney.ToString(), 5));
        }

        /// <summary>
        /// 获取做公益的金额
        /// </summary>
        public decimal GetPublicBenefitMoney()
        {
            decimal publicBenefitPercentum = new decimal(0.36);
            decimal publicBenefitMoney = new decimal(totalConsumptionMoney);
            return Math.Round(publicBenefitPercentum * publicBenefitMoney, 2);
        }

        /// <summary>
        /// 获取综合中奖率[参数小数保留位数]
        /// </summary>
        public decimal GetLotteryProbability(int decimals)
        {
            decimal buyTotalZhuDecimal = new decimal(buyTotalZhu);
            decimal winPrizeLotteryTotalZhuDecimal = new decimal(winPrizeAwardTotalZhu);

            return Math.Round(winPrizeLotteryTotalZhuDecimal / buyTotalZhuDecimal * 100, decimals);
        }
    }
}
