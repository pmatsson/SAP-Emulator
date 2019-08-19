using MQChatter.Common;
using MQChatter.Model.Action;
using MQChatter.Model.Condition;
using MQChatter.Model.Trigger;
using MQChatter.Processor;
using MQChatter.ViewModel.RuleGroup;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Threading;
using System.Xml.Serialization;

namespace MQChatter.ViewModel
{
    public class RuleViewModel : NotifyPropertyChangedBase
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private ObservableCollection<ARuleGroup> _ruleGroups;
        private ListCollectionView _groupedRuleGroups;
        private int _processedRulesCount;
        private int _errorCount;
        private RuleProcessor _ruleProcessor;
        private bool _isRunningEmulator;
        private string _openDocument;
        private ARuleGroup _selectedRule;
        private string _runTime;

        private DispatcherTimer _timer;
        private DateTime _startTime;

        public ObservableCollection<ARuleGroup> RuleGroups
        {
            get => _ruleGroups;
            set => SetProperty(ref _ruleGroups, value);
        }

        public ListCollectionView GroupedRuleGroups
        {
            get => _groupedRuleGroups;
            set => SetProperty(ref _groupedRuleGroups, value);
        }

        public ARuleGroup SelectedRule
        {
            get => _selectedRule;
            set => SetProperty(ref _selectedRule, value);
        }

        public int ProcessedRuleCount
        {
            get => _processedRulesCount;
            set => SetProperty(ref _processedRulesCount, value);
        }

        public int ErrorCount
        {
            get => _errorCount;
            set => SetProperty(ref _errorCount, value);
        }

        public bool IsRunningEmulator
        {
            get => _isRunningEmulator;
            set => SetProperty(ref _isRunningEmulator, value);
        }

        public string OpenDocument
        {
            get => _openDocument;
            set => SetProperty(ref _openDocument, value);
        }

        public string RunTime
        {
            get => _runTime;
            set => SetProperty(ref _runTime, value);
        }

        public RuleViewModel()
        {
            RuleGroups = new ObservableCollection<ARuleGroup>();
            GroupedRuleGroups = new ListCollectionView(RuleGroups);
            GroupedRuleGroups.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
            GroupedRuleGroups.IsLiveGrouping = true;

            _timer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 50), DispatcherPriority.Background, t_Tick, Dispatcher.CurrentDispatcher);
            _timer.Stop();
            _isRunningEmulator = false;
        }

        private void t_Tick(object sender, EventArgs e)
        {
            RunTime = (DateTime.Now - _startTime).ToString("hh\\:mm\\:ss\\.fff");
        }

        public async void StartEmulation()
        {
            _startTime = DateTime.Now;
            IsRunningEmulator = true;
            ProcessedRuleCount = 0;
            _ruleProcessor = new RuleProcessor(RuleGroups.ToList());
            _ruleProcessor.RuleProcessed += RuleProcessor_RuleProcessed;
            _ruleProcessor.ErrorEncountered += _ruleProcessor_ErrorEncountered;
            _ruleProcessor.MessageReceived += _ruleProcessor_MessageReceived;

            // Logging configuration
            NLog.Config.LoggingConfiguration config = new NLog.Config.LoggingConfiguration();
            NLog.Targets.FileTarget logfile = new NLog.Targets.FileTarget("logfile")
            {
                FileName = "chatter_" + DateTime.Now.ToString("yyyy-MM-ddTHH_mm_ss") + ".log"
            };

            NLog.Targets.ConsoleTarget logconsole = new NLog.Targets.ConsoleTarget("logconsole");
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, logfile);
            LogManager.Configuration = config;

            try
            {
                _timer?.Start();
                await _ruleProcessor.Start();
            }
            catch
            {
                logger.Warn("Stopped rule processing due to exception.");
            }

            await Task.Delay(1000);

            DestroyRuleProcessor();

            return;
        }

        private void _ruleProcessor_ErrorEncountered(object sender, string e)
        {
            ErrorCount++;

            logger.Trace("Error registered");
        }

        private void _ruleProcessor_MessageReceived(object sender, string s)
        {
            logger.Trace("Retrieved message: {0}", s);
        }

        public void CancelEmulation()
        {
            DestroyRuleProcessor();
        }

        private void DestroyRuleProcessor()
        {
            _timer?.Stop();
            if (_ruleProcessor != null)
            {
                _ruleProcessor.Stop();
                _ruleProcessor.RuleProcessed -= RuleProcessor_RuleProcessed;
                _ruleProcessor.ErrorEncountered -= _ruleProcessor_ErrorEncountered;
                _ruleProcessor.MessageReceived -= _ruleProcessor_MessageReceived;
                _ruleProcessor = null;
                IsRunningEmulator = false;
                ProcessedRuleCount = 0;
                ErrorCount = 0;
            }
        }

        private void RuleProcessor_RuleProcessed(object sender, ARuleGroup rule)
        {
            rule.ProcessCount++;
            ProcessedRuleCount = RuleGroups.Where(x => x?.ProcessCount > 0).Count();
            logger.Trace("Rule " + rule.Title + " processed. This rule has been processed {0} time(s)", rule.ProcessCount.ToString());
        }

        public void CreateRule()
        {
            RuleGroups.Add(new ARuleGroup());
        }

        public void RemoveSelectedRule()
        {
            if (SelectedRule != null)
            {
                RuleGroups.Remove(SelectedRule);
            }
        }

        public void SerializeRules(string filename)
        {
            TextWriter writer = new StreamWriter(filename);
            RuleSerializer.Serialize(RuleGroups.ToList(), writer);
        }

        public void DeSerializeRules(string filename)
        {
            RuleGroups = RuleSerializer.Deserialize(filename);
            GroupedRuleGroups = new ListCollectionView(RuleGroups);
            GroupedRuleGroups.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
            GroupedRuleGroups.IsLiveGrouping = true;
        }

        public static class RuleSerializer
        {
            public static void Serialize(List<ARuleGroup> rules, TextWriter stream)
            {
                List<Type> ruleTypes = new List<Type>();
                ARuleGroup rule = new ARuleGroup();

                foreach (TriggerBase trigger in rule.TriggerGroup.Triggers.First().AvailableTriggers)
                {
                    Type type = trigger.GetType();
                    if (!ruleTypes.Contains(type))
                    {
                        ruleTypes.Add(type);
                    }
                }

                foreach (ConditionBase condition in rule.ConditionGroup.Conditions.First().AvailableConditions)
                {
                    Type type = condition.GetType();
                    if (!ruleTypes.Contains(type))
                    {
                        ruleTypes.Add(type);
                    }
                }

                foreach (ActionBase action in rule.ActionGroup.Actions.First().AvailableActions)
                {
                    Type type = action.GetType();
                    if (!ruleTypes.Contains(type))
                    {
                        ruleTypes.Add(type);
                    }
                }

                XmlSerializer serializer = new XmlSerializer(typeof(List<ARuleGroup>), ruleTypes.ToArray());
                serializer.Serialize(stream, rules);
                stream.Close();
            }

            public static ObservableCollection<ARuleGroup> Deserialize(string path)
            {
                List<Type> ruleTypes = new List<Type>();
                ARuleGroup ruleGroup = new ARuleGroup();

                foreach (TriggerBase trigger in ruleGroup.TriggerGroup.Triggers.First().AvailableTriggers)
                {
                    Type type = trigger.GetType();
                    if (!ruleTypes.Contains(type))
                    {
                        ruleTypes.Add(type);
                    }
                }

                foreach (ConditionBase condition in ruleGroup.ConditionGroup.Conditions.First().AvailableConditions)
                {
                    Type type = condition.GetType();
                    if (!ruleTypes.Contains(type))
                    {
                        ruleTypes.Add(type);
                    }
                }

                foreach (ActionBase action in ruleGroup.ActionGroup.Actions.First().AvailableActions)
                {
                    Type type = action.GetType();
                    if (!ruleTypes.Contains(type))
                    {
                        ruleTypes.Add(type);
                    }
                }

                ObservableCollection<ARuleGroup> result = null;
                TextReader reader = new StreamReader(path);
                XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<ARuleGroup>), ruleTypes.ToArray());
                try
                {
                    result = (ObservableCollection<ARuleGroup>)serializer.Deserialize(reader);
                }
                catch (InvalidOperationException)
                {
                    // TODO: Handle exception
                    return null;
                }
                finally
                {
                    reader.Close();
                }

                return result;
            }
        }
    }
}