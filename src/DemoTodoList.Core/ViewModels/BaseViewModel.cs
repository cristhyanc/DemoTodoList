using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.ViewModels;

namespace DemoTodoList.Core.ViewModels
{
    public abstract class BaseViewModel : MvxViewModel
    {
        private bool _isBusy;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }
    }
}
