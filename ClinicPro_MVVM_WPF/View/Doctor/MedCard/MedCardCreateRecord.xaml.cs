using System.Windows.Controls;
using ClinicPro_MVVM_WPF.ViewModel.Doctor.MedCard;

namespace ClinicPro_MVVM_WPF.View.Doctor.MedCard
{
    
    public partial class MedCardCreateRecord : UserControl
    {
        public MedCardCreateRecord(MedCardVM parentVm, int doctorId)
        {
            InitializeComponent();
            DataContext = new MedCardCreateRecordVM(parentVm, doctorId);
        }
    }
}
