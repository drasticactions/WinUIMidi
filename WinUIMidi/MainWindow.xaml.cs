// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUIMidi
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint midiOutGetNumDevs();

        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int midiOutGetDevCaps(uint uDeviceID, ref MIDIOUTCAPS pmoc, uint cbmoc);

        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int midiOutOpen(out IntPtr phmo, uint uDeviceID, IntPtr dwCallback, IntPtr dwInstance, uint fdwOpen);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct MIDIOUTCAPS
        {
            public short wMid;
            public short wPid;
            public int vDriverVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string szPname;
            public short wTechnology;
            public short wVoices;
            public short wNotes;
            public short wChannelMask;
            public uint dwSupport;
        }

        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void myButton_Click(object sender, RoutedEventArgs e)
        {
            var midiOutCaps = new MIDIOUTCAPS();

            for (uint i = 0; i < midiOutGetNumDevs(); i++)
            {
                midiOutGetDevCaps(i, ref midiOutCaps, (uint)Marshal.SizeOf(midiOutCaps));

                if (midiOutCaps.szPname == "Microsoft GS Wavetable Synth")
                {
                    var hMidiOut = IntPtr.Zero;

                    Console.WriteLine("Opening the device...");
                    var result = midiOutOpen(out hMidiOut, 0, IntPtr.Zero, IntPtr.Zero, 0);
                    if (result != 0)
                        throw new Exception($"midiOutOpen failed with {result}.");

                    Console.WriteLine("Opened.");
                    break;
                }
            }
        }
    }
}
