using SuperLotto.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperLotto.Other
{
    public static class Info
    {
        /// <summary>
        /// 倍
        /// </summary>
        public static readonly string Multiple = "倍";
        /// <summary>
        /// 数据更新中
        /// </summary>
        public static readonly string RefreshData = "数据更新中";
        /// <summary>
        /// 操作提示
        /// </summary>
        public static readonly string MessageTitle = "操作提示";
        /// <summary>
        ///共需摇奖 ({0})
        /// </summary>
        public static readonly string LoopYearMessage = "共需大概 ({0})";
        /// <summary>
        /// 应用设置成功
        /// </summary>
        public static readonly string SaveSettingSuccess = "应用设置成功";
        /// <summary>
        /// 恢复默认设置成功
        /// </summary>
        public static readonly string RestoreDefaultSetting = "恢复默认设置成功";
        /// <summary>
        /// 操作失败
        /// </summary>
        public static readonly string OperatioFail = "操作失败!";
        /// <summary>
        /// 守号摇奖中、请稍后...
        /// </summary>
        public static readonly string LoopRunLotteryIng = "守号摇奖中、请稍后...";
        /// <summary>
        /// 请先从机选或者自选中选择我的大乐透号码!
        /// </summary>
        public static readonly string MyNumberIsEmpty = "请先从【机选、自选、胆拖】页中选择我的大乐透号码!";
        /// <summary>
        /// 很遗憾、经过 {0} 年的购买摇奖依然并未中{1}
        /// </summary>
        public static readonly string NotExpectationReward = "很遗憾、经过 {0} 年的购买摇奖依然并未中{1}";
        /// <summary>
        /// 最多允许机选的大乐透号码为_MaxSuperLottoCount!
        /// </summary>
        public static readonly string RestrictMaxRandomSuperLotto = "界面最大性能、限制最多生成机选的大乐透为 [" + SuperLottoView._MaxSimplexSuperLottoCount + "] 注";
        /// <summary>
        /// 限制最高可选择的复试大乐透注数为 [max] 注
        /// </summary>
        public static readonly string ComplexMaxNumber = "限制最高可选择的复试大乐透注数为 [" + SuperLottoView._ComplexSuperLottoSuperLottoCount + "] 注";
        /// <summary>
        /// 由于计算量过大,为了发挥CPU最大算力,界面来不及重绘将会无响应假死(无视等待即可),整个过程将会持续 半分 ~ 两分钟（不同电脑配置时间不同）、继续吗?
        /// </summary>
        public static readonly string LoopRunLotteryMessage1 = "由于计算量过大,为了发挥CPU最佳计算性能,界面来不及重绘将会无响应假死(无视等待即可),整个过程将会持续 半分 ~ 两分钟（不同电脑配置时间不同）、继续吗?";
        /// <summary>
        /// 由于计算量过大,为了发挥CPU最大算力、会去除动画过渡步骤,界面来不及重绘将会出现假死(无视等待即可),整个过程将会持续 1 ~ 5 分钟，继续吗?
        /// </summary>
        public static readonly string LoopRunLotteryMessage = "由于计算量过大,为了发挥CPU最佳计算性能、会去除动画过渡步骤,界面来不及重绘将会无响应假死(无视等待即可),整个过程将会持续 半分 ~ 两分钟（不同电脑配置时间不同）、继续吗?";
        /// <summary>
        /// {0}期开奖分析：
        /// </summary>
        public static readonly string RandomLotteryNumberPeriods = "{0}期开奖分析：";
        /// <summary>
        /// 感谢您为公益事业捐献了{0}元
        /// </summary>
        public static readonly string PublicBenefitTotalMoney = "感谢您为公益事业捐献了{0}元";
        /// <summary>
        /// "年", "天"
        /// </summary>
        public static readonly string[] YearOrDay = {"年", "天"};
        /// <summary>
        /// {0}中得该奖项
        /// </summary>
        public static readonly string LoopPeriodsMessage = "{0}中得该奖项";
        /// <summary>
        /// [{0}]{1}
        /// </summary>
        public static readonly string FormatLottery = "{0}> {1}";
        /// <summary>
        /// （{0}）{1}（期）
        /// </summary>
        public static readonly string LotteryPeriodsCountMessage = "（{0}）{1}（期）";
        /// <summary>
        /// （{0}）{1}（元）
        /// </summary>
        public static readonly string LotteryMoneyConutMessage = "（{0}）{1}（元）";
        /// <summary>
        /// （{0}）{1}（注）
        /// </summary>
        public static readonly string LotteryZhuCountMessage = "（{0}）{1}（注）";
        /// <summary>
        /// 红球已经达到最大的数量
        /// </summary>
        public static readonly string RedBallAlreadyMaxCount = "红色球已经达到最大的数量";
        /// <summary>
        /// 蓝色球已经达到最大的数量
        /// </summary>
        public static readonly string BlueBallAlreadyMaxCount = "蓝色球已经达到最大的数量";
        /// <summary>
        /// 红球胆号最多可选择4个号码
        /// </summary>
        public static readonly string RedBallDanMaxCount = "红球胆号最多可选择4个号码";
        /// <summary>
        /// 红球拖号最多可选择20个号码
        /// </summary>
        public static readonly string RedBallTuoMaxCount = "红球拖号最多可选择20个号码";
        /// <summary>
        /// 蓝球胆号最多可选择1个号码
        /// </summary>
        public static readonly string BlueBallDanMaxCount = "篮球胆号最多可选择1个号码";
        /// <summary>
        /// 蓝球拖号最多可选择11个号码
        /// </summary>
        public static readonly string BlueBallTuoMaxCount = "红球拖号最多可选择11个号码";
        /// <summary>
        /// 胆号和拖号的数量相加至少大等于7
        /// </summary>
        public static readonly string DantuoCountInsufficient = "胆号和拖号的数量相加至少大等于7";
        /// <summary>
        /// 红球胆、拖的数量至少大等于5
        /// </summary>
        public static readonly string DantuoRedCountInsufficient = "红球胆、拖的数量至少大等于5";
        /// <summary>
        /// 请先将未完成的自选号码选择完毕或者删除！
        /// </summary>
        public static readonly string UnfinishedOneselfNumber = "请先将未完成的自选号码选择完毕或者删除！";
        /// <summary>
        /// 相当于{0}购买
        /// </summary>
        public static readonly string GuardBuyMessage = "相当于{0}购买";
        /// <summary>
        /// 不固定习惯、后面的期数需大于前面的期数
        /// </summary>
        public static readonly string ApplySettingIniterval = "不固定习惯 - 后面的期数需大于前面的期数";
        /// <summary>
        /// 当前复试组合已不可再选、将会超过最高限制注数：(100万注)
        /// </summary>
        public static readonly string AnticipateTotalZhu = "当前复试组合已不可再选、将会超过最高限制注数：(100万注)";
        /// <summary>
        /// 已达到最高可选的注数：(100万注) | 或者已经达到最大的号码总数：
        /// </summary>
        public static readonly string OneselfRestrict = "已达到最高可选的注数：(100万注) | 或者已经达到最大的号码总数：" + SuperLottoView._MaxSerialNumberTotal;

        /// <summary>
        /// 获取循环开奖消息提示
        /// </summary>
        public static string GetLoopDataAnalyseInfo(Setting setting)
        {
            StringBuilder builder = new StringBuilder();

            if (setting.BuyHabit == BuyHabit.EveryPeriod)
            {
                builder.Append("每期");
            }
            else if (setting.BuyHabit == BuyHabit.WeeklyPeriod)
            {
                builder.Append("每周");
            }
            else if (setting.BuyHabit == BuyHabit.MonthlyPeriod)
            {
                builder.Append("每月");
            }
            else
            {
                if (setting.CustomizeModeType == CustomizeModeType.FixedHabit)
                {
                    builder.Append(setting.FixedPeriod == 1 ? "每期" : "每隔" + setting.FixedPeriod + "期");
                }
                else
                {
                    builder.Append("每隔" + setting.UncertaintyStartPeriod + " ~ " + setting.UncertaintyEndPeriod + "期");
                }
            }

            if (setting.myNumberType == MyNumberType.AlreadySelect)
            {
                builder.Append("守号");
            }
            else
            {
                builder.Append("随机");
            }

            return string.Format(GuardBuyMessage, builder);
        }

        /// <summary>
        /// 获取数值最大单位中文描述
        /// </summary>
        public static string GetNumberMaxUnit(long number)
        {
            int numberLength = number.ToString().Length;

            if (number == 0)
            {
                return "零";
            }
            else if (numberLength == 1)
            {
                return "个";
            }
            else if (numberLength == 2)
            {
                return "十";
            }
            else if (numberLength == 3)
            {
                return "百";
            }
            else if (numberLength == 4)
            {
                return "千";
            }
            else if (numberLength == 5)
            {
                return "万";
            }
            else if (numberLength == 6)
            {
                return "十万";
            }
            else if (numberLength == 7)
            {
                return "百万";
            }
            else if (numberLength == 8)
            {
                return "千万";
            }
            else if (numberLength == 9)
            {
                return "亿";
            }
            else if (numberLength == 10)
            {
                return "十亿";
            }
            else if (numberLength == 11)
            {
                return "百亿";
            }
            else if (numberLength == 12)
            {
                return "千亿";
            }
            else if (numberLength == 13)
            {
                return "万亿";
            }
            else if (numberLength == 14)
            {
                return "You Crazy. 马斯克都不敢这么捐！";
            }
            else if (numberLength == 15)
            {
                return "歇歇吧慈善家地球上资产都被你捐光了";
            }
            else if(numberLength == 16)
            {
                return "有个大胆的想法.干脆把地球也捐了把";
            }
            else
            {
                return "ok、fine、宇宙归你了";
            }
        }

        /// <summary>
        /// 获取该奖的中文描述
        /// </summary>
        public static string GetLotteryChineseMessage(AwardType awardType)
        {
            if (awardType == AwardType.NotAward)
            {
                return "未中奖";
            }
            else if (awardType == AwardType.NineAward)
            {
                return "九等奖";
            }
            else if (awardType == AwardType.EightAward)
            {
                return "八等奖";
            }
            else if (awardType == AwardType.SevenAward)
            {
                return "七等奖";
            }
            else if (awardType == AwardType.SixAward)
            {
                return "六等奖";
            }
            else if (awardType == AwardType.FiveAward)
            {
                return "五等奖";
            }
            else if (awardType == AwardType.FourAward)
            {
                return "四等奖";
            }
            else if (awardType == AwardType.ThreeAward)
            {
                return "三等奖";
            }
            else if (awardType == AwardType.TwoAward)
            {
                return "二等奖";
            }
            else if (awardType == AwardType.OneAward)
            {
                return "一等奖";
            }
            else
            {
                return "null";
            }
        }

        public static void ShowInfoMessage(string message)
        {
            MessageBox.Show(message, MessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, MessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ShowWarningMessage(string message)
        {
            MessageBox.Show(message, MessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static DialogResult ShowQuestionMessage(string message)
        {
            return MessageBox.Show(message, MessageTitle, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
        }
    }
}