using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using maintask;
using System.Threading.Tasks;
using parser;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Navigation;
using System.Diagnostics;
using LoadingSpinnerControl;


namespace frfrccrawler
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    /// 
    public class ResultModel : INotifyPropertyChanged
    {
        public ObservableCollection<siteinfo> SearchResults { get; set; } = new ObservableCollection<siteinfo>();

        // If you have other properties you might still need this for them:
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public partial class Window1 : Window
    {
        
        public Window1()
        {
            InitializeComponent();
            Window1 window1 = this;
            window1.DataContext = new ResultModel();

        }

        private void topMarginTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = (ComboBox)sender;
            var selectedItem = comboBox.SelectedItem;
            MessageBox.Show($"Selected: {selectedItem}");
        }
        private async void on_search(object sender, RoutedEventArgs e)
        {
            //loadingGrid.Visibility = Visibility.Visible;
            spinner.IsLoading = true;
            await on_search_async(sender,e);// while this is awaited, show loading ui
            //loadingGrid.Visibility = Visibility.Collapsed;
            spinner.IsLoading = false;
        }
        private async Task on_search_async(object sender, RoutedEventArgs e)
        {
            string query = topMarginTextBox.Text;
            string selectedText = preference.SelectedItem.ToString();
            string pref = "";
            Console.WriteLine(selectedText);
            if (selectedText == null )
                {
                
                Console.WriteLine("tttt");
                }
            if (selectedText == "System.Windows.Controls.ComboBoxItem: By Time")
            {
                pref = "date";
            }
            List<siteinfo> results = new List<siteinfo>();
            try
            {
                results = await crawlstart.start(query, 10, pref);
            }
            catch(misspellexception s){

                MessageBox.Show("It seems like there was a typo. Please check and try again.", "Typo Detected", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            var viewModel = (ResultModel)this.DataContext;

            // Clear previous results and add new ones
            viewModel.SearchResults.Clear();
            foreach (var result in results)
            {
                viewModel.SearchResults.Add(result);
            }
        }
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            // for .NET Core you need to add UseShellExecute = true
            // see https://learn.microsoft.com/dotnet/api/system.diagnostics.processstartinfo.useshellexecute#property-value
            Console.WriteLine(e.Uri.AbsoluteUri);
            if (e.Uri.AbsoluteUri == null)
            {
                Console.WriteLine("null");
            }
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }


    }
}
