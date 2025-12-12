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
        // Флаг для подавления автоматического TextChanged при внутреннем обновлении (Commit)
        private bool _suppressTextChangedRoutedEvent = false;

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

        // -------------------- ItemsSource --------------------
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(AutoCompleteBox));

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        // -------------------- DisplayMemberPath --------------------
        public static readonly DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register(nameof(DisplayMemberPath), typeof(string), typeof(AutoCompleteBox));

        public string DisplayMemberPath
        {
            get => (string)GetValue(DisplayMemberPathProperty);
            set => SetValue(DisplayMemberPathProperty, value);
        }

        // -------------------- SelectedItem (with callback) --------------------
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
                // если SelectedItem был установлен извне, обновляем Text (через DP)
                // это вызовет OnTextPropertyChanged и (если не подавлено) поднимет RoutedEvent TextChanged
                box.Text = box.GetItemText(e.NewValue);
            }
        }

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        // -------------------- Text (DP) с callback --------------------
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                nameof(Text),
                typeof(string),
                typeof(AutoCompleteBox),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTextPropertyChanged));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AutoCompleteBox box)
            {
                // Обновим реальный TextBox (если он существует) — но только если отличается,
                // чтобы не вызвать лишние TextChanged события/циклы.
                string newText = e.NewValue as string ?? string.Empty;

                if (box.PART_TextBox != null && box.PART_TextBox.Text != newText)
                {
                    // обновляем без лишней логики (поставим caret в конец)
                    box.PART_TextBox.Text = newText;
                    box.PART_TextBox.CaretIndex = newText.Length;
                }

                // Поднимем RoutedEvent TextChanged, чтобы внешняя XAML-обработчик видел изменения,
                // происходящие программно (через DP), но только если мы не в режиме подавления.
                if (!box._suppressTextChangedRoutedEvent)
                    box.RaiseTextChangedRoutedEvent();
            }
        }

        // -------------------- Routed TextChanged event --------------------
        public static readonly RoutedEvent TextChangedEvent =
            EventManager.RegisterRoutedEvent(
                nameof(TextChanged),
                RoutingStrategy.Bubble,
                typeof(TextChangedEventHandler),
                typeof(AutoCompleteBox));

        // CLR "обёртка" для удобства подписки в коде; в XAML можно писать TextChanged="..."
        public event TextChangedEventHandler TextChanged
        {
            add => AddHandler(TextChangedEvent, value);
            remove => RemoveHandler(TextChangedEvent, value);
        }

        private void RaiseTextChangedRoutedEvent()
        {
            var args = new TextChangedEventArgs(TextBox.TextChangedEvent, UndoAction.None)
            {
                RoutedEvent = TextChangedEvent,
                Source = this
            };
            RaiseEvent(args);
        }

        // -------------------- Вспомогательные методы --------------------
        private string GetItemText(object item)
        {
            if (item == null) return string.Empty;

            if (!string.IsNullOrEmpty(DisplayMemberPath))
            {
                var prop = item.GetType().GetProperty(DisplayMemberPath, BindingFlags.Public | BindingFlags.Instance);
                if (prop != null)
                    return prop.GetValue(item)?.ToString() ?? string.Empty;
            }

            return item.ToString() ?? string.Empty;
        }

        // -------------------- Поведение TextBox --------------------
        private void PART_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PART_TextBox == null) return;

            // Синхронизируем DP Text (это вызовет OnTextPropertyChanged, но там есть защита от рекурсии)
            if (Text != PART_TextBox.Text)
                Text = PART_TextBox.Text;

            // обновляем подсказки
            UpdateFilter();

            // В большинстве случаев OnTextPropertyChanged уже поднимет RoutedEvent.
            // Здесь можно вызвать RaiseTextChangedRoutedEvent() дополнительно, но это приведёт к двойному событию.
            // Оставим только DP → OnTextPropertyChanged → Raise.
        }

        // -------------------- Фильтрация --------------------
        private void UpdateFilter()
        {
            if (ItemsSource == null || PART_ListBox == null || PART_Popup == null)
            {
                if (PART_Popup != null) PART_Popup.IsOpen = false;
                return;
            }

            string q = Text ?? string.Empty;

            var filtered = ItemsSource.Cast<object>()
                .Where(o => GetItemText(o).IndexOf(q, StringComparison.CurrentCultureIgnoreCase) >= 0)
                .ToList();

            PART_ListBox.ItemsSource = filtered;

            PART_Popup.IsOpen = filtered.Count > 0;
            if (filtered.Count > 0)
                PART_ListBox.SelectedIndex = -1; // не выделяем автоматически — чтобы не показывать серую рамку
        }

        // -------------------- Commit / Selection --------------------
        private void Commit(object item)
        {
            if (item == null) return;

            var oldItem = SelectedItem;

            // Пометим, что делаем внутреннее обновление — подавим автоматический TextChanged
            _suppressTextChangedRoutedEvent = true;

            // Сначала SelectedItem — чтобы внешние обработчики (на TextChanged) увидели его при наступлении события
            SelectedItem = item;

            // Затем обновляем Text через DP (OnTextPropertyChanged не поднимет событие из-за suppression)
            Text = GetItemText(item);

            // Снимаем подавление
            _suppressTextChangedRoutedEvent = false;

            // Закрываем popup
            if (PART_Popup != null)
                PART_Popup.IsOpen = false;

            // Вручную поднимем TextChanged (теперь SelectedItem уже установлен)
            RaiseTextChangedRoutedEvent();

            // Поднимем SelectionChanged CLR-событие
            OnSelectionChanged(oldItem, SelectedItem);
        }

        // -------------------- ListBox handlers --------------------
        private void PART_ListBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (PART_ListBox == null) return;
            Commit(PART_ListBox.SelectedItem);
        }

        private void PART_ListBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Commit(PART_ListBox?.SelectedItem);
                e.Handled = true;
            }
        }

        // -------------------- Focus / Popup behavior --------------------
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

        // -------------------- SelectionChanged CLR event --------------------
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
