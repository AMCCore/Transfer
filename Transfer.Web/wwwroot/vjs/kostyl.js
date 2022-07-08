$(() => {
    $('#confirm-action').on('show.bs.modal', function (event) {
        const source = $(event.relatedTarget);
        $(this).find("h4").html(source.attr("data-bs-confirm-text"));
        $(this).find(".btn-primary").attr("href", source.attr("data-bs-href"));
    });
});