using System.Windows.Controls;
using ClinicPro_MVVM_WPF.ViewModel.Doctor.MedCard;

namespace ClinicPro_MVVM_WPF.View.Doctor.MedCard
{
   
    public partial class MedCardViewRecord : UserControl
    {
        public MedCardViewRecord(MedCardAllRecordVM allRecordVm, MedCardVM parentVm, int recordId)
        {
            InitializeComponent();
            DataContext = new MedCardRecordVM(parentVm, allRecordVm, recordId);
        }
    }
}
