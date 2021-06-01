using SuperLotto.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperLotto.Model
{
    /// <summary>
    /// 复式兼单式大乐透号码类
    /// </summary>
    public class ComplexSuperLottoNumber : SuperLottos
    {
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
        /// 七等奖中奖注数
        /// </summary>
        public int sevenAwardTotalZhu { get; set; }
        /// <summary>
        /// 八等奖中奖注数
        /// </summary>
        public int eightAwardTotalZhu { get; set; }
        /// <summary>
        /// 九等奖中奖注数
        /// </summary>
        public int nineAwardTotalZhu { get; set; }
        /// <summary>
        /// 记录该号码最高奖红色球中奖号码
        /// </summary>
        public int[] maxRedWinPrizes { get; set; }
        /// <summary>
        /// 记录该号码最高奖蓝色球中奖号码
        /// </summary>
        public int[] maxBlueWinPrize { get; set; }

        /**
         * 复式
         *  红球最大球数 18
         *  篮球最大球数 12
         */
        public ComplexSuperLottoNumber() : base(AwardType.Null) => InitComplexSuperLotto();

        /// <summary>
        /// 初始化复式大乐透信息
        /// </summary>
        private void InitComplexSuperLotto()
        {
            redBalls = new int[18];
            blueBalls = new int[12];

            for (int i = 0; i < redBalls.Length; i++)
                redBalls[i] = _INITVALUE;

            for (int i = 0; i < blueBalls.Length; i++)
                blueBalls[i] = _INITVALUE;

            maxRedWinPrizes = new int[5];
            maxBlueWinPrize = new int[2];
        }

        /// <summary>
        /// 排序红球和蓝球
        /// </summary>
        public void OrderSuperLottos()
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
            sevenAwardTotalZhu = 0;
            eightAwardTotalZhu = 0;
            nineAwardTotalZhu = 0;

            for (int i = 0; i < maxRedWinPrizes.Length; i++)
                maxRedWinPrizes[i] = 0;

            maxBlueWinPrize = new int[2];
            awardType = AwardType.Null;
        }

        /// <summary>
        /// 设置该复式号码最高奖的单式号码
        /// </summary>
        public void SettingMaxSuperLottoNumber()
        {
            if (IsSimplex())
            {
                for (int i = 0; i < redBallCount; i++)
                    maxRedWinPrizes[i] = redBalls[i];

                //TODO: 暂时这么写、记得改 原版 maxBlueWinPrize = blueBalls[0];
                blueBalls.CopyTo(maxBlueWinPrize, 0);
            }
            else
            {
                SetMaxWinPrizesNumber(maxRedWinPrizes, redBalls);
                SetMaxWinPrizesNumber(maxBlueWinPrize, blueBalls);
            }
        }

        private void SetMaxWinPrizesNumber(int[] maxBalls, int[] ballsType)
        {
            for (int i = 0; i < maxBalls.Length; i++)
            {
                if (maxBalls[i] == 0)
                {
                    for (int j = 0; j < ballsType.Length; j++)
                        if (!maxBalls.Contains(ballsType[j]))
                        {
                            maxBalls[i] = ballsType[j];
                            break;
                        }
                }
            }

            Array.Sort(maxBalls);
        }

        /// <summary>
        /// 是否是单注号码
        /// </summary>
        public bool IsSimplex() => redBallCount == 5 && blueBallCount == 2;

        /// <summary>
        /// 判断自选大乐透号码是否已经成号
        /// </summary>
        /// <returns></returns>
        public override bool IsOKNumber() => redBallCount >= 5 && blueBallCount >= 2;

        /// <summary>
        /// 获取大乐透号码有多少种组合
        /// </summary>
        public int GetSuperLottoCombination()
        {
            return redBallCount * (redBallCount - 1) * (redBallCount - 2) * (redBallCount - 3) * (redBallCount - 4)
                / (1 * 2 * 3 * 4 * 5) * (blueBallCount * (blueBallCount - 1) / (1 * 2));
        }

        public override bool isBallEmpty() => redBallCount == 0 && blueBallCount == 0;

        /// <summary>
        /// 获取复式号的中奖注数
        /// </summary>
        public override int GetTotalWinningTotalZhu()
        {
            return oneAwardTotalZhu + twoAwardTotalZhu + threeAwardTotalZhu + fourAwardTotalZhu
                + fiveAwardTotalZhu + sixAwardTotalZhu + sevenAwardTotalZhu + eightAwardTotalZhu + nineAwardTotalZhu;
        }

        /// <summary>
        /// 获取复式号未中奖的注数
        /// </summary>
        public override int GetNotLotteryTotalZhu() => GetSuperLottoCombination() - GetTotalWinningTotalZhu();

        /// <summary>
        /// 设定复式号码指定奖的中奖总注
        /// </summary>
        public override void SettingAwardTypeTotalZhu(LoopDataSummarizing _loopData)
        {
            if (awardType >= AwardType.NineAward)
            {
                _loopData.NinePrizeCount += nineAwardTotalZhu;
            }
            if (awardType >= AwardType.EightAward)
            {
                _loopData.EightPrizeCount += eightAwardTotalZhu;
            }
            if (awardType >= AwardType.SevenAward)
            {
                _loopData.SevenPrizeCount += sevenAwardTotalZhu;
            }
            if (awardType >= AwardType.SixAward)
            {
                _loopData.SixPrizeCount += sixAwardTotalZhu;
            }
            if (awardType >= AwardType.FiveAward)
            {
                _loopData.FivePrizeCount += fiveAwardTotalZhu;
            }
            if (awardType >= AwardType.FourAward)
            {
                _loopData.FourPrizeCount += fourAwardTotalZhu;
            }
            if (awardType >= AwardType.ThreeAward)
            {
                _loopData.ThreePrizeCount += threeAwardTotalZhu;
            }
            if (awardType >= AwardType.TwoAward)
            {
                _loopData.TwoPrizeCount += twoAwardTotalZhu;
            }
            if (awardType == AwardType.OneAward)
            {
                _loopData.OnePrizeCount += oneAwardTotalZhu;
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
                sixAwardTotalZhu * (long)AwardType.SixAward +
                sevenAwardTotalZhu * (long)AwardType.SevenAward +
                eightAwardTotalZhu * (long)AwardType.EightAward +
                nineAwardTotalZhu * (long)AwardType.NineAward;

            long twoWward = (long)(Config.Setting.GetTwoAward() * 0.8) * twoAwardTotalZhu;
            long oneAward = (long)(Config.Setting.GetOneAward() * 0.8) * oneAwardTotalZhu;

            return (oneAward + twoWward + otherAward) * multiple;
        }

        /// <summary>
        /// 复式大乐透的内容到单式大乐透中
        /// </summary>
        /// <param name="sdbn">单式大乐透类</param>
        public override void CopyLeftToRightDataValue(SimplexSuperLottoNumber sdbn)
        {
            SettingMaxSuperLottoNumber();

            sdbn.awardType = awardType;
            maxRedWinPrizes.CopyTo(sdbn.redBalls, 0);
            maxBlueWinPrize.CopyTo(sdbn.blueBalls, 0);
        }

        /// <summary>
        /// 复式重写父类比对大乐透中奖方法
        /// </summary>
        /// <param name="dbt">大乐透辅助类</param>
        /// <param name="sdbn">开奖的单式大乐透号</param>
        public override void ComparisonSuperLottoNumber(SuperLottoTool dbt, SimplexSuperLottoNumber sdbn)
        {
            base.ComparisonSuperLottoNumber(dbt, sdbn);
            dbt.ComparisonComplexSuperLottoNumber(this, sdbn);
        }
    }
}
