using System.Windows;
using OfferManagerDemo.ViewModels;

namespace OfferManagerDemo.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(OffersViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
            Loaded += async (_, __) => await vm.LoadAsync();
        }
    }
}
