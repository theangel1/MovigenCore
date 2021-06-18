$(document).ready(function () {
    Calcular();

    for (var i = 0; i < counter; i++) {
        $(document).on("focusout", "input[name='Detalles[" + i + "].Cantidad']", Calcular);
        $(document).on("focusout", "input[name='Detalles[" + i + "].Precio']", Calcular);
        $(document).on("focusout", "input[name='Detalles[" + i + "].Descuento']", Calcular);
    }

});



function eliminarLineaDet(boton) {
    var tabla = document.getElementById("tablaDetalle");
    var filas = tabla.rows.length;    
    if (filas > 2) {
        var indice = boton.parentNode.parentNode.rowIndex;
        tabla.deleteRow(-1);
        counter--;
    }
    else
        swal("¡Error!", "No se pueden eliminar más filas", "error");
    
    Calcular();
}

function Calcular() {
    for (var x = 0; x < counter; x++) {
        var cantidad = document.getElementsByName('Detalles[' + x + '].Cantidad');
        var precio = document.getElementsByName('Detalles[' + x + '].Precio');
        var descuento = document.getElementsByName('Detalles[' + x + '].Descuento');
        var subTotalProd = 0;

        for (var i = 0; i < cantidad.length; i++) {
            if (cantidad[i] !== null && cantidad[i].value !== "" && precio[i].value !== null && precio[i].value !== "") {
                var can = parseFloat(cantidad[i].value);
                var pre = parseFloat(precio[i].value);
                subTotalProd = can * pre;
            }

            //Descuento por detalle. Primero lo haremos por porcentaje
            if (!isNaN(descuento[i].value)) {
                var desc = parseFloat(descuento[i].value);
                montoDescDet = subTotalProd * (desc / 100);
                subTotalProd = subTotalProd - (subTotalProd * (desc / 100));
            }
            else {
                montoDescDet = 0;
            }

            if (isNaN(subTotalProd)) {
                document.getElementsByName('Detalles[' + x + '].Total')[i].value = 0;
            }
            else {
                document.getElementsByName('Detalles[' + x + '].Total')[i].value = subTotalProd;
            }

        }

    } //fin for principal
    CalcularTotal();
}
function CalcularTotal() {
    sum = 0;
    for (var ñ = 0; ñ < counter; ñ++) {
        sum += parseFloat(document.getElementsByName('Detalles[' + ñ + '].Total')[0].value);
    }
    document.getElementById("Pedidos_Total").value = sum;

}
function validacionStock() {
    /*console.log("Validando Stock.");
    var rowCount = $("#tablaDetalle tr").length - 3;
    var cantidad = parseFloat(document.getElementsByName("Detalles[" + rowCount + "].Cantidad")[0].value);    
    var stock = parseFloat(document.getElementsByName("Detalles[" + rowCount + "].Stock")[0].value);  

    console.log("row " + rowCount);
    console.log("cantidad " + cantidad);
    console.log("stock " + stock);

    if (cantidad > stock)
    {
        swal("¡Error!", "Cantidad debe ser menor o igual al stock", "error");        
        document.getElementsByName("Detalles[" + rowCount + "].Cantidad")[0].value = 0;
        Calcular();
    }*/
}

