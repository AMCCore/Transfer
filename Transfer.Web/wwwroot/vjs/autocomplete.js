$(() => {
    // /api/Values/TripRequestOrganisationSearch
    $(".basicAutoComplete").autoComplete({
        resolver: 'custom',
        minLength: 3,
        events: {
            search: function (qry, callback) {
                $.ajax(
                    {
                        url: '/api/Values/TripRequestOrganisationSearch',
                        data: { 'term': qry },
                        type: 'GET'
                    }
                ).done(function (res) {
                    //alert(res);
                    callback(res)
                })
            }
        }
    });
});