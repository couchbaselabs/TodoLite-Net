using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoLiteForms.ViewModel;
using Xamarin.Forms;

namespace ToDoLiteForms.View
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage(LoginPageViewModel viewModel)
        {
            InitializeComponent();

            viewModel.PageChangeRequested += (sender, args) => Navigation.PushAsync(args);
            BindingContext = viewModel;
        }
    }
}
