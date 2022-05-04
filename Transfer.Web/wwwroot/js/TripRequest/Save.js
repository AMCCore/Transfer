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
                    //withParent: 1,
                    //limit: 20,
                    //contentType: 'street',
                    //cityId: 7700000000000
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
                            district: data.suggestions[i].data.district,
                            region: data.suggestions[i].data.adm_area
                        });
                    }
                }
                return {
                    results: result
                };
            },
            cache: true,
        }
    }).on('change', function (e) {
    });
});
//# sourceMappingURL=Save.js.map