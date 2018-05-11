using Xamarin.Forms;

namespace Sample
{
    public partial class SideMenuPage : ContentPage
    {
        public SideMenuPage()
        {
            InitializeComponent();
            BindingContext = new SamplesViewModel();
        }

        public ListView ListView { get { return listView; } }
    }
}