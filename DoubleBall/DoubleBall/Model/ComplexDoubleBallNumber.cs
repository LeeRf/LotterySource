using DoubleBalls.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleBalls.Model
{
    /// <summary>
    /// 复式兼单式双色球号码类
    /// </summary>
    public class ComplexDoubleBallNumber : DoubleBall
    {
        /// <summary>
        /// 复式蓝色球
        /// </summary>
        public int[] blueBalls { get; set; }
        /// <summary>
        /// 红色球的数量
        /// </summary>
        public int redBallCount { get; set; }
        /// <summary>
        /// 蓝色球的数量
        /// </summary>
        public int blueBallCount { get; set; }
        
        /// <summary>
        /// 初始值(用于排序不至于把0值排序到最前面)
        /// </summary>
        public int _INITVALUE { get; } = 50;

        /// <summary>
        /// 一等奖中奖注数
        /// </summary>
        public int oneAwardTotalZhu { get; set; }
        /// <summary>
        /// 二等奖中奖注数
        /// </summary>
        public int twoAwardTotalZhu { get; set; }
        /// <summary>
        /// 三等奖中奖注数
        /// </summary>
        public int threeAwardTotalZhu { get; set; }
        /// <summary>
        /// 四等奖中奖注数
        /// </summary>
        public int fourAwardTotalZhu { get; set; }
        /// <summary>
        /// 五等奖中奖注数
        /// </summary>
        public int fiveAwardTotalZhu { get; set; }
        /// <summary>
        /// 六等奖中奖注数
        /// </summary>
        public int sixAwardTotalZhu { get; set; }
        /// <summary>
        /// 记录该号码最高奖红色球中奖号码
        /// </summary>
        public int[] redWinPrizes { get; set; }
        /// <summary>
        /// 记录该号码最高奖蓝色球中奖号码
        /// </summary>
        public int blueWinPrize { get; set; }

        /**
         * 复式
         *  红球最大球数 20
         *  篮球最大球数 16
         */
        public ComplexDoubleBallNumber() : base(AwardType.Null) => InitComplexDoubleBall();

        /// <summary>
        /// 初始化复式双色球信息
        /// </summary>
        private void InitComplexDoubleBall()
        {
            redBalls = new int[20];
            blueBalls = new int[16];

            for (int i = 0; i < redBalls.Length; i++)
                redBalls[i] = _INITVALUE;

            for (int i = 0; i < blueBalls.Length; i++)
                blueBalls[i] = _INITVALUE;

            redWinPrizes = new int[6];
        }

        /// <summary>
        /// 排序红球和蓝球
        /// </summary>
        public void OrderDoubleBalls()
        {
            Array.Sort(redBalls);
            Array.Sort(blueBalls);
        }

        /// <summary>
        /// 向复式红色球中添加一个球
        /// </summary>
        /// <param name="redNumber"></param>
        public void AddRedBall(int redNumber)
        {
            for (int i = 0; i < redBalls.Length; i++)
            {
                if (redBalls[i] == _INITVALUE)
                {
                    redBallCount++;
                    redBalls[i] = redNumber;
                    break;
                }
            }
        }

        /// <summary>
        /// 向复式蓝色球中添加一个球
        /// </summary>
        /// <param name="blueNumber"></param>
        public void AddBlueBall(int blueNumber)
        {
            for (int i = 0; i < blueBalls.Length; i++)
            {
                if (blueBalls[i] == _INITVALUE)
                {
                    blueBallCount++;
                    blueBalls[i] = blueNumber;
                    break;
                }
            }
        }

        /// <summary>
        /// 移除复式红色球中的一个球
        /// </summary>
        /// <param name="redNumber"></param>
        public void RemoveRedBall(int redNumber)
        {
            int removeIndex = 0;

            for (int i = 0; i < redBallCount; i++)
            {
                if (redBalls[i] == redNumber)
                {
                    removeIndex = i;
                    break;
                }
            }

            for (int i = removeIndex + 1; i < redBallCount; i++)
                redBalls[i - 1] = redBalls[i];

            redBallCount--;
            redBalls[redBallCount] = _INITVALUE;
        }

        /// <summary>
        /// 移除复式蓝色球中的一个球
        /// </summary>
        /// <param name="blueNumber"></param>
        public void RemoveBlueBall(int blueNumber)
        {
            int removeIndex = 0;

            for (int i = 0; i < blueBallCount; i++)
            {
                if (blueBalls[i] == blueNumber)
                {
                    removeIndex = i;
                    break;
                }
            }

            for (int i = removeIndex + 1; i < blueBallCount; i++)
                blueBalls[i - 1] = blueBalls[i];

            blueBallCount--;
            blueBalls[blueBallCount] = _INITVALUE;
        }

        /// <summary>
        /// 重置红色球和蓝色球
        /// </summary>
        public void ResetComplexNumber()
        {
            for (int i = 0; i < redBalls.Length; i++)
                redBalls[i] = _INITVALUE;

            for (int i = 0; i < blueBalls.Length; i++)
                blueBalls[i] = _INITVALUE;

            redBallCount = 0;
            blueBallCount = 0;
        }

        /// <summary>
        /// 重置该注中奖的信息
        /// </summary>
        public void ResetTotalZhuToDefault()
        {
            oneAwardTotalZhu = 0;
            twoAwardTotalZhu = 0;
            threeAwardTotalZhu = 0;
            fourAwardTotalZhu = 0;
            fiveAwardTotalZhu = 0;
            sixAwardTotalZhu = 0;

            for (int i = 0; i < redWinPrizes.Length; i++)
                redWinPrizes[i] = 0;

            blueWinPrize = 0;
            awardType = AwardType.Null;
        }

        /// <summary>
        /// 设置该复式号码最高奖的单式号码
        /// </summary>
        public void SettingMaxDoubleBallNumber()
        {
            if (IsSimplex())
            {
                for (int i = 0; i < redBallCount; i++)
                    redWinPrizes[i] = redBalls[i];

                blueWinPrize = blueBalls[0];
            }
            else
            {
                for (int i = 0; i < redWinPrizes.Length; i++)
                {
                    if (redWinPrizes[i] == 0)
                    {
                        for (int j = 0; j < redBallCount; j++)
                            if (!redWinPrizes.Contains(redBalls[j]))
                            {
                                redWinPrizes[i] = redBalls[j];
                                break;
                            }
                    }
                }

                Array.Sort(redWinPrizes);
                if (blueWinPrize == 0) blueWinPrize = blueBalls[0];
            }
        }

        /// <summary>
        /// 是否是单注号码
        /// </summary>
        public bool IsSimplex() => redBallCount == 6 && blueBallCount == 1;

        /// <summary>
        /// 判断自选双色球号码是否已经成号
        /// </summary>
        /// <returns></returns>
        public override bool IsOKNumber() => redBallCount >= 6 && blueBallCount >= 1;

        /// <summary>
        /// 获取双色球号码有多少种组合
        /// </summary>
        public int GetDoubleBallCombination()
        {
            return (redBallCount) * (redBallCount - 1) * (redBallCount - 2) * (redBallCount - 3) * (redBallCount - 4) * (redBallCount - 5)
                / (1 * 2 * 3 * 4 * 5 * 6) * blueBallCount;
        }

        public override bool isBallEmpty() => redBallCount == 0 && blueBallCount == 0;

        /// <summary>
        /// 获取复式号的中奖注数
        /// </summary>
        public override int GetTotalWinningTotalZhu()
        {
            return oneAwardTotalZhu + twoAwardTotalZhu + threeAwardTotalZhu + fourAwardTotalZhu + fiveAwardTotalZhu + sixAwardTotalZhu;
        }

        /// <summary>
        /// 获取复式号未中奖的注数
        /// </summary>
        public override int GetNotLotteryTotalZhu() => GetDoubleBallCombination() - GetTotalWinningTotalZhu();

        /// <summary>
        /// 设定复式号码指定奖的中奖总注
        /// </summary>
        public override void SettingAwardTypeTotalZhu(LoopDataSummarizing loopData)
        {
            if (awardType >= AwardType.SixAward)
            {
                loopData.SixPrizeCount += sixAwardTotalZhu;
            }
            if (awardType >= AwardType.FiveAward)
            {
                loopData.FivePrizeCount += fiveAwardTotalZhu;
            }
            if (awardType >= AwardType.FourAward)
            {
                loopData.FourPrizeCount += fourAwardTotalZhu;
            }
            if (awardType >= AwardType.ThreeAward)
            {
                loopData.ThreePrizeCount += threeAwardTotalZhu;
            }
            if (awardType >= AwardType.TwoAward)
            {
                loopData.TwoPrizeCount += twoAwardTotalZhu;
            }
            if (awardType == AwardType.OneAward)
            {
                loopData.OnePrizeCount += oneAwardTotalZhu;
            }
        }

        /// <summary>
        /// 获取复式号的中奖金额
        /// </summary>
        /// <param name="multiple">追加倍数</param>
        public override long GetTotalWinningMoney(int multiple)
        {
            long otherAward = 
                threeAwardTotalZhu * (long)AwardType.ThreeAward + 
                fourAwardTotalZhu * (long)AwardType.FourAward + 
                fiveAwardTotalZhu * (long)AwardType.FiveAward + 
                sixAwardTotalZhu * (long)AwardType.SixAward;

            long twoWward = (long)(Config.Setting.GetTwoAward() * 0.8) * twoAwardTotalZhu;
            long oneAward = (long)(Config.Setting.GetOneAward() * 0.8) * oneAwardTotalZhu;

            return (oneAward + twoWward + otherAward) * multiple;
        }

        /// <summary>
        /// 复式双色球的内容到单式双色球中
        /// </summary>
        /// <param name="sdbn">单式双色球类</param>
        public override void CopyLeftToRightDataValue(SimplexDoubleBallNumber sdbn)
        {
            SettingMaxDoubleBallNumber();

            sdbn.awardType = awardType;
            sdbn.blueBall = blueWinPrize;
            redWinPrizes.CopyTo(sdbn.redBalls, 0);
        }

        /// <summary>
        /// 复式重写父类比对双色球中奖方法
        /// </summary>
        /// <param name="dbt">双色球辅助类</param>
        /// <param name="sdbn">开奖的单式双色球号</param>
        public override void ComparisonDoubleBallNumber(DoubleBallTool dbt, SimplexDoubleBallNumber sdbn)
        {
            base.ComparisonDoubleBallNumber(dbt, sdbn);
            dbt.ComparisonComplexDoubleBallNumber(this, sdbn);
        }
    }
}
