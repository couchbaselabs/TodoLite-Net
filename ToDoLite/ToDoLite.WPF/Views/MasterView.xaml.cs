using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MvvmCross.Wpf.Views;
using ToDoLite.Core.ViewModels;

namespace ToDoLite.WPF.Views
{
    /// <summary>
    /// Interaction logic for MasterView.xaml
    /// </summary>
    public partial class MasterView : MvxWpfView
    {
        private NewListView _popup;
        public MasterView()
        {
            InitializeComponent();
            DataContextChanged += (sender, args) => {
                var oldVal = (args.OldValue as MasterViewModel);
                if(oldVal != null) {
                    oldVal.NewTaskRequested -= OnNewTaskRequested;
                }

                var newVal = (args.NewValue as MasterViewModel);
                if(newVal != null) {
                    newVal.NewTaskRequested += OnNewTaskRequested;
                }
            };
        }

        private void OnNewTaskRequested(object sender, EventArgs args)
        {
            var viewModel = new NewListViewModel((DataContext as MasterViewModel));
            _popup = new NewListView(viewModel);
            viewModel.Finished += OnPopupFinished;
            _popup.ShowDialog();
        }

        private void OnPopupFinished(object sender, bool okPressed)
        {
            _popup.Close();
        }
    }
}
