using System.Windows;
using System.Windows.Controls;
using System;
using System.Linq;

namespace ClinicPro_MVVM_WPF.View.Patient.Home.CreateAppointment
{
    public partial class TimePicker : UserControl
    {
        // Регистрация свойства зависимости SelectedTime
        public static readonly DependencyProperty SelectedTimeProperty =
            DependencyProperty.Register("SelectedTime", typeof(TimeSpan), typeof(TimePicker),
                new FrameworkPropertyMetadata(TimeSpan.Zero, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedTimeChanged));

        public TimePicker()
        {
            InitializeComponent();
            PopulatePickers();
        }

        // Свойство SelectedTime, обертка над свойством зависимости
        public TimeSpan SelectedTime
        {
            get { return (TimeSpan)GetValue(SelectedTimeProperty); }
            set { SetValue(SelectedTimeProperty, value); }
        }

		// Callback-функция при изменении SelectedTime
		private static void OnSelectedTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is TimePicker timePicker)
			{
				timePicker.UpdateTimePickerControls((TimeSpan)e.NewValue);
			}
		}

		// Метод для обновления контролов выбора времени
		private void UpdateTimePickerControls(TimeSpan time)
		{
			HourPicker.SelectedItem = time.Hours;
			MinutePicker.SelectedItem = time.Minutes;
		}

        private void PopulatePickers()
        {
            // Добавление значений в ComboBox для часов и минут
            HourPicker.ItemsSource = Enumerable.Range(8, 11).ToList(); // 8-18 часа
            MinutePicker.ItemsSource = Enumerable.Range(0, 60).Where(x => x % 5 == 0).ToList(); // 0-59 минут с шагом 5

			// Подписка на события изменения выбора
			HourPicker.SelectionChanged += TimePicker_SelectionChanged;
			MinutePicker.SelectionChanged += TimePicker_SelectionChanged;
        }

		// Обработчик события изменения выбора в ComboBox
		private void TimePicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			// Обновление SelectedTime при изменении выбора в ComboBox
			SelectedTime = new TimeSpan(
				HourPicker.SelectedItem != null ? (int)HourPicker.SelectedItem : 0,
				MinutePicker.SelectedItem != null ? (int)MinutePicker.SelectedItem : 0,
			0);
		}
	}
}