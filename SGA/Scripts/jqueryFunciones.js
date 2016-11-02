$("#link").click(function () {
    $("#link").attr('href', function () {
        //Busca los valores ingreados
        var Fecha = $("#Fecha").val();
        var TotalCancelado = $("#TotalCancelado").val();
        var Estado = $("#estado").val();
        var Descripcion = $("#Descripcion").val();
        var ClienteId = $("#ClienteId :selected").val();
        //alert(this.href);
        var EstudianteId = $("#EstudianteId :selected").val();
        var titulosSeleccionados = $("input[name=titulosSeleccionados]:checked").map(
                function () { return this.value; }).get().join(","); var mapObj = {//Crea un mapa con los valores originales y los nuevos valores que se quieren
            'Fecha': Fecha, 'TotalCancelado': TotalCancelado, 'Estado': Estado,
            'Descripcion': Descripcion, 'ClienteId': ClienteId, "EstudianteId": EstudianteId, "titulosSeleccionados": titulosSeleccionados
        };//Con replace y una función cambia las palabras que hacen match
        y = this.href.replace(/Fecha|TotalCancelado|Estado|Descripcion|ClienteId|EstudianteId|titulosSeleccionados/gi, function (matched) {
            //  alert(matched);
            return mapObj[matched];
        });
        //alert(y);
        return y;
    });
});

$("#link2").click(function () {
    $("#link2").attr('href', function () {
        //Busca los valores ingreados
        var Fecha = $("#Fecha").val();
        var TotalCancelado = $("#TotalCancelado").val();
        var Estado = $("#estado").val();
        var Descripcion = $("#Descripcion").val();
        var ClienteId = $("#ClienteId :selected").val();
        //alert(this.href);
        var EstudianteId = $("#EstudianteId :selected").val();
        var titulosSeleccionados = $("input[name=titulosSeleccionados]:checked").map(
        function () {return this.value;}).get().join(",");
       // alert(titulosSeleccionados);
        var mapObj = {//Crea un mapa con los valores originales y los nuevos valores que se quieren
            'Fecha': Fecha, 'TotalCancelado': TotalCancelado, 'Estado': Estado,
            'Descripcion': Descripcion, 'ClienteId': ClienteId, "EstudianteId": EstudianteId,"titulosSeleccionados":titulosSeleccionados
        };//Con replace y una función cambia las palabras que hacen match
        y = this.href.replace(/Fecha|TotalCancelado|Estado|Descripcion|ClienteId|EstudianteId|titulosSeleccionados/gi, function (matched) {
            //  alert(matched);
            return mapObj[matched];
        });
       // alert(y);
        return y;
    });
});


$(document).ready(function () {
    $('.table').DataTable();
});

