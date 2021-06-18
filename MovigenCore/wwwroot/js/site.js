$(document).ready(function () {
    // ******************************** Controlador Clientes *****************************

    //Get clientes ajax-json OJO, hay un bug al cargar un cliente en forma consecutiva
    $("#tablaClientes").DataTable({
        select: true,
        responsive: true,
        pageLength: 5,
        destroy: true,
        dom: 'Bfrtip',
        processing: true,
        buttons: [],
        ajax: {
            url: "../Clientes/GetClients",
            type: "GET",
            datatype: "json"
        },
        "columns": [
            { "data": "clrut" },
            { "data": "clnom" },
            { "data": "clobs" },
            { "data": "clcre" },
            { "data": "cldir" }
        ]
    });

   
    
    // ******************************** Controlador Clientes *****************************

    // ******************************** Controlador Productos *****************************


    $("#tablaProductos").DataTable({
        select: true,
        responsive: true,
        pageLength: 5,
        destroy: true,
        dom: 'Bfrtip',
        processing: true,
        buttons: [],
        ajax: {
            url: '/Seller/Productos/GetProducts',
            type: "GET",
            datatype: "json"
        },
        "columns": [
            { "data": "prcod" },
            { "data": "prdes" },
            { "data": "prvl1" },
            { "data": "prstk" },
            { "data": "descuento" }

        ]
    });

    // ******************************** Controlador Productos *****************************


   

});



function GetProductosAjaxPorCodigo(value) {
    var rowCount = $("#tablaDetalle tr").length - 2;
    codigo = value;
    $.ajax({
        type: "POST",
        url: '/Seller/Productos/GetProductosAjaxPorCodigo',
        data: { codigo },
        success: function (response) {
            //si me llega el objeto producto vacio, envio un mensaje, si no, pueblo la tabla.
            if (response === null) {
                swal("Sin Productos", "Debe ingresar nombre de producto", "error");
            }
            else if (response !== null) {
                if (response.length < 1) {
                    swal("Sin Productos", "Debe ingresar nombre de producto", "error");
                }
                else {
                    if (rowCount === 0) {
                        $("#Detalles_" + rowCount + "__Descripcion").val(response[0].prdes);
                        $("#Detalles_" + rowCount + "__Precio").val(response[0].prvl1);
                        $("#Detalles_" + rowCount + "__Stock").val(response[0].prstk);
                        $("#Detalles_" + rowCount + "__Descuento").val(response[0].descuento);
                    }
                    else if (rowCount > 0) {
                        document.getElementById("Detalles[" + rowCount + "].Descripcion").value = response[0].prdes;
                        document.getElementById("Detalles[" + rowCount + "].Precio").value = response[0].prvl1;
                        document.getElementById("Detalles[" + rowCount + "].Stock").value = response[0].prstk;
                        document.getElementById("Detalles[" + rowCount + "].Descuento").value = response[0].descuento;
                    }
                }
            }
            else {
                console.log("AjaxCodeProd: response es null");
            }
        },
        error: function (response) {
            console.log(response);
        }
    });
}






    //****************  Validacion RUT *********************

    function valRut() {
        var rut = document.getElementById("rutCliente").value;
        suma = 0;
        mul = 2;
        i = 0;

        for (i = rut.length - 3; i >= 0; i--) {
            suma = suma + parseInt(rut.charAt(i)) * mul;
            mul = mul === 7 ? 2 : mul + 1;
        }

        var dvr = '' + (11 - suma % 11);

        if (dvr === '10') dvr = 'K'; else if (dvr === '11') dvr = '0';
        {
            if (rut.charAt(rut.length - 2) !== "-" || rut.charAt(rut.length - 1).toUpperCase() !== dvr) {
                rut.IsValid = false;
                document.getElementById("rutCliente").value = "";
                swal("Error", "Rut con formato incorrecto. ", "error");

            }
            else
                rut.IsValid = true;

        }
}

//****************  Validacion RUT *********************





//Funciones create

//AGREGAR LINEA DE DETALLE
function agregarLineaDet() {

    var actualRowLine = $('#tablaDetalle tr').length - 2;
    //validador de campo descripcion. Si viene vacio, se envia mensaje de error.
    if (!$.trim($('input[name="Detalles[' + actualRowLine + '].Descripcion"]').val()) == '') {

        var tabla = document.getElementById("tablaDetalle");
        var i = document.getElementsByName("Detalles[" + (counter - 1) + "].Descripcion").length + counter;
        var row1 = tabla.insertRow(i);
        var cell1 = row1.insertCell(0);
        var cell2 = row1.insertCell(1);
        var cell3 = row1.insertCell(2);
        var cell4 = row1.insertCell(3);
        var cell5 = row1.insertCell(4);
        var cell6 = row1.insertCell(5);
        var cell7 = row1.insertCell(6);


        cell1.innerHTML = "<input asp-for='Detalles[" + counter + "].Codigo' class='form-control' name='Detalles[" + counter + "].Codigo' id='Detalles[" + counter + "].Codigo' onblur='GetProductosAjaxPorCodigo(this.value)' style='width: 80px'/> ";
        cell2.innerHTML = "<input asp-for='Detalles[" + counter + "].Descripcion' class='form-control' name='Detalles[" + counter + "].Descripcion' id='Detalles[" + counter + "].Descripcion' style='width: auto'/> ";
        cell3.innerHTML = "<input asp-for='Detalles[" + counter + "].Cantidad' type='number' class='form-control' name='Detalles[" + counter + "].Cantidad' onblur='validacionStock();' id='Detalles[" + counter + "].Cantidad' autofocus value='1' oninput='this.value = Math.abs(this.value)' /> ";
        cell4.innerHTML = "<input asp-for='Detalles[" + counter + "].Precio' step='0.01' class='form-control' name='Detalles[" + counter + "].Precio' id='Detalles[" + counter + "].Precio' readonly style='width: 80px'/> ";
        cell5.innerHTML = "<input asp-for='Detalles[" + counter + "].Stock' class='form-control' name='Detalles[" + counter + "].Stock' id='Detalles[" + counter + "].Stock' readonly style='width: 70px'/>";
        cell6.innerHTML = "<input asp-for='Detalles[" + counter + "].Descuento' step='0.01' class='form-control' name='Detalles[" + counter + "].Descuento' value='0' onblur='Calcular()' id='Detalles[" + counter + "].Descuento'/> ";
        cell7.innerHTML = "<input asp-for='Detalles[" + counter + "].Total' class='form-control' name='Detalles[" + counter + "].Total' value='0' readonly step='0.01' id='Detalles[" + counter + "].Total' style='width: 80px'/> ";

        counter++;

    }
    else
        swal("Información", "Completar campo Descripción para agregar mas detalles.", "info");

}

function recursividadEnDetalle() {

    //Porque -2? porque hay un tr en la parte de thead y porque quiero obtener el valor actual del arreglo
    var actualRow = $('#tablaDetalle tr').length - 2;

    if ($.trim($('input[name="Detalles[' + actualRow + '].Descripcion"]').val()) === '') {

        var codigo = $("#tablaProductos .selected td:first").text();
        var descripcion = $("#tablaProductos .selected td:eq(1)").text();
        var precio = $("#tablaProductos .selected td:eq(2)").text();
        var stock = $("#tablaProductos .selected td:eq(3)").text();
        var dscto = $("#tablaProductos .selected td:eq(4)").text();

        //poblar inputs
        $('input[name="Detalles[' + actualRow + '].Codigo"]').val(codigo);
        $('input[name="Detalles[' + actualRow + '].Descripcion"]').val(descripcion);
        $('input[name="Detalles[' + actualRow + '].Precio"]').val(precio);
        $('input[name="Detalles[' + actualRow + '].Stock"]').val(stock);
        $('input[name="Detalles[' + actualRow + '].Descuento"]').val(dscto);

        $('#modalProductos').modal('hide');

        swal("Exito", "Producto agregado", "success");
        Calcular();
    }
    else {
        agregarLineaDet();
        recursividadEnDetalle();
    }


}




