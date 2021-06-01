using SuperLotto.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperLotto.Model
{
    /// <summary>
    /// 单式大乐透号码类
    /// </summary>
    public class SimplexSuperLottoNumber : SuperLottos
    {
        /// <summary>
        /// 用于排序的篮球号码
        /// </summary>
        public long OrderByBlues { get; set; }
        /// <summary>
        /// 用于排序的红球号码
        /// </summary>
        public long OrderByReds { get; set; }
        /// <summary>
        /// 记录该单式号码的期数
        /// </summary>
        public long Periods { get; set; }

        public SimplexSuperLottoNumber() : base(AwardType.Null) => InitSimplexSuperLotto();

        public SimplexSuperLottoNumber(AwardType awardType) : base(awardType) => InitSimplexSuperLotto();

        private void InitSimplexSuperLotto()
        {
            redBalls = new int[5];
            blueBalls = new int[2];
        }

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
            if (awardType == AwardType.NineAward)
            {
                loopData.NinePrizeCount++;
            }
            else if (awardType == AwardType.EightAward)
            {
                loopData.EightPrizeCount++;
            }
            else if (awardType == AwardType.SevenAward)
            {
                loopData.SevenPrizeCount++;
            }
            else if (awardType == AwardType.SixAward)
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
        /// 将左边的大乐透的内容复制到右边的大乐透中
        /// </summary>
        /// <param name="sdbn">单式大乐透类</param>
        public override void CopyLeftToRightDataValue(SimplexSuperLottoNumber sdbn)
        {
            sdbn.awardType = awardType;
            redBalls.CopyTo(sdbn.redBalls, 0);
            blueBalls.CopyTo(sdbn.blueBalls, 0);
        }

        /// <summary>
        /// 单式重写父类比对大乐透中奖方法
        /// </summary>
        /// <param name="dbt">大乐透辅助类</param>
        /// <param name="sdbn">开奖的单式大乐透号</param>
        public override void ComparisonSuperLottoNumber(SuperLottoTool dbt, SimplexSuperLottoNumber sdbn)
        {
            base.ComparisonSuperLottoNumber(dbt, sdbn);
            dbt.ComparisonSimplexSuperLottoNumber(this, sdbn);
        }
    }
}
