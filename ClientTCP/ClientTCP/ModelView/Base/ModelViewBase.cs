using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ClientTCP
{
	public class ModelViewBase : INotifyPropertyChanged
	{
		/// <summary> Релизация интерфейса INotifyPropertyChanged </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary> Релизация интерфейса INotifyPropertyChanged </summary>
		public void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}

	}
}
