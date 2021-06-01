using DoubleBalls.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleBalls.Model
{
    /// <summary>
    /// 双色球胆拖号类
    /// </summary>
    public class DantuoDoubleBallNumber : DoubleBall
    {
        /// <summary>
        /// 双色球拖号码
        /// </summary>
        public int[] redBallTuos { get; set; }
        /// <summary>
        /// 双色球蓝色球号码
        /// </summary>
        public int[] blueBalls { get; set; }
        /// <summary>
        /// 红色球胆号的数量
        /// </summary>
        public int redBallDanCount { get; set; }
        /// <summary>
        /// 红色球拖号的数量
        /// </summary>
        public int redBallTuoCount { get; set; }
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
         * 胆拖
         *  红球胆最大球数 5
         *  红球拖最大球数 20
         *  篮球最大球数 16
         */
        public DantuoDoubleBallNumber() : base(AwardType.Null) => InitDantuoDoubleBall();

        /// <summary>
        /// 初始化胆拖双色球信息
        /// </summary>
        private void InitDantuoDoubleBall()
        {
            redBalls = new int[5];
            redBallTuos = new int[20];
            blueBalls = new int[16];

            for (int i = 0; i < redBalls.Length; i++)
                redBalls[i] = _INITVALUE;

            for (int i = 0; i < redBallTuos.Length; i++)
                redBallTuos[i] = _INITVALUE;

            for (int i = 0; i < blueBalls.Length; i++)
                blueBalls[i] = _INITVALUE;

            redWinPrizes = new int[6];
        }

        /// <summary>
        /// 排序红球胆、拖和蓝球
        /// </summary>
        public void OrderDoubleBalls()
        {
            Array.Sort(redBalls);
            Array.Sort(redBallTuos);
            Array.Sort(blueBalls);
        }

        /// <summary>
        /// 向复式红色球中添加一个胆球
        /// </summary>
        /// <param name="redNumber"></param>
        public void AddRedBallDan(int redNumber)
        {
            for (int i = 0; i < redBalls.Length; i++)
            {
                if (redBalls[i] == _INITVALUE)
                {
                    redBallDanCount++;
                    redBalls[i] = redNumber;
                    break;
                }
            }
        }

        /// <summary>
        /// 向复式红色球中添加一个拖球
        /// </summary>
        /// <param name="redNumber"></param>
        public void AddRedBallTuo(int redNumber)
        {
            for (int i = 0; i < redBallTuos.Length; i++)
            {
                if (redBallTuos[i] == _INITVALUE)
                {
                    redBallTuoCount++;
                    redBallTuos[i] = redNumber;
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
        /// 移除胆拖中的一个红色胆球
        /// </summary>
        /// <param name="redNumber"></param>
        public void RemoveRedBallDan(int redNumber)
        {
            int removeIndex = 0;

            for (int i = 0; i < redBallDanCount; i++)
            {
                if (redBalls[i] == redNumber)
                {
                    removeIndex = i;
                    break;
                }
            }

            for (int i = removeIndex + 1; i < redBallDanCount; i++)
                redBalls[i - 1] = redBalls[i];

            redBallDanCount--;
            redBalls[redBallDanCount] = _INITVALUE;
        }

        /// <summary>
        /// 移除胆拖中的一个红色拖球
        /// </summary>
        /// <param name="redNumber"></param>
        public void RemoveRedBallTuo(int redNumber)
        {
            int removeIndex = 0;

            for (int i = 0; i < redBallTuoCount; i++)
            {
                if (redBallTuos[i] == redNumber)
                {
                    removeIndex = i;
                    break;
                }
            }

            for (int i = removeIndex + 1; i < redBallTuoCount; i++)
                redBallTuos[i - 1] = redBallTuos[i];

            redBallTuoCount--;
            redBallTuos[redBallTuoCount] = _INITVALUE;
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
        /// 重置红色胆拖球和蓝色球
        /// </summary>
        public void ResetComplexNumber()
        {
            for (int i = 0; i < redBalls.Length; i++)
                redBalls[i] = _INITVALUE;

            for (int i = 0; i < redBallTuos.Length; i++)
                redBallTuos[i] = _INITVALUE;

            for (int i = 0; i < blueBalls.Length; i++)
                blueBalls[i] = _INITVALUE;

            redBallDanCount = 0;
            redBallTuoCount = 0;
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
        /// 设置该胆拖号码最高奖的单式号码
        /// </summary>
        public void SettingMaxDoubleBallNumber()
        {
            if (IsSimplex())
            {
                int useIndex = 0;
                for (int i = 0; i < redBallDanCount; i++)
                    redWinPrizes[useIndex++] = redBalls[i];

                int surplusCount = 6 - redBallDanCount;
                for (int i = 0; i < surplusCount; i++)
                    redWinPrizes[useIndex++] = redBallTuos[i];

                blueWinPrize = blueBalls[0];
            }
            else
            {
                List<int> mergeRedBalls = new List<int>();

                for (int i = 0; i < redBallDanCount; i++)
                {
                    if (!redWinPrizes.Contains(redBalls[i])) mergeRedBalls.Add(redBalls[i]);
                }

                for (int i = 0; i < redBallTuoCount; i++)
                {
                    if (!redWinPrizes.Contains(redBallTuos[i])) mergeRedBalls.Add(redBallTuos[i]);
                }

                int mergeRedIndex = 0;

                for (int i = 0; i < redWinPrizes.Length; i++)
                {
                    if (redWinPrizes[i] == 0)
                    {
                        redWinPrizes[i] = mergeRedBalls[mergeRedIndex++];
                    }
                }

                if (blueWinPrize == 0) blueWinPrize = blueBalls[0];
            }

            Array.Sort(redWinPrizes);
        }

        /// <summary>
        /// 是否是单注号码
        /// </summary>
        public bool IsSimplex() => (redBallDanCount + redBallTuoCount) == 6 && blueBallCount == 1;

        /// <summary>
        /// 判断自选双色球号码是否已经成号
        /// </summary>
        public override bool IsOKNumber()
        {
            return redBallDanCount >= 1 && (redBallDanCount + redBallTuoCount) >= 6 && blueBallCount >= 1;
        }

        /// <summary>
        /// 获取双色球号码有多少种组合
        /// </summary>
        public long GetDoubleBallCombination()
        {
            int pickCount = REDCOUNT - redBallDanCount;
            return Factorial(redBallTuoCount) / (Factorial(pickCount) * Factorial(redBallTuoCount - pickCount)) * blueBallCount;
        }

        /// <summary>
        /// 求一个数的阶乘
        /// </summary>
        public static long Factorial(int num)
        {
            long result = 1;
            
            for (int i = 1; i <= num; i++)
                result *= i;

            return result;
        }

        /// <summary>
        /// 该胆拖号是否是空号
        /// </summary>
        /// <returns></returns>
        public override bool isBallEmpty() => redBallDanCount == 0 && redBallTuoCount == 0 && blueBallCount == 0;

        /// <summary>
        /// 胆拖双色球的内容复制到单式双色球中
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
        /// 获取胆拖号未中奖的注数
        /// </summary>
        public override int GetNotLotteryTotalZhu()
        {
            string notCount = (GetDoubleBallCombination() - GetTotalWinningTotalZhu()).ToString();
            return int.Parse(notCount);
        }

        /// <summary>
        /// 获取胆拖号的中奖注数
        /// </summary>
        public override int GetTotalWinningTotalZhu()
        {
            return oneAwardTotalZhu + twoAwardTotalZhu + threeAwardTotalZhu + fourAwardTotalZhu + fiveAwardTotalZhu + sixAwardTotalZhu;
        }

        /// <summary>
        /// 获取复试号的中奖金额
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
        /// 设定胆拖号码指定奖的中奖总注
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
        /// 胆拖号重写父类比对双色球中奖方法
        /// </summary>
        /// <param name="dbt">双色球辅助类</param>
        /// <param name="sdbn">开奖的单式双色球号</param>
        public override void ComparisonDoubleBallNumber(DoubleBallTool dbt, SimplexDoubleBallNumber sdbn)
        {
            base.ComparisonDoubleBallNumber(dbt, sdbn);
            dbt.ComparisonDantuoDoubleBallNumber(this, sdbn);
        }
    }
}
