using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoLiteForms.ViewModel;
using Xamarin.Forms;

namespace ToDoLiteForms.View
{
    public partial class DetailPage : ContentPage
    {
        public DetailPage()
        {
            InitializeComponent();
        }

        private void AddTaskCompleted(object sender, EventArgs args)
        {
            (BindingContext as DetailPageViewModel).AddNewTask();
        }
    }
}
