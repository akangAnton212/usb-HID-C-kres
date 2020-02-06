using System;
using System.Collections.Generic;
using System.Text;
using LibUsbDotNet;
using LibUsbDotNet.Info;
using LibUsbDotNet.Main;

namespace usb_HID
{
    class Program
    {

        public static readonly int VendorID = 0x13BA;
        public static readonly int ProductID = 0x0018;

        public static void Main(string[] args)
        {
            UsbDevice usbDevice = null;

            UsbRegDeviceList allDevices = UsbDevice.AllDevices;

            Console.WriteLine("Found {0} devices", allDevices.Count);

            foreach (UsbRegistry usbRegistry in allDevices)
            {
                Console.WriteLine("Got device: {0}\r\n", usbRegistry.FullName);

                if (usbRegistry.Open(out usbDevice))
                {
                    Console.WriteLine("Device Information\r\n------------------");

                    Console.WriteLine("{0}", usbDevice.Info.ToString());

                    Console.WriteLine("VID & PID: {0} {1}", usbDevice.Info.Descriptor.VendorID, usbDevice.Info.Descriptor.ProductID);

                    Console.WriteLine("\r\nDevice configuration\r\n--------------------");
                    foreach (UsbConfigInfo usbConfigInfo in usbDevice.Configs)
                    {
                        Console.WriteLine("{0}", usbConfigInfo.ToString());

                        Console.WriteLine("\r\nDevice interface list\r\n---------------------");
                        IReadOnlyCollection<UsbInterfaceInfo> interfaceList = usbConfigInfo.InterfaceInfoList;
                        foreach (UsbInterfaceInfo usbInterfaceInfo in interfaceList)
                        {
                            Console.WriteLine("{0}", usbInterfaceInfo.ToString());

                            Console.WriteLine("\r\nDevice endpoint list\r\n--------------------");
                            IReadOnlyCollection<UsbEndpointInfo> endpointList = usbInterfaceInfo.EndpointInfoList;
                            foreach (UsbEndpointInfo usbEndpointInfo in endpointList)
                            {
                                Console.WriteLine("{0}", usbEndpointInfo.ToString());
                            }
                        }
                    }
                    usbDevice.Close();
                }
                Console.WriteLine("\r\n----- Device information finished -----\r\n");
            }



            Console.WriteLine("Trying to find our device: {0} {1}", VendorID, ProductID);
            UsbDeviceFinder usbDeviceFinder = new UsbDeviceFinder(VendorID, ProductID);

            // This does not work !!! WHY ?
            usbDevice = UsbDevice.OpenUsbDevice(usbDeviceFinder);

            if (usbDevice != null)
            {
                Console.WriteLine("OK");
            }
            else
            {
                Console.WriteLine("FAIL");
            }

            UsbDevice.Exit();

            Console.Write("Press anything to close");
            Console.ReadKey();
        }



    //public static UsbDevice MyUsbDevice;

    ////region SET YOUR USB Vendor and Product ID!

    //public static UsbDeviceFinder MyUsbFinder = new UsbDeviceFinder(0x13BA, 0x0018);

    ////endregion

    //static void Main(string[] args)
    //{
    //    ErrorCode ec = ErrorCode.None;

    //    try
    //    {
    //        // Find and open the usb device.
    //        MyUsbDevice = UsbDevice.OpenUsbDevice(MyUsbFinder);

    //        // If the device is open and ready
    //        if (MyUsbDevice == null) throw new Exception("Device Not Found.");

    //        // If this is a "whole" usb device (libusb-win32, linux libusb-1.0)
    //        // it exposes an IUsbDevice interface. If not (WinUSB) the 
    //        // 'wholeUsbDevice' variable will be null indicating this is 
    //        // an interface of a device; it does not require or support 
    //        // configuration and interface selection.
    //        IUsbDevice wholeUsbDevice = MyUsbDevice as IUsbDevice;
    //        if (!ReferenceEquals(wholeUsbDevice, null))
    //        {
    //            // This is a "whole" USB device. Before it can be used, 
    //            // the desired configuration and interface must be selected.

    //            // Select config #1
    //            wholeUsbDevice.SetConfiguration(1);

    //            // Claim interface #0.
    //            wholeUsbDevice.ClaimInterface(0);
    //        }

    //        // open read endpoint 1.
    //        UsbEndpointReader reader = MyUsbDevice.OpenEndpointReader(ReadEndpointID.Ep01);


    //        byte[] readBuffer = new byte[1024];
    //        while (ec == ErrorCode.None)
    //        {
    //            int bytesRead;

    //            // If the device hasn't sent data in the last 5 seconds,
    //            // a timeout error (ec = IoTimedOut) will occur. 
    //            ec = reader.Read(readBuffer, 5000, out bytesRead);

    //            if (bytesRead == 0) throw new Exception(string.Format("{0}:No more bytes!", ec));
    //            Console.WriteLine("{0} bytes read", bytesRead);

    //            // Write that output to the console.
    //            Console.Write(Encoding.Default.GetString(readBuffer, 0, bytesRead));
    //        }

    //        Console.WriteLine("\r\nDone!\r\n");
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine();
    //        Console.WriteLine((ec != ErrorCode.None ? ec + ":" : String.Empty) + ex.Message);
    //    }
    //    finally
    //    {
    //        if (MyUsbDevice != null)
    //        {
    //            if (MyUsbDevice.IsOpen)
    //            {
    //                // If this is a "whole" usb device (libusb-win32, linux libusb-1.0)
    //                // it exposes an IUsbDevice interface. If not (WinUSB) the 
    //                // 'wholeUsbDevice' variable will be null indicating this is 
    //                // an interface of a device; it does not require or support 
    //                // configuration and interface selection.
    //                IUsbDevice wholeUsbDevice = MyUsbDevice as IUsbDevice;
    //                if (!ReferenceEquals(wholeUsbDevice, null))
    //                {
    //                    // Release interface #0.
    //                    wholeUsbDevice.ReleaseInterface(0);
    //                }

    //                MyUsbDevice.Close();
    //            }
    //            MyUsbDevice = null;

    //            // Free usb resources
    //            UsbDevice.Exit();

    //        }

    //        // Wait for user input..
    //        Console.ReadKey();
    //    }
    //}
}
}
