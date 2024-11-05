using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperLotto.Model
{
    /// <summary>
    /// 大乐透中奖类型
    ///   几等奖 => 该奖的奖金
    /// </summary>
    public enum AwardType
    {
        //一等奖默认1000万~ 二等奖默认30万
        Null = -1, NotAward = 0, OneAward = 10000000, TwoAward = 300000, ThreeAward = 10000, FourAward = 3000, FiveAward = 300, SixAward = 200, SevenAward = 100, EightAward = 15, NineAward = 5
    }

    /// <summary>
    /// 大乐透的父类
    /// </summary>
    public class SuperLottos
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int serialNumber { get; set; }
        /// <summary>
        /// 大乐透的红球区号码数量
        /// </summary>
        public const int REDCOUNT = 5;
        /// <summary>
        /// 该注中奖(最高奖)情况
        /// </summary>
        public AwardType awardType { get; set; }
        /// <summary>
        /// 红色球号码
        /// </summary>
        public int[] redBalls { get; set; }
        /// <summary>
        /// 蓝色球号码 
        /// </summary>
        public int[] blueBalls { get; set; }

        public SuperLottos() { }
        public SuperLottos(AwardType awardType) => this.awardType = awardType;

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
        /// 比对该注大乐透中奖信息
        /// </summary>
        /// <param name="dbt">大乐透辅助类</param>
        /// <param name="sdbn">开奖的单式大乐透号</param>
        public virtual void ComparisonSuperLottoNumber(SuperLottoTool dbt, SimplexSuperLottoNumber sdbn) { }

        /// <summary>
        /// 复试该大乐透的值
        /// </summary>
        public virtual void CopyLeftToRightDataValue(SimplexSuperLottoNumber sdbn) => throw new MissingMethodException();
    }
}
