using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;

namespace Music.UserControls
{
    public partial class AutoCompleteBox : UserControl
    {
        public AutoCompleteBox()
        {
            InitializeComponent();
            Loaded += AutoCompleteBox_Loaded;
        }

        private void AutoCompleteBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (PART_Popup != null && PART_TextBox != null && PART_Popup.PlacementTarget == null)
            {
                PART_Popup.PlacementTarget = PART_TextBox;
            }

            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                if (PART_Popup != null)
                    PART_Popup.IsOpen = false;
            }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(AutoCompleteBox));

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static readonly DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register(nameof(DisplayMemberPath), typeof(string), typeof(AutoCompleteBox));

        public string DisplayMemberPath
        {
            get => (string)GetValue(DisplayMemberPathProperty);
            set => SetValue(DisplayMemberPathProperty, value);
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(
                nameof(SelectedItem),
                typeof(object),
                typeof(AutoCompleteBox),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemChanged));

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AutoCompleteBox box)
            {
                // если SelectedItem был установлен извне, обновляем Text
                box.Text = box.GetItemText(e.NewValue);
            }
        }


        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(AutoCompleteBox),
                new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        private string GetItemText(object item)
        {
            if (item == null) return "";

            if (!string.IsNullOrEmpty(DisplayMemberPath))
            {
                var prop = item.GetType().GetProperty(DisplayMemberPath, BindingFlags.Public | BindingFlags.Instance);
                if (prop != null)
                    return prop.GetValue(item)?.ToString() ?? "";
            }

            return item.ToString() ?? "";
        }

        private void PART_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PART_TextBox == null) return;
            Text = PART_TextBox.Text;
            UpdateFilter();
        }

        private void UpdateFilter()
        {
            if (ItemsSource == null || PART_ListBox == null || PART_Popup == null)
            {
                if (PART_Popup != null) PART_Popup.IsOpen = false;
                return;
            }

            string q = Text ?? "";

            var filtered = ItemsSource.Cast<object>()
                .Where(o => GetItemText(o).IndexOf(q, StringComparison.CurrentCultureIgnoreCase) >= 0)
                .ToList();

            PART_ListBox.ItemsSource = filtered;

            PART_Popup.IsOpen = filtered.Count > 0;
            if (filtered.Count > 0)
                PART_ListBox.SelectedIndex = 0;
        }

        private void Commit(object item)
        {
            if (item == null) return;

            var oldItem = SelectedItem;
            SelectedItem = item;
            Text = GetItemText(item);

            if (PART_TextBox != null)
            {
                PART_TextBox.Text = Text;
                PART_TextBox.CaretIndex = Text.Length;
                PART_TextBox.Focus();
            }

            if (PART_Popup != null)
                PART_Popup.IsOpen = false;

            OnSelectionChanged(oldItem, SelectedItem);
        }


        private void PART_ListBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (PART_ListBox == null) return;
            Commit(PART_ListBox.SelectedItem);
        }

        private void PART_TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (PART_Popup == null || PART_ListBox == null) return;

            UpdateFilter();

            Dispatcher.BeginInvoke((Action)(() =>
            {
                if (PART_ListBox.Items.Count > 0)
                {
                    if (PART_Popup.PlacementTarget == null && PART_TextBox != null)
                        PART_Popup.PlacementTarget = PART_TextBox;

                    PART_Popup.IsOpen = true;
                }
            }), DispatcherPriority.Input);
        }

        private void PART_TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PART_ListBox == null || PART_Popup == null) return;

            if (!PART_ListBox.IsKeyboardFocusWithin && !PART_ListBox.IsMouseOver)
            {
                PART_Popup.IsOpen = false;
            }
        }

        private void PART_ListBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PART_TextBox == null || PART_Popup == null) return;

            if (!PART_TextBox.IsKeyboardFocusWithin)
                PART_Popup.IsOpen = false;
        }

        private void PART_TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (PART_ListBox == null) return;

            if (e.Key == Key.Down && PART_ListBox.Items.Count > 0)
            {
                PART_ListBox.Focus();
                PART_ListBox.SelectedIndex = 0;
                e.Handled = true;
            }
        }

        private void PART_ListBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Commit(PART_ListBox?.SelectedItem!);
                e.Handled = true;
            }
        }

        public event SelectionChangedEventHandler? SelectionChanged;

        private void OnSelectionChanged(object oldItem, object newItem)
        {
            SelectionChanged?.Invoke(this, new SelectionChangedEventArgs(
                Selector.SelectionChangedEvent,
                oldItem != null ? new[] { oldItem } : Array.Empty<object>(),
                newItem != null ? new[] { newItem } : Array.Empty<object>()));
        }

    }
}
