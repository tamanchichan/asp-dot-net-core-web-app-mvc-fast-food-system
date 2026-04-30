using asp_dot_net_core_web_app_mvc_fast_food_system.Helpers;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.OrderProducts;
using ESC_POS_USB_NET.Printer;
using ESCPOS_NET;
using ESCPOS_NET.Emitters;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using Color = System.Drawing.Color;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.POS
{
    public class ThermalPrinterService
    {
        private readonly string _printername = "POS-80C";

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
        public void PrintReceiptUSBAlt(Order order)
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
        public void PrintReceiptUSB(Order order)
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrinterSettings.PrinterName = _printername;
            printDocument.DefaultPageSettings.PaperSize = printDocument.PrinterSettings.DefaultPageSettings.PaperSize;
            printDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0); // Adjust margins as needed
            printDocument.PrintPage += (sender, e) => PrintReceiptPage(e.Graphics, order);
            printDocument.Print();
        }

        public void PrintReceiptKitchenUSB(Order order)
        {

            PrintDocument printDocument = new PrintDocument();
            printDocument.PrinterSettings.PrinterName = _printername;
            printDocument.DefaultPageSettings.PaperSize = printDocument.PrinterSettings.DefaultPageSettings.PaperSize;
            printDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0); // Adjust margins as needed
            printDocument.PrintPage += (sender, e) => PrintReceiptPageKitchen(e.Graphics, order);
            printDocument.Print();
        }

        private float DrawTextBlock(Graphics graphics, string text, Font font, float x, float y, float width, StringFormat format)
        {
            Brush brush = Brushes.Black;
            SizeF size = graphics.MeasureString(text, font, (int)width);
            RectangleF rect = new RectangleF(x, y, width, 1000);
            
            graphics.DrawString(text, font, brush, rect, format);
            
            return size.Height;
        }

        private void PrintReceiptPage(Graphics graphics, Order order)
        {
            float yPos = 0;
            float xPos = 0;
            float widthPos = graphics.VisibleClipBounds.Width - 5;
            float halfWidthPos = widthPos / 2;

            StringFormat format = new StringFormat
            {
                Trimming = StringTrimming.Word,
                FormatFlags = StringFormatFlags.LineLimit
            };

            StringFormat rightAlign = new StringFormat
            {
                Alignment = StringAlignment.Far
            };

            #region Fonts
            // Fonts
            // Dine-In | Take-Out | Delivery
            Font orderTypeFont = new Font("Arial", 16, FontStyle.Bold);

            // (Customer Address)
            Font orderCustomerAddressFont = new Font("Arial", 16, FontStyle.Regular);

            // yyyy-MM-dd
            Font orderReadyTimeDateFont = new Font("Arial", 14, FontStyle.Regular);

            // hh-mm
            Font orderReadyTimeFont = new Font("Arial", 16, FontStyle.Bold);

            // Order nº: 1001
            Font orderNumberFont = new Font("Arial", 16, FontStyle.Regular);

            // (123-456-7890)
            Font orderCustomerPhoneNumberFont = new Font("Arial", 16, FontStyle.Regular);

            // Code. x Quantity - Product
            Font orderProductFont = new Font("Arial", 16, FontStyle.Regular);

            // $0.00
            Font orderProductTotalPriceFont = new Font("Arial", 16, FontStyle.Bold);

            // (INSTRUCTIONS)
            Font orderProductInstructionsFont = new Font("Arial", 14, FontStyle.Regular);

            // *OBSERVATIONS*
            Font orderObservationsFont = new Font("Arial", 16, FontStyle.Bold);
            
            // $0.00
            Font orderPriceFont = new Font("Arial", 16, FontStyle.Bold);
            #endregion

            #region POS Header
            // Order Type: Dine-In | Take-Out | Delivery
            string orderTypeText = OrderTypeExtensions.GetName(order.Type);
            yPos += DrawTextBlock(graphics, orderTypeText, orderTypeFont, xPos, yPos, widthPos, format);
            
            // Customer's Address: (123 Main St)
            if (order.Type == Enums.OrderType.Delivery)
            {
                if (!string.IsNullOrEmpty(order.CustomerAddress))
                {
                    string customerAddressText = $"({order.CustomerAddress})";
                    yPos += DrawTextBlock(graphics, customerAddressText, orderCustomerAddressFont, xPos, yPos, widthPos, format);
                }
            }
            
            yPos += 5;

            #region Space-Between: (Date and Time)
            // Date: (yyyy-MM-dd)
            string dateText = order.ReadyTime.ToString("yyyy-MM-dd");
            DrawTextBlock(graphics, dateText, orderReadyTimeDateFont, xPos, yPos, halfWidthPos, format);

            // Time: (hh-mm) // right-aligned
            string timeText = order.ReadyTime.ToString("t");
            DrawTextBlock(graphics, timeText, orderReadyTimeFont, xPos, yPos, widthPos, rightAlign);

            // Move down based on taller text (date or time)
            yPos += Math.Max(orderReadyTimeDateFont.GetHeight(graphics), orderReadyTimeFont.GetHeight(graphics)) + 5;
            #endregion

            // Order nº: 1001
            string orderNumberText = $"Order nº: {order.Number}";
            yPos += DrawTextBlock(graphics, orderNumberText, orderNumberFont, xPos, yPos, widthPos, format);

            // Customer's phone number: (123-456-7890)
            if (!string.IsNullOrEmpty(order.CustomerPhoneNumber))
            {
                string customerPhoneNumberText = $"({order.CustomerPhoneNumber})";
                yPos += DrawTextBlock(graphics, customerPhoneNumberText, orderCustomerPhoneNumberFont, xPos, yPos, widthPos, format);
            }
            #endregion

            // Separator
            yPos += 15;
            graphics.DrawLine(new Pen(Color.Black, 1), xPos, yPos, xPos + widthPos, yPos);
            yPos += 5;

            #region Items
            // Code. x Quantity - Product - Price
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
                string productCode = "";
                string optionCode = "";
                string foodOption = "";
                string productNameText = orderProduct.Product.Name;
                string productQuantity = orderProduct.Quantity.ToString();

                if (orderProduct is OrderFoodProduct ofp)
                {
                    productCode = ofp.Product.Code;
                    if (ofp.FoodOption.HasValue)
                    {
                        optionCode = ofp.FoodOption.ToString().Substring(0, 1);
                        productCode += $"{optionCode}";

                        foodOption = ofp.FoodOption.Value.ToString();
                        productNameText = $"{productNameText} {foodOption}";
                    }
                }

                //yPos += DrawTextBlock(graphics, itemText, orderItemFont, Brushes.Black, xPos, yPos, width, format);

                float codeWidth = 100;                 // left column
                float nameWidth = widthPos - codeWidth;  // remaining space

                #region Space-Between: Code and Product Name
                // Code x Quantity
                string codeAndQuantityText = $"{productCode}. x {productQuantity}";
                DrawTextBlock(graphics, codeAndQuantityText, orderProductFont, xPos, yPos, codeWidth, format);

                // Product name
                float productNameHeight = DrawTextBlock(graphics, productNameText, orderProductFont, xPos + codeWidth, yPos, nameWidth, format);

                // Move down based on taller text (productNameHeight or orderItemFont)
                yPos += Math.Max(productNameHeight, orderProductFont.GetHeight(graphics));
                #endregion

                #region Space-Between: Instructions and Price
                // Instructions
                string orderProductInstructionsText = "";
                //float orderProductInstructionsWidth = 150;

                float orderProductInstructionsHeight = 0;

                if (!string.IsNullOrEmpty(orderProduct.Instructions))
                {
                    orderProductInstructionsText = $"({orderProduct.Instructions.ToUpper()})";
                    orderProductInstructionsHeight = DrawTextBlock(graphics, orderProductInstructionsText, orderProductInstructionsFont, xPos, yPos, halfWidthPos, format);
                }

                //float orderProductTotalPriceWidth = widthPos - orderProductInstructionsWidth;

                // Product's total price: $0.00 (right-aligned)
                string orderItemTotalPrice = orderProduct.TotalPrice.ToString("C", new CultureInfo("en-CA"));
                DrawTextBlock(graphics, orderItemTotalPrice, orderProductTotalPriceFont, xPos, yPos, widthPos, rightAlign);

                // Move down based on taller text (instructions or price)
                yPos += Math.Max(orderProductInstructionsHeight, orderProductTotalPriceFont.GetHeight(graphics));
                #endregion

                // Separator
                using (Pen pen = new Pen(Color.Black, 1))
                {
                    yPos += 5;
                    graphics.DrawLine(pen, xPos, yPos, xPos + widthPos, yPos);
                    yPos += 5;
                }
            }
            #endregion

            #region Order Summary
            // Observations:
            if (!string.IsNullOrEmpty(order.Observations))
            {
                string orderObservationsText = $"*{order.Observations.ToUpper()}*";
                yPos += DrawTextBlock(graphics, orderObservationsText, orderObservationsFont, xPos, yPos, widthPos, format);

                using (Pen pen = new Pen(Color.Black, 1))
                {
                    yPos += 5;
                    graphics.DrawLine(pen, xPos, yPos, xPos + widthPos, yPos);
                    yPos += 5;
                }
            }

            if (order.AdditionalCharge != null && order.AdditionalCharge != 0m)
            {
                // Food: $0.00
                string orderFoodTotalPriceText = $"Food: {order.FoodTotalPrice.ToString("C", new CultureInfo("en-CA"))}";
                yPos += DrawTextBlock(graphics, orderFoodTotalPriceText, orderPriceFont, xPos, yPos, widthPos, format);

                // Additional Charge: $0.00
                string orderAdditionalChargeText = $"Additional Charge: {order.AdditionalCharge.Value.ToString("C", new CultureInfo("en-CA"))}";
                yPos += DrawTextBlock(graphics, orderAdditionalChargeText, orderPriceFont, xPos, yPos, widthPos, format);

                using (Pen pen = new Pen(Color.Black, 1))
                {
                    yPos += 5;
                    graphics.DrawLine(pen, xPos, yPos, xPos + widthPos, yPos);
                    yPos += 5;
                }
            }

            // Items: x
            string orderItemsText = $"Items: {order.Quantity}";
            yPos += DrawTextBlock(graphics, orderItemsText, orderProductFont, xPos, yPos, widthPos, format);

            // Subtotal: $0.00
            string orderSubtotalText = $"Subtotal: {order.SubTotalPrice.ToString("C", new CultureInfo("en-CA"))}";
            yPos += DrawTextBlock(graphics, orderSubtotalText, orderPriceFont, xPos, yPos, widthPos, rightAlign);

            // Gst: $0.00
            string orderGstText = $"GST: {order.Gst.ToString("C", new CultureInfo("en-CA"))}";
            yPos += DrawTextBlock(graphics, orderGstText, orderPriceFont, xPos, yPos, widthPos, rightAlign);

            // Pst: $0.00
            string orderPstText = $"PST: {order.Pst.ToString("C", new CultureInfo("en-CA"))}";
            yPos += DrawTextBlock(graphics, orderPstText, orderPriceFont, xPos, yPos, widthPos, rightAlign);

            // Delivery: $0.00
            if (order.Type == Enums.OrderType.Delivery)
            {
                string orderDeliveryText = $"Delivery: {order.DeliveryFee.Value.ToString("C", new CultureInfo("en-CA"))}";
                yPos += DrawTextBlock(graphics, orderDeliveryText, orderPriceFont, xPos, yPos, widthPos, rightAlign);
            }

            // Total: $0.00
            string orderTotalText = $"Total: {order.TotalPrice.ToString("C", new CultureInfo("en-CA"))}";
            yPos += DrawTextBlock(graphics, orderTotalText, orderPriceFont, xPos, yPos, widthPos, rightAlign);
            #endregion
        }

        private void PrintReceiptPageKitchen(Graphics graphics, Order order)
        {
            float xPos = 0;
            float yPos = 0;
            float widthPos = graphics.VisibleClipBounds.Width - 5;
            float halfWidthPos = widthPos / 2;

            StringFormat format = new StringFormat
            {
                Trimming = StringTrimming.Word,
                FormatFlags = StringFormatFlags.LineLimit
            };

            StringFormat rightAlign = new StringFormat
            {
                Alignment = StringAlignment.Far
            };

            #region Fonts
            // Dine-In | Take-Out | Delivery
            Font orderTypeFont = new Font("Arial", 18, FontStyle.Bold);

            // Order nº: 1001
            Font orderNumberFont = new Font("Arial", 14, FontStyle.Regular);

            // yyyy-MM-dd
            Font orderReadyTimeDateFont = new Font("Arial", 14, FontStyle.Regular);

            // hh-mm
            Font orderReadyTimeFont = new Font("Arial", 20, FontStyle.Bold);

            // Code. x Quantity
            Font orderProductFont = new Font("Arial", 22, FontStyle.Regular);

            // $0.00
            Font orderProductAdditionalPriceFont = new Font("Arial", 16, FontStyle.Bold);

            // (INSTRUCTIONS)
            Font orderProductInstructionsFont = new Font("Arial", 18, FontStyle.Regular);

            // *OBSERVATIONS*
            Font orderObservationsFont = new Font("Arial", 18, FontStyle.Bold);

            // $0.00
            Font orderPriceFont = new Font("Arial", 18, FontStyle.Bold);
            #endregion

            #region POS Header
            yPos += 100;

            // Order nº: 1001
            string orderNumberText = $"Order nº: {order.Number}";
            DrawTextBlock(graphics, orderNumberText, orderNumberFont, xPos, yPos, halfWidthPos, format);

            // yyyy-MM-dd
            string orderReadyTimeDateText = order.ReadyTime.ToString("yyyy-MM-dd");
            DrawTextBlock(graphics, orderReadyTimeDateText, orderReadyTimeDateFont, xPos, yPos, widthPos, rightAlign);

            // Move down once (based on tallest text)
            yPos += Math.Max(orderNumberFont.GetHeight(graphics), orderReadyTimeDateFont.GetHeight(graphics));
            #endregion

            // Separator
            yPos += 5;
            graphics.DrawLine(new Pen(Color.Black, 1), xPos, yPos, xPos + widthPos, yPos);
            yPos += 5;

            #region Items
            // Code. x Quantity
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
                string productCode = "";
                string optionCode = "";
                string foodOption = "";
                string productNameText = orderProduct.Product.Name;
                string productQuantity = orderProduct.Quantity.ToString();

                if (orderProduct is OrderFoodProduct ofp && ofp.FoodOption.HasValue)
                {
                    productCode = ofp.Product.Code;
                    optionCode = ofp.FoodOption.ToString().Substring(0, 1);
                }

                string codeAndQuantityText =
                    $"{orderProduct.Product.Code}{optionCode}.";

                if (orderProduct.Quantity > 1)
                {
                    codeAndQuantityText = $"{orderProduct.Product.Code}{optionCode}. x {orderProduct.Quantity}";
                }

                #region Space-Between: Code and Additional Price
                // Code. x Quantity
                float codeAndQuantityHeight = DrawTextBlock(graphics, codeAndQuantityText, orderProductFont, xPos, yPos, halfWidthPos + 50, format);

                // Additional Price: $0.00
                if (orderProduct.AdditionalPrice != null && orderProduct.AdditionalPrice != 0m)
                {
                    string additionalPriceText = orderProduct.AdditionalPrice.ToString("C", new CultureInfo("en-CA"));
                    DrawTextBlock(graphics, additionalPriceText, orderProductAdditionalPriceFont, xPos, yPos, widthPos, rightAlign);
                }

                yPos += Math.Max(codeAndQuantityHeight, orderProductAdditionalPriceFont.GetHeight(graphics));
                #endregion

                // Instructions
                if (!string.IsNullOrEmpty(orderProduct.Instructions))
                {
                    string orderProductInstructionsText = $"({orderProduct.Instructions.ToUpper()})";
                    yPos += DrawTextBlock(graphics,orderProductInstructionsText,orderProductInstructionsFont,xPos,yPos,widthPos,format);
                }

                // Separator
                using (Pen pen = new Pen(Color.Black, 1))
                {
                    yPos += 5;
                    graphics.DrawLine(pen, xPos, yPos, xPos + widthPos, yPos);
                    yPos += 5;
                }
            }
            #endregion

            #region Order Summary
            if (order.AdditionalCharge != null && order.AdditionalCharge != 0m)
            {
                // Food: $0.00
                string orderFoodTotalPriceText = $"Food: {order.FoodTotalPrice.ToString("C", new CultureInfo("en-CA"))}";
                yPos += DrawTextBlock(graphics, orderFoodTotalPriceText, orderPriceFont, xPos, yPos, widthPos, format);

                // Additional Charge: $0.00
                string orderAdditionalChargeText = $"Additional Charge: {order.AdditionalCharge.Value.ToString("C", new CultureInfo("en-CA"))}";
                yPos += DrawTextBlock(graphics, orderAdditionalChargeText, orderPriceFont, xPos, yPos, widthPos, format);

                using (Pen pen = new Pen(Color.Black, 1))
                {
                    yPos += 5;
                    graphics.DrawLine(pen, xPos, yPos, xPos + widthPos, yPos);
                    yPos += 5;
                }
            }

            // Observations:
            if (!string.IsNullOrEmpty(order.Observations))
            {
                string orderObservationsText = $"*{order.Observations.ToUpper()}*";
                yPos += DrawTextBlock(graphics, orderObservationsText, orderObservationsFont, xPos, yPos, widthPos, format);
            }

            // Order Type: Dine-In | Take-Out | Delivery
            string orderTypeText = $"{OrderTypeExtensions.GetName(order.Type)} ({OrderTypeExtensions.GetChineseName(order.Type)})";
            yPos += DrawTextBlock(graphics, orderTypeText, orderTypeFont, xPos, yPos, widthPos, rightAlign);

            // Order Ready Time: hh-mm
            string orderReadyTimeText = order.ReadyTime.ToString("t");
            yPos += DrawTextBlock(graphics, orderReadyTimeText, orderReadyTimeFont, xPos, yPos, widthPos, rightAlign);

            // Total: $0.00
            string orderTotalPriceText = order.TotalPrice.ToString("C", new CultureInfo("en-CA"));
            yPos += DrawTextBlock(graphics, orderTotalPriceText, orderPriceFont, xPos, yPos, widthPos, rightAlign);
            graphics.Dispose();
            #endregion
        }
    }
}
