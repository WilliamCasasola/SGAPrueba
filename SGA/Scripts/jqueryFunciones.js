$("#link").click(function () {
    $("#link").attr('href', function () {
        var Fecha = $("#Fecha").val();
        var TotalCancelado = $("#TotalCancelado").val();
        var Estado = $("#estado").val();
        var Descripcion = $("#Descripcion").val();
        var ClienteId = $("#ClienteId :selected").val();
        //alert(this.href);
        var mapObj = {
            'Fecha': Fecha, 'TotalCancelado': TotalCancelado, 'Estado': Estado,
            'Descripcion': Descripcion, 'ClienteId': ClienteId
        };
        y = this.href.replace(/Fecha|TotalCancelado|Estado|Descripcion|ClienteId/gi, function (matched) {
            //  alert(matched);
            return mapObj[matched];
        });
        //alert(y);
        return y;
    });
});