(function () {
    var elem = document.createElement('input');
    elem.setAttribute('type', 'date');

    if (elem.type === 'text') {
        $('#FechaInicio').datepicker({            
            dateFormat:"yy-mm-dd"
        });
        $('#FechaFinal').datepicker({
            dateFormat: "yy-mm-dd"
        });
        $('#Fecha').datepicker({
            dateFormat: 'yy-mm-dd'
        });

    }
})();
