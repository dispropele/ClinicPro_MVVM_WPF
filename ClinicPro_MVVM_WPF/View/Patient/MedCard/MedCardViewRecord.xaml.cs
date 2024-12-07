using System.Windows.Controls;
using ClinicPro_MVVM_WPF.Model;
using ClinicPro_MVVM_WPF.ViewModel.Patient.MedCard;

namespace ClinicPro_MVVM_WPF.View.Patient.MedCard;

public partial class MedCardViewRecord : UserControl
{
    public MedCardViewRecord(MedCardParentVM parentVm, MedRecordModel record)
    {
        InitializeComponent();
        DataContext = new MedCardViewRecordVM(parentVm, record);
    }
}