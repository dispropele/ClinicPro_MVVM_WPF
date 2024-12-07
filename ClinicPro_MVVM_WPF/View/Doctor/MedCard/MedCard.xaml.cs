using System.Windows.Controls;
using ClinicPro_MVVM_WPF.ViewModel.Doctor.MedCard;

namespace ClinicPro_MVVM_WPF.View.Doctor.MedCard;

public partial class MedCard : UserControl
{   
    
    public MedCard(int doctorId)
    {
        InitializeComponent();
        DataContext = new MedCardVM(doctorId);
    }
}