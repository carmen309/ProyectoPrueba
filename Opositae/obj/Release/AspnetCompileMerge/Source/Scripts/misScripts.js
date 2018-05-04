$(document).ready(function () {

    $("#Menu1").load("/Home/_Menu1");
    $("#Menu2").load("/Home/_Menu2");
    
    $("#slider").load("/Home/_Slider", function () {
        $("#loader").hide("slow")
    });
    
    $("#EnlacesCirculares").load("/Home/_EnlacesCirculares")
    $("#CuadrosPublicitarios").load("/Home/_CuadroPublicitario")
    $("#NosotrosIconos").load("/Home/_NosotrosIconos")

    $("#EnlaceAcceso").load("/Home/_EnlaceAcceso")
    $("#PiePagina").load("/Home/_PiePagina")

    $("#ListaCategorias").load("/Home/_CategoriasOposicion")
    $("#MensajeCabecera").load("/Home/_MensajeCabecera")
});

function enviarConfirmacionEmail() { //Enlace para confirmar un email ya registrado

    id = $("#uidEnvio").val();
    enlace = $("#enlaceEnvio").val();

    $.ajax({
        method: "GET",
        url: "/Account/_ConfirmacionCuenta",
        data: { 'id': id, 'enlace': enlace },
        success: function (data) {
            $("#enlaceConfirma").replaceWith("<span>Te hemos enviado un enlace de confirmaci\u00F3n</span>")
        }


    })

}




