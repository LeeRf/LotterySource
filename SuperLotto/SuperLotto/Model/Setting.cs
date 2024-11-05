using SuperLotto.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperLotto.Model
{
    /// <summary>
    /// 我的开奖号码类型
    /// </summary>
    public enum MyNumberType
    {
        //   已选              随机生成
        AlreadySelect = 0, RandomNumber = 1
    }

    /// <summary>
    /// 购买习惯
    /// </summary>
    public enum BuyHabit
    {
        EveryPeriod = 1, WeeklyPeriod = 3, MonthlyPeriod = 13, CustomizeMode = 15
    }

    /// <summary>
    /// 自定义习惯类型
    /// </summary>
    public enum CustomizeModeType
    {
        FixedHabit = 0, UncertaintyHabit = 1
    }

    /// <summary>
    /// 程序设置类
    /// </summary>
    public class Setting
    {
        private const int ONEMAXAWARD = 10000000;
        private const int ONEMINAWARD = 3000000;
        private const int TWOMAXAWARD = 1000000;
        private const int TWOMINAWARD = 100000;

        /// <summary>
        /// 配置文件路径和名称
        /// </summary>
        private static string SettingFilePath = Application.StartupPath + @"\Setting.ini";
        /// <summary>
        /// 产生摇奖号码时、比对中奖信息按照
        /// </summary>
        public MyNumberType myNumberType { get; set; } = MyNumberType.AlreadySelect;
        /// <summary>
        /// 随机摇奖单期生成我的随机号注数
        /// </summary>
        public int RandomGenerateCount { get; set; } = 1;
        /// <summary>
        /// 我的购买习惯
        /// </summary>
        public BuyHabit BuyHabit { get; set; } = BuyHabit.EveryPeriod;
        /// <summary>
        /// 自定义购买习惯类型
        /// </summary>
        public CustomizeModeType CustomizeModeType { get; set; } = CustomizeModeType.FixedHabit;
        /// <summary>
        /// 固定习惯期数
        /// </summary>
        public int FixedPeriod { get; set; } = 1;
        /// <summary>
        /// 不固定期数开始
        /// </summary>
        public int UncertaintyStartPeriod { get; set; } = 1;
        /// <summary>
        /// 不固定期数结束
        /// </summary>
        public int UncertaintyEndPeriod { get; set; } = 5;

        /// <summary>
        /// 循环摇奖停止条件
        /// </summary>
        public int LoopStopCondition { get; set; }
        /// <summary>
        /// 单注追加倍数
        /// </summary>
        public int ZhuMultiple { get; set; }
        /// <summary>
        /// 一等奖奖金金额
        /// </summary>
        public long OneAward { get; set; } = 10000000;
        /// <summary>
        /// 二等奖奖金金额
        /// </summary>
        public long TwoAward { get; set; } = 300000;
        /// <summary>
        /// 循环守号和随机守号的期数
        /// </summary>
        public int CustomizePeriods { get; set; } = 156;

        /// <summary>
        /// 保存程序设置
        /// </summary>
        /// <returns></returns>
        public bool SaveSetting()
        {
            return IniHelper.SaveOrUpdateIniData(this, SettingFilePath);
        }

        /// <summary>
        /// 获取一等奖金额
        /// </summary>
        /// <returns></returns>
        public long GetOneAward()
        {
            if (OneAward > ONEMAXAWARD) OneAward = ONEMAXAWARD;
            if (OneAward < ONEMINAWARD) OneAward = ONEMINAWARD;

            return OneAward;
        }

        /// <summary>
        /// 获取二等奖金额
        /// </summary>
        public long GetTwoAward()
        {
            if (TwoAward > TWOMAXAWARD) TwoAward = TWOMAXAWARD;
            if (TwoAward < TWOMINAWARD) TwoAward = TWOMINAWARD;

            return TwoAward;
        }

        /// <summary>
        /// 读取配置文件信息
        /// </summary>
        public static Setting LoadSetting()
        {
            if (File.Exists(SettingFilePath))
            {
                return IniHelper.ReadIniData<Setting>(SettingFilePath) as Setting;
            }
            return new Setting();
        }

        /// <summary>
        /// 恢复默认设置
        /// </summary>
        public static bool RestoreDefaultSetting(Setting setting)
        {
            if (File.Exists(SettingFilePath))
            {
                return IniHelper.SaveOrUpdateIniData(setting, SettingFilePath);
            }
            return true;
        }
    }
}
