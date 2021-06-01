using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DoubleBalls.Other
{
    public static class MyRandom
    {
        private static Random random = new Random();
        //只生成开奖随机数
        private static RNGCryptoServiceProvider runLotteryRng = new RNGCryptoServiceProvider();
        //只生成我的随机机选号
        private static RNGCryptoServiceProvider myRandomRng = new RNGCryptoServiceProvider();

        /// <summary>
        /// 其它项的随机数值生成 大等于 min 小于 max
        /// </summary>
        public static int GetRandomNum(int min, int max)
        {
            return random.Next(min, max);
        }

        /// <summary>
        /// 生成我的号码随机函数 大等于 min 小等于 max
        /// </summary>
        public static int GetMyRandom(int minVal, int maxVal)
        {
            int m = maxVal - minVal + 1;
            int rnd = int.MinValue;
            decimal _base = long.MaxValue;
            byte[] rndSeries = new byte[8];
            myRandomRng.GetBytes(rndSeries);
            long l = BitConverter.ToInt64(rndSeries, 0);
            rnd = (int)(Math.Abs(l) / _base * m);
            return minVal + rnd;
        }

        /// <summary>
        /// 生成公开开奖号码随机函数
        /// </summary>
        public static int GetRunPublicRandom(int minVal, int maxVal)
        {
            int m = maxVal - minVal + 1;
            int rnd = int.MinValue;
            decimal _base = long.MaxValue;
            byte[] rndSeries = new byte[8];
            runLotteryRng.GetBytes(rndSeries);
            long l = BitConverter.ToInt64(rndSeries, 0);
            rnd = (int)(Math.Abs(l) / _base * m);
            return minVal + rnd;
        }
    }
}
