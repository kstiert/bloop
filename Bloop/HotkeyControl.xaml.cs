﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using NHotkey.Wpf;
using Bloop.Core.i18n;
using Bloop.Infrastructure.Hotkey;
using Bloop.Plugin;

namespace Bloop
{
    public partial class HotkeyControl : UserControl
    {
        public HotkeyModel CurrentHotkey { get; private set; }
        public bool CurrentHotkeyAvailable { get; private set; }

        public event EventHandler HotkeyChanged;

        protected virtual void OnHotkeyChanged()
        {
            EventHandler handler = HotkeyChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public HotkeyControl()
        {
            InitializeComponent();
        }

        void TbHotkey_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            tbMsg.Visibility = Visibility.Hidden;

            //when alt is pressed, the real key should be e.SystemKey
            Key key = (e.Key == Key.System ? e.SystemKey : e.Key);

            SpecialKeyState specialKeyState = GlobalHotkey.Instance.CheckModifiers();

            var hotkeyModel = new HotkeyModel(
                specialKeyState.AltPressed, 
                specialKeyState.ShiftPressed, 
                specialKeyState.WinPressed, 
                specialKeyState.CtrlPressed, 
                key);

            var hotkeyString = hotkeyModel.ToString();

            if (hotkeyString == tbHotkey.Text)
            {
                return;
            }

            Dispatcher.DelayInvoke("HotkeyAvailabilityTest", 
                o =>
                {
                    SetHotkey(hotkeyModel);
                },
                TimeSpan.FromMilliseconds(500));
        }

        public void SetHotkey(HotkeyModel keyModel, bool triggerValidate = true)
        {
            CurrentHotkey = keyModel;

            tbHotkey.Text = CurrentHotkey.ToString();
            tbHotkey.Select(tbHotkey.Text.Length, 0);

            if (triggerValidate)
            {
                CurrentHotkeyAvailable = CheckHotkeyAvailability();
                if (!CurrentHotkeyAvailable)
                {
                    tbMsg.Foreground = new SolidColorBrush(Colors.Red);
                    tbMsg.Text = InternationalizationManager.Instance.GetTranslation("hotkeyUnavailable");
                }
                else
                {
                    tbMsg.Foreground = new SolidColorBrush(Colors.Green);
                    tbMsg.Text = InternationalizationManager.Instance.GetTranslation("succeed");
                }
                tbMsg.Visibility = Visibility.Visible;
                OnHotkeyChanged();
            }
        }

        public void SetHotkey(string keyStr, bool triggerValidate = true)
        {
            SetHotkey(new HotkeyModel(keyStr), triggerValidate);
        }

        private bool CheckHotkeyAvailability()
        {
            try
            {
                HotkeyManager.Current.AddOrReplace("HotkeyAvailabilityTest", CurrentHotkey.CharKey, CurrentHotkey.ModifierKeys, (sender, e) => {});

                return true;
            }
            catch
            {
            }
            finally
            {
                HotkeyManager.Current.Remove("HotkeyAvailabilityTest");
            }

            return false;
        }
    }
}
