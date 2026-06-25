using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SupplierManagement.Web.Models;

namespace SupplierManagement.Web.Services;

public class PdfService
{
    public byte[] GenerateOrderBill(OrderViewModel order, string userName)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(11));
                page.Header().Column(col =>
                {
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Text("SUPPLIER HUB").FontSize(22).Bold().FontColor("#4E342E");
                        row.ConstantItem(150).AlignRight().Text("ORDER INVOICE").FontSize(14).Bold().FontColor("#8D6E63");
                    });
                    col.Item().PaddingTop(4).LineHorizontal(1).LineColor("#D7CCC8");
                });
                page.Content().PaddingTop(20).Column(col =>
                {
                    col.Item().Background("#F5F0EB").Padding(16).Column(info =>
                    {
                        info.Item().Row(row =>
                        {
                            row.RelativeItem().Column(left =>
                            {
                                left.Item().Text($"Order ID: {order.OrderNumber}").Bold();
                                left.Item().Text($"Date: {order.OrderDate}");
                            });
                            row.RelativeItem().Column(right =>
                            {
                                right.Item().Text($"Customer: {userName}").Bold();
                                right.Item().Text($"Status: Confirmed").FontColor("#2E7D32");
                            });
                        });
                    });

                    col.Item().PaddingTop(20).Text("ITEMS").Bold().FontSize(13).FontColor("#4E342E");

                    col.Item().PaddingTop(8).Table(table =>
                    {
                        table.ColumnsDefinition(cols =>
                        {
                            cols.RelativeColumn(3);
                            cols.RelativeColumn(1);
                            cols.RelativeColumn(1);
                            cols.RelativeColumn(1);
                            cols.RelativeColumn(1);
                        });

                        static IContainer HeaderCell(IContainer c) =>
                            c.Background("#4E342E").Padding(8);

                        table.Header(header =>
                        {
                            header.Cell().Element(HeaderCell).Text("Product").Bold().FontColor("#FFFFFF");
                            header.Cell().Element(HeaderCell).Text("Price").Bold().FontColor("#FFFFFF");
                            header.Cell().Element(HeaderCell).Text("Discount").Bold().FontColor("#FFFFFF");
                            header.Cell().Element(HeaderCell).Text("Qty").Bold().FontColor("#FFFFFF");
                            header.Cell().Element(HeaderCell).Text("Total").Bold().FontColor("#FFFFFF");
                        });

                        bool alternate = false;
                        foreach (var item in order.OrderItems)
                        {
                            var bg = alternate ? "#FAF7F5" : "#FFFFFF";
                            alternate = !alternate;
                            static IContainer DataCell(IContainer c, string bg) =>
                                c.Background(bg).Padding(8);
                            table.Cell().Element(c => DataCell(c, bg)).Text(item.ProductName);
                            table.Cell().Element(c => DataCell(c, bg)).Text($"Rs.{item.Price:0.00}");
                            table.Cell().Element(c => DataCell(c, bg)).Text($"{item.Discount}%");
                            table.Cell().Element(c => DataCell(c, bg)).Text(item.Quantity.ToString());
                            table.Cell().Element(c => DataCell(c, bg)).Text($"Rs.{item.LineTotal:0.00}").Bold();
                        }
                    });

                    col.Item().PaddingTop(16).AlignRight().Column(total =>
                    {
                        total.Item().LineHorizontal(1).LineColor("#D7CCC8");
                        total.Item().PaddingTop(8).Row(row =>
                        {
                            row.RelativeItem().Text("GRAND TOTAL") .Bold().FontSize(13).FontColor("#4E342E");
                            row.ConstantItem(120).AlignRight().Text($"Rs.{order.TotalAmount:0.00}").Bold().FontSize(14).FontColor("#4E342E");
                        });
                    });

                    col.Item().PaddingTop(24).Background("#E8F5E9").Padding(12).Text("Thank you for shopping with Supplier Hub!").FontColor("#2E7D32").Italic();
                });
                page.Footer().AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Supplier Hub  |  Generated on ");
                        x.Span(DateTime.Now.ToString("dd-MMM-yyyy HH:mm")).FontColor("#8D6E63");
                    });
            });
        }).GeneratePdf();
    }
}