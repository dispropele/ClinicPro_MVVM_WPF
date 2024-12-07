using System.Windows.Controls;
using ClinicPro_MVVM_WPF.ViewModel.Patient.MedCard;

namespace ClinicPro_MVVM_WPF.View.Patient.MedCard;

public partial class MedCardViewAllRecord : UserControl
{
    public MedCardViewAllRecord(MedCardParentVM parentVm, int patientId)
    {
        InitializeComponent();
        DataContext = new MedCardViewAllRecordVM(parentVm, patientId);
    }
}