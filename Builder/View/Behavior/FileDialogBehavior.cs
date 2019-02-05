namespace MQChatter.View.Behavior
{
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Interactivity;
    using Button = System.Windows.Controls.Button;

    public class FileDialogBehavior : Behavior<Button>
    {
        public string SetterName { get; set; }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Click += OnClick;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Click -= OnClick;
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            bool? result = dialog.ShowDialog();
            if (result == true && dialog.CheckPathExists && AssociatedObject.DataContext != null)
            {
                PropertyInfo propertyInfo = AssociatedObject.DataContext.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(p => p.CanRead && p.CanWrite)
                    .First(p => p.Name.Equals(SetterName));

                propertyInfo.SetValue(AssociatedObject.DataContext, dialog.FileName, null);
            }
        }
    }
}