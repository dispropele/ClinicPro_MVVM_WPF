using System.Windows.Controls;
using ClinicPro_MVVM_WPF.ViewModel.Patient.MedCard;

namespace ClinicPro_MVVM_WPF.View.Patient.MedCard;

public partial class MedCardParent : UserControl
{
    public MedCardParent(int patientId)
    {
        InitializeComponent();
        DataContext = new MedCardParentVM(patientId);
    }
}