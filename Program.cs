using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

class Test
{
    [DllImport("user32.dll")]
    public static extern int GetAsyncKeyState(int i);

    [DllImport("user32.dll")]
    public static extern bool SetCursorPos(int X, int Y);
    [System.Runtime.InteropServices.DllImport("user32.dll")]
    public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

    public const int LMBDown = 0x02;
    public const int LMBUp = 0x04;


    private static NotifyIcon notifyIcon;
    private static Thread backgroundThread;

    [STAThread]
    static void Main()
    {
        // Initialisiere das NotifyIcon
        notifyIcon = new NotifyIcon();
        notifyIcon.Icon = SystemIcons.Application;
        notifyIcon.Visible = true;
        notifyIcon.Text = "F10 Insta Look";

        // Erstelle ein Kontextmenü für das NotifyIcon
        ContextMenu contextMenu = new ContextMenu();
        contextMenu.MenuItems.Add("Beenden", OnExit);

        // Setze das Kontextmenü für das NotifyIcon
        notifyIcon.ContextMenu = contextMenu;

        // Starte den Hintergrundthread
        backgroundThread = new Thread(BackgroundTask);
        backgroundThread.SetApartmentState(ApartmentState.STA);
        backgroundThread.IsBackground = true;
        backgroundThread.Start();

        // Starte die Anwendungsschleife
        Application.Run();
    }

    static void BackgroundTask()
    {
        while (true)
        {
            //Key F10
            if (IsKeyPressed(0x79))
            {
                //Position
                SetCursorPos(714, 843);
                mouse_event(LMBDown, 714, 843, 0, 0);
                mouse_event(LMBUp, 714, 843, 0, 0);

                //Wait 500ms
                Thread.Sleep(500);

                //Second Position
                SetCursorPos(960, 742);
                mouse_event(LMBDown, 960, 742, 0, 0);
                mouse_event(LMBUp, 960, 742, 0, 0);
            } //F10 

            Thread.Sleep(100);
        }
    }

    static bool IsKeyPressed(int keyCode)
    {
        return (GetAsyncKeyState(keyCode) & 0x8001) != 0;
    }

    static void OnExit(object sender, EventArgs e)
    {
        // Beende den Hintergrundthread und schließe die Anwendung
        backgroundThread.Abort();
        notifyIcon.Visible = false;
        Application.Exit();
    }
}
