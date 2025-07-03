using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp1
{
    public static class TreeViewItemRightClickBehavior
    {
        public static readonly DependencyProperty EnableRightClickSelectProperty = DependencyProperty.RegisterAttached("EnableRightClickSelect", typeof(bool), typeof(TreeViewItemRightClickBehavior), new UIPropertyMetadata(false, OnEnableRightClickSelectChanged));

        public static bool GetEnableRightClickSelect(DependencyObject obj) { return (bool)obj.GetValue(EnableRightClickSelectProperty); }
        public static void SetEnableRightClickSelect(DependencyObject obj, bool value) { obj.SetValue(EnableRightClickSelectProperty, value); }
        private static void OnEnableRightClickSelectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TreeViewItem item)
            {
                if ((bool)e.NewValue)
                {
                    item.PreviewMouseRightButtonDown +=
                        Item_PreviewMouseRightButtonDown;
                }
                else
                {
                    item.PreviewMouseRightButtonDown -=
                        Item_PreviewMouseRightButtonDown;
                }
            }
        }
        private static void Item_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TreeViewItem item)
            {
                if (!item.IsSelected)
                {
                    item.IsSelected = true;
                    item.Focus();
                    e.Handled = false;
                }
            }
        }
    }
}
