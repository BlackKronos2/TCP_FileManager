using System.Windows;
using System.Windows.Controls;

namespace ClientTCP
{
	public partial class MainWindow : Window
    {    
        public MainWindow(string ip)
        {
            InitializeComponent();
            DataContext = new ClientPresenter(ip, buttonGetListFiles);
        }

		private void OnClosed(object sender, System.EventArgs e)
		{
			if (!(DataContext is ClientPresenter viewModel))
				return;

			viewModel.ExitCommand.Execute(null);
		}

		private void ClientWindow_Loaded(object sender, RoutedEventArgs e)
		{
			if (!(DataContext is ClientPresenter viewModel))
				return;

			viewModel.LoadingCommand.Execute(null);
		}

		private void ListFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (!(DataContext is ClientPresenter viewModel))
				return;

			viewModel.SelectedIndex = ListFiles.SelectedIndex;
		}
	}
}
