using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusSaver.Maui.Messages;

public class PermissionRequestCompletedMessage : ValueChangedMessage<string>
{
    public PermissionRequestCompletedMessage(string value) : base(value)
    {
    }
}
