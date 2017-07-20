using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FunnyLoaderDemo
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ContentPage
	{
		private bool isRunning;
		private bool isRandom;

		public MainPage()
		{
			InitializeComponent();

			BindingContext = this;

			IsRunning = true;
		}

		public bool IsRunning
		{
			get { return isRunning; }
			set
			{
				isRunning = value;
				OnPropertyChanged();
			}
		}

		public bool IsRandom
		{
			get { return isRandom; }
			set
			{
				isRandom = value;
				OnPropertyChanged();
			}
		}
	}
}
