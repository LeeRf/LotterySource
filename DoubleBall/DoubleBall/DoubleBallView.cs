using DoubleBalls.Controls;
using DoubleBalls.Data;
using DoubleBalls.Model;
using DoubleBalls.Other;
using DoubleBalls.Style;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DoubleBalls
{
    public delegate void RefreshResizeEventHandler();
    public delegate void RefreshMoveLocationEventHandler();

    public partial class DoubleBallView : SkinMain
    {
        private Point _mPoint;
        /// <summary>
        /// 双色球类工具类
        /// </summary>
        public DoubleBallTool _doubleBallToo;
        /// <summary>
        /// 双色球公共开奖号码
        /// </summary>
        private SimplexDoubleBallNumber _publicDoubleNumber;
        /// <summary>
        /// 本次是否出现历史最高奖
        /// </summary>
        private bool _appearMaxNumber;
        /// <summary>
        /// 记录我的历史最高中奖的号码
        /// </summary>
        private SimplexDoubleBallNumber _historyMaxDoubleBallNumber;
        /// <summary>
        /// 记录我的历史最高奖号码对应的那期公开开奖号码
        ///    [用于随机五年、十年摇奖后对我的最高奖中奖号进行中奖上色]
        /// </summary>
        private SimplexDoubleBallNumber _historyMaxPublicDoubleBallNumber;

        /// <summary>
        /// 当机选双色球达到该数量时、排序用动画来过渡
        /// </summary>
        private const int _CartoonCount = 80;
        /// <summary>
        /// 最大机选单式双色球的数量
        /// </summary>
        public static readonly int _MaxSimplexDoubleBallCount = 300;
        /// <summary>
        /// 单式双色球我的机选号集合
        /// </summary>
        private List<SimplexDoubleBallNumber> _mySimplexDoubleBallNumberList;

        public event RefreshResizeEventHandler RefreshResize;
        public event RefreshMoveLocationEventHandler RefreshMoveLocation;
        public event ExitLoopWaitEventHandler ExitLoopWaitEvent;

        public DoubleBallView() { InitializeComponent(); }

        private void DoubleBall_Load(object sender, EventArgs e)
        {
            #region Control Initialization Selection

            WinSize = true;

            for (int i = 1; i < 100; i++)
            {
                cmbMultiple.Items.Add(FormatNumber(i.ToString(), 3));
            }

            cmbCPattern.SelectedIndex = 0;
            cmbRedComplexCount.SelectedIndex = 0;
            cmbBlueComplexCount.SelectedIndex = 0;
            cmbDPattern.SelectedIndex = 0;
            cmbRedDanCount.SelectedIndex = 0;
            cmbRedTuoCount.SelectedIndex = 0;
            cmbBlueDanCount.SelectedIndex = 0;
            cmbMultiple.SelectedIndex = 0;
            cmbStopCondition.SelectedIndex = 5;

            _redBallLabelStyles[0] = lblRedBallA;
            _redBallLabelStyles[1] = lblRedBallB;
            _redBallLabelStyles[2] = lblRedBallC;
            _redBallLabelStyles[3] = lblRedBallD;
            _redBallLabelStyles[4] = lblRedBallE;
            _redBallLabelStyles[5] = lblRedBallF;

            panTop.MouseMove += (se, ev) => OffSetIt(ev);
            panTop.MouseDown += (se, ev) => { _mPoint.X = ev.X; _mPoint.Y = ev.Y; };

            #endregion

            #region Initialize the new instance

            LoadSettingApply();
            LoadDefaultColor();

            ExitLoopWaitEvent += ExitThatLoop;

            _doubleBallToo = new DoubleBallTool();
            _publicDoubleNumber = new SimplexDoubleBallNumber();

            _ComplexHelper = new OneselfControlHelper(flpComplexNumber);
            _DantuoHelper = new OneselfControlHelper(flpDantuoNumber);

            _mySimplexDoubleBallNumberList = new List<SimplexDoubleBallNumber>();
            _myComplexDoubleBallNumberList = new List<ComplexDoubleBallNumber>();
            _myDantuoDoubleBallNumberList = new List<DantuoDoubleBallNumber>();

            _historyMaxDoubleBallNumber = new SimplexDoubleBallNumber(AwardType.NotAward);
            _maxLoopDoubleBallNumber = new SimplexDoubleBallNumber(AwardType.NotAward);

            _historyMaxPublicDoubleBallNumber = new SimplexDoubleBallNumber(AwardType.NotAward);
            _maxLoopLotteryBoubleBallNumber = new SimplexDoubleBallNumber(AwardType.NotAward);

            Config._historyPublicDoubleBallNumbers = new Queue<SimplexDoubleBallNumber>(100);

            #endregion

            #region Shown Event

            isLoad = false;
            lblDate.Text = DateTime.Now.ToString("yyyy.MM.dd");

            Shown += (o, args) =>
            {
                Red1.Visible = true;
                Red2.Visible = true;
                Red3.Visible = true;
                Red4.Visible = true;
                Red5.Visible = true;
                Red6.Visible = true;
                Blue1.Visible = true;
                lblLengthways.Visible = true;

                SoftwareExplain.ReadOnly = true;
                cmbStopCondition.Visible = true;
                cmbMultiple.Visible = true;

                foreach (Control item in panLotteryRules.Controls)
                {
                    if (!item.Visible) item.Visible = true;
                }

                LoopDataAnalyse.SetWindowRegion(panBody, 50);
            };

            Logger.Info("is running.");
            this.BackColor = Color.FromArgb(Config.Setting.BackColorArgb);
            SoftwareExplain.LoadFile(Application.StartupPath + @"\explain.data");

            #endregion
        }

        /// <summary>
        /// 鼠标移动方法
        /// </summary>
        /// <param name="ev"></param>
        private void OffSetIt(MouseEventArgs ev)
        {
            if (ev.Button == MouseButtons.Left)
            {
                Point myPosition = MousePosition;
                myPosition.Offset(-_mPoint.X, -_mPoint.Y);
                Location = myPosition;
            }
        }

        /// <summary>
        /// [主页]按钮：产生开奖的双色球号码
        /// </summary>
        private void btnCreateNo_Click(object sender, EventArgs e)
        {
            if (WhetherMeetsCriteria())
            {
                switch (CardInterface.SelectedIndex)
                {
                    //机选页
                    case 1:

                        if (UpdatePeriodsAndShowPublicNumber())
                        {
                            if (rbEveryRandom.Checked) RandomlyGenerateMyLotteryNumberAndRedrawLabel(CardInterface.SelectedIndex);

                            ComprehensiveLogicOfLotteryDrawing(_mySimplexDoubleBallNumberList.Cast<DoubleBall>().ToList(), flpSimplexNumber);
                        }

                        break;

                    //自选&复式
                    case 2:

                        if (UpdatePeriodsAndShowPublicNumber())
                        {
                            if (rbEveryRandom.Checked) RandomlyGenerateMyLotteryNumberAndRedrawLabel(CardInterface.SelectedIndex);

                            ComprehensiveLogicOfLotteryDrawing(_myComplexDoubleBallNumberList.Cast<DoubleBall>().ToList(), flpComplexNumber);
                        }

                        break;
                        
                    //自选&胆拖
                    case 3:

                        int redDanCount, redTuoCount;

                        if (rbEveryRandom.Checked && cmbRedDanCount.SelectedIndex != 0 && cmbRedTuoCount.SelectedIndex != 0)
                        {
                            redDanCount = int.Parse(cmbRedDanCount.Text);
                            redTuoCount = int.Parse(cmbRedTuoCount.Text);

                            if (redDanCount + redTuoCount < 6)
                            {
                                Info.ShowWarningMessage(Info.DantuoCountInsufficient);
                                return;
                            }
                        }

                        if (UpdatePeriodsAndShowPublicNumber())
                        {
                            if (rbEveryRandom.Checked) RandomlyGenerateMyLotteryNumberAndRedrawLabel(CardInterface.SelectedIndex);

                            ComprehensiveLogicOfLotteryDrawing(_myDantuoDoubleBallNumberList.Cast<DoubleBall>().ToList(), flpDantuoNumber);
                        }

                        break;
                }
            }
        }

        /// <summary>
        /// 单式号产生开奖号码的综合逻辑处理
        /// </summary>
        private void ComprehensiveLogicOfLotteryDrawing(List<DoubleBall> _myDoubleBallList, FlowLayoutPanel flpDoubleBallPanel)
        {
            RefreshIntervalPeriods();
            SettingControlParameter();

            long winTotalZhu = 0, winTotalMoney = 0, maxLotteryMoneySerialNumber = 0;

            DoubleBall _thisTimeMaxNumber = new DoubleBall(AwardType.NotAward);

            _myDoubleBallList.ForEach(dbn => dbn.ComparisonDoubleBallNumber(_doubleBallToo, _publicDoubleNumber));

            //循环流控件中的各个Panel(单个Panel里承载6个Label红球、1个蓝球)
            foreach (Panel panel in flpDoubleBallPanel.Controls)
            {
                int serialNumber = ParseTagValueToInt(panel);

                DoubleBall myDoubleBallNumber = _myDoubleBallList.Where(dbn => dbn.serialNumber == serialNumber).ToList()[0];

                //统计中奖注数和累加中奖金额
                if (myDoubleBallNumber.awardType != AwardType.NotAward)
                {
                    winTotalZhu += myDoubleBallNumber.GetTotalWinningTotalZhu();
                    winTotalMoney += myDoubleBallNumber.GetTotalWinningMoney(int.Parse(cmbMultiple.Text));
                }

                //对单个号码(Panel)中奖情况的N个Label进行绘色
                DrawingLabelColorLotteryNumber(panel, myDoubleBallNumber, _publicDoubleNumber);

                /**
                 * 循环比对找到本次机选页记录中第一个最高奖
                 *   并记录和展示最高奖该奖的序列号序号和中文描述
                 */
                if ((int)myDoubleBallNumber.awardType > (int)_thisTimeMaxNumber.awardType)
                {
                    //找到中奖双色球的序列号、排序后不能用辅助类里的
                    foreach (Label label in panel.Controls)
                    {
                        if (label.Tag.Equals("No"))
                        {
                            string viewSerialNumber = label.Text.Replace(">", "");
                            maxLotteryMoneySerialNumber = int.Parse(viewSerialNumber);
                            break;
                        }
                    }

                    _thisTimeMaxNumber = myDoubleBallNumber;
                }

                //循环守号模式下数据统计
                StatisticsLoopLotteryData(myDoubleBallNumber);
            }

            //对比 历史最高奖 和 本次最高奖 并记录
            if ((int)_thisTimeMaxNumber.awardType > (int)_historyMaxDoubleBallNumber.awardType)
            {
                _appearMaxNumber = true;
                _thisTimeMaxNumber.CopyLeftToRightDataValue(_historyMaxDoubleBallNumber);
            }

            //刷新和展示本期数据分析的内容
            RefreshCurrentPeriodMyData(winTotalZhu, winTotalMoney, maxLotteryMoneySerialNumber, _thisTimeMaxNumber.awardType);

            //[综合数据和循环数据] 根据需要刷新综合数据和循环数据
            RefreshRightSynthesisData(GetParseLabelToSyntheticDataSummarizing(), GetParseLabelToLoopDataSummarizing());

            //[循环数据弹出框数据]
            RefreshLoopDataAnalyse(_loopDataSummarizing);
        }

        /// <summary>
        /// 更新期数并产生和显示开奖号码并返回是否可以进行开奖
        /// </summary>
        /// <returns></returns>
        private bool UpdatePeriodsAndShowPublicNumber()
        {
            UpdateDoubleBallPeriods(1);

            _publicDoubleNumber = _doubleBallToo.GetSimplexDoubleBallNumber(false, ParseTagValueToLong(lblDoubleBallPeriods));

            ShowDoubleBallLotteryNumber(_publicDoubleNumber);

            return !(Config.RecordPeriods % Config.IntervalPeriods != 0);
        }

        /// <summary>
        /// 如果是循环摇奖、就统计循环摇奖的数据
        /// </summary>
        private void StatisticsLoopLotteryData(DoubleBall myDoubleBallNumber)
        {
            if (_loopRunLotterysFlag == 0)
            {
                _refreshLoopData = true;
                if ((int)myDoubleBallNumber.awardType >= (int)GetLoopStopCondition())
                {
                    //如果开到期望奖后设置标识退出循环
                    _loopRunLotterysFlag = 1;
                }

                //记录本次循环最高奖信息
                if ((int)myDoubleBallNumber.awardType > (int)_maxLoopDoubleBallNumber.awardType)
                {
                    _appearLoopMaxNumber = true;

                    myDoubleBallNumber.CopyLeftToRightDataValue(_maxLoopDoubleBallNumber);
                }
            }

            //点击了定时器或者找到了退出循环条件但本次依然继续统计本期中奖信息
            if (_loopRunLotterysFlag >= 0)
            {
                //统计循环数据信息[用于展示弹窗]
                StatisticsLoopLotteryAwardCount(myDoubleBallNumber, _loopDataSummarizing, _loopDataAnalyse);
            }
        }

        /// <summary>
        /// [主页] 守号 五年、十年、开奖
        /// </summary>
        private void btnFiveYearsRunLotterys_Click(object sender, EventArgs e)
        {
            if (WhetherMeetsCriteria())
            {
                string buttonType = ((MaterialFlatButton)sender).Name;
                if (buttonType == "btnFiveYearsRunLotterys")
                {
                    Logger.Info("five year fixed number run lottery.");
                }
                else if (buttonType == "btnTenYearsRunLotterys")
                {
                    Logger.Info("ten year fixed number run lottery.");
                }
                else if (buttonType == "btnCustomizeRunLotterys")
                {
                    Logger.Info($"customize {Config.Setting.CustomizePeriods} fixed number run lottery.");
                }

                switch (CardInterface.SelectedIndex)
                {
                    //机选页 & 自选复式页
                    case int index when index <= 3 && index >= 1:

                        LoopDataPreparationAndSettingFlag(long.Parse(((MaterialFlatButton)sender).Tag.ToString()));

                        break;
                }
            }
        }

        /// <summary>
        /// 记录点击守号摇奖时的期数
        /// </summary>
        private long _loopPeriods;
        /// <summary>
        /// 守号循环下更新右侧循环摇奖数据
        /// </summary>
        private bool _refreshLoopData;
        /// <summary>
        /// 守号循环开奖标志 
        ///   -1(初始值关闭定时器)、
        ///    0(点了守号循环摇奖打开定时器)、
        ///    1(找到了退出条件定时器退出、本次继续统计中奖信息)
        /// </summary>
        private int _loopRunLotterysFlag = -1;

        /// <summary>
        /// 循环记录本次是否出现最高奖
        /// </summary>
        private bool _appearLoopMaxNumber;
        /// <summary>
        /// 记录循环摇奖历史 我最高中奖的号码
        /// </summary>
        private SimplexDoubleBallNumber _maxLoopDoubleBallNumber;
        /// <summary>
        /// 记录循环摇奖历史 我最高中奖的号码对应的那期公开开奖号码 
        ///  [用于随机五年、十年摇奖后对我的循环内最高奖中奖号进行中奖上色]
        /// </summary>
        private SimplexDoubleBallNumber _maxLoopLotteryBoubleBallNumber;

        /// <summary>
        /// 循环摇奖弹窗展示对象
        /// </summary>
        private LoopDataAnalyse _loopDataAnalyse;
        /// <summary>
        /// 循环摇奖数据辅助类
        /// </summary>
        private LoopDataSummarizing _loopDataSummarizing;

        /// <summary>
        /// 循环开启前准备数据和设置标识
        /// </summary>
        /// <param name="loopCount">循环摇奖次数</param>
        private void LoopDataPreparationAndSettingFlag(long loopCount)
        {
            ResetLoopRunLotteryData();
            LoadingHelper.ShowLoading(Info.LoopRunLotteryIng, Color.White, ExitLoopWaitEvent, this, o =>
            {
                _loopRunLotterysFlag = 0;
                _loopRunLotteryCount = 0;
                _loopPeriods = ParseTagValueToLong(lblDoubleBallPeriods);

                _loopDataSummarizing = new LoopDataSummarizing();
                _loopDataAnalyse = new LoopDataAnalyse(this, _loopDataSummarizing);

                _loopRunLotteryDelegate = new LoopRunLotteryNumberYearsDelegate(LoopRunLotteryFunction);
                Invoke(_loopRunLotteryDelegate, loopCount);

                /**
                 * 因为这里委托方法开启定时器相当于在外部开启了一个线程
                 * 动画直执行开启定时器代码、不等待定时器模拟摇奖过程、所以这里判断如果摇奖没摇够一直休眠执行动画
                 */
                while (true)
                {
                    if (_loopRunLotterysFlag != -1 && _loopRunLotteryCount < _loopRunLotteryTotalCount)
                    {
                        Thread.Sleep(100);
                    }
                    else
                    {
                        Thread.Sleep(100);
                        break;
                    }
                }
            });
            //展示循环数据分析窗体
            _loopDataAnalyse.ShowDialog();
        }

        /// <summary>
        /// 将循环定时器启动方法
        /// </summary>
        private void LoopRunLotteryFunction(long loopRunLotteryTotalCount)
        {
            _loopRunLotteryTotalCount = loopRunLotteryTotalCount;

            LoopRunLotterysTimer.Enabled = true;
            LoopRunLotterysTimer.Start();
        }

        /// <summary>
        /// 退出循环方法事件
        /// </summary>
        private void ExitThatLoop(bool userExit = false)
        {
            if (userExit)
            {
                Logger.Info($"user exit loop : {_loopRunLotteryCount}，exit condition : {cmbStopCondition.SelectedIndex}");
            }
            else
            {
                string prefixText = "did not meet expectations";
                if (_loopRunLotterysFlag == 1) prefixText = "reach expectations";

                Logger.Info(prefixText + $" loop count : {_loopRunLotteryCount}，exit condition : {cmbStopCondition.SelectedIndex}");
            }

            _loopRunLotterysFlag = -1;
            LoopRunLotterysTimer.Stop();
        }

        /// <summary>
        /// 定时器摇奖次数
        /// </summary>
        private long _loopRunLotteryCount = 0;
        /// <summary>
        /// 循环摇奖总次数
        /// </summary>
        private long _loopRunLotteryTotalCount = 0;

        /// <summary>
        /// 循环五年十年摇奖定时器
        /// </summary>
        private void LoopRunLotterys_Tick(object sender, EventArgs e)
        {
            if(_loopRunLotterysFlag == 1 || _loopRunLotteryCount >= _loopRunLotteryTotalCount)
            {
                //恢复默认状态
                this.ExitThatLoop();
            }
            else
            {
                btnCreateNo_Click(null, null);
            }

            _loopRunLotteryCount++;
        }

        /// <summary>
        /// [主页] 无限守号开奖
        /// </summary>
        private void btnInfiniteLoopRunLottery_Click(object sender, EventArgs e)
        {
            if (WhetherMeetsCriteria())
            {
                Logger.Info("infinite fixed number run lottery.");
                switch (CardInterface.SelectedIndex)
                {
                    //[机选页]、自选复式页] 的无限守号开奖
                    case int index when index <= 3 && index >= 1:
                        //五等奖及以下用动画方案过渡
                        if ((int)GetLoopStopCondition() <= (int) AwardType.FiveAward) LoopDataPreparationAndSettingFlag(long.MaxValue);
                        else
                        {
                            //二等奖及以上用最佳算力方案
                            if (Info.ShowQuestionMessage(Info.LoopRunLotteryMessage) == DialogResult.OK)
                            {
                                if (index == 1)
                                {
                                    LoopComprehensiveLogicOfLotteryDrawing(_mySimplexDoubleBallNumberList.Cast<DoubleBall>().ToList(), flpSimplexNumber, false, 0);
                                }

                                if (index == 2)
                                {
                                    LoopComprehensiveLogicOfLotteryDrawing(_myComplexDoubleBallNumberList.Cast<DoubleBall>().ToList(), flpComplexNumber, false, 0);
                                }

                                if (index == 3)
                                {
                                    LoopComprehensiveLogicOfLotteryDrawing(_myDantuoDoubleBallNumberList.Cast<DoubleBall>().ToList(), flpDantuoNumber, false, 0);
                                }
                            }
                        }

                        break;
                }
            }
        }

        /// <summary>
        /// (随机五年、随机十年、无限随机、无限守号)循环抽奖的综合逻辑
        /// </summary>
        /// <param name="isCreateMyRandomNumber">是否随机生成我的号码</param>
        /// <param name="loopCount">随机摇奖次数</param>
        private void LoopComprehensiveLogicOfLotteryDrawing(List<DoubleBall> _myDoubleBallList, FlowLayoutPanel flpDoubleBallPanel, bool isCreateMyRandomNumber, int loopCount)
        {
            if (!CheckOneselfDantuoNumberCount()) return;

            ResetLoopRunLotteryData();
            SettingControlParameter();

            #region 本期我的消费金额和购买注数来源页

            bool exitLoop = false;
            int redCount = 0, redTuoCount = 0, blueCount = 0;

            long periodCount = 0;
            long startPeriod = ParseTagValueToLong(lblDoubleBallPeriods);

            int selectIndex = CardInterface.SelectedIndex;
            LeeLabel lblthisTimeMoney = null, lblConsumeZhu = null;

            AcquireThisTimeMoneyZhu(selectIndex, ref lblthisTimeMoney, ref lblConsumeZhu);
            DecideMyRandomDataSource(isCreateMyRandomNumber, selectIndex, ref redCount, ref redTuoCount,ref blueCount);

            #endregion

            #region 无限摇奖准备数据项、[本期数据、综合数据、循环数据]

            AwardType stopAwardType = GetLoopStopCondition();

            //本期我的消费金额
            long thisTimeMonetary = ParseTagValueToLong(lblthisTimeMoney);
            //本期我的购买注数
            long thisTimeBuyTotalZhu = ParseTagValueToLong(lblConsumeZhu);

            //循环摇奖数据变量项
            LoopDataSummarizing loopDataSummarizing = new LoopDataSummarizing();
            LoopDataAnalyse loopDataAnalyse = new LoopDataAnalyse(this, loopDataSummarizing);

            //综合数据变量项
            SyntheticDataVariate syntheticDataSummarizing = new SyntheticDataVariate();
            syntheticDataSummarizing = GetParseLabelToSyntheticDataSummarizing();

            #endregion

            while (true)
            {
                periodCount++;
                _publicDoubleNumber = _doubleBallToo.GetSimplexDoubleBallNumber(false, ++startPeriod);

                Config.RecordPeriods++;
                if (Config.RecordPeriods % Config.IntervalPeriods != 0) continue;

                RefreshIntervalPeriods();

                int maxLotteryMoneySerialNumber = 0;
                //本期我的号码中奖总注数和中奖总金额
                long winAwardTotalZhu = 0, winAwardTotalMonery = 0;

                //记录本次机选页最高奖的我的机选号
                DoubleBall _thisTimeMaxRandomNumber = new DoubleBall(AwardType.NotAward);

                #region 随机、无限摇奖随机生成并排序我的号码

                if (isCreateMyRandomNumber)
                {
                    _myDoubleBallList = RegenMyDoubleBallRandomNumber(
                        redCount, redTuoCount, blueCount, selectIndex, ref thisTimeMonetary, ref thisTimeBuyTotalZhu);
                }

                #endregion

                _myDoubleBallList.ForEach(dbn => dbn.ComparisonDoubleBallNumber(_doubleBallToo, _publicDoubleNumber));

                #region 统计摇奖数据各项内容

                for (int i = 0; i < _myDoubleBallList.Count; i++)
                {
                    //统计本次中奖注数和中奖金额
                    if (_myDoubleBallList[i].awardType != AwardType.NotAward)
                    {
                        winAwardTotalZhu += _myDoubleBallList[i].GetTotalWinningTotalZhu();
                        winAwardTotalMonery += _myDoubleBallList[i].GetTotalWinningMoney(int.Parse(cmbMultiple.Text));
                    }

                    StatisticsLoopLotteryAwardCount(_myDoubleBallList[i], loopDataSummarizing, loopDataAnalyse);

                    //循环比对找到本期我的机选号中的最高奖
                    if ((int)_myDoubleBallList[i].awardType > (int)_thisTimeMaxRandomNumber.awardType)
                    {
                        _thisTimeMaxRandomNumber = _myDoubleBallList[i];
                    }

                    //判断该我的机选号是否符合我期望的中奖条件
                    if (_myDoubleBallList[i].awardType >= stopAwardType)
                    {
                        exitLoop = true;
                    }
                }

                #endregion

                #region 统计历史最高奖和循环摇奖内的最高奖

                if ((int)_thisTimeMaxRandomNumber.awardType > (int)_historyMaxDoubleBallNumber.awardType)
                {
                    _appearMaxNumber = true;

                    _thisTimeMaxRandomNumber.CopyLeftToRightDataValue(_historyMaxDoubleBallNumber);
                    if (isCreateMyRandomNumber) _publicDoubleNumber.CopyLeftToRightDataValue(_historyMaxPublicDoubleBallNumber);
                }

                if ((int)_thisTimeMaxRandomNumber.awardType > (int)_maxLoopDoubleBallNumber.awardType)
                {
                    _appearLoopMaxNumber = true;
                    _thisTimeMaxRandomNumber.CopyLeftToRightDataValue(_maxLoopDoubleBallNumber);
                    if (isCreateMyRandomNumber) _publicDoubleNumber.CopyLeftToRightDataValue(_maxLoopLotteryBoubleBallNumber);
                }

                #endregion

                #region 随机五年十年摇奖退出条件

                if (isCreateMyRandomNumber && loopCount < 20000 && periodCount == loopCount)
                {
                    exitLoop = true;
                }

                #endregion

                #region 累加统计综合和循环摇奖数据

                if (!exitLoop)
                {
                    /**
                     * 这里因为最佳性能需要、不在无限调用RefreshDoubleBallTabulateData()方法、该方法会赋值计算 Label 应该显示的值、
                     *   而是定义变量累加统计、最后调用一次把统计的数据传入过去就行了、
                     *   方法是累加计算一次 Label 的 Tag 值
                     *   所以这里统计的时候最后退出那次不累加、这样少一次 、多一次正好是正确的值
                     */
                    //综合数据
                    syntheticDataSummarizing.winPrizeAwardTotalZhu += winAwardTotalZhu;
                    syntheticDataSummarizing.buyTotalZhu += thisTimeBuyTotalZhu;
                    syntheticDataSummarizing.winPrizeAwardTotalMoney += winAwardTotalMonery;
                    syntheticDataSummarizing.totalConsumptionMoney += thisTimeMonetary;

                    //循环数据
                    loopDataSummarizing.LoopWinPrizeLotteryTotalZhu += winAwardTotalZhu;
                    loopDataSummarizing.LoopWinPrizeLotteryTotalMoney += winAwardTotalMonery;
                    loopDataSummarizing.LoopTotalConsumptionMoney += thisTimeMonetary;
                    loopDataSummarizing.buyTotalZhu += thisTimeBuyTotalZhu;
                }

                //循环数据
                loopDataSummarizing.LoopPeriodsCount = periodCount;

                #endregion

                //退出标志、并更新信息
                if (exitLoop)
                {
                    _refreshLoopData = true;
                    //更新期数
                    UpdateDoubleBallPeriods(periodCount);
                    //展示开奖双色球号码
                    ShowDoubleBallLotteryNumber(_publicDoubleNumber);

                    if (isCreateMyRandomNumber)
                    {
                        #region Random Model Create And Show

                        if (selectIndex == 1)
                        {
                            DynamicRegulationControlAndShowNumber(_myDoubleBallList, flpSimplexNumber, selectIndex);
                        }
                        else if (selectIndex == 2)
                        {
                            RefreshOneselfNumberMoneyByComplexList(_myDoubleBallList);
                            DynamicRegulationControlAndShowNumber(_myDoubleBallList, flpComplexNumber, selectIndex);
                        }
                        else
                        {
                            RefreshOneselfNumberMoneyByDantuoList(_myDoubleBallList);
                            DynamicRegulationControlAndShowNumber(_myDoubleBallList, flpDantuoNumber, selectIndex);
                        }

                        #endregion
                    }

                    #region 对中期望奖的那期号码进行绘色

                    foreach (Panel panel in flpDoubleBallPanel.Controls)
                    {
                        int serialNumber = ParseTagValueToInt(panel);

                        DoubleBall myDoubleBallNumber = _myDoubleBallList.Where(sd => sd.serialNumber == serialNumber).ToList()[0];

                        //对单个号码(Panel)中奖情况的6个Label进行绘色
                        DrawingLabelColorLotteryNumber(panel, myDoubleBallNumber, _publicDoubleNumber);

                        if ((int)myDoubleBallNumber.awardType == (int)_historyMaxDoubleBallNumber.awardType)
                        {
                            //找到中奖双色球的序列号、排序后不能用辅助类里的
                            foreach (Label label in panel.Controls)
                            {
                                if (label.Tag.Equals("No"))
                                {
                                    string viewSerialNumber = label.Text.Replace(">", "");
                                    maxLotteryMoneySerialNumber = int.Parse(viewSerialNumber);
                                    break;
                                }
                            }
                        }
                    }

                    #endregion

                    //刷新机选页本期分析数据
                    RefreshCurrentPeriodMyData(winAwardTotalZhu, winAwardTotalMonery, maxLotteryMoneySerialNumber, _thisTimeMaxRandomNumber.awardType);
                    //刷新右侧栏综合数据
                    RefreshRightSynthesisData(syntheticDataSummarizing, loopDataSummarizing);

                    _refreshLoopData = false;

                    loopDataAnalyse.ShowDialog();

                    break;
                }
            }
        }

        /// <summary>
        /// 验证胆拖自组组合号码是否成号
        /// </summary>
        /// <returns></returns>
        private bool CheckOneselfDantuoNumberCount()
        {
            if (CardInterface.SelectedIndex == 3)
            {
                int redDanCount, redTuoCount;

                if (rbEveryRandom.Checked && cmbRedDanCount.SelectedIndex != 0 && cmbRedTuoCount.SelectedIndex != 0)
                {
                    redDanCount = int.Parse(cmbRedDanCount.Text);
                    redTuoCount = int.Parse(cmbRedTuoCount.Text);

                    if (redDanCount + redTuoCount < 6)
                    {
                        Info.ShowWarningMessage(Info.DantuoCountInsufficient);
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 本次循环数据累加页数据选择
        /// </summary>
        private void AcquireThisTimeMoneyZhu(int selectIndex, ref LeeLabel lblthisTimeMoney, ref LeeLabel lblConsumeZhu)
        {
            if (selectIndex == 1)
            {
                lblthisTimeMoney = lblRandomMonetary;
                lblConsumeZhu = lblRandomBuyZhu;
            }
            else if (selectIndex == 2)
            {
                lblthisTimeMoney = lblComplexMonetary;
                lblConsumeZhu = lblComplexBuyZhu;
            }
            else if (selectIndex == 3)
            {
                lblthisTimeMoney = lblDantuoMonetary;
                lblConsumeZhu = lblDantuoBuyZhu;
            }
        }

        /// <summary>
        /// 开奖前随机重新生成我的双色球号码
        /// </summary>
        private List<DoubleBall> RegenMyDoubleBallRandomNumber(int redCount, int redTuoCount, int blueCount, int selectIndex, ref long thisTimeMonetary, ref long thisTimeBuyTotalZhu)
        {
            if (selectIndex == 1)
            {
                _mySimplexDoubleBallNumberList.Clear();

                for (int i = 0; i < _randomDoubleBallCount; i++)
                {
                    _mySimplexDoubleBallNumberList.Add(_doubleBallToo.GetSimplexDoubleBallNumber(true, 0));
                }

                if (rbRandomOrderByRed.Checked) orderByRedBallAsc = !orderByRedBallAsc;
                if (rbRandomOrderByBlue.Checked) orderByBlueBallAsc = !orderByBlueBallAsc;
                if (!rbRandomOrderByNull.Checked) OrderByMySimplexDoubleBallNumberList(false);
                if (rbRandomOrderByRed.Checked) orderByRedBallAsc = !orderByRedBallAsc;
                if (rbRandomOrderByBlue.Checked) orderByBlueBallAsc = !orderByBlueBallAsc;

                return _mySimplexDoubleBallNumberList.Cast<DoubleBall>().ToList();
            }
            else if (selectIndex == 2)
            {
                thisTimeMonetary = 0;
                thisTimeBuyTotalZhu = 0;

                _ComplexDoubleBallDoubleBallCount = 0;
                _myComplexDoubleBallNumberList.Clear();

                int buyTotalZhu = 0;

                for (int i = 0; i < _randomDoubleBallCount; i++)
                {
                    CreateComplexRandomCombination(ref redCount, ref blueCount);
                    //动态调整生成的注数
                    DynamicAmendMaxComplexTotalZhu(ref redCount, ref blueCount, ref buyTotalZhu);

                    if (buyTotalZhu <= _MaxOneselfDoubleBallTotalZhu)
                    {
                        ComplexDoubleBallNumber cdbn = _doubleBallToo.GetComplexDoubleBallNumber(redCount, blueCount);

                        thisTimeBuyTotalZhu += cdbn.GetDoubleBallCombination();
                        thisTimeMonetary += cdbn.GetDoubleBallCombination() * 2 * int.Parse(cmbMultiple.Text);

                        _myComplexDoubleBallNumberList.Add(cdbn);
                    }
                    else break;
                }

                return _myComplexDoubleBallNumberList.Cast<DoubleBall>().ToList();
            }
            else if (selectIndex == 3)
            {
                thisTimeMonetary = 0;
                thisTimeBuyTotalZhu = 0;

                _DantuoDoubleBallDoubleBallCount = 0;
                _myDantuoDoubleBallNumberList.Clear();

                int buyTotalZhu = 0;

                for (int i = 0; i < _randomDoubleBallCount; i++)
                {
                    CreateDantuoRandomCombination(ref redCount, ref redTuoCount, ref blueCount);
                    
                    //动态调整生成的注数
                    DynamicAmendMaxDantuoTotalZhu(ref redCount, ref redTuoCount, ref blueCount, ref buyTotalZhu);

                    if (buyTotalZhu <= _MaxOneselfDoubleBallTotalZhu)
                    {
                        DantuoDoubleBallNumber cdbn = _doubleBallToo.GetDantuoDoubleBallNumber(redCount, redTuoCount, blueCount);

                        thisTimeBuyTotalZhu += cdbn.GetDoubleBallCombination();
                        thisTimeMonetary += cdbn.GetDoubleBallCombination() * 2 * int.Parse(cmbMultiple.Text);

                        _myDantuoDoubleBallNumberList.Add(cdbn);
                    }
                    else break;
                }

                return _myDantuoDoubleBallNumberList.Cast<DoubleBall>().ToList();
            }

            return null;
        }

        /// <summary>
        /// 创建复试号的随机组合号
        /// </summary>
        private void CreateComplexRandomCombination(ref int redCount, ref int blueCount)
        {
            if (cmbRedComplexCount.SelectedIndex == 0)
                redCount = MyRandom.GetRandomNum(6, 21);

            if (cmbBlueComplexCount.SelectedIndex == 0)
                blueCount = MyRandom.GetRandomNum(1, 17);
        }

        /// <summary>
        /// 创建胆拖的随机组合号
        /// </summary>
        private void CreateDantuoRandomCombination(ref int redDanCount, ref int redTuoCount, ref int blueCount)
        {
            if (cmbRedDanCount.SelectedIndex == 0)
                redDanCount = MyRandom.GetRandomNum(1, 6);

            if (cmbRedTuoCount.SelectedIndex == 0)
                redTuoCount = MyRandom.GetRandomNum(6 - redDanCount, 21);

            if (cmbBlueDanCount.SelectedIndex == 0)
                blueCount = MyRandom.GetRandomNum(1, 17);

            if (cmbRedDanCount.SelectedIndex == 0 && cmbRedTuoCount.SelectedIndex != 0)
            {
                redDanCount = MyRandom.GetRandomNum(6 - redTuoCount, 6);
            }
        }

        /// <summary>
        /// 随机摇奖前我的随机号码数据来源页准备
        /// </summary>
        private void DecideMyRandomDataSource(bool isCreateMyRandomNumber, int selectIndex, ref int redCount, ref int redTuoCount, ref int blueCount)
        {
            if (isCreateMyRandomNumber)
            {
                _randomDoubleBallCount = (int)nudRandomBuyZhu.Value;
                if (selectIndex == 1)
                {
                    //更新机选页购买注数和金额
                    lblRandomBuyZhu.Text = FormatNumber(nudRandomBuyZhu.Value.ToString(), 3);
                    lblRandomBuyZhu.Tag = nudRandomBuyZhu.Value;

                    lblRandomMonetary.Text = (_randomDoubleBallCount * 2 * int.Parse(cmbMultiple.Text)).ToString();
                    lblRandomMonetary.Tag = lblRandomMonetary.Text;
                }
                else if (selectIndex == 2)
                {
                    ParseComplexCombination(ref redCount, ref blueCount);
                }
                else if (selectIndex == 3)
                {
                    ParseDantuoCombination(ref redCount, ref redTuoCount, ref blueCount);
                }
            }
        }

        /// <summary>
        /// 统计循环摇奖期间开出的各个奖的数量
        /// </summary>
        /// <param name="_myDoubleBallNumber">我的双色球号码</param>
        /// <param name="loopDataSummarizing">循环摇奖数据汇总类</param>
        /// <param name="loopDataAnalyse">循环摇奖展示窗体</param>
        private void StatisticsLoopLotteryAwardCount(DoubleBall _myDoubleBallNumber, LoopDataSummarizing loopDataSummarizing, LoopDataAnalyse loopDataAnalyse)
        {
            _myDoubleBallNumber.SettingAwardTypeTotalZhu(loopDataSummarizing);

            if (_myDoubleBallNumber.awardType == AwardType.SixAward)
            {
                if (loopDataSummarizing.SixPrizeSimplexDoubleBall == null)
                {
                    loopDataAnalyse._LotteryDoubleBallNumbers[5] = new SimplexDoubleBallNumber();
                    loopDataSummarizing.SixPrizeSimplexDoubleBall = new SimplexDoubleBallNumber();

                    _publicDoubleNumber.CopyLeftToRightDataValue(loopDataAnalyse._LotteryDoubleBallNumbers[5]);
                    _myDoubleBallNumber.CopyLeftToRightDataValue(loopDataSummarizing.SixPrizeSimplexDoubleBall);
                }
            }

            else if (_myDoubleBallNumber.awardType == AwardType.FiveAward)
            {
                if (loopDataSummarizing.FivePrizeSimplexDoubleBall == null)
                {
                    loopDataAnalyse._LotteryDoubleBallNumbers[4] = new SimplexDoubleBallNumber();
                    loopDataSummarizing.FivePrizeSimplexDoubleBall = new SimplexDoubleBallNumber();

                    _publicDoubleNumber.CopyLeftToRightDataValue(loopDataAnalyse._LotteryDoubleBallNumbers[4]);
                    _myDoubleBallNumber.CopyLeftToRightDataValue(loopDataSummarizing.FivePrizeSimplexDoubleBall);
                }
            }

            else if (_myDoubleBallNumber.awardType == AwardType.FourAward)
            {
                if (loopDataSummarizing.FourPrizeSimplexDoubleBall == null)
                {
                    loopDataAnalyse._LotteryDoubleBallNumbers[3] = new SimplexDoubleBallNumber();
                    loopDataSummarizing.FourPrizeSimplexDoubleBall = new SimplexDoubleBallNumber();

                    _publicDoubleNumber.CopyLeftToRightDataValue(loopDataAnalyse._LotteryDoubleBallNumbers[3]);
                    _myDoubleBallNumber.CopyLeftToRightDataValue(loopDataSummarizing.FourPrizeSimplexDoubleBall);
                }
            }

            else if (_myDoubleBallNumber.awardType == AwardType.ThreeAward)
            {
                if (loopDataSummarizing.ThreePrizeSimplexDoubleBall == null)
                {
                    loopDataAnalyse._LotteryDoubleBallNumbers[2] = new SimplexDoubleBallNumber();
                    loopDataSummarizing.ThreePrizeSimplexDoubleBall = new SimplexDoubleBallNumber();

                    _publicDoubleNumber.CopyLeftToRightDataValue(loopDataAnalyse._LotteryDoubleBallNumbers[2]);
                    _myDoubleBallNumber.CopyLeftToRightDataValue(loopDataSummarizing.ThreePrizeSimplexDoubleBall);
                }
            }

            else if (_myDoubleBallNumber.awardType == AwardType.TwoAward)
            {
                if (loopDataSummarizing.TwoPrizeSimplexDoubleBall == null)
                {
                    loopDataAnalyse._LotteryDoubleBallNumbers[1] = new SimplexDoubleBallNumber();
                    loopDataSummarizing.TwoPrizeSimplexDoubleBall = new SimplexDoubleBallNumber();

                    _publicDoubleNumber.CopyLeftToRightDataValue(loopDataAnalyse._LotteryDoubleBallNumbers[1]);
                    _myDoubleBallNumber.CopyLeftToRightDataValue(loopDataSummarizing.TwoPrizeSimplexDoubleBall);
                }
            }

            else if (_myDoubleBallNumber.awardType == AwardType.OneAward)
            {
                if (loopDataSummarizing.OnePrizeSimplexDoubleBall == null)
                {
                    loopDataAnalyse._LotteryDoubleBallNumbers[0] = new SimplexDoubleBallNumber();
                    loopDataSummarizing.OnePrizeSimplexDoubleBall = new SimplexDoubleBallNumber();

                    _publicDoubleNumber.CopyLeftToRightDataValue(loopDataAnalyse._LotteryDoubleBallNumbers[0]);
                    _myDoubleBallNumber.CopyLeftToRightDataValue(loopDataSummarizing.OnePrizeSimplexDoubleBall);
                }
            }
        }

        /// <summary>
        /// 机选页 中奖分析位置栏偏移量
        /// </summary>
        private int offSetLastLength = 4;

        /// <summary>
        /// 自选复选页 中奖分析位置栏偏移量
        /// </summary>
        private int complexOffSetLastLength = 4;

        /// <summary>
        /// 自选胆拖页 中奖分析位置栏偏移量
        /// </summary>
        private int dantuoOffSetLastLength = 4;

        /// <summary>
        /// 刷新我的号码本期中奖数据
        /// </summary>
        private void RefreshCurrentPeriodMyData(long winTotalZhu, long winTotalMoney, long maxLotteryMoneySerialNumber, AwardType maxLotteryMessage)
        {
            if(CardInterface.SelectedIndex == 1)
            {
                lblRandomWinZhu.Text = FormatNumber(winTotalZhu.ToString(), 3);
                lblRandomWinZhu.Tag = winTotalZhu;

                lblRandomWinTotalMonery.Text = FormatNumber(winTotalMoney.ToString(), 6);
                lblRandomWinTotalMonery.Tag = winTotalMoney;

                if (maxLotteryMessage == AwardType.NotAward)
                {
                    lblRandomMaxAward.Text = Info.GetLotteryChineseMessage(maxLotteryMessage);
                }
                else
                {
                    lblRandomMaxAward.Text = string.Format(Info.FormatLottery, maxLotteryMoneySerialNumber, Info.GetLotteryChineseMessage(maxLotteryMessage));
                }

                if (lblDoubleBallPeriods.Tag.ToString().Length > offSetLastLength)
                {
                    int periodsLength = lblDoubleBallPeriods.Tag.ToString().Length;
                    int offSetX = (periodsLength - offSetLastLength) * 7;
                    offSetLastLength = periodsLength;
                    panStatisticsRandom.Location = new Point(panStatisticsRandom.Location.X + offSetX, panStatisticsRandom.Location.Y);
                }

                lblRandomBerperiods.Text = string.Format(Info.RandomLotteryNumberPeriods, FormatNumber(lblDoubleBallPeriods.Tag.ToString(), 3));
            }
            
            else if (CardInterface.SelectedIndex == 2)
            {
                lblComplexWinZhu.Tag = winTotalZhu;
                lblComplexWinZhu.Text =
                    string.Format("[{0}] {1}", Info.GetNumberMaxUnit(winTotalZhu), FormatNumber(winTotalZhu.ToString(), 5));

                lblComplexWPTotalMonery.Tag = winTotalMoney;
                lblComplexWPTotalMonery.Text =
                    string.Format("[{0}] {1}", Info.GetNumberMaxUnit(winTotalMoney), FormatNumber(winTotalMoney.ToString(), 6));

                if (maxLotteryMessage == AwardType.NotAward)
                {
                    lblComplexMaxAward.Text = Info.GetLotteryChineseMessage(maxLotteryMessage);
                }
                else
                {
                    lblComplexMaxAward.Text = string.Format(Info.FormatLottery, maxLotteryMoneySerialNumber, Info.GetLotteryChineseMessage(maxLotteryMessage));
                }

                if (lblDoubleBallPeriods.Tag.ToString().Length > complexOffSetLastLength)
                {
                    int periodsLength = lblDoubleBallPeriods.Tag.ToString().Length;
                    int offSetX = (periodsLength - complexOffSetLastLength) * 7;
                    complexOffSetLastLength = periodsLength;
                    panStatisticsComplex.Location = new Point(panStatisticsComplex.Location.X + offSetX, panStatisticsComplex.Location.Y);
                }

                lblComplexBerperiods.Text = string.Format(Info.RandomLotteryNumberPeriods, FormatNumber(lblDoubleBallPeriods.Tag.ToString(), 3));
            }
            else
            {
                lblDantuoWinZhu.Tag = winTotalZhu;
                lblDantuoWinZhu.Text =
                    string.Format("[{0}] {1}", Info.GetNumberMaxUnit(winTotalZhu), FormatNumber(winTotalZhu.ToString(), 5));

                lblDantuoWPTotalMonery.Tag = winTotalMoney;
                lblDantuoWPTotalMonery.Text =
                    string.Format("[{0}] {1}", Info.GetNumberMaxUnit(winTotalMoney), FormatNumber(winTotalMoney.ToString(), 6));

                if (maxLotteryMessage == AwardType.NotAward)
                {
                    lblDantuoMaxAward.Text = Info.GetLotteryChineseMessage(maxLotteryMessage);
                }
                else
                {
                    lblDantuoMaxAward.Text = string.Format(Info.FormatLottery, maxLotteryMoneySerialNumber, Info.GetLotteryChineseMessage(maxLotteryMessage));
                }

                if (lblDoubleBallPeriods.Tag.ToString().Length > dantuoOffSetLastLength)
                {
                    int periodsLength = lblDoubleBallPeriods.Tag.ToString().Length;
                    int offSetX = (periodsLength - dantuoOffSetLastLength) * 7;
                    dantuoOffSetLastLength = periodsLength;
                    panStatisticsDantuo.Location = new Point(panStatisticsDantuo.Location.X + offSetX, panStatisticsDantuo.Location.Y);
                }

                lblDantuoBerperiods.Text = string.Format(Info.RandomLotteryNumberPeriods, FormatNumber(lblDoubleBallPeriods.Tag.ToString(), 3));
            }
        }

        /// <summary>
        /// [主页]按钮：重置本次数据
        /// </summary>
        private void btnResetData_Click(object sender, EventArgs e)
        {
            Logger.Info("reset all data.");
            Logger.Info($"reset all of periods : {lblDoubleBallPeriods.Text}");

            //重置期数及次数
            _loopRunLotterysFlag = -1;
            lblDoubleBallPeriods.Tag = 0;
            lblDoubleBallPeriods.Text = "00000000000";

            //重置本次双色球号码
            _publicDoubleNumber = new SimplexDoubleBallNumber();

            //重置开奖双色球号码显示
            ShowDoubleBallLotteryNumber(_publicDoubleNumber);

            //重置综合数据汇总页
            lblWinTotalZhu.Tag = 0;
            lblWinTotalZhu.Text = string.Format(Info.LotteryZhuCountMessage, "零", FormatNumber(0, 5));

            lblBuyTotalZhu.Tag = 0;
            lblBuyTotalZhu.Text = string.Format(Info.LotteryZhuCountMessage, "零", FormatNumber(0, 5));

            lblWinAwardTotalMoney.Tag = 0;
            lblWinAwardTotalMoney.Text = string.Format(Info.LotteryMoneyConutMessage, "零", FormatNumber(0, 5));

            lblBuyTotalMoney.Tag = 0;
            lblBuyTotalMoney.Text = string.Format(Info.LotteryMoneyConutMessage, "零", FormatNumber(0, 5));

            lblProfitTotalMoney.Tag = 0;
            lblProfitTotalMoney.Text = string.Format(Info.LotteryMoneyConutMessage, "零", FormatNumber(0, 5));

            lblLoopBuyTotalZhu.Tag = 0;
            lblLoopBuyTotalZhu.Text = string.Format(Info.LotteryZhuCountMessage, "零", FormatNumber(0, 5));

            lblLotteryPercent.Text = "0.000%";
            lblLotteryPercent.ForeColor = Color.DarkGray;

            lblCharityTotalMoney.Text = string.Format(Info.PublicBenefitTotalMoney, FormatNumber(0, 5));

            //重置机选页偏移量和恢复Panel的初始Location
            offSetLastLength = 4;
            panStatisticsRandom.Location = new Point(134, 0);

            complexOffSetLastLength = 4;
            panStatisticsComplex.Location = new Point(134, 0);

            dantuoOffSetLastLength = 4;
            panStatisticsDantuo.Location = new Point(134, 0);

            //重置循环摇奖数据汇总页
            ResetLoopRunLotteryData();
            Config._historyPublicDoubleBallNumbers.Clear();
            _LotteryHistory?.ClearHistoryNumbers();

            Config.RecordPeriods = 0;

            //重置历史最高奖
            foreach (Label item in panMaxDoubleBall.Controls)
            {
                item.Text = "00";
                item.ForeColor = _NotLotteryColor;
            }
            _appearMaxNumber = false;
            _historyMaxDoubleBallNumber = new SimplexDoubleBallNumber(AwardType.NotAward);

            btnResetRandomNo_Click(btnResetRandomNo, e);
            btnResetOneself_Click(btnResetRandomNo, e);
            btnResetDOneself_Click(btnResetRandomNo, e);
        }

        private void ResetLoopRunLotteryData()
        {
            _loopPeriods = 0;

            lblLoopPeriods.Tag = 0;
            lblLoopPeriods.Text = string.Format(Info.LotteryPeriodsCountMessage, "零", FormatNumber(0, 5));

            lblLoopWinTotalZhu.Tag = 0;
            lblLoopWinTotalZhu.Text = string.Format(Info.LotteryZhuCountMessage, "零", FormatNumber(0, 5));

            lblLoopBuyTotalZhu.Tag = 0;
            lblLoopBuyTotalZhu.Text = string.Format(Info.LotteryZhuCountMessage, "零", FormatNumber(0, 5));

            lblLoopWinAwardTotalMoney.Tag = 0;
            lblLoopWinAwardTotalMoney.Text = string.Format(Info.LotteryMoneyConutMessage, "零", FormatNumber(0, 5));

            lblLoopBuyTotalMoney.Tag = 0;
            lblLoopBuyTotalMoney.Text = string.Format(Info.LotteryMoneyConutMessage, "零", FormatNumber(0, 5));

            _appearLoopMaxNumber = false;
            _maxLoopDoubleBallNumber = new SimplexDoubleBallNumber(AwardType.NotAward);
            //重置循环摇奖历史最高奖
            foreach (Label item in panLoopYear.Controls)
            {
                item.Text = "00";
                item.ForeColor = _NotLotteryColor;
            }
        }

        /// <summary>
        /// 是否随机摇奖
        /// </summary>
        private bool isRandomRun;

        /// <summary>
        /// [主页]按钮：随机五年十年无限摇奖
        /// </summary>
        private void btnInfiniteRondomLoopYear_Click(object sender, EventArgs e)
        {
            int loopCount = int.Parse((sender as Control).Tag.ToString());

            switch (CardInterface.SelectedIndex)
            {
                //机选页
                case int index when index >= 1 && index <= 3:

                    string buttonType = ((MaterialFlatButton)sender).Name;

                    if (buttonType == "btnRondomFiveYear")
                    {
                        Logger.Info("five year random number run lottery.");
                    }
                    else if (buttonType == "btnRondomTenYear")
                    {
                        Logger.Info("ten year random number run lottery.");
                    }
                    else if (buttonType == "btnRondomCustomize")
                    {
                        Logger.Info($"customize {Config.Setting.CustomizePeriods} random number run lottery.");
                    }
                    else if (buttonType == "btnInfiniteRondomLoopYear")
                    {
                        Logger.Info("infinite random number run lottery.");
                    }

                    if (loopCount == 9999)
                    {
                        if (Info.ShowQuestionMessage(Info.LoopRunLotteryMessage1) != DialogResult.OK)
                        {
                            return;
                        }
                    }
                    isRandomRun = true;

                    if (index == 1)
                    {
                        LoopComprehensiveLogicOfLotteryDrawing(_mySimplexDoubleBallNumberList.Cast<DoubleBall>().ToList(), flpSimplexNumber, isRandomRun, loopCount);
                    }

                    if (index == 2)
                    {
                        LoopComprehensiveLogicOfLotteryDrawing(_myComplexDoubleBallNumberList.Cast<DoubleBall>().ToList(), flpComplexNumber, isRandomRun, loopCount);
                    }

                    if (index == 3)
                    {
                        LoopComprehensiveLogicOfLotteryDrawing(_myDantuoDoubleBallNumberList.Cast<DoubleBall>().ToList(), flpDantuoNumber, isRandomRun, loopCount);
                    }

                    isRandomRun = false;
                    break;
            }
        }

        /// <summary>
        /// [主页]按钮：选择下拉框倍数
        /// </summary>
        private void cmbMultiple_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshRandomMoney();
            RefreshDoubleBallMoney(lblComplexBuyZhu, lblComplexMonetary);
            RefreshDoubleBallMoney(lblDantuoBuyZhu, lblDantuoMonetary);
        }

        private void RefreshDoubleBallMoney(LeeLabel labelBuyZhu, LeeLabel labelMonetary)
        {
            int multiple = int.Parse(cmbMultiple.Text);
            int totalZhu = int.Parse(labelBuyZhu.Tag.ToString());

            labelMonetary.Tag = totalZhu * 2 * multiple;
            labelMonetary.Text =
                string.Format("[{0}] {1}", Info.GetNumberMaxUnit(int.Parse(labelMonetary.Tag.ToString())), FormatNumber(labelMonetary.Tag.ToString(), 5));
        }

        /// <summary>
        /// 是否点击了摇奖号码：用于控制机选N注号码后，不可以用排序
        /// </summary>
        private bool _createNumber;

        /// <summary>
        /// [机选页]按钮：机选 N 注逻辑
        /// </summary>
        private void lblRandom1_Click(object sender, EventArgs e)
        {
            panRandomFilter.Enabled = !_createNumber;

            int randomCount = ParseTagValueToInt(((LeeLabel)sender));

            if (_randomDoubleBallCount < _MaxSimplexDoubleBallCount)
            {
                randomCount = AmendMaxCount(randomCount, _randomDoubleBallCount, _MaxSimplexDoubleBallCount);

                //生成机选双色球号码
                for (int i = 0; i < randomCount; i++)
                {
                    SimplexDoubleBallNumber doubleBallNumber = _doubleBallToo.GetSimplexDoubleBallNumber(true, 0);
                    _mySimplexDoubleBallNumberList.Add(doubleBallNumber);
                }

                //显示机选双色球号码
                ShowMyRandomDoubleBallNumber(_mySimplexDoubleBallNumberList, randomCount);

                //显示注数和所需金额
                RefreshRandomMoney();

                panOrderRandom.Visible = _randomDoubleBallCount > 1;

                Logger.Info($"random {randomCount} simplex number.");
            }
            else
            {
                Info.ShowWarningMessage(Info.RestrictMaxRandomDoubleBall);
            }
        }

        /// <summary>
        /// 修正生成的数量为最大限制的双色球数量
        /// </summary>
        /// <param name="randomCount">要生成的数量</param>
        /// <param name="currentDoubleBallCount">当前数量</param>
        /// <param name="maxDoubleBallCount">最高数量</param>
        private int AmendMaxCount(int randomCount, int currentDoubleBallCount, int maxDoubleBallCount)
        {
            int _amendCount = currentDoubleBallCount + randomCount;

            if (_amendCount > maxDoubleBallCount)
            {
                _amendCount = maxDoubleBallCount - currentDoubleBallCount;
            }
            else
            {
                _amendCount = randomCount;
            }

            return _amendCount;
        }

        /// <summary>
        /// 排序红球升降序排列标志
        /// </summary>
        private bool orderByRedBallAsc = true;
        /// <summary>
        /// 排序蓝球升降序排列标志
        /// </summary>
        private bool orderByBlueBallAsc = true;

        //定义委托方法
        RefreshOrderByDoubleBallDelegate _orderByDoubleBallDelegate;
        //排序后刷新界面委托
        private delegate void RefreshOrderByDoubleBallDelegate(object sender);

        LoopRunLotteryNumberYearsDelegate _loopRunLotteryDelegate;
        //循环开奖委托
        private delegate void LoopRunLotteryNumberYearsDelegate(long a);

        /**
         * [机选页] 单选按钮按照红球篮球排序
         */
        private void rbNull_Click(object sender, EventArgs e)
        {
            UpdateCmbRandomViewFilterNotEvent();

            //小于等于80个号码用普通方案、否则用动画方案过渡卡顿时间
            if (_randomDoubleBallCount <= _CartoonCount) OrderByDoubleBallAndShow(sender);
            else
            {
                //动画方案
                LoadingHelper.ShowLoading(Info.RefreshData, Color.White, this, o =>
                {
                    Thread.Sleep(350);

                    _orderByDoubleBallDelegate = new RefreshOrderByDoubleBallDelegate(OrderByDoubleBallAndShow);
                    Invoke(_orderByDoubleBallDelegate, sender);
                });
            }
        }

        /// <summary>
        /// [机选页] 将显示下拉框重置到默认值并不触发过滤事件
        /// </summary>
        private void UpdateCmbRandomViewFilterNotEvent()
        {
            _executeFilter = false;
            cmbRandomViewFilter.SelectedIndex = 0;
            _executeFilter = true;
        }

        /// <summary>
        /// 是否执行机选号结果过滤
        /// </summary>
        private bool _executeFilter;
        /// <summary>
        /// [机选页] 显示过滤 上一次过滤的条件|防止下拉框两次选择同样的值重复过滤
        /// </summary>
        private int _randomViewFilterLastSelectIndex;

        /// <summary>
        /// [机选页] 机选页显示下拉框选择过滤项
        /// </summary>
        private void cmbRandomViewFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_executeFilter && _randomViewFilterLastSelectIndex != cmbRandomViewFilter.SelectedIndex)
            {
                flpSimplexNumber.Controls.Clear();

                //临时将总数设置为0、生成控件方法因为用到追加、后续在恢复
                int tempRandomDoubleBallCount = _randomDoubleBallCount;
                _randomDoubleBallCount = 0;

                switch (cmbRandomViewFilter.SelectedIndex)
                {
                    case 0:

                        FilterRandomDoubleBallResult(sd => true);

                        break;
                    case 1:

                        FilterRandomDoubleBallResult(sd => sd.awardType != AwardType.NotAward);

                        break;
                    case 2:

                        FilterRandomDoubleBallResult(sd => sd.awardType == AwardType.NotAward);

                        break;
                    case 3:

                        FilterRandomDoubleBallResult(sd => sd.awardType == AwardType.SixAward);

                        break;
                    case 4:

                        FilterRandomDoubleBallResult(sd => sd.awardType == AwardType.FiveAward);

                        break;
                    case 5:

                        FilterRandomDoubleBallResult(sd => sd.awardType == AwardType.FourAward);

                        break;
                    case 6:

                        FilterRandomDoubleBallResult(sd => sd.awardType == AwardType.ThreeAward);

                        break;
                    case 7:

                        FilterRandomDoubleBallResult(sd => sd.awardType == AwardType.TwoAward);

                        break;
                    case 8:

                        FilterRandomDoubleBallResult(sd => sd.awardType == AwardType.OneAward);

                        break;
                }

                _randomDoubleBallCount = tempRandomDoubleBallCount;
                _randomViewFilterLastSelectIndex = cmbRandomViewFilter.SelectedIndex;
            }
        }

        /// <summary>
        /// 过滤机选双色球条件
        /// </summary>
        private void FilterRandomDoubleBallResult(Func<SimplexDoubleBallNumber, bool> filter)
        {
            List<SimplexDoubleBallNumber> _filterDoubleBallList = _mySimplexDoubleBallNumberList.Where(filter).ToList();

            ShowMyRandomDoubleBallNumber(_filterDoubleBallList, 0);

            foreach (Panel panel in flpSimplexNumber.Controls)
            {
                int serialNumber = ParseTagValueToInt(panel);

                //根据序列号得到机选双色球集合中的号码
                SimplexDoubleBallNumber randomDoubleBallNumber = _mySimplexDoubleBallNumberList.Where(sd => sd.serialNumber == serialNumber).ToList()[0];

                if (panRandomFilter.Visible && randomDoubleBallNumber.awardType != AwardType.Null)
                {
                    DrawingLabelColorLotteryNumber(panel, randomDoubleBallNumber, _publicDoubleNumber);
                }
            }
        }

        /// <summary>
        /// 排序并展示双色球号码
        /// </summary>
        private void OrderByDoubleBallAndShow(object sender)
        {
            if (_randomDoubleBallCount != 0)
            {
                _randomDoubleBallCount = 0;
                flpSimplexNumber.Controls.Clear();
                OrderByMySimplexDoubleBallNumberList(true);

                ShowMyRandomDoubleBallNumber(_mySimplexDoubleBallNumberList, _mySimplexDoubleBallNumberList.Count);

                FilterRandomDoubleBallResult(sd => true);
            }
        }

        /// <summary>
        /// 排序我的双色球集合
        /// </summary>
        /// <param name="bottomUp">是否启用升降序切换</param>
        private void OrderByMySimplexDoubleBallNumberList(bool bottomUp)
        {
            IOrderedEnumerable<SimplexDoubleBallNumber> orderBoubleBallEnumerable;

            if (rbRandomOrderByNull.Checked)
            {
                orderBoubleBallEnumerable = _mySimplexDoubleBallNumberList.OrderBy(sd => sd.serialNumber);
            }
            else if (rbRandomOrderByRed.Checked)
            {
                orderBoubleBallEnumerable = orderByRedBallAsc
                    ?
                    _mySimplexDoubleBallNumberList.OrderBy(sd => sd.OrderByReds)
                    :
                    _mySimplexDoubleBallNumberList.OrderByDescending(sd => sd.OrderByReds);

                if(bottomUp) orderByRedBallAsc = !orderByRedBallAsc;
            }
            else
            {
                orderBoubleBallEnumerable = orderByBlueBallAsc
                    ?
                    _mySimplexDoubleBallNumberList.OrderBy(sd => sd.OrderByBlues)
                    :
                    _mySimplexDoubleBallNumberList.OrderByDescending(sd => sd.OrderByBlues);

                if (bottomUp) orderByBlueBallAsc = !orderByBlueBallAsc;
            }

            _mySimplexDoubleBallNumberList = orderBoubleBallEnumerable.ToList();
        }

        /// <summary>
        /// [机选页] 计算注数和金额
        /// </summary>
        private void RefreshRandomMoney()
        {
            if(_randomDoubleBallCount > 0)
            {
                int multiple = int.Parse(cmbMultiple.Text);

                lblRandomBuyZhu.Text = FormatNumber(_randomDoubleBallCount, 3);
                lblRandomBuyZhu.Tag = _randomDoubleBallCount;

                _randomDoubleBallMoney = _randomDoubleBallCount * 2 * multiple;
                lblRandomMonetary.Text = _randomDoubleBallMoney.ToString();
                lblRandomMonetary.Tag = _randomDoubleBallMoney;
            }
        }

        /// <summary>
        /// [机选页]按钮：重置我的号码
        /// </summary>
        private void btnResetRandomNo_Click(object sender, EventArgs e)
        {
            Logger.Info($"reset my simplex number : {_mySimplexDoubleBallNumberList.Count} row");

            _randomDoubleBallCount = 0;
            _randomDoubleBallMoney = 0;
            _randomDoubleDoubleBallSerial = 0;
            UpdateCmbRandomViewFilterNotEvent();

            orderByRedBallAsc = true;
            orderByBlueBallAsc = true;
            rbRandomOrderByNull.Checked = true;

            panOrderRandom.Visible = false;
            panRandomFilter.Visible = false;
            panRandomStatistics.Visible = false;

            flpSimplexNumber.Controls.Clear();
            _mySimplexDoubleBallNumberList.Clear();

            lblRandomBuyZhu.Text = FormatNumber(_randomDoubleBallCount, 3);
            lblRandomBuyZhu.Tag = 0;

            lblRandomMonetary.Text = FormatNumber(_randomDoubleBallCount, 3);
            lblRandomMonetary.Tag = 0;
        }

        /// <summary>
        /// 如果每次都随机生成我的号码、这里重新获取并动态生成或该变Label的信息
        /// </summary>
        private void RandomlyGenerateMyLotteryNumberAndRedrawLabel(int selectIndex)
        {
            int myCount = (int)nudRandomBuyZhu.Value;

            if (selectIndex == 1)
            {
                _randomDoubleDoubleBallSerial = 0;
                _mySimplexDoubleBallNumberList.Clear();

                _randomDoubleBallCount = myCount;

                for (int v = 0; v < myCount; v++)
                    _mySimplexDoubleBallNumberList.Add(_doubleBallToo.GetSimplexDoubleBallNumber(true, 0));

                if (rbRandomOrderByRed.Checked) orderByRedBallAsc = !orderByRedBallAsc;
                if (rbRandomOrderByBlue.Checked) orderByBlueBallAsc = !orderByBlueBallAsc;
                if (!rbRandomOrderByNull.Checked) OrderByMySimplexDoubleBallNumberList(false);
                if (rbRandomOrderByRed.Checked) orderByRedBallAsc = !orderByRedBallAsc;
                if (rbRandomOrderByBlue.Checked) orderByBlueBallAsc = !orderByBlueBallAsc;

                DynamicRegulationControlAndShowNumber(_mySimplexDoubleBallNumberList.Cast<DoubleBall>().ToList(), flpSimplexNumber, selectIndex);
            }
            else if (selectIndex == 2)
            {
                int redCount = 0;
                int blueCount = 0;
                ParseComplexCombination(ref redCount, ref blueCount);

                _ComplexDoubleBallDoubleBallCount = 0;
                _myComplexDoubleBallNumberList.Clear();

                int buyTotalZhu = 0;

                for (int v = 0; v < myCount; v++)
                {
                    //动态调整生成的注数
                    CreateComplexRandomCombination(ref redCount, ref blueCount);
                    DynamicAmendMaxComplexTotalZhu(ref redCount, ref blueCount, ref buyTotalZhu);

                    if (buyTotalZhu <= _MaxOneselfDoubleBallTotalZhu)
                    {
                        _myComplexDoubleBallNumberList.Add(_doubleBallToo.GetComplexDoubleBallNumber(redCount, blueCount));
                    }
                    else break;
                }

                DynamicRegulationControlAndShowNumber(_myComplexDoubleBallNumberList.Cast<DoubleBall>().ToList(), flpComplexNumber, selectIndex);
            }
            else
            {
                int redDanCount = 0;
                int redTuoCount = 0;
                int blueCount = 0;
                ParseDantuoCombination(ref redDanCount, ref redTuoCount, ref blueCount);

                _DantuoDoubleBallDoubleBallCount = 0;
                _myDantuoDoubleBallNumberList.Clear();

                int buyTotalZhu = 0;

                for (int v = 0; v < myCount; v++)
                {
                    CreateDantuoRandomCombination(ref redDanCount, ref redTuoCount, ref blueCount);
                    //动态调整生成的注数
                    DynamicAmendMaxDantuoTotalZhu(ref redDanCount, ref redTuoCount, ref blueCount, ref buyTotalZhu);

                    if (buyTotalZhu <= _MaxOneselfDoubleBallTotalZhu)
                    {
                        _myDantuoDoubleBallNumberList.Add(_doubleBallToo.GetDantuoDoubleBallNumber(redDanCount, redTuoCount, blueCount));
                    }
                    else break;
                }

                DynamicRegulationControlAndShowNumber(_myDantuoDoubleBallNumberList.Cast<DoubleBall>().ToList(), flpDantuoNumber, selectIndex);
            }
        }

        /// <summary>
        /// 将复试数量控件转换
        /// </summary>
        private void ParseComplexCombination(ref int redCount, ref int blueCount)
        {
            if (cmbRedComplexCount.SelectedIndex != 0)
                redCount = int.Parse(cmbRedComplexCount.Text);

            if (cmbBlueComplexCount.SelectedIndex != 0)
                blueCount = int.Parse(cmbBlueComplexCount.Text);
        }

        /// <summary>
        /// 将胆拖数量控件转换
        /// </summary>
        private void ParseDantuoCombination(ref int redDanCount, ref int redTuoCount, ref int blueCount)
        {
            if (cmbRedDanCount.SelectedIndex != 0)
                redDanCount = int.Parse(cmbRedDanCount.Text);

            if (cmbRedTuoCount.SelectedIndex != 0)
                redTuoCount = int.Parse(cmbRedTuoCount.Text);

            if (cmbBlueDanCount.SelectedIndex != 0)
                blueCount = int.Parse(cmbBlueDanCount.Text);
        }

        /// <summary>
        /// 动态调整控件数量和显示单式双色球号码
        /// </summary>
        /// <param name="myCount"></param>
        private void DynamicRegulationControlAndShowNumber(List<DoubleBall> _myDoubleBallList,  FlowLayoutPanel flpDoubleBallPanel, int selectIndex)
        {
            //移除多余的Panel控件
            int removeCount = flpDoubleBallPanel.Controls.Count;
            for (int i = 0; i < removeCount; i++)
            {
                if (flpDoubleBallPanel.Controls.Count > _myDoubleBallList.Count)
                {
                    if (selectIndex == 2) _CreateIndexByComplex--;
                    flpDoubleBallPanel.Controls.RemoveAt(flpDoubleBallPanel.Controls.Count - 1);
                }
            }

            if (selectIndex == 1)
            {
                #region 动态调整我的单式号码控件

                for (int i = 0; i < _myDoubleBallList.Count; i++)
                {
                    //当控件不足于显示双色球号码的时候纯成控件并显示
                    if (i > flpDoubleBallPanel.Controls.Count - 1)
                    {
                        CreateDoubleBallControlAndShow(_myDoubleBallList[i] as SimplexDoubleBallNumber, i);
                    }
                    else
                    {
                        //否则改变已有控件的值
                        Panel simplexPanel = (Panel)flpDoubleBallPanel.Controls[i];
                        simplexPanel.Tag = _myDoubleBallList[i].serialNumber;

                        FormatRandomPanelContext(simplexPanel, _myDoubleBallList[i] as SimplexDoubleBallNumber, i + 1);
                    }
                }

                panOrderRandom.Visible = _randomDoubleBallCount > 1;
                //更新机选页购买注数和金额
                lblRandomBuyZhu.Text = FormatNumber(nudRandomBuyZhu.Value.ToString(), 3);
                lblRandomBuyZhu.Tag = nudRandomBuyZhu.Value;

                lblRandomMonetary.Text = (_randomDoubleBallCount * 2 * int.Parse(cmbMultiple.Text)).ToString();
                lblRandomMonetary.Tag = lblRandomMonetary.Text;

                #endregion
            }
            else if (selectIndex == 2)
            {
                #region 动态调整我的复试号码控件

                lblComplexBuyZhu.Tag = 0;
                lblComplexMonetary.Tag = 0;

                //自选号没选择完毕清空控件
                if (!_ComplexHelper.oneselfStatus)
                {
                    ResetComplexVariate();
                    flpDoubleBallPanel.Controls.Clear();
                }

                if(_ComplexTipInfo != null) _ComplexTipInfo.RemoveAll();

                for (int i = 0; i < _myDoubleBallList.Count; i++)
                {
                    //改变已有控件为新的双色球号码
                    if (i < flpDoubleBallPanel.Controls.Count)
                    {
                        Panel complexPanel = (Panel)flpDoubleBallPanel.Controls[i];
                        complexPanel.Tag = _myDoubleBallList[i].serialNumber;

                        DynamicRegulationComplexNumberLabels(complexPanel, _myDoubleBallList[i] as ComplexDoubleBallNumber);
                    }
                    else
                    {
                        _CreateIndexByComplex++;
                        //当控件不足于显示双色球号码的时候纯成控件并显示
                        CreateComplexAndShowDoubleBall(_myDoubleBallList[i] as ComplexDoubleBallNumber, false);
                    }
                }

                RefreshOneselfNumberMoneyByComplexList(_myDoubleBallList);

                #endregion
            }
            else
            {
                #region 动态调整我的胆拖号码控件

                lblDantuoBuyZhu.Tag = 0;
                lblDantuoMonetary.Tag = 0;

                if (!_DantuoHelper.oneselfStatus)
                {
                    ResetDantuoVariate();
                }

                if (_DantuoTipInfo != null) _DantuoTipInfo.RemoveAll();

                flpDoubleBallPanel.Controls.Clear();
                for (int i = 0; i < _myDoubleBallList.Count; i++)
                {
                    _CreateIndexByDantuo++;
                    CreateDantuoAndShowDoubleBall(_myDoubleBallList[i] as DantuoDoubleBallNumber, false);
                }

                RefreshOneselfNumberMoneyByDantuoList(_myDoubleBallList);

                #endregion
            }
        }

        /// <summary>
        /// 机选号花费金额
        /// </summary>
        private int _randomDoubleBallMoney = 0;

        /// <summary>
        /// 机选随机双色球总数
        /// </summary>
        private int _randomDoubleBallCount = 0;

        /// <summary>
        /// 机选随机双色球序号
        /// </summary>
        public static int _randomDoubleDoubleBallSerial = 0;

        /// <summary>
        /// 机选号红球的六个 Label 数组样式
        /// </summary>
        private Label[] _redBallLabelStyles = new Label[6];

        /// <summary>
        /// [机选页]根据机选号数量生成并显示在面板上
        /// </summary>
        /// <param name="simplexDoubleBallNumberList">要显示的双色球集合</param>
        /// <param name="_amendCount">新增机选时的注数、用于累加记录机选了多少注</param>
        private void ShowMyRandomDoubleBallNumber(List<SimplexDoubleBallNumber> simplexDoubleBallNumberList, int _amendCount)
        {
            for (int i = _randomDoubleBallCount; i < simplexDoubleBallNumberList.Count; i++)
            {
                CreateDoubleBallControlAndShow(simplexDoubleBallNumberList[i], i);
            }

            _randomDoubleBallCount += _amendCount;
        }

        /// <summary>
        /// 根据双色球对象创建控件承载并显示号码和颜色
        /// </summary>
        private void CreateDoubleBallControlAndShow(SimplexDoubleBallNumber simplexDoubleBallNumber, int i)
        {
            Panel copyPanel = new Panel();
            copyPanel.Size = panRandomStyle.Size;
            copyPanel.Name = "panRandom" + i;
            copyPanel.Tag = simplexDoubleBallNumber.serialNumber;

            //生成机选序号控件
            Label copyRandomNo = new Label();
            CopyLabelStyle(lblRandomNo, copyRandomNo);

            //生成显示红球6个号码的Label
            for (int j = 0; j < _redBallLabelStyles.Length; j++)
            {
                Label copyRedBall = new Label();

                CopyLabelStyle(_redBallLabelStyles[j], copyRedBall);

                copyPanel.Controls.Add(copyRedBall);
            }

            //生成显示篮球号码的Label
            Label copyBlueBall = new Label();
            CopyLabelStyle(lblBlueBall, copyBlueBall);

            copyPanel.Controls.Add(copyRandomNo);
            copyPanel.Controls.Add(copyBlueBall);
            flpSimplexNumber.Controls.Add(copyPanel);

            //格式并显示我的机选号码样式
            FormatRandomPanelContext(copyPanel, simplexDoubleBallNumber, i + 1);
        }

        /// <summary>
        /// 克隆机选号预设样式
        /// </summary>
        /// <param name="byCloneControl">被克隆控件</param>
        /// <param name="cloneControl">新控件</param>
        public static void CopyLabelStyle(Label byCloneControl, Label cloneControl)
        {
            cloneControl.Tag = byCloneControl.Tag;

            cloneControl.Size = byCloneControl.Size;
            cloneControl.Font = byCloneControl.Font;
            cloneControl.Cursor = byCloneControl.Cursor;

            cloneControl.AutoSize = byCloneControl.AutoSize;
            cloneControl.Location = byCloneControl.Location;
            cloneControl.ForeColor = byCloneControl.ForeColor;
        }

        /// <summary>
        /// 格式显示我的机选号码样式
        /// </summary>
        public static void FormatRandomPanelContext(Panel panRandom, SimplexDoubleBallNumber simplexDoubleBallNumber, int doubleBallNo)
        {
            foreach (Label control in panRandom.Controls)
            {
                switch (control.Tag.ToString())
                {
                    case "No": 
                        control.Text = doubleBallNo + ">"; 
                        break;
                    case "Period":
                        control.Text = FormatNumber(simplexDoubleBallNumber.Periods.ToString(), 11);
                        break;
                    case "Red1":
                        control.Text = FormatNumber(simplexDoubleBallNumber.redBalls[0], 2);
                        break;
                    case "Red2":
                        control.Text = FormatNumber(simplexDoubleBallNumber.redBalls[1], 2);
                        break;
                    case "Red3":
                        control.Text = FormatNumber(simplexDoubleBallNumber.redBalls[2], 2);
                        break;
                    case "Red4":
                        control.Text = FormatNumber(simplexDoubleBallNumber.redBalls[3], 2);
                        break;
                    case "Red5":
                        control.Text = FormatNumber(simplexDoubleBallNumber.redBalls[4], 2);
                        break;
                    case "Red6":
                        control.Text = FormatNumber(simplexDoubleBallNumber.redBalls[5], 2);
                        break;
                    case "Blue":
                        control.Text = FormatNumber(simplexDoubleBallNumber.blueBall, 2);
                        break;
                }
            }
        }

        /// <summary>
        /// 展示开奖的双色球号码
        /// </summary>
        private void ShowDoubleBallLotteryNumber(SimplexDoubleBallNumber _showDoubleBallNumbers)
        {
            Red1.Text = FormatNumber(_showDoubleBallNumbers.redBalls[0], 2);
            Red1.OnPaint();
            Red2.Text = FormatNumber(_showDoubleBallNumbers.redBalls[1], 2);
            Red2.OnPaint();

            Red3.Text = FormatNumber(_showDoubleBallNumbers.redBalls[2], 2);
            Red3.OnPaint();
            Red4.Text = FormatNumber(_showDoubleBallNumbers.redBalls[3], 2);
            Red4.OnPaint();

            Red5.Text = FormatNumber(_showDoubleBallNumbers.redBalls[4], 2);
            Red5.OnPaint();
            Red6.Text = FormatNumber(_showDoubleBallNumbers.redBalls[5], 2);
            Red6.OnPaint();
            Blue1.Text = FormatNumber(_showDoubleBallNumbers.blueBall, 2);
            Blue1.OnPaint();
        }

        /// <summary>
        /// 更新双色球期数
        /// </summary>
        private void UpdateDoubleBallPeriods(long periodCount)
        {
            long periods = ParseTagValueToLong(lblDoubleBallPeriods); 
            
            periods += periodCount;
            lblDoubleBallPeriods.Tag = periods;

            lblDoubleBallPeriods.Text = FormatNumber(periods.ToString(), 11);

            Config.RecordPeriods += periodCount;
        }

        /// <summary>
        /// 将综合数据页的信息转换为实体类
        /// </summary>
        private SyntheticDataVariate GetParseLabelToSyntheticDataSummarizing()
        {
            SyntheticDataVariate syntheticDataSummarizing = new SyntheticDataVariate();
            syntheticDataSummarizing.winPrizeAwardTotalZhu = ParseTagValueToLong(lblWinTotalZhu);
            syntheticDataSummarizing.buyTotalZhu = ParseTagValueToLong(lblBuyTotalZhu);
            syntheticDataSummarizing.winPrizeAwardTotalMoney = ParseTagValueToLong(lblWinAwardTotalMoney);
            syntheticDataSummarizing.totalConsumptionMoney = ParseTagValueToLong(lblBuyTotalMoney);

            return syntheticDataSummarizing;
        }

        /// <summary>
        /// 将循环摇奖数据页的信息转换为实体类
        /// </summary>
        private LoopDataSummarizing GetParseLabelToLoopDataSummarizing()
        {
            LoopDataSummarizing loopDataSummarizing = new LoopDataSummarizing();
            loopDataSummarizing.LoopPeriodsCount = ParseTagValueToLong(lblDoubleBallPeriods);
            loopDataSummarizing.LoopWinPrizeLotteryTotalZhu = ParseTagValueToLong(lblLoopWinTotalZhu);
            loopDataSummarizing.buyTotalZhu = ParseTagValueToLong(lblLoopBuyTotalZhu);
            loopDataSummarizing.LoopWinPrizeLotteryTotalMoney = ParseTagValueToLong(lblLoopWinAwardTotalMoney);
            loopDataSummarizing.LoopTotalConsumptionMoney = ParseTagValueToLong(lblLoopBuyTotalMoney);

            return loopDataSummarizing;
        }

        /// <summary>
        /// 刷新右侧栏双色球综合数据和循环摇奖数据
        /// </summary>
        /// <param name="synSumZing"></param>
        /// <param name="loopSumZing"></param>
        private void RefreshRightSynthesisData(SyntheticDataVariate synSumZing, LoopDataSummarizing loopSumZing)
        {
            LeeLabel winPrizeTotalZhu = null, 
                buyTotalZhu = null, winPrizeTotalMoney = null, totalConsumptionMoney = null;

            #region 累加数据来源页选择

            //停留在机选页时 汇总机选页数据
            if (CardInterface.SelectedIndex == 1)
            {
                winPrizeTotalZhu = lblRandomWinZhu;
                buyTotalZhu = lblRandomBuyZhu;
                winPrizeTotalMoney = lblRandomWinTotalMonery;
                totalConsumptionMoney = lblRandomMonetary;
            }
            //停留在自选&复试页时 汇总自选页数据
            if (CardInterface.SelectedIndex == 2)
            {
                winPrizeTotalZhu = lblComplexWinZhu;
                buyTotalZhu = lblComplexBuyZhu;
                winPrizeTotalMoney = lblComplexWPTotalMonery;
                totalConsumptionMoney = lblComplexMonetary;
            }
            //停留在自选&胆拖页时 汇总自选页数据
            if (CardInterface.SelectedIndex == 3)
            {
                winPrizeTotalZhu = lblDantuoWinZhu;
                buyTotalZhu = lblDantuoBuyZhu;
                winPrizeTotalMoney = lblDantuoWPTotalMonery;
                totalConsumptionMoney = lblDantuoMonetary;
            }

            #endregion

            if (CardInterface.SelectedIndex <= 3 && CardInterface.SelectedIndex >= 1)
            {

                #region 更新综合数据的中奖总注

                synSumZing.winPrizeAwardTotalZhu += long.Parse(winPrizeTotalZhu.Tag.ToString());

                lblWinTotalZhu.Tag = synSumZing.winPrizeAwardTotalZhu;
                lblWinTotalZhu.Text =
                    string.Format(Info.LotteryZhuCountMessage, Info.GetNumberMaxUnit(synSumZing.winPrizeAwardTotalZhu), FormatNumber(synSumZing.winPrizeAwardTotalZhu.ToString(), 5));

                #endregion

                #region 更新综合数据的购买总注

                synSumZing.buyTotalZhu += long.Parse(buyTotalZhu.Tag.ToString());

                lblBuyTotalZhu.Tag = synSumZing.buyTotalZhu;

                lblBuyTotalZhu.Text =
                    string.Format(Info.LotteryZhuCountMessage, Info.GetNumberMaxUnit(synSumZing.buyTotalZhu), FormatNumber(synSumZing.buyTotalZhu.ToString(), 5));

                #endregion

                #region 更新综合数据的中奖总额

                synSumZing.winPrizeAwardTotalMoney += long.Parse(winPrizeTotalMoney.Tag.ToString());

                lblWinAwardTotalMoney.Tag = synSumZing.winPrizeAwardTotalMoney;
                lblWinAwardTotalMoney.Text =
                    string.Format(Info.LotteryMoneyConutMessage, Info.GetNumberMaxUnit(synSumZing.winPrizeAwardTotalMoney), FormatNumber(synSumZing.winPrizeAwardTotalMoney.ToString(), 5));

                #endregion

                #region 更新综合数据的消费总额

                synSumZing.totalConsumptionMoney += long.Parse(totalConsumptionMoney.Tag.ToString());

                lblBuyTotalMoney.Tag = synSumZing.totalConsumptionMoney;
                lblBuyTotalMoney.Text =
                    string.Format(Info.LotteryMoneyConutMessage, Info.GetNumberMaxUnit(synSumZing.totalConsumptionMoney), FormatNumber(synSumZing.totalConsumptionMoney.ToString(), 5));

                #endregion

                #region 更新综合数据的盈亏情况

                lblProfitTotalMoney.Text = synSumZing.GetProfitTotalMoney();

                #endregion

                #region 更新综合数据中的公益金额

                lblCharityTotalMoney.Text = string.Format(Info.PublicBenefitTotalMoney, synSumZing.GetPublicBenefitMoney().ToString());

                #endregion

                #region 更新综合数据中的最高奖号

                if (_appearMaxNumber)
                {
                    _appearMaxNumber = false;
                    SettingColorForMaxDoubleBallPanel(panMaxDoubleBall, _historyMaxDoubleBallNumber, lblMaxBlue);
                }

                #endregion

                #region 更新综合中奖率

                lblLotteryPercent.ForeColor = _RedBallColor;
                lblLotteryPercent.Text = synSumZing.GetLotteryProbability(4).ToString() + "%";

                #endregion

                if (_refreshLoopData)
                {
                    LeeLabel loopWinPrizeTotalZhu = null, loopYearBuyTotalZhu = null,
                        loopWinPrizeTotalMoney = null, loopTotalConsumptionMoney = null;

                    #region 累加数据来源页选择

                    if (CardInterface.SelectedIndex == 1)
                    {
                        loopWinPrizeTotalZhu = lblRandomWinZhu;
                        loopYearBuyTotalZhu = lblRandomBuyZhu;
                        loopWinPrizeTotalMoney = lblRandomWinTotalMonery;
                        loopTotalConsumptionMoney = lblRandomMonetary;
                    }
                    if (CardInterface.SelectedIndex == 2)
                    {
                        loopWinPrizeTotalZhu = lblComplexWinZhu;
                        loopYearBuyTotalZhu = lblComplexBuyZhu;
                        loopWinPrizeTotalMoney = lblComplexWPTotalMonery;
                        loopTotalConsumptionMoney = lblComplexMonetary;
                    }
                    if (CardInterface.SelectedIndex == 3)
                    {
                        loopWinPrizeTotalZhu = lblDantuoWinZhu;
                        loopYearBuyTotalZhu = lblDantuoBuyZhu;
                        loopWinPrizeTotalMoney = lblDantuoWPTotalMonery;
                        loopTotalConsumptionMoney = lblDantuoMonetary;
                    }

                    #endregion

                    #region 更新循环摇奖数据的摇奖期数

                    loopSumZing.LoopPeriodsCount -= _loopPeriods;
                    lblLoopPeriods.Tag = loopSumZing.LoopPeriodsCount;

                    lblLoopPeriods.Text =
                        string.Format(Info.LotteryPeriodsCountMessage, Info.GetNumberMaxUnit(loopSumZing.LoopPeriodsCount), FormatNumber(loopSumZing.LoopPeriodsCount.ToString(), 5));

                    #endregion

                    #region 更新循环摇奖数据的中奖总注

                    loopSumZing.LoopWinPrizeLotteryTotalZhu += ParseTagValueToLong(loopWinPrizeTotalZhu);

                    lblLoopWinTotalZhu.Tag = loopSumZing.LoopWinPrizeLotteryTotalZhu;

                    lblLoopWinTotalZhu.Text =
                        string.Format(Info.LotteryZhuCountMessage, Info.GetNumberMaxUnit(loopSumZing.LoopWinPrizeLotteryTotalZhu), FormatNumber(loopSumZing.LoopWinPrizeLotteryTotalZhu.ToString(), 5));

                    #endregion

                    #region 更新循环摇奖数据的购买总注

                    loopSumZing.buyTotalZhu += ParseTagValueToLong(loopYearBuyTotalZhu);

                    lblLoopBuyTotalZhu.Tag = loopSumZing.buyTotalZhu;

                    lblLoopBuyTotalZhu.Text =
                        string.Format(Info.LotteryZhuCountMessage, Info.GetNumberMaxUnit(loopSumZing.buyTotalZhu), FormatNumber(loopSumZing.buyTotalZhu.ToString(), 5));

                    #endregion

                    #region 更新循环摇奖数据的中奖总额

                    loopSumZing.LoopWinPrizeLotteryTotalMoney += ParseTagValueToLong(loopWinPrizeTotalMoney);

                    lblLoopWinAwardTotalMoney.Tag = loopSumZing.LoopWinPrizeLotteryTotalMoney;
                    lblLoopWinAwardTotalMoney.Text =
                        string.Format(Info.LotteryMoneyConutMessage, Info.GetNumberMaxUnit(loopSumZing.LoopWinPrizeLotteryTotalMoney), FormatNumber(loopSumZing.LoopWinPrizeLotteryTotalMoney.ToString(), 5));

                    #endregion

                    #region 更新循环摇奖数据的消费总额

                    loopSumZing.LoopTotalConsumptionMoney += ParseTagValueToLong(loopTotalConsumptionMoney);

                    lblLoopBuyTotalMoney.Tag = loopSumZing.LoopTotalConsumptionMoney;
                    lblLoopBuyTotalMoney.Text =
                        string.Format(Info.LotteryMoneyConutMessage, Info.GetNumberMaxUnit(loopSumZing.LoopTotalConsumptionMoney), FormatNumber(loopSumZing.LoopTotalConsumptionMoney.ToString(), 5));

                    #endregion

                    #region 更新循环摇奖数据的最高奖号

                    if (_appearLoopMaxNumber)
                    {
                        _appearLoopMaxNumber = false;
                        SettingColorForMaxDoubleBallPanel(panLoopYear, _maxLoopDoubleBallNumber, lblLoopYearMaxBlue);
                    }

                    #endregion
                }
            }
        }

        /// <summary>
        /// 循环五年十年摇奖、统计分析选项卡数据 此方法需要在 RefreshDoubleBallTabulateData() 方法后执行
        /// 因为这里的数据用到以上方法刷新后的数据
        /// </summary>
        /// <param name="loopDataAnalyse"></param>
        private void RefreshLoopDataAnalyse(LoopDataSummarizing loopDataSummarizing)
        {
            if (_refreshLoopData)
            {
                _refreshLoopData = false;

                int selectIndex = CardInterface.SelectedIndex;

                if(selectIndex == 1)
                {
                    loopDataSummarizing.LoopWinPrizeLotteryTotalZhu += ParseTagValueToLong(lblRandomWinZhu);
                    loopDataSummarizing.LoopWinPrizeLotteryTotalMoney += ParseTagValueToLong(lblRandomWinTotalMonery);
                    loopDataSummarizing.LoopTotalConsumptionMoney += ParseTagValueToLong(lblRandomMonetary);
                    loopDataSummarizing.LoopPeriodsCount = ParseTagValueToLong(lblLoopPeriods);
                    _loopDataSummarizing.buyTotalZhu += ParseTagValueToLong(lblRandomBuyZhu);
                }
                else if (selectIndex == 2)
                {
                    loopDataSummarizing.LoopWinPrizeLotteryTotalZhu += ParseTagValueToLong(lblComplexWinZhu);
                    loopDataSummarizing.LoopWinPrizeLotteryTotalMoney += ParseTagValueToLong(lblComplexWPTotalMonery);
                    loopDataSummarizing.LoopTotalConsumptionMoney += ParseTagValueToLong(lblComplexMonetary);
                    loopDataSummarizing.LoopPeriodsCount = ParseTagValueToLong(lblLoopPeriods);
                    _loopDataSummarizing.buyTotalZhu += ParseTagValueToLong(lblComplexBuyZhu);
                }
                else
                {
                    loopDataSummarizing.LoopWinPrizeLotteryTotalZhu += ParseTagValueToLong(lblDantuoWinZhu);
                    loopDataSummarizing.LoopWinPrizeLotteryTotalMoney += ParseTagValueToLong(lblDantuoWPTotalMonery);
                    loopDataSummarizing.LoopTotalConsumptionMoney += ParseTagValueToLong(lblDantuoMonetary);
                    loopDataSummarizing.LoopPeriodsCount = ParseTagValueToLong(lblLoopPeriods);
                    _loopDataSummarizing.buyTotalZhu += ParseTagValueToLong(lblDantuoBuyZhu);
                }
            }
        }

        /**
         * 以下颜色定义只在代码动态更改中将状态内有效
         *    生成的控件颜色用的控件模板颜色
         */
        /// <summary>
        /// 红色球颜色
        /// </summary>
        private Color _RedBallColor = Color.OrangeRed;
        /// <summary>
        /// 蓝色球颜色
        /// </summary>
        private Color _BlueBallColor = Color.DodgerBlue;
        /// <summary>
        /// 单式未中奖颜色、复试红色球、或者胆拖的胆 未中奖颜色
        /// </summary>
        private Color _NotLotteryColor = Color.LightGray;
        /// <summary>
        /// 红球胆拖号的拖号颜色
        /// </summary>
        public Color _RedColorTuo = Color.DarkOrange;
        /// <summary>
        /// 胆拖号、拖未中奖的颜色
        /// </summary>
        private Color _NotRedTuoColor = Color.PeachPuff;
        /// <summary>
        /// 复试蓝色球未中奖颜色
        /// </summary>
        private Color _NotLotteryComplexBlueColor = Color.PowderBlue;

        /// <summary>
        /// 对中奖号码或者未中奖号码控件进行上色和失色处理
        /// </summary>
        /// <param name="doubleBallNumber"></param>
        /// <param name="myDoubleBallNumber">我的中奖类型</param>
        /// <param name="publicDoubleNumber">开奖号码</param>
        public void DrawingLabelColorLotteryNumber(Panel doubleBallNumber, DoubleBall myNumber, SimplexDoubleBallNumber publicDoubleNumber)
        {
            int selectIndex = CardInterface.SelectedIndex;

            switch (selectIndex)
            {
                case 1:
                    DrawingSimplexColorLotteryNumber(doubleBallNumber, myNumber, publicDoubleNumber);
                    break;
                case 2:
                    DrawingComplexColorLotteryNumber(doubleBallNumber, myNumber, publicDoubleNumber);
                    break;
                case 3:
                    DrawingDantuoColorLotteryNumber(doubleBallNumber, myNumber, publicDoubleNumber);
                    break;
            }
        }

        /// <summary>
        /// 机选单式号码的中奖情况颜色绘制
        /// </summary>
        public void DrawingSimplexColorLotteryNumber(Panel doubleBallNumber, DoubleBall myNumber, SimplexDoubleBallNumber publicDoubleNumber)
        {
            foreach (Control control in doubleBallNumber.Controls)
            {
                if (control is Label label)
                {
                    string ballColorType = label.Tag.ToString();

                    //未中奖情况下
                    if (myNumber.awardType == AwardType.NotAward)
                    {
                        if (!ballColorType.Equals("No"))
                        {
                            label.ForeColor = _NotLotteryColor;
                        }
                    }

                    else if (myNumber.awardType != AwardType.NotAward)
                    {
                        //红球中奖情况下
                        if (ballColorType.IndexOf("Red") != -1)
                        {
                            SettingRedColor(publicDoubleNumber, label);
                        }

                        //蓝球中奖情况下
                        if (ballColorType.Equals("Blue"))
                        {
                            SettingBlueColor(publicDoubleNumber, label, _BlueBallColor, _NotLotteryColor);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 自选&复试号码的中奖情况颜色绘制
        /// </summary>
        private void DrawingComplexColorLotteryNumber(Panel doubleBallNumber, DoubleBall myNumber, SimplexDoubleBallNumber publicDoubleNumber)
        {
            foreach (Control control in doubleBallNumber.Controls)
            {
                if (control is Label label)
                {
                    string ballColorType = label.Tag.ToString();

                    //未中奖情况下
                    if (myNumber.awardType == AwardType.NotAward)
                    {
                        if (!ballColorType.Equals("No"))
                        {
                            bool isRed = ballColorType.IndexOf("Red") != -1;
                            label.ForeColor = isRed ? _NotLotteryColor : _NotLotteryComplexBlueColor;
                        }
                    }
                    else if (myNumber.awardType != AwardType.NotAward)
                    {
                        if (ballColorType.IndexOf("Red") != -1)
                        {
                            SettingRedColor(publicDoubleNumber, label);
                        }

                        if (ballColorType.Equals("Blue"))
                        {
                            SettingBlueColor(publicDoubleNumber, label, _BlueBallColor, _NotLotteryComplexBlueColor);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 自选&胆拖号码的中奖情况颜色绘制
        /// </summary>
        private void DrawingDantuoColorLotteryNumber(Panel doubleBallNumber, DoubleBall myNumber, SimplexDoubleBallNumber publicDoubleNumber)
        {
            //拖号理应上色的数量
            int tuoShouldCount = 0;
            //拖号已经上色的数量
            int tuoAlreadyCount = 0;

            if (CardInterface.SelectedIndex == 3)
            {
                tuoShouldCount = 6 - (myNumber as DantuoDoubleBallNumber).redBallDanCount;
            }

            foreach (Control control in doubleBallNumber.Controls)
            {
                if (control is Label label)
                {
                    string ballColorType = label.Tag.ToString();
                    bool isRedTuo = ballColorType.Equals("TRed");

                    //未中奖情况下
                    if (myNumber.awardType == AwardType.NotAward)
                    {
                        if (!ballColorType.Equals("No"))
                        {
                            label.ForeColor = GetNotWinDantuoColorType(ballColorType);
                        }
                    }
                    //中奖情况下
                    else if (myNumber.awardType != AwardType.NotAward)
                    {
                        //红球胆拖号中奖情况
                        if (ballColorType.IndexOf("Red") != -1)
                        {
                            #region 胆拖号的颜色特殊逻辑

                            int ballValue = int.Parse(label.Text);
                            if (publicDoubleNumber.redBalls.Contains(ballValue))
                            {
                                //拖号
                                if (isRedTuo)
                                {
                                    tuoAlreadyCount++;
                                    Color dantuoColor = _NotRedTuoColor;

                                    if (tuoAlreadyCount <= tuoShouldCount)
                                    {
                                        dantuoColor = _RedColorTuo;
                                    }

                                    label.ForeColor = dantuoColor;
                                }
                                //胆号
                                else
                                {
                                    label.ForeColor = _RedBallColor;
                                }
                            }
                            else
                            {
                                label.ForeColor = isRedTuo ? _NotRedTuoColor : _NotLotteryColor;
                            }

                            #endregion
                        }

                        //蓝球中奖情况
                        else if (ballColorType.Equals("Blue"))
                        {
                            SettingBlueColor(publicDoubleNumber, label, _BlueBallColor, _NotLotteryComplexBlueColor);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取胆拖未中奖颜色类型
        /// </summary>
        /// <param name="ballColorType"></param>
        /// <returns></returns>
        private Color GetNotWinDantuoColorType(string ballColorType)
        {
            switch (ballColorType)
            {
                case "Red": return _NotLotteryColor;
                case "TRed": return _NotRedTuoColor;
                case "Blue": return _NotLotteryComplexBlueColor;
            }
            return _NotLotteryColor;
        }

        /// <summary>
        /// 设置红球颜色
        /// </summary>
        private void SettingRedColor(SimplexDoubleBallNumber publicDoubleNumber, Label label)
        {
            int ballValue = int.Parse(label.Text);
            Color lotteryColor = _NotLotteryColor;

            if (publicDoubleNumber.redBalls.Contains(ballValue))
            {
                lotteryColor = _RedBallColor;
            }

            label.ForeColor = lotteryColor;
        }

        /// <summary>
        /// 设置蓝球颜色
        /// </summary>
        /// <param name="winLotteryColor">中奖颜色</param>
        /// <param name="notLotteryColor">未中奖颜色</param>
        private void SettingBlueColor(SimplexDoubleBallNumber publicDoubleNumber, Label label, Color winLotteryColor, Color notLotteryColor)
        {
            int ballValue = int.Parse(label.Text);
            Color lotteryColor = notLotteryColor;

            if (ballValue == publicDoubleNumber.blueBall)
            {
                lotteryColor = winLotteryColor;
            }
            label.ForeColor = lotteryColor;
        }

        /// <summary>
        /// 绘制最高奖的中奖颜色显示
        /// </summary>
        private void SettingColorForMaxDoubleBallPanel(Panel maxDoubleBallPanel, SimplexDoubleBallNumber maxSimplexDoubleBall, Label labelBlue)
        {
            int maxRedBallIndex = 0;

            SimplexDoubleBallNumber _lotteryDoubleNumber = _publicDoubleNumber;

            if (isRandomRun)
            {
                _lotteryDoubleNumber = maxDoubleBallPanel.Name.Equals("panMaxDoubleBall") ? _historyMaxPublicDoubleBallNumber : _maxLoopLotteryBoubleBallNumber;
            }

            foreach (Label label in maxDoubleBallPanel.Controls)
            {
                if (label.Tag.ToString() == "Red")
                {
                    if (_lotteryDoubleNumber.redBalls.Contains(maxSimplexDoubleBall.redBalls[maxRedBallIndex]))
                    {
                        label.ForeColor = _RedBallColor;
                    }
                    else
                    {
                        label.ForeColor = _NotLotteryColor;
                    }

                    int redBallValue = maxSimplexDoubleBall.redBalls[maxRedBallIndex];
                    label.Text = FormatNumber(redBallValue, 2);
                    maxRedBallIndex++;
                }
            }

            labelBlue.Text = FormatNumber(maxSimplexDoubleBall.blueBall, 2);
            if (_lotteryDoubleNumber.blueBall == maxSimplexDoubleBall.blueBall)
            {
                labelBlue.ForeColor = _BlueBallColor;
            }
            else
            {
                labelBlue.ForeColor = _NotLotteryColor;
            }
        }

        #region Oneself Select And Complex

        /// <summary>
        /// 自选双色球之间的间隔
        /// </summary>
        public const int _INTERVAL = 22;
        /// <summary>
        /// 手选或者随机生成的复试双色球的数量
        /// </summary>
        public static int _ComplexDoubleBallDoubleBallCount = 0;
        /// <summary>
        /// 最高复试和胆拖自选号的总数
        /// </summary>
        public static readonly int _MaxSerialNumberTotal = 100;
        /// <summary>
        /// 最高自选号的总注数限制
        /// </summary>
        public static readonly int _MaxOneselfDoubleBallTotalZhu = 1000000;

        /// <summary>
        /// 自选复试号辅助生成控件
        /// </summary>
        private OneselfControlHelper _ComplexHelper;
        /// <summary>
        /// 单次选择的复试双色球号码
        /// </summary>
        private ComplexDoubleBallNumber _ComplexDoubleBallNumber;
        /// <summary>
        /// 复试双色球我的号码集合
        /// </summary>
        private List<ComplexDoubleBallNumber> _myComplexDoubleBallNumberList;

        #region 滚动条相关API

        private const int WS_HSCROLL = 0x100000;
        private const int WS_VSCROLL = 0x200000;
        private const int GWL_STYLE = (-16);

        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern int GetWindowLong(IntPtr hwnd, int nIndex);

        /// <summary>
        /// 判断是否出现垂直滚动条
        /// </summary>
        /// <param name="ctrl">待测控件</param>
        internal static bool IsVerticalScrollBarVisible(Control ctrl)
        {
            if (!ctrl.IsHandleCreated)
                return false;

            return (GetWindowLong(ctrl.Handle, GWL_STYLE) & WS_VSCROLL) != 0;
        }

        /// <summary>
        /// 判断是否出现水平滚动条
        /// </summary>
        /// <param name="ctrl">待测控件</param>
        internal static bool IsHorizontalScrollBarVisible(Control ctrl)
        {
            if (!ctrl.IsHandleCreated)
                return false;
            return (GetWindowLong(ctrl.Handle, GWL_STYLE) & WS_HSCROLL) != 0;
        }

        #endregion

        /// <summary>
        /// 自选复试号码点击生成一个数字
        /// </summary>
        public void OneselfSelectComplexNumber(int ballType, int number)
        {
            if (_ComplexDoubleBallNumber == null)
                _ComplexDoubleBallNumber = new ComplexDoubleBallNumber();

            if (ballType == 0) _ComplexDoubleBallNumber.AddRedBall(number);
            else
            {
                _ComplexDoubleBallNumber.AddBlueBall(number);
            }

            CreateComplexDoubleBallControl(ballType, number);
        }

        /// <summary>
        /// 重置我的复试号码
        /// </summary>
        private void btnResetOneself_Click(object sender, EventArgs e)
        {
            Logger.Info($"reset my complex number : {_myComplexDoubleBallNumberList.Count} row");

            _CreateIndexByComplex = 0;
            _ComplexDoubleBallDoubleBallCount = 0;
            _myComplexDoubleBallNumberList.Clear();

            flpComplexNumber.Controls.Clear();
            _OneselfComplex?.ResetDoubleBallNumber();

            lblComplexBuyZhu.Tag = 0;
            lblComplexBuyZhu.Text = "[零] 00000";
            lblComplexMonetary.Tag = 0;
            lblComplexMonetary.Text = "[零] 00000";

            panComplexStatistics.Visible = false;

            if(_ComplexTipInfo != null) _ComplexTipInfo.RemoveAll();

            ResetComplexVariate();
        }

        #region 手选复试双色球号码逻辑

        /// <summary>
        /// 重置复试自选号标识
        /// </summary>
        private void ResetComplexVariate()
        {
            _ComplexHelper.Reset();
            _ComplexDoubleBallNumber = null;
        }

        /// <summary>
        /// 创建承载手选双色球的控件
        /// </summary>
        private void CreateComplexDoubleBallControl(int ballType, int number)
        {
            _ComplexHelper.oneselfStatus = false;

            if (_ComplexHelper.oneselfPanel == null)
                InitialFirstControls(_ComplexDoubleBallNumber, _ComplexHelper, ballType, number);
            else
            {
                ContinueAddDoubleBallToControls(
                    _ComplexDoubleBallNumber, _ComplexHelper, ballType, number);
            }

            if (IsHorizontalScrollBarVisible(flpComplexNumber))
            {
                flpComplexNumber.HorizontalScroll.Value = flpComplexNumber.HorizontalScroll.Maximum;
            }
        }

        /// <summary>
        /// 继续添加自选双色球号码
        /// </summary>
        private void ContinueAddDoubleBallToControls(DoubleBall _OneselfNumberDoubleBall, OneselfControlHelper Chelper, int ballType, int number)
        {
            //继续选择添加自选双色球号码
            Label ballOne = new Label();
            ballOne.Text = FormatNumber(number, 2);

            ballOne.MouseEnter += LabelBall_MouseEnter;
            ballOne.MouseLeave += LabelBall_MouseLeave;
            ballOne.Click += lblComplexRollBack_Click;

            if (ballType == 0) 
                CopyComplexLabelStyle(lblComplexRed, ballOne);
            else if (ballType == 1) 
                CopyComplexLabelStyle(lblRedBallTuo, ballOne);
            else 
                CopyComplexLabelStyle(lblComplexBlue, ballOne);

            //调整新添加的球的位置
            ballOne.Location = new Point(Chelper.cumsumPoint.X + _INTERVAL, Chelper.cumsumPoint.Y);
            Chelper.cumsumPoint = ballOne.Location;

            //调整Panel的大小
            Chelper.oneselfPanel.Size = new Size(Chelper.oneselfPanel.Width + _INTERVAL, Chelper.oneselfPanel.Height);
            Chelper.oneselfPanel.Controls.Add(ballOne);

            //如果已经创建了图标、颠倒图标与新创建的号码位置
            if (Chelper.isCreateOKImage)
            {
                Point tempPoint = ballOne.Location;
                ballOne.Location = Chelper.buttonImage.Location;
                Chelper.buttonImage.Location = tempPoint;
            }

            //如果符合双色球成号条件就追加一个图标
            if (_OneselfNumberDoubleBall.IsOKNumber() && !Chelper.isCreateOKImage)
            {
                Chelper.isCreateOKImage = true;
                Chelper.buttonImage = new PictureBox();

                if (CardInterface.SelectedIndex == 2) Chelper.buttonImage.Click += picComplexAccomplish_Click;
                else
                {
                    Chelper.buttonImage.Click += picDantuoAccomplish_Click;
                }
                
                Chelper.buttonImage.Location = new Point(Chelper.cumsumPoint.X + _INTERVAL, Chelper.cumsumPoint.Y);

                CopyPictureStyle(picComplexAccomplish, Chelper.buttonImage);

                Chelper.cumsumPoint = Chelper.buttonImage.Location;
                Chelper.oneselfPanel.Size = new Size(Chelper.oneselfPanel.Width + _INTERVAL - 2, Chelper.oneselfPanel.Height);
                Chelper.oneselfPanel.Controls.Add(Chelper.buttonImage);
            }
        }

        /// <summary>
        /// 我的自选号码未成型阶段、点击删除自选号码
        /// </summary>
        private void lblComplexRollBack_Click(object sender, EventArgs e)
        {
            Label number = (sender as Label);
            RollBackDoubleBallNumber(number);
        }

        /// <summary>
        /// 未成形阶段删除自选号
        /// </summary>
        private void RollBackDoubleBallNumber(Label number)
        {
            Label rollBackBall = null;
            Point lastBall = new Point();
            bool isComplex = CardInterface.SelectedIndex == 2;

            RemoveBallToMyDoubleBallNumber(number);

            RemoveLabelAndChangeLocation(number, ref lastBall, ref rollBackBall);

            if (isComplex)
            {
                ResetSizeAndSettingFlag(_ComplexHelper, _ComplexDoubleBallNumber, number, rollBackBall);
            }
            else
            {
                ResetSizeAndSettingFlag(_DantuoHelper, _DantuoDoubleBallNumber, number, rollBackBall);
            }
        }

        /// <summary>
        /// 如果号码成型了移动图片控件位置、或者删除后又未满足成型条件就去除图片控件
        /// </summary>
        private void ResetSizeAndSettingFlag(OneselfControlHelper Chelper, DoubleBall doubleBall, Label number, Label rollBackBall)
        {
            if (Chelper.isCreateOKImage)
            {
                Chelper.buttonImage.Location = new Point(
                    Chelper.buttonImage.Location.X - _INTERVAL, Chelper.buttonImage.Location.Y);

                if (!doubleBall.IsOKNumber())
                {
                    Chelper.buttonImage.Dispose();
                    Chelper.buttonImage = null;
                    Chelper.isCreateOKImage = false;

                    Chelper.cumsumPoint = new Point(Chelper.cumsumPoint.X - _INTERVAL, Chelper.cumsumPoint.Y);
                    Chelper.oneselfPanel.Size = new Size(Chelper.oneselfPanel.Width - _INTERVAL, Chelper.oneselfPanel.Height);
                }
            }

            //删除红球控件后位置需要做减法
            Chelper.cumsumPoint = new Point(Chelper.cumsumPoint.X - _INTERVAL, Chelper.cumsumPoint.Y);
            Chelper.oneselfPanel.Size = new Size(Chelper.oneselfPanel.Width - _INTERVAL, Chelper.oneselfPanel.Height);

            if (CardInterface.SelectedIndex == 2)
                _OneselfComplex.RollBackSelectBall("Red".Equals(number.Tag), int.Parse(number.Text));
            else
                _OneselfDantuo.RollBackSelectBall(number.Tag.ToString(), int.Parse(number.Text));

            //如果删的一个球都不剩就释放并重置掉承载本次号码的控件
            if (doubleBall.isBallEmpty())
            {
                Chelper.oneselfPanel.Dispose();
                Chelper.oneselfStatus = true;

                if(CardInterface.SelectedIndex == 2)
                {
                    ResetComplexVariate();
                    _ComplexDoubleBallDoubleBallCount--;
                }
                else
                {
                    ResetDantuoVariate();
                    _DantuoDoubleBallDoubleBallCount--;
                }
            }

            rollBackBall.Dispose();
        }

        /// <summary>
        /// 复试自选号点击完成选项
        /// </summary>
        private void picComplexAccomplish_Click(object sender, EventArgs e)
        {
            _ComplexDoubleBallNumber.OrderDoubleBalls();
            _myComplexDoubleBallNumberList.Add(_ComplexDoubleBallNumber);

            #region 重新给承载双色球的Label赋值

            int redIndex = 0;
            int blueIndex = 0;

            foreach (var im in _ComplexHelper.oneselfPanel.Controls)
            {
                if (im is Label doubleBall && doubleBall.Tag.ToString() != "No")
                {
                    //移除已选定号码的相关事件
                    doubleBall.MouseEnter -= LabelBall_MouseEnter;
                    doubleBall.MouseLeave -= LabelBall_MouseLeave;
                    doubleBall.Click -= lblComplexRollBack_Click;

                    if (redIndex <= _ComplexDoubleBallNumber.redBallCount - 1)
                    {
                        doubleBall.Tag = "Red";
                        doubleBall.ForeColor = _RedBallColor;
                        doubleBall.Text = FormatNumber(_ComplexDoubleBallNumber.redBalls[redIndex++], 2);
                    }
                    else
                    {
                        doubleBall.Tag = "Blue";
                        doubleBall.ForeColor = _BlueBallColor;
                        doubleBall.Text = FormatNumber(_ComplexDoubleBallNumber.blueBalls[blueIndex++], 2);
                    }
                }
            }

            #endregion

            _ComplexHelper.buttonImage.Click -= picComplexAccomplish_Click;
            _ComplexHelper.buttonImage.Click += picAccomplishDelete_Click;
            _ComplexHelper.buttonImage.Image = Properties.Resources.delete1;

            _CreateIndexByComplex++;
            _ComplexHelper.oneselfStatus = true;

            RefreshOneselfNumberMoneyByComplex(true, _ComplexDoubleBallNumber);

            string selfCount = $"[{_ComplexDoubleBallNumber.redBallCount}-{_ComplexDoubleBallNumber.blueBallCount}]";
            SetDoubleBallInfo(_ComplexHelper.oneselfLabelNo, selfCount);

            _OneselfComplex.ResetDoubleBallNumber();
            ResetComplexVariate();

            Logger.Info($"self complex number : {selfCount}");
        }

        /// <summary>
        /// 初始化并生成自选号初次选择的号码
        /// </summary>
        private void InitialFirstControls(DoubleBall doubleBall, OneselfControlHelper Chelper, int ballType, int number)
        {
            bool isComplex = doubleBall is ComplexDoubleBallNumber;

            doubleBall.serialNumber = isComplex ? ++_ComplexDoubleBallDoubleBallCount : ++_DantuoDoubleBallDoubleBallCount;

            Chelper.oneselfPanel = new Panel();
            Chelper.oneselfPanel.Size = Chelper.initPanelSize;
            Chelper.oneselfPanel.Tag = doubleBall.serialNumber;

            //生成复试序号Label
            Chelper.oneselfLabelNo = new Label();
            Chelper.oneselfLabelNo.Text = doubleBall.serialNumber + ">";
            CopyLabelStyle(lblRandomNo, Chelper.oneselfLabelNo);

            //生成第一个号码
            Label ball = new Label();
            ball.Text = FormatNumber(number, 2);

            if (ballType == 0) CopyLabelStyle(lblComplexRed, ball);
            if (ballType == 1)
            {
                ball.Location = lblRedBallA.Location;
                CopyComplexLabelStyle(lblRedBallTuo, ball);
            }
            if (ballType == 2)
            {
                ball.Location = lblRedBallA.Location;
                CopyComplexLabelStyle(lblComplexBlue, ball);
            }

            ball.MouseEnter += LabelBall_MouseEnter;
            ball.MouseLeave += LabelBall_MouseLeave;
            ball.Click += lblComplexRollBack_Click;

            Chelper.oneselfPanel.Controls.Add(ball);
            Chelper.oneselfPanel.Controls.Add(Chelper.oneselfLabelNo);

            Chelper.LayoutPanel.Controls.Add(Chelper.oneselfPanel);
        }

        /// <summary>
        /// 删除红球控件、其它红球位置依次往前移动
        /// </summary>
        private void RemoveLabelAndChangeLocation(Label number, ref Point lastBall, ref Label rollBackBall)
        {
            int index = CardInterface.SelectedIndex;

            foreach (Control item in index == 2 ? _ComplexHelper.oneselfPanel.Controls : _DantuoHelper.oneselfPanel.Controls)
            {
                if ("No" != item.Tag.ToString())
                {
                    //从删除红球的位置开始移动后面球的位置
                    if (item is Label && rollBackBall != null)
                    {
                        Point tempLocation = item.Location;
                        item.Location = lastBall;
                        lastBall = tempLocation;

                        lastBall = tempLocation;
                    }

                    //记录从开始删除的红球的位置
                    if (item == number)
                    {
                        lastBall = item.Location;
                        rollBackBall = item as Label;
                    }
                }
            }
        }

        /// <summary>
        /// 从自选号码中移除我的选择号码
        /// </summary>
        /// <param name="number"></param>
        private void RemoveBallToMyDoubleBallNumber(Label number)
        {
            string ballType = number.Tag.ToString();
            int selectIndex = CardInterface.SelectedIndex;

            if (selectIndex == 2)
            {
                if (ballType.Equals("Red"))
                    _ComplexDoubleBallNumber.RemoveRedBall(int.Parse(number.Text));
                else
                    _ComplexDoubleBallNumber.RemoveBlueBall(int.Parse(number.Text));
            }
            else
            {
                if (ballType.Equals("Red"))
                    _DantuoDoubleBallNumber.RemoveRedBallDan(int.Parse(number.Text));
                else if (ballType.Equals("TRed"))
                    _DantuoDoubleBallNumber.RemoveRedBallTuo(int.Parse(number.Text));
                else
                    _DantuoDoubleBallNumber.RemoveBlueBall(int.Parse(number.Text));
            }
        }

        #endregion

        #region 机选复试双色球号码逻辑

        private void lblComplexRandom1_Click(object sender, EventArgs e)
        {
            #region Check Create Count

            if (_ComplexDoubleBallNumber != null && !_ComplexHelper.oneselfStatus)
            {
                Info.ShowWarningMessage(Info.UnfinishedOneselfNumber);
                return;
            }

            int totalZhu = int.Parse(lblComplexBuyZhu.Tag.ToString());

            if (totalZhu >= _MaxOneselfDoubleBallTotalZhu || (totalZhu <= _MaxOneselfDoubleBallTotalZhu && _myComplexDoubleBallNumberList.Count >= _MaxSerialNumberTotal))
            {
                Info.ShowWarningMessage(Info.OneselfRestrict);
                return;
            }

            #endregion

            int redCount = 0;
            int blueCount = 0;
            ParseComplexCombination(ref redCount, ref blueCount);

            int buyTotalZhu = ParseTagValueToInt(lblComplexBuyZhu);
            int complexCount = ParseTagValueToInt((LeeLabel)sender);

            complexCount = AmendMaxCount(complexCount, _ComplexDoubleBallDoubleBallCount, _MaxSerialNumberTotal);

            int createIndex = 0;
            //生成机选双色球号码
            for (int i = 0; i < complexCount; i++)
            {
                //动态调整生成的注数
                CreateComplexRandomCombination(ref redCount, ref blueCount);
                DynamicAmendMaxComplexTotalZhu(ref redCount, ref blueCount, ref buyTotalZhu);

                if (buyTotalZhu <= _MaxOneselfDoubleBallTotalZhu)
                {
                    createIndex++;
                    ComplexDoubleBallNumber doubleBallNumber = _doubleBallToo.GetComplexDoubleBallNumber(redCount, blueCount);
                    _myComplexDoubleBallNumberList.Add(doubleBallNumber);
                }
                else break;
            }

            //调整创建的号码数量
            if (createIndex != complexCount) complexCount = createIndex;

            RefreshOneselfNumberMoneyByComplexList(_myComplexDoubleBallNumberList.Cast<DoubleBall>().ToList());

            //显示机选双色球号码
            CreateAllComplexDoubleBalls(complexCount, true);

            Logger.Info($"random {complexCount} complex number.");
        }

        /// <summary>
        /// 动态调整复试号生成的注数限制在最大值
        /// </summary>
        /// <param name="redCount">红球生成的数量</param>
        /// <param name="blueCount">蓝球生成的数量</param>
        /// <param name="buyTotalZhu">目前已购买的总注数</param>
        private void DynamicAmendMaxComplexTotalZhu(ref int redCount, ref int blueCount, ref int buyTotalZhu)
        {
            bool redSubtraction = false;

            while (_doubleBallToo.GetComplexCombinationTotalZhu(redCount, blueCount) + buyTotalZhu > _MaxOneselfDoubleBallTotalZhu)
            {
                //红球个数大于6 并且 (轮到红球减 或者 蓝球数量只剩1减无可减)
                if (redCount > 6 && (redSubtraction || blueCount == 1))
                {
                    redCount--;
                    redSubtraction = false;
                }
                //蓝球数量需要大于 1 才可减
                else if (blueCount > 1)
                {
                    blueCount--;
                    redSubtraction = true;
                }

                if (redCount == 6 && blueCount == 1) break;
            }

            buyTotalZhu += _doubleBallToo.GetComplexCombinationTotalZhu(redCount, blueCount);
        }

        private int _CreateIndexByComplex = 0;
        /// <summary>
        /// 循环创建我的机选号复试号码
        /// </summary>
        /// <param name="complexCount">创建数量</param>
        /// <param name="showImage">是否显示图标</param>
        private void CreateAllComplexDoubleBalls(int complexCount, bool showImage)
        {
            for (int i = _CreateIndexByComplex; i < _myComplexDoubleBallNumberList.Count; i++)
            {
                CreateComplexAndShowDoubleBall(_myComplexDoubleBallNumberList[i], showImage);
            }

            _CreateIndexByComplex += complexCount;
        }

        /// <summary>
        /// 创建一个显示复试号码的控件
        /// </summary>
        /// <param name="showImage">是否显示图标</param>
        /// <param name="complexDoubleBallNumber">要创建的复试号码</param>
        private void CreateComplexAndShowDoubleBall(ComplexDoubleBallNumber complexDoubleBall, bool showImage)
        {
            _ComplexHelper.oneselfPanel = new Panel();

            int redWidth = complexDoubleBall.redBallCount * _INTERVAL;
            int blueWidth = complexDoubleBall.blueBallCount * _INTERVAL;
            int cumsumWidth = redWidth + blueWidth - 2;

            //如果不创建图标Panel的宽度 -22 ，正好遮挡住图标
            if (!showImage) cumsumWidth -= _INTERVAL;

            _ComplexHelper.oneselfPanel.Size = new Size(
                _ComplexHelper.initPanelSize.Width + cumsumWidth, _ComplexHelper.initPanelSize.Height);

            _ComplexHelper.oneselfPanel.Tag = complexDoubleBall.serialNumber;

            Label serial = AddSerialFirstRedBall(complexDoubleBall, _ComplexHelper);

            SetDoubleBallInfo(serial, complexDoubleBall.redBallCount + "-" + complexDoubleBall.blueBallCount);

            //继续添加红色球号码
            AddDoubleBallsLabel(1,
                complexDoubleBall.redBallCount, complexDoubleBall.redBalls, lblComplexRed, _ComplexHelper);

            //添加蓝色双色球号码
            AddDoubleBallsLabel(0,
                complexDoubleBall.blueBallCount, complexDoubleBall.blueBalls, lblComplexBlue, _ComplexHelper);

            //创建最后的图标
            AddHandleButtonImage(_ComplexHelper, flpComplexNumber, showImage);

            ResetComplexVariate();
        }

        /// <summary>
        /// 动态调整复试双色球Panel控件的号码
        /// </summary>
        private void DynamicRegulationComplexNumberLabels(Panel complexPanel, ComplexDoubleBallNumber cdbn)
        {
            int redIndex = 0;
            int blueIndex = 0;
            int endLocationX = 0;

            int addIndex = 0;
            Label[] disposeLabels = new Label[35];
            PictureBox operImage = null;

            //1、改变现有Label的双色球号码阶段
            foreach (var complexNumber in complexPanel.Controls)
            {
                if (complexNumber is Label no && "No".Equals(no.Tag))
                {
                    SetDoubleBallInfo(no, cdbn.redBallCount + "-" + cdbn.blueBallCount);
                }
                else if (complexNumber is Label complex && !"No".Equals(complex.Tag))
                {
                    if (redIndex == cdbn.redBallCount && blueIndex == cdbn.blueBallCount)
                    {
                        disposeLabels[addIndex++] = complex;
                        continue;
                    }

                    //先改变现有红球为新的号码、并记录已改变的红球下标
                    if ("Red".Equals(complex.Tag))
                    {
                        /**
                         * 如果现有红色球控件足以显示完新的红色球号码
                         *   就改变剩下的红色球控件去承载新的蓝色球号码
                         */
                        if (redIndex == cdbn.redBallCount)
                        {
                            complex.Tag = "Blue";
                            complex.ForeColor = _BlueBallColor;
                            complex.Text = FormatNumber(cdbn.blueBalls[blueIndex], 2);
                            blueIndex++;
                        }
                        else
                        {
                            complex.Text = FormatNumber(cdbn.redBalls[redIndex], 2);
                            redIndex++;
                        }
                    }
                    else
                    {
                        /**
                         * 如果现有红色球控件不足以显示完新红色球号码
                         *   就改变剩下的蓝色球控件去承载新的红色球号码
                         */
                        if (redIndex < cdbn.redBallCount)
                        {
                            complex.Tag = "Red";
                            complex.ForeColor = _RedBallColor;
                            complex.Text = FormatNumber(cdbn.redBalls[redIndex], 2);
                            redIndex++;
                        }
                        else
                        {
                            complex.Text = FormatNumber(cdbn.blueBalls[blueIndex], 2);
                            blueIndex++;
                        }
                    }

                    endLocationX = complex.Location.X;
                }
                else if (complexNumber is PictureBox ptx)
                {
                    operImage = ptx;
                }
            }

            int spacingDistance = 0;

            //2、如果控件足以显示完全新的复试号码并且多余、就释放多余的控件
            if (addIndex != 0)
            {
                if (operImage != null && operImage.Visible)
                {
                    addIndex++;
                    operImage.Location = new Point(disposeLabels[0].Location.X, operImage.Location.Y);
                    operImage.Visible = false;
                }

                for (int i = 0; i < disposeLabels.Length; i++)
                {
                    disposeLabels[i]?.Dispose();
                }

                complexPanel.Size = new Size(complexPanel.Width - addIndex * _INTERVAL, complexPanel.Height);
            }
            //3、控件不足以承载双色球号码生成控件阶段
            else if (redIndex < cdbn.redBallCount || blueIndex < cdbn.blueBallCount)
            {
                if (redIndex < cdbn.redBallCount)
                {
                    for (int i = redIndex; i < cdbn.redBallCount; i++)
                    {
                        Label ballOne = new Label();
                        ballOne.Text = FormatNumber(cdbn.redBalls[i], 2);

                        CopyComplexLabelStyle(lblComplexRed, ballOne);

                        ballOne.Location = new Point(endLocationX + _INTERVAL, _ComplexHelper.cumsumPoint.Y);
                        endLocationX = ballOne.Location.X;

                        spacingDistance++;
                        complexPanel.Controls.Add(ballOne);
                    }
                }

                if (blueIndex < cdbn.blueBallCount)
                {
                    for (int i = blueIndex; i < cdbn.blueBallCount; i++)
                    {
                        Label ballOne = new Label();
                        ballOne.Text = FormatNumber(cdbn.blueBalls[i], 2);

                        CopyComplexLabelStyle(lblComplexBlue, ballOne);

                        ballOne.Location = new Point(endLocationX + _INTERVAL, _ComplexHelper.cumsumPoint.Y);
                        endLocationX = ballOne.Location.X;

                        spacingDistance++;
                        complexPanel.Controls.Add(ballOne);
                    }
                }
            }

            if (operImage != null && operImage.Visible)
            {
                spacingDistance--;
                operImage.Location = new Point(endLocationX + _INTERVAL, operImage.Location.Y);
                operImage.Visible = false;
            }

            complexPanel.Size = new Size(complexPanel.Width + spacingDistance * _INTERVAL, complexPanel.Height);
        }

        /// <summary>
        /// 累加刷新复试自选号金额注数
        /// </summary>
        /// <param name="complexDoubleBall"></param>
        private void RefreshOneselfNumberMoneyByComplexList(List<DoubleBall> _doubleBallList)
        {
            long totalZhu = 0, buyTotalMoney = 0;
            int multiple = int.Parse(cmbMultiple.Text);

            _doubleBallList.ForEach(db =>
            {
                int combination = (db as ComplexDoubleBallNumber).GetDoubleBallCombination();
                totalZhu += combination;
                buyTotalMoney += combination * 2 * multiple;
            });

            lblComplexBuyZhu.Tag = totalZhu;
            lblComplexBuyZhu.Text = string.Format("[{0}] {1}", Info.GetNumberMaxUnit(totalZhu), FormatNumber(totalZhu.ToString(), 5));

            lblComplexMonetary.Tag = buyTotalMoney;
            lblComplexMonetary.Text = string.Format("[{0}] {1}", Info.GetNumberMaxUnit(buyTotalMoney), FormatNumber(buyTotalMoney.ToString(), 5));
        }

        /// <summary>
        /// 刷新自选号金额和显示
        /// </summary>
        /// <param name="isAdd">是否累加</param>
        private void RefreshOneselfNumberMoneyByComplex(bool isAdd, ComplexDoubleBallNumber complexDoubleBall)
        {
            int multiple = int.Parse(cmbMultiple.Text);

            int totalZhu = ParseTagValueToInt(lblComplexBuyZhu);
            if (isAdd)
            {
                lblComplexBuyZhu.Tag = totalZhu + complexDoubleBall.GetDoubleBallCombination();
            }
            else
            {
                lblComplexBuyZhu.Tag = totalZhu - complexDoubleBall.GetDoubleBallCombination();
            }

            lblComplexBuyZhu.Text =
                string.Format("[{0}] {1}", Info.GetNumberMaxUnit(ParseTagValueToInt(lblComplexBuyZhu)), FormatNumber(lblComplexBuyZhu.Tag.ToString(), 5));

            int totalMoney = ParseTagValueToInt(lblComplexMonetary);

            if (isAdd)
            {
                lblComplexMonetary.Tag = totalMoney + complexDoubleBall.GetDoubleBallCombination() * 2 * multiple;
            }
            else
            {
                lblComplexMonetary.Tag = totalMoney - complexDoubleBall.GetDoubleBallCombination() * 2 * multiple;
            }

            lblComplexMonetary.Text =
                string.Format("[{0}] {1}", Info.GetNumberMaxUnit(ParseTagValueToInt(lblComplexMonetary)), FormatNumber(lblComplexMonetary.Tag.ToString(), 5));
        }

        #endregion

        #region Style

        /// <summary>
        /// 从复试号中删除双色球
        /// </summary>
        private void picAccomplishDelete_Click(object sender, EventArgs e)
        {
            int selectIndex = CardInterface.SelectedIndex;

            if (selectIndex == 2)
            {
                RemoveDoubleBallNumberByControl(sender, flpComplexNumber, selectIndex, ref _CreateIndexByComplex, ref _ComplexDoubleBallDoubleBallCount);
            }
            else
            {
                RemoveDoubleBallNumberByControl(sender, flpDantuoNumber, selectIndex, ref _CreateIndexByDantuo, ref _DantuoDoubleBallDoubleBallCount);
            }
        }

        private void RemoveDoubleBallNumberByControl(object sender, FlowLayoutPanel flp, int selectIndex, ref int _CreateIndex, ref int _DoubleBallDoubleBallCount)
        {
            //要释放的panel
            Control faterControl = ((Control)sender).Parent;

            DoubleBall disposeDoubleNumber;

            int removeSerialNumber = int.Parse(faterControl.Tag.ToString());
            if (selectIndex == 2)
            {
                disposeDoubleNumber = _myComplexDoubleBallNumberList.Where(cdbn => cdbn.serialNumber == removeSerialNumber).ToList()[0];
                _myComplexDoubleBallNumberList.Remove(disposeDoubleNumber as ComplexDoubleBallNumber);
            }
            else
            {
                disposeDoubleNumber = _myDantuoDoubleBallNumberList.Where(ddbn => ddbn.serialNumber == removeSerialNumber).ToList()[0];
                _myDantuoDoubleBallNumberList.Remove(disposeDoubleNumber as DantuoDoubleBallNumber);
            }

            _CreateIndex--;
            _DoubleBallDoubleBallCount--;
            RefreshDoubleBallNumbersSerialNumber(selectIndex, disposeDoubleNumber.serialNumber);

            int resetIndex = 0;
            //得到释放双色球的序号
            foreach (Label label in faterControl.Controls)
            {
                if ("No".Equals(label.Tag))
                {
                    resetIndex = int.Parse(label.Text.Substring(0, label.Text.Length - 1));
                    break;
                }
            }

            bool resetFlag = false;
            foreach (Panel panel in flp.Controls)
            {
                //循环定位到释放控件的位置开始重新刷新序号
                if (faterControl == panel)
                {
                    resetFlag = true;
                    continue;
                }
                else if (!resetFlag) continue;

                foreach (Label label in panel.Controls)
                {
                    if ("No".Equals(label.Tag))
                    {
                        panel.Tag = resetIndex;
                        label.Text = resetIndex + ">";
                        resetIndex++;
                        break;
                    }
                }
            }

            faterControl.Dispose();

            if (selectIndex == 2)
            {
                RefreshOneselfNumberMoneyByComplex(false, disposeDoubleNumber as ComplexDoubleBallNumber);
            }
            else
            {
                RefreshOneselfNumberMoneyByDantuo(false, disposeDoubleNumber as DantuoDoubleBallNumber);
            }
        }

        /// <summary>
        /// 刷新双色球序列、从移除位置开始统一减1
        /// </summary>
        /// <param name="selectIndex"></param>
        /// <param name="serialNumber"></param>
        private void RefreshDoubleBallNumbersSerialNumber(int selectIndex, int serialNumber)
        {
            if (selectIndex == 2)
            {
                _myComplexDoubleBallNumberList.ForEach(cdbn =>
                {
                    if(cdbn.serialNumber > serialNumber)
                    {
                        cdbn.serialNumber--;
                    }
                });
            }
            else
            {
                _myDantuoDoubleBallNumberList.ForEach(ddbn =>
                {
                    if (ddbn.serialNumber > serialNumber)
                    {
                        ddbn.serialNumber--;
                    }
                });
            }
        }

        private void CopyPictureStyle(PictureBox byClonePic, PictureBox clonePic)
        {
            clonePic.Tag = byClonePic.Tag;
            clonePic.Size = byClonePic.Size;
            clonePic.Cursor = byClonePic.Cursor;
            clonePic.AutoSize = byClonePic.AutoSize;
            clonePic.Image = byClonePic.Image;
            clonePic.SizeMode = byClonePic.SizeMode;
        }

        private void CopyComplexLabelStyle(Label byCloneLabel, Label cloneLabel)
        {
            cloneLabel.Tag = byCloneLabel.Tag;

            cloneLabel.Size = byCloneLabel.Size;
            cloneLabel.Font = byCloneLabel.Font;
            cloneLabel.Cursor = byCloneLabel.Cursor;

            cloneLabel.AutoSize = byCloneLabel.AutoSize;
            cloneLabel.ForeColor = byCloneLabel.ForeColor;
        }

        private void LabelBall_MouseEnter(object sender, EventArgs e)
        {
            ((Control)sender).ForeColor = Color.Silver;
        }

        private void LabelBall_MouseLeave(object sender, EventArgs e)
        {
            string ballType = ((Control)sender).Tag.ToString();

            if (ballType == "Red")
            {
                ((Control)sender).ForeColor = _RedBallColor;
            }
            else if (ballType == "TRed")
            {
                ((Control)sender).ForeColor = _RedColorTuo;
            }
            else
            {
                ((Control)sender).ForeColor = _BlueBallColor;
            }
        }

        #endregion

        #endregion

        #region Oneself Select And Dantuo

        /// <summary>
        /// 自选胆拖号辅助生成控件
        /// </summary>
        private OneselfControlHelper _DantuoHelper;
        /// <summary>
        /// 手选或者随机生成的胆拖双色球的数量
        /// </summary>
        public static int _DantuoDoubleBallDoubleBallCount = 0;
        /// <summary>
        /// 单次选择的胆拖双色球号码
        /// </summary>
        private DantuoDoubleBallNumber _DantuoDoubleBallNumber;
        /// <summary>
        /// 胆拖双色球我的号码集合
        /// </summary>
        private List<DantuoDoubleBallNumber> _myDantuoDoubleBallNumberList;

        /// <summary>
        /// 自选复试号码点击生成号码
        /// </summary>
        public void OneselfSelectDantuoNumber(int ballType, int number)
        {
            if (_DantuoDoubleBallNumber == null)
                _DantuoDoubleBallNumber = new DantuoDoubleBallNumber();

            AddDoubleBallToDantuo(ballType, number);

            CreateDantuoDoubleBallControl(ballType, number);
        }

        /// <summary>
        /// 自选一个一个创建胆拖号
        /// </summary>
        /// <param name="ballType"></param>
        /// <param name="number"></param>
        private void CreateDantuoDoubleBallControl(int ballType, int number)
        {
            _DantuoHelper.oneselfStatus = false;

            if (_DantuoHelper.oneselfPanel == null)
                InitialFirstControls(_DantuoDoubleBallNumber, _DantuoHelper, ballType, number);
            else 
            {
                ContinueAddDoubleBallToControls(
                    _DantuoDoubleBallNumber, _DantuoHelper, ballType, number);
            }

            if (IsHorizontalScrollBarVisible(flpDantuoNumber))
            {
                flpDantuoNumber.HorizontalScroll.Value = flpDantuoNumber.HorizontalScroll.Maximum;
            }
        }

        /// <summary>
        /// 胆拖自选号点击完成选项
        /// </summary>
        private void picDantuoAccomplish_Click(object sender, EventArgs e)
        {
            _DantuoDoubleBallNumber.OrderDoubleBalls();
            _myDantuoDoubleBallNumberList.Add(_DantuoDoubleBallNumber);

            #region 重新给承载双色球的Label赋值

            int redDIndex = 0;
            int redTIndex = 0;
            int blueIndex = 0;

            foreach (var im in _DantuoHelper.oneselfPanel.Controls)
            {
                if (im is Label doubleBall && doubleBall.Tag.ToString() != "No")
                {
                    //移除已选定号码的相关事件
                    doubleBall.MouseEnter -= LabelBall_MouseEnter;
                    doubleBall.MouseLeave -= LabelBall_MouseLeave;
                    doubleBall.Click -= lblComplexRollBack_Click;

                    if (redDIndex <= _DantuoDoubleBallNumber.redBallDanCount - 1)
                    {
                        doubleBall.Tag = "Red";
                        doubleBall.ForeColor = _RedBallColor;
                        doubleBall.Text = FormatNumber(_DantuoDoubleBallNumber.redBalls[redDIndex++], 2);
                    }
                    else if (redTIndex <= _DantuoDoubleBallNumber.redBallTuoCount - 1)
                    {
                        doubleBall.Tag = "TRed";
                        doubleBall.ForeColor = _RedColorTuo;
                        doubleBall.Text = FormatNumber(_DantuoDoubleBallNumber.redBallTuos[redTIndex++], 2);
                    }
                    else
                    {
                        doubleBall.Tag = "Blue";
                        doubleBall.ForeColor = _BlueBallColor;
                        doubleBall.Text = FormatNumber(_DantuoDoubleBallNumber.blueBalls[blueIndex++], 2);
                    }
                }
            }

            #endregion

            _DantuoHelper.buttonImage.Click -= picDantuoAccomplish_Click;
            _DantuoHelper.buttonImage.Click += picAccomplishDelete_Click;
            _DantuoHelper.buttonImage.Image = Properties.Resources.delete1;

            _CreateIndexByDantuo++;
            _DantuoHelper.oneselfStatus = true;

            RefreshOneselfNumberMoneyByDantuo(true, _DantuoDoubleBallNumber);

            string selfCount = "[" + _DantuoDoubleBallNumber.redBallDanCount + "-"
                + _DantuoDoubleBallNumber.redBallTuoCount + "-"
                + _DantuoDoubleBallNumber.blueBallCount
                + "]";

            SetDoubleBallInfo(_DantuoHelper.oneselfLabelNo, selfCount);

            _OneselfDantuo.ResetDoubleBallNumber();
            ResetDantuoVariate();

            Logger.Info($"self dantuo number : {selfCount}");
        }

        /// <summary>
        /// 往双色球胆拖号码中添加一个球
        /// </summary>
        /// <param name="ballType"></param>
        /// <param name="number"></param>
        private void AddDoubleBallToDantuo(int ballType, int number)
        {
            if (ballType == 0) _DantuoDoubleBallNumber.AddRedBallDan(number);
            if (ballType == 1) _DantuoDoubleBallNumber.AddRedBallTuo(number);
            if (ballType == 2) _DantuoDoubleBallNumber.AddBlueBall(number);
        }

        /// <summary>
        /// 重置胆拖自选号标识
        /// </summary>
        private void ResetDantuoVariate()
        {
            _DantuoHelper.Reset();
            _DantuoDoubleBallNumber = null;
        }
        
        /// <summary>
        /// 生成机选胆拖号
        /// </summary>
        private void lblDantuoRandom1_Click(object sender, EventArgs e)
        {
            #region Check Create Count 

            if (_DantuoDoubleBallNumber != null && !_DantuoHelper.oneselfStatus)
            {
                Info.ShowWarningMessage(Info.UnfinishedOneselfNumber);
                return;
            }

            int totalZhu = int.Parse(lblDantuoBuyZhu.Tag.ToString());

            if (totalZhu >= _MaxOneselfDoubleBallTotalZhu || (totalZhu <= _MaxOneselfDoubleBallTotalZhu && _myDantuoDoubleBallNumberList.Count >= _MaxSerialNumberTotal))
            {
                Info.ShowWarningMessage(Info.OneselfRestrict);
                return;
            }

            #endregion

            int redDanCount = 0;
            int redTuoCount = 0;
            int blueCount = 0;
            ParseDantuoCombination(ref redDanCount, ref redTuoCount, ref blueCount);

            if (redDanCount != 0 && redTuoCount != 0)
            {
                if (redDanCount + redTuoCount < 6)
                {
                    Info.ShowWarningMessage(Info.DantuoCountInsufficient);
                    return;
                }
            }

            int buyTotalZhu = ParseTagValueToInt(lblDantuoBuyZhu);
            int createCount = ParseTagValueToInt((LeeLabel)sender);

            createCount = AmendMaxCount(createCount, _DantuoDoubleBallDoubleBallCount, _MaxSerialNumberTotal);

            int createIndex = 0;

            for (int i = 0; i < createCount; i++)
            {
                CreateDantuoRandomCombination(ref redDanCount, ref redTuoCount, ref blueCount);

                //动态调整生成的注数
                DynamicAmendMaxDantuoTotalZhu(ref redDanCount, ref redTuoCount, ref blueCount, ref buyTotalZhu);

                if (buyTotalZhu <= _MaxOneselfDoubleBallTotalZhu)
                {
                    createIndex++;
                    DantuoDoubleBallNumber doubleBallNumber = _doubleBallToo.GetDantuoDoubleBallNumber(redDanCount, redTuoCount, blueCount);
                    _myDantuoDoubleBallNumberList.Add(doubleBallNumber);
                }
                else break;
            }

            if (createIndex != createCount) createCount = createIndex;

            RefreshOneselfNumberMoneyByDantuoList(_myDantuoDoubleBallNumberList.Cast<DoubleBall>().ToList());

            CreateAllDantuoDoubleBalls(createCount, true);

            Logger.Info($"random {createCount} dantuo number.");
        }

        /// <summary>
        /// 动态调整胆拖号生成的注数限制在最大值
        /// </summary>
        /// <param name="redDanCount">红色球胆号数量</param>
        /// <param name="redTuoCount">红色球拖号数量</param>
        /// <param name="blueCount">蓝色球数量</param>
        /// <param name="buyTotalZhu">目前已购买的总注数</param>
        private void DynamicAmendMaxDantuoTotalZhu(ref int redDanCount, ref int redTuoCount,ref int blueCount, ref int buyTotalZhu)
        {
            bool redSubtraction = false;
            int redTuoMinCount = 6 - redDanCount;

            //只减红球拖号数量和蓝球数量
            while (_doubleBallToo.GetDantuoCombinationTotalZhu(redDanCount, redTuoCount, blueCount) + buyTotalZhu > _MaxOneselfDoubleBallTotalZhu)
            {
                //红球拖个数大于拖应最小数量 并且 (轮到红球减 或者 蓝球数量只剩1减无可减)
                if (redTuoCount > redTuoMinCount && (redSubtraction || blueCount == 1))
                {
                    redTuoCount--;
                    redSubtraction = false;
                }
                //蓝球数量需要大于 1 才可减
                else if (blueCount > 1)
                {
                    blueCount--;
                    redSubtraction = true;
                }

                if (((redDanCount + redTuoCount) == 6) && blueCount == 1) break;
            }

            buyTotalZhu += (int)_doubleBallToo.GetDantuoCombinationTotalZhu(redDanCount, redTuoCount,blueCount);
        }

        private int _CreateIndexByDantuo = 0;
        /// <summary>
        /// 循环创建我的机选胆拖号码
        /// </summary>
        /// <param name="complexCount">创建数量</param>
        /// <param name="showImage">是否显示图标</param>
        private void CreateAllDantuoDoubleBalls(int complexCount, bool showImage)
        {
            for (int i = _CreateIndexByDantuo; i < _myDantuoDoubleBallNumberList.Count; i++)
            {
                CreateDantuoAndShowDoubleBall(_myDantuoDoubleBallNumberList[i], showImage);
            }

            _CreateIndexByDantuo += complexCount;
        }

        /// <summary>
        /// 创建一个显示胆拖号码的控件
        /// </summary>
        /// <param name="dantuoNumber">要创建的胆拖号码</param>
        /// <param name="showImage">是否显示图标</param>
        private void CreateDantuoAndShowDoubleBall(DantuoDoubleBallNumber dantuoNumber, bool showImage)
        {
            _DantuoHelper.oneselfPanel = new Panel();

            int danWidth = dantuoNumber.redBallDanCount * _INTERVAL;
            int tuoWidth = dantuoNumber.redBallTuoCount * _INTERVAL;
            int blueWidth = dantuoNumber.blueBallCount * _INTERVAL;
            int cumsumWidth = danWidth + tuoWidth + blueWidth - 2;

            if (!showImage) cumsumWidth -= _INTERVAL;

            _DantuoHelper.oneselfPanel.Size = new Size(
                _DantuoHelper.initPanelSize.Width + cumsumWidth, _DantuoHelper.initPanelSize.Height);

            _DantuoHelper.oneselfPanel.Tag = dantuoNumber.serialNumber;

            Label serial = AddSerialFirstRedBall(dantuoNumber, _DantuoHelper);

            SetDoubleBallInfo(serial,
                dantuoNumber.redBallDanCount + "-" + dantuoNumber.redBallTuoCount + "-" + dantuoNumber.blueBallCount);

            AddDoubleBallsLabel(1, 
                dantuoNumber.redBallDanCount, dantuoNumber.redBalls, lblComplexRed, _DantuoHelper);

            AddDoubleBallsLabel(0, 
                dantuoNumber.redBallTuoCount, dantuoNumber.redBallTuos, lblRedBallTuo, _DantuoHelper);

            AddDoubleBallsLabel(0, 
                dantuoNumber.blueBallCount, dantuoNumber.blueBalls, lblComplexBlue, _DantuoHelper);

            AddHandleButtonImage(_DantuoHelper, flpDantuoNumber, showImage);

            ResetDantuoVariate();
        }

        /// <summary>
        /// 显示序列号和第一个红球号码
        /// </summary>
        private Label AddSerialFirstRedBall(DoubleBall doubleBall, OneselfControlHelper _OH)
        {
            //生成序列号
            Label randomNo = new Label();
            randomNo.Text = doubleBall.serialNumber + ">";
            CopyLabelStyle(lblRandomNo, randomNo);

            //生成第一个号码
            Label ball = new Label();
            ball.Text = FormatNumber(doubleBall.redBalls[0], 2);
            CopyLabelStyle(lblComplexRed, ball);

            _OH.oneselfPanel.Controls.Add(ball);
            _OH.oneselfPanel.Controls.Add(randomNo);

            return randomNo;
        }

        /// <summary>
        /// 添加最后的操作图片按钮
        /// </summary>
        private void AddHandleButtonImage(OneselfControlHelper _OH, FlowLayoutPanel LayoutPanel, bool showImage)
        {
            _OH.buttonImage = new PictureBox();
            _OH.buttonImage.Click += picAccomplishDelete_Click;
            _OH.buttonImage.Location = new Point(_OH.cumsumPoint.X + _INTERVAL, _OH.cumsumPoint.Y);
            _OH.buttonImage.Visible = showImage;

            CopyPictureStyle(picComplexAccomplish, _OH.buttonImage);
            _OH.buttonImage.Image = Properties.Resources.delete1;

            _OH.oneselfPanel.Controls.Add(_OH.buttonImage);

            LayoutPanel.Controls.Add(_OH.oneselfPanel);
        }

        /// <summary>
        /// 添加并显示一个双色球单个数字控件
        /// </summary>
        /// <param name="steartIndex">起始创建下标</param>
        /// <param name="createCount">创建数量</param>
        /// <param name="balls">要显示的号码数组</param>
        /// <param name="byCopy">被克隆的样式</param>
        /// <param name="_OH">辅助生成控件</param>
        private void AddDoubleBallsLabel(int steartIndex, int createCount, int[] balls, LeeLabel byCopy, OneselfControlHelper _OH)
        {
            for (int i = steartIndex; i < createCount; i++)
            {
                Label ballOne = new Label();
                ballOne.Text = FormatNumber(balls[i], 2);

                CopyComplexLabelStyle(byCopy, ballOne);

                ballOne.Location = new Point(_OH.cumsumPoint.X + _INTERVAL, _OH.cumsumPoint.Y);
                _OH.cumsumPoint = ballOne.Location;

                _OH.oneselfPanel.Controls.Add(ballOne);
            }
        }

        /// <summary>
        /// 累加刷新胆拖自选号金额注数
        /// </summary>
        /// <param name="complexDoubleBall"></param>
        private void RefreshOneselfNumberMoneyByDantuoList(List<DoubleBall> _dantuoList)
        {
            long totalZhu = 0, buyTotalMoney = 0;
            int multiple = int.Parse(cmbMultiple.Text);

            _dantuoList.ForEach(db =>
            {
                long combination = (db as DantuoDoubleBallNumber).GetDoubleBallCombination();
                totalZhu += combination;
                buyTotalMoney += combination * 2 * multiple;
            });

            lblDantuoBuyZhu.Tag = totalZhu;
            lblDantuoBuyZhu.Text = string.Format("[{0}] {1}", Info.GetNumberMaxUnit(totalZhu), FormatNumber(totalZhu.ToString(), 5));

            lblDantuoMonetary.Tag = buyTotalMoney;
            lblDantuoMonetary.Text = string.Format("[{0}] {1}", Info.GetNumberMaxUnit(buyTotalMoney), FormatNumber(buyTotalMoney.ToString(), 5));
        }

        /// <summary>
        /// 刷新胆拖自选号金额和显示
        /// </summary>
        /// <param name="isAdd">是否累加</param>
        private void RefreshOneselfNumberMoneyByDantuo(bool isAdd, DantuoDoubleBallNumber dantuoDoubleBall)
        {
            int multiple = int.Parse(cmbMultiple.Text);

            int totalZhu = ParseTagValueToInt(lblDantuoBuyZhu);

            if (isAdd)
            {
                lblDantuoBuyZhu.Tag = totalZhu + dantuoDoubleBall.GetDoubleBallCombination();
            }
            else
            {
                lblDantuoBuyZhu.Tag = totalZhu - dantuoDoubleBall.GetDoubleBallCombination();
            }

            lblDantuoBuyZhu.Text =
                string.Format("[{0}] {1}", Info.GetNumberMaxUnit(ParseTagValueToInt(lblDantuoBuyZhu)), FormatNumber(lblDantuoBuyZhu.Tag.ToString(), 5));

            int totalMoney = ParseTagValueToInt(lblDantuoMonetary);

            if (isAdd)
            {
                lblDantuoMonetary.Tag = totalMoney + dantuoDoubleBall.GetDoubleBallCombination() * 2 * multiple;
            }
            else
            {
                lblDantuoMonetary.Tag = totalMoney - dantuoDoubleBall.GetDoubleBallCombination() * 2 * multiple;
            }

            lblDantuoMonetary.Text =
                string.Format("[{0}] {1}", Info.GetNumberMaxUnit(ParseTagValueToInt(lblDantuoMonetary)), FormatNumber(lblDantuoMonetary.Tag.ToString(), 5));
        }

        /// <summary>
        /// 重置我的胆拖号码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnResetDOneself_Click(object sender, EventArgs e)
        {
            Logger.Info($"reset my dantuo number : {_myDantuoDoubleBallNumberList.Count} row");

            _CreateIndexByDantuo = 0;
            _DantuoDoubleBallDoubleBallCount = 0;
            _myDantuoDoubleBallNumberList.Clear();

            flpDantuoNumber.Controls.Clear();
            _OneselfDantuo?.ResetDoubleBallNumber();

            lblDantuoBuyZhu.Tag = 0;
            lblDantuoBuyZhu.Text = "[零] 00000";
            lblDantuoMonetary.Tag = 0;
            lblDantuoMonetary.Text = "[零] 00000";

            panDantuoStatistics.Visible = false;

            if (_DantuoTipInfo != null) _DantuoTipInfo.RemoveAll();

            ResetDantuoVariate();
        }

        #endregion

        #region fold

        /// <summary>
        /// 产生摇奖号码后部分控件可见和标识设置
        /// </summary>
        private void SettingControlParameter()
        {
            if (CardInterface.SelectedIndex == 1)
            {
                panRandomFilter.Visible = true;
                panRandomStatistics.Visible = true;

                _createNumber = true;
                panRandomFilter.Enabled = true;

                _executeFilter = true;
                cmbRandomViewFilter.SelectedIndex = 0;
            }
            else if (CardInterface.SelectedIndex == 2)
            {
                panComplexStatistics.Visible = true;
            }
            else
            {
                panDantuoStatistics.Visible = true;
            }
        }

        /// <summary>
        /// 获取循环开奖终止条件
        /// </summary>
        public AwardType GetLoopStopCondition()
        {
            switch (cmbStopCondition.SelectedIndex)
            {
                case 0: return AwardType.SixAward;
                case 1: return AwardType.FiveAward;
                case 2: return AwardType.FourAward;
                case 3: return AwardType.ThreeAward;
                case 4: return AwardType.TwoAward;
                case 5: return AwardType.OneAward;
                default: return AwardType.OneAward;
            }
        }

        /// <summary>
        /// 判断开奖之前是否符合条件
        /// </summary>
        private bool WhetherMeetsCriteria()
        {
            bool selfSelect = rbAlreadySelect.Checked;

            switch (CardInterface.SelectedIndex)
            {
                case 1:

                    if (!selfSelect) return true;

                    else if(_randomDoubleBallCount == 0)
                    {
                        Info.ShowWarningMessage(Info.MyNumberIsEmpty);
                    }

                    return _randomDoubleBallCount != 0;

                case 2:
                    
                    if (!selfSelect) return true;

                    else if (_myComplexDoubleBallNumberList.Count == 0 && _ComplexDoubleBallNumber == null)
                    {
                        Info.ShowWarningMessage(Info.MyNumberIsEmpty);
                        return false;
                    }
                    else if (!_ComplexHelper.oneselfStatus)
                    {
                        Info.ShowWarningMessage(Info.UnfinishedOneselfNumber);
                        return false;
                    }

                    return true;

                case 3:

                    if (!selfSelect) return true;

                    else if (_myDantuoDoubleBallNumberList.Count == 0 && _DantuoDoubleBallNumber == null)
                    {
                        Info.ShowWarningMessage(Info.MyNumberIsEmpty);
                        return false;
                    }
                    else if (!_DantuoHelper.oneselfStatus)
                    {
                        Info.ShowWarningMessage(Info.UnfinishedOneselfNumber);
                        return false;
                    }

                    return true;

                default:
                    Info.ShowWarningMessage(Info.MyNumberIsEmpty);
                    break;
            }

            return false;
        }

        #endregion

        #region FormatNumber

        /// <summary>
        /// 号码不足时位补0填充长度
        /// </summary>
        /// <param name="number">要补齐的数字</param>
        /// <param name="totalLength">总长度</param>
        public static string FormatNumber(string number, int totalLength)
        {
            //数字的长度
            int nowLength = number.Length;
            //总长度 - 数字的长度 = 要补齐的位数
            int fillCount = totalLength - nowLength;

            StringBuilder quantityText = new StringBuilder();

            for (int i = 0; i < fillCount; i++)
            {
                quantityText.Append("0");
            }

            return quantityText + number;
        }

        public static string FormatNumber(int number, int totalLength)
        {
            //数字的长度
            int nowLength = number.ToString().Length;
            //总长度 - 数字的长度 = 要补齐的位数
            int fillCount = totalLength - nowLength;

            StringBuilder quantityText = new StringBuilder();

            for (int i = 0; i < fillCount; i++)
            {
                quantityText.Append("0");
            }

            return quantityText.ToString() + number;
        }

        #endregion

        #region Application Setting

        /// <summary>
        /// 胆拖号自选窗体
        /// </summary>
        private OneselfDantuo _OneselfDantuo;
        /// <summary>
        /// 复试自选号窗体
        /// </summary>
        private OneselfComplex _OneselfComplex;
        /// <summary>
        /// 是否加载过异常日志
        /// </summary>
        private bool loadExceptionLog = false;

        /// <summary>
        /// 选项卡切换到自选页时展示自选号窗体
        /// </summary>
        private void CardInterface_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CardInterface.SelectedIndex != 2) _OneselfComplex?.HideThis();
            if (CardInterface.SelectedIndex != 3) _OneselfDantuo?.HideThis();

            //唤出复试自选窗体
            if (CardInterface.SelectedIndex == 2 && cmbCPattern.SelectedIndex == 0)
            {
                if (_OneselfComplex == null)
                {
                    _OneselfComplex = new OneselfComplex(this);
                    _OneselfComplex.Show(this);
                }
                else _OneselfComplex.ShowThis();
            }

            //唤出胆拖自选窗体
            if (CardInterface.SelectedIndex == 3 && cmbDPattern.SelectedIndex == 0)
            {
                if (_OneselfDantuo == null)
                {
                    _OneselfDantuo = new OneselfDantuo(this);
                    _OneselfDantuo.Show(this);
                }
                else _OneselfDantuo.ShowThis();
            }

            //Load Exception Log
            if (CardInterface.SelectedIndex == 5)
            {
                this.DataError.AutoGenerateColumns = false;

                if (!loadExceptionLog)
                {
                    loadExceptionLog = true;
                    RefreshExceptionData();
                }
            }

            //Log it
            if (CardInterface.SelectedIndex == 0) Logger.Info("to software desc page.");
            else if (CardInterface.SelectedIndex == 1) Logger.Info("to simplex page.");
            else if (CardInterface.SelectedIndex == 2) Logger.Info("to complex page.");
            else if (CardInterface.SelectedIndex == 3) Logger.Info("to dantuo page.");
            else if (CardInterface.SelectedIndex == 4) Logger.Info("to setting page.");
            else if (CardInterface.SelectedIndex == 5) Logger.Info("to exception log page.");
            else if (CardInterface.SelectedIndex == 6) Logger.Info("to update log page.");
            else if (CardInterface.SelectedIndex == 7) Logger.Info("to about software page.");

        }

        /// <summary>
        /// 加载应用设置
        /// </summary>
        private void LoadSettingApply()
        {
            Setting setting = Setting.LoadSetting();

            cmbStopCondition.SelectedIndex = setting.LoopStopCondition;
            cmbMultiple.SelectedIndex = setting.ZhuMultiple;

            nudRandomBuyZhu.Value = setting.RandomGenerateCount;

            if (setting.myNumberType == MyNumberType.AlreadySelect)
            {
                rbAlreadySelect.Checked = true;
            }
            else if (setting.myNumberType == MyNumberType.RandomNumber)
            {
                rbEveryRandom.Checked = true;

                cmbRedComplexCount.Enabled = true;
                cmbBlueComplexCount.Enabled = true;

                lblComplexRandom1.Enabled = false;
                lblComplexRandom5.Enabled = false;
                lblComplexRandom10.Enabled = false;

                btnResetCOneself.Enabled = false;
            }

            if (setting.BuyHabit == BuyHabit.EveryPeriod)
            {
                rbEveryPeriod.Checked = true;
            }
            else if (setting.BuyHabit == BuyHabit.WeeklyPeriod)
            {
                rbWeeklyPeriod.Checked = true;
            }
            else if (setting.BuyHabit == BuyHabit.MonthlyPeriod)
            {
                rbMonthlyPeriod.Checked = true;
            }
            else if (setting.BuyHabit == BuyHabit.CustomizeMode)
            {
                rbCustomizeMode.Checked = true;
            }

            nudFixedPeriod.Value = setting.FixedPeriod;
            nudUncertaintyStartPeriod.Value = setting.UncertaintyStartPeriod;
            nudUncertaintyEndPeriod.Value = setting.UncertaintyEndPeriod;

            nudOneBonus.Value = setting.GetOneAward();
            nudTwoBonus.Value = setting.GetTwoAward();
            lblOneBonus.Text = setting.GetOneAward().ToString();
            lblTwoBonus.Text = setting.GetTwoAward().ToString();

            nudCustomizePeriods.Value = setting.CustomizePeriods;
            btnCustomizeRunLotterys.Tag = setting.CustomizePeriods;
            btnRondomCustomize.Tag = setting.CustomizePeriods;

            Config.Setting = setting;
            RefreshIntervalPeriods();

            rbAlreadySelect_Click(null, null);
            rbCustomizeMode_Click(null, null);
        }

        /// <summary>
        /// 获取我的设置购买习惯设置
        /// </summary>
        private int GetIntervalPeriods(Setting setting)
        {
            if (setting.BuyHabit != BuyHabit.CustomizeMode)
            {
                return (int)setting.BuyHabit;
            }
            else
            {
                if (setting.CustomizeModeType == CustomizeModeType.FixedHabit)
                {
                    return setting.FixedPeriod;
                }
                else
                {
                    int interval = MyRandom.GetRandomNum(setting.UncertaintyStartPeriod, setting.UncertaintyEndPeriod);
                    return interval;
                }
            }
        }

        /// <summary>
        /// 恢复默认设置
        /// </summary>
        private void btnRestoreDefault_Click(object sender, EventArgs e)
        {
            Config.Setting = new Setting();

            if (Setting.RestoreDefaultSetting(Config.Setting))
            {
                rbAlreadySelect.Checked = true;

                nudRandomBuyZhu.Value = 1;

                rbEveryPeriod.Checked = true;
                rbFixedPeriod.Checked = true;

                nudFixedPeriod.Value = 1;
                nudUncertaintyStartPeriod.Value = 1;
                nudUncertaintyEndPeriod.Value = 5;

                panEveryRandom.Enabled = false;
                panCustomizeMode.Enabled = false;

                cmbCPattern.SelectedIndex = 0;
                cmbMultiple.SelectedIndex = 0;
                cmbStopCondition.SelectedIndex = 5;

                nudOneBonus.Value = (int)AwardType.OneAward;
                nudTwoBonus.Value = (int)AwardType.TwoAward;
                lblOneBonus.Text = nudOneBonus.Value.ToString();
                lblTwoBonus.Text = nudTwoBonus.Value.ToString();

                nudCustomizePeriods.Value = 156;
                
                RefreshIntervalPeriods();

                rbAlreadySelect_Click(sender, e);
                rbCustomizeMode_Click(sender, e);

                btnCustomizeRunLotterys.Tag = 156;
                btnRondomCustomize.Tag = 156;
                this.BackColor = Color.White;
                picUseIt.Visible = false;

                Logger.Warning("reset default setting success.");
                Info.ShowInfoMessage(Info.RestoreDefaultSetting);
            }
            else
            {
                Logger.Warning("reset default setting fail.");
                Info.ShowErrorMessage(Info.OperatioFail);
            }
        }

        /// <summary>
        /// 应用我的设置
        /// </summary>
        private void btnSaveSetting_Click(object sender, EventArgs e)
        {
            if(nudUncertaintyEndPeriod.Value <= nudUncertaintyStartPeriod.Value)
            {
                Info.ShowWarningMessage(Info.ApplySettingIniterval);
                return;
            }

            Setting setting = new Setting();

            setting.myNumberType = rbAlreadySelect.Checked ? MyNumberType.AlreadySelect : MyNumberType.RandomNumber;

            setting.RandomGenerateCount = (int)nudRandomBuyZhu.Value;

            if (rbEveryPeriod.Checked)
            {
                setting.BuyHabit = BuyHabit.EveryPeriod;
            }
            else if (rbWeeklyPeriod.Checked)
            {
                setting.BuyHabit = BuyHabit.WeeklyPeriod;
            }
            else if (rbMonthlyPeriod.Checked)
            {
                setting.BuyHabit = BuyHabit.MonthlyPeriod;
            }
            else if (rbCustomizeMode.Checked)
            {
                setting.BuyHabit = BuyHabit.CustomizeMode;

                if (rbFixedPeriod.Checked)
                {
                    setting.CustomizeModeType = CustomizeModeType.FixedHabit;
                }
                else if (rbUncertaintyPeriod.Checked)
                {
                    setting.CustomizeModeType = CustomizeModeType.UncertaintyHabit;
                }
            }

            setting.FixedPeriod = (int)nudFixedPeriod.Value;
            setting.UncertaintyStartPeriod = (int)nudUncertaintyStartPeriod.Value;
            setting.UncertaintyEndPeriod = (int)nudUncertaintyEndPeriod.Value;

            setting.OneAward = (int)nudOneBonus.Value;
            setting.TwoAward = (int)nudTwoBonus.Value;

            setting.CustomizePeriods = (int)nudCustomizePeriods.Value;

            lblOneBonus.Text = nudOneBonus.Value.ToString();
            lblTwoBonus.Text = nudTwoBonus.Value.ToString();
            btnCustomizeRunLotterys.Tag = setting.CustomizePeriods;
            btnRondomCustomize.Tag = setting.CustomizePeriods;

            setting.LoopStopCondition = cmbStopCondition.SelectedIndex;
            setting.BackColorArgb = Config.Setting.BackColorArgb;
            setting.ZhuMultiple = cmbMultiple.SelectedIndex;
            setting.AgreeDeclaration = true;

            Config.Setting = setting;

            if (setting.SaveSetting())
            {
                RefreshIntervalPeriods();
                Logger.Warning($"apply my setting success : {setting.ToString()}");
                Info.ShowInfoMessage(Info.SaveSettingSuccess);
            }
            else
            {
                Logger.Warning("apply my setting fail.");
                Info.ShowErrorMessage(Info.OperatioFail);
            }
        }

        /// <summary>
        /// 刷新间隔周期
        /// </summary>
        private void RefreshIntervalPeriods()
        {
            Config.RecordPeriods = 0;
            Config.IntervalPeriods = GetIntervalPeriods(Config.Setting);
        }

        /// <summary>
        /// 窗体退出前事件
        /// </summary>
        private void DoubleBallView_FormClosing(object sender, FormClosingEventArgs e)
        {
            Setting setting = Setting.LoadSetting();

            setting.LoopStopCondition = cmbStopCondition.SelectedIndex;
            setting.ZhuMultiple = cmbMultiple.SelectedIndex;

            setting.SaveSetting();

            Logger.Info($"number of lottery periods : {lblDoubleBallPeriods.Text}");
            Logger.Info("stop it.");
        }

        /// <summary>
        /// 号码设置单选按钮按下事件
        /// </summary>
        private void rbAlreadySelect_Click(object sender, EventArgs e)
        {
            if (rbAlreadySelect.Checked)
            {
                //机选
                lblRandom1.Enabled = true;
                lblRandom5.Enabled = true;
                lblRandom10.Enabled = true;
                lblRandom20.Enabled = true;
                lblRandom50.Enabled = true;

                //自选&复试
                cmbCPattern.SelectedIndex = 0;
                cmbCPattern.Enabled = true;
                lblScheme.Enabled = false;
                cmbRedComplexCount.Enabled = false;
                cmbBlueComplexCount.Enabled = false;
                btnResetCOneself.Enabled = true;

                //自选&胆拖
                cmbDPattern.SelectedIndex = 0;
                cmbDPattern.Enabled = true;

                lblRedDanTitle.Enabled = false;
                cmbRedDanCount.Enabled = false;
                lblRedTuo.Enabled = false;
                cmbRedTuoCount.Enabled = false;
                lblDanBlueTitle.Enabled = false;
                cmbBlueDanCount.Enabled = false;

                btnResetDOneself.Enabled = true;

                //主界面守号
                btnResetRandomNo.Enabled = true;
                btnFiveYearsRunLotterys.Enabled = true;
                btnTenYearsRunLotterys.Enabled = true;
                btnCustomizeRunLotterys.Enabled = true;
                btnInfiniteLoopRunLottery.Enabled = true;

                //主界面随机
                btnRondomFiveYear.Enabled = false;
                btnRondomTenYear.Enabled = false;
                btnRondomCustomize.Enabled = false;
                btnInfiniteRondomLoopYear.Enabled = false;
                panEveryRandom.Enabled = false;
            }
            else if (rbEveryRandom.Checked)
            {
                //机选
                lblRandom1.Enabled = false;
                lblRandom5.Enabled = false;
                lblRandom10.Enabled = false;
                lblRandom20.Enabled = false;
                lblRandom50.Enabled = false;

                //自选&复试
                cmbCPattern.SelectedIndex = 1;
                cmbCPattern.Enabled = false;
                lblScheme.Enabled = true;
                cmbRedComplexCount.Enabled = true;
                cmbBlueComplexCount.Enabled = true;
                btnResetCOneself.Enabled = false;

                //自选&胆拖
                cmbDPattern.SelectedIndex = 1;
                cmbDPattern.Enabled = false;

                lblRedDanTitle.Enabled = true;
                cmbRedDanCount.Enabled = true;
                lblRedTuo.Enabled = true;
                cmbRedTuoCount.Enabled = true;
                lblDanBlueTitle.Enabled = true;
                cmbBlueDanCount.Enabled = true;

                btnResetDOneself.Enabled = false;

                //主界面守号按钮
                btnResetRandomNo.Enabled = false;
                btnFiveYearsRunLotterys.Enabled = false;
                btnTenYearsRunLotterys.Enabled = false;
                btnCustomizeRunLotterys.Enabled = false;
                btnInfiniteLoopRunLottery.Enabled = false;
                //主界面随机按钮
                btnRondomFiveYear.Enabled = true;
                btnRondomTenYear.Enabled = true;
                btnRondomCustomize.Enabled = true;
                btnInfiniteRondomLoopYear.Enabled = true;
                //设置界面
                panEveryRandom.Enabled = true;
            }

            lblComplexRandom1.Enabled = false;
            lblComplexRandom5.Enabled = false;
            lblComplexRandom10.Enabled = false;

            lblDantuoRandom1.Enabled = false;
            lblDantuoRandom5.Enabled = false;
            lblDantuoRandom10.Enabled = false;
        }

        /// <summary>
        /// 自选&随机 方案下拉框切换事件
        /// </summary>
        private void cmbPattern_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CardInterface.SelectedIndex == 2)
            {
                if (cmbCPattern.SelectedIndex == 0)
                {
                    _OneselfComplex?.ShowThis();

                    lblScheme.Enabled = false;
                    cmbRedComplexCount.Enabled = false;
                    cmbBlueComplexCount.Enabled = false;
                    lblComplexRandom1.Enabled = false;
                    lblComplexRandom5.Enabled = false;
                    lblComplexRandom10.Enabled = false;
                }
                else
                {
                    _OneselfComplex?.HideThis();

                    lblScheme.Enabled = true;
                    cmbRedComplexCount.Enabled = true;
                    cmbBlueComplexCount.Enabled = true;
                    lblComplexRandom1.Enabled = true;
                    lblComplexRandom5.Enabled = true;
                    lblComplexRandom10.Enabled = true;
                }
            }
        }

        /// <summary>
        /// 自选&胆拖 方案下拉框切换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbDPattern_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CardInterface.SelectedIndex == 3)
            {
                if (cmbDPattern.SelectedIndex == 0)
                {
                    _OneselfDantuo?.ShowThis();
                    
                    lblRedDanTitle.Enabled = false;
                    cmbRedDanCount.Enabled = false;
                    lblRedTuo.Enabled = false;
                    cmbRedTuoCount.Enabled = false;
                    lblDanBlueTitle.Enabled = false;
                    cmbBlueDanCount.Enabled = false;
                    lblDantuoRandom1.Enabled = false;
                    lblDantuoRandom5.Enabled = false;
                    lblDantuoRandom10.Enabled = false;
                }
                else
                {
                    _OneselfDantuo?.HideThis();

                    lblRedDanTitle.Enabled = true;
                    cmbRedDanCount.Enabled = true;
                    lblRedTuo.Enabled = true;
                    cmbRedTuoCount.Enabled = true;
                    lblDanBlueTitle.Enabled = true;
                    cmbBlueDanCount.Enabled = true;
                    lblDantuoRandom1.Enabled = true;
                    lblDantuoRandom5.Enabled = true;
                    lblDantuoRandom10.Enabled = true;
                }
            }
        }

        #endregion

        #region Other Code

        private ToolTip _ComplexTipInfo;
        private ToolTip _DantuoTipInfo;

        /// <summary>
        /// 设置控件悬停提示信息
        /// </summary>
        public void SetDoubleBallInfo(Control trol, string text)
        {
            if (CardInterface.SelectedIndex == 2)
            {
                if (_ComplexTipInfo == null)
                {
                    _ComplexTipInfo = new ToolTip();
                    _ComplexTipInfo.AutoPopDelay = 3500;
                    _ComplexTipInfo.UseFading = true;
                    _ComplexTipInfo.ShowAlways = true;
                    _ComplexTipInfo.IsBalloon = true;
                }

                _ComplexTipInfo.SetToolTip(trol, text);
            }
            else
            {
                if (_DantuoTipInfo == null)
                {
                    _DantuoTipInfo = new ToolTip();
                    _DantuoTipInfo.AutoPopDelay = 3500;
                    _DantuoTipInfo.UseFading = true;
                    _DantuoTipInfo.ShowAlways = true;
                    _DantuoTipInfo.IsBalloon = true;
                }

                _DantuoTipInfo.SetToolTip(trol, text);
            }
        }

        private bool _max = false;
        private void lblMaxUndo_Click(object sender, EventArgs e)
        {
            if (!_max)
            {
                _max = true;
                lblMaxUndo.Text = "Uno";
                Screen currentScreen = Screen.FromControl(this);
                MaximumSize = currentScreen.WorkingArea.Size;
                WindowState = FormWindowState.Maximized;
                Logger.Info("maximize window.");
            }
            else
            {
                _max = false;
                lblMaxUndo.Text = "Max";
                WindowState = FormWindowState.Normal;
                Logger.Info("normal window.");
            }
        }

        private void DoubleBallView_Resize(object sender, EventArgs e) => RefreshResize?.Invoke();

        private void DoubleBallView_Move(object sender, EventArgs e) => RefreshMoveLocation?.Invoke();

        /// <summary>
        /// 单击改变皮肤事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblSkin1_Click(object sender, EventArgs e)
        {
            ExtendLabel colorLabel = sender as ExtendLabel;
            this.BackColor = colorLabel.BackColor;

            picUseIt.Visible = true;
            colorLabel.removePicture();
            colorLabel.Controls.Add(picUseIt);

            Config.Setting.BackColorArgb = colorLabel.BackColor.ToArgb();
            Config.Setting.SaveSetting();

            Logger.Info($"skin modification:{colorLabel.BackColor}");
        }

        private void lblDefaultSkin_Click(object sender, EventArgs e)
        {
            picUseIt.Visible = false;
            this.BackColor = Color.White;
            Config.Setting.BackColorArgb = -1;
            Config.Setting.SaveSetting();

            Logger.Info("setting default skin.");
        }

        /// <summary>
        /// 加载默认皮肤颜色
        /// </summary>
        private void LoadDefaultColor()
        {
            ExtendLabel useThat = null;
            int backColor = Config.Setting.BackColorArgb;
            //深蓝色
            this.lblSkin1.BackColor = Color.FromArgb(-2892833);
            if (backColor == -2892833) useThat = lblSkin1;

            //星空灰
            this.lblSkin2.BackColor = Color.FromArgb(-1382941);
            if (backColor == -1382941) useThat = lblSkin2;

            //天空蓝
            this.lblSkin3.BackColor = Color.FromArgb(-12094);
            if (backColor == -12094) useThat = lblSkin3;

            //水彩粉
            this.lblSkin4.BackColor = Color.FromArgb(-270873);
            if (backColor == -270873) useThat = lblSkin4;

            //活力橙
            this.lblSkin5.BackColor = Color.FromArgb(-4594978);
            if (backColor == -4594978) useThat = lblSkin5;

            this.lblSkin6.BackColor = Color.FromArgb(-2101555);
            if (backColor == -2101555) useThat = lblSkin6;

            this.lblSkin7.BackColor = Color.FromArgb(-1123343);
            if (backColor == -1123343) useThat = lblSkin7;

            this.lblSkin8.BackColor = Color.FromArgb(-2436916);
            if (backColor == -2436916) useThat = lblSkin8;

            this.lblSkin9.BackColor = Color.FromArgb(-2822677);
            if (backColor == -2822677) useThat = lblSkin9;

            this.lblSkin10.BackColor = Color.FromArgb(-2756355);
            if (backColor == -2756355) useThat = lblSkin10;

            if (useThat != null)
            {
                picUseIt.Visible = true;
                useThat.Controls.Add(picUseIt);
            }
        }

        /// <summary>
        /// 刷新异常日志
        /// </summary>
        private void RefreshExceptionData()
        {
            List<ExceptionLog> logs = SqlLite.Table<ExceptionLog>() as List<ExceptionLog>;

            if (logs != null)
            {
                BindingList<ExceptionLog> bindings = new BindingList<ExceptionLog>(logs);
                this.DataError.DataSource = bindings;
                this.DataError.ClearSelection();
            }
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            DialogResult oKCancel = MessageBox.Show(Info.ClearExceptionLog, Info.DoubleBall, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (oKCancel == DialogResult.OK)
            {
                int result = SqlLite.DeleteAll<ExceptionLog>();
                if (result >= 0)
                {
                    this.DataError.DataSource = null;
                    Logger.Info($"clear all exception log : {result} row");
                }
            }
        }

        private void btnRefreshLog_Click(object sender, EventArgs e)
        {
            RefreshExceptionData();
            Logger.Info($"refresh exception log : {DataError.Rows.Count} row");
        }

        private void MenuDelete_Click(object sender, EventArgs e)
        {
            if (this.DataError.SelectedRows.Count == 0)
            {
                return;
            }

            List<int> primaryKeys = new List<int>();
            foreach (DataGridViewRow row in DataError.SelectedRows)
            {
                primaryKeys.Add(int.Parse(row.Cells[0].Value.ToString()));
                this.DataError.Rows.Remove(row);
            }
            //batch delete
            int result = SqlLite.Execute(SQL.DeleteLogByIds(primaryKeys));

            Logger.Info($"delete exception log : {result} row");
        }

        private void MenuExport_Click(object sender, EventArgs e)
        {
            if (this.DataError.SelectedRows.Count == 0)
                return;

            FolderBrowserDialog locationResult = new FolderBrowserDialog();
            locationResult.Description = Info.SaveLocation;

            if (locationResult.ShowDialog() != DialogResult.OK)
                return;

            List<int> primaryKeys = new List<int>();
            string exportPath = locationResult.SelectedPath;

            foreach (DataGridViewRow row in DataError.SelectedRows)
            {
                primaryKeys.Add(int.Parse(row.Cells[0].Value.ToString()));
            }

            List<ExceptionLog> exList
                = SqlLite.Query<ExceptionLog>(SQL.SelectLogByIds(primaryKeys)) as List<ExceptionLog>;

            int exportCount = ExportThatToFile(exList, exportPath);

            Logger.Info($"export exception log to {exportPath} : {exportCount} row");

            notify.ShowBalloonTip(500, string.Empty, string.Format(Info.ExprotLogCount, exportCount), ToolTipIcon.Info);
        }

        private void MenuView_Click(object sender, EventArgs e)
        {
            if (this.DataError.SelectedRows.Count == 0)
                return;

            string rowId = DataError.SelectedRows[0].Cells[0].Value.ToString();

            ExceptionLog exLog = SqlLite.Get<ExceptionLog>(rowId) as ExceptionLog;

            if (exLog != null)
            {
                if (ViewException.isOpenThat(exLog.Id))
                {
                    notify.ShowBalloonTip(500, "", Info.AlreadyOpenLog, ToolTipIcon.Info);
                    return;
                }
                Logger.Info($"view exception : id {exLog.Id} | message : {exLog.ExceptionMessage}");
                ViewException viewException = new ViewException(exLog);
                viewException.Show();
            }
        }

        /// <summary>
        /// 导出数据到文件
        /// </summary>
        /// <param name="exList"></param>
        /// <param name="directory"></param>
        /// <returns></returns>
        public int ExportThatToFile(List<ExceptionLog> exList, string directory)
        {
            int count = 0;
            StringBuilder errorText = new StringBuilder();

            foreach (var ex in exList)
            {
                errorText.Append("encoding：" + "\t\t\t" + ex.Encoding + "\n");
                errorText.Append("exception date：" + "\t" + ex.ExceptionDate + "\n");
                errorText.Append("exception type：" + "\t" + ex.ExceptionType + "\n");
                errorText.Append("exception source：" + "\t" + ex.ExceptionSource + "\n");
                errorText.Append("exception method：" + "\t" + ex.ExceptionMethod + "\n");
                errorText.Append("exception message：" + "\t" + ex.ExceptionMessage + "\n");
                errorText.Append("call stack：" + "\n");
                errorText.Append(ex.CallStack + "\n");
                count++;
            }

            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfffff");
            Logger.WriteContentToFile(directory, directory + @"\" + "exception-" + fileName + ".log", errorText.ToString());

            errorText.Clear();
            return count;
        }

        /// <summary>
        /// 获取单注追加倍数
        /// </summary>
        /// <returns></returns>
        public int GetMulttple() => int.Parse(cmbMultiple.Text);
        /// <summary>
        /// 获取本次消费金额
        /// </summary>
        /// <returns></returns>
        public int GetRandomMoney()
        {
            if (CardInterface.SelectedIndex == 1)
            {
                return ParseTagValueToInt(lblRandomMonetary);
            }
            else if (CardInterface.SelectedIndex == 2)
            {
                return ParseTagValueToInt(lblComplexMonetary);
            }
            else
            {
                return ParseTagValueToInt(lblDantuoMonetary);
            }
        }
        /// <summary>
        /// 获取我的号码选择注数
        /// </summary>
        /// <returns></returns>
        public int GetDoubleBallConsumeZhu()
        {
            if (CardInterface.SelectedIndex == 1)
            {
                return ParseTagValueToInt(lblRandomBuyZhu);
            }
            else if (CardInterface.SelectedIndex == 2)
            {
                return ParseTagValueToInt(lblComplexBuyZhu);
            }
            else
            {
                return ParseTagValueToInt(lblDantuoBuyZhu);
            }
        }

        private bool isHide = true;
        private LotteryHistory _LotteryHistory = null;

        /// <summary>
        /// 历史开奖号码窗体
        /// </summary>
        private void lblShowHistory_Click(object sender, EventArgs e)
        {
            if (_LotteryHistory == null)
            {
                _LotteryHistory = new LotteryHistory(this);
                _LotteryHistory.Show();
            }
            else
            {
                if(isHide) _LotteryHistory.ShowThis();
                if (!isHide) _LotteryHistory.HideThis();
            }

            isHide = !isHide;

            Logger.Info("open history number windons");
        }

        //窗体 Load 时不要触发圆角设置
        bool isLoad = true;
        private void DoubleBallView_SizeChanged(object sender, EventArgs e)
        {
            if (!isLoad)
            {
                //窗体大小变化时重新应用圆角
                LoopDataAnalyse.SetWindowRegion(panBody, 50);
            }
        }

        private void lblClose_Click(object sender, EventArgs e) => Application.Exit();

        private void lblMin_Click(object sender, EventArgs e)
        {
            _OneselfComplex?.HideThis();
            WindowState = FormWindowState.Minimized;

            Logger.Info("minimized window.");
        }

        private void lblGitHubLink_Click(object sender, EventArgs e)
        {
            ExtendLabel link = sender as ExtendLabel;
            Process.Start(link.Text);

            Logger.Info("click author link : " + link.Text);
        }

        private void lblOneBonus_TextChanged(object sender, EventArgs e)
        {
            if (lblOneBonus.Text.Length > 7)
            {
                lblOneBonus.Location = new Point(221, lblOneBonus.Location.Y);
            }
            else
            {
                lblOneBonus.Location = new Point(229, lblOneBonus.Location.Y);
            }
        }

        private void lblTwoBonus_TextChanged(object sender, EventArgs e)
        {
            if (lblTwoBonus.Text.Length > 6)
            {
                lblTwoBonus.Location = new Point(230, lblTwoBonus.Location.Y);
            }
            else
            {
                lblTwoBonus.Location = new Point(236, lblTwoBonus.Location.Y);
            }
        }


        private int ParseTagValueToInt(Control control) => int.Parse(control.Tag.ToString());

        private long ParseTagValueToLong(Control control) => long.Parse(control.Tag.ToString());

        private void rbCustomizeMode_Click(object sender, EventArgs e) => panCustomizeMode.Enabled = rbCustomizeMode.Checked;

        #endregion
    }
}
