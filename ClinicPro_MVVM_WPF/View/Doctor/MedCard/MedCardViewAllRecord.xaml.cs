
using System.Windows.Controls;
using ClinicPro_MVVM_WPF.ViewModel.Doctor.MedCard;


namespace ClinicPro_MVVM_WPF.View.Doctor.MedCard
{
    
    public partial class MedCardViewAllRecord : UserControl
    {
        public MedCardViewAllRecord(MedCardVM parentVm, int medCardId)
        {
            InitializeComponent();
            DataContext = new MedCardAllRecordVM(parentVm, medCardId);
            Console.WriteLine(medCardId);
        }
    }
}
