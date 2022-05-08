$(function () {
    $(".input-addr").select2({
        minimumInputLength: 3,
        placeholder: "Начните вводить адрес",
        allowClear: true,
        theme: 'bootstrap-5',
        selectOnClose: true,
        ajax: {
            url: '/api/FIAS',
            data: function (params) {
                var query = {
                    query: params.term,
                };
                return query;
            },
            type: 'GET',
            processResults: function (data) {
                var result = [];
                for (var i = 0; i < data.suggestions.length; i++) {
                    if (data.suggestions[i].data.fias_id) {
                        result.push({
                            id: data.suggestions[i].data.fias_id,
                            text: data.suggestions[i].value,
                            region: data.suggestions[i].data.region
                        });
                    }
                }
                return {
                    results: result
                };
            },
            cache: true,
        }
    }).on('select2:select', function (e) {
        var edata = e.params.data;
        $(this).closest(".form-group").find(".r1").val('');
        $(this).closest(".form-group").find(".r2").val(edata.region);
        $(this).closest(".form-group").find(".r3").val(edata.text);
    });
});
//# sourceMappingURL=Save.js.map