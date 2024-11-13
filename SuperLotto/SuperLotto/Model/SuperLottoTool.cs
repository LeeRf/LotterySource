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
            int winRedCount = 0;

            //篮球是否相同
            int winBlueCount = 0;

            //比较红球中的数量
            for (int i = 0; i < _publicSuperLottoNumber.redBalls.Length; i++)
            {
                if (_mySuperLottoNumber.redBalls.Contains(_publicSuperLottoNumber.redBalls[i]))
                {
                    winRedCount++;
                }
            }

            for (int i = 0; i < _publicSuperLottoNumber.blueBalls.Length; i++)
            {
                if (_mySuperLottoNumber.blueBalls.Contains(_publicSuperLottoNumber.blueBalls[i]))
                {
                    winBlueCount++;
                }
            }

            SettingSuperLottoAwardLevel(_mySuperLottoNumber, winRedCount, winBlueCount);
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
            int winRedCount = 0;
            //篮球是否相同
            int winBlueCount = 0;

            int redIndex = 0, blueIndex = 0;

            _mySuperLottoNumber.ResetTotalZhuToDefault();

            //比较红球中的数量
            for (int i = 0; i < _publicSuperLottoNumber.redBalls.Length; i++)
            {
                if (_mySuperLottoNumber.redBalls.Contains(_publicSuperLottoNumber.redBalls[i]))
                {
                    winRedCount++;
                    //记录中奖的红球号码
                    _mySuperLottoNumber.maxRedWinPrizes[redIndex++] = _publicSuperLottoNumber.redBalls[i];
                }
            }

            for (int i = 0; i < _publicSuperLottoNumber.blueBalls.Length; i++)
            {
                if (_mySuperLottoNumber.blueBalls.Contains(_publicSuperLottoNumber.blueBalls[i]))
                {
                    winBlueCount++;
                    _mySuperLottoNumber.maxBlueWinPrize[blueIndex++] = _publicSuperLottoNumber.blueBalls[i];
                }
            }

            SettingSuperLottoAwardLevel(_mySuperLottoNumber, winRedCount, winBlueCount);

            if (_mySuperLottoNumber.awardType != AwardType.NotAward)
            {
                CalculateComplexWinningAmount(
                    _mySuperLottoNumber, _mySuperLottoNumber.redBallCount, _mySuperLottoNumber.blueBallCount, winRedCount, winBlueCount);
            }
        }

        /// <summary>
        /// 统计复试号码中奖注数
        /// </summary>
        /// <param name="_mySuperLottoNumber">我的复试号码</param>
        /// <param name="redCount">红色球总数量</param>
        /// <param name="blueCount">蓝色球总数量</param>
        /// <param name="winRedCount">红色球中奖个数</param>
        /// <param name="winBlueCount">蓝色球中奖个数</param>
        /// <returns></returns>
        public void CalculateComplexWinningAmount(ComplexSuperLottoNumber _mySuperLottoNumber, int redCount, int blueCount, int winRedCount, int winBlueCount)
        {
            int[] awardResults = new int[9];
            for (var i = 0; i <= winRedCount; i++)
            {
                for (var x = 0; x <= winBlueCount; x++)
                {
                    var m = Rank9x(i, x);
                    if (m != -1)
                    {
                        awardResults[m] += Combo(winRedCount, i) * Combo(redCount - winRedCount, 5 - i) * Combo(blueCount - winBlueCount, 2 - x) * Combo(winBlueCount, x);
                    }
                }
            }

            SettingComplexWinningCount(_mySuperLottoNumber, awardResults);
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
        /// 设置复试号码中奖的数量
        /// </summary>
        /// <param name="_mySuperLottoNumber"></param>
        /// <param name="awardResults"></param>
        private static void SettingComplexWinningCount(ComplexSuperLottoNumber _mySuperLottoNumber, int[] awardResults)
        {
            if (_mySuperLottoNumber.awardType == AwardType.OneAward)
            {
                _mySuperLottoNumber.oneAwardTotalZhu = awardResults[0];
            }

            if (_mySuperLottoNumber.awardType >= AwardType.TwoAward)
            {
                _mySuperLottoNumber.twoAwardTotalZhu = awardResults[1];
            }

            if (_mySuperLottoNumber.awardType >= AwardType.ThreeAward)
            {
                _mySuperLottoNumber.threeAwardTotalZhu = awardResults[2];
            }

            if (_mySuperLottoNumber.awardType >= AwardType.FourAward)
            {
                _mySuperLottoNumber.fourAwardTotalZhu = awardResults[3];
            }

            if (_mySuperLottoNumber.awardType >= AwardType.FiveAward)
            {
                _mySuperLottoNumber.fiveAwardTotalZhu = awardResults[4];
            }

            if (_mySuperLottoNumber.awardType >= AwardType.SixAward)
            {
                _mySuperLottoNumber.sixAwardTotalZhu = awardResults[5];
            }

            if (_mySuperLottoNumber.awardType >= AwardType.SevenAward)
            {
                _mySuperLottoNumber.sevenAwardTotalZhu = awardResults[6];
            }

            if (_mySuperLottoNumber.awardType >= AwardType.EightAward)
            {
                _mySuperLottoNumber.eightAwardTotalZhu = awardResults[7];
            }

            if (_mySuperLottoNumber.awardType >= AwardType.NineAward)
            {
                _mySuperLottoNumber.nineAwardTotalZhu = awardResults[8];
            }
        }

        /// <summary>
        /// 设置胆拖号码中奖的数量
        /// </summary>
        /// <param name="_mySuperLottoNumber"></param>
        /// <param name="awardResults"></param>
        private static void SettingDanTuoWinningCount(DantuoSuperLottoNumber _mySuperLottoNumber, int[] awardResults)
        {
            if (_mySuperLottoNumber.awardType == AwardType.OneAward)
            {
                _mySuperLottoNumber.oneAwardTotalZhu = awardResults[0];
            }

            if (_mySuperLottoNumber.awardType >= AwardType.TwoAward)
            {
                _mySuperLottoNumber.twoAwardTotalZhu = awardResults[1];
            }

            if (_mySuperLottoNumber.awardType >= AwardType.ThreeAward)
            {
                _mySuperLottoNumber.threeAwardTotalZhu = awardResults[2];
            }

            if (_mySuperLottoNumber.awardType >= AwardType.FourAward)
            {
                _mySuperLottoNumber.fourAwardTotalZhu = awardResults[3];
            }

            if (_mySuperLottoNumber.awardType >= AwardType.FiveAward)
            {
                _mySuperLottoNumber.fiveAwardTotalZhu = awardResults[4];
            }

            if (_mySuperLottoNumber.awardType >= AwardType.SixAward)
            {
                _mySuperLottoNumber.sixAwardTotalZhu = awardResults[5];
            }

            if (_mySuperLottoNumber.awardType >= AwardType.SevenAward)
            {
                _mySuperLottoNumber.sevenAwardTotalZhu = awardResults[6];
            }

            if (_mySuperLottoNumber.awardType >= AwardType.EightAward)
            {
                _mySuperLottoNumber.eightAwardTotalZhu = awardResults[7];
            }

            if (_mySuperLottoNumber.awardType >= AwardType.NineAward)
            {
                _mySuperLottoNumber.nineAwardTotalZhu = awardResults[8];
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
            int winRedDanCount = 0;
            //红球拖的中奖位数
            int winRedTuoCount = 0;
            //蓝球胆的中奖位数
            int winBlueDanCount = 0;
            //蓝球拖的中奖位数
            int winBlueTuoCount = 0;

            _mySuperLottoNumber.ResetTotalZhuToDefault();

            int redWinPrizesIndex = 0;
            //比较红球胆中的数量
            for (int i = 0; i < _publicSuperLottoNumber.redBalls.Length; i++)
            {
                if (_mySuperLottoNumber.redBalls.Contains(_publicSuperLottoNumber.redBalls[i]))
                {
                    winRedDanCount++;
                    //记录中奖的红球胆号码
                    _mySuperLottoNumber.maxRedWinPrizes[redWinPrizesIndex++] = _publicSuperLottoNumber.redBalls[i];
                }
            }

            int pickRedTuoCount = 5 - _mySuperLottoNumber.redBallDanCount;

            //比较红球拖中的数量
            for (int i = 0; i < _publicSuperLottoNumber.redBalls.Length; i++)
            {
                if (_mySuperLottoNumber.redBallTuos.Contains(_publicSuperLottoNumber.redBalls[i]))
                {
                    winRedTuoCount++;
                    //记录中奖的红球拖号码
                    _mySuperLottoNumber.maxRedWinPrizes[redWinPrizesIndex++] = _publicSuperLottoNumber.redBalls[i];

                    if (winRedTuoCount >= pickRedTuoCount)
                    {
                        break;
                    }
                }
            }

            int blueWinPrizesIndex = 0;
            int pickBlueTuoCount = 2 - _mySuperLottoNumber.blueBallDanCount;

            winBlueDanCount = _mySuperLottoNumber.blueBalls.Contains(_publicSuperLottoNumber.blueBalls[0]) ? 1 : 0;

            if (winBlueDanCount > 0)
            {
                _mySuperLottoNumber.maxBlueWinPrize[blueWinPrizesIndex++] = _publicSuperLottoNumber.blueBalls[0];
            }
            
            //比较蓝球拖中的数量
            for (int i = 0; i < _publicSuperLottoNumber.blueBalls.Length; i++)
            {
                if (_mySuperLottoNumber.blueBallTuos.Contains(_publicSuperLottoNumber.blueBalls[i]))
                {
                    winBlueTuoCount++;
                    _mySuperLottoNumber.maxBlueWinPrize[blueWinPrizesIndex++] = _publicSuperLottoNumber.blueBalls[i];

                    if (winBlueTuoCount >= pickBlueTuoCount)
                    {
                        break;
                    }
                }
            }

            //中的红球总和
            int winRedTotalCount = winRedDanCount + winRedTuoCount;
            //中的蓝球总和
            int winBlueTotalCount = winBlueDanCount + winBlueTuoCount;
            //设置该奖的最高奖等级
            SettingSuperLottoAwardLevel(_mySuperLottoNumber, winRedTotalCount, winBlueTotalCount);
            //若中奖了统计该注号码各奖的金额
            if (_mySuperLottoNumber.awardType != AwardType.NotAward)
            {
                CalculateDantuoWinningAmount(
                    _mySuperLottoNumber,
                    _mySuperLottoNumber.redBallDanCount,
                    _mySuperLottoNumber.redBallTuoCount,
                    _mySuperLottoNumber.blueBallDanCount,
                    _mySuperLottoNumber.blueBallTuoCount,
                    winRedDanCount,
                    winRedTuoCount,
                    winBlueDanCount,
                    winBlueTuoCount);
            }
        }

        /// <summary>
        /// 统计胆拖号码中奖注数
        /// </summary>
        /// <param name="_mySuperLottoNumber">我的胆拖号码</param>
        /// <param name="redDanCount">红球胆总数量</param>
        /// <param name="redTuoCount">红球拖总数量</param>
        /// <param name="blueDanCount">蓝球胆总数量</param>
        /// <param name="blueTuoCount">蓝球拖总数量</param>
        /// <param name="winRedDanCount">中红球胆的数量</param>
        /// <param name="winRedTuoCount">中红球拖的数量</param>
        /// <param name="winBlueDanCount">中蓝球球胆的数量</param>
        /// <param name="winBlueTuoCount">中蓝球拖的数量</param>
        /// <returns></returns>
        public void CalculateDantuoWinningAmount(DantuoSuperLottoNumber _mySuperLottoNumber,
            int redDanCount, int redTuoCount, int blueDanCount, int blueTuoCount,
            int winRedDanCount, int winRedTuoCount, int winBlueDanCount, int winBlueTuoCount)
        {

            var thatRed = winRedTuoCount <= 5 - redDanCount ? winRedTuoCount : 5 - redDanCount;
            var thatBlue = winBlueTuoCount <= 2 - blueDanCount ? winBlueTuoCount : 2 - blueDanCount;

            int[] redCond = new int[thatRed + 1];
            int[] blueCond = new int[thatBlue + 1];

            for (var i = 0; i <= thatRed; i++)
                redCond[i] = winRedDanCount + i;

            for (var i = 0; i <= thatBlue; i++)
                blueCond[i] = winBlueDanCount + i;

            int rankRes = 0;
            int[] awardResults = new int[9];

            for (var i = 0; i <= thatRed; i++)
            {
                for (var v = 0; v <= thatBlue; v++)
                {
                    rankRes = Rank9x(redCond[i], blueCond[v]);
                    if (rankRes != -1)
                    {
                        awardResults[rankRes] += Ccombo(redTuoCount - winRedTuoCount, 5 - redDanCount - i)
                            * Ccombo(winRedTuoCount, i)
                            * Ccombo(blueTuoCount - winBlueTuoCount, 2 - blueDanCount - v)
                            * Ccombo(winBlueTuoCount, v);
                    }
                }
            }

            SettingDanTuoWinningCount(_mySuperLottoNumber, awardResults);
        }

        /// <summary>
        /// 计算排列组合
        /// </summary>
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
