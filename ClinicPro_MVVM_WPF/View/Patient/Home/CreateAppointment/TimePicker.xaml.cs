using System.Windows;
using System.Windows.Controls;
using System;
using System.Linq;

namespace ClinicPro_MVVM_WPF.View.Patient.Home.CreateAppointment
{
    public partial class TimePicker : UserControl
    {
        // ����������� �������� ����������� SelectedTime
        public static readonly DependencyProperty SelectedTimeProperty =
            DependencyProperty.Register("SelectedTime", typeof(TimeSpan), typeof(TimePicker),
                new FrameworkPropertyMetadata(TimeSpan.Zero, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedTimeChanged));

        public TimePicker()
        {
            InitializeComponent();
            PopulatePickers();
        }

        // �������� SelectedTime, ������� ��� ��������� �����������
        public TimeSpan SelectedTime
        {
            get { return (TimeSpan)GetValue(SelectedTimeProperty); }
            set { SetValue(SelectedTimeProperty, value); }
        }

		// Callback-������� ��� ��������� SelectedTime
		private static void OnSelectedTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is TimePicker timePicker)
			{
				timePicker.UpdateTimePickerControls((TimeSpan)e.NewValue);
			}
		}

		// ����� ��� ���������� ��������� ������ �������
		private void UpdateTimePickerControls(TimeSpan time)
		{
			HourPicker.SelectedItem = time.Hours;
			MinutePicker.SelectedItem = time.Minutes;
		}

        private void PopulatePickers()
        {
            // ���������� �������� � ComboBox ��� ����� � �����
            HourPicker.ItemsSource = Enumerable.Range(8, 11).ToList(); // 8-18 ����
            MinutePicker.ItemsSource = Enumerable.Range(0, 60).Where(x => x % 5 == 0).ToList(); // 0-59 ����� � ����� 5

			// �������� �� ������� ��������� ������
			HourPicker.SelectionChanged += TimePicker_SelectionChanged;
			MinutePicker.SelectionChanged += TimePicker_SelectionChanged;
        }

		// ���������� ������� ��������� ������ � ComboBox
		private void TimePicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			// ���������� SelectedTime ��� ��������� ������ � ComboBox
			SelectedTime = new TimeSpan(
				HourPicker.SelectedItem != null ? (int)HourPicker.SelectedItem : 0,
				MinutePicker.SelectedItem != null ? (int)MinutePicker.SelectedItem : 0,
			0);
		}
	}
}