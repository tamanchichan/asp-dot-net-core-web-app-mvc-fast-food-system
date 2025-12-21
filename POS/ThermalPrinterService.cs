using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base;
using ESC_POS_USB_NET.EpsonCommands;
using ESC_POS_USB_NET.Printer;
using ESCPOS_NET;
using ESCPOS_NET.Emitters;
using ESCPOS_NET.Utilities;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.POS
{
    public class ThermalPrinterService
    {
        private readonly string _printername = "POS-80";

        // Needs to configure virtual COM port for USB printer first
        public void PrintReceiptSerial(Order order)
        {
            SerialPrinter printer = new SerialPrinter(portName: "COM4", baudRate: 9600);

            EPSON e = new EPSON();

            printer.Write(
                e.CenterAlign(),
                e.PrintLine("STORE NAME"),
                e.PrintLine($"Order: {order.Number.ToString()}"),
                e.PrintLine($"Total Price: {order.TotalPrice.ToString("C", new CultureInfo("en-CA"))}"),
                e.FeedLines(3),
                e.FullCut()
            );
        }

        // https://github.com/mtmsuhail/ESC-POS-USB-NET
        public void PrintReceiptUSB(Order order)
        {
            Printer printer = new Printer("POS-80");

            #region Header
            Bitmap bmp = new Bitmap(300, 100); // width = paper width in pixels

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(System.Drawing.Color.White);
                g.DrawString(order.Type.ToString(), new Font("Arial", 24, FontStyle.Bold), Brushes.Black, 0, 0);
            }

            printer.AlignCenter();
            printer.Image(bmp); // Weird padding at the left side, does not align properly with other text
            printer.Append($"Order - {order.Number}");
            #endregion

            #region Items
            printer.AlignLeft();
            foreach (OrderProduct orderProduct in order.OrderProducts)
            {
                string leftText = $"{orderProduct.Product.Code}. x {orderProduct.Quantity}";
                string rightText = orderProduct.TotalPrice.ToString("C", new CultureInfo("en-CA"));
                string line = $"{leftText,-20}{rightText,10}"; // Adjust spacing as needed to achieve space between
                printer.Append(line);
            }
            #endregion

            #region Total Price
            printer.AlignRight();
            printer.SetLineHeight(25);
            printer.Append($"Total: {order.TotalPrice.ToString("C", new CultureInfo("en-CA"))}");
            #endregion

            // Feed and cut
            printer.NewLines(3);
            printer.FullPaperCut();

            // Send to printer
            printer.PrintDocument();
        }
        
        /*
         * Uses PrintDocument to print
         * Advantages:
         *  - Easy to setup
         *  - "Easier" to control font-size of a text
         * Disadvantages:
         *  - Too much blank space at the bottom after printed content
         *  - Needs to control 'x' and 'y' position where content will be printed
        */
        public void PrintReceiptUSBAlt(Order order)
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrinterSettings.PrinterName = _printername;
            printDocument.PrintPage += (sender, e) => PrintReceiptPage(e.Graphics, order);
            printDocument.Print();
        }

        private void PrintReceiptPage(Graphics graphics, Order order)
        {
            float yPos = 0;
            int padding = 5;
            Font orderFont = new Font("Arial", 35, FontStyle.Bold);
            Font font = new Font("Arial", 20, FontStyle.Regular);
            Font totalPriceFont = new Font("Arial", 30, FontStyle.Bold);

            graphics.DrawString($"{order.Type.ToString()}", font, Brushes.Black, 0, yPos);
            yPos += font.GetHeight();

            graphics.DrawString($"Order: {order.Number.ToString()}", orderFont, Brushes.Black, 0, yPos);
            yPos += orderFont.GetHeight();

            foreach (OrderProduct orderProduct in order.OrderProducts)
            {
                graphics.DrawString($"{orderProduct.Product.Code} x {orderProduct.Quantity} - {orderProduct.TotalPrice.ToString("C", new CultureInfo("en-CA"))}", font, Brushes.Black, 0, yPos);
                yPos += font.GetHeight();
            }

            graphics.DrawString($"Total Price: {order.TotalPrice.ToString("C", new CultureInfo("en-CA"))}", font, Brushes.Black, 0, yPos);
        }
    }
}
