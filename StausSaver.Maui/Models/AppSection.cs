using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StausSaver.Maui.Models;

public class AppSection
{
    public string Title { get; set; }
    public string Route { get; set; }
    public string Icon { get; set; }
    public string IconDark { get; set; }
    public Type TargetType { get; set; }
}
