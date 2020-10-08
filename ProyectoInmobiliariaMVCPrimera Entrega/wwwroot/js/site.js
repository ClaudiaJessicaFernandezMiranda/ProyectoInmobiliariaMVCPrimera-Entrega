// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

ScrollReveal().reveal('.fadeReveal', { duration: 2000 });

//Buscar Alquiler
$(document).ready(function () {
    $("#inputBusquedaAlquiler").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#listAlquileres .contenedor").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });
});

$(document).ready(function () {
    $('#dtBasicExample').DataTable();
    $('.dataTables_length').addClass('bs-select');
});
//Tablas con scroll horizontal
$(document).ready(function () {
    $('#dtHorizontalVertical').DataTable({
        "scrollX": true,
        "scrollY": 350,
        "language": {
            "lengthMenu": "Mostrar _MENU_ registros",
            "zeroRecords": "No se encontraron resultados",
            "info": "Mostrando los registros del _START_ al _END_ de un total de _TOTAL_ registros",
            "infoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
            "infoFiltered": "(filtrado de un total de _MAX_ registros)",
            "sSearch": "Buscar: ",
            "oPaginate": {
                "sFirst": "Primero",
                "sLast": "Último",
                "sNext": " Siguiente",
                "sPrevious": " Anterior"
            },
            "sProcessing": "Procesando...",
        }
    });
    $('.dataTables_length').addClass('bs-select');
});

// Tooltips Initialization
$(function () {
    $('[data-tip="tooltip"]').tooltip()
})
