using System.Windows.Controls;
using ClinicPro_MVVM_WPF.ViewModel.Patient.MedCard;

namespace ClinicPro_MVVM_WPF.View.Patient.MedCard;

public partial class MedCardPatient : UserControl
{
    public MedCardPatient(MedCardParentVM parentVm, int patientId)
    {
        InitializeComponent();
        DataContext = new MedCardPatientVM(parentVm, patientId);
    }
}