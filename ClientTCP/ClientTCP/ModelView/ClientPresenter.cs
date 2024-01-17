using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ClientTCP
{
	public sealed class ClientPresenter : ModelViewBase
    {
        private string _filePath;
        private ObservableCollection<string> _items;
        private int _selectedIndex;

        private string _ipAddress;

        private Button _button;

		public string FilePath
        {
            get { return _filePath; }
            set { 
                _filePath = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> Items
        {
            get { return _items; }
            set 
            {
                _items = value;
                OnPropertyChanged();
            }
        }
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged();
                ListSelect();
			}
        }

        public ViewModelCommand FileConnectCommand { get; }
        public ViewModelCommand BackCommand { get; }

        public ViewModelCommand ExitCommand { get; }
        public ViewModelCommand LoadingCommand { get; }


		public ClientPresenter(string ip, Button button)
        {
            _ipAddress = ip;
            _button = button;

            FileConnectCommand = new ViewModelCommand(ClickButton);
            BackCommand = new ViewModelCommand(ButtonBackClick);
            ExitCommand = new ViewModelCommand(ClientExit);
            LoadingCommand = new ViewModelCommand(OnLoad);
        }

        private void OnLoad(object obj)
        {
			try
			{
				var fileList = CommandMaster.GetInstance().RequestDirectory("");
				Items = new ObservableCollection<string>(fileList);
                FilePath = CommandMaster.GetInstance().UserPath;
			}
			catch (Exception ex)
			{
				FilePath = ex.Message;
				Items = new ObservableCollection<string>(new List<string>(0));
				_button.IsEnabled = false;
			}
		}

        private void ClickButton(object obj)
        {
            try
            {
                //Получение файла
                if (Items[SelectedIndex].Any(x => x == '.'))
                {
                    CommandMaster.GetInstance().RequestFile(Items[0] + "\\" + Items[SelectedIndex]);
                    MessageBox.Show($"Файл {Items[SelectedIndex]} скачан", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
				//Открытие файла
				var fileList = CommandMaster.GetInstance().RequestDirectory(Items[0] + "\\" + Items[SelectedIndex]);
                Items = new ObservableCollection<string>(fileList);
				FilePath = CommandMaster.GetInstance().UserPath;
			}
            catch (Exception ex)
            {
                FilePath = ex.Message;
				Items = new ObservableCollection<string>(new List<string>(0));
			}
        }

        private void ListSelect()
        {
            _button.IsEnabled = !(SelectedIndex <= 0);
        }

        private void ButtonBackClick(object obj)
        {
            _button.IsEnabled = false;

            try
            {
				var fileList = CommandMaster.GetInstance().RequestDirectory(CommandMaster.GetInstance().GetBackPath());
				Items = new ObservableCollection<string>(fileList);
				FilePath = CommandMaster.GetInstance().UserPath;
			}
            catch (Exception ex)
            {
                FilePath = ex.Message;
                Items = new ObservableCollection<string>(new List<string>(0));
            }
        }

        private void ClientExit(object obj)
        {
			var window = Application.Current.Windows.OfType<ConnectWindow>().FirstOrDefault();

            ConnectWindow connectWindow = new ConnectWindow();
            connectWindow.Show();

			if (window != null)
				window.Close();
		}
    }
}
