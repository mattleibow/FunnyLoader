using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FunnyLoader
{
	public class FunnyLoaderLabel : Label
	{
		public static readonly BindableProperty PrefixProperty =
			BindableProperty.Create(
				nameof(Prefix),
				typeof(string),
				typeof(FunnyLoaderLabel),
				"");

		public static readonly BindableProperty SuffixProperty =
			BindableProperty.Create(
				nameof(Suffix),
				typeof(string),
				typeof(FunnyLoaderLabel),
				"...");

		public static readonly BindableProperty DurationProperty =
			BindableProperty.Create(
				nameof(Duration),
				typeof(int),
				typeof(FunnyLoaderLabel),
				2000);

		public static readonly BindableProperty MessagesProperty =
			BindableProperty.Create(
				nameof(Messages),
				typeof(IList<string>),
				typeof(FunnyLoaderLabel),
				(IList<string>)null);

		public static readonly BindableProperty IsRandomProperty =
			BindableProperty.Create(
				nameof(IsRandom),
				typeof(bool),
				typeof(FunnyLoaderLabel),
				false);

		public static readonly BindableProperty IsRunningProperty =
			BindableProperty.Create(
				nameof(IsRunning),
				typeof(bool),
				typeof(FunnyLoaderLabel),
				false,
				propertyChanged: OnIsRunningChanged);

		public static string[] DefaultMessages { get; }

		static FunnyLoaderLabel()
		{
			DefaultMessages = FunnyMessages.Messages.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
		}

		// lazy load so we can get some sort of semi-random
		private readonly Lazy<Random> randomizer = new Lazy<Random>(() => new Random());
		private int position = -1;

		public FunnyLoaderLabel()
		{
		}

		public string Prefix
		{
			get { return (string)GetValue(PrefixProperty); }
			set { SetValue(PrefixProperty, value); }
		}

		public string Suffix
		{
			get { return (string)GetValue(SuffixProperty); }
			set { SetValue(SuffixProperty, value); }
		}

		public int Duration
		{
			get { return (int)GetValue(DurationProperty); }
			set { SetValue(DurationProperty, value); }
		}

		public IList<string> Messages
		{
			get { return (IList<string>)GetValue(MessagesProperty); }
			set { SetValue(MessagesProperty, value); }
		}

		public bool IsRandom
		{
			get { return (bool)GetValue(IsRandomProperty); }
			set { SetValue(IsRandomProperty, value); }
		}

		public bool IsRunning
		{
			get { return (bool)GetValue(IsRunningProperty); }
			set { SetValue(IsRunningProperty, value); }
		}

		private static void OnIsRunningChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var label = bindable as FunnyLoaderLabel;
			if (label == null)
				return;

			if (true.Equals(newValue))
				label.StartAnimation();
			else
				label.StopAnimation();
		}

		private async void StartAnimation()
		{
			while (IsRunning)
			{
				ChangeText();

				if (IsRunning)
					await this.FadeTo(1.0, 500, Easing.SinOut);

				if (IsRunning)
					await Task.Delay(Duration);

				if (IsRunning)
					await this.FadeTo(0.0, 500, Easing.SinIn);
			}
		}

		private void StopAnimation()
		{
		}

		private void ChangeText()
		{
			var messages = GetMessages();

			if (messages.Count == 0)
				return;

			if (IsRandom)
			{
				// just pick something
				position = randomizer.Value.Next(messages.Count);
			}
			else
			{
				// start or step
				if (position < 0)
					position = 0;
				else
					position++;
				// wrap
				if (position >= messages.Count)
					position = 0;
			}

			Text = string.Concat(Prefix, messages[position], Suffix);
		}

		private IList<string> GetMessages() => Messages ?? DefaultMessages;
	}
}
