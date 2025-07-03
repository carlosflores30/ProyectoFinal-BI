using ClosedXML.Excel;
using SmartStockAI.Application.DTOs.Reports;
using SmartStockAI.Infrastructure.Reports.Interfaces;

namespace SmartStockAI.Infrastructure.Reports.Services;

public class ReporteExcelService : IReporteExcelService
{
    public byte[] GenerarReporteMovimientos(List<MovimientoReportDto> datos)
    {
        using var workbook = new XLWorkbook();
        var sheet = workbook.Worksheets.Add("Movimientos");

        // Título centrado en B1-F1
        var titulo = sheet.Range("B1:K1");
        titulo.Merge();
        titulo.Value = "REPORTE DE MOVIMIENTOS DE INVENTARIO";
        titulo.Style.Font.SetBold();
        titulo.Style.Font.FontSize = 14;
        titulo.Style.Font.FontColor = XLColor.Black;
        titulo.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        sheet.Row(1).Height = 22;

        // Fecha en D2-E2
        sheet.Cell("D2").Value = "Fecha:";
        sheet.Cell("D2").Style.Font.SetBold();
        sheet.Cell("D2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
        var horaPeru = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time"));
        sheet.Cell("E2").Value = horaPeru.ToString("yyyy-MM-dd HH:mm:ss");
        sheet.Cell("E2").Style.Font.FontColor = XLColor.DarkGray;

        // Encabezados en B4-F4
        var headers = new[] { "Producto", "Tipo", "Cantidad", "Fecha", "Observación" };
        for (int i = 0; i < headers.Length; i++)
        {
            var cell = sheet.Cell(4, i + 2); 
            cell.Value = headers[i];
            cell.Style.Font.SetBold();
            cell.Style.Fill.SetBackgroundColor(XLColor.LightGray);
            cell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        }

        // Datos desde fila 5
        for (int i = 0; i < datos.Count; i++)
        {
            var row = i + 5;
            var item = datos[i];

            sheet.Cell(row, 2).Value = item.NombreProducto;
            sheet.Cell(row, 3).Value = item.TipoMovimiento;
            sheet.Cell(row, 4).Value = item.Cantidad;
            sheet.Cell(row, 5).Value = item.Fecha.ToString("yyyy-MM-dd");
            sheet.Cell(row, 6).Value = item.Observacion;

            for (int col = 2; col <= 6; col++)
            {
                var cell = sheet.Cell(row, col);
                cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                cell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            }
        }

        sheet.ColumnsUsed().AdjustToContents();
        sheet.RowsUsed().AdjustToContents();
        foreach (var row in sheet.RowsUsed())
        {
            row.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
    
    public byte[] GenerarReporteProductos(List<ProductoReportDto> productos)
{
    using var workbook = new XLWorkbook();
    var sheet = workbook.Worksheets.Add("Productos");
    
    var titulo = sheet.Range("B1:K1");
    titulo.Merge();
    titulo.Value = "REPORTE DE PRODUCTOS";
    titulo.Style.Font.SetBold();
    titulo.Style.Font.FontSize = 14;
    titulo.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    sheet.Row(1).Height = 22;

    // Fecha en J2-K2 (ajustado)
    sheet.Cell("J2").Value = "Fecha:";
    sheet.Cell("J2").Style.Font.SetBold();
    var horaPeru = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time"));
    sheet.Cell("K2").Value = horaPeru.ToString("yyyy-MM-dd HH:mm:ss");
    sheet.Cell("K2").Style.Font.FontColor = XLColor.DarkGray;

    // Encabezados en B4-K4
    var headers = new[]
    {
        "Código", "Nombre", "Descripción", "Stock", "Umbral",
        "Precio Compra", "Precio Venta", "Precio Descuento", "Categoría", "Fecha Ingreso"
    };

    for (int i = 0; i < headers.Length; i++)
    {
        var cell = sheet.Cell(4, i + 2); 
        cell.Value = headers[i];
        cell.Style.Font.SetBold();
        cell.Style.Fill.SetBackgroundColor(XLColor.LightGray);
        cell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
    }
    
    for (int i = 0; i < productos.Count; i++)
    {
        var row = i + 5;
        var p = productos[i];

        sheet.Cell(row, 2).Value = p.CodProducto;
        sheet.Cell(row, 3).Value = p.Nombre;
        sheet.Cell(row, 4).Value = p.Descripcion;
        sheet.Cell(row, 5).Value = p.Stock;
        sheet.Cell(row, 6).Value = p.Umbral;
        
        sheet.Cell(row, 7).Value = p.PrecioCompra;
        sheet.Cell(row, 7).Style.NumberFormat.Format = "\"S/\" #,##0.00";

        sheet.Cell(row, 8).Value = p.PrecioVenta;
        sheet.Cell(row, 8).Style.NumberFormat.Format = "\"S/\" #,##0.00";

        sheet.Cell(row, 9).Value = p.PrecioDescuento;
        sheet.Cell(row, 9).Style.NumberFormat.Format = "\"S/\" #,##0.00";

        sheet.Cell(row, 10).Value = p.Categoria;
        sheet.Cell(row, 11).Value = p.FechaIngreso.ToString("yyyy-MM-dd");

        for (int col = 2; col <= 11; col++)
        {
            var cell = sheet.Cell(row, col);
            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            cell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
        }
    }

    sheet.ColumnsUsed().AdjustToContents();
    sheet.RowsUsed().AdjustToContents();

    foreach (var row in sheet.RowsUsed())
        row.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    using var stream = new MemoryStream();
    workbook.SaveAs(stream);
    return stream.ToArray();
}
    
    public byte[] GenerarReporteClientes(List<ClienteReportDto> clientes)
{
    using var workbook = new XLWorkbook();
    var sheet = workbook.Worksheets.Add("Clientes");

    var titulo = sheet.Range("B1:K1");
    titulo.Value = "REPORTE DE CLIENTES";
    titulo.Style.Font.SetBold();
    titulo.Style.Font.FontSize = 14;
    titulo.Style.Font.FontColor = XLColor.Black;
    titulo.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    sheet.Row(1).Height = 22;

    sheet.Cell("F2").Value = "Fecha:";
    sheet.Cell("F2").Style.Font.SetBold();
    var horaPeru = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time"));
    sheet.Cell("G2").Value = horaPeru.ToString("yyyy-MM-dd HH:mm:ss");
    sheet.Cell("G2").Style.Font.FontColor = XLColor.DarkGray;

    var headers = new[] { "Nombre", "Documento", "Teléfono", "Correo", "Dirección" };

    for (int i = 0; i < headers.Length; i++)
    {
        var cell = sheet.Cell(4, i + 2); 
        cell.Value = headers[i];
        cell.Style.Font.SetBold();
        cell.Style.Fill.SetBackgroundColor(XLColor.LightGray);
        cell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
    }

    for (int i = 0; i < clientes.Count; i++)
    {
        var row = i + 5;
        var c = clientes[i];
        sheet.Cell(row, 2).Value = c.Nombre;
        sheet.Cell(row, 3).Value = c.Documento;
        sheet.Cell(row, 4).Value = c.Telefono;
        sheet.Cell(row, 5).Value = c.Correo;
        sheet.Cell(row, 6).Value = c.Direccion;

        for (int col = 2; col <= 6; col++)
        {
            var cell = sheet.Cell(row, col);
            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            cell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
        }
    }

    sheet.ColumnsUsed().AdjustToContents();
    sheet.RowsUsed().AdjustToContents();

    foreach (var row in sheet.RowsUsed())
        row.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    using var stream = new MemoryStream();
    workbook.SaveAs(stream);
    return stream.ToArray();
}
    
    public byte[] GenerarReporteVentas(List<VentaReportDto> ventas)
{
    using var workbook = new XLWorkbook();
    var sheet = workbook.Worksheets.Add("Ventas");

    var titleRange = sheet.Range("B1:E1").Merge();
    titleRange.Value = "REPORTE DE VENTAS";
    titleRange.Style.Font.SetBold();
    titleRange.Style.Font.FontSize = 14;
    titleRange.Style.Font.FontColor = XLColor.Black;
    titleRange.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    sheet.Row(1).Height = 22;

    sheet.Cell("D2").Value = "Fecha:";
    sheet.Cell("D2").Style.Font.SetBold();
    var horaPeru = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time"));
    sheet.Cell("E2").Value = horaPeru.ToString("yyyy-MM-dd HH:mm:ss");
    sheet.Cell("E2").Style.Font.FontColor = XLColor.Gray;

    var headers = new[] { "Código", "Cliente", "Total", "Fecha", "Método de Pago" };
    for (int i = 0; i < headers.Length; i++)
    {
        var cell = sheet.Cell(4, i + 2);
        cell.Value = headers[i];
        cell.Style.Font.SetBold();
        cell.Style.Fill.SetBackgroundColor(XLColor.LightGray);
        cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        cell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    }

    for (int i = 0; i < ventas.Count; i++)
    {
        var row = i + 5;
        var v = ventas[i];

        sheet.Cell(row, 2).Value = v.Codigo;
        sheet.Cell(row, 3).Value = v.Cliente;
        sheet.Cell(row, 4).Value = v.Total;
        sheet.Cell(row, 4).Style.NumberFormat.Format = "\"S/\" #,##0.00";
        sheet.Cell(row, 5).Value = v.FechaVenta.ToString("yyyy-MM-dd");
        sheet.Cell(row, 6).Value = v.MetodoPago;
        for (int col = 2; col <= 6; col++)
        {
            sheet.Cell(row, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            sheet.Cell(row, col).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
        }
    }
    sheet.ColumnsUsed().AdjustToContents();
    sheet.RowsUsed().AdjustToContents();
    using var stream = new MemoryStream();
    workbook.SaveAs(stream);
    return stream.ToArray();
}
    
    public byte[] GenerarReporteDetalleVentas(List<DetalleVentaReportDto> detalles)
{
    using var workbook = new XLWorkbook();
    var sheet = workbook.Worksheets.Add("DetalleVentas");

    var titleRange = sheet.Range("B1:H1").Merge();
    titleRange.Value = "REPORTE DE DETALLES DE VENTA";
    titleRange.Style.Font.SetBold();
    titleRange.Style.Font.FontSize = 14;
    titleRange.Style.Font.FontColor = XLColor.Black;
    titleRange.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    sheet.Row(1).Height = 22;

    sheet.Cell("G2").Value = "Fecha:";
    sheet.Cell("G2").Style.Font.SetBold();
    var horaPeru = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time"));
    sheet.Cell("H2").Value = horaPeru.ToString("yyyy-MM-dd HH:mm:ss");
    sheet.Cell("H2").Style.Font.FontColor = XLColor.DarkGray;

    var headers = new[]
    {
        "VentaID", "Cliente", "Producto", "Cantidad", 
        "Precio Unitario", "Descuento", "Total Items", "Fecha"
    };

    for (int i = 0; i < headers.Length; i++)
    {
        var cell = sheet.Cell(4, i + 2);
        cell.Value = headers[i];
        cell.Style.Font.SetBold();
        cell.Style.Fill.SetBackgroundColor(XLColor.LightGray);
        cell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
    }

    for (int i = 0; i < detalles.Count; i++)
    {
        var row = i + 5;
        var d = detalles[i];

        sheet.Cell(row, 2).Value = d.IdVenta;
        sheet.Cell(row, 3).Value = d.Cliente;
        sheet.Cell(row, 4).Value = d.Producto;
        sheet.Cell(row, 5).Value = d.Cantidad;

        sheet.Cell(row, 6).Value = d.PrecioUnitario;
        sheet.Cell(row, 6).Style.NumberFormat.Format = "\"S/\" #,##0.00";

        sheet.Cell(row, 7).Value = d.DescuentoAplicado;
        sheet.Cell(row, 7).Style.NumberFormat.Format = "\"S/\" #,##0.00";

        sheet.Cell(row, 8).Value = d.TotalItem;
        sheet.Cell(row, 8).Style.NumberFormat.Format = "\"S/\" #,##0.00";

        sheet.Cell(row, 9).Value = d.Fecha.ToString("yyyy-MM-dd");

        for (int col = 2; col <= 9; col++)
        {
            var cell = sheet.Cell(row, col);
            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            cell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
        }
    }

    sheet.ColumnsUsed().AdjustToContents();
    sheet.RowsUsed().AdjustToContents();
    foreach (var row in sheet.RowsUsed())
        row.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

    using var stream = new MemoryStream();
    workbook.SaveAs(stream);
    return stream.ToArray();
}


}