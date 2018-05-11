using System;
using Xamarin.Forms;
using FormsControls.Base;

namespace Sample
{
    public partial class RootPage : MasterDetailPage
    {
        public RootPage()
        {
            InitializeComponent();
            masterPage.ListView.ItemSelected += OnListItemSelected;
        }

        private void OnListItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as SingleSample;
            if (item != null)
            {
                IsPresented = false;
                Detail = new AnimationNavigationPage(item.CreateAnimationPage()){BarTextColor=Color.White, BarBackgroundColor = Color.FromHex("#3F51B5")};
                masterPage.ListView.SelectedItem = null; 
            }
        }
    }
}