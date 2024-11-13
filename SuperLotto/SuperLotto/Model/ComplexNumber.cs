using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperLotto.Model
{
    /// <summary>
    /// 复试号码类：复试以及胆拖都是复试
    /// </summary>
    public class ComplexNumber : SuperLottos
    {
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

        public ComplexNumber() { }
        public ComplexNumber(AwardType initAward) : base(initAward) { }

        /// <summary>
        /// 根据获奖类型取得该类型的注数
        /// </summary>
        /// <param name="_myNumber"></param>
        /// <returns></returns>
        public override int getWinCount()
        {
            /*if (this is SimplexSuperLottoNumber)
                throw new MissingMethodException("class 'SimplexSuperLottoNumber' cannot call this method: getWinCountBy()");

            ComplexNumber this = this as ComplexNumber;*/

            if (this.awardType == AwardType.OneAward)
            {
                return this.oneAwardTotalZhu;
            }
            else if (this.awardType == AwardType.TwoAward)
            {
                return this.twoAwardTotalZhu;
            }
            else if (this.awardType == AwardType.ThreeAward)
            {
                return this.threeAwardTotalZhu;
            }
            else if (this.awardType == AwardType.FourAward)
            {
                return this.fourAwardTotalZhu;
            }
            else if (this.awardType == AwardType.FiveAward)
            {
                return this.fiveAwardTotalZhu;
            }
            else if (this.awardType == AwardType.SixAward)
            {
                return this.sixAwardTotalZhu;
            }
            else if (this.awardType == AwardType.SevenAward)
            {
                return this.sevenAwardTotalZhu;
            }
            else if (this.awardType == AwardType.EightAward)
            {
                return this.eightAwardTotalZhu;
            }
            else if (this.awardType == AwardType.NineAward)
            {
                return this.nineAwardTotalZhu;
            }

            return 0;
        }

        /// <summary>
        /// 该奖级是否包含中奖注数
        /// </summary>
        /// <param name="_myNumber"></param>
        /// <returns></returns>
        public override bool hasWinCount()
        {
            return getWinCount() > 0;
        }
    }
}
