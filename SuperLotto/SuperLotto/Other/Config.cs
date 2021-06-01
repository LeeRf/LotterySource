using SuperLotto.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperLotto.Other
{
    public static class Config
    {
        /// <summary>
        /// 设置类
        /// </summary>
        public static Setting Setting;
        /// <summary>
        /// 记录购买习惯用的的总期数
        /// </summary>
        public static long RecordPeriods = 0;
        /// <summary>
        /// 购买习惯-间隔期数
        /// </summary>
        public static int IntervalPeriods = 5;
        /// <summary>
        /// 记录最近100期大乐透历史开奖号码
        /// </summary>
        public static Queue<SimplexSuperLottoNumber> _historyPublicSuperLottoNumbers;
    }
}
