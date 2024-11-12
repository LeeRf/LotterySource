using DoubleBalls.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DoubleBalls.Model
{
    /// <summary>
    /// 双色球类
    ///   Athor：Lee
    ///   编写于 2020/12/29
    /// </summary>
    public class DoubleBallTool
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
            17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33 };
        //蓝球常量
        private readonly int[] _blues = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

        public DoubleBallTool()
        {
            for (int i = 0; i < _reds.Length; i++)
                _redsBallList.Add(_reds[i]);

            for (int i = 0; i < _reds.Length; i++)
                _redBallTuoList.Add(_reds[i]);

            for (int i = 0; i < _blues.Length; i++)
                _bluesBallList.Add(_blues[i]);
        }

        /// <summary>
        /// 获取一个双色球单式号码[机选号和开奖号使用不同的加密种子]
        /// </summary>
        /// <param name="myRandom">是否是我的机选号</param>
        public SimplexDoubleBallNumber GetSimplexDoubleBallNumber(bool myRandom, long periods)
        {
            StringBuilder orderRedsBuilder = new StringBuilder();
            SimplexDoubleBallNumber simplexDoubleBallNumber = new SimplexDoubleBallNumber();

            //生成6个红球
            for (int i = 0; i < 6; i++)
            {
                //随机生成红球所在集合数组下标
                int redIndex = myRandom ? 
                    MyRandom.GetMyRandom(0, _redsBallList.Count - 1) : MyRandom.GetRunPublicRandom(0, _redsBallList.Count - 1);

                //得到下标所对应的值
                simplexDoubleBallNumber.redBalls[i] = _redsBallList[redIndex];

                //从红球集合中移除已经开出的下标元素
                _redsBallList.RemoveAt(redIndex);
            }

            //排序红球
            Array.Sort(simplexDoubleBallNumber.redBalls);

            //排序后得到红球的总值用于排序
            for (int i = 0; i < 6; i++)
            {
                orderRedsBuilder.Append(
                    DoubleBallView.FormatNumber(simplexDoubleBallNumber.redBalls[i], 2));
            }
            
            //随机生成最后的篮球
            int blueResult = myRandom ? 
                MyRandom.GetMyRandom(1, _blues.Length) : MyRandom.GetRunPublicRandom(1, _blues.Length);

            simplexDoubleBallNumber.OrderByBlues = blueResult;
            simplexDoubleBallNumber.blueBall = blueResult;
            simplexDoubleBallNumber.OrderByReds = long.Parse(orderRedsBuilder.ToString());

            if (!myRandom) RecordHistoryNumbers(simplexDoubleBallNumber, periods);
            else
            {
                simplexDoubleBallNumber.serialNumber = ++DoubleBallView._randomDoubleDoubleBallSerial;
            }

            ResetRedBallList();
            return simplexDoubleBallNumber;
        }

        /// <summary>
        /// 获取一个复式双色球号码
        /// </summary>
        /// <param name="redBallCount">红球数量</param>
        /// <param name="blueBallCount">蓝球数量</param>
        public ComplexDoubleBallNumber GetComplexDoubleBallNumber(int redBallCount, int blueBallCount)
        {
            ComplexDoubleBallNumber complexDoubleBallNumber = new ComplexDoubleBallNumber();

            for (int i = 0; i < redBallCount; i++)
            {
                int redIndex = MyRandom.GetMyRandom(0, _redsBallList.Count - 1);

                complexDoubleBallNumber.redBalls[i] = _redsBallList[redIndex];

                _redsBallList.RemoveAt(redIndex);
            }

            Array.Sort(complexDoubleBallNumber.redBalls);

            for (int i = 0; i < blueBallCount; i++)
            {
                int blueIndex = MyRandom.GetMyRandom(0, _bluesBallList.Count - 1);

                complexDoubleBallNumber.blueBalls[i] = _bluesBallList[blueIndex];

                _bluesBallList.RemoveAt(blueIndex);
            }

            Array.Sort(complexDoubleBallNumber.blueBalls);

            complexDoubleBallNumber.redBallCount = redBallCount;
            complexDoubleBallNumber.blueBallCount = blueBallCount;
            complexDoubleBallNumber.serialNumber = ++DoubleBallView._ComplexDoubleBallDoubleBallCount;

            ResetRedBallList();
            ResetBlueBallList();
            return complexDoubleBallNumber;
        }

        /// <summary>
        /// 获取一个胆拖号双色球号码
        /// </summary>
        /// <param name="redBallDanCount">红胆球数量</param>
        /// <param name="redBallTuoCount">红拖球数量</param>
        /// <param name="blueBallCount">蓝色球数量</param>
        public DantuoDoubleBallNumber GetDantuoDoubleBallNumber(int redBallDanCount, int redBallTuoCount, int blueBallCount)
        {
            DantuoDoubleBallNumber dantuoDoubleBallNumber = new DantuoDoubleBallNumber();

            for (int i = 0; i < redBallDanCount; i++)
            {
                int redIndex = MyRandom.GetMyRandom(0, _redsBallList.Count - 1);

                dantuoDoubleBallNumber.redBalls[i] = _redsBallList[redIndex];

                _redsBallList.RemoveAt(redIndex);
                _redBallTuoList.RemoveAt(redIndex);
            }

            Array.Sort(dantuoDoubleBallNumber.redBalls);

            for (int i = 0; i < redBallTuoCount; i++)
            {
                int redIndex = MyRandom.GetMyRandom(0, _redBallTuoList.Count - 1);

                dantuoDoubleBallNumber.redBallTuos[i] = _redBallTuoList[redIndex];

                _redBallTuoList.RemoveAt(redIndex);
            }

            Array.Sort(dantuoDoubleBallNumber.redBallTuos);

            for (int i = 0; i < blueBallCount; i++)
            {
                int blueIndex = MyRandom.GetMyRandom(0, _bluesBallList.Count - 1);

                dantuoDoubleBallNumber.blueBalls[i] = _bluesBallList[blueIndex];

                _bluesBallList.RemoveAt(blueIndex);
            }

            Array.Sort(dantuoDoubleBallNumber.blueBalls);

            dantuoDoubleBallNumber.redBallDanCount = redBallDanCount;
            dantuoDoubleBallNumber.redBallTuoCount = redBallTuoCount;
            dantuoDoubleBallNumber.blueBallCount = blueBallCount;
            dantuoDoubleBallNumber.serialNumber = ++DoubleBallView._DantuoDoubleBallDoubleBallCount;

            ResetRedBallList();
            ResetBlueBallList();
            ResetRedBallTuoList();

            return dantuoDoubleBallNumber;
        }

        /// <summary>
        /// 比较单式双色球号码是否中奖、并将中奖数据修改到辅助类
        /// </summary>
        /// <param name="_myDoubleBallNumber">我的号码</param>
        /// <param name="_publicDoubleNumber">开奖号码</param>
        public void ComparisonSimplexDoubleBallNumber(SimplexDoubleBallNumber _myDoubleBallNumber, SimplexDoubleBallNumber _publicDoubleNumber)
        {
            //红球中奖位数
            int winningRedCount = 0;

            //篮球是否相同
            bool winningBlue;

            //比较红球中的数量
            for (int i = 0; i < _publicDoubleNumber.redBalls.Length; i++)
            {
                if (_myDoubleBallNumber.redBalls.Contains(_publicDoubleNumber.redBalls[i]))
                {
                    winningRedCount++;
                }
            }

            //比较蓝色球是否中奖
            winningBlue = _myDoubleBallNumber.blueBall == _publicDoubleNumber.blueBall;

            //未中奖条件
            if (winningRedCount <= 3 && !winningBlue)
            {
                _myDoubleBallNumber.awardType = AwardType.NotAward;
            }
            //六等奖条件
            else if (winningRedCount <= 2 && winningBlue)
            {
                _myDoubleBallNumber.awardType = AwardType.SixAward;
            }
            //五等奖条件
            else if ((winningRedCount == 3 && winningBlue) || (winningRedCount == 4 && !winningBlue))
            {
                _myDoubleBallNumber.awardType = AwardType.FiveAward;
            }
            //四等奖条件
            else if ((winningRedCount == 4 && winningBlue) || (winningRedCount == 5 && !winningBlue))
            {
                _myDoubleBallNumber.awardType = AwardType.FourAward;
            }
            //三等奖条件
            else if (winningRedCount == 5 && winningBlue)
            {
                _myDoubleBallNumber.awardType = AwardType.ThreeAward;
            }
            //二等奖条件
            else if (winningRedCount == 6 && !winningBlue)
            {
                _myDoubleBallNumber.awardType = AwardType.TwoAward;
            }
            //一等奖条件
            else if (winningRedCount == 6 && winningBlue)
            {
                _myDoubleBallNumber.awardType = AwardType.OneAward;
            }
        }

        /// <summary>
        /// 比较复式双色球号码是否中奖
        ///   并统计每注数中奖的金额、几等奖
        /// </summary>
        /// <param name="_myDoubleBallNumber">我的号码</param>
        /// <param name="_publicDoubleNumber">开奖号码</param>
        public void ComparisonComplexDoubleBallNumber(ComplexDoubleBallNumber _myDoubleBallNumber, SimplexDoubleBallNumber _publicDoubleNumber)
        {
            //红球中奖位数
            int winningRedCount = 0;

            //篮球是否相同
            bool winningBlue;

            _myDoubleBallNumber.ResetTotalZhuToDefault();

            int redWinPrizeIndex = 0;
            //比较红球中的数量
            for (int i = 0; i < _publicDoubleNumber.redBalls.Length; i++)
            {
                if (_myDoubleBallNumber.redBalls.Contains(_publicDoubleNumber.redBalls[i]))
                {
                    winningRedCount++;
                    //记录中奖的红球号码
                    _myDoubleBallNumber.redWinPrizes[redWinPrizeIndex++] = _publicDoubleNumber.redBalls[i];
                }
            }

             winningBlue = _myDoubleBallNumber.blueBalls.Contains(_publicDoubleNumber.blueBall);

            //如果有记录蓝色球中奖号码
            if (winningBlue)
            {
                _myDoubleBallNumber.blueWinPrize = _publicDoubleNumber.blueBall;
            }

            if (winningRedCount <= 3 && !winningBlue)
            {
                _myDoubleBallNumber.awardType = AwardType.NotAward;
            }
            else if (winningRedCount <= 2 && winningBlue)
            {
                _myDoubleBallNumber.awardType = AwardType.SixAward;
            }
            else if ((winningRedCount == 3 && winningBlue) || (winningRedCount == 4 && !winningBlue))
            {
                _myDoubleBallNumber.awardType = AwardType.FiveAward;
            }
            else if ((winningRedCount == 4 && winningBlue) || (winningRedCount == 5 && !winningBlue))
            {
                _myDoubleBallNumber.awardType = AwardType.FourAward;
            }
            else if (winningRedCount == 5 && winningBlue)
            {
                _myDoubleBallNumber.awardType = AwardType.ThreeAward;
            }
            else if (winningRedCount == 6 && !winningBlue)
            {
                _myDoubleBallNumber.awardType = AwardType.TwoAward;
            }
            else if (winningRedCount == 6 && winningBlue)
            {
                _myDoubleBallNumber.awardType = AwardType.OneAward;
            }

            if (_myDoubleBallNumber.awardType != AwardType.NotAward)
            {
                CalculateComplexWinningAmount(
                    _myDoubleBallNumber, _myDoubleBallNumber.redBallCount, winningRedCount, _myDoubleBallNumber.blueBallCount, winningBlue ? 1 : 0);
            }
        }

        /// <summary>
        /// 统计复试号码中奖注数
        /// </summary>
        /// <param name="_myDoubleBallNumber">我的复试号码</param>
        /// <param name="redA">红色球总数量</param>
        /// <param name="redB">红色球中奖个数</param>
        /// <param name="blueA">蓝色球数量</param>
        /// <param name="blueB">蓝色球中奖个数</param>
        /// <returns></returns>
        public void CalculateComplexWinningAmount(ComplexDoubleBallNumber _myDoubleBallNumber, int redA, int redB, int blueA, int blueB)
        {
            if (_myDoubleBallNumber.awardType == AwardType.OneAward)
            {
                _myDoubleBallNumber.oneAwardTotalZhu = blueB * clat(6, redB) * clat(0, redA - redB);
            }

            if (_myDoubleBallNumber.awardType >= AwardType.TwoAward)
            {
                _myDoubleBallNumber.twoAwardTotalZhu = (blueA - blueB) * clat(6, redB) * clat(0, redA - redB);
            }

            if (_myDoubleBallNumber.awardType >= AwardType.ThreeAward)
            {
                _myDoubleBallNumber.threeAwardTotalZhu = blueB * clat(5, redB) * clat(1, redA - redB);
            }

            if (_myDoubleBallNumber.awardType >= AwardType.FourAward)
            {
                _myDoubleBallNumber.fourAwardTotalZhu =
                    blueB * clat(4, redB) * clat(2, redA - redB) +
                    (blueA - blueB) * clat(5, redB) * clat(1, redA - redB);
            }

            if (_myDoubleBallNumber.awardType >= AwardType.FiveAward)
            {
                _myDoubleBallNumber.fiveAwardTotalZhu =
                    blueB * clat(3, redB) * clat(3, redA - redB) +
                    (blueA - blueB) * clat(4, redB) * clat(2, redA - redB);
            }

            if (_myDoubleBallNumber.awardType >= AwardType.SixAward)
            {
                _myDoubleBallNumber.sixAwardTotalZhu = 
                    blueB * clat(2, redB) * clat(4, redA - redB) + 
                    blueB * clat(1, redB) * clat(5, redA - redB) + 
                    blueB * clat(0, redB) * clat(6, redA - redB);
            }
        }

        public int clat(int start, int end)
        {
            var C = 1;
            for (var i = end - start + 1; i <= end; i++)
                C *= i;

            for (var i = 2; i <= start; i++)
                C /= i;

            return C;
        }

        /// <summary>
        /// 比较胆拖双色球号码是否中奖
        ///   并统计每注数中奖的金额、几等奖
        /// </summary>
        /// <param name="_myDoubleBallNumber">我的号码</param>
        /// <param name="_publicDoubleNumber">开奖号码</param>
        public void ComparisonDantuoDoubleBallNumber(DantuoDoubleBallNumber _myDoubleBallNumber, SimplexDoubleBallNumber _publicDoubleNumber)
        {
            //红球胆的中奖位数
            int winningRedCount = 0;
            //红球拖的中奖位数
            int winningRedTuoCount = 0;

            //篮球是否相同
            bool winningBlue;
            _myDoubleBallNumber.ResetTotalZhuToDefault();

            int redWinPrizesIndex = 0;
            //比较红球胆中的数量
            for (int i = 0; i < _publicDoubleNumber.redBalls.Length; i++)
            {
                if (_myDoubleBallNumber.redBalls.Contains(_publicDoubleNumber.redBalls[i]))
                {
                    winningRedCount++;
                    //记录中奖的红球胆号码
                    _myDoubleBallNumber.redWinPrizes[redWinPrizesIndex++] = _publicDoubleNumber.redBalls[i];
                }
            }

            int pickTuoCount = 6 - _myDoubleBallNumber.redBallDanCount;

            //比较红球拖中的数量
            for (int i = 0; i < _publicDoubleNumber.redBalls.Length; i++)
            {
                if (_myDoubleBallNumber.redBallTuos.Contains(_publicDoubleNumber.redBalls[i]))
                {
                    winningRedTuoCount++;
                    //记录中奖的红球拖号码
                    _myDoubleBallNumber.redWinPrizes[redWinPrizesIndex++] = _publicDoubleNumber.redBalls[i];

                    if (winningRedTuoCount >= pickTuoCount)
                    {
                        break;
                    }
                }
            }

            winningRedCount += winningRedTuoCount;
            winningBlue = _myDoubleBallNumber.blueBalls.Contains(_publicDoubleNumber.blueBall);

            if (winningBlue)
            {
                _myDoubleBallNumber.blueWinPrize = _publicDoubleNumber.blueBall;
            }

            if (winningRedCount <= 3 && !winningBlue)
            {
                _myDoubleBallNumber.awardType = AwardType.NotAward;
            }
            else if (winningRedCount <= 2 && winningBlue)
            {
                _myDoubleBallNumber.awardType = AwardType.SixAward;
            }
            else if ((winningRedCount == 3 && winningBlue) || (winningRedCount == 4 && !winningBlue))
            {
                _myDoubleBallNumber.awardType = AwardType.FiveAward;
            }
            else if ((winningRedCount == 4 && winningBlue) || (winningRedCount == 5 && !winningBlue))
            {
                _myDoubleBallNumber.awardType = AwardType.FourAward;
            }
            else if (winningRedCount == 5 && winningBlue)
            {
                _myDoubleBallNumber.awardType = AwardType.ThreeAward;
            }
            else if (winningRedCount == 6 && !winningBlue)
            {
                _myDoubleBallNumber.awardType = AwardType.TwoAward;
            }
            else if (winningRedCount == 6 && winningBlue)
            {
                _myDoubleBallNumber.awardType = AwardType.OneAward;
            }

            if (_myDoubleBallNumber.awardType != AwardType.NotAward)
            {
                CalculateDantuoWinningAmount(
                    _myDoubleBallNumber,
                    _myDoubleBallNumber.redBallDanCount,
                    _myDoubleBallNumber.redBallTuoCount,
                    _myDoubleBallNumber.blueBallCount,
                    winningRedCount > 0 ? winningRedCount - winningRedTuoCount : 0,
                    winningRedTuoCount,
                    winningBlue ? 1 : 0
                 );
            }
        }

        /// <summary>
        /// 统计胆拖号码中奖注数
        /// </summary>
        /// <param name="_myDoubleBallNumber">我的胆拖号码</param>
        /// <param name="redDa">红色球胆总数量</param>
        /// <param name="redTA">红色球拖总数量</param>
        /// <param name="blueA">蓝色球总数量</param>
        /// <param name="redDB">红色球胆的中奖个数</param>
        /// <param name="redTB">红色球拖的中奖个数</param>
        /// <param name="blueB">蓝色球中奖个数</param>
        /// <returns></returns>
        public void CalculateDantuoWinningAmount(DantuoDoubleBallNumber _myDoubleBallNumber, int redDa, int RedTa, int blueA, int redDb, int redTb, int blueB)
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

            if (_myDoubleBallNumber.awardType == AwardType.OneAward)
            {
                _myDoubleBallNumber.oneAwardTotalZhu = aResult[0];
            }

            if (_myDoubleBallNumber.awardType >= AwardType.TwoAward)
            {
                _myDoubleBallNumber.twoAwardTotalZhu = aResult[1];
            }

            if (_myDoubleBallNumber.awardType >= AwardType.ThreeAward)
            {
                _myDoubleBallNumber.threeAwardTotalZhu = aResult[2];
            }

            if (_myDoubleBallNumber.awardType >= AwardType.FourAward)
            {
                _myDoubleBallNumber.fourAwardTotalZhu = aResult[3];
            }

            if (_myDoubleBallNumber.awardType >= AwardType.FiveAward)
            {
                _myDoubleBallNumber.fiveAwardTotalZhu = aResult[4];
            }

            if (_myDoubleBallNumber.awardType >= AwardType.SixAward)
            {
                _myDoubleBallNumber.sixAwardTotalZhu = aResult[5];
            }
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

        public long Factorial(int num) => DantuoDoubleBallNumber.Factorial(num);

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


        /// <summary>
        /// 动态向队列增加历史开奖号
        /// </summary>
        private void RecordHistoryNumbers(SimplexDoubleBallNumber _publicDoubleNumber, long periods)
        {
            if (Config._historyPublicDoubleBallNumbers.Count > 100)
            {
                Config._historyPublicDoubleBallNumbers.Dequeue();
            }

            _publicDoubleNumber.Periods = periods;
            Config._historyPublicDoubleBallNumbers.Enqueue(_publicDoubleNumber);
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
            return (redCount) * (redCount - 1) * (redCount - 2) * (redCount - 3) * (redCount - 4) * (redCount - 5)
                / (1 * 2 * 3 * 4 * 5 * 6) * blueCount;
        }

        /// <summary>
        /// 获取胆拖号指定组合的总注数
        /// </summary>
        public long GetDantuoCombinationTotalZhu(int redDanCount, int redTuoCount, int blueCount)
        {
            int pickCount = 6 - redDanCount;
            return 
                Factorial(redTuoCount) / (Factorial(pickCount) * Factorial(redTuoCount - pickCount)) * blueCount;
        }
    }
}
