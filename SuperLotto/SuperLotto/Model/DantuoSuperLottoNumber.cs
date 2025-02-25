﻿using SuperLotto.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperLotto.Model
{
    /// <summary>
    /// 大乐透胆拖号类
    /// </summary>
    public class DantuoSuperLottoNumber : ComplexNumber
    {
        /// <summary>
        /// 大乐透红球拖号码
        /// </summary>
        public int[] redBallTuos { get; set; }
        /// <summary>
        /// 大乐透蓝球拖号码
        /// </summary>
        public int[] blueBallTuos { get; set; }
        /// <summary>
        /// 记录该号码最高奖红色球中奖号码
        /// </summary>
        public int[] maxRedWinPrizes { get; set; }
        /// <summary>
        /// 记录该号码最高奖蓝色球中奖号码
        /// </summary>
        public int[] maxBlueWinPrize { get; set; }
        /// <summary>
        /// 红色球胆号的数量
        /// </summary>
        public int redBallDanCount { get; set; }
        /// <summary>
        /// 红色球拖号的数量
        /// </summary>
        public int redBallTuoCount { get; set; }
        /// <summary>
        /// 蓝色球胆号的数量
        /// </summary>
        public int blueBallDanCount { get; set; }
        /// <summary>
        /// 蓝色球拖号得数量
        /// </summary>
        public int blueBallTuoCount { get; set; }

        /**
         * 胆拖
         *  红球胆最大球数 4
         *  红球拖最大球数 20
         *  篮球胆最大球数 1
         *  蓝球拖最大球数 11
         */
        public DantuoSuperLottoNumber() : base(AwardType.Null) => InitDantuoSuperLotto();

        /// <summary>
        /// 初始化胆拖大乐透信息
        /// </summary>
        private void InitDantuoSuperLotto()
        {
            redBalls = new int[4];
            redBallTuos = new int[20];
            blueBalls = new int[1];
            blueBallTuos = new int[11];

            for (int i = 0; i < redBalls.Length; i++)
                redBalls[i] = _INITVALUE;

            for (int i = 0; i < redBallTuos.Length; i++)
                redBallTuos[i] = _INITVALUE;

            for (int i = 0; i < blueBalls.Length; i++)
                blueBalls[i] = _INITVALUE;

            for (int i = 0; i < blueBallTuos.Length; i++)
                blueBallTuos[i] = _INITVALUE;

            maxRedWinPrizes = new int[5];
            maxBlueWinPrize = new int[2];
        }

        /// <summary>
        /// 排序红球胆、拖和蓝球
        /// </summary>
        public void OrderSuperLottos()
        {
            Array.Sort(redBalls);
            Array.Sort(redBallTuos);
            Array.Sort(blueBalls);
            Array.Sort(blueBallTuos);
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
        /// 向复式蓝色中添加一个胆球
        /// </summary>
        /// <param name="blueNumber"></param>
        public void AddBlueBallDan(int blueNumber)
        {
            for (int i = 0; i < blueBalls.Length; i++)
            {
                if (blueBalls[i] == _INITVALUE)
                {
                    blueBallDanCount++;
                    blueBalls[i] = blueNumber;
                    break;
                }
            }
        }

        /// <summary>
        /// 向复试蓝色球中添加一个拖球
        /// </summary>
        /// <param name="blueNumber"></param>
        public void AddBlueBallTuo(int blueNumber)
        {
            for (int i = 0; i < blueBallTuos.Length; i++)
            {
                if (blueBallTuos[i] == _INITVALUE)
                {
                    blueBallTuoCount++;
                    blueBallTuos[i] = blueNumber;
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
        /// 移除复式蓝色球中的一个胆球
        /// </summary>
        /// <param name="blueNumber"></param>
        public void RemoveBlueBallDan(int blueNumber)
        {
            int removeIndex = 0;

            for (int i = 0; i < blueBallDanCount; i++)
            {
                if (blueBalls[i] == blueNumber)
                {
                    removeIndex = i;
                    break;
                }
            }

            for (int i = removeIndex + 1; i < blueBallDanCount; i++)
                blueBalls[i - 1] = blueBalls[i];

            blueBallDanCount--;
            blueBalls[blueBallDanCount] = _INITVALUE;
        }

        /// <summary>
        /// 移除复试蓝色球中的一个拖球
        /// </summary>
        /// <param name="blueNumber"></param>
        public void RemoveBlueBallTuo(int blueNumber)
        {
            int removeIndex = 0;

            for (int i = 0; i < blueBallTuoCount; i++)
            {
                if (blueBallTuos[i] == blueNumber)
                {
                    removeIndex = i;
                    break;
                }
            }

            for (int i = removeIndex + 1; i < blueBallTuoCount; i++)
                blueBallTuos[i - 1] = blueBallTuos[i];

            blueBallTuoCount--;
            blueBallTuos[blueBallTuoCount] = _INITVALUE;
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

            for (int i = 0; i < blueBallTuos.Length; i++)
                blueBallTuos[i] = _INITVALUE;

            redBallDanCount = 0;
            redBallTuoCount = 0;
            blueBallDanCount = 0;
            blueBallTuoCount = 0;
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
        /// 设置该胆拖号码最高奖的单式号码
        /// </summary>
        public void SettingMaxSuperLottoNumber()
        {
            if (IsSimplex())
            {
                //设置红球
                int useRedIndex = 0;
                for (int i = 0; i < redBallDanCount; i++)
                    maxRedWinPrizes[useRedIndex++] = redBalls[i];

                int redSurplusCount = 5 - redBallDanCount;
                for (int i = 0; i < redSurplusCount; i++)
                    maxRedWinPrizes[useRedIndex++] = redBallTuos[i];

                //设置篮球
                int useBlueIndex = 0;
                for (int i = 0; i < blueBallDanCount; i++)
                    maxBlueWinPrize[useBlueIndex++] = blueBalls[i];

                int blueSurplusCount = 2 - blueBallDanCount;
                for (int i = 0; i < blueSurplusCount; i++)
                    maxBlueWinPrize[useBlueIndex++] = blueBallTuos[i];
            }
            else
            {
                SetMaxWinPrizesNumber(maxRedWinPrizes, redBalls);
                SetMaxWinPrizesNumber(maxRedWinPrizes, redBallTuos);
                SetMaxWinPrizesNumber(maxBlueWinPrize, blueBalls);
                SetMaxWinPrizesNumber(maxBlueWinPrize, blueBallTuos);
            }

            Array.Sort(maxRedWinPrizes);
        }

        private void SetMaxWinPrizesNumber(int[] maxWinPrizes, int[] ballValues)
        {
            for (int i = 0; i < maxWinPrizes.Length; i++)
            {
                if (maxWinPrizes[i] != 0) continue;
                for (int j = 0; j < ballValues.Length; j++)
                    if (!maxWinPrizes.Contains(ballValues[j]) && ballValues[j] != _INITVALUE)
                    {
                        maxWinPrizes[i] = ballValues[j];
                        break;
                    }
            }

            Array.Sort(maxWinPrizes);
        }

        /// <summary>
        /// 是否是单注号码
        /// </summary>
        public override bool IsSimplex() => (redBallDanCount + redBallTuoCount) == 5 && (blueBallDanCount + blueBallTuoCount) == 2;

        /// <summary>
        /// 判断自选大乐透号码是否已经成号
        /// </summary>
        public override bool IsOKNumber()
        {
            return /*redBallDanCount >= 1 && */(redBallDanCount + redBallTuoCount) >= 5 && /*blueBallDanCount >= 1 && */(blueBallDanCount + blueBallTuoCount) >= 2;
        }

        /// <summary>
        /// 获取大乐透号码有多少种组合
        /// </summary>
        public long GetSuperLottoCombination()
        {
            return SuperLottoTool
                .Combo(redBallTuoCount, 5 - redBallDanCount) * SuperLottoTool.Combo(blueBallTuoCount, 2 - blueBallDanCount);
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
        public override bool isBallEmpty() => redBallDanCount == 0 && redBallTuoCount == 0 && blueBallDanCount == 0 && blueBallTuoCount == 0;

        /// <summary>
        /// 复制左边大乐透的最高奖到右边对象中
        /// </summary>
        /// <param name="sdbn">单式大乐透类</param>
        public override void CopyLeftToRightDataOfMaxAward(SimplexSuperLottoNumber sdbn)
        {
            SettingMaxSuperLottoNumber();

            sdbn.awardType = awardType;
            sdbn.blueBalls = maxBlueWinPrize;
            maxRedWinPrizes.CopyTo(sdbn.redBalls, 0);
        }

        /// <summary>
        /// 根据奖的等级复制左边大乐透号码到右边对象
        /// </summary>
        /// <param name="sdbn"></param>
        /// <param name="awardType"></param>
        public override void CopyLeftToRightBy(SimplexSuperLottoNumber sdbn, AwardType awardType)
        {
            //TODO: 该方法是否可实现有待探讨暂时先留着，暂用 CopyLeftToRightDataOfMaxAward 方法
            this.CopyLeftToRightDataOfMaxAward(sdbn);
        }

        /// <summary>
        /// 获取胆拖号未中奖的注数
        /// </summary>
        public override int GetNotLotteryTotalZhu()
        {
            string notCount = (GetSuperLottoCombination() - GetTotalWinningTotalZhu()).ToString();
            return int.Parse(notCount);
        }

        /// <summary>
        /// 获取胆拖号的中奖注数
        /// </summary>
        public override int GetTotalWinningTotalZhu()
        {
            return oneAwardTotalZhu + twoAwardTotalZhu + threeAwardTotalZhu + fourAwardTotalZhu
                + fiveAwardTotalZhu + sixAwardTotalZhu + sevenAwardTotalZhu + eightAwardTotalZhu + nineAwardTotalZhu;
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
                sixAwardTotalZhu * (long)AwardType.SixAward +
                sevenAwardTotalZhu * (long)AwardType.SevenAward +
                eightAwardTotalZhu * (long)AwardType.EightAward +
                nineAwardTotalZhu * (long)AwardType.NineAward;

            long twoAward = (long)(Config.Setting.GetTwoAward() * 0.8) * twoAwardTotalZhu;
            long oneAward = (long)(Config.Setting.GetOneAward() * 0.8) * oneAwardTotalZhu;

            return (oneAward + twoAward + otherAward) * multiple;
        }

        /// <summary>
        /// 设定胆拖号码指定奖的中奖总注
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
        /// 胆拖号重写父类比对大乐透中奖方法
        /// </summary>
        /// <param name="dbt">大乐透辅助类</param>
        /// <param name="sdbn">开奖的单式大乐透号</param>
        public override void ComparisonSuperLottoNumber(SuperLottoTool dbt, SimplexSuperLottoNumber sdbn)
        {
            base.ComparisonSuperLottoNumber(dbt, sdbn);
            dbt.ComparisonDantuoSuperLottoNumber(this, sdbn);
        }
    }
}
