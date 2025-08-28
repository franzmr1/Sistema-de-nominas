<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListarTrabajadores.aspx.cs" Inherits="NominaEmpresa.Presentacion.ListarTrabajadores" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Lista de Trabajadores Activos</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Trabajadores Activos</h2>
            <asp:GridView ID="gvTrabajadores" runat="server" AutoGenerateColumns="false" EmptyDataText="No hay trabajadores activos.">
                <Columns>
                    <asp:BoundField DataField="TrabajadoresCodigo" HeaderText="Código" />
                    <asp:BoundField DataField="TrabajadoresNombreCompleto" HeaderText="Nombre Completo" />
                    <asp:BoundField DataField="TrabajadoresDocumentoIdentidad" HeaderText="DNI" />
                    <asp:BoundField DataField="TrabajadoresTelefono" HeaderText="Teléfono" />
                    <asp:BoundField DataField="TrabajadoresEmail" HeaderText="Email" />
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
