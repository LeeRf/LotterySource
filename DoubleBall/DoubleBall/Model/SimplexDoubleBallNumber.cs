using DoubleBalls.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleBalls.Model
{
    /// <summary>
    /// 单式双色球号码类
    /// </summary>
    public class SimplexDoubleBallNumber : DoubleBall
    {
        /// <summary>
        /// 双色球蓝色球号码
        /// </summary>
        public int blueBall { get; set; }
        /// <summary>
        /// 用于排序的篮球号码
        /// </summary>
        public int OrderByBlues { get; set; }
        /// <summary>
        /// 用于排序的红球号码
        /// </summary>
        public long OrderByReds { get; set; }
        /// <summary>
        /// 记录该单式号码的期数
        /// </summary>
        public long Periods { get; set; }

        public SimplexDoubleBallNumber() : base(AwardType.Null) => redBalls = new int[6];

        public SimplexDoubleBallNumber(AwardType awardType) : base(awardType) => redBalls = new int[6];

        /// <summary>
        /// 获取单式号的中奖注数
        /// </summary>
        public override int GetTotalWinningTotalZhu() => base.isEmptyAward() ? 0 : 1;

        /// <summary>
        /// 获取单式号未中奖的注数
        /// </summary>
        public override int GetNotLotteryTotalZhu() => base.isEmptyAward() ? 1 : 0;

        /// <summary>
        /// 设定单式号码指定奖的中奖总注
        /// </summary>
        public override void SettingAwardTypeTotalZhu(LoopDataSummarizing loopData)
        {
            if (awardType == AwardType.SixAward)
            {
                loopData.SixPrizeCount++;
            }
            else if (awardType == AwardType.FiveAward)
            {
                loopData.FivePrizeCount++;
            }
            else if (awardType == AwardType.FourAward)
            {
                loopData.FourPrizeCount++;
            }
            else if (awardType == AwardType.ThreeAward)
            {
                loopData.ThreePrizeCount++;
            }
            else if (awardType == AwardType.TwoAward)
            {
                loopData.TwoPrizeCount++;
            }
            else if (awardType == AwardType.OneAward)
            {
                loopData.OnePrizeCount++;
            }
        }

        /// <summary>
        /// 获取单式号的中奖金额
        /// </summary>
        /// <param name="multiple">追加倍数</param>
        public override long GetTotalWinningMoney(int multiple)
        {
            if (base.isEmptyAward()) return 0;

            if (awardType != AwardType.OneAward && awardType != AwardType.TwoAward)
            {
                return (int)awardType * multiple;
            }
            else
            {
                if (awardType == AwardType.OneAward)
                {
                    return (long)(Config.Setting.GetOneAward() * 0.8) * multiple;
                }
                else if(awardType == AwardType.TwoAward)
                {
                    return (long)(Config.Setting.GetTwoAward() * 0.8) * multiple;
                }
            }

            return 0;
        }

        /// <summary>
        /// 将左边的双色球的内容复制到右边的双色球中
        /// </summary>
        /// <param name="sdbn">单式双色球类</param>
        public override void CopyLeftToRightDataValue(SimplexDoubleBallNumber sdbn)
        {
            sdbn.awardType = awardType;
            sdbn.blueBall = blueBall;
            redBalls.CopyTo(sdbn.redBalls, 0);
        }

        /// <summary>
        /// 单式重写父类比对双色球中奖方法
        /// </summary>
        /// <param name="dbt">双色球辅助类</param>
        /// <param name="sdbn">开奖的单式双色球号</param>
        public override void ComparisonDoubleBallNumber(DoubleBallTool dbt, SimplexDoubleBallNumber sdbn)
        {
            base.ComparisonDoubleBallNumber(dbt, sdbn);
            dbt.ComparisonSimplexDoubleBallNumber(this, sdbn);
        }
    }
}
