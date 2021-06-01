using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleBalls.Model
{
    /// <summary>
    /// 双色球中奖类型
    ///   几等奖 => 该奖的奖金
    /// </summary>
    public enum AwardType
    {
        //一等奖默认600万~ 二等奖默认30万
        Null = -1, NotAward = 0, OneAward = 6000000, TwoAward = 300000, ThreeAward = 3000, FourAward = 200, FiveAward = 10, SixAward = 5
    }

    /// <summary>
    /// 双色球的父类
    /// </summary>
    public class DoubleBall
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int serialNumber { get; set; }
        /// <summary>
        /// 双色球的红球区号码数量
        /// </summary>
        public const int REDCOUNT = 6;
        /// <summary>
        /// 该注中奖(最高奖)情况
        /// </summary>
        public AwardType awardType { get; set; }
        /// <summary>
        /// 红色球号码
        /// </summary>
        public int[] redBalls { get; set; }

        public DoubleBall() { }
        public DoubleBall(AwardType awardType) => this.awardType = awardType;

        /// <summary>
        /// 是否是空号
        /// </summary>
        /// <returns></returns>
        public virtual bool isBallEmpty() => throw new MissingMethodException();

        /// <summary>
        /// 号码是否成型
        /// </summary>
        /// <returns></returns>
        public virtual bool IsOKNumber() => throw new MissingMethodException();

        /// <summary>
        /// 获取该注的中奖注数
        /// </summary>
        public virtual int GetTotalWinningTotalZhu() => throw new MissingMethodException();

        /// <summary>
        /// 获取该注未中奖的注数
        /// </summary>
        public virtual int GetNotLotteryTotalZhu() => throw new MissingMethodException();

        /// <summary>
        /// 设定指定奖的获奖总数
        /// </summary>
        public virtual void SettingAwardTypeTotalZhu(LoopDataSummarizing loopData) => throw new MissingMethodException();

        /// <summary>
        /// 获取该注中将的金额
        /// </summary>
        public virtual long GetTotalWinningMoney(int multiple) => throw new MissingMethodException();

        /// <summary>
        /// 是否是Null奖或者未中奖
        /// </summary>
        public bool isEmptyAward() => awardType == AwardType.NotAward || awardType == AwardType.Null;

        /// <summary>
        /// 比对该注双色球中奖信息
        /// </summary>
        /// <param name="dbt">双色球辅助类</param>
        /// <param name="doubleBall">开奖的单式双色球号</param>
        public virtual void ComparisonDoubleBallNumber(DoubleBallTool dbt, SimplexDoubleBallNumber sdbn) { }

        /// <summary>
        /// 复试该双色球的值
        /// </summary>
        public virtual void CopyLeftToRightDataValue(SimplexDoubleBallNumber sdbn) => throw new MissingMethodException();
    }
}
