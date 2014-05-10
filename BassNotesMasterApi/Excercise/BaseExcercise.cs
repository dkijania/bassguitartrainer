using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using BassNotesMasterApi.Const;
using BassNotesMasterApi.Fretboard;
using BassNotesMasterApi.Notation;
using BassNotesMasterApi.Settings;
using BassNotesMasterApi.Statistics;
using BassNotesMasterApi.Utils;
using BassNotesMasterApi.Utils.ResultSerializer;

namespace BassNotesMasterApi.Excercise
{
    public abstract class BaseExcercise : IExcercise, ISettingListener
    {
        protected List<StringFretPair> StringFretPairs;
        protected readonly RandomItemGenerator Generator;
        protected readonly NotesToStringFretBoardMapping FretBoardMapping = new NotesToStringFretBoardMapping();
        protected readonly FretboardManager FretBoardManager;
        protected Note NoteToFind;
        protected IExcerciseOptionGuiManager GuiManager;
        protected IVisibilityManager VisibilityManager;
        protected Options Options = new Options();
        protected MusicNotation MusicNotationManager;
        protected IResultSerializer ResultSerializer = ManagersLocator.Instance.ResultSerializer;

        public StatisticRow StatisticData { get; set; }
        public ISelectionSetter SelectionSetter { get; private set; }
        protected static int Attempts;
        protected static int TryNo;
        protected double DelayValue;

        protected BaseExcercise(Settings.Settings settings,
                                IExcerciseOptionGuiManager guiManager, IVisibilityManager visibilityManager)
            : this(settings, guiManager, visibilityManager, new DefaultSelectionSetter())

        {
        }

        protected BaseExcercise(Settings.Settings settings,
                                IExcerciseOptionGuiManager guiManager, IVisibilityManager visibilityManager,
                                ISelectionSetter setter)
        {
            SelectionSetter = setter;
            VisibilityManager = visibilityManager;
            Attempts = settings.AttemptsCount.Value;
            DelayValue = settings.DelayTime.Value;
            FretBoardManager = ManagersLocator.Instance.FretboardManager;
            MusicNotationManager = ManagersLocator.Instance.MusicNotationManager;
            Generator = new RandomItemGenerator();
            GuiManager = guiManager;
            Subscribe(settings);
        }

        public bool IsPaused { get; private set; }

        protected void ResetTryNo()
        {
            TryNo = 0;
        }

        public virtual void Start(IEnumerable<StringFretPair> pairs)
        {
            BeforeStart();
            ManagersLocator.Instance.Mode = ManagerMode.Excercise;
            ReadSettings();
            EnableDefinedComponentsExlusive();
            SetTestItems(pairs);
            VisibilityManager.SetOnTop(ComponentId.Statistic);
            StatisticData.ResetAll();
            RenewSubscriptions();
            IntroduceSettingsForNotesViewTipsVisibility();
            AfterStart();
            NextTest();
        }

        public virtual void EnableDefinedComponentsExlusive()
        {
            EnableComponentsExlusive(ComponentsVisibleDuringExcercise);
        }

        public virtual void EnableComponentsExlusive(params ComponentId[] components)
        {
            components = AddPlayerComponentIfRequired(components);
            VisibilityManager.SetVisibleExlusive(components);
            VisibilityManager.SetOnTop(components);
        }

        private ComponentId[] AddPlayerComponentIfRequired(ComponentId[] components)
        {
            if (GuiManager.GetBooleanOption(Options.PlayNote))
            {
                var list = new List<ComponentId>(components) {ComponentId.Player};
                components = list.ToArray();
            }
            return components;
        }

        protected void PlaySoundIfEnabled(params StringFretPair[] stringFretPairs)
        {
            if (Options.ShouldplayNote)
                ManagersLocator.Instance.PlayerManager.PlayNote(stringFretPairs);
        }


        public virtual void DisableAllComponents()
        {
            VisibilityManager.HideAll();
        }

        protected void ReadSettings()
        {
            Options.ShouldShowTips = GuiManager.GetBooleanOption(Options.ShowTips);
            Options.ShouldplayNote = GuiManager.GetBooleanOption(Options.PlayNote);
            Options.AlwaysStartFromLowestNote = GuiManager.GetBooleanOption(Options.AlwaysStartFromLowestNoteParamName);
            Options.ShouldRequireCorrectOctave = GuiManager.GetBooleanOption(Options.RequireCorrectOctave);
            Options.ShouldHideNoteLabel = GuiManager.GetBooleanOption(Options.HideNoteLabel);
        }

        private void SetTestItems(IEnumerable<StringFretPair> pairs)
        {
            if (pairs == null || !pairs.Any())
            {
                throw new ExcerciseException("No Selected Positions");
            }
            StringFretPairs = new List<StringFretPair>(pairs);
        }

        public virtual void Stop()
        {
            BeforeStop();
            ClearView();
            FretBoardManager.SelectionManager.CleanUp();
            ShowAllComponents();
            ResultSerializer.Save(StatisticData);
            AfterStop();
        }

        private void ShowAllComponents()
        {
            VisibilityManager.ShowAll();
        }

        protected virtual void ClearView()
        {
            FretBoardManager.ClearView();
            MusicNotationManager.ResetGui();
        }

        public void Skip()
        {
            StatisticData.Skip();
            NextTest();
        }

        public virtual void Pause()
        {
            BeforePause();
            ManagersLocator.Instance.Mode = ManagerMode.Selection;
            FretBoardManager.SelectionManager.SelectItems(StringFretPairs.ToArray());
            IsPaused = true;
            AfterPause();
        }

        public virtual void Continue(StringFretPair[] pairs)
        {
            BeforeContinue();
            ManagersLocator.Instance.Mode = ManagerMode.Excercise;
            EnableDefinedComponentsExlusive();
            SetTestItems(pairs);
            ReadSettings();
            ClearView();
            RenewSubscriptions();
            ContinuteTest();
            IsPaused = false;
            AfterContinue();
        }

        protected void RegisterResult(bool result)
        {
            StatisticData.AddResult(result);
        }

        protected StringFretPair GetRandomItem()
        {
            return StringFretPairs[GetRandomIndex()];
        }

        private int GetRandomIndex()
        {
            return Generator.Next(StringFretPairs.Count);
        }

        public void Subscribe(Settings.Settings settings)
        {
            settings.SettingChangedEvent += OnSettingChanged;
        }

        public void OnSettingChanged(Settings.Settings settings)
        {
            Attempts = settings.AttemptsCount.Value;
            DelayValue = settings.DelayTime.Value;
        }

        protected void WaitXAndCheckResult(EventHandler action, double second)
        {
            var timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(second)};
            timer.Start();
            timer.Tick += (sender, args) => timer.Stop();
            timer.Tick += action;
        }

        public virtual void ShowOptions()
        {
            GuiManager.Clear();
            foreach (var option in OptionsToShow)
            {
                GuiManager.AddOption(option, false);
            }
            GuiManager.Build();
        }

        protected void IntroduceSettingsForNotesViewTipsVisibility()
        {
            var notesViewManager = ManagersLocator.Instance.NotesViewManager;
            if (Options.ShouldShowTips)
            {
                var notes = FretBoardMapping.GetNotes(StringFretPairs);
                notesViewManager.EnableNotesButtonsExlusive(notes);
            }
            else
            {
                notesViewManager.EnableNotesButtons();
            }
            SetShowNoteLabelOptions();
        }

        protected void SetShowNoteLabelOptions()
        {
            FretBoardManager.FretBoard.FretBoardGuiBuilder.HideNoteLabel = Options.ShouldHideNoteLabel;
        }


        protected abstract void RenewSubscriptions();
        protected abstract void NextTest();
        protected abstract void ContinuteTest();
        protected abstract ComponentId[] ComponentsVisibleDuringExcercise { get; }
        protected abstract string[] OptionsToShow { get; }


        protected virtual void BeforeStart()
        {
        }

        protected virtual void AfterStart()
        {
        }

        protected virtual void BeforeStop()
        {
        }

        protected virtual void AfterStop()
        {
        }

        protected virtual void BeforeContinue()
        {
        }

        protected virtual void AfterContinue()
        {
        }

        protected virtual void BeforePause()
        {
        }

        protected virtual void AfterPause()
        {
        }
    }
}