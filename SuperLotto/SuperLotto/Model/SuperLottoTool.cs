using SuperLotto.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperLotto.Model
{
    /// <summary>
    /// 大乐透类
    ///   Athor：Lee
    ///   编写于 2020/12/29
    /// </summary>
    public class SuperLottoTool
    {
        /// <summary>
        /// 存储红球常量开到移除
        /// </summary>
        private List<int> _redsBallList = new List<int>();
        /// <summary>
        /// 存储红球拖常量开到移除
        /// </summary>
        private List<int> _redBallTuoList = new List<int>();

        //存储篮球常量开到移除
        private List<int> _bluesBallList = new List<int>();

        //红球常量
        private readonly int[] _reds = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16,
            17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35 };
        //蓝球常量
        private readonly int[] _blues = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

        public SuperLottoTool()
        {
            for (int i = 0; i < _reds.Length; i++)
                _redsBallList.Add(_reds[i]);

            for (int i = 0; i < _reds.Length; i++)
                _redBallTuoList.Add(_reds[i]);

            for (int i = 0; i < _blues.Length; i++)
                _bluesBallList.Add(_blues[i]);
        }

        /// <summary>
        /// 获取一个大乐透单式号码[机选号和开奖号使用不同的加密种子]
        /// </summary>
        /// <param name="myRandom">是否是我的机选号</param>
        public SimplexSuperLottoNumber GetSimplexSuperLottoNumber(bool myRandom, long periods)
        {
            SimplexSuperLottoNumber simplexSuperLottoNumber = new SimplexSuperLottoNumber();

            //生成5个红球
            for (int i = 0; i < simplexSuperLottoNumber.redBalls.Length; i++)
            {
                //随机生成红球所在集合数组下标
                int redIndex = myRandom ?
                    MyRandom.GetMyRandom(0, _redsBallList.Count - 1) : MyRandom.GetRunPublicRandom(0, _redsBallList.Count - 1);

                //得到下标所对应的值
                simplexSuperLottoNumber.redBalls[i] = _redsBallList[redIndex];

                //从红球集合中移除已经开出的下标元素
                _redsBallList.RemoveAt(redIndex);
            }

            //生成2个蓝球
            for (int i = 0; i < simplexSuperLottoNumber.blueBalls.Length; i++)
            {
                int blueIndex = myRandom ?
                    MyRandom.GetMyRandom(0, _bluesBallList.Count - 1) : MyRandom.GetRunPublicRandom(0, _bluesBallList.Count - 1);

                simplexSuperLottoNumber.blueBalls[i] = _bluesBallList[blueIndex];
                _bluesBallList.RemoveAt(blueIndex);
            }

            //排序红球
            Array.Sort(simplexSuperLottoNumber.redBalls);
            Array.Sort(simplexSuperLottoNumber.blueBalls);

            simplexSuperLottoNumber.OrderByReds = GetAppendArrayValue(simplexSuperLottoNumber.redBalls);
            simplexSuperLottoNumber.OrderByBlues = GetAppendArrayValue(simplexSuperLottoNumber.blueBalls);

            if (!myRandom) RecordHistoryNumbers(simplexSuperLottoNumber, periods);
            else
            {
                simplexSuperLottoNumber.serialNumber = ++SuperLottoView._randomSuperLottoBallSerial;
            }

            ResetRedBallList();
            ResetBlueBallList();
            return simplexSuperLottoNumber;
        }

        public long GetAppendArrayValue(int[] arrays)
        {
            StringBuilder orderRedsBuilder = new StringBuilder();

            for (int i = 0; i < arrays.Length; i++)
            {
                orderRedsBuilder.Append(SuperLottoView.FormatNumber(arrays[i], 2));
            }

            return long.Parse(orderRedsBuilder.ToString());
        }

        /// <summary>
        /// 获取一个复式大乐透号码
        /// </summary>
        /// <param name="redBallCount">红球数量</param>
        /// <param name="blueBallCount">蓝球数量</param>
        public ComplexSuperLottoNumber GetComplexSuperLottoNumber(int redBallCount, int blueBallCount)
        {
            ComplexSuperLottoNumber complexSuperLottoNumber = new ComplexSuperLottoNumber();

            for (int i = 0; i < redBallCount; i++)
            {
                int redIndex = MyRandom.GetMyRandom(0, _redsBallList.Count - 1);

                complexSuperLottoNumber.redBalls[i] = _redsBallList[redIndex];

                _redsBallList.RemoveAt(redIndex);
            }

            Array.Sort(complexSuperLottoNumber.redBalls);

            for (int i = 0; i < blueBallCount; i++)
            {
                int blueIndex = MyRandom.GetMyRandom(0, _bluesBallList.Count - 1);

                complexSuperLottoNumber.blueBalls[i] = _bluesBallList[blueIndex];

                _bluesBallList.RemoveAt(blueIndex);
            }

            Array.Sort(complexSuperLottoNumber.blueBalls);

            complexSuperLottoNumber.redBallCount = redBallCount;
            complexSuperLottoNumber.blueBallCount = blueBallCount;
            complexSuperLottoNumber.serialNumber = ++SuperLottoView._ComplexSuperLottoSuperLottoCount;

            ResetRedBallList();
            ResetBlueBallList();
            return complexSuperLottoNumber;
        }

        /// <summary>
        /// 获取一个胆拖号大乐透号码
        /// </summary>
        /// <param name="redBallDanCount">红胆球数量</param>
        /// <param name="redBallTuoCount">红拖球数量</param>
        /// <param name="blueBallDanCount">蓝胆球数量</param>
        /// /// <param name="blueBallTuoCount">蓝拖球数量</param>
        public DantuoSuperLottoNumber GetDantuoSuperLottoNumber(int redBallDanCount, int redBallTuoCount, int blueBallDanCount, int blueBallTuoCount)
        {
            DantuoSuperLottoNumber dantuoSuperLottoNumber = new DantuoSuperLottoNumber();

            for (int i = 0; i < redBallDanCount; i++)
            {
                int redIndex = MyRandom.GetMyRandom(0, _redsBallList.Count - 1);

                dantuoSuperLottoNumber.redBalls[i] = _redsBallList[redIndex];

                _redsBallList.RemoveAt(redIndex);
                _redBallTuoList.RemoveAt(redIndex);
            }

            Array.Sort(dantuoSuperLottoNumber.redBalls);

            for (int i = 0; i < redBallTuoCount; i++)
            {
                int redIndex = MyRandom.GetMyRandom(0, _redBallTuoList.Count - 1);

                dantuoSuperLottoNumber.redBallTuos[i] = _redBallTuoList[redIndex];

                _redBallTuoList.RemoveAt(redIndex);
            }

            Array.Sort(dantuoSuperLottoNumber.redBallTuos);

            for (int i = 0; i < blueBallDanCount; i++)
            {
                int blueIndex = MyRandom.GetMyRandom(0, _bluesBallList.Count - 1);

                dantuoSuperLottoNumber.blueBalls[i] = _bluesBallList[blueIndex];

                _bluesBallList.RemoveAt(blueIndex);
            }

            Array.Sort(dantuoSuperLottoNumber.blueBalls);

            for (int i = 0; i < blueBallTuoCount; i++)
            {
                int blueIndex = MyRandom.GetMyRandom(0, _bluesBallList.Count - 1);

                dantuoSuperLottoNumber.blueBallTuos[i] = _bluesBallList[blueIndex];

                _bluesBallList.RemoveAt(blueIndex);
            }

            Array.Sort(dantuoSuperLottoNumber.blueBallTuos);

            dantuoSuperLottoNumber.redBallDanCount = redBallDanCount;
            dantuoSuperLottoNumber.redBallTuoCount = redBallTuoCount;
            dantuoSuperLottoNumber.blueBallDanCount = blueBallDanCount;
            dantuoSuperLottoNumber.blueBallTuoCount = blueBallTuoCount;
            dantuoSuperLottoNumber.serialNumber = ++SuperLottoView._DantuoSuperLottoSuperLottoCount;

            ResetRedBallList();
            ResetBlueBallList();
            ResetRedBallTuoList();

            return dantuoSuperLottoNumber;
        }

        /// <summary>
        /// 比较单式大乐透号码是否中奖、并将中奖数据修改到辅助类
        /// </summary>
        /// <param name="_mySuperLottoNumber">我的号码</param>
        /// <param name="_publicSuperLottoNumber">开奖号码</param>
        public void ComparisonSimplexSuperLottoNumber(SimplexSuperLottoNumber _mySuperLottoNumber, SimplexSuperLottoNumber _publicSuperLottoNumber)
        {
            //红球中奖位数
            int winningRedCount = 0;

            //篮球是否相同
            int winningBlueCount = 0;

            //比较红球中的数量
            for (int i = 0; i < _publicSuperLottoNumber.redBalls.Length; i++)
            {
                if (_mySuperLottoNumber.redBalls.Contains(_publicSuperLottoNumber.redBalls[i]))
                {
                    winningRedCount++;
                }
            }

            for (int i = 0; i < _publicSuperLottoNumber.blueBalls.Length; i++)
            {
                if (_mySuperLottoNumber.blueBalls.Contains(_publicSuperLottoNumber.blueBalls[i]))
                {
                    winningBlueCount++;
                }
            }

            SettingSuperLottoAwardLevel(_mySuperLottoNumber, winningRedCount, winningBlueCount);
        }

        /// <summary>
        /// 比较复式大乐透号码是否中奖
        ///   并统计每注数中奖的金额、几等奖
        /// </summary>
        /// <param name="_mySuperLottoNumber">我的号码</param>
        /// <param name="_publicSuperLottoNumber">开奖号码</param>
        public void ComparisonComplexSuperLottoNumber(ComplexSuperLottoNumber _mySuperLottoNumber, SimplexSuperLottoNumber _publicSuperLottoNumber)
        {
            //红球中奖位数
            int winningRedCount = 0;

            //篮球是否相同
            int winningBlueCount = 0;

            int redIndex = 0, blueIndex = 0;

            _mySuperLottoNumber.ResetTotalZhuToDefault();

            //比较红球中的数量
            for (int i = 0; i < _publicSuperLottoNumber.redBalls.Length; i++)
            {
                if (_mySuperLottoNumber.redBalls.Contains(_publicSuperLottoNumber.redBalls[i]))
                {
                    winningRedCount++;
                    //记录中奖的红球号码
                    _mySuperLottoNumber.maxRedWinPrizes[redIndex++] = _publicSuperLottoNumber.redBalls[i];
                }
            }

            for (int i = 0; i < _publicSuperLottoNumber.blueBalls.Length; i++)
            {
                if (_mySuperLottoNumber.blueBalls.Contains(_publicSuperLottoNumber.blueBalls[i]))
                {
                    winningBlueCount++;
                    _mySuperLottoNumber.maxBlueWinPrize[blueIndex++] = _publicSuperLottoNumber.blueBalls[i];
                }
            }

            SettingSuperLottoAwardLevel(_mySuperLottoNumber, winningRedCount, winningBlueCount);

            if (_mySuperLottoNumber.awardType != AwardType.NotAward)
            {
                CalculateComplexWinningAmount(
                    _mySuperLottoNumber, _mySuperLottoNumber.redBallCount, _mySuperLottoNumber.blueBallCount, winningRedCount, winningBlueCount);
            }
        }

        /// <summary>
        /// 统计复试号码中奖注数
        /// </summary>
        /// <param name="_mySuperLottoNumber">我的复试号码</param>
        /// <param name="redC">红色球总数量</param>
        /// <param name="blueC">蓝色球总数量</param>
        /// <param name="redZ">红色球中奖个数</param>
        /// <param name="blueZ">蓝色球中奖个数</param>
        /// <returns></returns>
        public void CalculateComplexWinningAmount(ComplexSuperLottoNumber _mySuperLottoNumber, int redC, int blueC, int redZ, int blueZ)
        {
            int[] aResult = new int[9];
            for (var i = 0; i <= redZ; i++)
            {
                for (var x = 0; x <= blueZ; x++)
                {
                    var m = Rank9x(i, x);
                    if (m != -1)
                    {
                        aResult[m] += Combo(redZ, i) * Combo(redC - redZ, 5 - i) * Combo(blueC - blueZ, 2 - x) * Combo(blueZ, x);
                    }
                }
            }

            SettingWinningCount(_mySuperLottoNumber, aResult);
        }

        /// <summary>
        /// 设置该注大乐透中奖级别
        /// </summary>
        /// <param name="_mySuperLottoNumber">要设置的号码</param>
        /// <param name="winningRedCount">红球中奖数</param>
        /// <param name="winningBlueCount">蓝球中奖数</param>
        private void SettingSuperLottoAwardLevel(SuperLottos _mySuperLottoNumber, int winningRedCount, int winningBlueCount)
        {
            _mySuperLottoNumber.awardType = AwardType.NotAward;

            //九等奖条件
            if ((winningRedCount == 3 && winningBlueCount == 0) || (winningRedCount == 1 && winningBlueCount == 2) || (winningRedCount == 2 && winningBlueCount == 1) || (winningRedCount == 0 && winningBlueCount == 2))
            {
                _mySuperLottoNumber.awardType = AwardType.NineAward;
            }
            //八等奖条件
            else if ((winningRedCount == 3 && winningBlueCount == 1) || (winningRedCount == 2 && winningBlueCount == 2))
            {
                _mySuperLottoNumber.awardType = AwardType.EightAward;
            }
            //七等奖条件
            else if (winningRedCount == 4 && winningBlueCount == 0)
            {
                _mySuperLottoNumber.awardType = AwardType.SevenAward;
            }
            //六等奖条件
            else if (winningRedCount == 3 && winningBlueCount == 2)
            {
                _mySuperLottoNumber.awardType = AwardType.SixAward;
            }
            //五等奖条件
            else if (winningRedCount == 4 && winningBlueCount == 1)
            {
                _mySuperLottoNumber.awardType = AwardType.FiveAward;
            }
            //四等奖条件
            else if (winningRedCount == 4 && winningBlueCount == 2)
            {
                _mySuperLottoNumber.awardType = AwardType.FourAward;
            }
            //三等奖条件
            else if (winningRedCount == 5 && winningBlueCount == 0)
            {
                _mySuperLottoNumber.awardType = AwardType.ThreeAward;
            }
            //二等奖条件
            else if (winningRedCount == 5 && winningBlueCount == 1)
            {
                _mySuperLottoNumber.awardType = AwardType.TwoAward;
            }
            //一等奖条件
            else if (winningRedCount == 5 && winningBlueCount == 2)
            {
                _mySuperLottoNumber.awardType = AwardType.OneAward;
            }
        }

        /// <summary>
        /// 设置该注号码中奖的数量
        /// </summary>
        /// <param name="_mySuperLottoNumber"></param>
        /// <param name="aResult"></param>
        private static void SettingWinningCount(ComplexSuperLottoNumber _mySuperLottoNumber, int[] aResult)
        {
            if (_mySuperLottoNumber.awardType == AwardType.OneAward)
            {
                _mySuperLottoNumber.oneAwardTotalZhu = aResult[0];
            }

            if (_mySuperLottoNumber.awardType >= AwardType.TwoAward)
            {
                _mySuperLottoNumber.twoAwardTotalZhu = aResult[1];
            }

            if (_mySuperLottoNumber.awardType >= AwardType.ThreeAward)
            {
                _mySuperLottoNumber.threeAwardTotalZhu = aResult[2];
            }

            if (_mySuperLottoNumber.awardType >= AwardType.FourAward)
            {
                _mySuperLottoNumber.fourAwardTotalZhu = aResult[3];
            }

            if (_mySuperLottoNumber.awardType >= AwardType.FiveAward)
            {
                _mySuperLottoNumber.fiveAwardTotalZhu = aResult[4];
            }

            if (_mySuperLottoNumber.awardType >= AwardType.SixAward)
            {
                _mySuperLottoNumber.sixAwardTotalZhu = aResult[5];
            }

            if (_mySuperLottoNumber.awardType >= AwardType.SevenAward)
            {
                _mySuperLottoNumber.sevenAwardTotalZhu = aResult[6];
            }

            if (_mySuperLottoNumber.awardType >= AwardType.EightAward)
            {
                _mySuperLottoNumber.eightAwardTotalZhu = aResult[7];
            }

            if (_mySuperLottoNumber.awardType >= AwardType.NineAward)
            {
                _mySuperLottoNumber.nineAwardTotalZhu = aResult[8];
            }
        }

        /// <summary>
        /// 比较胆拖大乐透号码是否中奖
        ///   并统计每注数中奖的金额、几等奖
        /// </summary>
        /// <param name="_mySuperLottoNumber">我的号码</param>
        /// <param name="_publicSuperLottoNumber">开奖号码</param>
        public void ComparisonDantuoSuperLottoNumber(DantuoSuperLottoNumber _mySuperLottoNumber, SimplexSuperLottoNumber _publicSuperLottoNumber)
        {
            //红球胆的中奖位数
            int winningRedCount = 0;
            //红球拖的中奖位数
            int winningRedTuoCount = 0;

            //篮球是否相同
            bool winningBlue;
            _mySuperLottoNumber.ResetTotalZhuToDefault();

            int redWinPrizesIndex = 0;
            //比较红球胆中的数量
            for (int i = 0; i < _publicSuperLottoNumber.redBalls.Length; i++)
            {
                if (_mySuperLottoNumber.redBalls.Contains(_publicSuperLottoNumber.redBalls[i]))
                {
                    winningRedCount++;
                    //记录中奖的红球胆号码
                    _mySuperLottoNumber.maxRedWinPrizes[redWinPrizesIndex++] = _publicSuperLottoNumber.redBalls[i];
                }
            }

            int pickTuoCount = 6 - _mySuperLottoNumber.redBallDanCount;

            //比较红球拖中的数量
            for (int i = 0; i < _publicSuperLottoNumber.redBalls.Length; i++)
            {
                if (_mySuperLottoNumber.redBallTuos.Contains(_publicSuperLottoNumber.redBalls[i]))
                {
                    winningRedTuoCount++;
                    //记录中奖的红球拖号码
                    _mySuperLottoNumber.maxRedWinPrizes[redWinPrizesIndex++] = _publicSuperLottoNumber.redBalls[i];

                    if (winningRedTuoCount >= pickTuoCount)
                    {
                        break;
                    }
                }
            }

            winningRedCount += winningRedTuoCount;
            winningBlue = _mySuperLottoNumber.blueBalls.Contains(_publicSuperLottoNumber.blueBalls[0]);

            if (winningBlue)
            {
                _mySuperLottoNumber.maxBlueWinPrize = _publicSuperLottoNumber.blueBalls;
            }

            if (winningRedCount <= 3 && !winningBlue)
            {
                _mySuperLottoNumber.awardType = AwardType.NotAward;
            }
            else if (winningRedCount <= 2 && winningBlue)
            {
                _mySuperLottoNumber.awardType = AwardType.SixAward;
            }
            else if ((winningRedCount == 3 && winningBlue) || (winningRedCount == 4 && !winningBlue))
            {
                _mySuperLottoNumber.awardType = AwardType.FiveAward;
            }
            else if ((winningRedCount == 4 && winningBlue) || (winningRedCount == 5 && !winningBlue))
            {
                _mySuperLottoNumber.awardType = AwardType.FourAward;
            }
            else if (winningRedCount == 5 && winningBlue)
            {
                _mySuperLottoNumber.awardType = AwardType.ThreeAward;
            }
            else if (winningRedCount == 6 && !winningBlue)
            {
                _mySuperLottoNumber.awardType = AwardType.TwoAward;
            }
            else if (winningRedCount == 6 && winningBlue)
            {
                _mySuperLottoNumber.awardType = AwardType.OneAward;
            }

            if (_mySuperLottoNumber.awardType != AwardType.NotAward)
            {
                CalculateDantuoWinningAmount(_mySuperLottoNumber,
                    _mySuperLottoNumber.redBallDanCount, _mySuperLottoNumber.redBallTuoCount, _mySuperLottoNumber.blueBallDanCount,
                    winningRedCount > 0 ? winningRedCount - winningRedTuoCount : 0, winningRedTuoCount, winningBlue ? 1 : 0);
            }
        }

        /// <summary>
        /// 统计胆拖号码中奖注数
        /// </summary>
        /// <param name="_mySuperLottoNumber">我的胆拖号码</param>
        /// <param name="redDa">红色球胆总数量</param>
        /// <param name="redTA">红色球拖总数量</param>
        /// <param name="blueA">蓝色球数量</param>
        /// <param name="redDB">红色球胆的中奖个数</param>
        /// <param name="redTB">红色球拖的中奖个数</param>
        /// <param name="blueB">蓝色球中奖个数</param>
        /// <returns></returns>
        public void CalculateDantuoWinningAmount(DantuoSuperLottoNumber _mySuperLottoNumber, int redDa, int RedTa, int blueA, int redDb, int redTb, int blueB)
        {
            var v = redTb <= 6 - redDa ? redTb : 6 - redDa;
            int[] t = new int[v + 1];

            for (var i = 0; i <= v; i++)
            {
                t[i] = redDb + i;
            }

            int q;
            int[] aResult = new int[6];

            if (blueB == 0)
            {
                for (var i = 0; i <= v; i++)
                {
                    q = Rank(t[i], 0);
                    if (q != -1)
                    {
                        aResult[q] += Ccombo(RedTa - redTb, 6 - redDa - i) * blueA * Ccombo(redTb, i);
                    }
                }
            }
            if (blueB == 1)
            {
                for (var i = 0; i <= v; i++)
                {
                    q = Rank(t[i], 0);
                    if (q != -1)
                    {
                        aResult[q] += Ccombo(RedTa - redTb, 6 - redDa - i) * (blueA - 1) * Ccombo(redTb, i);
                    }
                    q = Rank(t[i], 1);
                    aResult[q] += Ccombo(RedTa - redTb, 6 - redDa - i) * Ccombo(redTb, i);
                }
            }

            if (_mySuperLottoNumber.awardType == AwardType.OneAward)
            {
                _mySuperLottoNumber.oneAwardTotalZhu = aResult[0];
            }

            if (_mySuperLottoNumber.awardType >= AwardType.TwoAward)
            {
                _mySuperLottoNumber.twoAwardTotalZhu = aResult[1];
            }

            if (_mySuperLottoNumber.awardType >= AwardType.ThreeAward)
            {
                _mySuperLottoNumber.threeAwardTotalZhu = aResult[2];
            }

            if (_mySuperLottoNumber.awardType >= AwardType.FourAward)
            {
                _mySuperLottoNumber.fourAwardTotalZhu = aResult[3];
            }

            if (_mySuperLottoNumber.awardType >= AwardType.FiveAward)
            {
                _mySuperLottoNumber.fiveAwardTotalZhu = aResult[4];
            }

            if (_mySuperLottoNumber.awardType >= AwardType.SixAward)
            {
                _mySuperLottoNumber.sixAwardTotalZhu = aResult[5];
            }
        }

        public static int Combo(int n1, int n2)
        {
            int h, f;
            if (n1 / 2 < n2) n2 = n1 - n2;

            if (n1 < n2 || n2 < 0) return 0;
            if (n1 >= 0 && n2 == 0) return 1;

            h = 1;
            f = n1;
            for (var i = 1; i <= n2; i++)
            {
                h *= i;
                if (i < n2) f *= n1 - i;
            }
            return f / h;
        }

        public int Ccombo(int n1, int n2)
        {
            if (n2 <= n1)
            {
                var d = Factorial(n1 - n2) * Factorial(n2);
                string result = (Factorial(n1) / d).ToString();
                return int.Parse(result);
            }
            return 0;
        }

        public int Rank9x(int n1, int n2)
        {
            var g = n1 * 1 + n2 * 1;

            switch (g)
            {
                case 7:
                    return 0;
                case 6:
                    return n2 == 1 ? 1 : 3;
                case 5:
                    if (n2 == 0) return 2;
                    else
                    {
                        if (n2 == 1) return 4;
                        else
                        {
                            if (n2 == 2) return 5;
                        }
                    }
                    return -1;
                case 4:
                    return n2 == 0 ? 6 : 7;
                case 3:
                    return 8;
                case 2:
                    return n1 == 0 ? 8 : -1;
                default:
                    return -1;
            }
        }

        private int Rank(int n1, int n2)
        {
            var g = n1 * 1 + n2 * 1;
            switch (g)
            {
                case 7:
                    return 0;
                case 6:
                    return n2 == 1 ? 2 : 1;
                case 5:
                    return 3;
                case 4:
                    return 4;
                default:
                    return n2 == 1 ? 5 : -1;
            }
        }

        public long Factorial(int num) => DantuoSuperLottoNumber.Factorial(num);

        /// <summary>
        /// 动态向队列增加历史开奖号
        /// </summary>
        private void RecordHistoryNumbers(SimplexSuperLottoNumber _publicSuperLottoNumber, long periods)
        {
            if (Config._historyPublicSuperLottoNumbers.Count > 100)
            {
                Config._historyPublicSuperLottoNumbers.Dequeue();
            }

            _publicSuperLottoNumber.Periods = periods;
            Config._historyPublicSuperLottoNumbers.Enqueue(_publicSuperLottoNumber);
        }

        /// <summary>
        /// 重置存储红球常量的集合
        /// </summary>
        private void ResetRedBallList()
        {
            _redsBallList.Clear();

            for (int i = 0; i < _reds.Length; i++)
                _redsBallList.Add(_reds[i]);
        }

        /// <summary>
        /// 重置存储红球拖常量的集合
        /// </summary>
        private void ResetRedBallTuoList()
        {
            _redBallTuoList.Clear();

            for (int i = 0; i < _reds.Length; i++)
                _redBallTuoList.Add(_reds[i]);
        }

        /// <summary>
        /// 重置蓝色球常量集合
        /// </summary>
        private void ResetBlueBallList()
        {
            _bluesBallList.Clear();

            for (int i = 0; i < _blues.Length; i++)
                _bluesBallList.Add(_blues[i]);
        }

        /// <summary>
        /// 获取复试号指定组合的总注数
        /// </summary>
        public int GetComplexCombinationTotalZhu(int redCount, int blueCount)
        {
            return redCount * (redCount - 1) * (redCount - 2) * (redCount - 3) * (redCount - 4)
                / (1 * 2 * 3 * 4 * 5) * (blueCount * (blueCount - 1) / (1 * 2));
        }

        /// <summary>
        /// 获取胆拖号指定组合的总注数
        /// </summary>
        public long GetDantuoCombinationTotalZhu(int redDanCount, int redTuoCount, int blueDanCount, int blueTuoCount)
        {
            return Combo(redTuoCount, 5 - redDanCount) * Combo(blueTuoCount, 2 - blueDanCount);
        }
    }
}
