(function () {
    var elem = document.createElement('input');
    elem.setAttribute('type', 'date');

    if (elem.type === 'text') {
        $('#FechaInicio').datepicker({            
            dateformat:"yy-mm-dd"
        });
        $('#FechaFinal').datepicker({
            dateformat: "yy-mm-dd"
        });
        $('#Fecha').datepicker({
            dateFormat: 'yy-mm-dd'
        });

    }
})();
