using System.Windows.Input;
using ClinicPro_MVVM_WPF.Model;
using ClinicPro_MVVM_WPF.Utils;
using ClinicPro_MVVM_WPF.View.Patient.MedCard;
using ClinicPro_MVVM_WPF.ViewModel.Doctor.MedCard;

namespace ClinicPro_MVVM_WPF.ViewModel.Patient.MedCard;

public class MedCardViewRecordVM : BaseViewModel
{
    private MedCardParentVM _parentVm;
    
    public ICommand BackToCommand { get; set; }

    public MedCardViewRecordVM(MedCardParentVM parentVm, MedRecordModel record)
    {
        _parentVm = parentVm;
        Record = record;

        BackToCommand = new RelayCommand(BackTo);
    }
    
    private void BackTo(object obj) => _parentVm.CurrentView = new MedCardViewAllRecord(_parentVm, Record.MedCard.Patient.patientId);
    
    private MedRecordModel _record;
    public MedRecordModel Record
    {
        get => _record;
        set
        {
            _record = value;
            OnPropertyChanged();
        }
    }
    
}