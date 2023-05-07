using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using static System.Net.WebRequestMethods;

namespace ClientTCP
{
    public partial class MainWindow : Window
    {
        private ClientPresenter _presenter;   
        
        public MainWindow()
        {
            InitializeComponent();
            _presenter = new ClientPresenter(LabelLink, buttonGetListFiles, ListFiles);
        }

        private void buttonGetListFiles_Click(object sender, RoutedEventArgs e) => _presenter.ClickButton();

        private void ListFiles_SelectionChanged(object sender, SelectionChangedEventArgs e) => _presenter.ListSelect();

        private void buttonBack_Click(object sender, RoutedEventArgs e) => _presenter.ButtonBackClick();
    }
}
