using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base;
using asp_dot_net_core_web_app_mvc_fast_food_system.Helpers;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.OrderProducts;
using ESC_POS_USB_NET.EpsonCommands;
using ESC_POS_USB_NET.Printer;
using ESCPOS_NET;
using ESCPOS_NET.Emitters;
using ESCPOS_NET.Utilities;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using asp_dot_net_core_web_app_mvc_fast_food_system.Helpers;
using Color = System.Drawing.Color;

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
            // Order Type
            Bitmap bmp = new Bitmap(300, 100); // width = paper width in pixel
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(System.Drawing.Color.White);
                g.DrawString(order.Type.ToString(), new Font("Arial", 24, FontStyle.Bold), Brushes.Black, 0, 0);
            }

            printer.AlignRight();
            printer.Image(bmp); // Weird padding at the left side, does not align properly with other text

            // Order Number
            Bitmap orderNumber = new Bitmap(300, 30);
            using (Graphics g = Graphics.FromImage(orderNumber))
            {
                g.Clear(System.Drawing.Color.White);
                g.DrawString($"Order - {order.Number}", new Font("Arial", 24, FontStyle.Regular), Brushes.Black, 0, 0);
            }
            printer.Image(orderNumber);
                //printer.Append($"Order - {order.Number}");
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

            printer.AlignCenter();
            

            #region Total Price
            printer.AlignCenter();
            printer.SetLineHeight(25);
            printer.Append($"Total: {order.TotalPrice.ToString("C", new CultureInfo("en-CA"))}");
            #endregion

            // Feed and cut
            printer.NewLines(5);
            printer.FullPaperCut();

            // Send to printer
            printer.PrintDocument();
        }

        // Prints all orders at a specific time, aggregating quantities of the same products
        public void PrintAllOrdersAt(HashSet<Order> orders, DateTime time)
        {
            Printer printer = new Printer("POS-80");

            int ordersCount = orders.Count;

            HashSet<OrderProduct> orderProducts = new HashSet<OrderProduct>() { };

            foreach (Order order in orders)
            {
                foreach (OrderProduct orderProduct in order.OrderProducts)
                {
                    OrderProduct existingOrderProduct = orderProducts
                        .FirstOrDefault(op => op.ProductId == orderProduct.ProductId);

                    if (existingOrderProduct != null)
                    {
                        existingOrderProduct.Quantity += orderProduct.Quantity;
                    }
                    else
                    {
                        OrderProduct newOrderProduct;

                        if (orderProduct is OrderBeverageProduct)
                        {
                            newOrderProduct = new OrderBeverageProduct()
                            {
                                ProductId = orderProduct.ProductId,
                                Product = orderProduct.Product,
                                Quantity = orderProduct.Quantity,
                                AdditionalPrice = orderProduct.AdditionalPrice,
                                Instructions = orderProduct.Instructions,
                                BeverageOption = (orderProduct as OrderBeverageProduct).BeverageOption
                            };
                        }
                        else if (orderProduct is OrderFoodProduct)
                        {
                            newOrderProduct = new OrderFoodProduct()
                            {
                                ProductId = orderProduct.ProductId,
                                Product = orderProduct.Product,
                                Quantity = orderProduct.Quantity,
                                AdditionalPrice = orderProduct.AdditionalPrice,
                                Instructions = orderProduct.Instructions,
                                FoodOption = (orderProduct as OrderFoodProduct).FoodOption
                            };
                        }
                        else
                        {
                            newOrderProduct = new OrderSauceProduct()
                            {
                                ProductId = orderProduct.ProductId,
                                Product = orderProduct.Product,
                                Quantity = orderProduct.Quantity,
                                AdditionalPrice = orderProduct.AdditionalPrice,
                                Instructions = orderProduct.Instructions,
                                SauceOption = (orderProduct as OrderSauceProduct).SauceOption
                            };
                        }

                        orderProducts.Add(newOrderProduct);
                    }
                }
            }

            Bitmap header = new Bitmap(300, 45); // width = paper width in pixels
            using (Graphics g = Graphics.FromImage(header))
            {
                g.Clear(System.Drawing.Color.White);
                //g.DrawString($"{ordersCount} Orders at {time.ToString("HH:mm")}", new Font("Arial", 26, FontStyle.Bold), Brushes.Black, 0, 0);
                g.DrawString($"({ordersCount}) {time.ToString("HH:mm")}", new Font("Arial", 26, FontStyle.Bold), Brushes.Black, 0, 0);
            }

            printer.Image(header);
            //printer.Append($"Orders at {time.ToString("HH:mm")}");
            printer.Separator();

            Bitmap productXquantity = new Bitmap(300, 40);
            Font fontProductXquantity = new Font("Arial", 25, FontStyle.Bold);
            Brush brush = Brushes.Black;

            foreach (OrderProduct orderProduct in orderProducts.OrderBy(op => op.Product.Code))
            {
                if (orderProduct is OrderBeverageProduct obp)
                {
                    using (Graphics g = Graphics.FromImage(productXquantity))
                    {
                        g.Clear(System.Drawing.Color.White);
                        g.DrawString($"{obp.BeverageOption}. x {obp.Quantity}", fontProductXquantity, brush, 0, 0);
                    }

                    printer.Image(productXquantity);
                    //printer.Append($"{obp.BeverageOption}. x {obp.Quantity}");
                }
                else if (orderProduct is OrderFoodProduct ofp)
                {
                    using (Graphics g = Graphics.FromImage(productXquantity))
                    {
                        g.Clear(System.Drawing.Color.White);
                        g.DrawString($"{ofp.Product.Code}{(ofp.Product.HasOptions ? ofp.FoodOption.ToString().Substring(0, 1) : null)}. x {ofp.Quantity}", fontProductXquantity, brush, 0, 0);
                    }

                    printer.Image(productXquantity);
                    //printer.Append($"{ofp.Product.Code}{(ofp.Product.HasOptions ? ofp.FoodOption : null)}. x {ofp.Quantity}");
                }
                else
                {
                    using (Graphics g = Graphics.FromImage(productXquantity))
                    {
                        g.Clear(System.Drawing.Color.White);
                        g.DrawString($"{SauceOptionExtensions.GetSauceAbbreviation((orderProduct as OrderSauceProduct).SauceOption)} x {orderProduct.Quantity}", fontProductXquantity, brush, 0, 0);
                    }

                    printer.Image(productXquantity);
                    //printer.Append($"{(orderProduct as OrderSauceProduct).SauceOption}. x {orderProduct.Quantity}");
                }
            }

            printer.NewLines(3);
            printer.FullPaperCut();
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

        private float DrawTextBlock
        (
            Graphics g,
            string text,
            Font font,
            Brush brush,
            float x,
            float y,
            float width,
            StringFormat format
        )
        {
            RectangleF rect = new RectangleF(x, y, width, 1000);

            SizeF size = g.MeasureString(text, font, (int)width);

            g.DrawString(text, font, brush, rect, format);

            return size.Height;
        }

        private void PrintReceiptPage(Graphics graphics, Order order)
        {
            float yPos = 0;
            float xPos = 0;
            float maxWidth = 200f;
            float width = maxWidth - 10;

            StringFormat format = new StringFormat
            {
                Trimming = StringTrimming.Word,
                FormatFlags = StringFormatFlags.LineLimit
            };

            // Fonts
            Font orderTypeFont = new Font("Arial", 20, FontStyle.Bold);
            Font orderCustomerAddressFont = new Font("Arial", 16, FontStyle.Regular);
            Font orderReadyTimeDateFont = new Font("Arial", 20, FontStyle.Regular);
            Font orderReadyTimeFont = new Font("Arial", 22, FontStyle.Bold);
            Font orderNumberFont = new Font("Arial", 18, FontStyle.Regular);
            Font orderCustomerPhoneNumberFont = new Font("Arial", 18, FontStyle.Regular);
            Font orderItemFont = new Font("Arial", 22, FontStyle.Regular);
            Font orderItemInstructionsFont = new Font("Arial", 18, FontStyle.Regular);
            Font orderPriceFont = new Font("Arial", 18, FontStyle.Bold);

            // Header
            // Order Type: Dine-In | Take-Out | Delivery
            yPos += DrawTextBlock(graphics, OrderTypeExtensions.GetName(order.Type), orderTypeFont, Brushes.Black, xPos, yPos, width, format);
            
            // (Customer Address)
            if (order.Type == Enums.OrderType.Delivery)
            {
                if (!string.IsNullOrEmpty(order.CustomerAddress))
                {
                    yPos += DrawTextBlock(graphics, $"({order.CustomerAddress})", orderCustomerAddressFont, Brushes.Black, xPos, yPos, width, format);
                }
            }

            yPos += 15;

            // Order nº: 1001
            yPos += DrawTextBlock(graphics, $"Order: {order.Number}", orderNumberFont, Brushes.Black, xPos, yPos, width, format);

            // Customer's phone number
            if (!string.IsNullOrEmpty(order.CustomerPhoneNumber))
            {
                yPos += DrawTextBlock(graphics, $"({order.CustomerPhoneNumber})", orderCustomerPhoneNumberFont, Brushes.Black, xPos, yPos, width, format);
            }

            // Order Ready Date: yyyy-MM-dd
            yPos += DrawTextBlock(graphics, order.ReadyTime.ToString("yyyy-MM-dd"), orderReadyTimeDateFont, Brushes.Black, xPos, yPos, width, format);

            // Order Ready Time: hh-mm
            yPos += DrawTextBlock(graphics, order.ReadyTime.ToString("t"), orderReadyTimeFont, Brushes.Black, xPos, yPos, width, format);

            yPos += 15;
            graphics.DrawLine(new Pen(Color.Black, 1), xPos, yPos, xPos + width, yPos);
            yPos += 15;

            // Items
            // Code x Quantity - Price
            foreach (OrderProduct orderProduct in order.OrderProducts.OrderBy(item =>
            {
                string code = item.Product.Code;
                int i = 0;

                while (i < code.Length && char.IsDigit(code[i])) i++;

                if (i > 0)
                {
                    int number = int.Parse(code.Substring(0, i));
                    string letter = code.Substring(i);
                    return (0, number, letter);
                }

                return (1, 0, code);
            }))
            {
                string itemText =
                    $"{orderProduct.Product.Code}. x {orderProduct.Quantity} - {orderProduct.TotalPrice.ToString("C", new CultureInfo("en-CA"))}";

                yPos += DrawTextBlock(graphics, itemText, orderItemFont, Brushes.Black, xPos, yPos, width, format);

                // Instructions
                if (!string.IsNullOrEmpty(orderProduct.Instructions))
                {
                    yPos += DrawTextBlock(
                        graphics,
                        $"({orderProduct.Instructions.ToUpper()})",
                        orderItemInstructionsFont,
                        Brushes.Black,
                        xPos,
                        yPos,
                        width,
                        format
                    );
                }
                
                // Separator
                using (Pen pen = new Pen(Color.Black, 1))
                {
                    yPos += 15;
                    graphics.DrawLine(pen, xPos, yPos, xPos + width, yPos);
                    yPos += 15;
                }
            }

            // Summary
            if (order.AdditionalCharge != null && order.AdditionalCharge != 0m)
            {
                // Food: $0.00
                yPos += DrawTextBlock(graphics, $"Food: {order.FoodTotalPrice.ToString("C", new CultureInfo("en-CA"))}", orderPriceFont, Brushes.Black, xPos, yPos, width, format);

                // Additional Charge: $0.00
                yPos += DrawTextBlock(graphics, $"Additional Charge: {order.AdditionalCharge.Value.ToString("C", new CultureInfo("en-CA"))}", orderPriceFont, Brushes.Black, xPos, yPos, width, format);
                using (Pen pen = new Pen(Color.Black, 1))
                {
                    yPos += 5;
                    graphics.DrawLine(pen, xPos, yPos, xPos + width, yPos);
                    yPos += 5;
                }
            }

            // Subtotal:  $0.00
            yPos += DrawTextBlock(graphics, $"Subtotal: {order.SubTotalPrice.ToString("C", new CultureInfo("en-CA"))}", orderPriceFont, Brushes.Black, xPos, yPos, width, format);

            // Gst: $0.00
            yPos += DrawTextBlock(graphics, $"GST: {order.Gst.ToString("C", new CultureInfo("en-CA"))}", orderPriceFont, Brushes.Black, xPos, yPos, width, format);

            // Pst: $0.00
            yPos += DrawTextBlock(graphics, $"PST: {order.Pst.ToString("C", new CultureInfo("en-CA"))}", orderPriceFont, Brushes.Black, xPos, yPos, width, format);

            // Delivery: $0.00
            if (order.Type == Enums.OrderType.Delivery)
            {
                yPos += DrawTextBlock(graphics, $"Delivery: {order.DeliveryFee.Value.ToString("C", new CultureInfo("en-CA"))}", orderPriceFont, Brushes.Black, xPos, yPos, width, format);
            }

            // Total: $0.00
            yPos += DrawTextBlock(graphics, $"Total: {order.TotalPrice.ToString("C", new CultureInfo("en-CA"))}", orderPriceFont, Brushes.Black, xPos, yPos, width, format);
        }
    }
}
