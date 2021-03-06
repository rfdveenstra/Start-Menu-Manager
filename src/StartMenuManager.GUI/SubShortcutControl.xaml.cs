﻿// -------------------------------------------------------------------------------------------------
// Start Menu Manager - © Copyright 2020 - Jam-Es.com
// Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// -------------------------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using StartMenuManager.Core.DataStructures;
using StartMenuManager.Core.DataStructures.Actions;
using StartMenuManager.GUI.Structures;

namespace StartMenuManager.GUI
{
    /// <summary>
    /// Interaction logic for ShortcutControl.xaml
    /// </summary>
    public partial class SubShortcutControl : UserControl
    {
        private bool eventBlock = false;
        private ShortcutControl parent;

        public SubShortcutControl(ShortcutControl parentControl)
        {
            parent = parentControl;
            InitializeComponent();
            InitControl(ShortcutType.Web);
        }

        public ShortcutType ShortcutType { get; set; }

        public Shortcut Shortcut { get; set; }

        private void InitControl(ShortcutType shortcutType)
        {
            ShortcutType = shortcutType;

            // Set up the underlying Shortcut class
            Shortcut = new Shortcut();
            eventBlock = true;
            Shortcut.Name = "My Shortcut";
            SetDefaultAction();

            // Set up right UI:
            SetIcon();
            InitUiBasedOnType();
            SetFieldValuesFromShortcut();

            eventBlock = false;
        }

        private void SetDefaultAction()
        {
            Shortcut.Actions.Clear();

            switch (ShortcutType)
            {
                case ShortcutType.Web:
                    Shortcut.Actions.Add(new WebsiteAction());
                    break;
                case ShortcutType.File:
                    Shortcut.Actions.Add(new FileAction());
                    break;
                case ShortcutType.Folder:
                    Shortcut.Actions.Add(new FolderAction());
                    break;
                case ShortcutType.Software:
                    Shortcut.Actions.Add(new SoftwareAction());
                    break;
                case ShortcutType.Command:
                    Shortcut.Actions.Add(new CommandAction());
                    break;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////
        // Update UI from Shortcut class

        public void UpdateUi()
        {
            SetIcon();
            InitUiBasedOnType();
            SetFieldValuesFromShortcut();
        }

        private void SetIcon()
        {
            switch (ShortcutType)
            {
                case ShortcutType.Web:
                    Icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Web;
                    break;
                case ShortcutType.File:
                    Icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.FileDocument;
                    break;
                case ShortcutType.Folder:
                    Icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Folder;
                    break;
                case ShortcutType.Software:
                    Icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
                    break;
                case ShortcutType.Command:
                    Icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Terminal;
                    break;
            }
        }

        private void InitUiBasedOnType()
        {
            WebsiteUi.Visibility = Visibility.Collapsed;
            FileUi.Visibility = Visibility.Collapsed;
            FolderUi.Visibility = Visibility.Collapsed;
            SoftwareUi.Visibility = Visibility.Collapsed;
            CommandUi.Visibility = Visibility.Collapsed;

            switch (ShortcutType)
            {
                case ShortcutType.Web:
                    WebsiteUi.Visibility = Visibility.Visible;
                    break;
                case ShortcutType.File:
                    FileUi.Visibility = Visibility.Visible;
                    break;
                case ShortcutType.Folder:
                    FolderUi.Visibility = Visibility.Visible;
                    break;
                case ShortcutType.Software:
                    SoftwareUi.Visibility = Visibility.Visible;
                    break;
                case ShortcutType.Command:
                    CommandUi.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void SetFieldValuesFromShortcut()
        {
            eventBlock = true;
            ShortcutTitleName.Text = $"{Enum.GetName(typeof(ShortcutType), ShortcutType)} Action";
            eventBlock = true;
            ShortcutTypeComboBox.SelectedIndex = (int)ShortcutType;

            switch (ShortcutType)
            {
                case ShortcutType.Web:
                    WebsiteAction webaction = (WebsiteAction)Shortcut.Actions[0];
                    eventBlock = true;
                    WebsiteUriField.Text = webaction.Url;
                    eventBlock = true;
                    break;
                case ShortcutType.File:
                    FileAction fileAction = (FileAction)Shortcut.Actions[0];
                    eventBlock = true;
                    FileUi_Path.Text = fileAction.Path;
                    break;
                case ShortcutType.Folder:
                    FolderAction folderAction = (FolderAction)Shortcut.Actions[0];
                    eventBlock = true;
                    FolderUi_Path.Text = folderAction.Path;
                    break;
                case ShortcutType.Software:
                    SoftwareAction softwareAction = (SoftwareAction)Shortcut.Actions[0];
                    eventBlock = true;
                    SoftwareUi_Path.Text = softwareAction.Path;
                    break;
                case ShortcutType.Command:
                    CommandAction comaction = (CommandAction)Shortcut.Actions[0];
                    eventBlock = true;
                    CommandUi_Comand.Text = comaction.Command;
                    eventBlock = true;
                    CommandUi_KeepOpen.IsChecked = comaction.KeepOpen;
                    break;
            }

            eventBlock = false;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////
        // Update Shortcut class from UI Events

        // Main changed events

        private void ShortcutTypeChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!eventBlock)
            {
                ShortcutType = (ShortcutType)ShortcutTypeComboBox.SelectedIndex;
                SetDefaultAction();
                SetIcon();
                InitUiBasedOnType();
                SetFieldValuesFromShortcut();
            }

            eventBlock = false;
        }

        // Website action changed events

        private void WebsiteUriTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!eventBlock)
            {
                WebsiteAction action = (WebsiteAction)Shortcut.Actions[0];
                action.Url = WebsiteUriField.Text;
            }

            eventBlock = false;
        }

        // File action changed events

        private void FileUi_PathChanged(object sender, RoutedEventArgs e)
        {
            if (!eventBlock)
            {
                FileAction action = (FileAction)Shortcut.Actions[0];
                action.Path = FileUi_Path.Text;
            }

            eventBlock = false;
        }

        private void FileUi_PathSelectPressed(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.SpecialFolder.Personal.ToString();
            if (openFileDialog.ShowDialog() == true)
            {
                if (!string.IsNullOrEmpty(openFileDialog.FileName))
                {
                    FileAction action = (FileAction)Shortcut.Actions[0];
                    action.Path = openFileDialog.FileName;

                    if (FileUi_Path.Text != openFileDialog.FileName)
                    {
                        eventBlock = true;
                        FileUi_Path.Text = openFileDialog.FileName;
                    }
                }
            }
        }

        // Folder action changed events

        private void FolderUi_PathChanged(object sender, RoutedEventArgs e)
        {
            if (!eventBlock)
            {
                FolderAction action = (FolderAction)Shortcut.Actions[0];
                action.Path = FolderUi_Path.Text;
            }

            eventBlock = false;
        }

        private void FolderUi_PathSelectPressed(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog openFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            openFolderDialog.Description = "Select the directory for the shortcut to open.";
            System.Windows.Forms.DialogResult result = openFolderDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(openFolderDialog.SelectedPath))
                {
                    FolderAction action = (FolderAction)Shortcut.Actions[0];
                    action.Path = openFolderDialog.SelectedPath;

                    if (FolderUi_Path.Text != openFolderDialog.SelectedPath)
                    {
                        eventBlock = true;
                        FolderUi_Path.Text = openFolderDialog.SelectedPath;
                    }
                }
            }
        }

        // Software action changed evetns

        private void SoftwareUi_PathChanged(object sender, RoutedEventArgs e)
        {
            if (!eventBlock)
            {
                SoftwareAction action = (SoftwareAction)Shortcut.Actions[0];
                action.Path = SoftwareUi_Path.Text;
            }

            eventBlock = false;
        }

        private void SoftwareUi_PathSelectPressed(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Exe Files (*.exe)|*.exe|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.SpecialFolder.ProgramFiles.ToString();
            if (openFileDialog.ShowDialog() == true)
            {
                if (!string.IsNullOrEmpty(openFileDialog.FileName))
                {
                    SoftwareAction action = (SoftwareAction)Shortcut.Actions[0];
                    action.Path = openFileDialog.FileName;

                    if (SoftwareUi_Path.Text != openFileDialog.FileName)
                    {
                        eventBlock = true;
                        SoftwareUi_Path.Text = openFileDialog.FileName;
                    }
                }
            }
        }

        // Command action changed events

        private void CommandUi_CommandTextChanged(object sender, RoutedEventArgs e)
        {
            if (!eventBlock)
            {
                CommandAction action = (CommandAction)Shortcut.Actions[0];
                action.Command = CommandUi_Comand.Text;
            }

            eventBlock = false;
        }

        private void CommandUi_KeepOpen_Checked(object sender, RoutedEventArgs e)
        {
            if (!eventBlock)
            {
                CommandAction action = (CommandAction)Shortcut.Actions[0];
                action.KeepOpen = true;
            }

            eventBlock = false;
        }

        private void CommandUi_KeepOpen_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!eventBlock)
            {
                CommandAction action = (CommandAction)Shortcut.Actions[0];
                action.KeepOpen = false;
            }

            eventBlock = false;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////
        // Other Events

        private void DeleteShortcutButtonClick(object sender, RoutedEventArgs e)
        {
            DeleteShortcut();
        }

        public void MenuComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (MenuComboBox.SelectedIndex)
            {
                case 0:
                    TestRunShortcut();
                    break;
                case 1:
                    DuplicateShortcut();
                    break;
                case 2:
                    MoveUpShortcut();
                    break;
                case 3:
                    MoveDownShortcut();
                    break;
                case 4:
                    DeleteShortcut();
                    break;
            }

            MenuComboBox.SelectedIndex = -1;
        }

        private void TestRunShortcut()
        {
            ShortcutListArea.TestRunShortcut(Shortcut, true);
        }

        private void DuplicateShortcut()
        {
            parent.DuplicateChild(this);
        }

        private void MoveUpShortcut()
        {
            parent.MoveUpChild(this);
        }

        private void MoveDownShortcut()
        {
            parent.MoveDownChild(this);
        }

        private void DeleteShortcut()
        {
            parent.DeleteChild(this);
        }
    }
}
