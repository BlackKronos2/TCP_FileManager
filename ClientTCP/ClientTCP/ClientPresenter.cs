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
    public sealed class ClientPresenter
    {
        private List<string> _listFiles = new List<string>(0);

        private Label _label;
        private Button _button;
        private ListBox _listBox;

        public ClientPresenter(Label label, Button button, ListBox listBox)
        {
            _label = label;
            _button = button;
            _listBox = listBox;

            _label.Content = CommandMaster.GetInstance().Start(_button);
            try
            {
                _listFiles = CommandMaster.GetInstance().RequestDirectory("");
            }
            catch (Exception ex)
            {
                _label.Content = ex.Message;
            }
            foreach (string file in _listFiles)
                _listBox.Items.Add(file);
            _button.IsEnabled = false;
        }

        public void ClickButton()
        {
            _button.IsEnabled = false;
            try
            {
                //Получение файла
                if (_listFiles[_listBox.SelectedIndex].Any(x => x == '.'))
                {
                    CommandMaster.GetInstance().RequestFile(_listFiles[0] + "\\" + _listFiles[_listBox.SelectedIndex]);
                    return;
                }
                //Открытие файла
                _listFiles = CommandMaster.GetInstance().RequestDirectory(_listFiles[0] + "\\" + _listFiles[_listBox.SelectedIndex]);
            }
            catch (Exception ex)
            {
                _label.Content = ex.Message;
            }
            _listBox.Items.Clear();
            foreach (string file in _listFiles)
                _listBox.Items.Add(file);
        }

        public void ListSelect()
        {
            _button.IsEnabled = !(_listBox.SelectedIndex <= 0);
        }

        public void ButtonBackClick()
        {
            _button.IsEnabled = false;

            try
            {
                _listFiles = CommandMaster.GetInstance().RequestDirectory(CommandMaster.GetInstance().GetBackPath());
            }
            catch (Exception ex)
            {
                _label.Content = ex.Message;
            }
            _listBox.Items.Clear();
            foreach (string file in _listFiles)
                _listBox.Items.Add(file);
        }
    }
}
