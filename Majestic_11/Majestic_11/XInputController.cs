using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Use NuGet to install SharpDX and SharpDX.XInput
using SharpDX.XInput;

namespace Majestic_11
{
    public class XInputController
    {
        protected Gamepad pad;
        protected Controller controller;
        protected bool connected = false;
        protected Frm_MJOY_Main mainForm;

        public XInputController(Frm_MJOY_Main frm)
        {
            controller = new Controller(UserIndex.One);
            connected = controller.IsConnected;
            Log.Line("Controller connect: " + connected);

            mainForm = frm;
            if (connected)
            {
                string txt = "Controller specs:\n";
                string q = controller.GetBatteryInformation(BatteryDeviceType.Gamepad).BatteryType.ToString();
                if (q == "Disconnected")
                    q = "Wired or Unknown";
                txt += "Battery type: " + q+"\n";
                txt += "Battery level: " + controller.GetBatteryInformation(BatteryDeviceType.Gamepad).BatteryLevel+"\n";
                mainForm.setLbl_connected(txt);
            }else{
                mainForm.setLbl_connected("-- no controller connected or controller not found. --");
            }
        }
    }
}
