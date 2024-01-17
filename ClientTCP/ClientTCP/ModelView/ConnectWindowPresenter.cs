using System;
using System.Linq;
using System.Windows;

namespace ClientTCP
{
	public class ConnectWindowPresenter : ModelViewBase
	{
		private string _ipAddress;

		public string IpAddress
		{
			get { return _ipAddress; }
			set
			{ 
				_ipAddress = value;
				OnPropertyChanged();
			}
		}

		public ViewModelCommand ConnectCommand { get; }
		public ViewModelCommand CloseWindowCommand { get; }

		public ConnectWindowPresenter()
		{
			ConnectCommand = new ViewModelCommand(Connect);
			IpAddress = "127.0.0.1";
		}

		private void Connect(object obj)
		{
			try
			{
				CommandMaster.GetInstance().Start(IpAddress);
				var window = Application.Current.Windows.OfType<ConnectWindow>().FirstOrDefault();

				MainWindow mainWindow = new MainWindow(IpAddress);
				mainWindow.Show();
				if (window != null)
					window.Close();
			}
			catch (Exception ex) 
			{
				MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}
