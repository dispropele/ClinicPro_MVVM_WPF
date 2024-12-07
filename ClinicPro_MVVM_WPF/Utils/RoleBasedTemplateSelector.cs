using System.IO;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace ClinicPro_MVVM_WPF.Utils;

public class RoleBasedTemplateSelector : DataTemplateSelector
{
    public DataTemplate DoctorTemplate { get; set; }
    public DataTemplate PatientTemplate { get; set; }
    public DataTemplate AdminTemplate { get; set; }

    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        var role = item as string;

        return role switch
        {
            "doctor" => DoctorTemplate,
            "patient" => PatientTemplate,
            "admin" => AdminTemplate,
            _ => base.SelectTemplate(item, container)
        };
    }
    
}
