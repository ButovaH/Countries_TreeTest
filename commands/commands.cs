using System;
using System.Windows.Input;

namespace WpfApp1
{
    /// <summary>
	/// Команда для вызова функции с типизированным параметром
	/// </summary>
	public class Command<T> : ICommand
    {
        protected Action<T> do_execute;
        protected bool can_execute;

        public bool CanExecuting
        {
            get { return can_execute; }
            set
            {
                if (can_execute != value)
                {
                    can_execute = value;
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public Command(Action<T> callback, bool executing = true)
        {
            do_execute = callback;
            can_execute = executing;
        }

        public virtual bool CanExecute(object parameter)
            => parameter is bool ? CanExecuting = (bool)parameter : can_execute;

        public event EventHandler CanExecuteChanged;

        public virtual void Execute(object args = null)
            => do_execute(args != null ? (T)Convert.ChangeType(args, typeof(T)) : default(T));

    }

    /// <summary>
    /// Команда для вызова функции с любым параметром
    /// </summary>
    public class Command : Command<object>
    {

        public Command(Action callback, bool executing = true)
            : base(x => callback(), executing)
        {
        }

        public Command(Action<object> callback, bool executing = true)
            : base(callback, executing)
        {
        }

        public override void Execute(object args = null)
            => do_execute(args);

    }

}
